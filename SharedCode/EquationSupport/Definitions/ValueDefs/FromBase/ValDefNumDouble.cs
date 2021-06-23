// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefOpMathSubt.cs
// Created:      2021-05-30 (11:11 AM)

using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.ParseSupport;
using SharedCode.EquationSupport.TokenSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;

namespace SharedCode.EquationSupport.Definitions.ValueDefs.FromBase
{
	public class ValDefNumDouble : AValDefBase
	{
		public ValDefNumDouble(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_NUM_DBL, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value)
		{
			return new AmtNumDbl(value);
		}

		public override string ToString()
		{
			return $"this is| {nameof(ValDefNumDouble)} ({ValueStr})";
		}

	}
}