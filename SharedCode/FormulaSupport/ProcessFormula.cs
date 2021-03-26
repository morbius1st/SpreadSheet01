#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharedCode.FormulaSupport.ParseSupport;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  3/15/2021 10:03:00 PM

namespace SharedCode.FormulaSupport
{
	public enum FormulaVariableType
	{
		UNASSIGNED			= -1,
		CELL_ADDRESS,		// [], <ca>
		SYST_VARIABLE,		// $, <sv>
		REVIT_PARAMETER,	// #, <rp>
		PROJECT_PARAMETER,	// %, <pp>
		GLOBAL_PARAMETER,	// !, <gp>
		LABEL_NAME			// @, <ln>
	}

	public class ValuePair<T,U>
	{
		public T Key { get; set; }
		public U Value { get; set; }

		public ValuePair()
		{
			Key = default(T);
			Value = default(U);
		}

		public ValuePair(T key, U value)
		{
			Key = key;
			Value = value;
		}
	}


	public class ProcessFormula
	{

	#region private fields

		private ProcessFormulaSupport pfs = new ProcessFormulaSupport();

	#endregion

	#region ctor

		// public ProcessFormula()
		// { }

	#endregion

	#region public properties

		public bool Processed { get; private set; }


	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool Process(string formula)
		{
			Processed = false;

			string f = formula.Trim();

			pfs.Clear();

			if (!pfs.FormulaParse(f)) return false;

			Processed = true;

			return true;
		}

		public bool GetKeyVars(string formula, out bool gotLeftSide,
			out ValuePair<String, string> leftSide, 
			out ValuePair<String, string> rightSide )
		{
			gotLeftSide = false;

			leftSide = new ValuePair<string, string>();
			rightSide = new ValuePair<string, string>();

			List<ValuePair<string, string>> bothSides = pfs.GetKeyVar(formula);

			Debug.WriteLine("results are| " + (bothSides == null ? "null" : " count| " + bothSides.Count));

			if (bothSides == null) return false;

			if (bothSides.Count !=2 && bothSides.Count != 1)
			{
				return false;
			}

			int rightSideIdx = 0;

			if (bothSides.Count == 2)
			{
				leftSide.Key = bothSides[0].Key;
				leftSide.Value = bothSides[0].Value;
				gotLeftSide = true;
				rightSideIdx = 1;
			} 

			rightSide.Key = bothSides[rightSideIdx].Key;
			rightSide.Value = bothSides[rightSideIdx].Value;

			return true;
		}

		public bool GetKeyVars2(string formula,
			out List<ValuePair<int, string>> keyVars )
		{
			// parse formula / create complete token list
			pfs.FormulaParse(formula);

			return pfs.GetKeyVars2(out keyVars);
		}

		public Tuple<int, char, TestType, TestStatusCode> GetKeyVars3(string formula,
			out ValuePair<int, string> keyVar )
		{
			// parse formula / create complete token list
			pfs.FormulaParse(formula);

			if (!pfs.GetKeyVars3(out keyVar)) return AStringValidate.GeneralFail;

			return pfs.ValidateKeyVar(keyVar);
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
			return "this is ProcessFormula";
		}

	#endregion
	}
}