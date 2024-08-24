using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace JMTOYStranslator.Functions
{
    //public class StructureData 
    //{
    //    public StructureData(string content, string translate, Element element, bool hasTranslated, string tipo)
    //    {
    //        Content = content;
    //        Translate = translate;
    //        Element = element;
    //        HasTranslated = hasTranslated;
    //        Tipo = tipo;
    //    }
       
    //    public string Content { get; set; }
    //    public string Translate { get; set; }
    //    public Element Element { get; set; }
    //    public bool HasTranslated { get; set; }
    //    public string Tipo { get; set; }
         
    //}


    public class StructureData : INotifyPropertyChanged
    {
        private string content;
        private string translate;
        private Element element;
        private bool hasTranslated;
        private string tipo;

        public StructureData(string content, string translate, Element element, bool hasTranslated, string tipo)
        {
            Content = content;
            Translate = translate;
            Element = element;
            HasTranslated = hasTranslated;
            Tipo = tipo;
        }

        public string Content
        {
            get => content;
            set
            {
                if (content != value)
                {
                    content = value;
                    OnPropertyChanged(nameof(Content));
                }
            }
        }

        public string Translate
        {
            get => translate;
            set
            {
                if (translate != value)
                {
                    translate = value;
                    OnPropertyChanged(nameof(Translate));
                }
            }
        }

        public Element Element
        {
            get => element;
            set
            {
                if (element != value)
                {
                    element = value;
                    OnPropertyChanged(nameof(Element));
                }
            }
        }

        public bool HasTranslated
        {
            get => hasTranslated;
            set
            {
                if (hasTranslated != value)
                {
                    hasTranslated = value;
                    OnPropertyChanged(nameof(HasTranslated));
                }
            }
        }

        public string Tipo
        {
            get => tipo;
            set
            {
                if (tipo != value)
                {
                    tipo = value;
                    OnPropertyChanged(nameof(Tipo));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
