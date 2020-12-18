using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using JPMorrow.Tools.Diagnostics;
using System.Linq;

namespace MainApp
{
    public partial class ThisApplication : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            View view = doc.ActiveView;

            using (Transaction tr = new Transaction(doc, "check view"))
            {
                tr.Start();
                view.EnableRevealHiddenMode();
                tr.Commit();
            }

            FilteredElementCollector el_coll = new FilteredElementCollector(doc, view.Id);
            var els = el_coll.Where(x => x.IsHidden(view)).ToList();

            using(Transaction tx = new Transaction(doc, "unhide elements"))
            {
                tx.Start();
                foreach(var el in els)
                {
                    if(el.IsHidden(view))
                        doc.ActiveView.UnhideElements(new ElementId[] { el.Id});
                }
                tx.Commit();
            }

            using (Transaction tr = new Transaction(doc, "check view"))
            {
                tr.Start();
                TemporaryViewMode vm = TemporaryViewMode.RevealHiddenElements;
                view.DisableTemporaryViewMode(vm);
                tr.Commit();
            }

            return Result.Succeeded;
        }
    }
}
