#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitParamManagement;
using SpreadSheet01.RevitSupport.RevitParamValue;
using SpreadSheet01.RevitSupport.RevitSelectionSupport;
using UtilityLibrary;
#if NOREVIT
using CellsTest.Windows;
using CellsTest.CellsTests;
#endif
#endregion

// username: jeffs
// created:  3/1/2021 10:29:39 PM

/* ------------------------ /
 *  Shared Revit Code
 * ----------------------- */

namespace RevitSupport.RevitChartManagement
{
	public partial class RevitChartManager : INotifyPropertyChanged
	{
	#region private fields

		// private const string ROOT_TRANSACTION_NAME = "Transaction Name";
		//
		// private const string KEY_IDX_BEGIN  = "《";
		// private const string KEY_IDX_END    = "》";


		// private readonly RevitParamCatagorize revitCat;
		// private RevitSelectSupport rvtSelect;

		private Application app;
		private Document doc;

		// private int errorIdx;

	#endregion

	#region ctor

		public RevitChartManager(Application app, Document doc) : this()
		{
			this.app = app;
			this.doc = doc;

			// revitCat = new RevitParamCatagorize();
			// rvtSelect = new RevitSelectSupport();
			//
			// Reset();
		}

	#endregion

	#region public properties

		// collection of all revit charts  
		// this holds a collection of individual charts
		// public RevitCharts Charts { get; private set; } = new RevitCharts();
		// public RevitSelectSupport RvtSelect
		// {
		// 	get { return rvtSelect; }
		// }


	#endregion

	#region private properties

	#endregion

	#region public methods


		// public bool CollectAllCharts()
		// {
		// 	ICollection<Element> chartFamilies;
		//
		// 	// step 1 - select all of the chart cells - place them into: 'chartFamilies'
		// 	chartFamilies = findAllChartFamilies(RevitParamManager.CHART_FAMILY_NAME);
		//
		// 	// step 2 - process 'chartFamilies' and process all of the parameters
		// 	getChartParams(chartFamilies);
		//
		// 	return chartFamilies != null && chartFamilies.Count > 0;
		// }


		// // scan revit and get all chart and label information
		// public bool ProcessCharts(CellUpdateTypeCode which)
		// {
		// 	// Families Fams = RevitParamManager.Fams;
		// 	// ChartFamily ChartParams = RevitParamManager.ChartParams;
		// 	// CellFamily CellParams = RevitParamManager.CellParams;
		//
		// 	int fail = 0;
		//
		// 	// process all charts and add to list
		// 	foreach (KeyValuePair<string, RevitChart> kvp in Charts.ListOfCharts)
		// 	{
		// 		// chart has all parameters retreived
		// 		RevitChart chart = kvp.Value;
		//
		// 		if (which != CellUpdateTypeCode.ALL &&
		// 			kvp.Value.UpdateType != which) continue;
		//
		// 		if (!processOneChart(kvp.Value)) fail++;
		// 	}
		//
		// 	return true;
		// }

	#endregion

	#region private methods


		private ICollection<Element> findAllChartFamilies(string chartFamilyName)
		{
			return rvtSelect.FindGenericAnnotationByName(doc, chartFamilyName);
		}


		// private void getChartParams(ICollection<Element> chartFamilies)
		// {
		// 	foreach (Element el in chartFamilies)
		// 	{
		// 		RevitChartData chartData = revitCat.CatagorizeChartSymParams(el);
		//
		// 		chartData.RevitElement = el;
		// 		chartData.AnnoSymbol = (AnnotationSymbol) el;
		//
		// 		string key;
		//
		// 		if (!chartData.IsValid)
		// 		{
		// 			key = "*** error *** (" + (++errorIdx).ToString("D3") + ")";
		// 		}
		// 		else
		// 		{
		// 			// fixed this
		// 			// 	why 8 parameters
		// 			key = RevitParamUtil.MakeAnnoSymKey(chartData,
		// 				(int) RevitParamManager.NameIdx, (int) RevitParamManager.SeqIdx);
		// 		}
		//
		// 		RevitChart chart = new RevitChart();
		//
		// 		chart.RevitChartData = chartData;
		//
		// 		Charts.Add(key, chart);
		// 	}
		// }



		// // provide the list of cell families
		// // process a chart and get the parameters for a family
		// private bool processOneChart(RevitChart chart)
		// {
		// 	string cellFamilyTypeName = chart.RevitChartData.GetValue();
		//
		// 	ICollection<Element> cellElements
		// 		= rvtSelect.GetCellFamilies(RevitDoc.Doc, cellFamilyTypeName);
		//
		// 	if (cellElements == null || cellElements.Count == 0) return false;
		//
		// 	chart.ListOfCellSyms = new Dictionary<string, RevitCellData>();
		//
		// 	foreach (Element cell in cellElements)
		// 	{
		// 		RevitCellData revitCellData = processCellFamily2(cell);
		//
		// 		chart.Add(revitCellData);
		// 	}
		//
		// 	return true;
		// }


		// private RevitCellData processCellFamily2(Element el)
		// {
		// 	RevitCellData revitCellData = revitCat.catagorizeCellParams(el as AnnotationSymbol);
		//
		// 	if (!revitCellData.IsValid) return null;
		//
		// 	return revitCellData;
		// }



		// private void Reset()
		// {
		// 	Charts = new RevitCharts();
		// }

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		// public event PropertyChangedEventHandler PropertyChanged;
		//
		// private void OnPropertyChange([CallerMemberName] string memberName = "")
		// {
		// 	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		// }

	#endregion

	#region system overrides

		public string LocalToString()
		{
			return "shared revit code";
		}

	#endregion
	}
}