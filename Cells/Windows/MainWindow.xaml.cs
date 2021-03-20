#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using SharedCode.FormulaSupport;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitParamManagement;
using SpreadSheet01.RevitSupport.RevitParamValue;
using UtilityLibrary;

#endregion

// projname: Cells
// itemname: MainWindow
// username: jeffs
// created:  2/28/2021 5:54:07 AM

namespace Cells.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
	#region private fields

		// public RevitParamTest RevitParamTests { get; } = new RevitParamTest();

		public RevitCellSystManager RevitSystMgr { get; } = new RevitCellSystManager();

		private static MainWindow me;

	#endregion

	#region ctor

		public MainWindow()
		{
			me = this;

			InitializeComponent();
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		private static int tabs = 0;
		private static string tabId = null;

		public static bool showTabId = true;

		private static void listTabId()
		{
			if (tabId != null && showTabId)
			{
				WriteLine(tabId);
			}
		}

		public static  void TabUp(int id = -1)
		{
			tabs++;
			tabId = id.ToString("D3") + " up";
			listTabId();
		}

		public static void TabDn(int id = -1)
		{
			tabs--;
			tabId = id.ToString("D3") + " dn";
			listTabId();
		}

		public static void TabClr(int id = -1)
		{
			tabs = 0;
			tabId = id.ToString("D3") + " clr";
			listTabId();
		}

		public static void TabSet(int t)
		{
			tabs = t;
		}

		public static void WriteLineTab(string msg)
		{
			WriteTab(msg + "\n");
		}

		public static void WriteTab(string msg)
		{
			me.TbxMsg.Text +=
				// (tabId > 0 ? tabId.ToString("D3") : "") + 
				(tabs > 0 ? "    ".Repeat(tabs) : "" ) + msg;
		}

		public static void WriteLine(string msg)
		{
			Write(msg + "\n");
		}

		public static void Write(string msg)
		{
			me.TbxMsg.Text += msg;
		}

	#endregion

	#region private methods

		private void splitTest2()
		{
			//(?<E>=)|(?<eq>.*?)(?<v>\{(\[(?<ca>(([a-wA-W]{0,1}[a-zA-Z]{1,2}|[xX][a-eA-E][a-zA-Z]|[xX][fF][a-dA-D])[1-9]\d{0,6}))\]|\$(?<sv>.+?)|\@(?<ln>.+?)|\!(?<gp>.+?)|\%(?<pp>.+?)|\#(?<rp>.+?))\})|(?<eq>.+$)

			//                 (?<E>=)|(?<eq>.*?)(?<v>\{(\[(?<ca>.+?)\]|\$(?<sv>.+?)|\@(?<ln>.+?)|\!(?<gp>.+?)|\%(?<pp>.+?)|\#(?<rp>.+?))\})|(?<eq>.+$)
			//(?<E>=)|(?<eq>.*?)(?<v>\{(\[(?<ca>(?i:[a-z]{1,3}[1-9]\d{0,6}))\]|\$(?<sv>.+?)|\@(?<ln>.+?)|\!(?<gp>.+?)|\%(?<pp>.+?)|\#(?<rp>.+?))\})|(?<eq>.+$)
			string pattern = @"(?<E>=)|(?<eq>.*?)(?<v>\{(\[(?<ca>(?i:[a-z]{1,3}[1-9]\d{0,6}))\]|\$(?<sv>.+?)|\@(?<ln>.+?)|\!(?<gp>.+?)|\%(?<pp>.+?)|\#(?<rp>.+?))\})|(?<eq>.+$)";
			string input = @"= {[a1]}
={[A1]}
={[ a1]}
={[ a1 ]}
=1 + {[aaa12345]} asdf 
={[ aaa12345]}
={[ A112345 ]}
={@Bx4}+""this time""+{@Bx4} + asf +{[x3} dfdfd + asf
={!Bx4}+""this time""+{!Bx4} + asf +{[x3} dfdfd
={%Bx2}+""this time""+{%Bx1} + asf +{[x1]} dfdfd
={#Bx1}+""this time""+{#Bx1} + asf +{[x1]} dfdfd
={[A1]} +""this time""+ {[B1]} +""this time""+ {[C1]}   
={[A1]} +""this time""+ {$variable} +""this time""+ {[C1]} + {$} dfadsf 
= asf asdf asf + {[A1]} +""this time""+ {$variable} +""this time""+ {[C1]} + {$} dfadsf ";
			RegexOptions options = RegexOptions.Multiline|RegexOptions.ExplicitCapture;

			foreach (Match m in Regex.Matches(input, pattern, options))
			{
				WriteLine(string.Format("'{0}' found at index {1}.", m.Value, m.Index));
			}
		}



		private static class rx
		{
			static string patt = @"(?<E>=)|(?<eq>.*?)(?<v>\{(?:\[(?<ca>(?i:[a-z]{1,3}[1-9]\d{0,6}))\]|\$(?<sv>.+?)|\@(?<ln>.+?)|\!(?<gp>.+?)|\%(?<pp>.+?)|\#(?<rp>.+?))\})|(?<eq>.+$)";

			static Regex r = new Regex(patt, RegexOptions.ExplicitCapture);

			public static MatchCollection MatchFormulaForCellVar(string formula)
			{
				return r.Matches(formula);
			}

			public static List<KeyValuePair<string, string>> ParseFormula(string formula)
			{
				MatchCollection mc = MatchFormulaForCellVar(formula);

				List<KeyValuePair<string, string>> varList = new List<KeyValuePair<string, string>>();

				foreach (Match m in mc)
				{
					foreach (Group g in m.Groups)
					{
						if (!g.Value.IsVoid() && g.Index > 0)
						{
							varList.Add(new KeyValuePair<string, string>(g.Name, g.Value));
						}
					}
				}

				return varList;
			}

		}
		
		private void splitTest5()
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


		private void splitTest6()
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




		// xfd
		private void splitTest3()
		{
			WriteLine("split test");
			WriteLine("splitting| ");

			string[] test = new string[15];

			// test[0] = "= asf asdf asf + {[A1]} +\"this time\"+ {$variable} +\"this time\"+ {[C1]} + {$} dfadsf ";
			// test[1] = "=#[bx1]+\"this time\"+#[bx2] + asf +{[x1]} dfdfd";
			// test[2] = "=%[bx4]+\"this time\"+%[bx5] + asf +{[x1]} dfdfd";
			// test[3] = "=![Bx4]+\"this time\"+![Bx4] + asf +{[x3} dfdfd";
			// test[4] = "#[cx1]=@[Bx4]+\"this time\"+@[Bx4] + asf +{[x3} dfdfd + asf";
			// test[5] = "#[dx1)]=@[Bx4]";
			// test[6] = "=#[ex1]";
			// test[7] = "=![A1]";
			// test[8] = "=%[bx6]";
			// test[9] = "=@[A1]";
			// test[10] = "=[$A1]";
			// test[11] = "=~[A1]";
			// test[12] = "=@[A1]";
			// test[13] = "=[$A1]";

			// (?<E>=)|(?<eq>.*?)(?<v>\{(\[(?<ca>(([a-wA-W]{0,1}[a-zA-Z]{1,2}|[xX][a-eA-E][a-zA-Z]|[xX][fF][a-dA-D])[1-9]\d{0,6}))\]|\$(?<sv>.+?)|\@(?<ln>.+?)|\!(?<gp>.+?)|\%(?<pp>.+?)|\#(?<rp>.+?))\})|(?<eq>.+$)
			// (?<E>=)|(?<eq>.*?)(?<v>\{(\[(?<ca>.+?)\]|\$(?<sv>.+?)|\@(?<ln>.+?)|\!(?<gp>.+?)|\%(?<pp>.+?)|\#(?<rp>.+?))\})|(?<eq>.+$)
			// (?<E>=)|(?<eq>.*?)(?<v>\{(\[(?<ca>(?i:[a-z]{1,3}[1-9]\d{0,6}))\]|\$(?<sv>.+?)|\@(?<ln>.+?)|\!(?<gp>.+?)|\%(?<pp>.+?)|\#(?<rp>.+?))\})|(?<eq>.+$)
			// string patt = @"(?<E>=)|(?<eq>.*?)(?<v>\{(\[(?<ca>.+?)\]|\$(?<sv>.+?)|\@(?<ln>.+?)|\!(?<gp>.+?)|\%(?<pp>.+?)|\#(?<rp>.+?))\})|(?<eq>.+$)";

			//final:       (?<E>=)|(?<eq>.*?)(?<v>\{(\[(?<ca>(?i:[a-z]{1,3}[1-9]\d{0,6}))\]|\$(?<sv>.+?)|\@(?<ln>.+?)|\!(?<gp>.+?)|\%(?<pp>.+?)|\#(?<rp>.+?))\})|(?<eq>.+$)
			// string patt@"(?<E>=)|(?<eq>.*?)(?<v>\{(\[(?<ca>(?i:[a-z]{1,3}[1-9]\d{0,6}))\]|\$(?<sv>.+?)|\@(?<ln>.+?)|\!(?<gp>.+?)|\%(?<pp>.+?)|\#(?<rp>.+?))\})|(?<eq>.+$)";

			// string patt = @"(?<E>=)|(?<eq1>.*?)(?<v>(?:\@\[(?<ca>(?i:[a-z]{1,3}[1-9]\d{0,6}))|\#\[(?<cb>(?<id>(?i:[a-z]+[a-z1-9 ]{1,16})))|\!\[(?<cc>(?P>id))|\$\[(?<cd>(?P>id))|\%\[(?<ce>(?P>id))|\~\[(?<cf>(?P>id)))\])|(?<eq2>.+$)";

			// string patt = @"(?<E>=)|(?<eq>.*?)(?<v>(?:\@\[(?<ca>(?i:[a-z]{1,3}[1-9]\d{0,6}))|\#\[(?<cb>(?<id>(?i:[a-z]+[a-z1-9 ]{1,16})))|\!\[(?<cc>.+?)|\$\[(?<cd>.+?)|\%\[(?<ce>.+?)|\~\[(?<cf>.+?))\])|(?<eq>.+$)";


			
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

			//


			string    patt = @"(?<E>=)|(?<eq>.*?)(?<v>\{(?:\[(?<ca>(?i:[a-z]{1,3}[1-9]\d{0,6}))\]|\$(?<sv>.+?)|\@(?<ln>.+?)|\!(?<gp>.+?)|\%(?<pp>.+?)|\#(?<rp>.+?))\})|(?<eq>.+$)";

			Regex r = new Regex(patt, RegexOptions.ExplicitCapture);

			// my groups
			// E, eq, v, ca, sv, ln, gp, pp, rp

			for (var i = 0; i < test.Length; i++)
			{
				if (test[i].IsVoid()) continue;

				Write("\n");
				WriteLine("testing| " + i + "| " + test[i]);
				Write("\n");

				Match m = r.Match(test[i]);
				if (!m.Success)
				{
					WriteLine("*** MATCH FAILED ***");
					continue;
				}

				string result = "";

				GroupCollection gc = m.Groups;
				CaptureCollection cc = m.Captures;

				Group g = gc[0];
				Capture c = cc[0];


				do
				{

					// WriteLine("found captures| " + m.Captures.Count);
					//
					// for (int j = 0; j <  m.Captures.Count; j++)
					// {
					// 	WriteLine("capture| " + j + "| " +  m.Captures[j].Value);
					// }

					Write("\n");

					// WriteLine("found groups| " + m.Groups.Count);

					
					

					for (int j = 1; j < m.Groups.Count; j++)
					{
						if (!m.Groups[j].Value.IsVoid())
						{
							WriteLine("found   name| " + j + "| " +  m.Groups[j].Name);
							WriteLine("found  value| " + j + "| " +  m.Groups[j].Value);

							if (m.Groups[j].Name.Equals("E") || m.Groups[j].Name.Equals("eq") || m.Groups[j].Name.Equals("v"))
							{
								result += m.Groups[j].Value + " ";
							}



							// WriteLine("found captures| " + j + "| " +  m.Groups[j].Captures.Count);

							// WriteLine("capture| " +  m.Groups[j].Captures[0].Value);
							// WriteLine("capture| " +  m.Groups[j].Captures[1].Value);

							// for (int k = 0; k < m.Groups[j].Captures.Count; k++)
							// {
							// 	WriteLine("capture| " + k + "| " +  m.Groups[j].Captures[k].Value);
							// }
						}
					}

					m = m.NextMatch();

				}
				while (m.Success);


				Write("\n");

				WriteLine(result);

				// foreach (Group g in m.Groups)
				// {
				// 	// Write("\n");
				//
				// 	Write("match| idx| " + g.Index.ToString().PadRight(4));
				// 	Write(" name| " + g.Name.PadRight(4));
				// 	Write(" value| >" + (g.Value + "<").PadRight(30));
				// 	Write(" success| " + g.Success);
				// 	Write("\n");
				// }
			}
		}

		private void splitTest4()
		{
			WriteLine("split test");
			WriteLine("splitting| ");

			string[] test = new string[5];

			test[0] = "= asf asdf asf + {[A1]} +\"this time\"+ {$variable} +\"this time\"+ {[C1]} + {$} dfadsf ";
			test[1] = "={#Bx1}+\"this time\"+{#Bx1} + asf +{[x1]} dfdfd";
			test[2] = "={%Bx2}+\"this time\"+{%Bx1} + asf +{[x1]} dfdfd";
			test[3] = "={!Bx4}+\"this time\"+{!Bx4} + asf +{[x3} dfdfd";
			test[4] = "{#Bx1}={@Bx4}+\"this time\"+{@Bx4} + asf +{[x3} dfdfd + asf";

			string patt = @"(?<E>=)|(?<eq>.*?)(?<v>\{(\[(?<ca>.+?)\]|\$(?<sv>.+?)|\@(?<ln>.+?)|\!(?<gp>.+?)|\%(?<pp>.+?)|\#(?<rp>.+?))\})|(?<eq>.+$)";

			Regex r = new Regex(patt, RegexOptions.ExplicitCapture);

			for (var i = 0; i < test.Length; i++)
			{
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

					foreach (Group g in m.Groups)
					{
						Write("\n");
						WriteLine("group| value| " + g.Value);

						foreach (Capture cp in g.Captures)
						{
							Write("match| idx| " + cp.Index.ToString().PadRight(4));
							Write(" value| >" + (cp.Value + "<").PadRight(30));
							Write("\n");
						}
					}
				}
			}
		}


		private void splitTest()
		{
			WriteLine("split test");
			WriteLine("splitting| ");

			string[] test = new string[5];

			test[0] = "= asf asdf asf + {[A1]} +\"this time\"+ {$variable} +\"this time\"+ {[C1]} + {$} dfadsf ";
			test[1] = "={#Bx1}+\"this time\"+{#Bx1} + asf +{[x1]} dfdfd";
			test[2] = "={%Bx2}+\"this time\"+{%Bx1} + asf +{[x1]} dfdfd";
			test[3] = "={!Bx4}+\"this time\"+{!Bx4} + asf +{[x3} dfdfd";
			test[4] = "{#Bx1}={@Bx4}+\"this time\"+{@Bx4} + asf +{[x3} dfdfd + asf";

			string patt = @"(?<E>=)|(?<eq>.*?)(?<v>\{(\[(?<ca>.+?)\]|\$(?<sv>.+?)|\@(?<ln>.+?)|\!(?<gp>.+?)|\%(?<pp>.+?)|\#(?<rp>.+?))\})|(?<eq>.+$)";

			Regex r = new Regex(patt, RegexOptions.Compiled);

			for (var i = 0; i < test.Length; i++)
			{
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
					foreach (Group g in m.Groups)
					{
						// Write("\n");

						Write("match| idx| " + g.Index.ToString().PadRight(4));
						Write(" name| " + g.Name.PadRight(4));
						Write(" value| >" + (g.Value + "<").PadRight(30));
						Write(" success| " + g.Success);
						Write("\n");
					}

					Write("\n");
					WriteLine("found| Captures| " + m.Groups.Count);
					foreach (Capture cp in m.Captures)
					{
						Write("match| idx| " + cp.Index.ToString().PadRight(4));
						Write(" value| >" + (cp.Value + "<").PadRight(30));
						Write("\n");
					}
				}
			}
		}

	#endregion

	#region event consuming

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@Debug");
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			WriteLineTab("Loaded...");

			bool result;

			// splitTest6();

			ParseFormula.Tests();


			return;

			// test that the params have been defined
			// RevitParamTests.Process();

			// get all of the revit chart families and 
			// process each to get its parameters
			result = RevitSystMgr.GetCurrentCharts();

			if (!result) return;

			RevitSystMgr.ProcessCharts(CellUpdateTypeCode.ALL);

			RevitSystMgr.listCharts2();

			// proess labels

			OnPropertyChange("RevitParamTests");
		}

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
			return "this is MainWindow";
		}

	#endregion
	}
}