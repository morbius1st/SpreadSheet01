#region using

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitParamInfo;
using SpreadSheet01.RevitSupport.RevitParamValue;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitChartInfo;
using SpreadSheet01.RevitSupport.RevitSelectionSupport;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamReadReqmt;

#endregion

// username: jeffs
// created:  2/14/2021 10:56:46 PM

namespace SpreadSheet01.RevitSupport.RevitCellsManagement
{
	public class RevitManager : INotifyPropertyChanged
	{
	#region private fields

		private const string ROOT_TRANSACTION_NAME = "Transaction Name";

		private const string KEY_ADDR_BEGIN = "〖";
		private const string KEY_ADDR_END   = "〗";
		private const string KEY_IDX_BEGIN  = "《";
		private const string KEY_IDX_END    = "》";

		// private Dictionary<string, RevitCellParams> CellItemsBySeqName { get; set; }
		// private Dictionary<string, RevitCellParams> CellItemsByNameSeq { get; set; }

		// private ICollection<Element> cellFamilies;

		// private List<RevitCellParams> errorList;

		private RevitAnnoSyms annoSyms;

		private RevitManagementSupport rvtMgmtSupport = new RevitManagementSupport();
		private RevitChartManager rvtChartMgr = new RevitChartManager();
		private RevitCatagorizeParam revitCat;
		private RevitSelectSupport rvtSelect;

		private RevitCharts rvtCharts = new RevitCharts();

	#endregion

	#region ctor

		public RevitManager()
		{
			// chartList = new ChartList();
			// errorList = new List<RevitCellParams>();
			annoSyms = new RevitAnnoSyms();
			revitCat = new RevitCatagorizeParam();
			rvtSelect = new RevitSelectSupport();
		}

	#endregion

	#region public properties

		public bool GotCellFamilies { get; set; }

		public RevitAnnoSyms Symbols => annoSyms;

		public RevitSelectSupport RvtSelect
		{
			get { return rvtSelect; }
		}

		// public ICollection<Element> CellFamilies
		// {
		// 	get { return cellFamilies; }
		// 	set { cellFamilies = value; }
		// }

	#endregion

	#region private properties

	#endregion

	#region public methods

		// process all "always" charts / cells
		public bool ProcessAlwaysCharts()
		{
			// get the list of charts to process
			if (! getAllCharts()) return false;

			RevitCharts c = rvtCharts;
			Dictionary<string, RevitChart> cx = c.Containers;

			return true;
		}

	#endregion

	#region private methods

		private bool getAllCharts()
		{
			int count = rvtChartMgr.GetCurrentCharts();

			if (count == 0)
			{
				rvtMgmtSupport.ErrorNoChartsFound(rvtChartMgr.RevitChartFamilyName);
				return false;
			}

			rvtCharts = rvtChartMgr.Charts;

			bool result = processAllCharts2();

			return true;
		}

		private bool processAllCharts2()
		{
			int fail = 0;
			// process all charts and add to list
			foreach (KeyValuePair<string, RevitChart> kvp in rvtCharts.Containers)
			{
				// chart has all parameters retreived
				RevitChart chart = kvp.Value;

				processOneChart2(chart);
			}

			return true;
		}

		private bool processOneChart2(RevitChart chart)
		{
			// get a list of the associated cell families
			// process this list
			chart.Containers = processOneCellFamilyType(chart.RvtChartSym.GetValue());

			return true;
		}

		// provide the list of cell families
		private Dictionary<string, RevitCellSym> processOneCellFamilyType(string cellFamilyTypeName)
		{
			ICollection<Element> cellElements = RvtSelect.GetCellFamilies(RevitDoc.Doc, cellFamilyTypeName);

			if (cellElements == null || cellElements.Count == 0) return null;

			Dictionary<string, RevitCellSym> cellFamilies = new Dictionary<string, RevitCellSym>();

			foreach (Element cell in cellElements)
			{
				RevitCellSym rvtCellSym = processCellFamily2(cell);

				string key = RevitParamUtil.MakeAnnoSymKey(rvtCellSym,
					(int) RevitCellParameters.NameIdx, (int) RevitCellParameters.SeqIdx, false);

				cellFamilies.Add(key, rvtCellSym);
			}

			return cellFamilies;
		}

		private RevitCellSym processCellFamily2(Element el)
		{
			RevitCellSym rvtCellSym = revitCat.catagorizeAnnoSymParams(el as AnnotationSymbol, ParamClass.LABEL);

			if (!rvtCellSym.IsValid) return null;

			return rvtCellSym;
		}


















