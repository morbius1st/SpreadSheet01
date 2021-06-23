// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefValue.cs
// Created:      2021-05-30 (7:45 AM)

using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.TokenSupport.Amounts;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;

namespace SharedCode.EquationSupport.Definitions.ValueDefs.FromString
{
	public class ValDefOpMathLogOr : AValDefBaseString
	{
		public ValDefOpMathLogOr(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_STRING, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value)
		{
			return new AmtString(value);
		}
	}

	public class ValDefOpMathLogAnd : AValDefBaseString
	{
		public ValDefOpMathLogAnd(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_STRING, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value)
		{
			return new AmtString(value);
		}
	}

	public class ValDefOpMathLogEq : AValDefBaseString
	{
		public ValDefOpMathLogEq(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_STRING, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value)
		{
			return new AmtString(value);
		}
	}

	public class ValDefOpMathLogInEq : AValDefBaseString
	{
		public ValDefOpMathLogInEq(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_STRING, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value)
		{
			return new AmtString(value);
		}
	}

	public class ValDefOpMathLogNot : AValDefBaseString
	{
		public ValDefOpMathLogNot(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_STRING, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value)
		{
			return new AmtString(value);
		}
	}
}