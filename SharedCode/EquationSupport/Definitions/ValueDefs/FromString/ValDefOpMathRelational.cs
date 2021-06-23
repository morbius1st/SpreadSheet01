// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefValue.cs
// Created:      2021-05-30 (7:45 AM)

using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.TokenSupport.Amounts;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;

namespace SharedCode.EquationSupport.Definitions.ValueDefs.FromString
{
	public class ValDefOpMathRelLt : AValDefBaseString
	{
		public ValDefOpMathRelLt(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_STRING, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value)
		{
			return new AmtString(value);
		}
	}

	public class ValDefOpMathRelLte : AValDefBaseString
	{
		public ValDefOpMathRelLte(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_STRING, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value)
		{
			return new AmtString(value);
		}
	}

	public class ValDefOpMathRelGt : AValDefBaseString
	{
		public ValDefOpMathRelGt(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_STRING, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value)
		{
			return new AmtString(value);
		}
	}

	public class ValDefOpMathRelGte : AValDefBaseString
	{
		public ValDefOpMathRelGte(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_STRING, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value)
		{
			return new AmtString(value);
		}
	}
}