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

	public class TokenAmt2
	{
	#region private fields

		private int level = 0;
		private IAmtBase2 amtBase2;
		private List<TokenAmt2> tokenAmts2 = null;

	#endregion

	#region ctor

		public TokenAmt2(IAmtBase2 amtBase)
		{
			this.amtBase2 = amtBase;
		}

	#endregion

	#region public properties

		public IAmtBase2 AmountBase => amtBase2;
		public ValueType DataType => amtBase2.DataType;
		public TokenAmt2 this[int idx] => tokenAmts2[idx];
	#endregion

	#region private properties

	#endregion

	#region public methods

		public TokenAmt2 MakeBranch()
		{
			TokenAmt2 t = new TokenAmt2(null);
			t.level++;
			t.tokenAmts2 = new List<TokenAmt2>();
			tokenAmts2.Add(t);

			return t;
		}

		public void Add(TokenAmt2 t)
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
			return "this is| " + nameof(TokenAmt2) + "(" + amtBase2.AsString() + ")";
		}

	#endregion
	}
}