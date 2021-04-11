#region using directives

using System.Collections.Generic;
using System.ComponentModel;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitParamManagement;
#if NOREVIT
using CellsTest.CellsTests;
#endif
#endregion

// username: jeffs
// created:  3/1/2021 10:29:39 PM

/* ------------------------ /
 *  CellsTest
 * ----------------------- */


namespace SpreadSheet01.RevitSupport.RevitCellsManagement
{
	public partial class RevitSystemManager : INotifyPropertyChanged
	{
	#region private fields


	#endregion

	#region ctor

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods


		private ICollection<Element> findAllChartFamilies(string chartFamilyName)
		{
		#if NOREVIT

			SampleAnnoSymbols samples = new SampleAnnoSymbols();

			samples.Process(RevitParamManager.CHART_FAMILY_NAME);

			return samples.ChartElements;
		#endif

		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public string LocalToString()
		{
			return "Shared code";
		}

	#endregion

	}
}