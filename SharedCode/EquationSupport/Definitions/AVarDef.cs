﻿// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefVar.cs
// Created:      2021-05-30 (7:45 AM)

using SharedCode.EquationSupport.TokenSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;

namespace SharedCode.EquationSupport.Definitions
{
	public abstract class AVarDef : AValDefBase
	{
		private int valStrLen;
		private int tokStrTrmLen;

		public string TokenStrTerm { get; private set; }
		public ParseGroupVar Group { get; private set; } // functional grouping

		public AVarDef(int index, string description, string valueStr, string tokenStrTerm, ValueType valType, ParseGroupVar @group, int order, bool isNumeric = false
			) : base(index, description, valueStr, valType, ValueDataGroup.VDG_STRING, order, isNumeric)
		{
			TokenStrTerm = tokenStrTerm;
			Group = group;

			valStrLen = valueStr.Length;
			tokStrTrmLen = TokenStrTerm.Length;
		}

		// public  AAmtBase MakeAmt(int index, string value) { return null; }

		// public override Token MakeToken(string value, int pos, int len, int level)
		// {
		// 	return null;
		// }

		public override bool Equals(string test)
		{
			if (ValueStr == null) return false;

			string prefix = test.Substring(0, valStrLen);
			string suffix = test.Substring(test.Length - tokStrTrmLen, tokStrTrmLen);

			return prefix.Equals(ValueStr) && suffix.Equals(TokenStrTerm);
		}
	}
}