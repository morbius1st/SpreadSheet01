// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefValue.cs
// Created:      2021-05-30 (7:45 AM)

using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.TokenSupport.Amounts;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;

namespace SharedCode.EquationSupport.Definitions.ValueDefs.FromString
{
	public class ValDefOpStrRelStartsWith : AValDefBaseString
	{
		public ValDefOpStrRelStartsWith(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_STRING, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value) { return null; }
	}
	
	public class ValDefOpStrRelEndsWith : AValDefBaseString
	{
		public ValDefOpStrRelEndsWith(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_STRING, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value) { return null; }
	}

	public class ValDefOpStrRelContains : AValDefBaseString
	{
		public ValDefOpStrRelContains(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_STRING, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value) { return null; }
	}

	public class ValDefOpStrRelStartsWithCi : AValDefBaseString
	{
		public ValDefOpStrRelStartsWithCi(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_STRING, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value) { return null; }
	}
	
	public class ValDefOpStrRelEndsWithCi : AValDefBaseString
	{
		public ValDefOpStrRelEndsWithCi(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_STRING, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value) { return null; }
	}

	public class ValDefOpStrRelContainsCi : AValDefBaseString
	{
		public ValDefOpStrRelContainsCi(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_STRING, order, isNumeric) { }
		
		public override AAmtBase MakeAmt( string value) { return null; }


	}


}