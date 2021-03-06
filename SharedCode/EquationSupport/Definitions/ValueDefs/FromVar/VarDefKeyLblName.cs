﻿// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefValue.cs
// Created:      2021-05-30 (7:45 AM)

using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.ParseSupport;
using SharedCode.EquationSupport.TokenSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

namespace SharedCode.EquationSupport.Definitions.ValueDefs.FromVar
{
	public class VarDefKeyLblName : AVarDef
	{
		public VarDefKeyLblName(
			int index, string description, string valueStr, string tokenStrTerm, ValueType valType, 
			ParseGroupVar @group, int order, bool isNumeric = false) : 
			base(index, description, valueStr, tokenStrTerm, valType,@group, order, isNumeric) { }

		public override AAmtBase MakeAmt( string value)
		{
			return new AmtStrText(value);
		}

		// public override Token MakeToken(string value, int pos, int len, int level)
		// {
		// 	AAmtBase ab = new AmtVarKey(Vd_VarLblName, value);
		// 	Token t = new Token(ab, new ParseDataInfo(pos,len,level));
		//
		// 	return t;
		// }
	
		// public override bool Equals(string test)
		// {
		// 	return (ValueStr?.Equals(string.Empty) ?? false) || (ValueStr?.Equals(test.Substring(0, ValueStr.Length)) ?? false);
		// }

		public override string ToString()
		{
			return $"this is| {nameof(VarDefKeyLblName)} ({ValueStr})";
		}

	}
}