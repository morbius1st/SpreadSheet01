#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.TokenSupport.Values;
using ValueType = SharedCode.EquationSupport.Definitions.ValueType;

#endregion

// username: jeffs
// created:  5/21/2021 7:03:28 AM

namespace SharedCode.EquationSupport.TokenSupport
{
	public class Token
	{
	#region private fields

		private int level = 0;
		private IValBase amountBase;
		private List<Token> tokens = new List<Token>();

	#endregion

	#region ctor

		public Token(IValBase amtBase)
		{
			amountBase = amtBase;
		}

	#endregion

	#region public properties

		public IValBase AmountBase    => amountBase;
		public ValueType DataType => amountBase.DataType;
		// public List<Token> Tokens
		// {
		// 	get => tokens;
		// 	set => tokens = value;
		// }

		public Token this[int idx] => tokens[idx];
 
	#endregion

	#region private properties

	#endregion

	#region public methods

		public Token MakeBranch()
		{
			Token t = new Token(null);
			t.level++;
			t.tokens = new List<Token>();
			tokens.Add(t);

			return t;
		}

		public void Add(Token t)
		{
			tokens.Add(t);
		}

		public object AsObject() => amountBase.AsObject();
		public string AsString() => amountBase.AsString();
		public bool AsBool()     => amountBase.AsBool();
		public int AsInteger()   => amountBase.AsInteger();
		public double AsDouble() => amountBase.AsDouble();
		public UoM AsUnit()      => amountBase.AsUnit();

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is Token";
		}

	#endregion
	}
}