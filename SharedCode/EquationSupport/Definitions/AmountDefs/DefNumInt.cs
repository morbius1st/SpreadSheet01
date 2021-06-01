// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             DefOpMathSubt.cs
// Created:      2021-05-30 (11:11 AM)

using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.TokenSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;

namespace EquationSupport.Definitions.AmountDefs
{
	public class DefNumInt : ADefBase2
	{

		public DefNumInt(int index, string description, string valueStr, ValueType valType, 
			int order, bool isNumeric = false) : base(index, description, valueStr, valType, VDG_NUM_INT, order, isNumeric) { }
	
		public override Token MakeToken(string value, int pos, int len)
		{
			AAmtBase ab = new AmtInteger(value);
			Token t = new Token(ab, pos, len);
	
			return t;
		}
	
		public override bool Equals(string test)
		{
			return (ValueStr?.Equals(string.Empty) ?? false) || (ValueStr?.Equals(test) ?? false);
		}
	}
}