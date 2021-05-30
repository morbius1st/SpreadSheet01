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
// created:  5/22/2021 5:29:23 PM

namespace SharedCode.EquationSupport.TokenSupport
{
	public class testxxx2
	{
		public void txx(AAmtBase iab)
		{
			Tokens tas2 = new Tokens();

			AmtInteger ai2 = new AmtInteger("1234");
			ai2 = AmtInteger.Invalid;
			Token ta = new Token(ai2, 0, 0);
			

		}
	}



	public class Tokens
	{
	#region private fields

		private Token tokenAmtRoot2;
	#endregion

	#region ctor

		public Tokens()
		{
			tokenAmtRoot2 = new Token();
		}

	#endregion

	#region public properties

		public Token this[int idx] => tokenAmtRoot2[idx];

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Add(Token ta2)
		{
			tokenAmtRoot2.Add(ta2);
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
			return "this is| " + nameof(Token) + 
				"(" + tokenAmtRoot2.AmountBase.AsString() + ")";
		}

	#endregion
	}
}