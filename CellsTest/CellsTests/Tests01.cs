#region using

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;
using CellsTest.Windows;
using UtilityLibrary;
using static CellsTest.Windows.MainWindow;
using SharedCode.FormulaSupport;
#pragma warning disable CS0105 // The using directive for 'UtilityLibrary' appeared previously in this namespace
using UtilityLibrary;
#pragma warning restore CS0105 // The using directive for 'UtilityLibrary' appeared previously in this namespace

#endregion

// username: jeffs
// created:  3/26/2021 7:24:40 PM

namespace CellsTest.CellsTests
{
	public class Tests01
	{

		private const int MAX_EQ_DEPTH = 20;
		

	#region private fields

		private static MainWindow win;

	#endregion

	#region ctor

		public Tests01(MainWindow win1)
		{
			win = win1;
		}

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

				win.WriteLine("\nrx| start");

				foreach (Match m in mc)
				{
					win.WriteLine("\ngot match| idx| " + m.Index
						+ " name| " + m.Name
						+ " value| " + m.Value);
					win.WriteLine("");

					for (var i = 1; i < m.Groups.Count; i++)
					{
						Group g = m.Groups[i];
						if (g.Index > 0 || !g.Value.IsVoid())
						{
							win.WriteLine("    got group| idx| " + g.Index
								+ " name| " + g.Name
								+ " value | " + g.Value);

							varList.Add(new KeyValuePair<string, string>(g.Name, g.Value));
						}
					}
				}

				win.WriteLine("rx| end\n");

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

				win.WriteLine("\nrx| start");

				foreach (Match m in mc)
				{
					win.WriteLine("\ngot match| idx| " + m.Index
						+ " name| " + m.Name
						+ " value| " + m.Value);
					win.WriteLine("");

					for (var i = 1; i < m.Groups.Count; i++)
					{
						Group g = m.Groups[i];
						if (g.Index > 0 || !g.Value.IsVoid())
						{
							win.WriteLine("    got group| idx| " + g.Index
								+ " name| " + g.Name
								+ " value | " + g.Value);

							varList.Add(new KeyValuePair<string, string>(g.Name, g.Value));
						}
					}
				}

				win.WriteLine("rx| end\n");

				return varList;
			}
		}

// simple split a formula
		internal void splitTest5()
		{
			win.WriteLine("split test 5");
			win.WriteLine("splitting| ");

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

				win.Write("\n");
				win.WriteLine("testing| " + s);

				List<KeyValuePair<string, string>> varList = rx.ParseFormula(s);

				if (varList == null || varList.Count <= 0)
				{
					win.WriteLine("*** MATCH FAILED ***");
					continue;
				}

				foreach (KeyValuePair<string, string> kvp in varList)
				{
					win.WriteLine(" key| " + kvp.Key + " value| " + kvp.Value);
				}

				win.Write("\n");
			}
		}

// simple work with vars
		internal void splitTest6()
		{
			win.WriteLine("split test 6");
			win.WriteLine("splitting| ");

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

					win.WriteLine("adding| " + i
						+ "| code| " + code.PadRight(5)
						+ "| name| " + m.Groups["name"].Value
						);

					varList.Add(new KeyValuePair<string, string>(code, m.Groups["name"].Value));
				}

				win.WriteLine("Test| " + i + " complete\n");
			}

			win.WriteLine("splitest 6| done");
		}

