#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#endregion

// username: jeffs
// created:  5/18/2021 1:42:14 PM


// step 1:  take raw formula and parse into 
// coded strings

namespace SharedCode.EquationSupport.ParseSupport
{
	public class ParseInitial
	{
	#region private fields

		private string pattern = @"(?<l1>[-+]?(?>\d+'-(?>\d*\.\d+|(?>\d+ )?\d+\/\d+|\d+)""|(?>\d+ \d+\/\d+|\d+\/\d+|\d*\.\d+|\d+)[""']))|(?<fr1>[-+]?\d+ \d+\/\d+|\d+\/\d+)|(?<d1>[-+]?(?>\d+\.\d*|\d*\.\d+))|(?<n1>[-+]?\d+(?![.\/]))|(?<fn1>[a-zA-Z]\w*(?=\())|(?<s1>\"".+?\"")|(?<op1>\<[oO][rR]\>|\<[aA][nN][dD]\>|\+|\-|&|<=|>=|<|>|==|!=|\*|\/)|(?<eq>=)|(?<pdn>\()|(?<pup>\))|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<w1>[a-zA-Z]\w*)|(?<x1>[^ ])";

	#endregion

	#region ctor

		public ParseInitial() { }

	#endregion

	#region public properties

		public List<Tuple<string, string>> FormulaComponents { get; private set; }

		public string Pattern => pattern;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool Parse(string formula)
		{
			Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
			MatchCollection c = r.Matches(formula);

			FormulaComponents = new List<Tuple<string, string>>();

			bool result = GetMatches(c, FormulaComponents);

			if (result != true || FormulaComponents.Count == 0)
			{
				result = false;
			}

			return result;
		}

	#endregion

	#region private methods

		private bool GetMatches(MatchCollection c, List<Tuple<string, string>> matches)
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
						matches.Add(new Tuple<string, string>(g.Name, g.Value));
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