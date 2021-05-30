#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.TokenSupport.Amounts;

#endregion

// username: jeffs
// created:  5/29/2021 9:06:45 AM

namespace SharedCode.EquationSupport.TokenSupport
{
	public class TokenAssist
	{
	#region private fields

	#endregion

	#region ctor

		public TokenAssist() { }

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public Token MakeToken(ParseGen pg, string value, int pos, int len)
		{
			AAmtBase ab;

			switch (pg.Group)
			{
			case ParseGroupGeneral.PGG_ASSIGNMENT:
				{
					ab = new AmtAssignment(value);

					return new Token(ab, pos, len);
				}
			case ParseGroupGeneral.PGG_OPERATOR:
				{

					break;
				}
			}

			return null;
		}

	#endregion

	#region private methods

		private Token makeTokenOp(ParseGen pg, string value, int pos, int len)
		{
			DefValue vd = (DefValue) pg.Classify(value);

			

			// AAmtBase ab = new AmtOpAdd()

			return null;
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is TokenAssist";
		}

	#endregion
	}
}