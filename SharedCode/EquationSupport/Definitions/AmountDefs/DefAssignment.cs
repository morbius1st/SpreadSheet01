// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefValue.cs
// Created:      2021-05-30 (7:45 AM)

using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;

namespace EquationSupport.Definitions.AmountDefs
{
	public class DefAssignment : ADefBaseString2
	{
		public DefAssignment(int index, string description, string valueStr, ValueType valType,
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_TEXT, order, isNumeric)
		{

		}

	}
}