		// private ICollection<Element> getCellFamilies(string familyTypeName)
		// {
		// 	ICollection<Element> cellFamilies = RvtSelect.GetCellFamilies(RevitDoc.Doc, familyTypeName);
		//
		// 	if (cellFamilies == null || cellFamilies.Count == 0)
		// 	{
		// 		rvtMgmtSupport.ErrorNoCellsFound(familyTypeName);
		// 		return null;
		// 	}
		//
		// 	return cellFamilies;
		// }


		// private bool processAllCharts()
		// {
		// 	int fail = 0;
		//
		// 	// process all charts and add to list
		// 	foreach (KeyValuePair<string, RevitChart> kvp in rvtCharts.Containers)
		// 	{
		// 		// chart has all parameters retreived
		// 		RevitChart chart = kvp.Value;
		//
		// 		string chartFamilyTypeName = chart.RvtChartSym[RevitChartParameters.ChartFamilyNameIdx].GetValue();
		// 		
		// 		// find all chart families
		// 		ICollection<Element> chartFamilies = getCellFamilies(chartFamilyTypeName);
		//
		//
		//
		// 		if (chartFamilies == null)
		// 		{
		// 			chart.ErrorCode = RevitCellErrorCode;
		// 			fail++;
		// 			continue;
		// 		}
		//
		// 		foreach (Element cell in cellFamilies)
		// 		{
		// 			RevitCellSym rvtCellSym = new RevitCellSym();
		//
		// 			rvtCellSym.RvtElement = cell;
		// 			rvtCellSym.AnnoSymbol = (AnnotationSymbol) cell;
		//
		// 			string key = RevitParamUtil.MakeAnnoSymKey(rvtCellSym,
		// 				(int) RevitCellParameters.NameIdx, (int) RevitCellParameters.SeqIdx, false);
		//
		// 			chart.Containers.Add(key, rvtCellSym);
		// 		}
		// 	}
		//
		// 	int a = rvtCharts.Containers.Count - fail == 0 ? -1 : fail;
		//
		// 	return true;
		// }


		// private RevitCellSym processOneChart(RevitChart chart)
		// {
		// 	RevitCellSym rvtCellSym = null;
		//
		// 	string familyTypeName = chart.RvtChartSym[RevitChartParameters.ChartFamilyNameIdx].GetValue();
		//
		// 	ICollection<Element> found = getCellFamilies(familyTypeName);
		//
		// 	if (found == null)
		// 	{
		// 		chart.ErrorCode = RevitCellErrorCode.NO_CELLS_FOUND_CS100200;
		// 		return null;
		// 	}
		//
		// 	foreach (Element cell in found)
		// 	{
		// 		rvtCellSym = new RevitCellSym();
		//
		// 		rvtCellSym.RvtElement = cell;
		// 		rvtCellSym.AnnoSymbol = (AnnotationSymbol) cell;
		//
		// 		bool result = getCellParameters(rvtCellSym);
		//
		// 		string key = RevitParamUtil.MakeAnnoSymKey(rvtCellSym,
		// 			(int) RevitCellParameters.NameIdx, (int) RevitCellParameters.SeqIdx, false);
		//
		// 		chart.Containers.Add(key, rvtCellSym);
		// 	}
		//
		//
		// 	return rvtCellSym;
		// }





		// private bool getCellParameters(RevitCellSym cell)
		// {
		// 	RevitCellSym rvtCellSym = revitCat.catagorizeAnnoSymParams(cell.AnnoSymbol, ParamClass.LABEL);
		//
		// 	if (!rvtCellSym.IsValid) return false;
		//
		// 	string key = RevitParamUtil.MakeAnnoSymKey(rvtCellSym,
		// 		(int) RevitCellParameters.NameIdx, (int) RevitCellParameters.SeqIdx, false);
		//
		// 	return true;
		// }







		// // get all cell families for all charts
		// private int getAllCellFamilies()
		// {
		// 	int fail = 0;
		//
		// 	// process all charts and add to list
		// 	foreach (KeyValuePair<string, RevitChart> kvp in rvtCharts.Containers)
		// 	{
		// 		RevitChart chart = kvp.Value;
		// 		string familyTypeName = chart.RvtChartSym[RevitChartParameters.ChartFamilyNameIdx].GetValue();
		//
		// 		ICollection<Element> found = getCellFamilies(familyTypeName);
		//
		// 		if (found == null)
		// 		{
		// 			chart.ErrorCode = RevitCellErrorCode.NO_CELLS_FOUND_CS100200;
		// 			fail++;
		// 			continue;
		// 		}
		//
		// 		foreach (Element cell in found)
		// 		{
		// 			RevitCellSym rvtCellSym = new RevitCellSym();
		//
		// 			rvtCellSym.RvtElement = cell;
		// 			rvtCellSym.AnnoSymbol = (AnnotationSymbol) cell;
		//
		// 			string key = RevitParamUtil.MakeAnnoSymKey(rvtCellSym,
		// 				(int) RevitCellParameters.NameIdx, (int) RevitCellParameters.SeqIdx, false);
		//
		// 			chart.Containers.Add(key, rvtCellSym);
		// 		}
		// 	}
		//
		// 	return rvtCharts.Containers.Count - fail == 0 ? -1 : fail;
		// }

