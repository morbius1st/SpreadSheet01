#region using
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UtilityLibrary;

using static CellsTest.Windows.MainWindow;


using SharedCode.FormulaSupport;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  3/26/2021 7:24:40 PM

namespace CellsTest.CellsTests
{
	public class Tests01
	{
		#region private fields



		#endregion

		#region ctor

		public Tests01() { }

		#endregion

		#region public properties



		#endregion

		#region private properties



		#endregion

		#region public methods
		
		internal static class rxx
		{
			static string patt = @"\s*(?<E>=?)(?<eq1>.*?)\{(?:(?<keyvar>[\$\#\%\!\@].*?)|(?<keyvar>\[.*?\]))\}|\s*(?<E>=?)(?<eq2>.+$)";

			static Regex r = new Regex(patt, RegexOptions.ExplicitCapture);

			public static MatchCollection MatchFormulaForCellVar(string formula)
			{
				return r.Matches(formula);
			}

			public static List<KeyValuePair<string, string>> ParseFormula(string formula)
			{
				MatchCollection mc = MatchFormulaForCellVar(formula);

				List<KeyValuePair<string, string>> varList = new List<KeyValuePair<string, string>>();

				WriteLine("\nrx| start");

				foreach (Match m in mc)
				{
					WriteLine("\ngot match| idx| " + m.Index
						+ " name| " + m.Name
						+ " value| " + m.Value);
					WriteLine("");

					for (var i = 1; i < m.Groups.Count; i++)
					{
						Group g = m.Groups[i];
						if (g.Index > 0 || !g.Value.IsVoid())
						{
							WriteLine("    got group| idx| " + g.Index
								+ " name| " + g.Name
								+ " value | " + g.Value);

							varList.Add(new KeyValuePair<string, string>(g.Name, g.Value));
						}
					}
				}

				WriteLine("rx| end\n");

				return varList;
			}
		}


		internal static class rx
		{
			static string patt =
				@"(?<E>=)|(?<eq>.*?)(?<v>\{(?:\[(?<ca>(?i:[a-z]{1,3}[1-9]\d{0,6}))\]|\$(?<sv>.+?)|\@(?<ln>.+?)|\!(?<gp>.+?)|\%(?<pp>.+?)|\#(?<rp>.+?))\})|(?<eq>.+$)";

			static Regex r = new Regex(patt, RegexOptions.ExplicitCapture);

			public static MatchCollection MatchFormulaForCellVar(string formula)
			{
				return r.Matches(formula);
			}

			public static List<KeyValuePair<string, string>> ParseFormula(string formula)
			{
				MatchCollection mc = MatchFormulaForCellVar(formula);

				List<KeyValuePair<string, string>> varList = new List<KeyValuePair<string, string>>();

				WriteLine("\nrx| start");

				foreach (Match m in mc)
				{
					WriteLine("\ngot match| idx| " + m.Index
						+ " name| " + m.Name
						+ " value| " + m.Value);
					WriteLine("");

					for (var i = 1; i < m.Groups.Count; i++)
					{
						Group g = m.Groups[i];
						if (g.Index > 0 || !g.Value.IsVoid())
						{
							WriteLine("    got group| idx| " + g.Index
								+ " name| " + g.Name
								+ " value | " + g.Value);

							varList.Add(new KeyValuePair<string, string>(g.Name, g.Value));
						}
					}
				}

				WriteLine("rx| end\n");

				return varList;
			}
		}

		internal void splitTest5()
		{
			WriteLine("split test 5");
			WriteLine("splitting| ");

			string[] test = new string[15];

			test[0] = "= asf asdf asf + {[A1]} +\"this time\"+ {$variable} +\"this time\"+ {[C1]} + {$} dfadsf ";
			test[1] = "={#bx1}+\"this time\"+{#bx2} + asf +{[x1]} dfdfd";
			test[2] = "={%bx4}+\"this time\"+{%bx5} + asf +{[x1]} dfdfd";
			test[3] = "={!Bx4}+\"this time\"+{!Bx4} + asf +{[x3} dfdfd";
			test[4] = "{#cx1}={[Bx4]}+\"this time\"+{[Bx4]} + asf +{[x3} dfdfd + asf";
			test[5] = "{#dx1)}={[Bx4]}";
			test[6] = "={[A1]}";
			test[7] = "={$A1}";
			test[8] = "={#ex1}";
			test[9] = "={%bx6}";
			test[10] = "={!A1}";
			test[11] = "={@A1}";

			foreach (string s in test)
			{
				if (s.IsVoid()) continue;

				Write("\n");
				WriteLine("testing| " + s);

				List<KeyValuePair<string, string>> varList = rx.ParseFormula(s);

				if (varList == null || varList.Count <= 0)
				{
					WriteLine("*** MATCH FAILED ***");
					continue;
				}

				foreach (KeyValuePair<string, string> kvp in varList)
				{
					WriteLine(" key| " + kvp.Key + " value| " + kvp.Value);
				}

				Write("\n");
			}
		}


