// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             ADefBase2.cs
// Created:      2021-05-30 (7:44 AM)

using System;
using SharedCode.EquationSupport.ParseSupport;
using SharedCode.EquationSupport.TokenSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;

namespace SharedCode.EquationSupport.Definitions
{
	public abstract class AValDefBase : ADefBase
	{
		// public int Seq { get; private set; }   // the sequence number in a group
		public int Order { get; set; } // order of operation - higher gets done first
		public bool IsNumeric { get; set; }
		public int Index {get;  set; }
		public ValueDataGroup DataGroup { get; } // functional grouping

		protected AValDefBase() { }

		public AValDefBase(int index, 
			string description,
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

		public abstract AAmtBase MakeAmt( string value);

		public Token MakeToken(string value, int pos, int len, int level)
		{
			AAmtBase ab = MakeAmt(value);
			Token t = new Token(this, ab, new ParseDataInfo(pos,len,level));
	
			return t;
		}

		public override bool Equals(string test)
		{
			return (ValueStr?.Equals(string.Empty) ?? false) || (ValueStr?.Equals(test) ?? false);
		}

	}
}