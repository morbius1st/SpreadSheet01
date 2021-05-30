using SharedCode.EquationSupport.Definitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtGpRef.cs
// Created:      2021-05-24 (10:18 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtInvalid : AAmtTypeSpecific<string>
	{
		static AmtInvalid()
		{
			ValueDefIdx = ValueDefinitions.Vd_Invalid;
		}

		public AmtInvalid() : base("invalid") { }

		public override string AsString() => Amount;

		public override string ConvertFromString(string original)
		{
			return original;
		}

		public override string ToString()
		{
			return "This is| " + nameof(AmtInvalid) + " (" + AsString() + ")";
		}
	}
}