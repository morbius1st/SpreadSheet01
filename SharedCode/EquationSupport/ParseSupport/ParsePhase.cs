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
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;
using static SharedCode.EquationSupport.Definitions.ParseDefinitions;

#endregion

// username: jeffs
// created:  5/18/2021 1:42:14 PM


// step 1:  take raw formula and parse into 
// coded strings

namespace SharedCode.EquationSupport.ParseSupport
{
	public class ParsePhase
	{
	#region private fields

		// private string pattern =
		// 	@"(?<l1>[-+]?(?>\d+'-(?>\d*\.\d+|(?>\d+ )?\d+\/\d+|\d+)""|(?>\d+ \d+\/\d+|\d+\/\d+|\d*\.\d+|\d+)[""']))|(?<fr1>[-+]?(?>\d+\s+\d+\/\d+|\d+\/\d+))|(?<d1>[-+]?(?>\d+\.\d*|\d*\.\d+))|(?<n1>[-+]?\d+(?![.\/]))|(?<b1>\bTrue\b|\bFalse\b)|(?<fn1>[a-zA-Z]\w*(?=\())|(?<s1>\"".+?\"")|(?<op1>\<[oO][rR]\>|\<[aA][nN][dD]\>|\+|\-|&|<=|>=|<|>|==|!=|\*|\/|\!|\^|%)|(?<eq>=)|(?<pb>\()|(?<pe>\))|(?<as1>,)|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<v3>[a-zA-Z]\w*)|(?<x1>[^ ])";

