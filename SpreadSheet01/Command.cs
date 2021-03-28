#region using

using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using SpreadSheet01.Windows;

#endregion

// projname: SpreadSheet01
// itemname: Command
// username: jeffs
// created:  2/13/2021 2:44:52 PM

namespace SpreadSheet01
{
	public static class RevitDoc
	{
		public static Document Doc { get; set; }
	}



	/// <summary>
	/// revit command entry point.  this command will automatically update all
	/// cells according to its "update type"
	/// </summary>

	[Transaction(TransactionMode.Manual)]
	public class Command : IExternalCommand
	{
	#region fields

		private const string ROOT_TRANSACTION_NAME = "Transaction Name";

		// private ChartList chartList = new ChartList();

	#endregion

	#region entry point: Execute

		public Result Execute(
			ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Application app = uiapp.Application;
			Document doc = uidoc.Document;

			RevitDoc.Doc = doc;

			// Access current selection
			Selection sel = uidoc.Selection;

			Result result;

			SelectCharts charts = new SelectCharts(app, doc);

			charts.ShowDialog();


/*
			// // Modify document within a transaction
			using (Transaction tx = new Transaction(doc))
			{
				tx.Start(ROOT_TRANSACTION_NAME);

				result = rt.TestSpreadSheet1(doc);

				// ExcelExchange exe = new ExcelExchange();

				// exe.UpdateValues(doc,
				// 	chartPath, chartWorkSheet, cells);

				tx.Commit();
			}
*/

			// result = rt.TestSpreadSheet1(doc);

			// Review r = new Review();
			//
			// r.ShowDialog();

			return Result.Succeeded;
		}

	#endregion

	#region public methods


	#endregion

	#region private methods


	#endregion
	}

}