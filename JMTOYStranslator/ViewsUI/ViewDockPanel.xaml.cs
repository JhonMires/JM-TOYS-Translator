using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using JMTOYStranslator.Events;
using JMTOYStranslator.Functions;
using DeepL;
using DeepL.Model;
using Ellipse = System.Windows.Shapes.Ellipse;
using Line = System.Windows.Shapes.Line;

namespace JMTOYStranslator.ViewsUI
{
    public partial class ViewDockPanel : Page, IDisposable, IDockablePaneProvider
    {
        private readonly CreateObjectEventChangeValues handlerChange = new CreateObjectEventChangeValues();
        private ExternalEvent externalChange;
        private Document doc;

        public ObservableCollection<StructureData> Tasks { get; set; }
        public ViewDockPanel()
        {
            InitializeComponent();

            this.textApiKey.Text = Cache.apikey;
            try
            {
                var data = LanguageTools.LoadLanguages().Select(x => new { name = x.Name, code = x.LanguageCode });

                this.boxLenguajeMain.ItemsSource = data;
                this.boxLenguajeSecond.ItemsSource = data;
                externalChange = ExternalEvent.Create(handlerChange);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void BoxLenguajeMain_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            var selectedLanguageCode = this.boxLenguajeMain.SelectedValue as string;

            if (!string.IsNullOrEmpty(selectedLanguageCode))
            {
                Cache.baseLang = selectedLanguageCode;
            }
            else
            {
                Cache.baseLang = "";
            }
        }

        private void BoxLenguajeSecond_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            var selectedLanguageCode = this.boxLenguajeSecond.SelectedValue as string;

            if (!string.IsNullOrEmpty(selectedLanguageCode))
            {
                Cache.setLang = selectedLanguageCode;
            }
            else
            {
                Cache.setLang = "";
            }
        }


        public void Dispose()
        {
            this.Dispose();
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this;
            data.InitialState = new DockablePaneState
            {
                DockPosition = DockPosition.Tabbed,
                TabBehind = DockablePanes.BuiltInDockablePanes.ProjectBrowser
            };
        }



        public void SetData(Document eDocument)
        {
            this.doc = eDocument;
            handlerChange.mensaje = doc.Title;
            if (this.autoUpdate.IsChecked == false)
            {
                return;
            }

            if (!string.IsNullOrEmpty(Cache.baseLang) && !string.IsNullOrEmpty(Cache.baseLang) &&
                !Cache.baseLang.Equals(Cache.setLang))
            {
                FillDataAsync();
            }
        }

        private void BtnAppli_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateLanguaje();
        }

        private async void UpdateLanguaje()
        {


            if (!string.IsNullOrEmpty(Cache.baseLang) && !string.IsNullOrEmpty(Cache.baseLang) && !Cache.baseLang.Equals(Cache.setLang))
            {
                if (this.Tasks != null)
                {
                    if (Tasks.Any()) await UpdateTranslationsAsync();
                }
                else
                {
                    FillDataAsync();
                }
            }


        }

        private async Task FillDataAsync()
        {
            Tasks = new ObservableCollection<StructureData>();

            var elementos = new FilteredElementCollector(doc, doc.ActiveView.Id)
                .OfCategory(BuiltInCategory.OST_Rooms).WhereElementIsNotElementType().ToElements();
         
            var translationTasks = new List<Task>();
            foreach (var element in elementos)
            {
                string roomName = element.get_Parameter(BuiltInParameter.ROOM_NAME).AsValueString();
                string categoryLabel = LabelUtils.GetLabelFor(element.Category.BuiltInCategory);
                var translationTask = TranslateAndAddToCollectionAsync(roomName, element, categoryLabel);
                translationTasks.Add(translationTask);
            }
            await Task.WhenAll(translationTasks);
            this.ViewData.ItemsSource = Tasks;

        }

        private async Task TranslateAndAddToCollectionAsync(string roomName, Element element, string categoryLabel)
        {
            string translatedText = await TranslateTextAsync(roomName);
            Tasks.Add(new StructureData(roomName, translatedText, element, false, categoryLabel));
        }

        private async Task<string> TranslateTextAsync(string textToTranslate)
        {
            var authKey = this.textApiKey.Text; // clave API
            var translator = new DeepLTranslator(authKey);

            try
            {
                var translatedText = await translator.TranslateTextAsync(
                    textToTranslate,
                    Cache.baseLang, // Código de idioma de origen
                    Cache.setLang   // Código de idioma de destino
                );

                return translatedText;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return textToTranslate; // Devuelve el texto original en caso de error
            }
        }


        private async Task UpdateTranslationsAsync()
        {
            var authKey = this.textApiKey.Text;
            var translator = new DeepLTranslator(authKey);
            var translationTasks = new List<Task>();

            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 10 
            };

            try
            {
                await Task.Run(() =>
                {
                    Parallel.ForEach(Tasks, parallelOptions, (task) =>
                    {
                        if (task != null) 
                        {
                            var translationTask = Task.Run(async () =>
                            {
                                try
                                {
                                    var translatedText = await translator.TranslateTextAsync(
                                        task.Content,
                                        Cache.baseLang,
                                        Cache.setLang
                                    );
                               
                                    lock (task)
                                    {
                                        task.Translate = translatedText;
                                        task.HasTranslated = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Error al traducir: {ex.Message}");
                                }
                            });
                            translationTasks.Add(translationTask);
                        }
                    });
                });
             
                translationTasks.RemoveAll(t => t == null);
              
                await Task.WhenAll(translationTasks);
            }
            catch (Exception ex)
            {
               
                MessageBox.Show($"Error general: {ex.Message}");
            }
        }



        private T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null && !(current is T))
            {
                current = VisualTreeHelper.GetParent(current);
            }
            return current as T;
        }
        
        private T FindChild<T>(DependencyObject parent, string childName) where T : FrameworkElement
        {
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;
                if (childType != null && childType.Name == childName)
                {
                    foundChild = (T)child;
                    break;
                }

                foundChild = FindChild<T>(child, childName);

                if (foundChild != null) break;
            }

            return foundChild;
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                ListViewItem listViewItem = FindAncestor<ListViewItem>(button);
                if (listViewItem != null)
                {
                    var dataContext = listViewItem.DataContext;
                    var myData = dataContext as StructureData; 

                    if (myData != null)
                    {
                        var traductor = myData.Translate;
                        if (!traductor.Equals(myData.Content))
                        {
                            handlerChange.elements = new List<StructureData>() { myData };
                            externalChange.Raise();
                        }
                       
                    }
                }
            }
        }

        private void BtnAlls_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.Tasks != null)
            {
                handlerChange.elements = Tasks.ToList();
                externalChange.Raise();
            }
        }

        private void BtnSelects_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.Tasks != null)
            {
               
                var data = new List<StructureData>();
                if (this.ViewData.SelectedItems != null)
                {
                    foreach (var selectedItem in this.ViewData.SelectedItems)
                    {
                        try
                        {
                            if (selectedItem is StructureData structureData)
                            {
                                data.Add(structureData);
                            }
                        }
                        catch (Exception exception)
                        {
                           // MessageBox.Show($"Error: {exception.Message}");
                            continue;
                        }
                    }
                }
                if (data.Any())
                {
                    handlerChange.elements = data; 
                    externalChange.Raise();
                }
            }

        }
    }
}
