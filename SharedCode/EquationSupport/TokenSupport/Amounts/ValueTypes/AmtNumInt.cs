using SharedCode.EquationSupport.EqSupport.ValueSupport;
using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtInteger.cs
// Created:      2021-05-24 (10:18 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtNumInt : AAmtTypeSpecific<int>
	{
		// the underlying data type
		public override ValueDataGroup DataGroup => ValueDataGroup.VDG_NUM_INT;

		public AmtNumInt(string original) : base(original) { }

		public override int AsInteger() => Amount;

		public override int ConvertFromString(string original, out bool isValid)
		{
			int result;

			isValid = NumConversions.StringToInteger(original, out result);

			return result;
		}
	}
}