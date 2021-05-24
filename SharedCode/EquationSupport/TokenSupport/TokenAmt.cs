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
// created:  5/22/2021 5:04:13 PM

namespace SharedCode.EquationSupport.TokenSupport
{
	public class TokenAmt
	{
	#region private fields

		private int level = 0;
		private IAmtBase amtBase;
		private List<TokenAmt> tokenAmts = null;

	#endregion

	#region ctor

		public TokenAmt(IAmtBase amtBase)
		{
			this.amtBase = amtBase;
		}

	#endregion

	#region public properties

		public IAmtBase AmountBase => amtBase;
		public ValueType DataType => amtBase.DataType;
		public TokenAmt this[int idx] => tokenAmts[idx];
	#endregion

	#region private properties

	#endregion

	#region public methods

		public TokenAmt MakeBranch()
		{
			TokenAmt t = new TokenAmt(null);
			t.level++;
			t.tokenAmts = new List<TokenAmt>();
			tokenAmts.Add(t);

			return t;
		}

		public void Add(TokenAmt t)
		{
			tokenAmts.Add(t);
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
			return "this is| " + nameof(TokenAmt) + "(" + amtBase.AsString() + ")";
		}

	#endregion
	}
}