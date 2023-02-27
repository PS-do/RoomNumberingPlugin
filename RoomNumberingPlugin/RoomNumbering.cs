using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomNumberingPlugin 
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class RoomNumbering : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            List<Room> rooms = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Rooms)
                .OfType<Room>()
                .ToList();

            if (rooms == null || rooms.Count == 0)
            {
                TaskDialog.Show("Ошибка", "Не найдены помещения");
                return Result.Cancelled;
            }

            Transaction transaction = new Transaction(doc);
            transaction.Start("Нумерация помещений");
            for (int i = 0; i < rooms.Count; i++)
            {
                Parameter number = rooms[i].get_Parameter(BuiltInParameter.ROOM_NUMBER);
                number.Set(Convert.ToString(i + 1));
            }
            transaction.Commit();
            return Result.Succeeded;
        }
    }
}
