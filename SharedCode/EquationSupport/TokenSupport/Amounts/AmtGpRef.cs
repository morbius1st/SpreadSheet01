using SharedCode.EquationSupport.Definitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtGpRef.cs
// Created:      2021-05-24 (10:18 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtGpRef : AAmtTypeSpecific<string>
	{
		static AmtGpRef()
		{
			ValueDefIdx = ValueDefinitions.Vd_GrpRef;
		}

		public AmtGpRef(string original) : base(original) { }

		public override string AsString() => Amount;

		public override string ConvertFromString(string original)
		{
			return original;
		}

		public override string ToString()
		{
			return "This is| " + nameof(AmtGpRef) + " (" + AsString() + ")";
		}
	}
}