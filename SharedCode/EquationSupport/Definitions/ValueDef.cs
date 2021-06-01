// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefValue.cs
// Created:      2021-05-30 (7:45 AM)

using SharedCode.EquationSupport.TokenSupport;

namespace SharedCode.EquationSupport.Definitions
{
	public class ValDef : ADefBase2
	{
		public ValDef() { }

		public ValDef(int index, string description, string valueStr, ValueType valType, ValueDataGroup dataGroup,
			/*int seq,*/ int order, bool isNumeric = false) : base(index, description, valueStr, valType, dataGroup, /*seq,*/ order, isNumeric) { }

		public override Token MakeToken(string value, int pos, int len)
		{
			return null;
		}

		public override bool Equals(string test)
		{
			return (ValueStr?.Equals(string.Empty) ?? false) || (ValueStr?.Equals(test) ?? false);
		}
	}
}