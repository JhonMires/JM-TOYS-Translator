using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using JMTOYStranslator.ViewsUI;

namespace JMTOYStranslator
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class CdmStarted : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var gd = new DockablePaneId(new Guid(Cache.guid));
                DockablePane dockpanel = commandData.Application.GetDockablePane(gd);
                if (dockpanel != null)
                {
                    dockpanel.Show();
                    return 0;
                }
                else
                {
                    return Result.Failed;
                }

            }
            catch (Exception e)
            {
                return Result.Failed;
            }
        }
    }

    public class CmdMain : IExternalCommand
    {
        ViewDockPanel DockPanel = null;
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            Result result = Register(commandData.Application);

            return result;
        }

        public Result Register(UIApplication application)
        {
            var dockProvider = new DockablePaneProviderData();
            DockPanel = new ViewDockPanel();
            dockProvider.FrameworkElement = DockPanel as FrameworkElement;
            application.RegisterDockablePane(new DockablePaneId(new Guid(Cache.guid)), "[JM TOYS] Translator", DockPanel);

            application.Application.DocumentOpened += new EventHandler<DocumentOpenedEventArgs>(application_openDoc);
            application.ViewActivated += new EventHandler<ViewActivatedEventArgs>(application_viewactive);
            return 0;
        }

        private void application_viewactive(object sender, ViewActivatedEventArgs e)
        {
            DockPanel.SetData(e.Document);
        }
        private void application_openDoc(object sender, DocumentOpenedEventArgs e)
        {
            DockPanel.SetData(e.Document);
        }
    }

}
