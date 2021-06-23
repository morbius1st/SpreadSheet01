#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using CellsTest.Windows;
using SharedCode.DebugAssist;
using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.ParseSupport;

using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

#endregion

// username: jeffs
// created:  5/17/2021 2:47:36 PM

/*
The plan with these tests is to 
1. parse an equation into the basic parts
2. validate the equation
3. adjust the parts to create a fully urinary or binary configuration
4. categorize & tokenize the parts
*/

namespace CellsTest.CellsTests
{
	public class Tests02A : INotifyPropertyChanged
	{
	#region private fields

		private static MainWindow win;

		private ValueDefinitions valDefs = ValueDefinitions.ValDefInst;
		private ParseDefinitions pgDefs = ParseDefinitions.PgDefInst;
		// private VariableDefinitions varDefs = VariableDefinitions.VarDefInst;

		private ShowInfo show = ShowInfo.Inst;

	#endregion

	#region ctor

		public Tests02A(MainWindow win1)
		{
			win = win1;
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void parseTest01()
		{
			ValueDefinitions opValues = ValueDefinitions.ValDefInst;

			win.WriteLine("parse test 01");

			int numLoops = 1000;
			int maxTestStrings = 10;
			int numTestStrings = 1;

			string[] tests = new string[maxTestStrings];

			tests[numTestStrings++] = "=";
			tests[numTestStrings++] = "<and>";
			tests[numTestStrings++] =  "&";
			tests[numTestStrings++] =  ")";

			FindTokens(0, opValues, tests, numLoops, numTestStrings);
			FindTokens(1, opValues, tests, numLoops, numTestStrings);
		}

		public void ShowParseGens()
		{
			win.WriteLineTab("");
			win.WriteLineTab("*** Parse General ***");

			for (int k = 0; k < 2; k++)
			{
				if (k==0) show.ShowParsGenHeader();
				for (int i = 0; i < pgDefs.Count; i++)
				{
					if (k==1) show.ShowParsGenHeader();
					show.ShowParseGen(pgDefs[i]);

					if (pgDefs[i].ValDefs == null) continue;

					if (k == 1)
					{

						if (pgDefs[i].ValDefs.Count > 0)
						{
							win.Write("\n");
							show.ShowParsValDefsHeader();

							for (int j = 0; j < pgDefs[i].ValDefs.Count; j++)
							{
								show.ShowParsVarDef( pgDefs[i].ValDefs[j]);
							}
							win.WriteLine("");

						}
					}

				}

				win.WriteLine("\n");
			}
		}

		// public void ShowValDefs2()
		// {
		// 	win.WriteLineTab("");
		// 	win.WriteLineTab("*** Value Definitions ***");
		//
		// 	for (int i = 0; i < valDefs.Count; i++)
		// 	{
		// 		// win.WriteLine("");
		//
		// 		show.ShowValDefs2D(valDefs[i]);
		// 	}
		// }

		public void ShowValDefs()
		{
			win.WriteLineTab("");
			win.WriteLineTab("*** Value Definitions ***");

			int gotValDef = 0;

			int[] orderValDef;
			int[] orderVarDef;

			List<Tuple<string[], int, int, bool>> valDefInfoList =
				show.ValDefInfoList(out orderValDef);

			List<Tuple<string[], int, int, bool>> varDefInfoList =
				show.VarDefInfoList(out orderVarDef);

			orderValDef = new [] {1, 6, 0, 3, 4, 8, 9, 10};
			orderVarDef = new [] {1, 2, 7, 0, 4, 5, 9, 10, 11};

			for (int i = 0; i < valDefs.Count; i++)
			{
				if (valDefs[i] is AVarDef)
				{
					if (gotValDef != -1)
					{
						gotValDef = -1;

						win.WriteLine("");
						show.ShowHeader(varDefInfoList, orderVarDef);
					}

					show.ShowVarDef((AVarDef) valDefs[i], varDefInfoList, orderVarDef);
				} 
				else 
				{
					if (gotValDef != 1)
					{
						gotValDef = 1;

						win.WriteLine("");
						show.ShowHeader(valDefInfoList, orderValDef);
					}

					show.ShowValDef(valDefs[i], valDefInfoList, orderValDef);
				}
			}
		}


		// public void ShowVarDefs()
		// {
		// 	win.WriteLineTab("");
		// 	win.WriteLineTab("*** Special Variable Definitions ***");
		//
		// 	for (int i = 0; i < varDefs.Count; i++)
		// 	{
		// 		win.WriteLine("");
		//
		// 		show.ShowValDefs2D(varDefs[i]);
		// 	}
		// }

	#endregion

	#region private methods

		private void FindTokens(int which, ValueDefinitions opValues,
			string[] tests, int numLoops, int numTestStrings)
		{
			int countLoops;
			int countTests = 0;
			int countFails = 0;

			AValDefBase vdIdentifier;

			Stopwatch s = new Stopwatch();
			s.Start();

			for (countLoops = 0; countLoops < numLoops; countLoops++)
			{
				for (int t = 0; t < numTestStrings; t++)
				{
					if (tests[t] == null) continue;

					countTests++;

					// if (which == 0)
					// {
					if (countTests == 1)
					{
						win.WriteLine("\ntest         | test 0");
					}

					vdIdentifier = GetToken0(tests[t]);
					// }
					// else
					// {
					// 	if (countTests == 1)
					// 	{
					// 		win.WriteLine("\ntest         | test 1");
					// 	}
					// 	opToken = GetToken1(opTokens, tests[t]);
					// }

					if (vdIdentifier == null ||
						!vdIdentifier.ValueStr.Equals(tests[t]))
					{
						countFails++;
					}
				}
			}

			s.Stop();

			double secs = s.Elapsed.TotalSeconds;

			win.WriteLine("*** loop count| " + countLoops + "  total tests| " + countTests);

			win.WriteLine("time for test| " + secs.ToString("N6") + " seconds");
			win.WriteLine("time per test| " + (secs / countTests).ToString("N6") + " seconds");
			win.WriteLine($"** fails **  | {countFails}");
			win.WriteLine("\n");
		}

		private AValDefBase GetToken0(string test)
		{
			return (AValDefBase) ValueDefinitions.Classify(test);
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is Test02A";
		}

	#endregion



		// // parse formula and create prenthses hierarchy at the same time
		// internal void parseTest04()
		// {
		// 	win.WriteLine("parse test 02A-04");
		//
		// 	ParsePhase p1 = new ParsePhase();
		//
		// 	string[] test = new string[15];
		//
		// 	int k = 0;
		//
		// 	test[k++] = "= 123 + (456+(123 + 246) + (abc + def) + xyz) + 321";
		//
		// 	// test[k++] = "=(1 + (21 + 22 + (31 + 32 * (41 + 42) * (sign(51+52) ) ) ) + 2*3) + 4+5 + {[A1]} + ({!B1}) & \"text\"";
		// 	// test[k++] = "= 123 (456+(123 + (678 + (123 + 123 + (123 + (123 + 456) + 456)) + 456) + 246) + variable)";
		//
		// 	for (int i = 0; i < test.Length; i++)
		// 	{
		// 		if (test[i] == null) break;
		//
		// 		bool result = p1.Parse2(test[i]);
		//
		// 		if (result)
		// 		{
		// 			win.showTabId = false;
		// 			win.WriteLine("\ntesting| " + i + "| length| " + test[i].Length);
		// 			win.WriteLine("");
		// 			ListPattern(test[i]);
		// 			win.WriteLine("");
		// 			ListMatches04(p1.FormulaComponents);
		// 		}
		// 	}
		// }



		// parse line show matches
		// internal void parseTest03()
		// {
		// 	win.WriteLine("parse test 02A-02");
		//
		// 	ParsePhase p1 = new ParsePhase();
		// 	TokenizePhase p2 = new TokenizePhase();
		//
		// 	p2.Messenger(win);
		//
		// 	string[] test = new string[15];
		//
		// 	int k = 0;
		//
		// 	test[k++] = "= 123 + (456+(123 + 246))";
		//
		// 	// test[k++] = "=(1 + (21 + 22 + (31 + 32 * (41 + 42) * (sign(51+52) ) ) ) + 2*3) + 4+5 + {[A1]} + ({!B1}) & \"text\"";
		// 	// test[k++] = "= 123 (456+(123 + (678 + (123 + 123 + (123 + (123 + 456) + 456)) + 456) + 246) + variable)";
		//
		// 	for (int i = 0; i < test.Length; i++)
		// 	{
		// 		if (test[i] == null) break;
		//
		// 		bool result = p1.Parse(test[i]);
		//
		// 		if (result)
		// 		{
		// 			win.WriteLine("\ntesting| " + i + "| length| " + test[i].Length);
		// 			ListPattern(test[i]);
		//
		// 			p2.Parse2(p1);
		// 		}
		// 	}
		// }

		// parse line show matches

		
		// internal void parseTest02()
		// {
		// 	win.WriteLine("parse test 01");
		//
		// 	ParsePhase pi = new ParsePhase();
		//
		// 	string[] test = new string[15];
		//
		// 	int k = 0;
		//
		// 	test[k++] = "=(1 + (21 + 22 + (31 + 32 * (41 + 42) * (sign(51+52) ) ) ) + 2*3) + 4+5 + {[A1]} + ({!B1}) & \"text\"";
		// 	test[k++] = "= 123 (456+(123 + (678 + (123 + 123 + (123 + (123 + 456) + 456)) + 456) + 246))";
		//
		// 	for (int i = 0; i < test.Length; i++)
		// 	{
		// 		if (test[i] == null) break;
		//
		// 		bool result = pi.Parse(test[i]);
		//
		// 		if (result)
		// 		{
		// 			win.WriteLine("\ntesting| " + i + "| length| " + test[i].Length);
		// 			ListPattern(test[i]);
		// 			ListMatches(pi);
		// 		}
		// 	}
		// }

		// test classifying equation component items
		// but this is a timed test for a set number of loops

		// private void ListMatches(ParsePhase pi)
		// {
		// 	for (var i = 0; i < pi.FormulaComponents.Count; i++)
		// 	{
		// 		ParsePhData pd1 = pi.FormulaComponents[i];
		// 		win.WriteLine($"name|\t{pd1.Name,-6}\tvalue|\t{pd1.Value}\tindex|\t{pd1.Position}\tlength|\t{pd1.Length}");
		// 	}
		// }

		// this method was lots slower (something like 4000 versus 700)
		// private TokenParse GetToken1(OperationTokenDefinitions opTokens, string test)
		// {
		// 	return opTokens.Find(test);
		// }


		// private void ListMatches04(List<ParsePhData> phData)
		// {
		// 	int i;
		//
		// 	for (i = 0; i < phData.Count; i++)
		// 	{
		// 		win.Write(phData[i].Value);
		//
		// 		if (phData[i].Value.Equals(vds.Definitions[Vd_PrnBeg].ValueStr))
		// 		{
		// 			win.TabUp(1);
		// 			win.WriteLine("");
		// 			win.WriteTab("");
		// 			ListMatches04(phData[i].children);
		// 			win.TabDn(1);
		// 			win.WriteLine("");
		// 			win.WriteTab("");
		// 		} 
		// 		else if (phData[i].Value.Equals(vds.Definitions[Vd_PrnEnd].ValueStr))
		// 		{
		// 			return;
		// 		}
		// 	}
		// }
		//
		// private void ListPattern(string pattern)
		// {
		// 	win.Write("");
		// 	win.WriteLine("0---  0---1--- 10---2--- 20---3--- 30---4--- 40---5--- 50---6--- 60---7--- 70---8--- 80---0--- 90---1");
		// 	win.WriteLine("0123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|");
		// 	win.WriteLine(pattern);
		// 	win.Write("\n");
		// }

	}
}