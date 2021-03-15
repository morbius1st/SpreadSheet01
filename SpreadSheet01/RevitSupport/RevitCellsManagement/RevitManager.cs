#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport;
using SpreadSheet01.RevitSupport.RevitParamInfo;
using SpreadSheet01.RevitSupport.RevitParamValue;
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

		private RevitCatagorizeParam revitCat;
		private RevitSelectSupport rvtSelect;

		private RevitCharts rvtCharts = new RevitCharts();

	#endregion

	#region ctor

		public RevitManager()
		{
			revitCat = new RevitCatagorizeParam();
			rvtSelect = new RevitSelectSupport();
		}

	#endregion

	#region public properties

		public RevitSelectSupport RvtSelect
		{
			get { return rvtSelect; }
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

		public bool ProcessCharts(RevitCharts Charts, CellUpdateTypeCode which)
		{
			int fail = 0;
			// process all charts and add to list
			foreach (KeyValuePair<string, RevitChart> kvp in Charts.ListOfCharts)
			{
				// chart has all parameters retreived
				RevitChart chart = kvp.Value;

				if (which != CellUpdateTypeCode.ALL &&
					kvp.Value.UpdateType != which) continue;

				processOneChart(kvp.Value);
			}

			return true;
		}

		// provide the list of cell families
		// process a chart and get the parameters for a family
		private bool processOneChart(RevitChart chart)
		{
			string cellFamilyTypeName = chart.RevitChartData.GetValue();

		#if REVIT
			ICollection<Element> cellElements
				= RvtSelect.GetCellFamilies(RevitDoc.Doc, cellFamilyTypeName);
		#endif

		#if NOREVIT
			int i = Int32.Parse(chart[RevitParamManager.SeqIdx].GetValue());

			ICollection<Element> cellElements
				= RvtSelect.GetCellFamilies(RevitDoc.Doc, cellFamilyTypeName, i);

		#endif


			if (cellElements == null || cellElements.Count == 0) return false;

			chart.ListOfCellSyms = new Dictionary<string, RevitCell>();

			foreach (Element cell in cellElements)
			{
				RevitCell rvtCell = processCellFamily2(cell);

				chart.Add(rvtCell);
			}

			return true;
		}


		private RevitCell processCellFamily2(Element el)
		{
			RevitCell rvtCell = revitCat.catagorizeCellParams(el as AnnotationSymbol);

			if (!rvtCell.IsValid) return null;

			return rvtCell;
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