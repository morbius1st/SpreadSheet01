#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharedCode.EquationSupport.TokenSupport.Amounts;

#endregion

// username: jeffs
// created:  5/22/2021 5:29:23 PM

namespace SharedCode.EquationSupport.TokenSupport
{
	public class testxxx2
	{
		public void txx(IAmtBase2 iab)
		{
			TokenAmts2 tas2 = new TokenAmts2();

			AmtInteger2 ai2 = new AmtInteger2("1234");
			ai2 = AmtInteger2.Invalid;
			TokenAmt2 ta = new TokenAmt2(ai2);
			

		}
	}



	public class TokenAmts2
	{
	#region private fields

		private TokenAmt2 tokenAmtRoot2;
	#endregion

	#region ctor

		public TokenAmts2()
		{
			tokenAmtRoot2 = new TokenAmt2(null);
		}

	#endregion

	#region public properties

		public TokenAmt2 this[int idx] => tokenAmtRoot2[idx];

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Add(TokenAmt2 ta2)
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
			return "this is| " + nameof(TokenAmt2) + 
				"(" + tokenAmtRoot2.AmountBase.AsString() + ")";
		}

	#endregion
	}
}