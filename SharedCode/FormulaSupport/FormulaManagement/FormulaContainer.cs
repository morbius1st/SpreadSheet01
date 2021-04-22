#region using

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SpreadSheet01.RevitSupport.RevitCellsManagement;

#endregion

// username: jeffs
// created:  4/19/2021 6:33:31 PM

/*
={[A8]}  = excel cell address
={$name} = system variable	
={#name} = revit variable	
={%name} = project parameter
={!name} = global parameter
={@name} = label name
*/


namespace SharedCode.FormulaSupport.FormulaManagement
{
	public class FormulaContainer : IEnumerable<RevitLabel>
	{
	#region private fields

		private static FormulaContainer me;

		private SortedDictionary<string, RevitLabel> labels;

	#endregion

	#region ctor

		static FormulaContainer()
		{
			if (me == null)
			{
				me = new FormulaContainer();
			}
		}

		public FormulaContainer()
		{
			labels = new SortedDictionary<string, RevitLabel>();
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Add(string key, RevitLabel label)
		{
			labels.Add(key, label);
		}

		public RevitLabel Find(RevitChart chart, string labelName, string labelId)
		{


			return null;
		}

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<RevitLabel> GetEnumerator()
		{
			foreach (KeyValuePair<string, RevitLabel> kvp in labels)
			{
				yield return kvp.Value;
			}
		}

		public override string ToString()
		{
			return "this is FormulaContainer";
		}

	#endregion
	}
}