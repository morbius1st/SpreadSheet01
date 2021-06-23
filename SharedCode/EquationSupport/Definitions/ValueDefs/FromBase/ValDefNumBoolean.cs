// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefOpMathSubt.cs
// Created:      2021-05-30 (11:11 AM)

using SharedCode.EquationSupport.TokenSupport.Amounts;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

namespace SharedCode.EquationSupport.Definitions.ValueDefs.FromBase
{

	public class ValDefNumBoolTrue : AValDefBase
	{
		public ValDefNumBoolTrue(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_BOOLEAN, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value)
		{
			return new AmtBool(value);
		}
	}

	public class ValDefNumBoolFalse : AValDefBase
	{
		public ValDefNumBoolFalse(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_BOOLEAN, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value)
		{
			return new AmtBool(value);
		}
	}

	public class ValDefNumBoolNull : AValDefBase
	{
		public ValDefNumBoolNull(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_BOOLEAN, order, isNumeric) { }

		public override AAmtBase MakeAmt(string value)
		{
			return new AmtBool(value);
		}
	}
}