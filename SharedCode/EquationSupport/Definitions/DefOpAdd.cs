// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefValue.cs
// Created:      2021-05-30 (7:45 AM)

using SharedCode.EquationSupport.TokenSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;

namespace SharedCode.EquationSupport.Definitions
{
	public class DefOpAdd : ADefBase2
	{
		public DefOpAdd() { }

		public DefOpAdd(string description, string valueStr, ValueType valType, 
			int seq, int order, bool isNumeric = false) : base(description, valueStr, valType, seq, order, isNumeric) { }

		public override Token MakeToken(int pos, int len)
		{

			AAmtBase ab = new AmtOpAdd(ValueStr);

			// Token t = new Token()

			return null;
		}

		public override bool Equals(string test)
		{
			return (ValueStr?.Equals(string.Empty) ?? false) || (ValueStr?.Equals(test) ?? false);
		}
	}
}