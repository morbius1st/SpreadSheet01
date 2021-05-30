#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharedCode.EquationSupport.ParseSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;

#endregion

// username: jeffs
// created:  5/22/2021 5:04:13 PM

namespace SharedCode.EquationSupport.TokenSupport
{

	public class Token
	{
	#region private fields

		private int position;	// position in the formula string
		private int length; // length in the formula string

		private int level = 0;
		private AAmtBase aAmtBase;
		private List<Token> tokenAmts2 = null;

	#endregion

	#region ctor

		public Token()
		{
			this.aAmtBase = null;
			position = 0;
			length = 0;
		}

		public Token(AAmtBase aAmtBase, int pos, int len)
		{
			this.aAmtBase = aAmtBase;
			position = pos;
			length = len;
		}

	#endregion

	#region public properties

		public AAmtBase AmountBase => aAmtBase;
		public ValueType DataType => aAmtBase.DataType;
		public Token this[int idx] => tokenAmts2[idx];
		public int Position => position;
		public int Length => length;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public Token MakeBranch()
		{
			Token t = new Token();
			t.level++;
			t.tokenAmts2 = new List<Token>();
			tokenAmts2.Add(t);

			return t;
		}

		public void Add(Token t)
		{
			tokenAmts2.Add(t);
		}


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
			return "this is| " + nameof(Token) + "(" + aAmtBase.AsString() + ")";
		}

	#endregion
	}
}