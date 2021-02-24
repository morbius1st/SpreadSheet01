#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using SpreadSheet01.ExcelSupport;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  2/15/2021 9:20:31 PM

namespace SpreadSheet01.RevitSupport
{
	public class RevitTests
	{
	#region private fields

		private const string ROOT_TRANSACTION_NAME = "Transaction Name";

		private RevitManager rvtMgr = new RevitManager();

	#endregion

	#region ctor

		public RevitTests() { }

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		
		public Result TestSpreadSheet1(Document doc)
		{
			string chartFamily = rvtMgr.getChart(doc);

			// chartFamily = "CellParkingAnalysis-02";
			chartFamily = "CellLegend-01";

			if (chartFamily.IsVoid()) return Result.Failed;

			bool result = rvtMgr.GetCells(doc, chartFamily);

			if (!result) return Result.Failed;


			// 	// Modify document within a transaction
				using (Transaction tx = new Transaction(doc))
				{
					tx.Start(ROOT_TRANSACTION_NAME);
			
					ExcelExchange exe = new ExcelExchange();
			
					// exe.UpdateValues(doc,
						// chartPath, chartWorkSheet, cells);
			
					tx.Commit();
				}

			return Result.Succeeded;
		}


		// public Result TestSpreadSheet(Document doc,
		// 	ChartList chartList,
		// 	ICollection<Element> charts,
		// 	ICollection<Element> cells
		// 	)
		// {
		// 	TaskDialog td;
		//
		// 	charts = rvtMgr.FindGenericAnnotationByName(doc, "SpreadSheetData");
		//
		// 	if (charts == null && charts.Count == 0)
		// 	{
		// 		td = new TaskDialog("Spread Sheets");
		// 		td.MainContent = "No charts found";
		// 		td.MainIcon = TaskDialogIcon.TaskDialogIconError;
		// 		td.CommonButtons = TaskDialogCommonButtons.Ok;
		// 		td.Show();
		//
		// 		return Result.Failed;
		// 	}
		//
		// 	string chartPath = chartList.Charts[0].Path;
		// 	string chartWorkSheet = chartList.Charts[0].WorkSheet;
		//
		// 	if (chartPath.IsVoid() || chartWorkSheet.IsVoid())
		// 	{
		// 		td = new TaskDialog("Spread Sheets");
		// 		td.MainContent = "Incomplete Chart Information";
		// 		td.MainIcon = TaskDialogIcon.TaskDialogIconError;
		// 		td.CommonButtons = TaskDialogCommonButtons.Ok;
		// 		td.Show();
		//
		// 		return Result.Failed;
		// 	}
		//
		// 	string chartFamily = chartList.Charts[0].FamilyTypeName;
		//
		// 	cells = rvtMgr.FindGenericAnnotationByName(doc, chartFamily);
		//
		// 	if (cells == null && cells.Count == 0)
		// 	{
		// 		td = new TaskDialog("Spread Sheet Cells for| " + chartFamily);
		// 		td.MainContent = "No charts found";
		// 		td.MainIcon = TaskDialogIcon.TaskDialogIconError;
		// 		td.CommonButtons = TaskDialogCommonButtons.Ok;
		// 		td.Show();
		//
		// 		return Result.Failed;
		// 	}
		//
		// 	// Modify document within a transaction
		// 	using (Transaction tx = new Transaction(doc))
		// 	{
		// 		tx.Start(ROOT_TRANSACTION_NAME);
		//
		// 		ExcelExchange exe = new ExcelExchange();
		//
		// 		exe.UpdateValues(doc,
		// 			chartPath, chartWorkSheet, cells);
		//
		// 		tx.Commit();
		// 	}
		//
		// 	return Result.Succeeded;
		// }

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is RevitTests";
		}

	#endregion
	}
}