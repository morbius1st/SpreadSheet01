#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using CellsTest.CellsTests;
using SpreadSheet01.RevitSupport.RevitParamManagement;

#endregion

// user name: jeffs
// created:   3/6/2021 10:21:32 AM

namespace SpreadSheet01.RevitSupport.RevitSelectionSupport
{
	public class RevitSelectSupport
	{
		private SampleAnnoSymbols sample = new SampleAnnoSymbols();

		public int seq = 0;

		// public ICollection<Element> GetCellFamilies(Document doc, string familyTypeName)
		public ICollection<Element> GetCellFamilies(Document doc, string familyTypeName)
		{
			sample.Process(RevitParamManager.CHART_FAMILY_NAME);

			return sample.CellSyms[familyTypeName];
			// return sample.CellElements[seq++];
		}
	}
}
