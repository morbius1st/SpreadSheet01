#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Cells.CellsTests;
using SpreadSheet01.RevitSupport;
using SpreadSheet01.RevitSupport.RevitChartInfo;
using SpreadSheet01.RevitSupport.RevitParamValue;
using UtilityLibrary;
using static SpreadSheet01.RevitSupport.RevitChartInfo.RevitChartParameters;

#endregion

// username: jeffs
// created:  3/1/2021 10:29:39 PM

namespace SpreadSheet01.RevitSupport
{
	public class RevitChartManager : INotifyPropertyChanged
	{
	#region private fields

		private const string KEY_IDX_BEGIN  = "《";
		private const string KEY_IDX_END    = "》";

		private RevitParamManager paramMgr;

		public RevitCharts Charts { get; private set; } = new RevitCharts();

		private readonly RevitCatagorizeParam revitCat;

		private static int annoSymUniqueIdx = 0;

	#endregion

	#region ctor

		public RevitChartManager()
		{
			Reset();

			paramMgr = new RevitParamManager();
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void GetCurrentCharts() { }

		public string MakeChartSymKey(RevitChartSym cSym, bool asSeqName)
		{
			string seq = cSym[ChartSeqIdx].GetValue();

			seq = KEY_IDX_BEGIN + $"{(seq.IsVoid() ? "ZZZZZ" : seq),8}" + KEY_IDX_END;

			string name = cSym[ChartNameIdx].GetValue();
			name = name.IsVoid() ? "un-named" : name;

			string eid = cSym.AnnoSymbol?.Id.ToString() ?? "Null Symbol " + annoSymUniqueIdx++.ToString("D7");

			if (asSeqName) return seq + name + eid;

			return name + seq + eid;
		}

	#endregion

	#region private methods

		private void selectChartFamilies()
		{
		#if NOREVIT

			SampleAnnoSymbols sample = new SampleAnnoSymbols();

			foreach (AnnotationSymbol cSym in sample.Charts)
			{
				RevitChartSym chartSym = revitCat.catagorizeChartSymParams(cSym, ParamClass.CHART);

				chartSym.RvtElement = new Element();
				chartSym.AnnoSymbol = cSym;

				string key = MakeChartSymKey(chartSym);

				Charts.Add(key, chartSym);
			}


		#endif

		#if REVIT
			ICollection<Element> chartFamilies = null;

			// chartFamilies = rvtSelect.FindGenericAnnotationByName(RevitDoc.Doc, "SpreadSheetData");

			foreach (Element chartFamily in chartFamilies)
			{
				RevitChart c = new RevitChart();

				c.RvtElement = chartFamily;

				c.AnnoSymbol = (AnnotationSymbol) chartFamily;
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