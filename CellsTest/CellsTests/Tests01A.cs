#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CellsTest.Windows;
using Microsoft.Office.Interop.Excel;
using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.ParseSupport;
using UtilityLibrary;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;
using static SharedCode.EquationSupport.Definitions.ParseDefinitions;
using ValueType = SharedCode.EquationSupport.Definitions.ValueType;

#endregion

// username: jeffs
// created:  5/3/2021 10:08:22 PM

namespace CellsTest.CellsTests
{
	public class Tests01A
	{
		private const int MAX_EQ_DEPTH = 20;

		private struct Eq1
		{
			public int Level { get; set; }     // Level
			public string EqCode { get; set; } // EqCode
			public string Value { get; set; }  // Value
			public int SortCode { get; set; }

			public Eq1(int level, string eqCode, string value, int sortCode = 0)
			{
				Level = level;
				EqCode = eqCode;
				Value = value;
				SortCode = sortCode;
			}

			public override string ToString()
			{
				return $"lev| {Level, -3}  code| {EqCode, -10}  val| {Value}";
			}
		}

	#region private fields

		private static MainWindow win;

		private static ParseDefinitions pgd = ParseDefinitions.PgDefInst;
		
		ParsePhase parse = new ParsePhase();

		private string grpRefName;
		private string grpBegName;
		private string grpEndName;

	#endregion

	#region ctor

		public Tests01A(MainWindow win1)
		{
			win = win1;

			grpRefName = pgd.Definitions[Pgd_GrpRef].ValueStr; // grpref
			grpBegName = pgd.Definitions[Pgd_GrpBeg].ValueStr; // grpbeg
			grpEndName = pgd.Definitions[Pgd_GrpEnd].ValueStr; // grpend
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void splitTest10()
		{
			win.WriteLine("split test 10");
			win.WriteLine("splitting| ");

			string[] patt = new string[10];

			int numLoops = 1;
			int numstrings = 500;

			int countLoops;
			int countTests;
			int numPatterns = 0;
			int groupCountTotalLoops;


			// test 0  about 0.735 sec for 1000 loops
			// pattern difference:                   																																											    ** v
			patt[numPatterns++] =
				@"(?<l2>(?>(?<=^[\t ]*|[-+'""*=()&\/][\t ]*)[-+]?)(?>(?>(?>(?>\d+ )?(?>\d+\/\d+['""])))|(?>(?>\d+'-(?>(?>\d+)?(?> \d+\/\d+""|\d+(?>\.\d*""|"")))))|(?>\d+(?>\.\d*""|""))|(?>(?>\d+\.\d*['""]))|(?>(?>\d+['""]))))|(?<d1>(?>(?>(?<=^[\t ]*|[-+'""*=()&\/][\t ]*)[-+]?)\d+\.\d*))|(?<n1>(?>(?<=^[\t ]*|[-+'""*=()&\/][\t ]*)[-+]?)(?>\d+(?!\.)))|(?<f1>[a-zA-Z]\w*(?=\())|(?<s1>\"".+?\"")|(?<op1>\<[oO][rR]\>|\<[aA][nN][dD]\>|\+|\-|&|<=|>=|<|>|==|!=|\*|\/)|(?<eq>=)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<x1>[^ ])";

			// test 1 about 0.738 sec for 1000 loops
			// pattern difference:                       																																											** v
			patt[numPatterns++] =
				@"(?<l1>(?>(?<=^[\t ]*|[-+'""*=()&\/][\t ]*)[-+]?)(?>(?>(?>(?>\d+ )?(?>\d+\/\d+['""])))|(?>(?>\d+'-(?>(?>\d+)?(?> \d+\/\d+""|\d+(?>\.\d*""|"")))))|(?>\d+(?>\.\d*""|""))|(?>(?>\d+\.\d*['""]))|(?>(?>\d+['""]))))|(?<f1>[a-zA-Z]\w*(?=\())|(?<s1>\"".+?\"")|(?<d1>(?>(?>(?<=^[\t ]*|[-+'""*=()&\/][\t ]*)[-+]?)\d+\.\d*))|(?<n1>(?>(?<=^[\t ]*|[-+'""*=()&\/][\t ]*)[-+]?)(?>\d+(?!\.)))|(?<op1>\<[oO][rR]\>|\<[aA][nN][dD]\>|\+|\-|&|<=|>=|<|>|==|!=|\*|\/)|(?<eq>=)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<x1>[^ ])";

			// test 2  about 0.643 sec for 1000 loops
			// pattern difference: always associates the + or - against a digit
			// as a part of the digit even though an operator is then missing
			// final "approved" pattern
			patt[numPatterns++] =
				@"(?<l1>[-+]?(?>\d+'-(?>\d*\.\d+|(?>\d+ )?\d+\/\d+|\d+)""|(?>\d+ \d+\/\d+|\d+\/\d+|\d*\.\d+|\d+)[""']))|(?<fr1>[-+]?(?>\d+ \d+\/\d+|\d+\/\d+))|(?<d1>[-+]?(?>\d+\.\d*|\d*\.\d+))|(?<n1>[-+]?\d+(?![.\/]))|(?<fn1>[a-zA-Z]\w*(?=\())|(?<s1>\"".+?\"")|(?<op1>\<[oO][rR]\>|\<[aA][nN][dD]\>|\+|\-|&|<=|>=|<|>|==|!=|\*|\/)|(?<eq>=)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<x1>[^ ])";

