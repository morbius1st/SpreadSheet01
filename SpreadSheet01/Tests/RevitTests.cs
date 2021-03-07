#region using

using SpreadSheet01.RevitSupport;
using SpreadSheet01.RevitSupport.RevitCellsManagement;

#endregion

// username: jeffs
// created:  2/15/2021 9:20:31 PM

namespace SpreadSheet01.Tests
{
	public class RevitTests
	{
	#region private fields

		private const string ROOT_TRANSACTION_NAME = "Cells";

		public static RevitManager rvtMgr { get; private set; }

	#endregion

	#region ctor

		public RevitTests() { }

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		// public Result TestSpreadSheet1(Document doc)
		// {
		// 	rvtMgr = new RevitManager();
		// 	
		// 	string chartFamily = rvtMgr.getChart(doc);
		//
		// 	// chartFamily = "CellParkingAnalysis-02";
		// 	chartFamily = "CellLegend-02";
		//
		// 	if (chartFamily.IsVoid() ||
		// 		!rvtMgr.GotChart) return Result.Failed;
		//
		// 	bool result = rvtMgr.RvtSelect.GetCellFamilies(doc, chartFamily, rvtMgr);
		//
		// 	if (result)
		// 	{
		// 		result = rvtMgr.GetAllCellParameters(ParamClass.LABEL);
		// 	}
		//
		// 	if (!result) return Result.Failed;
		//
		//
		// 	// Modify document within a transaction
		// 	using (Transaction tx = new Transaction(doc))
		// 	{
		// 		tx.Start(ROOT_TRANSACTION_NAME+ ": Update cell family| " + chartFamily);
		//
		// 		ExcelExchange exe = new ExcelExchange();
		//
		// 		exe.UpdateValues(doc,
		// 			rvtMgr.SelectedChart.ChartPath,
		// 			rvtMgr.SelectedChart.ChartWorkSheet,
		// 			rvtMgr.Symbols);
		//
		// 		tx.Commit();
		// 	}
		//
		// 	return Result.Succeeded;
		// }


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