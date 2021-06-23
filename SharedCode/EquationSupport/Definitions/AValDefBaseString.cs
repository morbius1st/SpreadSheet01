// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             ADefBaseString.cs
// Created:      2021-06-18 (2:29 PM)

using System;
using SharedCode.EquationSupport.ParseSupport;
using SharedCode.EquationSupport.TokenSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;

namespace SharedCode.EquationSupport.Definitions
{
	public abstract class AValDefBaseString : AValDefBase
	{
		protected AAmtBase a;

		protected AValDefBaseString(int index, string description, string valueStr, 
			ValueType valType, ValueDataGroup dataGroup, int order, bool isNumeric) 
			: base(index, description, valueStr, valType, dataGroup, order, isNumeric) { }

		// public override Token MakeToken(string value,int pos, int len, int level)
		// {
		// 	AAmtBase ab;
		// 	ab = MakeAmt(Index, value);
		// 	ab = new AmtTypeString(Index, value);
		// 	Token t = new Token(ab, new ParseDataInfo(pos,len,level));
		//
		// 	return t;
		// }
		
		public override string ToString()
		{
			Type t = this.GetType();

			string a = t.Name;
			string b = t.DeclaringType?.Name ?? "null name";

			return $"This is| {this.GetType().Name} ({this.ValueStr})";
			// return $"({a}) ({b})";
		}

		

	}
}