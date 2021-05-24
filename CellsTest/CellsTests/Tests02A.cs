#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CellsTest.Windows;
using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.ParseSupport;

using SharedCode.EquationSupport.Definitions;

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

		internal void parseTest02()
		{
			win.WriteLine("parse test 01");

			ParseInitial pi = new ParseInitial();

			string[] test = new string[15];

			int k = 0;

			test[k++] = "=(1 + (21 + 22 + (31 + 32 * (41 + 42) * (sign(51+52) ) ) ) + 2*3) + 4+5 + {[A1]} + ({!B1}) & \"text\"";
			test[k++] = "= 123 (456+(123 + (678 + (123 + 123 + (123 + (123 + 456) + 456)) + 456) + 246))";

			for (int i = 0; i < test.Length; i++)
			{
				if (test[i] == null) break;

				bool result = pi.Parse(test[i]);

				if (result)
				{
					win.WriteLine("\ntesting| " + i + "| length| " + test[i].Length);
					ListPattern(test[i]);
					ListMatches(pi);
				}

			}
		}

		private void ListPattern(string pattern)
		{
			win.Write("");
			win.WriteLine("0---  0---1--- 10---2--- 20---3--- 30---4--- 40---5--- 50---6--- 60---7--- 70---8--- 80---0--- 90---1");
			win.WriteLine("0123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|");
			win.WriteLine(pattern);
			win.Write("\n");
		}

		private void ListMatches(ParseInitial pi)
		{
			for (var i = 0; i < pi.FormulaComponents.Count; i++)
			{
				Tuple<string, string> pc = pi.FormulaComponents[i];
				win.WriteLine($"name|\t{pc.Item1,-6}\t\tvalue|\t{pc.Item2}");
			}
		}

		internal void parseTest01()
		{
			ValueDefinitions opValues = ValueDefinitions.VdefInst;

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

		private void FindTokens(int which, ValueDefinitions opValues,
			string[] tests, int numLoops, int numTestStrings)
		{
			int countLoops;
			int countTests = 0;
			int countFails = 0;

			ValueDef vdIdentifier;

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
					vdIdentifier = GetToken0(opValues, tests[t]);
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

		private ValueDef GetToken0(ValueDefinitions opValues, string test)
		{
			return opValues.Classify(test);
		}

		// this method was lots slower (something like 4000 versus 700)
		// private TokenParse GetToken1(OperationTokenDefinitions opTokens, string test)
		// {
		// 	return opTokens.Find(test);
		// }

	#endregion

	#region private methods

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
	}
}