// simple work with vars
		internal void splitTest7()
		{
			win.WriteLine("split test 7");
			win.WriteLine("splitting| ");

			int count = 70;

			string[] test = new string[count];

			test[1] = "{[ 1addd1 ]} = {[a1]}";
			test[2] = " {[a1]} = {[a1]}";
			test[3] = " {[a1]} = \"this 1 is a test\"";
			test[4] = "= {#b1}";
			test[5] = "= {!c1a} ";
			test[6] = "= {$dd}";
			test[7] = "=  {#E1}";
			test[8] = "= {%F1}";
			test[9] = "={@H1}";
			test[10] = "={@cellname: }";
			test[11] = "={@cellname1:label1name}";
			test[12] = "={@chartname1:cellname:labelname}";
			test[13] = "={@a]H1]}";
			test[14] = "{#b1}";
			test[15] = "= {#b1}";
			test[16] = "={#b1}";
			test[17] = "a{#b1}";
			test[18] = "function(test)";
			test[19] = "a(x+b)";
			test[20] = "= 1234 + function(ddd) + asdf";
			test[21] = "= asf + function(afadf) + asf";
			test[22] = "= {asf}";
			test[23] = "12";
			test[24] = "={asf]}";
			test[25] = "abcdef";
			test[26] = "abcdfg";
			test[27] = "{a}def";
			test[28] = "{[1abc]}def";
			test[29] = "{[abc]}=";
			test[30] = "{[abc]}   =";
			test[31] = " = {[abc]}";
			test[32] = " ={[abc]}";
			test[33] = "={@xEz9999} ";
			test[34] = "={[ a1]}";
			test[35] = "={[ a1 ]}}";
			test[36] = "=12 + {[aaa12345]} asdf ";
			test[37] = "={[ aaa12345 ]}";
			test[38] = "={[ A112345 ]}";
			test[39] = " {[abc]} = \"cellname:time\" & {@Bx4} + \"asf\" +{@x3} & \"dfdfd + asf\"  ";
			test[40] = " {[abc]} = \"cell:name : time\" & {@Bx4} + \"asf\" +{@x3} & \"dfdfd + asf\"  ";
			test[41] = " {[abc]} = \"cellname :time\" & {@Bx4} + \"asf\" +{@x3} & \"dfdfd + asf\"  ";
			test[42] = " {[abc]} = {@Bx4]} & \"this time\"+{@Bx4} + \"asf\" +{@x3} \"dfdfd + asf\"  ";
			test[43] = "={@Bx4]} & \"this time\"+{@Bx4} + asf +{@x3} dfdfd + asf";
			test[44] = "={!Bx4]}+\"this time\"+{![Bx4]} + asf +{[x3} dfdfd";
			test[45] = "={%Bx2}+\"this time\"+{%[Bx1]} + asf +{@[x1]} dfdfd";
			test[46] = "= asdf {#[Bx1]}+\"this time\"+{#[Bx1]} + asf +{@[x1]} dfdfd";
			test[47] = "={@[A1]} +\"this time\"+ {@[B1]} +\"this time\"+ {@[C1]}   ";
			test[48] = "={@[A1]} +\"this time\"+ {$[variable]} +\"this time\"+ {@[C1]} + {$[]} dfadsf ";
			test[49] = "= asf asdf asf + {[A1]} +\"this time\"+ {$variable} +\"this time\"+ {[C1]} + {$} dfadsf ";
			test[50] = "= asf asdf asf + {[A1]} +\"this time\"+ {$variable} +\"this time\"+ {[C1]} + {$} dfadsf ";
			test[51] = "={#bx1}+\"this time\"+{#bx2} + asf +{[x1]} dfdfd";
			test[52] = "={%bx4}+\"this time\"+{%bx5} + asf +{[x1]} dfdfd";
			test[53] = "={!Bx4}+\"this time\"+{!Bx4} + asf +{[x3} dfdfd";
			test[54] = "{#cx1}={[Bx4]}+\"this time\"+{[Bx4]} + asf +{[x3} dfdfd + asf";
			test[55] = "{#dx1)}={[Bx4]}";
			test[56] = "={[A1]}";
			test[57] = "={$A1}";
			test[58] = "={#ex1}";
			test[59] = "={%bx6}";
			test[60] = "={!A1}";
			test[61] = "={@A1}";

			string[] pattCodes = new [] {"eq", "op", "dx", "vx", "fx", "sx", "ex"};

			string[] patt = new string[5];

			// extract basic components
			patt[0] = @"(?<eq>=)|(?<op>[+&])|(?<dx>\d+)|(?<vx>{[^{]+})|(?<fx>\p{L}(?>\p{L})*(?=\()\(.+\))|(?<sx>""[^""]+"")|(?<e1>\b\w+\b)";

			// validate an excell address
			patt[1] = @"(?<name>(?i:([a-w]?[a-z]{1,2}|x[a-f][a-d])\d{1,7}))";

			// validate a cell name - replace with pure code
			patt[2] = @"(?<name>[a-zA-Z]\w{1,23})";

			// validate a parameter name
			// use code - starts with a letter, unlimited letters/numbers follow
			Regex r = new Regex(patt[0], RegexOptions.ExplicitCapture);

			for (int i = 0; i < count; i++)
			{
				if (test[i] == null) continue;

				win.WriteLine("testing| " + test[i]);

				MatchCollection c = r.Matches(test[i]);

				if (c.Count < 1)
				{
					win.WriteLine("*** MATCH FAILED ***");
					continue;
				}

				win.WriteLine("found| " + c.Count);

				foreach (Match m in c)
				{
					// win.Write("\n");
					// win.WriteLine("found| Groups| " + m.Groups.Count);

					for (var j = 1; j < m.Groups.Count; j++)
					{
						Group g = m.Groups[j];


						if (g.Success && !string.IsNullOrWhiteSpace(g.Value))
						{
							win.Write("group| idx| " + g.Index.ToString().PadRight(4));
							win.Write(" len| " + g.Length.ToString().PadRight(4));
							win.Write(" code| " + g.Name.PadRight(8));
							win.Write(" value| >" + (g.Value + "<"));
							// win.Write(" success| " + g.Success);
							win.Write("\n");
						}
					}


					// captures do not include the group code
					// // win.Write("\n");
					// // win.WriteLine("found| Captures| " + m.Captures.Count);
					// foreach (Capture cp in m.Captures)
					// {
					// 	win.Write("capture| idx| " + cp.Index.ToString().PadRight(4));
					// 	win.Write(" value| >" + (cp.Value + "<"));
					// 	win.Write("\n");
					// }
				}

				win.WriteLine("Test| " + i + " complete\n");
			}

			win.WriteLine("splitest 7| done");
		}