		// get all cells of a specific family type


		//
		// private bool getCellParameters(Element el, ParamClass pClass)
		// {
		// 	AnnotationSymbol annoSym = el as AnnotationSymbol;
		//
		// 	RevitCellSym rvtCellSym = revitCat.catagorizeAnnoSymParams(annoSym, pClass);
		//
		// 	if (!rvtCellSym.IsValid) return false;
		//
		// 	rvtCellSym.RvtElement = el;
		// 	rvtCellSym.AnnoSymbol = annoSym;
		//
		// 	string key = RevitParamUtil.MakeAnnoSymKey(rvtCellSym,
		// 		(int) RevitCellParameters.NameIdx, (int) RevitCellParameters.SeqIdx, false);
		//
		// 	annoSyms.Add(key, rvtCellSym);
		//
		// 	return true;
		// }


	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is RevitManager";
		}

	#endregion


		// public ChartList ChartList => chartList;
		//
		// public RevitChartItem SelectedChart => selectedChart;
		//
		// public bool GotChart => selectedChart.IsValid;


		// private void getCharts(ICollection<Element> charts)
		// {
		// 	foreach (Element e in charts)
		// 	{
		// 		int found = 0;
		//
		// 		RevitChartItem cp = new RevitChartItem();
		//
		// 		foreach (Parameter p in e.ParametersMap)
		// 		{
		// 			string name = p.Definition.Name;
		//
		// 			foreach (KeyValuePair<string, int> kvp in RevitChartItem.ChartItemIds)
		// 			{
		// 				if (name.Equals(kvp.Key))
		// 				{
		// 					cp.Chart[kvp.Value] = p.AsString();
		// 					found++;
		// 				}
		// 			}
		//
		// 			if (found == RevitChartItem.ItemIdCount) break;
		// 		}
		//
		// 		if (cp != null) chartList.Add(cp);
		// 	}
		// }


		// public void GetCharts()
		// {
		// 	TaskDialog td;
		//
		// 	chartFamilies = rvtSelect.FindGenericAnnotationByName(RevitDoc.Doc, "SpreadSheetData");
		//
		// 	if (chartFamilies == null && chartFamilies.Count == 0)
		// 	{
		// 		td = new TaskDialog("Spread Sheets");
		// 		td.MainContent = "No charts found";
		// 		td.MainIcon = TaskDialogIcon.TaskDialogIconError;
		// 		td.CommonButtons = TaskDialogCommonButtons.Ok;
		// 		td.Show();
		//
		// 		return;
		// 	}
		//
		// 	getCharts(chartFamilies);
		//
		// 	OnPropertyChanged(nameof(ChartList));
		// 	
		// }


		// public string getChart(Document doc, int whichChart = 0)
		// {
		//
		// 	TaskDialog td;
		//
		// 	chartFamilies = rvtSelect.FindGenericAnnotationByName(doc, "SpreadSheetData");
		//
		// 	if (chartFamilies == null && chartFamilies.Count == 0)
		// 	{
		// 		td = new TaskDialog("Spread Sheets");
		// 		td.MainContent = "No charts found";
		// 		td.MainIcon = TaskDialogIcon.TaskDialogIconError;
		// 		td.CommonButtons = TaskDialogCommonButtons.Ok;
		// 		td.Show();
		//
		// 		return null;
		// 	}
		//
		// 	
		// 	getCharts(chartFamilies);
		//
		// 	string chartPath = chartList.Charts[whichChart].ChartPath;
		// 	string chartWorkSheet = chartList.Charts[whichChart].ChartWorkSheet;
		//
		// 	if (chartPath.IsVoid() || chartWorkSheet.IsVoid())
		// 	{
		// 		td = new TaskDialog("Spread Sheets");
		// 		td.MainContent = "Incomplete Chart Information";
		// 		td.MainIcon = TaskDialogIcon.TaskDialogIconError;
		// 		td.CommonButtons = TaskDialogCommonButtons.Ok;
		// 		td.Show();
		//
		// 		return null;
		// 	}
		//
		// 	selectedChart = chartList.Charts[whichChart];
		//
		// 	return chartList.Charts[whichChart].ChartFamilyTypeName;
		// }
	}
}