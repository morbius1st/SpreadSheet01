#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#endregion

// username: jeffs
// created:  5/4/2021 8:33:18 PM

namespace CellsTest.CellsTests
{
	public class EqComponentList
	{

		private const string DIVIDER = "♦";
	#region private fields

		private Tuple<string, Tuple<string, int>[]>[] componentList;

		// private const int EQ_CODE_PARTS_MAX = 20;
		// private string[] eqPartCodeStr;
		// private int[] eqPartCodeIdx;
		// private Tuple<int, int>[] eqPartCodeIdx2;

	#endregion

	#region ctor

		public EqComponentList(Tuple<string, Tuple<string, int>[]>[] compList)
		{
			componentList = compList;
		}


	#endregion

	#region public properties

		public Tuple<string, Tuple<string, int>[]>[] ComponentList => componentList;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public int Classify(string code)
		{
			foreach (Tuple<string, Tuple<string, int>[]> t1 in componentList)
			{
				foreach (Tuple<string, int> t in t1.Item2)
				{
					if (t.Item1.Equals(code)) return t.Item2;
				}
			}

			return -1;
		}

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is EqComponentList";
		}

	#endregion
	}
}