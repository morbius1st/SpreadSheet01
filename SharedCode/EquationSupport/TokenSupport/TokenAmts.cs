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
	public class testxxx
	{
		public void txx(IAmtBase iab)
		{
			TokenAmts tas = new TokenAmts();
			AmtInt ai = AmtInt.Default;
			TokenAmt ta = new TokenAmt(ai);

			AmtInteger2 ai2 = new AmtInteger2("1234");
			ai2 = AmtInteger2.Invalid;

		}
	}



	public class TokenAmts
	{
	#region private fields

		private TokenAmt tokenAmtRoot;
	#endregion

	#region ctor

		public TokenAmts()
		{
			tokenAmtRoot = new TokenAmt(null);
		}

	#endregion

	#region public properties

		public TokenAmt this[int idx] => tokenAmtRoot[idx];

	#endregion

	#region private properties

	#endregion

	#region public methods

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
			return "this is| " + nameof(TokenAmt) + 
				"(" + tokenAmtRoot.AmountBase.AsString() + ")";
		}

	#endregion
	}
}