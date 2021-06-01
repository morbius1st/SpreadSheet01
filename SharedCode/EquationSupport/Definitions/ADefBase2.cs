// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             ADefBase2.cs
// Created:      2021-05-30 (7:44 AM)

using SharedCode.EquationSupport.TokenSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;

namespace SharedCode.EquationSupport.Definitions
{
	public abstract class ADefBase2 : ADefBase
	{
		// public int Seq { get; private set; }   // the sequence number in a group
		public int Order { get; private set; } // order of operation - higher gets done first
		public bool IsNumeric { get; private set; }
		public int Index {get; private set; }
		public ValueDataGroup DataGroup { get; } // functional grouping

		protected ADefBase2() { }

		protected ADefBase2(int index, string description,
			string valueStr,
			ValueType valType,
			ValueDataGroup dataGroup,
			int order,
			bool isNumeric) : base(description, valueStr, valType)
		{
			// Seq = seq;
			Order = (int) valType + order;
			IsNumeric = isNumeric;
			Index = index;
			DataGroup = dataGroup;
		}

		public abstract Token MakeToken(string value, int pos, int len);
	}

	public abstract class ADefBaseString2 : ADefBase2
	{
		protected ADefBaseString2(int index, string description, string valueStr, ValueType valType, ValueDataGroup dataGroup, int order,
			bool isNumeric) : base(index, description, valueStr, valType, dataGroup, order, isNumeric) { }

		public override Token MakeToken(string value,int pos, int len)
		{
			AAmtBase ab = new AmtGenericString(Index, value);
			Token t = new Token(ab, pos, len);
	
			return t;
		}
	
		public override bool Equals(string test)
		{
			return (ValueStr?.Equals(string.Empty) ?? false) || (ValueStr?.Equals(test) ?? false);
		}

	}


}