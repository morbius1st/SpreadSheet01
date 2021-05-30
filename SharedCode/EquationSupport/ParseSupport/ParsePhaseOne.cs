#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SharedCode.EquationSupport.Definitions;

#endregion

// username: jeffs
// created:  5/18/2021 1:42:14 PM


// step 1:  take raw formula and parse into 
// coded strings

namespace SharedCode.EquationSupport.ParseSupport
{
	public struct ParsePh1Data
	{
		public string Name { get; }
		public string Value { get; }
		public int Position { get; }
		public int Length { get; }
		public ADefBase2 Definition { get; set; }
		public bool IsValueDef { get; set; }

		public ParsePh1Data(string name, string value, int position, int length)
		{
			Name = name;
			Value = value;
			Position = position;
			Length = length;
			Definition = null;
			IsValueDef = false;
		}
	}


	public class ParsePhaseOne
	{
	#region private fields

		private string pattern = @"(?<l1>[-+]?(?>\d+'-(?>\d*\.\d+|(?>\d+ )?\d+\/\d+|\d+)""|(?>\d+ \d+\/\d+|\d+\/\d+|\d*\.\d+|\d+)[""']))|(?<fr1>[-+]?(?>\d+ \d+\/\d+|\d+\/\d+))|(?<d1>[-+]?(?>\d+\.\d*|\d*\.\d+))|(?<n1>[-+]?\d+(?![.\/]))|(?<b1>\bTrue\b|\bFalse\b)|(?<fn1>[a-zA-Z]\w*(?=\())|(?<s1>\"".+?\"")|(?<op1>\<[oO][rR]\>|\<[aA][nN][dD]\>|\+|\-|&|<=|>=|<|>|==|!=|\*|\/)|(?<eq>=)|(?<pb>\()|(?<pe>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<v3>[a-zA-Z]\w*)|(?<x1>[^ ])";

	#endregion

	#region ctor

		public ParsePhaseOne() { }

	#endregion

	#region public properties

		public List<ParsePh1Data> FormulaComponents { get; private set; }

		public string Pattern => pattern;

	#endregion

	#region private properties

	#endregion

	#region public methods

		// phase one - use the regex to parse the line into its equation components
		// store as a List<> of tuples<string, string>
		public bool Parse1(string formula)
		{
			Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
			MatchCollection c = r.Matches(formula);

			FormulaComponents = new List<ParsePh1Data>();

			bool result = GetMatches(c, FormulaComponents);

			if (result != true || FormulaComponents.Count == 0)
			{
				result = false;
			}

			return result;
		}

	#endregion

	#region private methods

		private bool GetMatches(MatchCollection c, List<ParsePh1Data> matches)
		{
			Match m;
			Group g;

			if (c.Count < 1)
			{
				return false;
			}

			for (int i = 0; i < c.Count; i++)
			{
				m = c[i];

				for (int j = 1; j < m.Groups.Count; j++)
				{
					g = m.Groups[j];

					if (g.Success)
					{
						
						matches.Add(new ParsePh1Data(g.Name, g.Value, g.Index, g.Length));
					}
				}
			}

			return true;
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ParseInitial";
		}

	#endregion

	}
}