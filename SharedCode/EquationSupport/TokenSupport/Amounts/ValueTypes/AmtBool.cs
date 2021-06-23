using SharedCode.EquationSupport.EqSupport.ValueSupport;
using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtInteger.cs
// Created:      2021-05-24 (10:18 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtBool : AAmtTypeSpecific<bool?>
	{
		// the underlying data type
		public override ValueDataGroup DataGroup => ValueDataGroup.VDG_BOOLEAN;

		public AmtBool(string original) : base(original) { }

		public override bool? AsBool() => Amount;

		public override bool? ConvertFromString(string original, out bool isValid)
		{
			bool? result;

			isValid = NumConversions.StringToBoolean(original, out result);

			return result;
		}
	}
}