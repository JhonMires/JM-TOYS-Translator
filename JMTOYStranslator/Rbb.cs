using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Autodesk.Revit.ApplicationServices;
using JMTOYStranslator.Properties;

namespace JMTOYStranslator
{
    public class Rbb : IExternalApplication
    {
        private readonly string title = "[JM] Toys";

        public Result OnStartup(UIControlledApplication application)
        {
            crearPanel(application);
            application.ControlledApplication.ApplicationInitialized += ControlledApplication_ApplicationInitialized;
            return Result.Succeeded;
        }

        private void ControlledApplication_ApplicationInitialized(object sender,
            Autodesk.Revit.DB.Events.ApplicationInitializedEventArgs e)
        {
            var cmd = new CmdMain().Register(new UIApplication(sender as Application));
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public void crearPanel(UIControlledApplication application)
        {
            var RUTA = Assembly.GetExecutingAssembly().Location;
            var nombrePestaña = title;
            var nombrePanelTab = "Translator";

            try
            {
                application.CreateRibbonTab(nombrePestaña);
            }
            catch (Exception)
            {
            }

            RibbonPanel panelTab = null;
            foreach (var panelR in application.GetRibbonPanels(nombrePestaña))
                if (panelR.Name == nombrePanelTab)
                    panelTab = panelR;

            if (panelTab == null) panelTab = application.CreateRibbonPanel(nombrePestaña, nombrePanelTab);

            try
            {
                var datosPushButtonData1 = new PushButtonData("Translate",
                    "Translate", RUTA, typeof(CdmStarted).FullName)
                {
                    LargeImage = GetBitmapSource(Resources.ICON_DEEPL),
                    ToolTip = "Permite traducir textos."
                };
                var buttonPush1 = panelTab.AddItem(datosPushButtonData1) as PushButton;
            }
            catch (Exception)
            {
                //
            }
        }

        private BitmapSource GetBitmapSource(Image img)
        {
            BitmapSource bmp = null;

            using (var ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                ms.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();
                bmp = ResizeBitmap(bitmapImage, 32, 32);
            }

            return bmp;
        }

        private BitmapSource ResizeBitmap(BitmapImage source, int targetWidth, int targetHeight)
        {
            var drawingVisual = new DrawingVisual();
            using (var drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(source, new System.Windows.Rect(0, 0, targetWidth, targetHeight));
            }

            var targetBitmap = new RenderTargetBitmap(targetWidth, targetHeight, 96, 96, PixelFormats.Pbgra32);
            targetBitmap.Render(drawingVisual);
            return targetBitmap;
        }
    }
}
