#region using
using System.Collections.Generic;
using SharedCode.EquationSupport.TokenSupport.Values;

#endregion

// username: jeffs
// created:  5/18/2021 10:54:34 PM


// using SharedCode.EquationSupport.TokenSupport.ValueAssist;



namespace SharedCode.EquationSupport.TokenSupport
{
	public class TestXX
	{
		public void TX(IValBase ib)
		{
			Tokens ts = new Tokens();
			Token tk = new Token(ValInteger.Default);
			ts[0].MakeBranch();
			ts[0].Add(tk);

		}
	}

	public struct TokenLocation
	{
		public int Depth { get; set; } 
		public int Branch { get; set; }
		public int Position { get; set; }

		public TokenLocation(int depth, int branch, int position)
		{
			// token position            0  1 2                     3  4  5  6  7 8 
			// depth = 0, branch = 0 |  abc + (                   ) + 123 + def + (                   )
			// token index                    0 1 2 3         4 5 6               0 1 2 3         4 5 6 
			//                                v                   ^               v                   ^
			// depth = 1, branch = 2|         ( a + (       ) + b )   branch = 8| ( 9 + (       ) + 8 )
			//                                      v       ^                           v       ^
			// depth = 2, branch = 3|               ( 1 + 2 )         branch = 3|       ( 3 + 4 )

			Depth = depth;			// how far down the parenthesis tree
			Branch = branch;		// which branch along a depth
			Position = position;	// which position in the branch
		}
	}


	public class Tokens
	{
	#region private fields

		private Token tokenRoot;
		

	#endregion

	#region ctor

		public Tokens()
		{
			tokenRoot = new Token(null);
		}

	#endregion

	#region public properties


		public Token this[int idx] => tokenRoot[idx];

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
			return "this is Token";
		}

	#endregion
	}
}