// split by paren's
		internal void splitTest8()
		{
			win.WriteLine("split test 8");
			win.WriteLine("splitting| ");

			string[] test = new string[15];

			int k = 0;

			test[k++] = "=(1 + (21 + 22 + (31 + 32 * (41 + 42) * (sign(51+52) ) ) ) + 2*3) + 4+5";
			test[k++] = "= 123 (456+(123 + (678 + (123 + 123 + (123 + (123 + 456) + 456)) + 456) + 246))";
			test[k++] = "= 123 (123 + (123 + 123 + (123 + (123 + 456) + 456)) + 456)";
			test[k++] = "= 123 ((123 + 567) + (678 + 891) + (123 + 123 + (123 + (123 + 456) + 456)) + 456) + 246";
			test[k++] = "= 123 ((123 + 567) + 246";
			test[k++] = "= 123 (123 + 567)) + 246";
			test[k++] = "= 123 ((((123 + 567) + 246";
			test[k++] = "= 123 (123 + 567)))) + 246";

			for (int i = 0; i < test.Length; i++)
			{
				if (test[i] == null) break;

				win.Write("\n\n");
				win.WriteLine("testing| " + i + "| length| " + test[i].Length + " >" + test[i] + "<");

				win.WriteLine("0---  0---1--- 10---2--- 20---3--- 30---4--- 40---5--- 50---6--- 60---7--- 70---8--- 80---0--- 90---1");
				win.WriteLine("0123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|");
				win.WriteLine(test[i]);
				win.Write("\n");

				List<List<int[]>> prenArray = startParsePren(test[i]);

				if (prenArray != null)
				{
					if (prenArray[0].Count == 0)
					{
						win.WriteLine("pren pairs| no prens found");
					}
					else
					{
						win.WriteLine("pren pairs| pairs found| " + prenArray.Count);

						foreach (List<int[]> prenList in prenArray)
						{
							if (prenList.Count == 0) break;

							win.Write("\n");
							win.WriteLine("pren pairs| pairs found| " + prenList.Count);

							foreach (int[] prenPair in prenList)
							{
								int len = prenPair[2] - prenPair[1] + 1;

								win.Write("pren pairs| level| " + prenPair[0]);
								win.Write(" start| " + prenPair[1]);
								win.Write(" end| " + prenPair[2]);
								win.Write(" length| " + len);
								win.Write("\n");

								win.Write("pren pairs| string| >");

								if (len != 0)
								{
									win.Write(test[i].Substring(prenPair[1], len));
									win.Write("\n");
								}
								else
								{
									win.WriteLine("** zero length **");
								}
							}
						}
					}
				}
				else 
				{
					win.WriteLine("pren pairs| failed");
				}
			}
		}

		private List<List<int[]>> startParsePren(string eq)
		{
			int level = 0;

			List<List<int[]>> prenArray = new List<List<int[]>>();

			parsePren(0, eq.ToCharArray(), ref level, prenArray);

			if (level != 0)
			{
				return null;
			}

			return prenArray;
		}

		private int parsePren(int beg, char[] eq, ref int level, List<List<int[]>> prenPairs)
		{
			if (level == MAX_EQ_DEPTH+1)
			{
				level = -1;
				return beg;
			};

			int end = beg + 1;

			for (; end < eq.Length; end++)
			{
				if (eq[end] == '(')
				{
					if (prenPairs.Count == level)
					{
						prenPairs.Add(new List<int[]>());
					}

					level += 1;
					beg = end;
					end = parsePren(end, eq, ref level, prenPairs);

					if (level < 0) return beg;

					prenPairs[level].Add(new []{level, beg, end});
				} 
				else if (eq[end] == ')')
				{
					level -= 1;
					return end;
				}
			}

			return end;
		}


