#region using
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using SpreadSheet01;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitParamManagement;
using SpreadSheet01.RevitSupport.RevitSelectionSupport;

//using static SharedCode.RevitSupport.RevitParamManagement.ErrorCodeList2;

#endregion

// username: jeffs
// created:  3/28/2021 6:43:42 AM

/* ------------------------ /
 *  Shared Code
 * ----------------------- */



namespace RevitSupport.RevitChartManagement
{
	public partial class RevitChartManager
	{
	#region private fields

		private const string NAME_OF_CHARTS = "Prime Charts";

		private const string ROOT_TRANSACTION_NAME = "Transaction Name";

		// private const string KEY_IDX_BEGIN  = "《";
		// private const string KEY_IDX_END    = "》";

		private readonly RevitParamCatagorize revitCat;
		private RevitSelectSupport rvtSelect;

		private int errorIdx;

		private int fail;

	#endregion

	#region ctor

		public RevitChartManager()
		{
			revitCat = new RevitParamCatagorize();
			rvtSelect = new RevitSelectSupport();

			ResetAllCharts();
		}

	#endregion

	#region public properties

		// collection of all revit charts  
		// this holds a collection of individual charts
		public RevitCharts Charts { get; private set; } = new RevitCharts(NAME_OF_CHARTS);

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool CollectAllCharts()
		{
			ResetAllCharts();

			// step 1 - select all of the chart cells - place them into: 'chartFamilies'
			ICollection<Element> chartFamilies = findAllChartFamilies(RevitParamManager.CHART_FAMILY_NAME);

			// step 2 - process 'chartFamilies' and process all of the parameters
			getChartParams(chartFamilies);

			return chartFamilies != null && chartFamilies.Count > 0;
		}

		// scan revit and get all cell and label information
		public void PreProcessCharts(CellUpdateTypeCode which)
		{
			// process all charts and add to list
			foreach (KeyValuePair<string, RevitChart> kvp in Charts.ListOfCharts)
			{
				// chart has all parameters retreived
				RevitChart chart = kvp.Value;

				if (which != CellUpdateTypeCode.ALL &&
					chart.UpdateType != which) continue;

				if (!processOneChart(chart))
				{
//					ErrCodeList.Add(this, ErrorCodes.CHT_PROCESS_CHART_FAILED_CS001145);
					chart.ErrorCode = ErrorCodes.RCHT_PROCESS_CHART_FAILED_CS001145;
				}

				if (chart.HasErrors)
				{
//					ErrCodeList.Add(this, ErrorCodes.CHTS_CHART_HAS_ERRORS_CS001200);
					Charts.ErrorCode = ErrorCodes.RCHTS_CHART_HAS_ERRORS_CS001200;
				}
			}
		}

		public void ResetAllCharts()
		{
		#if NOREVIT
			rvtSelect.seq = 0;
		#endif

			Charts = new RevitCharts(NAME_OF_CHARTS);
		}

	#endregion

	#region private methods

		private void getChartParams(ICollection<Element> chartFamilies)
		{
			RevitChartData chartData;

			foreach (Element el in chartFamilies)
			{
				chartData = getChartData(el);

				RevitChart chart = null;
				ChartFamily chartFamily = null;
				CellFamily cellFamily = null;

				string key = RevitParamUtil.MakeAnnoSymKey(chartData,
					(int) RevitParamManager.NameIdx, (int) RevitParamManager.SeqIdx);

				if (chartData.HasErrors)
				{
					key += "*error* (" + (++errorIdx).ToString("D3") + ")";
					chart = setInvalidRevitChart(key, chartData);
//					ErrCodeList.Add(this, ErrorCodes.CHT_RCD_HAS_ERRORS_CS001138);
					chart.ErrorCode = ErrorCodes.RCHT_RCD_HAS_ERRORS_CS001138;
				}
				else
				{
					// key = RevitParamUtil.MakeAnnoSymKey(chartData,
					// 	(int) RevitParamManager.NameIdx, (int) RevitParamManager.SeqIdx);

					chart = getRevitChart(key, chartData);
				}

				bool result = Charts.Add(key, chart);

				if (!result)
				{
//					ErrCodeList.Add(this, ErrorCodes.DUPLICATE_KEY_CS000I01_2);
					Charts.ErrorCode = ErrorCodes.DUPLICATE_KEY_CS000I01_2;
				}
			}
		}

		private RevitChart getRevitChart(string key, RevitChartData chartData)
		{
			RevitChart chart;
			ChartFamily chartFamily;
			// CellFamily cellFamily;

			bool answer = RevitParamManager.GetChartFamily(chartData.FamilyName, out chartFamily);

			if (!answer) return RevitChart.Invalid();

			chart = new RevitChart();
			chart.RevitChartData = chartData;

			return chart;
		}

