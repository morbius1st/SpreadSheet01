#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CellsTest.Windows;
using Microsoft.Office.Interop.Excel;

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
		}

	#region private fields

		private static MainWindow win;

		private EqComponentList opCodes;
		private EqComponentList compCodes;

	#endregion

	#region ctor

		public Tests01A(MainWindow win1)
		{
			win = win1;

			opCodes = new EqComponentList(eqPartCodes2);
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#region main routines

		internal void splitTest10()
		{
			win.WriteLine("split test 10");
			win.WriteLine("splitting| ");

			string[] patt = new string[10];

			int numLoops = 1;
			int numstrings = 500;

			int countLoops;
			int countTests;
			int m = 0;
			int groupCountTotalLoops;

			// // test about 1.0094 sec for 1000 loops	**v																																											** v
			// patt[m++] =
			// 	@"(?<l0>(?>(?<=^|[-+'""*=()&\/])(?>[\t ]*[-+]?))(?>(?>(?>(?>\d+ )?(?>\d+\/\d+['""])))|(?>(?>\d+'-(?>(?>\d+)?(?> \d+\/\d+""|\d+(?>\.\d*""|"")))))|(?>\d+(?>\.\d*""|""))|(?>(?>\d+\.\d*['""]))|(?>(?>\d+['""]))))|(?<f1>[a-zA-Z]\w*(?=\())|(?<s1>\"".+?\"")|(?<d1>(?>(?>(?<=^|[-+'""*=()&\/])(?>[\t ]*[-+]?))\d+\.\d*))|(?<n1>(?>(?<=^|[-+'""*=()&\/])(?>[\t ]*[-+]?))(?>\d+(?!\.)))|(?<op1>\<[oO][rR]\>|\<[aA][nN][dD]\>|\+|\-|&|<=|>=|<|>|==|!=|\*|\/)|(?<eq>=)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<x1>[^ ])";

			// test 1 about 0.720 sec for 1000 loops  **v																																											** v
			patt[m++] =
				@"(?<l1>(?>(?<=^[\t ]*|[-+'""*=()&\/][\t ]*)[-+]?)(?>(?>(?>(?>\d+ )?(?>\d+\/\d+['""])))|(?>(?>\d+'-(?>(?>\d+)?(?> \d+\/\d+""|\d+(?>\.\d*""|"")))))|(?>\d+(?>\.\d*""|""))|(?>(?>\d+\.\d*['""]))|(?>(?>\d+['""]))))|(?<f1>[a-zA-Z]\w*(?=\())|(?<s1>\"".+?\"")|(?<d1>(?>(?>(?<=^[\t ]*|[-+'""*=()&\/][\t ]*)[-+]?)\d+\.\d*))|(?<n1>(?>(?<=^[\t ]*|[-+'""*=()&\/][\t ]*)[-+]?)(?>\d+(?!\.)))|(?<op1>\<[oO][rR]\>|\<[aA][nN][dD]\>|\+|\-|&|<=|>=|<|>|==|!=|\*|\/)|(?<eq>=)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<x1>[^ ])";

			// test 2  about 0.655 sec for 1000 loops  **v																																											** v
			// final "approved" pattern
			patt[m++] =
				@"(?<l2>(?>(?<=^[\t ]*|[-+'""*=()&\/][\t ]*)[-+]?)(?>(?>(?>(?>\d+ )?(?>\d+\/\d+['""])))|(?>(?>\d+'-(?>(?>\d+)?(?> \d+\/\d+""|\d+(?>\.\d*""|"")))))|(?>\d+(?>\.\d*""|""))|(?>(?>\d+\.\d*['""]))|(?>(?>\d+['""]))))|(?<d1>(?>(?>(?<=^[\t ]*|[-+'""*=()&\/][\t ]*)[-+]?)\d+\.\d*))|(?<n1>(?>(?<=^[\t ]*|[-+'""*=()&\/][\t ]*)[-+]?)(?>\d+(?!\.)))|(?<f1>[a-zA-Z]\w*(?=\())|(?<s1>\"".+?\"")|(?<op1>\<[oO][rR]\>|\<[aA][nN][dD]\>|\+|\-|&|<=|>=|<|>|==|!=|\*|\/)|(?<eq>=)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<x1>[^ ])";

			// // test 3 about 0.954 sec for 1000 loops  **v																																							** v
			// patt[m++] =
			// 	@"(?<l3>(?>(?<=^|[-+'""*=()&])[\t ]*[-+]?)(?>(?<l1>(?>(?>(?>\d+ )?(?>\d+\/\d+['""])))|(?>(?>\d+'-(?>(?>\d+)?(?> \d+\/\d+""|\d+(?>\.\d*""|"")))))|(?>\d+(?>\.\d*""|"")))|(?>(?>\d+\.\d*['""]))|(?<d1>\d+\.\d*)|(?>(?>\d+['""]))|(?<n1>\d+(?!\.))))|(?<f1>[a-zA-Z]\w*(?=\())|(?<s1>\"".+?\"")|(?<op1>\<[oO][rR]\>|\<[aA][nN][dD]\>|\+|\-|&|<=|>=|<|>|==|!=|\*|\/)|(?<eq>=)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<x1>[^ ])";
			//
			// // test 4 about  0.725 sec for 1000 loops  *v																																											** v
			// patt[m++] =
			// 	@"(?<l4>(?>(?<=^|[-+'""*=()&\/])(?>[\t ]*[-+]?))(?>(?>(?>(?>\d+ )?(?>\d+\/\d+['""])))|(?>(?>\d+'-(?>(?>\d+)?(?> \d+\/\d+""|\d+(?>\.\d*""|"")))))|(?>\d+(?>\.\d*""|""))|(?>(?>\d+\.\d*['""]))|(?>(?>\d+['""]))))|(?<d1>(?>(?>(?<=^|[-+'""*=()&\/])(?>[\t ]*[-+]?))\d+\.\d*))|(?<n1>(?>(?<=^|[-+'""*=()&\/])(?>[\t ]*[-+]?))(?>\d+(?!\.)))|(?<f1>[a-zA-Z]\w*(?=\())|(?<s1>\"".+?\"")|(?<op1>\<[oO][rR]\>|\<[aA][nN][dD]\>|\+|\-|&|<=|>=|<|>|==|!=|\*|\/)|(?<eq>=)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<x1>[^ ])";


			string[] test = new string[50];

			int[] matchCount = new int[m];
			string[][] result = new string[m][];

			for (int i = 0; i < m; i++)
			{
				result[i] = new string[numstrings];

			}

			int k = 0;

			test[k++] = @"=(1 + (21 + 22 + (31 + 32 * (41 + 42) * (sign(51+52) ) ) ) + 2*3) + 4+5 + {[A1]} + ([{A1]}) & ""text""";
			test[k++] = @"{!a}= 223 (-123 + (+123 / 123 + (123 + (123 + 156) + 456)) + 456) + {[A1]} & ""text"" + ""tex() & t"" d  {!afdfd} + 100";
			test[k++] = @"=(1 + (21 + 22 + (31 + 32 * (41  -42) * Sign(51 ++52) ) ) + 2*3) asfasf x <= y + - z > a t <= u  m != n + 1/2";
			test[k++] = @"= 2 <or> 3 < 4 <and> 5 * f();";
			test[k++] = @"=1.05";
			test[k++] = @"=-1.05 * 1.05 + 123 + +1. ++1.05";
			test[k++] = @"= 100/1.05";
			test[k++] = @"1' + -2' + +3'";
			test[k++] = @"4"" +5"" +-6""";
			test[k++] = @"1.5' + +2.5'+ -2.5'";
			test[k++] = @"4.5"" + +5.5"" * -6.5""";
			test[k++] = @"= 1'-0"" + -2'-0"" * +3'-0""";
			test[k++] = @"1'-1.5"" + -2'-1.5"" * +3'-1.5""";
			test[k++] = @"1.1'-5"" + -2.1'-6.5"" *+3.1'-6.5""";
			test[k++] = @"1/2""  + +1/2"" -1/2""";
			test[k++] = @" 1/2' + -1/2' +1/2'";
			test[k++] = @"  +1/2""";
			test[k++] = @"    1/2"" + -1/2' + -4 1/2' - +4.5'";
			test[k++] = @"=-5'-6.5"" +600' * 700.05' + -fun(3*4) + d";

			Stopwatch s = new Stopwatch();

			for (int j = 0; j < m; j++)
			{
				countLoops = 0;
				countTests = 0;
				groupCountTotalLoops = 0;

				win.WriteLine("\n*** start loop| " + j);

				s.Reset();
				s.Start();

				Regex r = new Regex(patt[j], RegexOptions.Compiled | RegexOptions.ExplicitCapture);

				for (int l = 0; l < numLoops; l++)
				{
					countLoops++;
					for (int i = 0; i < k; i++)
					{
						MatchCollection c = r.Matches(test[i]);

						groupCountTotalLoops += countMatches(c);
						matchCount[j] += getMatches(c, result[j], matchCount[j]);
						countTests++;
					}
				}

				s.Stop();

				double secs = s.Elapsed.TotalSeconds;

				win.WriteLine("\n\n*** loop #| " + j + "  loop count| " + countLoops + "  total tests| " + countTests);

				win.WriteLine("time for patt| " + secs.ToString("N6") + " seconds");
				win.WriteLine("time per loop| " + (secs/countLoops).ToString("N6") + " seconds");
				win.WriteLine("time per test| " + (secs/countTests).ToString("N6") + " seconds");

				win.WriteLine("total groups found| " + groupCountTotalLoops.ToString("N0"));
				win.WriteLine("avg groups per loop| " + (groupCountTotalLoops/countLoops).ToString("N4"));
				win.WriteLine("pattern| " + patt[j]);
				win.WriteLine("\n\n");

			}

			showMatches(result, matchCount[0], matchCount[1]);
		}

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

		private void showMatches(string[][] s, int s0Count, int s1Count)
		{

			win.WriteLine("\nshow pattern test|");

			int cntSmaller;
			int posSmaller = 0;
			string[] sSmaller;

			int cntLarger;
			int posLarger = 0;
			string[] sLarger;

			if (s0Count < s1Count)
			{
				sSmaller = s[0];
				sLarger = s[1];
				cntSmaller = s0Count;
				cntLarger = s1Count;
			}
			else
			{
				sSmaller = s[1];
				sLarger = s[0];
				cntSmaller = s1Count;
				cntLarger = s0Count;
			}

			string smaller;
			string larger;

			win.WriteLine($"small count| {cntSmaller,-4:D}  larger count| {cntLarger,-4:D}\n");


			for (int i = 0; i < cntLarger; i++)
			{

				// while (string.IsNullOrWhiteSpace(sSmaller[posSmaller]) || !sSmaller[posSmaller].Equals(sLarger[posLarger]))
				// {
				// 	win.WriteLine($"smaller| {posSmaller,-4:D}| larger| {posLarger,-4:D}| value| {sLarger[posLarger]} - does not match");
				// 	posSmaller++;
				// }

				smaller = string.IsNullOrWhiteSpace(sSmaller[posSmaller]) ? "empty" : sSmaller[posSmaller];
				larger = string.IsNullOrWhiteSpace(sLarger[posSmaller]) ? "empty" : sLarger[posSmaller];

				win.WriteLine($"smaller| {posSmaller,-4:D}| \t{smaller,-15} \tlarger| {posLarger,-4:D}| \t{larger,-15}");

				posSmaller++;
				posLarger++;

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
						matches[matchCount++] = $"{g.Name,-5}| {g.Value}";
					}
				}
			}

			return matchCount - start;
		}


		internal void splitTest9()
		{
			win.WriteLine("split test 9A");
			win.WriteLine("splitting| ");

			// string patt = @"(?<eq>=)|(?<f1>[a-zA-Z]\w+?\(.+?\))|(?<s1>\"".+?\"")|(?<op1>\+|\-)|(?<op2>\\|\*)|(?<op3>&)|(?<p1>\(|\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<n1>\d+)|(?<x1>.*?)";
			//string patt = @"(?<eq>=)|(?<f1>[a-zA-Z]\w+?\(.+?\))|(?<s1>\"".+?\"")|(?<op1>\+|\-)|(?<op2>\\|\*)|(?<op3>&)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<n1>\d+)";
			// string patt = @"(?<eq>=)|(?<f1>[a-zA-Z]\w+(?=\())|(?<s1>\"".+?\"")|(?<op1>\+|\-)|(?<op2>\\|\*)|(?<op3>&)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<n1>\d+)";
			string patt =
				@"(?<f1>[a-zA-Z]\w+(?=\())|(?<s1>\"".+?\"")|(?<n1>-?\d+)|(?<op1>\+|\-|&|<=|>=|<|>|==|!=)|(?<op2>\*|\\)|(?<eq>=)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)";

			Regex r = new Regex(patt, RegexOptions.Compiled);

			string[] test = new string[15];

			int k = 0;

			test[k++] = "=(1 + (21 + 22 + (31 + 32 * (41 + 42) * (sign(51+52) ) ) ) + 2*3) + 4+5 + {[A1]} + ([{A1]}) & \"text\"";
			test[k++] = "= 123 (456+(123 + (678 + (123 + 123 + (123 + (123 + 456) + 456)) + 456) + 246))";
			// test[k++] = "= 123 (123 + (123 + 123 + (123 + (123 + 456) + 456)) + 456)";
			// test[k++] = "= 123 ((123 + 567) + (678 + 891) + (123 + 123 + (123 + (123 + 456) + 456)) + 456) + 246";
			// test[k++] = "= 123 ((123 + 567) + 246";
			// test[k++] = "= 123 (123 + 567)) + 246";
			// test[k++] = "= 123 ((((123 + 567) + 246";
			// test[k++] = "= 123 (123 + 567)))) + 246";

			// makePartCodes(EQ_CODE_PARTS_MAX);
			// listCodeParts();

			// testClassifyCodePart();

			makePartCodes2(EQ_CODE_PARTS_MAX);
			// listCodeParts2();
			// testClassifyCodePart2();

			// return;

			for (int i = 0; i < test.Length; i++)
			{
				if (test[i] == null) break;

				win.Write("\n\n");
				win.WriteLine("testing| " + i + "| length| " + test[i].Length);

				win.WriteLine("0---  0---1--- 10---2--- 20---3--- 30---4--- 40---5--- 50---6--- 60---7--- 70---8--- 80---0--- 90---1");
				win.WriteLine("0123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|");
				win.WriteLine(test[i]);
				win.Write("\n");


				// key = sort order
				// tupple = level (int), op (string), value (string) 
				List<List<Eq1>>  result = parseEq(test[i], r);

				if (result != null)
				{
					// showEqAllLevels2(result);
					showEqList(result);

					win.WriteLine("split test 9| idx| " + i + " (worked)");
					win.Write("\n");
				}
				else
				{
					win.WriteLine("split test 9| idx| " + i + " (failed)");
					win.Write("\n");
				}
			}
		}

	#endregion

	#region parse eq part A

		private List<List<Eq1>>  parseEq(string eq, Regex r)
		{
			Dictionary<string, Eq1> result = new Dictionary<string, Eq1>();

			MatchCollection c = r.Matches(eq);

			if (c.Count < 1)
			{
				win.WriteLine("*** MATCH FAILED ***");
				return null;
			}

			win.WriteLine("collection found| " + c.Count + " matches");

			// List<Eq1> t = orgRxString(c);
			List<List<Eq1>> t1 = orgRxString2(c);

			return t1;
		}

	#endregion

	#region parse eq part B

		private List<List<Eq1>> orgRxString2(MatchCollection c)
		{
			List<List<Eq1>> result = new List<List<Eq1>>();
			result.Add(new List<Eq1>());

			string grpBeg = "grpBeg";
			int grpBegCode =  classifyCodePartOp2B("(") - 100;

			string grpEnd = "grpEnd";
			int grpEndCode =  classifyCodePartOp2B(")") - 100;

			string grpRef = "grpRef";
			int grpRefCode =  grpBegCode - 1;

			int grp = 0;
			int level = 0;
			string k;
			Eq1 t;

			Match m;

			string opMatchList1 = "op1|op2|pdn|pup";


			for (var i = 0; i < c.Count; i++)
			{
				m = c[i];

				for (var j = 1; j < m.Groups.Count; j++)
				{
					Group g = m.Groups[j];

					if (g.Success)
					{
						if (g.Name == "pdn")
						{
							result[level].Add(new Eq1(level, grpRef, grp.ToString("D3"), grpRefCode));
							level++;
							if (result.Count == level)
							{
								result.Add(new List<Eq1>());
							}

							result[level].Add(new Eq1(level, grpBeg, grp++.ToString("D3"), grpBegCode));
						}

						t = new Eq1(level, g.Name, g.Value);

						if (opMatchList1.IndexOf(g.Name) >= 0)
						{
							t.SortCode = classifyCodePartOp2B(g.Value);
						}

						result[level].Add(t);

						if (g.Name == "pup")
						{
							result[level].Add(new Eq1(level, grpEnd, "", grpEndCode));
							level--;
						}
					}
				}
			}

			return result;
		}

		private List<Eq1> orgRxString(MatchCollection c)
		{
			List<Eq1> result = new List<Eq1>();

			string grpRef = "grpRef";
			string grpBeg = "grpBeg";
			string grpEnd = "grpEnd";
			int grp = 0;
			int level = 0;
			string k;
			Eq1 t;


			foreach (Match m in c)
			{
				for (var i = 1; i < m.Groups.Count; i++)
				{
					Group g = m.Groups[i];

					if (g.Success)
					{
						if (g.Name == "pdn")
						{
							result.Add(new Eq1(level, grpRef, grp.ToString("D3")));
							level++;
							result.Add(new Eq1(level, grpBeg, grp++.ToString("D3")));
						}

						t = new Eq1(level, g.Name, g.Value);
						result.Add(t);

						if (g.Name == "pup")
						{
							result.Add(new Eq1(level, grpEnd, ""));
							level--;
						}
					}
				}
			}

			return result;
		}

	#endregion

	#region Utilities

	#region code part 1

		// "regex pattern code" "description" {index} {id/sort code} {width}
		private Tuple<string, string, int>[] eqPartCodes = new []
		{
			new Tuple<string, string, int>(@"<or>♦", "Logical or"   , 200),
			new Tuple<string, string, int>(@"<and>♦", "Logical and" , 300),
			new Tuple<string, string, int>(@"==♦!=♦", "equality"    , 400),
			new Tuple<string, string, int>(@"<♦>♦", "relational"    , 500),
			new Tuple<string, string, int>(@"<=♦=>♦", "relational"  , 550),
			new Tuple<string, string, int>(@"+♦-♦", "additive"      , 600),
			new Tuple<string, string, int>(@"&♦", "additive string" , 700),
			new Tuple<string, string, int>(@"*♦\♦", "Multiplicative", 800),
			new Tuple<string, string, int>(@"(♦)♦", "grouping"      , 10000),
		};


		private const int EQ_CODE_WIDTH = 5;
		private const int EQ_CODE_PARTS_MAX = 20;
		private string eqPartCodeStr;
		private int[] eqPartCodeIdx;
		private Tuple<int, int>[] eqPartCodeIdx2;

		private void makePartCodes(int len)
		{
			int j = 0;
			int k = 0;
			int before;
			eqPartCodeIdx = new int[len];

			for (var i = 0; i < eqPartCodes.Length; i++)
			{
				before = 0;

				j = eqPartCodes[i].Item1.IndexOf('♦', before);

				while (j > 0)
				{
					eqPartCodeStr += $"{(eqPartCodes[i].Item1.Substring(before, j - before)),-EQ_CODE_WIDTH}♦";
					eqPartCodeIdx[k++] = i;

					before = j + 1;

					j = eqPartCodes[i].Item1.IndexOf('♦', before);
				}

				while (j > 0) ;
			}
		}

		private void testClassifyCodePart()
		{
			win.WriteLine("test classify| ");
			win.Write("\n");

			string[] test = new [] {"*", "+", "&", "<=", "!="};

			foreach (string s in test)
			{
				int idx = classifyCodePartOp(s);

				if (idx >= 0)
				{
					win.Write("part| " + $"{s,-8}");
					win.Write($"code| {(eqPartCodes[idx].Item3),-8:0}");
					win.Write($"desc| {(eqPartCodes[idx].Item2)}");
					win.Write("\n");
				}
			}
		}

		private int classifyCodePartOp(string op)
		{
			string test = $"{op,-EQ_CODE_WIDTH}♦";
			int j = eqPartCodeStr.IndexOf(test);

			if (j < 0)
			{
				return -1;
			}

			int idx1 = j / (EQ_CODE_WIDTH + 1);

			return eqPartCodeIdx[idx1];
		}


		private void listCodeParts()
		{
			win.WriteLine("listCodeParts| start");
			int j;
			int idx1;
			int idx2;
			int before = 0;
			string part;

			j = eqPartCodeStr.IndexOf('♦', before);

			while (j > 0)
			{
				idx1 = j / (EQ_CODE_WIDTH + 1);
				idx2 = eqPartCodeIdx[idx1];
				part = eqPartCodeStr.Substring(before, j - before).Trim();

				win.Write("part| " + $"{part,-8}");
				win.Write($"code| {(eqPartCodes[idx2].Item3),-8:0}");
				win.Write($"desc| {(eqPartCodes[idx2].Item2)}");

				win.Write("\n");

				before = j + 1;
				j = eqPartCodeStr.IndexOf('♦', before);
			}
		}

	#endregion

	#region code parts 2

		// "description", "matched op code", ##:sort code
		private int classifyCodePartOp2B(string op)
		{
			string test = $"{op,-EQ_CODE_WIDTH}♦";
			int j = eqPartCodeStr.IndexOf(test);

			if (j < 0)
			{
				return -1;
			}

			int idx1 = j / (EQ_CODE_WIDTH + 1);

			Tuple<int, int> t = eqPartCodeIdx2[idx1];

			return eqPartCodes2[t.Item1].Item2[t.Item2].Item2;
		}


		// "description", "matched op code", ##:sort code
		private Tuple<string, string, int> classifyCodePartOp2A(string op)
		{
			string test = $"{op,-EQ_CODE_WIDTH}♦";
			int j = eqPartCodeStr.IndexOf(test);

			if (j < 0)
			{
				return null;
			}

			int idx1 = j / (EQ_CODE_WIDTH + 1);

			Tuple<int, int> t = eqPartCodeIdx2[idx1];

			return new Tuple<string, string, int>(eqPartCodes2[t.Item1].Item1,
				eqPartCodes2[t.Item1].Item2[t.Item2].Item1, eqPartCodes2[t.Item1].Item2[t.Item2].Item2 );
		}

		// data for code parts
		private Tuple<string, Tuple<string, int>[]>[] eqPartCodes2 = new []
		{
			new Tuple<string, Tuple<string, int>[]>("logical or",  new [] {new Tuple<string, int>("<or>", 101)}),
			new Tuple<string, Tuple<string, int>[]>("logical and", new [] {new Tuple<string, int>("<and>", 151)}),
			new Tuple<string, Tuple<string, int>[]>("equality",    new []
			{
				new Tuple<string, int>("==", 201),
				new Tuple<string, int>("!=", 202),
			}),
			new Tuple<string, Tuple<string, int>[]>("relational",  new []
			{
				new Tuple<string, int>("<", 251),
				new Tuple<string, int>(">", 252), new Tuple<string, int>("<=", 253), new Tuple<string, int>(">=", 254),
			}),
			new Tuple<string, Tuple<string, int>[]>("additive string", new [] {new Tuple<string, int>("&", 301)}),
			new Tuple<string, Tuple<string, int>[]>("additive",    new []
			{
				new Tuple<string, int>("+", 351),
				new Tuple<string, int>("-", 352),
			}),
			new Tuple<string, Tuple<string, int>[]>("Multiplicative", new []
			{
				new Tuple<string, int>("*", 401),
				new Tuple<string, int>("\\", 402),
			}),
			new Tuple<string, Tuple<string, int>[]>("grouping", new []
			{
				new Tuple<string, int>("(", 901),
				new Tuple<string, int>(")", 902),
			}),
		};


		private  Tuple<string, Tuple<string, int>[]>[] getCompCodes()
		{
			Tuple<string, Tuple<string, int>[]>[] eqCompCodes2 = new []
			{
				new Tuple<string, Tuple<string, int>[]>("assignment",  new [] {new Tuple<string, int>("eq", 11)}),
				new Tuple<string, Tuple<string, int>[]>("number",
					new []
					{
						new Tuple<string, int>("n1", 21),
						new Tuple<string, int>("d1", 22),
						new Tuple<string, int>("l1", 23),
					}),
				new Tuple<string, Tuple<string, int>[]>("variable",
					new [] {new Tuple<string, int>("v1", 31), new Tuple<string, int>("v2", 32)}),
				new Tuple<string, Tuple<string, int>[]>("word",  new [] {new Tuple<string, int>("w1", 41)}), // could be "true" or "false"
				new Tuple<string, Tuple<string, int>[]>("string",  new [] {new Tuple<string, int>("s1", 51)}),
				new Tuple<string, Tuple<string, int>[]>("error",  new [] {new Tuple<string, int>("x1", 51)}),
				new Tuple<string, Tuple<string, int>[]>("function",  new [] {new Tuple<string, int>("f1", 91)}),
			};


			return eqCompCodes2;
		}


		// configure code parts
		private void makePartCodes2(int len)
		{
			int k = 0;
			eqPartCodeIdx2 = new Tuple<int, int>[len];

			for (int i = 0; i < eqPartCodes2.Length; i++)
			{
				Tuple<string, int>[] codes = eqPartCodes2[i].Item2;

				for (int j = 0; j < codes.Length; j++)
				{
					eqPartCodeStr += $"{(codes[j].Item1),-EQ_CODE_WIDTH}♦";
					eqPartCodeIdx2[k++] = new Tuple<int, int>(i, j);
				}
			}
		}


	#region not used

		private void listCodeParts2()
		{
			win.WriteLine("listCodeParts| start");

			foreach (Tuple<int, int> t in eqPartCodeIdx2)
			{
				if (t == null) break;

				Tuple<string, Tuple<string, int>[]> tx = eqPartCodes2[t.Item1];

				Tuple<string, int> tz = tx.Item2[t.Item2];

				win.Write("part| " + $"{tz.Item1,-8}");
				win.Write($"code| {(tz.Item2),-8:0}");
				win.Write($"desc| {(tx.Item1)}");

				win.Write("\n");
			}
		}

		private void testClassifyCodePart2()
		{
			win.WriteLine("test classify2| ");
			win.Write("\n");

			string[] test = new [] {"*", "+", "&", "<=", "!="};

			foreach (string s in test)
			{
				Tuple<string, string, int> t = classifyCodePartOp2A(s);

				if (t != null)
				{
					win.Write("part| " + $"{(">" + s + "<"),-8}");
					win.Write($"str|  {(">" + t.Item2 + "<"),-8:0}");
					win.Write($"code| {(t.Item3),-8:0}");
					win.Write($"desc| {(t.Item1)}");
					win.Write("\n");
				}
			}
		}

	#endregion

	#endregion

	#region list info

		private void showEqList(List<List<Eq1>> eq)
		{
			win.showTabId = false;

			win.WriteLine("show eq list| ");
			win.TabUp();

			int item = 0;
			int level = 0;

			foreach (List<Eq1> tList in eq)
			{
				win.WriteLineTab($"for level| {level++,3:###}" );

				win.TabUp();
				foreach (Eq1 t in tList)
				{
					win.WriteTab($"level| {t.Level,-4:##}");
					win.Write($"item| {item++,-5:###}");
					win.Write($"op| {t.EqCode,-12}");
					win.Write($"val| " + t.Value.PadRight(10));
					win.Write($"sort| {t.SortCode:##0}");
					win.Write("\n");
				}

				win.TabDn();
			}

			win.TabDn();
		}


		private void showEqAllLevels2(List<List<Eq1>> eq)
		{
			win.WriteLine("show eq by level| ");

			int level = 0;

			foreach (List<Eq1> tList in eq)
			{
				string es = getEqForLevel2(tList);
				win.WriteLine("for level| >" + level.ToString("4:0") + "< " + es);

				level++;
			}
		}

		private string getEqForLevel2(List<Eq1> eq)
		{
			Eq1 t;
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < eq.Count; i++)
			{
				t = eq[i];

				sb.Append("[").Append(t.EqCode).Append(":").Append(t.Value).Append("]");

				if (i != eq.Count - 1) sb.Append(" ");
			}

			return sb.ToString();
		}

		private void showEqAllLevels(List<Eq1> eq)
		{
			win.WriteLine("show eq by level| ");

			for (int level = 0; level < MAX_EQ_DEPTH; level++)
			{
				string es = getEqForLevel(eq, level);

				if (string.IsNullOrWhiteSpace(es)) break;

				win.WriteLine("for level| >" + level.ToString("4,0") + "< " + es);
			}
		}

		private string getEqForLevel(List<Eq1> eq, int level)
		{
			Eq1 t;
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < eq.Count; i++)
			{
				t = eq[i];

				if (t.Level != level) continue;

				sb.Append("[").Append(t.EqCode).Append(":").Append(t.Value).Append("]");

				if (i != eq.Count - 1) sb.Append(" ");
			}

			return sb.ToString();
		}

		private void showEqListItem(Eq1 item)
		{
			win.Write("item| level| " + item.Level.ToString("D3").PadRight(5));
			win.Write("code| " + item.EqCode.PadRight(5));
			win.Write("value| " + item.Value);
			win.Write("\n");
		}

	#endregion


		private string makeEqKey(int level, int order, int op, int seq)
		{
			return level.ToString("D3") + ":" + order.ToString("D3") + ":" + op.ToString("D3") + ":" + seq.ToString("D3");
		}

	#endregion

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is Tests01A";
		}

	#endregion
	}
}