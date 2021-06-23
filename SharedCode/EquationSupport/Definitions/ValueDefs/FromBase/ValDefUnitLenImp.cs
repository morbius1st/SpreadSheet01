// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefOpMathSubt.cs
// Created:      2021-05-30 (11:11 AM)

using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.TokenSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;

namespace SharedCode.EquationSupport.Definitions.ValueDefs.FromBase
{
	public class ValDefUnitLenMetric : AValDefBase
	{
		public ValDefUnitLenMetric(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_UNIT, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value)
		{
			return new AmtUnit(value);
		}

	}
}