#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SpreadSheet01.RevitSupport.RevitChartInfo;
using SpreadSheet01.RevitSupport.RevitParamInfo;
using UtilityLibrary;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamReadReqmt;

#endregion

// username: jeffs
// created:  2/14/2021 10:56:46 PM

namespace SpreadSheet01.RevitSupport
{
	public class ChartList
	{
		public List<RevitChartItem> Charts { get; } = new List<RevitChartItem>();

		public void Add(RevitChartItem revitChart)
		{
			Charts.Add(revitChart);
		}
	}

	public class RevitManager : INotifyPropertyChanged
	{
	#region private fields

		private const string ROOT_TRANSACTION_NAME = "Transaction Name";

		private const string KEY_ADDR_BEGIN = "〖";
		private const string KEY_ADDR_END   = "〗";
		private const string KEY_IDX_BEGIN  = "《";
		private const string KEY_IDX_END    = "》";

		private Dictionary<string, RevitCellParams> CellItemsBySeqName { get; set; }
		private Dictionary<string, RevitCellParams> CellItemsByNameSeq { get; set; }

		private ChartList chartList;

		private ICollection<Element> chartFamilies;
		private ICollection<Element> cellFamilies;

		private RevitChartItem selectedChart;

		private List<RevitCellParams> errorList;

		private RevitAnnoSyms annoSyms;
		private readonly RevitCatagorizeParam revitCat;
		private readonly RevitSelectSupport rvtSelect;

	#endregion

	#region ctor

		public RevitManager()
		{
			chartList = new ChartList();
			errorList = new List<RevitCellParams>();
			annoSyms = new RevitAnnoSyms();
			revitCat = new RevitCatagorizeParam();
			rvtSelect = new RevitSelectSupport();
		}

	#endregion

	#region public properties

		public ChartList ChartList => chartList;

		public RevitChartItem SelectedChart => selectedChart;

		public bool GotCellFamilies { get; set; }

		public bool GotChart => selectedChart.IsValid;

		public RevitAnnoSyms Symbols => annoSyms;

		public RevitSelectSupport RvtSelect
		{
			get { return rvtSelect; }
		}

		public ICollection<Element> CellFamilies
		{
			get { return cellFamilies; }
			set { cellFamilies = value; }
		}

	#endregion

	#region private properties

	#endregion

	#region public methods


		public void GetCharts()
		{
			TaskDialog td;

			chartFamilies = rvtSelect.FindGenericAnnotationByName(RevitDoc.Doc, "SpreadSheetData");

			if (chartFamilies == null && chartFamilies.Count == 0)
			{
				td = new TaskDialog("Spread Sheets");
				td.MainContent = "No charts found";
				td.MainIcon = TaskDialogIcon.TaskDialogIconError;
				td.CommonButtons = TaskDialogCommonButtons.Ok;
				td.Show();

				return;
			}

			getCharts(chartFamilies);

			OnPropertyChanged(nameof(ChartList));
			
		}



		public string getChart(Document doc, int whichChart = 0)
		{

			TaskDialog td;

			chartFamilies = rvtSelect.FindGenericAnnotationByName(doc, "SpreadSheetData");

			if (chartFamilies == null && chartFamilies.Count == 0)
			{
				td = new TaskDialog("Spread Sheets");
				td.MainContent = "No charts found";
				td.MainIcon = TaskDialogIcon.TaskDialogIconError;
				td.CommonButtons = TaskDialogCommonButtons.Ok;
				td.Show();

				return null;
			}

			getCharts(chartFamilies);

			string chartPath = chartList.Charts[whichChart].ChartPath;
			string chartWorkSheet = chartList.Charts[whichChart].ChartWorkSheet;

			if (chartPath.IsVoid() || chartWorkSheet.IsVoid())
			{
				td = new TaskDialog("Spread Sheets");
				td.MainContent = "Incomplete Chart Information";
				td.MainIcon = TaskDialogIcon.TaskDialogIconError;
				td.CommonButtons = TaskDialogCommonButtons.Ok;
				td.Show();

				return null;
			}

			selectedChart = chartList.Charts[whichChart];

			return chartList.Charts[whichChart].ChartFamilyTypeName;
		}

		// create a list of annotation families of the proper type

		public bool GetAllCellParameters(ParamClass paramClass)
		{
			if (!GotCellFamilies) return false;

			foreach (Element el in cellFamilies)
			{
				AnnotationSymbol annoSym = el as AnnotationSymbol;

				RevitAnnoSym rvtAnnoSym = revitCat.catagorizeAnnoSymParams(annoSym, paramClass);

				rvtAnnoSym.RvtElement = el;
				rvtAnnoSym.AnnoSymbol = annoSym;

				string key = RevitParamUtil.MakeAnnoSymKey(rvtAnnoSym, false);

				annoSyms.Add(key, rvtAnnoSym);
			}

			return true;
		}

		// get all family instances that
		// are generic annotation and
		// have the name provided

	#endregion

	#region private methods

		private void getCharts(ICollection<Element> charts)
		{
			foreach (Element e in charts)
			{
				int found = 0;

				RevitChartItem cp = new RevitChartItem();

				foreach (Parameter p in e.ParametersMap)
				{
					string name = p.Definition.Name;

					foreach (KeyValuePair<string, int> kvp in RevitChartItem.ChartItemIds)
					{
						if (name.Equals(kvp.Key))
						{
							cp.Chart[kvp.Value] = p.AsString();
							found++;
						}
					}

					if (found == RevitChartItem.ItemIdCount) break;
				}

				if (cp != null) chartList.Add(cp);
			}
		}

		public void errorNoCells(string familyTypeName)
		{
			TaskDialog td = new TaskDialog("Spread Sheet Cells for| " + familyTypeName);
			td.MainContent = "No cells found";
			td.MainIcon = TaskDialogIcon.TaskDialogIconError;
			td.CommonButtons = TaskDialogCommonButtons.Ok;
			td.Show();
		}

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
	}

}