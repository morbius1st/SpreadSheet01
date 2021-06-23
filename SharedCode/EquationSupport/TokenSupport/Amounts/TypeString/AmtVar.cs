using SharedCode.EquationSupport.Definitions.ValueDefs.FromVar;
using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtInteger.cs
// Created:      2021-05-24 (10:18 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtVar : AAmtTypeSpecific<string>
	{
		// the underlying data type
		public override ValueDataGroup DataGroup => ValueDataGroup.VDG_STRING;

		public AmtVar(string original) : base(original) { }

		public override string AsString() => Amount;

		public override string ConvertFromString(string original, out bool isValid)
		{
			isValid = true;
			return original;
		}
	}
}