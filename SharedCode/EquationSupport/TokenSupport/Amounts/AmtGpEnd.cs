using SharedCode.EquationSupport.Definitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtGpEnd.cs
// Created:      2021-05-24 (10:18 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtGpEnd : AAmtTypeSpecific<string>
	{
		static AmtGpEnd()
		{
			ValueDefIdx = ValueDefinitions.Vd_GrpEnd;
		}

		public AmtGpEnd(string original) : base(original) { }

		public override string AsString() => Amount;

		public override string ConvertFromString(string original)
		{
			return original;
		}

		public override string ToString()
		{
			return "This is| " + nameof(AmtGpEnd) + " (" + AsString() + ")";
		}
	}
}