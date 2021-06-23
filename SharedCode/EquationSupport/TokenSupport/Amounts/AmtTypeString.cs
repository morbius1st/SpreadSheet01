// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AAmtTypeString.cs
// Created:      2021-06-17 (6:31 PM)

using System.Reflection;
using SharedCode.EquationSupport.Definitions;

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtTypeString : AAmtTypeSpecific<string>
	{
		// the underlying data type
		public override ValueDataGroup DataGroup => ValueDataGroup.VDG_STRING;

		public AmtTypeString(string original) : base(original) { }

		public override string AsString() => Amount;

		public override string ConvertFromString(string original, out bool isValid)
		{
			isValid = true;

			return original;
		}
	}
}