// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefVar.cs
// Created:      2021-05-30 (7:45 AM)

using SharedCode.EquationSupport.TokenSupport;

namespace SharedCode.EquationSupport.Definitions
{
	public class DefVar : ADefBase2
	{
		private int valStrLen;
		private int tokStrTrmLen;

		public string TokenStrTerm { get; private set; }
		public ParseGroupVar Group { get; private set; } // functional grouping

		public DefVar() { }

		public DefVar(string description, string valueStr, string tokenStrTerm, ValueType valType, ParseGroupVar @group, 
			int seq, int order, bool isNumeric = false) : base(description, valueStr, valType, seq, order, isNumeric)
		{
			TokenStrTerm = tokenStrTerm;
			Group = group;

			valStrLen = valueStr.Length;
			tokStrTrmLen = TokenStrTerm.Length;
		}

		public override Token MakeToken(int pos, int len)
		{
			return null;
		}

		public override bool Equals(string test)
		{
			if (ValueStr == null) return false;

			string prefix = test.Substring(0, valStrLen);
			string suffix = test.Substring(test.Length - tokStrTrmLen, tokStrTrmLen);

			return prefix.Equals(ValueStr) && suffix.Equals(TokenStrTerm);
		}
	}
}