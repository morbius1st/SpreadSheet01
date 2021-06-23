#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.ParseSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;
using ValueType = SharedCode.EquationSupport.Definitions.ValueType;

#endregion

// username: jeffs
// created:  5/22/2021 5:04:13 PM

namespace SharedCode.EquationSupport.TokenSupport
{

	public class Token
	{
	#region private fields

		private ParseDataInfo info;

		private AAmtBase aAmtBase = new AmtDefault();

		private AValDefBase aValDef;

	#endregion

	#region ctor

		public Token()
		{
			this.aAmtBase = new AmtDefault();
			// position = 0;
			// length = 0;
		}

		public Token( AValDefBase aValDef1, AAmtBase aAmtBase, ParseDataInfo info)
		{
			this.aAmtBase = aAmtBase;
			// position = pos;
			// length = len;

			this.info = info;
		}

	#endregion

	#region public properties

		public AValDefBase ValDef => aValDef;
		public AAmtBase AmountBase => aAmtBase;
		// public ValueType ValueType => aAmtBase.ValueType;
		// public AValDefBase ValDef => aAmtBase.ValDef;
		// public Token this[int idx] => tokenAmts2[idx];

		public int Position => info.Position;
		public int Length => info.Length;
		public int Level => info.Level;

		public int RefIdx => info.RefIdx;
		public bool IsRefIdx => info.GotRefIdx;

	#endregion

	#region private properties

	#endregion

	#region public methods
		//
		// public Token MakeBranch()
		// {
		// 	Token t = new Token();
		// 	t.level++;
		// 	t.tokenAmts2 = new List<Token>();
		// 	tokenAmts2.Add(t);
		//
		// 	return t;
		// }
		//
		// public void Add(Token t)
		// {
		// 	tokenAmts2.Add(t);
		// }


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
			return "this is| " + nameof(Token) + " (" + aAmtBase.AsString() + ")";
		}

	#endregion
	}
}