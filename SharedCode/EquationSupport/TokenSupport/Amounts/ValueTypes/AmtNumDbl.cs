using SharedCode.EquationSupport.EqSupport.ValueSupport;
using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtDouble.cs
// Created:      2021-05-24 (10:19 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtNumDbl : AAmtTypeSpecific<double>
	{
		// the underlying data type
		public override ValueDataGroup DataGroup => ValueDataGroup.VDG_NUM_DBL;

		public AmtNumDbl(string original) : base(original) { }

		public override double AsDouble() => Amount;

		public override double ConvertFromString(string original, out bool isValid)
		{
			double result;

			isValid = NumConversions.StringToDouble(original, out result);

			return result;
		}
	}
}