		private string pattern =
		// 	@"(?<l1>[-+]?(?>\d+'-(?>\d*\.\d+|(?>\d+ )?\d+\/\d+|\d+)""|(?>\d+ \d+\/\d+|\d+\/\d+|\d*\.\d+|\d+)[""']))|(?<fr1>[-+]?(?>\d+\s+\d+\/\d+|\d+\/\d+))|(?<d1>[-+]?(?>\d+\.\d*|\d*\.\d+))|(?<n1>[-+]?\d+(?![.\/]))|(?<b1>\bTrue\b|\bFalse\b)|(?<fn1>[a-zA-Z]\w*(?=\())|(?<s1>\"".+?\"")                                                  |(?<op1>\<[oO][rR]\>|\<[aA][nN][dD]\>|\+|\-|&|<=|>=|<|>|==|!=|\*|\/|\!|\^|%)|(?<eq>=)|(?<pb>\()|(?<pe>\))|(?<as1>,)|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<v3>[a-zA-Z]\w*)|(?<x1>[^ ])";
			@"(?<l1>[-+]?(?>\d+'-(?>\d*\.\d+|(?>\d+ )?\d+\/\d+|\d+)""|(?>\d+ \d+\/\d+|\d+\/\d+|\d*\.\d+|\d+)[""']))|(?<fr1>[-+]?(?>\d+\s+\d+\/\d+|\d+\/\d+))|(?<d1>[-+]?(?>\d+\.\d*|\d*\.\d+))|(?<n1>[-+]?\d+(?![.\/]))|(?<b1>\bTrue\b|\bFalse\b)|(?<fn1>[a-zA-Z]\w*(?=\())|(?<s1>\"".+?\"")|(?>(?<=\+|\-|\*|\/|^)(\s*)(?<ng>-)(?=[a-zA-Z({]))|(?<op1>\<[oO][rR]\>|\<[aA][nN][dD]\>|\+|\-|&|<=|>=|<|>|==|!=|\*|\/|\!|\^|%)|(?<eq>=)|(?<pb>\()|(?<pe>\))|(?<as1>,)|(?<v1>{\[.+?\]})|(?<v2>\{[!@#$%].+?\})|(?<v3>[a-zA-Z]\w*)|(?<x1>[^ ])";


		private ValueDefinitions vds = ValueDefinitions.ValDefInst;
		private ParseDefinitions pgd = ParseDefinitions.PgDefInst;

		private string vd_grpRefVal;
		private string vd_grpBegVal;
		private string vd_grpEndVal;
		
		private string vd_fnGrpBegVal;
		private string vd_fnGrpEndVal;

		private string pg_prnBegName;
		private string pg_prnEndName;

		private string pg_grpRefName;
		private string pg_grpBegName;
		private string pg_grpEndName;

		private string pg_fgBegName;
		private string pg_fgEndName;

		private string pg_functName;


		public bool IsBalanced { get; private set; }
		public int MaxLevels { get; private set; }


	#endregion

	#region ctor

		public ParsePhase()
		{
			vd_grpRefVal  = vds.Definitions[Vd_GrpRef].ValueStr;      // <gpr>
			vd_grpBegVal  = vds.Definitions[Vd_GrpBeg].ValueStr;      // <gpb>
			vd_grpEndVal  = vds.Definitions[Vd_GrpEnd].ValueStr;      // <gpe>

			vd_fnGrpBegVal= vds.Definitions[Vd_FnGrpBeg].ValueStr;    // <fgb>
			vd_fnGrpEndVal= vds.Definitions[Vd_FnGrpEnd].ValueStr;    // <fge>

			pg_prnBegName = pgd.Definitions[Pgd_PrnBeg].ValueStr;     // 'pb'
			pg_prnEndName = pgd.Definitions[Pgd_PrnEnd].ValueStr;     // 'pe'

			pg_grpRefName = pgd.Definitions[Pgd_GrpRef].ValueStr;     // grpref
			pg_grpBegName = pgd.Definitions[Pgd_GrpBeg].ValueStr;     // grpbeg
			pg_grpEndName = pgd.Definitions[Pgd_GrpEnd].ValueStr;     // grpend

			pg_fgBegName = pgd.Definitions[Pgd_FunctGrpBeg].ValueStr; // fngrpbeg
			pg_fgEndName = pgd.Definitions[Pgd_FunctGrpEnd].ValueStr; // fngrpend

			pg_functName = pgd.Definitions[Pgd_Funct].ValueStr;       // fn1
		}

	#endregion

	#region public properties


		public List<List<ParseData>> ParseList {get; private set; }

		public string Pattern => pattern;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool Parse4(string formula)
		{
			if (string.IsNullOrWhiteSpace(formula)) return false;

			Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture);

			MatchCollection c = r.Matches(formula);

			if (c == null || c.Count < 1) return false;

			List<List<ParseData>> phdList = 
				new List<List<ParseData>>() {new List<ParseData>()};

			int result = GetMatches4(c, 0, 0, 0, phdList);

			ParseList = result == 0 ? phdList : null;

			return true;
		}

	#endregion

	#region private methods


		private int GetMatches4(MatchCollection c, 
			int collIdx, int lstIdx, int level, List<List<ParseData>> result)
		{
			ParseData pd;
			int listIdx = lstIdx;

			bool gotFunction = collIdx > 0 && result[listIdx][0].Name.Equals(pg_fgBegName);

			if (level > MaxLevels) MaxLevels = level;

			for (int i = collIdx; i < c.Count; i++)
			{
				Match m = c[i];

				for (int j = 1; j < m.Groups.Count; j++)
				{
					Group g = m.Groups[j];

					if (g.Success)
					{
						if (g.Name.Equals(pg_prnBegName))
						{
							result.Add(new List<ParseData>());

							int newListIdx = result.Count - 1;

							result[listIdx].Add(
								new ParseData(pg_grpRefName, vd_grpRefVal, -newListIdx, 0, level));

							int posIdx = result[listIdx].Count - 2;

							if (posIdx >= 0 && !result[listIdx][posIdx].Name.Equals(pg_functName))
							{
								result[newListIdx].Add(
									new ParseData(pg_grpBegName, vd_grpBegVal, listIdx, 0, level+1));
							}
							else
							{
								result[newListIdx].Add(
									new ParseData(pg_fgBegName, vd_fnGrpBegVal, listIdx, 0, level+1));
							}

							result[newListIdx].Add(
								new ParseData(g.Name, g.Value, g.Index, g.Length, level+1));
							
							i = GetMatches4(c, i + 1, newListIdx, level + 1, result);

							if (i == -1) return -1;

							break;
						}

						pd = new ParseData(g.Name, g.Value, g.Index, g.Length, level);

						result[listIdx].Add(pd);

						if (g.Name.Equals(pg_prnEndName))
						{
							if (!gotFunction)
							{
								result[listIdx].Add(new ParseData(
									pg_grpEndName, vd_grpEndVal, result[listIdx][0].Position, 0, level));
							}
							else
							{
								result[listIdx].Add(new ParseData(
									pg_fgEndName, vd_fnGrpEndVal, result[listIdx][0].Position, 0, level));
							}

							return i;
						}

						break;
					}
				}
			}

			IsBalanced = level == 0;

			return level == 0 ? 0 : -1;
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


		// public List<ParsePhData> FormulaComponents { get; private set; }

		// private List<List<ParsePhData>> GetMatches4(MatchCollection c)
		// {
		// 	List<List<ParsePhData>> phdList = new List<List<ParsePhData>>() {new List<ParsePhData>()};
		// 	int result = GetMatches4(c, 0, 0, 0, phdList);
		// 	return result == 0 ? phdList : null;
		// }

		// // phase one - use the regex to parse the line into its equation components
		// // store as a List<> of tuples<string, string>
		// public bool Parse(string formula)
		// {
		// 	Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
		// 	MatchCollection c = r.Matches(formula);
		//
		// 	FormulaComponents = new List<ParsePhData>();
		//
		// 	bool result = GetMatches(c, FormulaComponents);
		//
		// 	if (result != true || FormulaComponents.Count == 0)
		// 	{
		// 		result = false;
		// 	}
		//
		// 	return result;
		// }

		// public bool Parse2(string formula)
		// {
		// 	Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
		// 	MatchCollection c = r.Matches(formula);
		//
		// 	ParsePhData pd = new ParsePhData("root", "root", 0, 0, 0);
		//
		// 	FormulaComponents = new List<ParsePhData>();
		//
		// 	pd.children = FormulaComponents;
		//
		// 	int idx;
		//
		// 	ParsePhData result = GetMatches2(c, pd, 0, 0, out idx);
		//
		// 	if (result == null || FormulaComponents.Count == 0 || idx < 0)
		// 	{
		// 		return false;
		// 	}
		//
		// 	return true;
		// }

		// public List<List<ParsePhData>> Parse3(string formula)
		// {
		// 	Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.ExplicitCapture);
		// 	MatchCollection c = r.Matches(formula);
		//
		// 	List<List<ParsePhData>> result = GetMatches3(c);
		//
		// 	if (result == null)
		// 	{
		// 		return null;
		// 	}
		//
		// 	return result;
		// }


		// private bool GetMatches(MatchCollection c, List<ParsePhData> matches)
		// {
		// 	Match m;
		// 	Group g;
		//
		// 	if (c.Count < 1)
		// 	{
		// 		return false;
		// 	}
		//
		// 	for (int i = 0; i < c.Count; i++)
		// 	{
		// 		m = c[i];
		//
		// 		for (int j = 1; j < m.Groups.Count; j++)
		// 		{
		// 			g = m.Groups[j];
		//
		// 			if (g.Success)
		// 			{
		// 				matches.Add(new ParsePhData(g.Name, g.Value, g.Index, g.Length, 0));
		// 			}
		// 		}
		// 	}
		//
		// 	return true;
		// }


		// private List<List<ParsePhData>> GetMatches3(MatchCollection c)
		// {
		// 	List<List<ParsePhData>> result = new List<List<ParsePhData>>();
		//
		// 	result.Add(new List<ParsePhData>());
		//
		// 	int currIdx = 0;
		// 	int priorIdx = 0;
		// 	int level = 0;
		//
		// 	ParsePhData pd;
		//
		// 	for (int i = 0; i < c.Count; i++)
		// 	{
		// 		Match m = c[i];
		//
		// 		for (int j = 1; j < m.Groups.Count; j++)
		// 		{
		// 			Group g = m.Groups[j];
		//
		// 			if (g.Success)
		// 			{
		// 				if (g.Name.Equals(prnBegName))
		// 				{
		// 					result.Add(new List<ParsePhData>());
		//
		// 					priorIdx = currIdx;
		// 					currIdx = result.Count - 1;
		//
		// 					result[priorIdx].Add(
		// 						new ParsePhData(grpRefName, grpRef, currIdx, 0, level));
		//
		// 					level++;
		//
		// 					result[currIdx].Add(new ParsePhData(
		// 						grpBegName, grpBeg, priorIdx, 0, level));
		// 				}
		//
		// 				pd = new ParsePhData(g.Name, g.Value, g.Index, g.Length, level);
		//
		// 				result[currIdx].Add(pd);
		//
		// 				if (g.Name.Equals(prnEndName))
		// 				{
		// 					result[currIdx].Add(new ParsePhData(
		// 						grpEndName, grpEnd, priorIdx, 0, level));
		// 					level--;
		// 					currIdx = result[currIdx][0].Position;
		// 				}
		// 			}
		// 		}
		// 	}
		//
		// 	IsBalanced = level == 0;
		//
		// 	return result;
		// }


		// private ParsePhData GetMatches2(MatchCollection c, ParsePhData pdx, int pos, int depth, out int idx)
		// {
		// 	Match m;
		// 	Group g;
		// 	int i = pos;
		//
		// 	if (c.Count < 1 || pos < 0)
		// 	{
		// 		idx = -1;
		// 		return null;
		// 	}
		//
		// 	string refName = pgd.Definitions[Pgd_GrpRef].ValueStr;
		// 	string refVal = vds.Definitions[Vd_GrpRef].ValueStr;
		// 	string begVal = vds.Definitions[Vd_PrnBeg].ValueStr;
		// 	string endVal = vds.Definitions[Vd_PrnEnd].ValueStr;
		//
		// 	for (; i < c.Count; i++)
		// 	{
		// 		m = c[i];
		//
		// 		for (int j = 1; j < m.Groups.Count; j++)
		// 		{
		// 			g = m.Groups[j];
		//
		// 			if (g.Success)
		// 			{
		// 				idx = i;
		//
		// 				ParsePhData pd = new ParsePhData(g.Name, g.Value, g.Index, g.Length, depth);
		//
		// 				if (g.Value.Equals(begVal))
		// 				{
		// 					pdx.children.Add(pd);
		//
		// 					pd.children =  new List<ParsePhData>();
		// 					int index;
		//
		// 					pd = GetMatches2(c, pd, ++i, depth++, out index);
		//
		// 					i = index;
		//
		// 					if (pd == null) return pd;
		// 				}
		// 				else if (g.Value.Equals(endVal))
		// 				{
		// 					idx = ++i;
		// 					return pd;
		// 				}
		//
		// 				pdx.children.Add(pd);
		//
		// 				break;
		// 			}
		// 		}
		// 	}
		//
		// 	idx = i;
		// 	return null;
		// }

	}
}