		private RevitChart setInvalidRevitChart(string key, RevitChartData chartData)
		{
			// ChartFamily chartFamily = ChartFamily.invalid(key);
			RevitChart chart = new RevitChart();

			// chartData.ChartFamily = ChartFamily.invalid(key);

			chart.RevitChartData = chartData;
			// chart.RevitChartData.CellFamily = CellFamily.invalid(key);

			return chart;
		}

		private RevitChartData getChartData(Element el)
		{
			RevitChartData chartData;
			ChartFamily chartFam;
			AnnotationSymbol aSym = (AnnotationSymbol) el;

			bool value1 = RevitParamManager.GetChartFamily(aSym.Symbol.FamilyName, out chartFam);

			if (value1)
			{
				chartData = revitCat.CatagorizeChartSymParams(el, chartFam);
			}
			else
			{
				chartData = new RevitChartData(ChartFamily.Invalid);
//				ErrCodeList.Add(this, ErrorCodes.INVALID_DATA_FORMAT_CS000I10);
				chartData.ErrorCode = ErrorCodes.INVALID_DATA_FORMAT_CS000I10;
			}

			chartData.RevitElement = el;
			chartData.AnnoSymbol = aSym;

			return chartData;
		}

		// provide the list of cell families
		// process a chart and get the parameters for a family
		private bool processOneChart(RevitChart chart)
		{
			string cellFamilyTypeName = chart.CellFamilyName;

			ICollection<Element> cellElements
				= rvtSelect.GetCellFamilies(RevitDoc.Doc, cellFamilyTypeName);

			if (cellElements == null || cellElements.Count == 0) return false;

			chart.ListOfCellSyms = new Dictionary<string, RevitCellData>();

			foreach (Element el in cellElements)
			{
				if (el.Parameters.Size < 1) continue;

				RevitCellData rcd = processCellFamily2(el, chart.RevitChartData.CellFamily);

				// RevitCellData rcd = 
				// 	revitCat.catagorizeCellParams(el as AnnotationSymbol, chart.RevitChartData.CellFamily);

				if (rcd.HasErrors)
				{
//					ErrCodeList.Add(this, ErrorCodes.CHT_CELL_HAS_ERROR_CS001135);
					chart.ErrorCode = ErrorCodes.RCHT_CELL_HAS_ERROR_CS001135;
				}

//				ErrCodeList.Add(this, chart.Add(rcd) ? ErrorCodes.EC_NO_ERROR : ErrorCodes.CHT_FAIL_TO_ADD_CS001036);
				chart.ErrorCode = chart.Add(rcd) ? ErrorCodes.EC_NO_ERROR : ErrorCodes.RCHT_FAIL_TO_ADD_CS001036;
			}

			return true;
		}

		private RevitCellData processCellFamily2(Element el, CellFamily cellFam)
		{
			RevitCellData revitCellData
				= revitCat.catagorizeCellParams(el as AnnotationSymbol, cellFam);

			return revitCellData;
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is RevitSystemManager| " + LocalToString();
		}

	#endregion
	}
}


// provide the list of cell families
// process a chart and get the parameters for a family
// private bool processOneChartLabels(RevitChart chart)
// {
// 	string cellFamilyTypeName = chart.RevitChartData.GetValue();
//
// #if REVIT
// 	ICollection<Element> cellElements
// 		= RvtSelect.GetCellFamilies(RevitDoc.Doc, cellFamilyTypeName);
// #endif
//
//
//
// #if NOREVIT
// 	MainWindow.WriteLine("");
// 	MainWindow.WriteLine("Process one chart's labels");
// 	foreach (KeyValuePair<string, RevitLabel> kvp in chart.AllCellLabels)
// 	{
// 		MainWindow.WriteLine("");
// 		MainWindow.WriteLine("Process one label| " + kvp.Value.Name);
// 	}
//
// 	int i = Int32.Parse(chart[RevitParamManager.SeqIdx].GetValue());
//
// 	ICollection<Element> cellElements
// 		= RvtSelect.GetCellFamilies(RevitDoc.Doc, cellFamilyTypeName, i);
//
// #endif
//
// 	if (cellElements == null || cellElements.Count == 0) return false;
//
// 	chart.ListOfCellSyms = new Dictionary<string, RevitCell>();
//
// 	foreach (Element cell in cellElements)
// 	{
// 		RevitCell rvtCell = processCellFamily2(cell);
//
// 		chart.Add(rvtCell);
// 	}
//
// 	return true;
// }


// public bool ProcessLabels(CellUpdateTypeCode which)
// {
// 	int fail = 0;
// 	// process all charts and add to list
// 	foreach (KeyValuePair<string, RevitChart> kvp in Charts.ListOfCharts)
// 	{
// 		// chart has all parameters retreived
// 		RevitChart chart = kvp.Value;
//
// 		if (which != CellUpdateTypeCode.ALL &&
// 			kvp.Value.UpdateType != which) continue;
//
// 		processOneChartLabels(kvp.Value);
// 	}
//
// 	return true;
// }