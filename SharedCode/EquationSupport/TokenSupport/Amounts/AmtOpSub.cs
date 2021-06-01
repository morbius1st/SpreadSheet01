using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtOpAdd.cs
// Created:      2021-05-24 (10:18 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtOpSub : AAmtTypeString
	{
		static AmtOpSub()
		{
			// ValueDefIdx = ValueDefinitions.Vd_MathSubt;
		}

		public AmtOpSub(string original) : base(Vd_MathSubt, original) { }

		// public override string AsString() => Amount;
		//
		// public override string ConvertFromString(string original)
		// {
		// 	return original;
		// }

		public override string ToString()
		{
			return "This is| " + nameof(AmtOpSub) + " (" + AsString() + ")";
		}
	}
}