// simple test to split a formula
		internal void splitTest1()
		{
			win.WriteLine("split test 1");
			win.WriteLine("splitting| ");

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

				win.Write("\n");
				win.WriteLine("testing| " + i + "| " + test[i]);
				win.Write("\n");

				MatchCollection c = r.Matches(test[i]);
				if (c.Count < 1)
				{
					win.WriteLine("*** MATCH FAILED ***");
					continue;
				}

				win.WriteLine("found| " + c.Count);

				foreach (Match m in c)
				{
					win.Write("\n");
					win.WriteLine("found| Groups| " + m.Groups.Count);

					for (var j = 1; j < m.Groups.Count; j++)
					{
						Group g = m.Groups[j];

						if (g.Success && !string.IsNullOrWhiteSpace(g.Value))
						{
							win.Write("match| idx| " + g.Index.ToString().PadRight(4));
							win.Write(" len| " + g.Length.ToString().PadRight(4));
							win.Write(" name| " + g.Name.PadRight(8));
							win.Write(" value| >" + (g.Value + "<"));
							// win.Write(" success| " + g.Success);
							win.Write("\n");
						}
					}

					// win.Write("\n");
					// win.WriteLine("found| Captures| " + m.Captures.Count);
					// foreach (Capture cp in m.Captures)
					// {
					// 	win.Write("match| idx| " + cp.Index.ToString().PadRight(4));
					// 	win.Write(" value| >" + (cp.Value + "<").PadRight(30));
					// 	win.Write("\n");
					// }
				}
			}
		}

// tests working with key vars
		internal void splitTest2()
		{
			win.WriteLine("split test 2");

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

					win.WriteLine("testing| " + test[i]);

					bool result = pf.GetKeyVars(test[i], out gotLeftSide, out leftSide, out rightSide);

					win.WriteLine("final answer| worked?| " + result );
					win.WriteLine("");

					if (result)
					{
						win.WriteLine("tested| " + test[i] + "| passed "
							+ ">" + rightSide.Key + "< >" + rightSide.Value + "<\n"
							+ "gotLeftSize| " + gotLeftSide + "\n"
							);
					}
					else
					{
						win.WriteLine("testing| " + test[i] + "| failed \n");
					}
				}

				win.WriteLine("done");
			}
		}

// tests working with key vars
		internal void splitTest3()
		{
			win.WriteLine("split test 3");

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

				win.WriteLine("\ntesting| " + test[i]);

				Tuple<int, char, TestType, TestStatusCode> result =
					pf.GetKeyVars3(test[i], out keyVar);

				// win.WriteLine("final answer| worked?| " + result );
				// win.Write("\n");

				if (result.Item1 == 0)
				{
					win.WriteLine("PASSED| " + test[i]);
					win.Write(" index| " + keyVar.Key);
					win.Write(" keyvar| " + keyVar.Value);
					win.Write(" id| " + ProcessFormulaSupport.varIds[keyVar.Key].Id);
					win.Write(" prefix| " + ProcessFormulaSupport.varIds[keyVar.Key].Prefix);
					win.Write("\n");
				}
				else
				{
					win.WriteLine("FAILED| " + test[i]);
					win.Write(" index| " + result.Item1);
					win.Write(" char| " + (result.Item2 == 0 ? "none" : result.Item2.ToString()));
					win.Write(" test type| " + result.Item3);
					win.Write(" stat code| " + result.Item4);
					win.Write("\n");
				}

				win.WriteLine("");
			}

			win.WriteLine("done");
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