		internal void splitTest6()
		{
			WriteLine("split test 6");
			WriteLine("splitting| ");

			int count = 15;

			string[] test = new string[count];

			test[0] = "= asf asdf asf + {[A1]} +\"this time\"+ {$variable} +\"this time\"+ {[C1]} + {$} dfadsf ";
			test[1] = "={#bx1}+\"this time\"+{#bx2} + asf +{[x1]} dfdfd";
			test[2] = "={%bx4}+\"this time\"+{%bx5} + asf +{[x1]} dfdfd";
			test[3] = "={!Bx4}+\"this time\"+{!Bx4} + asf +{[x3} dfdfd";
			test[4] = "{#cx1}={[Bx4]}+\"this time\"+{[Bx4]} + asf +{[x3} dfdfd + asf";
			test[5] = "{#dx1)}={[Bx4]}";
			test[6] = "={[A1]}";
			test[7] = "={$A1}";
			test[8] = "={#ex1}";
			test[9] = "={%bx6}";
			test[10] = "={!A1}";
			test[11] = "={@A1}";

			string[] patt = new string[5];

			// extract variable strings + type code
			patt[0] = @"(?<E>=)|(?<eq>.*?)(?<v>\{((?<code>[\$\#\%\!\@])(?<name>.*?)|(?<name>(?<code>\[).*?\]))\})|(?<eq>.+$)";

			// validate an excell address
			patt[1] = @"(?<name>(?i:([a-w]?[a-z]{1,2}|x[a-f][a-d])\d{1,7}))";

			// validate a cell name - replace with pure code
			patt[2] = @"(?<name>[a-zA-Z]\w{1,23})";

			// validate a parameter name
			// use code - starts with a letter, unlimited letters/numbers follow

			for (int i = 0; i < count; i++)
			{
				if (test[i] == null) continue;

				Regex r = new Regex(patt[0], RegexOptions.ExplicitCapture);

				MatchCollection mc = r.Matches(test[i]);

				List<KeyValuePair<string, string>> varList = new List<KeyValuePair<string, string>>();

				foreach (Match m in mc)
				{
					string code = m.Groups["code"].Value;
					if (string.IsNullOrWhiteSpace(code)) continue;

					Debug.WriteLine("adding| " + i
						+ "| code| " + code.PadRight(5)
						+ "| name| " + m.Groups["name"].Value
						);

					varList.Add(new KeyValuePair<string, string>(code, m.Groups["name"].Value));
				}

				Debug.WriteLine("Test| " + i + " complete\n");
			}

			Debug.WriteLine("splitest 6| done");
		}


		internal void splitTest()
		{
			WriteLine("split test");
			WriteLine("splitting| ");

			string[] test = new string[15];

			test[0] = "= asf asdf asf + {[A1]} +\"this time\"+ {$variable} +\"this time\"+ {[C1]} + {$} dfadsf ";
			test[1] = "={#Bx1}+\"this time\"+{#Bx1} + asf +{[x1]} dfdfd";
			test[2] = "={%Bx2}+\"this time\"+{%Bx1} + asf +{[x1]} dfdfd";
			test[3] = "={!Bx4}+\"this time\"+{!Bx4} + asf +{[x3} dfdfd";
			test[4] = "{#Bx1}={@Bx4}+\"this time\"+{@Bx4} + asf +{[x3} dfdfd + asf";
			test[6] = "={[A1]}";
			test[7] = "={$A1}";
			test[8] = "={#ex1}";
			test[9] = "={%bx6}";
			test[10] = "={!A1}";
			test[11] = "={@A1}";

			string patt = @"\s*(?<E>=?)(?<eq>.*?)\{(?:(?<keyvar>[\$\#\%\!\@].+?)|(?<keyvar>\[.+?\]))\}|\s*(?<E>=?)(?<eq>.+$)";

			Regex r = new Regex(patt, RegexOptions.Compiled);

			for (var i = 0; i < test.Length; i++)
			{
				if (test[i] == null) break;

				Write("\n");
				WriteLine("testing| " + i + "| " + test[i]);
				Write("\n");

				MatchCollection c = r.Matches(test[i]);
				if (c.Count < 1)
				{
					WriteLine("*** MATCH FAILED ***");
					continue;
				}

				WriteLine("found| " + c.Count);

				foreach (Match m in c)
				{
					Write("\n");
					WriteLine("found| Groups| " + m.Groups.Count);

					for (var j = 1; j < m.Groups.Count; j++)
					{
						Group g = m.Groups[j];

						if (g.Success && !string.IsNullOrWhiteSpace(g.Value))
						{
							Write("match| idx| " + g.Index.ToString().PadRight(4));
							Write(" len| " + g.Length.ToString().PadRight(4));
							Write(" name| " + g.Name.PadRight(8));
							Write(" value| >" + (g.Value + "<"));
							// Write(" success| " + g.Success);
							Write("\n");
						}
					}

					// Write("\n");
					// WriteLine("found| Captures| " + m.Captures.Count);
					// foreach (Capture cp in m.Captures)
					// {
					// 	Write("match| idx| " + cp.Index.ToString().PadRight(4));
					// 	Write(" value| >" + (cp.Value + "<").PadRight(30));
					// 	Write("\n");
					// }
				}
			}
		}

		
		internal void tests2()
		{
			splitTest();
			// for (int j = 0; j < 5; j++)

			// }
		}


