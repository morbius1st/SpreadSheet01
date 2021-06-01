#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharedCode.DebugAssist;
using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.TokenSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;

#endregion

// username: jeffs
// created:  5/27/2021 6:48:13 PM

namespace SharedCode.EquationSupport.ParseSupport
{
	// take the data from ParsePhaseOne and
	// tokenize
	public class TokenizePhase
	{
	#region private fields

		private List<Token> tokens;
		private ParseGeneralDefinitions pgd;

		private ISendMessages win;
		// private IShowResults show;

		private ShowInfo show = ShowInfo.Inst;

	#endregion

	#region ctor

		public TokenizePhase()
		{
			tokens = new List<Token>();
			// pgd = new ParseGeneralDefinitions();
		}

	#endregion

	#region public properties

		public void Parse2(ParsePhase p1)
		{
			parse2(p1);
		}

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Messenger(ISendMessages win)
		{
			this.win = win;
		}

	#endregion

	#region private methods

		// tested from Test02A / parfseTest03
		private void parse2(ParsePhase p1)
		{
			ParsePhData fc;
			ParseGen pg;
			Token tk;
			AAmtBase aa;
			ADefBase ab;

			win.showTabId = false;

			for (var i = 0; i < p1.FormulaComponents.Count; i++)
			{
				fc = p1.FormulaComponents[i];

				if (fc.Name == null || fc.Value == null) continue;
				//
				// win.WriteLine("");
				//
				// win.WriteLine($"name| {fc.Name,-6} value| {fc.Value} index| {fc.Position} length| {fc.Length}");

				fc.Definition = ParseGeneralDefinitions.Classify(fc.Name, fc.Value);

				Token t = fc.Definition.MakeToken(fc.Value, fc.Position, fc.Length);

				win.TabUp();
				win.WriteLineTab("");
				show.ShowToken(t, false);
				win.TabDn();
			}
		}

		// private Token MakeToken()
		// {
		// 	Token t = new Token()
		// }
		


	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ParsePhaseTwo";
		}

	#endregion
	}
}