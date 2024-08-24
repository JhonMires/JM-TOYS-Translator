using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using JMTOYStranslator.Functions;
using JMTOYStranslator.ViewsUI;

namespace JMTOYStranslator.Events
{
    internal class CreateObjectEventChangeValues : IExternalEventHandler
    {
        public ViewDockPanel dock;
        public Document doc;
        public UIDocument uidoc;
        public List<StructureData> elements;

        public string mensaje;

        public void Execute(UIApplication app)
        {
            doc = app.ActiveUIDocument.Document;
            uidoc = app.ActiveUIDocument;

            using (Transaction tx = new Transaction(doc,"CHANGE VALUES"))
            {
                tx.Start();
                foreach (var item in elements)
                {
                    try
                    {
                        var p = item.Element.get_Parameter(BuiltInParameter.ROOM_NAME);
                        if (p != null) p.Set(item.Translate);
                    }   
                    catch (Exception e)
                    {
                        continue;
                    }
                }
                doc.Regenerate();
                uidoc.RefreshActiveView();
                tx.Commit();
                
            }
            TaskDialog.Show("JHON MIRES", "Traduccion finalizada");
        }

        public string GetName()
        {
            return "ChangeValues";
        }
    }
}
