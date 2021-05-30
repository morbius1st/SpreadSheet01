#region + Using Directives
using System;


using CellsTest.Windows;
using SharedCode.DebugAssist;
using SharedCode.EquationSupport.TokenSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;
using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.ParseSupport;
using static SharedCode.EquationSupport.Definitions.ValueType;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;
#endregion

// user name: jeffs
// created:   5/30/2021 6:50:44 AM

namespace CellsTest.CellsTests
{
	public class Show01
	{
		private static MainWindow win;
		private ParseGeneralDefinitions pgDefs = ParseGeneralDefinitions.PgDefInst;
		private VariableDefinitions varDefs = VariableDefinitions.VarDefInst;
		private ValueDefinitions valDefs = ValueDefinitions.ValDefInst;

		private ShowResults show = ShowResults.Inst;

		public Show01(MainWindow win1)
		{
			win = win1;
		}

		public void ShowParseGens()
		{
			win.WriteLineTab("");
			win.WriteLineTab("*** Parse General ***");

			for (int i = 0; i < pgDefs.Count; i++)
			{
				win.WriteLine("");

				show.ShowParseGen(pgDefs[i]);
			}
		}
		
		public void ShowDefVals()
		{
			win.WriteLineTab("");
			win.WriteLineTab("*** Value Definitions ***");

			for (int i = 0; i < valDefs.Count; i++)
			{
				win.WriteLine("");

				show.ShowParsVarDefs2D(valDefs[i]);
			}
		}
		
		public void ShowDefVars()
		{
			win.WriteLineTab("");
			win.WriteLineTab("*** Special Variable Definitions ***");

			for (int i = 0; i < varDefs.Count; i++)
			{
				win.WriteLine("");

				show.ShowParsVarDefs2D(varDefs[i]);
			}
		}




	}
}
