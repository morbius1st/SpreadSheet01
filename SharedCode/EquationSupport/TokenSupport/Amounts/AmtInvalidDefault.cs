using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtGpRef.cs
// Created:      2021-05-24 (10:18 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtInvalid : AAmtTypeSpecific<string>
	{
		public AmtInvalid() : base(Vd_Invalid, "invalid") { }

		public override string AsString() => Amount;

		public override string ConvertFromString(string original, out bool isValid)
		{
			isValid = false;
			return original;
		}

		public override string ToString()
		{
			return "This is| " + nameof(AmtInvalid) + " (" + AsString() + ")";
		}
	}

		public class AmtDefault : AAmtTypeSpecific<string>
	{
		public AmtDefault() : base(Vd_Default, "default") { }

		public override string AsString() => Amount;

		public override string ConvertFromString(string original, out bool isValid)
		{
			isValid = false;
			return original;
		}

		public override string ToString()
		{
			return "This is| " + nameof(AmtDefault) + " (" + AsString() + ")";
		}
	}


}