using SharedCode.EquationSupport.Definitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtGpBeg.cs
// Created:      2021-05-24 (10:18 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtGpBeg : AAmtTypeSpecific<string>
	{
		static AmtGpBeg()
		{
			ValueDefIdx = ValueDefinitions.Vd_GrpBeg;
		}

		public AmtGpBeg(string original) : base(original) { }

		public override string AsString() => Amount;

		public override string ConvertFromString(string original)
		{
			return original;
		}

		public override string ToString()
		{
			return "This is| " + nameof(AmtGpBeg) + " (" + AsString() + ")";
		}
	}
}