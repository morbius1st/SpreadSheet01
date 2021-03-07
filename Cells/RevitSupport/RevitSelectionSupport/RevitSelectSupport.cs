#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Cells.CellsTests;

#endregion

// user name: jeffs
// created:   3/6/2021 10:21:32 AM

namespace SpreadSheet01.RevitSupport.RevitSelectionSupport
{
	public class RevitSelectSupport
	{
		private SampleAnnoSymbols sample = new SampleAnnoSymbols();

		public ICollection<Element> GetCellFamilies(Document doc, string familyTypeName)
		{
			sample.Process();

			return sample.CellElements;
		}
	}
}
