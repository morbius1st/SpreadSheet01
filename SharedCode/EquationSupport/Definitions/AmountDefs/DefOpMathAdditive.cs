// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefValue.cs
// Created:      2021-05-30 (7:45 AM)

using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;

namespace EquationSupport.Definitions.AmountDefs
{
	public class DefOpMathAdd : ADefBaseString2
	{
		public DefOpMathAdd(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_TEXT, order, isNumeric) { }
	}

	public class DefOpMathSubt : ADefBaseString2
	{
		public DefOpMathSubt(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_TEXT, order, isNumeric) { }
	}
}