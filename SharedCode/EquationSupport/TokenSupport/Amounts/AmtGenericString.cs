using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtOpAdd.cs
// Created:      2021-05-24 (10:18 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtGenericString : AAmtTypeString
	{
		static AmtGenericString()
		{
			// as this is used by multiple values, the index 
			// gets set to invalid and must be adjusted
			// ValueDefIdx = Vd_Invalid;
		}

		public AmtGenericString(int index, string original) : base(index, original) { }

		// public override string AsString() => Amount;
		//
		// public override string ConvertFromString(string original)
		// {
		// 	return original;
		// }

		public override string ToString()
		{
			return "This is| " + nameof(AmtGenericString) + " (" + AsString() + ")";
		}
	}
}