// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefValue.cs
// Created:      2021-05-30 (7:45 AM)

using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;

namespace EquationSupport.Definitions.AmountDefs
{
	public class DefGrpRef : ADefBaseString2
	{
		public DefGrpRef(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_TEXT, order, isNumeric) { }
	}
	public class DefGrpBeg : ADefBaseString2
	{
		public DefGrpBeg(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_TEXT, order, isNumeric) { }
	}
	public class DefGrpEnd : ADefBaseString2
	{
		public DefGrpEnd(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_TEXT, order, isNumeric) { }
	}
}