/*
	prior tests - did not pass

			// // test about 1.0094 sec for 1000 loops	**v																																											** v
			// patt[m++] =
			// 	@"(?<l0>(?>(?<=^|[-+'""*=()&\/])(?>[\t ]*[-+]?))(?>(?>(?>(?>\d+ )?(?>\d+\/\d+['""])))|(?>(?>\d+'-(?>(?>\d+)?(?> \d+\/\d+""|\d+(?>\.\d*""|"")))))|(?>\d+(?>\.\d*""|""))|(?>(?>\d+\.\d*['""]))|(?>(?>\d+['""]))))|(?<f1>[a-zA-Z]\w*(?=\())|(?<s1>\"".+?\"")|(?<d1>(?>(?>(?<=^|[-+'""*=()&\/])(?>[\t ]*[-+]?))\d+\.\d*))|(?<n1>(?>(?<=^|[-+'""*=()&\/])(?>[\t ]*[-+]?))(?>\d+(?!\.)))|(?<op1>\<[oO][rR]\>|\<[aA][nN][dD]\>|\+|\-|&|<=|>=|<|>|==|!=|\*|\/)|(?<eq>=)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<x1>[^ ])";


			// // test 3 about 0.954 sec for 1000 loops  **v																																							** v
			// patt[m++] =
			// 	@"(?<l3>(?>(?<=^|[-+'""*=()&])[\t ]*[-+]?)(?>(?<l1>(?>(?>(?>\d+ )?(?>\d+\/\d+['""])))|(?>(?>\d+'-(?>(?>\d+)?(?> \d+\/\d+""|\d+(?>\.\d*""|"")))))|(?>\d+(?>\.\d*""|"")))|(?>(?>\d+\.\d*['""]))|(?<d1>\d+\.\d*)|(?>(?>\d+['""]))|(?<n1>\d+(?!\.))))|(?<f1>[a-zA-Z]\w*(?=\())|(?<s1>\"".+?\"")|(?<op1>\<[oO][rR]\>|\<[aA][nN][dD]\>|\+|\-|&|<=|>=|<|>|==|!=|\*|\/)|(?<eq>=)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<x1>[^ ])";
			//
			// // test 4 about  0.725 sec for 1000 loops  *v																																											** v
			// patt[m++] =
			// 	@"(?<l4>(?>(?<=^|[-+'""*=()&\/])(?>[\t ]*[-+]?))(?>(?>(?>(?>\d+ )?(?>\d+\/\d+['""])))|(?>(?>\d+'-(?>(?>\d+)?(?> \d+\/\d+""|\d+(?>\.\d*""|"")))))|(?>\d+(?>\.\d*""|""))|(?>(?>\d+\.\d*['""]))|(?>(?>\d+['""]))))|(?<d1>(?>(?>(?<=^|[-+'""*=()&\/])(?>[\t ]*[-+]?))\d+\.\d*))|(?<n1>(?>(?<=^|[-+'""*=()&\/])(?>[\t ]*[-+]?))(?>\d+(?!\.)))|(?<f1>[a-zA-Z]\w*(?=\())|(?<s1>\"".+?\"")|(?<op1>\<[oO][rR]\>|\<[aA][nN][dD]\>|\+|\-|&|<=|>=|<|>|==|!=|\*|\/)|(?<eq>=)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<x1>[^ ])";
*/

			string[] test = new string[50];

			int[] matchCount = new int[numPatterns];
			string[][] result = new string[numPatterns][];

			for (int i = 0; i < numPatterns; i++)
			{
				result[i] = new string[numstrings];
			}

			int numTestStr = 0;

			test[numTestStr++] = @"{[A1]} + {!B2} + {@C2} + {#D3} + {$E4} + {%F5}";
			test[numTestStr++] = @"=(1 + (21 + 22 + (31 + 32 * (41 + 42) * (sign(51+52) ) ) ) + 2*3) + 4+5 + {[A1]} + ([{A1]}) & ""text""";
			test[numTestStr++] = @"{!a}= 223 (-123 + (+123 / 123 + (123 + (123 + 156) + 456)) + 456) + {[A1]} & ""text"" + ""tex() & t"" d  {!afdfd} + 100";
			test[numTestStr++] = @"=(1 + (21 + 22 + (31 + 32 * (41  -42) * Sign(51 ++52) ) ) + 2*3) asfasf x <= y + - z > a t <= u  m != n + 1/2";
			test[numTestStr++] = @"= 2 <or> 3 < 4 <and> 5 * f()";
			test[numTestStr++] = @"=1.05";
			test[numTestStr++] = @"=-1.05 * 1.05 + 123 + +1. ++1.05";
			test[numTestStr++] = @"= 100/1.05";
			test[numTestStr++] = @"1' + -2' + +3'";
			test[numTestStr++] = @"4"" +5"" +-6""";
			test[numTestStr++] = @"1.5' + +2.5'+ -2.5'";
			test[numTestStr++] = @"4.5"" + +5.5"" * -6.5""";
			test[numTestStr++] = @"= 1'-0"" + -2'-0"" * +3'-0""";
			test[numTestStr++] = @"1'-1.5"" + -2'-1.5"" * +3'-1.5""";
			test[numTestStr++] = @"1.1'-5"" + -2.1'-6.5"" *+3.1'-6.5""";
			test[numTestStr++] = @"1/2""  + +1/2"" -1/2""";
			test[numTestStr++] = @" 1/2' + -1/2' +1/2'";
			test[numTestStr++] = @"  +1/2""";
			test[numTestStr++] = @"    1/2"" + -1/2' + -4 1/2' - +4.5'";
			test[numTestStr++] = @"=-5'-6.5"" +600' * 700.05' + -fun(3*4) + d";

			Stopwatch s = new Stopwatch();

			int[] tests = new int[numTestStr + 1];

			// m = number of patterns
			for (int loop = 0; loop < numPatterns; loop++)
			{
				countLoops = 0;
				countTests = 0;
				groupCountTotalLoops = 0;

				win.WriteLine("\n*** start loop| " + loop + "\n");

				s.Reset();
				s.Start();

				Regex r = new Regex(patt[loop], RegexOptions.Compiled | RegexOptions.ExplicitCapture);

				for (int l = 0; l < numLoops; l++)
				{
					countLoops++;
					matchCount[loop] = 0;

					for (int testIdx = 0; testIdx < numTestStr; testIdx++)
					{
						MatchCollection c = r.Matches(test[testIdx]);

						int x = countMatches(c);
						int y = getMatches(c, result[loop], matchCount[loop]);

						// win.WriteLine($"pattern| {j,-4:D}  loop| {l,-4:D}  test| {i,-4:D}" );
						// win.Write($"count x| {x,-4:D}  y| {y,-4:D}  " );
						//
						// if (tests[i] != 0)
						// {
						// 	win.WriteLine($"tests[]| {tests[i],-4:D}  match?| {x==tests[i]}" );
						// }
						// else
						// {
						// 	win.WriteLine($"no tests" );
						// }

						// tests[testIdx] = x;

						groupCountTotalLoops += x;
						matchCount[loop] += y;
						countTests++;
					}
				}

				s.Stop();

				double secs = s.Elapsed.TotalSeconds;

				win.WriteLine("\n\n*** loop #| " + loop + "  loop count| " + countLoops + "  total tests| " + countTests);

				win.WriteLine("time for patt| " + secs.ToString("N6") + " seconds");
				win.WriteLine("time per loop| " + (secs / countLoops).ToString("N6") + " seconds");
				win.WriteLine("time per test| " + (secs / countTests).ToString("N6") + " seconds");

				win.WriteLine("total groups found| " + groupCountTotalLoops.ToString("N0"));
				win.WriteLine("avg groups per loop| " + (groupCountTotalLoops / countLoops).ToString("N4"));
				win.WriteLine("pattern| " + patt[loop]);
				win.WriteLine("\n\n");
			}

			if (numLoops == 1)
			{
				// compare the matches from one pattern to the other
				showMatches2(result, 0, 1 , matchCount);
				showMatches2(result, 0, 2, matchCount);
			}
		}

		// initial split test for a formula - incomplete
		public void splitTest9()
		{
			win.WriteLine("split test 9-4");
			win.WriteLine("splitting| ");

			// string patt = @"(?<eq>=)|(?<f1>[a-zA-Z]\w+?\(.+?\))|(?<s1>\"".+?\"")|(?<op1>\+|\-)|(?<op2>\\|\*)|(?<op3>&)|(?<p1>\(|\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<n1>\d+)|(?<x1>.*?)";
			//string patt = @"(?<eq>=)|(?<f1>[a-zA-Z]\w+?\(.+?\))|(?<s1>\"".+?\"")|(?<op1>\+|\-)|(?<op2>\\|\*)|(?<op3>&)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<n1>\d+)";
			// string patt = @"(?<eq>=)|(?<f1>[a-zA-Z]\w+(?=\())|(?<s1>\"".+?\"")|(?<op1>\+|\-)|(?<op2>\\|\*)|(?<op3>&)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<n1>\d+)";
			// string patt = @"(?<f1>[a-zA-Z]\w+(?=\())|(?<s1>\"".+?\"")|(?<n1>-?\d+)|(?<op1>\+|\-|&|<=|>=|<|>|==|!=)|(?<op2>\*|\\)|(?<eq>=)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)";

			// string patt =
			// 	@"(?<l1>[-+]?(?>\d+'-(?>\d*\.\d+|(?>\d+ )?\d+\/\d+|\d+)""|(?>\d+ \d+\/\d+|\d+\/\d+|\d*\.\d+|\d+)[""']))|(?<fr1>[-+]?\d+ \d+\/\d+|\d+\/\d+)|(?<d1>[-+]?(?>\d+\.\d*|\d*\.\d+))|(?<n1>[-+]?\d+(?![.\/]))|(?<fn1>[a-zA-Z]\w*(?=\())|(?<s1>\"".+?\"")|(?<op1>\<[oO][rR]\>|\<[aA][nN][dD]\>|\+|\-|&|<=|>=|<|>|==|!=|\*|\/)|(?<eq>=)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<x1>[^ ])";
			//
			// Regex r = new Regex(patt, RegexOptions.Compiled);

			string[] test = new string[15];

			int k = 0;

			test[k++] = "=( 1 + (21 + 22 + (31 + 32 * (41 + 42) * (sign(51+52) ) ) * sign( alpha, beta, (1 + 2) ) ) + 2*3) + 4+5 + {[A1]} + ({!B1}) & \"text text text\"";
			// test[k++] = "= 123 (456 + 789) + (321 + 654) + 987";
			// test[k++] = "= 123 (456 + (123 + 456) + 246) + 567";
			// test[k++] = "= 123 (456 + (123 + 456) + 246) + 567)";
			// test[k++] = "= 123 (456 + (123 + 456) + 246 + 567";
			// test[k++] = "= 123 (456 + (123 + (678 + (123 + (123 + 456 + 789) + (123 + 456) + (123 + (123 + 456) + 456)) + 456) + 246))";
			// test[k++] = "= 123 (123 + (123 + 123 + (123 + (123 + 456) + 456)) + 456)";
			// test[k++] = "= 123 ((123 + 567) + (678 + 891) + (123 + 123 + (123 + (123 + 456) + 456)) + 456) + 246";
			// test[k++] = "= 123 ((123 + 567) + 246";
			// test[k++] = "= 123 (123 + 567)) + 246";
			// test[k++] = "= 123 ((((123 + 567) + 246";
			// test[k++] = "= 123 (123 + 567)))) + 246";

			// makePartCodes(EQ_CODE_PARTS_MAX);
			// listCodeParts();

			// testClassifyCodePart();

			// makePartCodes2(EQ_CODE_PARTS_MAX);
			// listCodeParts2();
			// testClassifyCodePart2();

			// return;

			for (int i = 0; i < test.Length; i++)
			{
				if (test[i] == null) break;

				win.Write("\n");
				win.WriteLine("testing| " + i + "| length| " + test[i].Length);
				win.Write("\n");

				win.WriteLine("0         1         2         3         4         5         6         7         8         9        10");
				win.WriteLine("0123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|");
				win.WriteLine(test[i]);
				win.Write("\n");


				// key = sort order
				// tupple = level (int), op (string), value (string) 
				// List<List<Eq1>>  result = parseEq(test[i], r);
				List<List<ParseData>> result = parseEq4(test[i]);

				if (result != null)
				{
					
					win.Write("\n");
					win.WriteLine($"split test 9| idx| {i} (worked)| Balanced?| {parse.IsBalanced} maxLevels| {parse.MaxLevels}");
					win.Write("\n");

					// showEqAllLevels2(result);
					// showEqList(result);

					ShowEq(result);
					ShowEqList(result);

				}
				else
				{
					win.Write("\n");
					win.WriteLine($"split test 9| idx| {i} (failed)| Balanced?| {parse.IsBalanced} maxLevels| {parse.MaxLevels}");
					win.Write("\n");
				}
			}
		}

	#endregion

	#region private methods

	#region main routines


		private int countMatches(MatchCollection c)
		{
			int k = 0;
			Match m;
			Group g;

			if (c.Count < 1)
			{
				win.WriteLine("*** MATCH FAILED ***");
				return 0;
			}

			for (int i = 0; i < c.Count; i++)
			{
				m = c[i];

				for (int j = 1; j < m.Groups.Count; j++)
				{
					g = m.Groups[j];

					if (g.Success) k++;
				}
			}

			return k;
		}

		private void showMatches2(string[][] s, int pos0, int pos1, int[] count)
		{
			win.WriteLine("\nshow pattern test|");

			int colLeft = pos0;
			int colRight = pos1;

		#pragma warning disable CS0219 // The variable 'idxLeft' is assigned but its value is never used
			int idxLeft = 0;
		#pragma warning restore CS0219 // The variable 'idxLeft' is assigned but its value is never used

			if (count[colLeft] >= count[colRight])
			{
				colLeft = pos1;
				colRight = pos0;
			}

			win.WriteLine($"small count| {count[colLeft],-4:D}  larger count| {count[colRight],-4:D}\n");

			for (int i = 0; i < count[colRight]; i++)
			{
				string left = string.IsNullOrWhiteSpace(s[colLeft][i]) ? "empty" : s[colLeft][i];
				string right = string.IsNullOrWhiteSpace(s[colRight][i]) ? "empty" : s[colRight][i];

				win.WriteLine($"smaller| {i,-4:D}| \t{left,-15} \tlarger| {i,-4:D}| \t{right,-15}");
			}

			win.WriteLine("\n");
		}

		private int getMatches(MatchCollection c, string[] matches, int start)
		{
			int matchCount = start;
			Match m;
			Group g;

			if (c.Count < 1)
			{
				win.WriteLine("*** MATCH FAILED ***");
				return 0;
			}

			for (int i = 0; i < c.Count; i++)
			{
				m = c[i];

				for (int j = 1; j < m.Groups.Count; j++)
				{
					g = m.Groups[j];

					if (g.Success)
					{
						// win.WriteLine("match| " + $"{g.Name,-5}|\t{g.Value}");
						matches[matchCount++] = $"{g.Name,-5}|\t{g.Value}";
					}
				}
			}

			return matchCount - start;
		}

		private List<List<ParseData>> parseEq4(string eq)
		{
			parse = new ParsePhase();
			bool result = parse.Parse4(eq);
			return result ? parse.ParseList : null;
		}

	#endregion

	#region list info

		private void ShowEqList(List<List<ParseData>> eq)
		{
			
			win.TabClr("begin");
			win.WriteLine("show eq list 2| ");
			win.WriteLine("");
			win.TabUp();
			//                        0       1          2   3       4           5            6
			int[] cols      = new [] {10,     10,        5,  5,       22,         12,         25};
			string[] titles = new [] {"name", "val str", "", "level", "val type", "data grp", "desc"};

			int[] order     = new [] {6, 0, 1, 2, 3, 4, 5};

			int margin = 2;

			int item = 0;
			int rf = 0;
			string val = "";

			foreach (List<ParseData> tList in eq)
			{
				win.WriteLineTab($"for reference| {rf++:D}");

				win.TabUp();

				ShowPhaseDataHeader(order, titles, cols, margin);

				foreach (ParseData pd in tList)
				{
					ShowParseData2a(pd, order, cols, margin);
				}

				win.TabDn();
			}

			win.TabDn();
		}

		private void ShowEq(List<List<ParseData>> phl)
		{
			win.WriteLine("");
			win.WriteLine("show whole eq");

			ShowEqForRef2(phl, 0, 0);

			win.TabClr("restart");
			win.WriteLine("");
			win.WriteLine("");

			ShowEqForRef(phl, 0);

			win.WriteLine("");
			win.WriteLine("");
		}

		private int ShowEqForRef(List<List<ParseData>> phl, int r)
		{
			ParseData ph;
			int i;
			int p;

			win.WriteTab("");

			for (i = 0; i < phl[r].Count; i++)
			{
				ph = phl[r][i];

				win.Write($"[{ph.Name}:{ph.Value}] ");

				if (ph.Name.Equals(grpRefName))
				{
					win.Write($"[{ph.RefIdx}] ");

					win.TabSet(i);
					win.WriteLine("");
					p = ShowEqForRef(phl, ph.RefIdx);

					win.WriteLine("");
					win.TabSet(p);
					win.WriteTab("");
				}
			}

			return i;
		}
		
		private int ShowEqForRef2(List<List<ParseData>> phl, int r, int o)
		{
			
			ParseData ph;
			int i;
			int l = o;
			int p;

			win.TabSet(l);
			win.WriteTab("");

			for (i = 0; i < phl[r].Count; i++)
			{
				ph = phl[r][i];

				if (!ph.Name.Equals(grpRefName))
				{
					if (!ph.Name.Equals(grpBegName) && !ph.Name.Equals(grpEndName))
					{
						l++;
						win.Write($"{ph.Value} ");
					}
				} 
				else
				{
					win.WriteLine("");
					l = ShowEqForRef2(phl, ph.RefIdx, l - 1);
					win.WriteLine("");
					win.TabSet(l);
					win.WriteTab("");
				}
			}

			return l;
		}

		private void ShowPhaseDataHeader(int[] order, string[] t, int[] c, int margin)
		{
			win.WriteTab("");
			int j;

			for (var i = 0; i < t.Length; i++)
			{
				j = order[i];
				win.Write(t[j].PadRight(c[j]+margin));
			}

			win.WriteLine("");

			win.WriteTab("");

			for (var i = 0; i < c.Length; i++)
			{
				j = order[i];
				win.Write("-".Repeat(c[j]));
				win.Write(" ".Repeat(margin));
			}

			win.WriteLine("");

		}

		private void ShowParseData2a(ParseData pd, int[] order, int[] c, int margin)
		{
			string[] vals = new string[c.Length];

			string val = pd.Value;

			vals[0] = pd.Name;
			vals[1] = val;

			if (pd.GotRefIdx)
			{
				vals[2] = $" [{pd.RefIdx}]";
			}
			else
			{
				vals[2] = "";
			}

			
			vals[3] = pd.Level.ToString("D");

			vals[4] = pd.Definition?.ValueType.ToString() ?? "null";
			vals[5] = pd.Definition?.DataGroup.ToString() ?? "null";
			vals[6] = pd.Definition?.Description ?? "null";



			ShowData(order, vals, c, margin);
		}


		private void ShowData(int[] order, string[] values, int[] c, int margin)
		{
			win.WriteTab("");

			int j;

			for (int i = 0; i < values.Length; i++)
			{
				j = order[i];

				win.Write(Truncate(values[j], c[j]).PadRight(c[j] + margin));
			}

			win.WriteLine("");
		}

		private const int SUFFIX = 3;
		private const int GAP = 3;

		private string Truncate(string s, int l)
		{
			int len = s.Length;

			if (len <= l) return s;
			if (len < SUFFIX + GAP) return s.Substring(0, l);

			int div = l - SUFFIX - GAP;

			return s.Substring(0, div) + ".." + s.Substring(len - SUFFIX, SUFFIX);
		} 


		// private void ShowPhaseData(ParseData pd, int[] c)
		// {
		// 	string val = pd.Value;
		//
		// 	if (pd.GotRefIdx)
		// 	{
		// 		val += $" [{pd.RefIdx}]";
		// 	}
		//
		// 	win.WriteTab("");
		// 	win.Write($"{ pd.Name.PadRight(c[0])} ");
		// 	win.Write($"{val.PadRight(c[1])} ");
		// 	win.Write($"{pd.Level.ToString("D").PadRight(c[2])} ");
		//
		// 	if (pd.Definition != null)
		// 	{
		// 		win.Write($"{(pd.Definition?.ValueType.ToString() ?? "is null").PadRight(c[3])} ");
		// 		win.Write($"{pd.Definition.DataGroup.ToString().PadRight(c[4])} ");
		// 		win.Write($"{pd.Definition.Description.PadRight(c[5])} ");
		//
		// 	}
		// 	else
		// 	{
		// 		win.Write("def val type| is null");
		// 	}
		//
		// 	win.Write("\n");
		// }


	#endregion

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is Tests01A";
		}

	#endregion

		// private EqComponentList opCodes;
		// private EqComponentList compCodes;

		// private static ValueDefinitions vds = ValueDefinitions.ValDefInst;

		// opCodes = new EqComponentList(eqPartCodes2);

		// private void makePartCodes2(int len)
		// {
		// 	int k = 0;
		// 	eqPartCodeIdx2 = new Tuple<int, int>[len];
		//
		// 	for (int i = 0; i < eqPartCodes2.Length; i++)
		// 	{
		// 		Tuple<string, int>[] codes = eqPartCodes2[i].Item2;
		//
		// 		for (int j = 0; j < codes.Length; j++)
		// 		{
		// 			eqPartCodeStr += $"{(codes[j].Item1),-EQ_CODE_WIDTH}♦";
		// 			eqPartCodeIdx2[k++] = new Tuple<int, int>(i, j);
		// 		}
		// 	}
		// }
		// private const int EQ_CODE_WIDTH = 5;
		// private const int EQ_CODE_PARTS_MAX = 20;
		// private string eqPartCodeStr;
		// private int[] eqPartCodeIdx;
		// private Tuple<int, int>[] eqPartCodeIdx2;

		// private List<List<ParsePhData>> parseEq3(string eq)
		// {
		// 	parse = new ParsePhase();
		// 	List<List<ParsePhData>> t1 = parse.Parse3(eq);
		// 	return t1;
		// }

		// private void showEqList(List<List<Eq1>> eq)

		// {
		// 	win.showTabId = false;
		//
		// 	win.WriteLine("show eq list| ");
		// 	win.TabUp();
		//
		// 	int item = 0;
		// 	int level = 0;
		//
		// 	foreach (List<Eq1> tList in eq)
		// 	{
		// 		win.WriteLineTab($"for level| {level++,3:###}" );
		//
		// 		win.TabUp();
		// 		foreach (Eq1 t in tList)
		// 		{
		// 			win.WriteTab($"level| {t.Level,-4:##}");
		// 			win.Write($"item| {item++,-5:###}");
		// 			win.Write($"op| {t.EqCode,-12}");
		// 			win.Write($"val| " + t.Value.PadRight(10));
		// 			win.Write($"sort| {t.SortCode:##0}");
		// 			win.Write("\n");
		// 		}
		//
		// 		win.TabDn();
		// 	}
		//
		// 	win.TabDn();
		// }
		//
		// private void showEqListItem(Eq1 item)
		// {
		// 	win.Write("item| level| " + item.Level.ToString("D3").PadRight(5));
		// 	win.Write("code| " + item.EqCode.PadRight(5));
		// 	win.Write("value| " + item.Value);
		// 	win.Write("\n");
		// }
		//
		// private void showEqAllLevels2(List<List<Eq1>> eq)
		// {
		// 	win.WriteLine("show eq by level| ");
		//
		// 	int level = 0;
		//
		// 	foreach (List<Eq1> tList in eq)
		// 	{
		// 		string es = getEqForLevel2(tList);
		// 		win.WriteLine("for level| >" + level.ToString("4:0") + "< " + es);
		//
		// 		level++;
		// 	}
		// }
		//
		// private void showEqAllLevels(List<Eq1> eq)
		// {
		// 	win.WriteLine("show eq by level| ");
		//
		// 	for (int level = 0; level < MAX_EQ_DEPTH; level++)
		// 	{
		// 		string es = getEqForLevel(eq, level);
		//
		// 		if (string.IsNullOrWhiteSpace(es)) break;
		//
		// 		win.WriteLine("for level| >" + level.ToString("4,0") + "< " + es);
		// 	}
		// }
		// private string getEqForLevel2(List<Eq1> eq)
		// {
		// 	Eq1 t;
		// 	StringBuilder sb = new StringBuilder();
		//
		// 	for (int i = 0; i < eq.Count; i++)
		// 	{
		// 		t = eq[i];
		//
		// 		sb.Append("[").Append(t.EqCode).Append(":").Append(t.Value).Append("]");
		//
		// 		if (i != eq.Count - 1) sb.Append(" ");
		// 	}
		//
		// 	return sb.ToString();
		// }
		//
		// private string getEqForLevel(List<Eq1> eq, int level)
		// {
		// 	Eq1 t;
		// 	StringBuilder sb = new StringBuilder();
		//
		// 	for (int i = 0; i < eq.Count; i++)
		// 	{
		// 		t = eq[i];
		//
		// 		if (t.Level != level) continue;
		//
		// 		sb.Append("[").Append(t.EqCode).Append(":").Append(t.Value).Append("]");
		//
		// 		if (i != eq.Count - 1) sb.Append(" ");
		// 	}
		//
		// 	return sb.ToString();
		// }

		// private string makeEqKey(int level, int order, int op, int seq)
		// {
		// 	return level.ToString("D3") + ":" + order.ToString("D3") + ":" + op.ToString("D3") + ":" + seq.ToString("D3");
		// }

		// private void ShowEq(List<List<ParseData>> eq, int maxLevels)
		// {
		// 	StringBuilder[] sb = GetMatchingEq(eq, maxLevels);
		//
		//
		// }

		// private StringBuilder[] GetMatchingEq(List<List<ParseData>> eq, int maxLevels)
		// {
		// 	if (eq == null || eq.Count < 1) return null;
		// 	int elemIdx = 0;
		// 	int priorElemIdx = 0;
		// 	int level = 0;
		// 	int priorLevel = 0;
		// 	ParseData ph;
		//
		// 	bool cont = true;
		//
		// 	StringBuilder[] sb = new StringBuilder[maxLevels];
		//
		// 	for (int i = 0; i < maxLevels; i++)
		// 	{
		// 		sb[i] = new StringBuilder();
		// 	}
		//
		// 	do
		// 	{
		// 		ph = eq[level][elemIdx];
		//
		// 		if (ph.Name.Equals(grpRefName))
		// 		{
		// 			priorLevel = level;
		// 			priorElemIdx = elemIdx;
		//
		// 			level = ph.Position;
		// 			elemIdx = 1;
		//
		// 			continue;
		// 		} 
		// 		// else if (ph.Name.Equals(grpBegName))
		// 		// {
		// 		// 	elemIdx++;
		// 		// 	continue;
		// 		// } 
		// 		else  if (ph.Name.Equals(grpEndName))
		// 		{
		// 			level = priorLevel;
		// 			elemIdx = priorElemIdx + 1;
		// 			continue;
		// 		}
		//
		// 		sb[level].Append(ph.Value).Append(" ");
		//
		// 		elemIdx++;
		//
		// 		if (elemIdx == eq[level].Count) cont = false;
		//
		// 	}
		// 	while (cont);
		//
		// 	return sb;
		// }

		// private void listCodeParts2()
		// {
		// 	win.WriteLine("listCodeParts| start");
		//
		// 	foreach (Tuple<int, int> t in eqPartCodeIdx2)
		// 	{
		// 		if (t == null) break;
		//
		// 		Tuple<string, Tuple<string, int>[]> tx = eqPartCodes2[t.Item1];
		//
		// 		Tuple<string, int> tz = tx.Item2[t.Item2];
		//
		// 		win.Write("part| " + $"{tz.Item1,-8}");
		// 		win.Write($"code| {(tz.Item2),-8:0}");
		// 		win.Write($"desc| {(tx.Item1)}");
		//
		// 		win.Write("\n");
		// 	}
		// }
		//
		// private void testClassifyCodePart2()
		// {
		// 	win.WriteLine("test classify2| ");
		// 	win.Write("\n");
		//
		// 	string[] test = new [] {"*", "+", "&", "<=", "!="};
		//
		// 	foreach (string s in test)
		// 	{
		// 		Tuple<string, string, int> t = classifyCodePartOp2A(s);
		//
		// 		if (t != null)
		// 		{
		// 			win.Write("part| " + $"{(">" + s + "<"),-8}");
		// 			win.Write($"str|  {(">" + t.Item2 + "<"),-8:0}");
		// 			win.Write($"code| {(t.Item3),-8:0}");
		// 			win.Write($"desc| {(t.Item1)}");
		// 			win.Write("\n");
		// 		}
		// 	}
		// }


		// private List<List<Eq1>>  parseEq(string eq, Regex r)
		// {
		// 	Dictionary<string, Eq1> result = new Dictionary<string, Eq1>();
		//
		// 	MatchCollection c = r.Matches(eq);
		//
		// 	if (c.Count < 1)
		// 	{
		// 		win.WriteLine("*** MATCH FAILED ***");
		// 		return null;
		// 	}
		//
		// 	win.WriteLine("collection found| " + c.Count + " matches");
		//
		// 	// List<Eq1> t = orgRxString(c);
		// 	List<List<Eq1>> t1 = orgRxString2(c);
		//
		// 	return t1;
		// }

		// private List<List<Eq1>> orgRxString2(MatchCollection c)
		// {
		// 	List<List<Eq1>> result = new List<List<Eq1>>();
		// 	result.Add(new List<Eq1>());
		//
		// 	string grpBeg = "grpBeg";
		// 	int grpBegCode =  classifyCodePartOp2B("(") - 100;
		//
		// 	string grpEnd = "grpEnd";
		// 	int grpEndCode =  classifyCodePartOp2B(")") - 100;
		//
		// 	string grpRef = "grpRef";
		// 	int grpRefCode =  grpBegCode - 1;
		//
		// 	int grp = 0;
		// 	int level = 0;
		//
		//
		// 	Eq1 t;
		//
		// 	Match m;
		//
		// 	string opMatchList1 = "op1|op2|pdn|pup";
		//
		//
		// 	for (var i = 0; i < c.Count; i++)
		// 	{
		// 		m = c[i];
		//
		// 		for (var j = 1; j < m.Groups.Count; j++)
		// 		{
		// 			Group g = m.Groups[j];
		//
		// 			if (g.Success)
		// 			{
		// 				if (g.Name == "pdn")
		// 				{
		// 					result[level].Add(new Eq1(level, grpRef, grp.ToString("D3"), grpRefCode));
		// 					level++;
		// 					if (result.Count == level)
		// 					{
		// 						result.Add(new List<Eq1>());
		// 					}
		//
		// 					result[level].Add(new Eq1(level, grpBeg, grp++.ToString("D3"), grpBegCode));
		// 				}
		//
		// 				t = new Eq1(level, g.Name, g.Value);
		//
		// 				if (opMatchList1.IndexOf(g.Name) >= 0)
		// 				{
		// 					t.SortCode = classifyCodePartOp2B(g.Value);
		// 				}
		//
		// 				result[level].Add(t);
		//
		// 				if (g.Name == "pup")
		// 				{
		// 					result[level].Add(new Eq1(level, grpEnd, "", grpEndCode));
		// 					level--;
		// 				}
		// 			}
		// 		}
		// 	}
		//
		// 	return result;
		// }
		//
		// private List<Eq1> orgRxString(MatchCollection c)
		// {
		// 	List<Eq1> result = new List<Eq1>();
		//
		// 	string grpRef = "grpRef";
		// 	string grpBeg = "grpBeg";
		// 	string grpEnd = "grpEnd";
		// 	int grp = 0;
		// 	int level = 0;
		// #pragma warning disable CS0168 // The variable 'k' is declared but never used
		// 	string k;
		// #pragma warning restore CS0168 // The variable 'k' is declared but never used
		// 	Eq1 t;
		//
		//
		// 	foreach (Match m in c)
		// 	{
		// 		for (var i = 1; i < m.Groups.Count; i++)
		// 		{
		// 			Group g = m.Groups[i];
		//
		// 			if (g.Success)
		// 			{
		// 				if (g.Name == "pdn")
		// 				{
		// 					result.Add(new Eq1(level, grpRef, grp.ToString("D3")));
		// 					level++;
		// 					result.Add(new Eq1(level, grpBeg, grp++.ToString("D3")));
		// 				}
		//
		// 				t = new Eq1(level, g.Name, g.Value);
		// 				result.Add(t);
		//
		// 				if (g.Name == "pup")
		// 				{
		// 					result.Add(new Eq1(level, grpEnd, ""));
		// 					level--;
		// 				}
		// 			}
		// 		}
		// 	}
		//
		// 	return result;
		// }

		// // "description", "matched op code", ##:sort code
		// private int classifyCodePartOp2B(string op)
		// {
		// 	string test = $"{op,-EQ_CODE_WIDTH}♦";
		// 	int j = eqPartCodeStr.IndexOf(test);
		//
		// 	if (j < 0)
		// 	{
		// 		return -1;
		// 	}
		//
		// 	int idx1 = j / (EQ_CODE_WIDTH + 1);
		//
		// 	Tuple<int, int> t = eqPartCodeIdx2[idx1];
		//
		// 	return eqPartCodes2[t.Item1].Item2[t.Item2].Item2;
		// }
		//
		// // "description", "matched op code", ##:sort code
		// private Tuple<string, string, int> classifyCodePartOp2A(string op)
		// {
		// 	string test = $"{op,-EQ_CODE_WIDTH}♦";
		// 	int j = eqPartCodeStr.IndexOf(test);
		//
		// 	if (j < 0)
		// 	{
		// 		return null;
		// 	}
		//
		// 	int idx1 = j / (EQ_CODE_WIDTH + 1);
		//
		// 	Tuple<int, int> t = eqPartCodeIdx2[idx1];
		//
		// 	return new Tuple<string, string, int>(eqPartCodes2[t.Item1].Item1,
		// 		eqPartCodes2[t.Item1].Item2[t.Item2].Item1, eqPartCodes2[t.Item1].Item2[t.Item2].Item2 );
		// }
		//
		// // data for code parts
		// private Tuple<string, Tuple<string, int>[]>[] eqPartCodes2 = new []
		// {
		// 	new Tuple<string, Tuple<string, int>[]>("logical or",  new [] {new Tuple<string, int>("<or>", 101)}),
		// 	new Tuple<string, Tuple<string, int>[]>("logical and", new [] {new Tuple<string, int>("<and>", 151)}),
		// 	new Tuple<string, Tuple<string, int>[]>("equality",    new []
		// 	{
		// 		new Tuple<string, int>("==", 201),
		// 		new Tuple<string, int>("!=", 202),
		// 	}),
		// 	new Tuple<string, Tuple<string, int>[]>("relational",  new []
		// 	{
		// 		new Tuple<string, int>("<", 251),
		// 		new Tuple<string, int>(">", 252), new Tuple<string, int>("<=", 253), new Tuple<string, int>(">=", 254),
		// 	}),
		// 	new Tuple<string, Tuple<string, int>[]>("additive string", new [] {new Tuple<string, int>("&", 301)}),
		// 	new Tuple<string, Tuple<string, int>[]>("additive",    new []
		// 	{
		// 		new Tuple<string, int>("+", 351),
		// 		new Tuple<string, int>("-", 352),
		// 	}),
		// 	new Tuple<string, Tuple<string, int>[]>("Multiplicative", new []
		// 	{
		// 		new Tuple<string, int>("*", 401),
		// 		new Tuple<string, int>("\\", 402),
		// 	}),
		// 	new Tuple<string, Tuple<string, int>[]>("grouping", new []
		// 	{
		// 		new Tuple<string, int>("(", 901),
		// 		new Tuple<string, int>(")", 902),
		// 	}),
		// };
		//
		// private  Tuple<string, Tuple<string, int>[]>[] getCompCodes()
		// {
		// 	Tuple<string, Tuple<string, int>[]>[] eqCompCodes2 = new []
		// 	{
		// 		new Tuple<string, Tuple<string, int>[]>("assignment",  new [] {new Tuple<string, int>("eq", 11)}),
		// 		new Tuple<string, Tuple<string, int>[]>("number",
		// 			new []
		// 			{
		// 				new Tuple<string, int>("n1", 21),
		// 				new Tuple<string, int>("d1", 22),
		// 				new Tuple<string, int>("l1", 23),
		// 			}),
		// 		new Tuple<string, Tuple<string, int>[]>("variable",
		// 			new [] {new Tuple<string, int>("v1", 31), new Tuple<string, int>("v2", 32)}),
		// 		new Tuple<string, Tuple<string, int>[]>("word",  new [] {new Tuple<string, int>("w1", 41)}), // could be "true" or "false"
		// 		new Tuple<string, Tuple<string, int>[]>("string",  new [] {new Tuple<string, int>("s1", 51)}),
		// 		new Tuple<string, Tuple<string, int>[]>("error",  new [] {new Tuple<string, int>("x1", 61)}),
		// 		new Tuple<string, Tuple<string, int>[]>("function",  new [] {new Tuple<string, int>("f1", 91)}),
		// 	};
		//
		//
		// 	return eqCompCodes2;
		// }

		// configure code parts


		// private void makePartCodes(int len)
		// {
		// 	int j = 0;
		// 	int k = 0;
		// 	int before;
		// 	eqPartCodeIdx = new int[len];
		//
		// 	for (var i = 0; i < eqPartCodes.Length; i++)
		// 	{
		// 		before = 0;
		//
		// 		j = eqPartCodes[i].Item1.IndexOf('♦', before);
		//
		// 		while (j > 0)
		// 		{
		// 			eqPartCodeStr += $"{(eqPartCodes[i].Item1.Substring(before, j - before)),-EQ_CODE_WIDTH}♦";
		// 			eqPartCodeIdx[k++] = i;
		//
		// 			before = j + 1;
		//
		// 			j = eqPartCodes[i].Item1.IndexOf('♦', before);
		// 		}
		//
		// 		while (j > 0) ;
		// 	}
		// }
		//
		// private void testClassifyCodePart()
		// {
		// 	win.WriteLine("test classify| ");
		// 	win.Write("\n");
		//
		// 	string[] test = new [] {"*", "+", "&", "<=", "!="};
		//
		// 	foreach (string s in test)
		// 	{
		// 		int idx = classifyCodePartOp(s);
		//
		// 		if (idx >= 0)
		// 		{
		// 			win.Write("part| " + $"{s,-8}");
		// 			win.Write($"code| {(eqPartCodes[idx].Item3),-8:0}");
		// 			win.Write($"desc| {(eqPartCodes[idx].Item2)}");
		// 			win.Write("\n");
		// 		}
		// 	}
		// }
		//
		// private void listCodeParts()
		// {
		// 	win.WriteLine("listCodeParts| start");
		// 	int j;
		// 	int idx1;
		// 	int idx2;
		// 	int before = 0;
		// 	string part;
		//
		// 	j = eqPartCodeStr.IndexOf('♦', before);
		//
		// 	while (j > 0)
		// 	{
		// 		idx1 = j / (EQ_CODE_WIDTH + 1);
		// 		idx2 = eqPartCodeIdx[idx1];
		// 		part = eqPartCodeStr.Substring(before, j - before).Trim();
		//
		// 		win.Write("part| " + $"{part,-8}");
		// 		win.Write($"code| {(eqPartCodes[idx2].Item3),-8:0}");
		// 		win.Write($"desc| {(eqPartCodes[idx2].Item2)}");
		//
		// 		win.Write("\n");
		//
		// 		before = j + 1;
		// 		j = eqPartCodeStr.IndexOf('♦', before);
		// 	}
		// }

		
		// private int classifyCodePartOp(string op)
		// {
		// 	string test = $"{op,-EQ_CODE_WIDTH}♦";
		// 	int j = eqPartCodeStr.IndexOf(test);
		//
		// 	if (j < 0)
		// 	{
		// 		return -1;
		// 	}
		//
		// 	int idx1 = j / (EQ_CODE_WIDTH + 1);
		//
		// 	return eqPartCodeIdx[idx1];
		// }
		//

		
		// // "regex pattern code" "description" {index} {id/sort code} {width}
		// private Tuple<string, string, int>[] eqPartCodes = new []
		// {
		// 	new Tuple<string, string, int>(@"<or>♦", "Logical or"   , 200),
		// 	new Tuple<string, string, int>(@"<and>♦", "Logical and" , 300),
		// 	new Tuple<string, string, int>(@"==♦!=♦", "equality"    , 400),
		// 	new Tuple<string, string, int>(@"<♦>♦", "relational"    , 500),
		// 	new Tuple<string, string, int>(@"<=♦=>♦", "relational"  , 550),
		// 	new Tuple<string, string, int>(@"+♦-♦", "additive"      , 600),
		// 	new Tuple<string, string, int>(@"&♦", "additive string" , 700),
		// 	new Tuple<string, string, int>(@"*♦\♦", "Multiplicative", 800),
		// 	new Tuple<string, string, int>(@"(♦)♦", "grouping"      , 10000),
		// };

	}
}