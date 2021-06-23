// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefValue.cs
// Created:      2021-05-30 (7:45 AM)

using System;
using SharedCode.EquationSupport.TokenSupport.Amounts;
using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.ParseSupport;
using SharedCode.EquationSupport.TokenSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;

namespace SharedCode.EquationSupport.Definitions.ValueDefs.FromString
{
	public class ValDefAssignment : AValDefBaseString
	{
		public ValDefAssignment(int index, string description, string valueStr, ValueType valType,
			int order, bool isNumeric = false) :
			base(index, description, valueStr, valType, VDG_STRING, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value)
		{
			return new AmtString(value);
		}
	}
}