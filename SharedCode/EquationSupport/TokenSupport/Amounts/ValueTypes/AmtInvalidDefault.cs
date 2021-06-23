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
		// the underlying data type
		public override ValueDataGroup DataGroup => ValueDataGroup.VDG_INVALID;

		public AmtInvalid() : base("invalid") { }
	
		public override string AsString() => Amount;
	
		public override string ConvertFromString(string original, out bool isValid)
		{
			isValid = false;
			return original;
		}
	}
	
		public class AmtDefault : AAmtTypeSpecific<string>
	{
		// the underlying data type
		public override ValueDataGroup DataGroup => ValueDataGroup.VDG_DEFAULT;

		public AmtDefault() : base("default") { }
	
		public override string AsString() => Amount;
	
		public override string ConvertFromString(string original, out bool isValid)
		{
			isValid = false;
			return original;
		}
	}


}