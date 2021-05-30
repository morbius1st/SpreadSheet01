using SharedCode.EquationSupport.Definitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtOpAdd.cs
// Created:      2021-05-24 (10:18 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtOpAdd : AAmtTypeSpecific<string>
	{
		static AmtOpAdd()
		{
			ValueDefIdx = ValueDefinitions.Vd_AddStr;
		}

		public AmtOpAdd(string original) : base(original) { }

		public override string AsString() => Amount;

		public override string ConvertFromString(string original)
		{
			return original;
		}

		public override string ToString()
		{
			return "This is| " + nameof(AmtOpAdd) + " (" + AsString() + ")";
		}
	}
}