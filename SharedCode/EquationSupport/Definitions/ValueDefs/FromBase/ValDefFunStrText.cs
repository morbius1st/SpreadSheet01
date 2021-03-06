﻿// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefValue.cs
// Created:      2021-05-30 (7:45 AM)

using SharedCode.EquationSupport.TokenSupport.Amounts;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

namespace SharedCode.EquationSupport.Definitions.ValueDefs.FromBase
{
	public class ValDefFunStrText : AValDefBaseString
	{
		public ValDefFunStrText(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_STRING, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value)
		{
			return new AmtStrText(value);
		}
	}
}