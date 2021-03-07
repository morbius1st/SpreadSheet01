#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using UtilityLibrary;
using SpreadSheet01.RevitSupport.RevitParamInfo;
#if NOREVIT
using Cells.CellsTests;
using SpreadSheet01.RevitSupport.RevitChartInfo;


#endif

#endregion

// username: jeffs
// created:  3/1/2021 10:29:39 PM

namespace SpreadSheet01.RevitSupport
{
	public class RevitChartManager : INotifyPropertyChanged
	{
	#region private fields

		private const string CHART_FAMILY_NAME = "SpreadSheetData";

		private const string KEY_IDX_BEGIN  = "《";
		private const string KEY_IDX_END    = "》";

		private ICollection<Element> chartFamilies;

		private RevitParamManager paramMgr;



		private int errorIdx;
		private List<RevitChartSym> errorSyms = new List<RevitChartSym>();

		private readonly RevitCatagorizeParam revitCat = new RevitCatagorizeParam();

		private static int annoSymUniqueIdx = 0;

		// collection of all revit charts  
		// this holds a collection of individual charts
		public RevitCharts Charts { get; private set; } = new RevitCharts();

	#endregion

	#region ctor

		public RevitChartManager()
		{
			Reset();

			paramMgr = new RevitParamManager();
		}

	#endregion

	#region public properties

		public string RevitChartFamilyName => CHART_FAMILY_NAME;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public int GetCurrentCharts()
		{
			// step 1 - select all of the chart cells - place them into: 'chartFamilies'
			ICollection<Element> chartFamilies = findAllChartFamilies(CHART_FAMILY_NAME);

			// step 2 - process 'chartFamilies' and process all of the parameters
			getChartFamilyParameters(chartFamilies);

			return chartFamilies != null ? chartFamilies.Count : 0;

		}


		// #if NOREVIT
		// 	public string MakeChartSymKey(RevitChartSym cSym, bool asSeqName = false)
		// 	{
		// 		string seq = cSym[ChartSeqIdx].GetValue();
		//
		// 		seq = KEY_IDX_BEGIN + $"{(seq.IsVoid() ? "ZZZZZ" : seq),8}" + KEY_IDX_END;
		//
		// 		string name = cSym[ChartNameIdx].GetValue();
		// 		name = name.IsVoid() ? "un-named" : name;
		//
		// 		string eid = cSym.AnnoSymbol?.Id.ToString() ?? "Null Symbol " + annoSymUniqueIdx++.ToString("D7");
		//
		// 		if (asSeqName) return seq + name + eid;
		//
		// 		return name + seq + eid;
		// 	}
		// #endif

	#endregion

	#region private methods

		private ICollection<Element> findAllChartFamilies(string chartFamilyName)
		{
		#if NOREVIT

			SampleAnnoSymbols samples = new SampleAnnoSymbols();

			samples.Process();

			return samples.ChartElements;
		#endif

		#if REVIT
			return null;

		#endif
		}

		private void getChartFamilyParameters(ICollection<Element> chartFamilies)
		{
		#if NOREVIT
			foreach (Element el in chartFamilies)
			{
				RevitChartSym chartSym = revitCat.CatagorizeChartSymParams(el, ParamClass.CHART);

				chartSym.RvtElement = el;
				chartSym.AnnoSymbol = (AnnotationSymbol) el;

				string key;

				if (!chartSym.IsValid)
				{
					key = "*** error *** (" + (++errorIdx).ToString("D3") + ")";
				}
				else
				{
					key = RevitParamUtil.MakeAnnoSymKey(chartSym, 
						(int) RevitChartParameters.ChartNameIdx, (int) RevitChartParameters.ChartSeqIdx);
				}

				RevitChart chart = new RevitChart();

				chart.RvtChartSym = chartSym;

				Charts.Add(key, chart);
			}
		#endif
		}


		private void Reset()
		{
			Charts = new RevitCharts();
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
			return "this is RevitChartManager";
		}

	#endregion
	}
}