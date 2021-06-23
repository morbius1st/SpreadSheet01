using SharedCode.EquationSupport.EqSupport.ValueSupport;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;
using SharedCode.EquationSupport.Definitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtFract.cs
// Created:      2021-05-24 (10:19 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtFract : AAmtTypeSpecific<double>
	{
		// the underlying data type
		public override ValueDataGroup DataGroup => ValueDataGroup.VDG_NUM_DBL;

		public AmtFract(string original) : base(original) { }

		public override double AsDouble() => Amount;

		public override double ConvertFromString(string original, out bool isValid)
		{
			double result;

			isValid = NumConversions.FractToDouble(original, out result);

			return result;
		}
	}
}