		internal void tests()
		{
			for (int j = 0; j < 10; j++)
			{
				ProcessFormula pf = new ProcessFormula();

				string[] test = new string[15];

				// test[0] = "={[A1]}";
				// test[1] = "={$A1}";
				// test[2] = "={#ex1}";
				// test[3] = "={%bx6}";
				// test[4] = "={!A1}";
				// test[5] = "={@A1}";

				test[6] = "asdf={[A1]}";
				test[7] = "{$A1}={$A1}";
				test[8] = "{#ex1}";
				test[9] = "={%bx6} + {$A1}";
				test[10] = "{!A1} + {$A1}";
				test[11] = "={@A[1}";

				ValuePair<string, string> leftSide;
				ValuePair<string, string> rightSide;

				bool gotLeftSide;


				for (var i = 0; i < test.Length; i++)
				{
					if (test[i] == null) continue;

					Debug.WriteLine("testing| " + test[i]);

					bool result = pf.GetKeyVars(test[i], out gotLeftSide, out leftSide, out rightSide);

					Debug.WriteLine("final answer| worked?| " + result );
					Debug.WriteLine("");

					if (result)
					{
						Debug.WriteLine("tested| " + test[i] + "| passed "
							+ ">" + rightSide.Key + "< >" + rightSide.Value + "<\n"
							+ "gotLeftSize| " + gotLeftSide + "\n"
							);
					}
					else
					{
						Debug.WriteLine("testing| " + test[i] + "| failed \n");
					}
				}

				Debug.WriteLine("done");
			}
		}

		internal void test3()
		{
			// for (int j = 0; j < 5; j++)
			// {
			ProcessFormula pf = new ProcessFormula();

			string[] test = new string[15];

			test[0] = "={[A1]]}";
			test[1] = "={[A1]}";
			test[3] = "={$A1}";
			test[4] = "={#ex1}";
			test[5] = "={%bx6}";
			test[6] = "={!A1}";
			test[7] = "={@A1}";

			test[9] = "asdf={[A1]}";
			test[10] = "{$A1}={$A1}";
			test[11] = "{#ex1}";
			test[12] = "={%bx6} + {$A1}";
			test[13] = "{!A1} + {$A1}";
			test[14] = "={@A[1}";

			ValuePair<int, string> keyVar;

			for (var i = 0; i < test.Length; i++)
			{
				if (test[i] == null) continue;

				Debug.WriteLine("\ntesting| " + test[i]);

				Tuple<int, char, TestType, TestStatusCode> result =
					pf.GetKeyVars3(test[i], out keyVar);

				// Debug.WriteLine("final answer| worked?| " + result );
				// Debug.Write("\n");

				if (result.Item1 == 0)
				{
					Debug.WriteLine("PASSED| " + test[i]);
					Debug.Write(" index| " + keyVar.Key);
					Debug.Write(" keyvar| " + keyVar.Value);
					Debug.Write(" id| " + ProcessFormulaSupport.varIds[keyVar.Key].Id);
					Debug.Write(" prefix| " + ProcessFormulaSupport.varIds[keyVar.Key].Prefix);
					Debug.Write("\n");
				}
				else
				{
					Debug.WriteLine("FAILED| " + test[i]);
					Debug.Write(" index| " + result.Item1);
					Debug.Write(" char| " + (result.Item2 == 0 ? "none" : result.Item2.ToString()));
					Debug.Write(" test type| " + result.Item3);
					Debug.Write(" stat code| " + result.Item4);
					Debug.Write("\n");
				}

				Debug.WriteLine("");
			}

			Debug.WriteLine("done");
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
			return "this is Tests01";
		}

		#endregion
	}
}
