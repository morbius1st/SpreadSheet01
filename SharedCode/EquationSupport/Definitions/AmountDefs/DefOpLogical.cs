// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefValue.cs
// Created:      2021-05-30 (7:45 AM)

using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;

namespace EquationSupport.Definitions.AmountDefs
{
	public class DefOpLogOr : ADefBaseString2
	{
		public DefOpLogOr(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_TEXT, order, isNumeric) { }
	}

	public class DefOpLogAnd : ADefBaseString2
	{
		public DefOpLogAnd(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_TEXT, order, isNumeric) { }
	}

	public class DefOpLogEq : ADefBaseString2
	{
		public DefOpLogEq(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_TEXT, order, isNumeric) { }
	}

	public class DefOpLogInEq : ADefBaseString2
	{
		public DefOpLogInEq(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_TEXT, order, isNumeric) { }
	}

	public class DefOpLogNot : ADefBaseString2
	{
		public DefOpLogNot(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_TEXT, order, isNumeric) { }
	}
}