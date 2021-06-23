// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefValue.cs
// Created:      2021-05-30 (7:45 AM)

using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.TokenSupport.Amounts;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;

namespace SharedCode.EquationSupport.Definitions.ValueDefs.FromString
{
	public class ValueDefText : AValueDefBaseString
	{
		public ValueDefText(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_TEXT, order, isNumeric) { }

		public override AAmtBase MakeAmt(int index, string value) { return null; }
	}
}
