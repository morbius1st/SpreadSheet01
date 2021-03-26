#region + Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Windows.Forms;
using SharedCode.FormulaSupport.ParseSupport;

#endregion

// user name: jeffs
// created:   3/20/2021 7:57:37 AM

namespace SharedCode.FormulaSupport
{
	public enum TestType
	{
		NONE = 0,
		ID_FIRST_CHAR = 1,
		ID_REMAIN_CHARS = 2,
		PARAM_FIRST_CHAR = 3,
		PARAM_REMAIN_CHARS =4
	}

	public enum TestStatusCode
	{
		FAIL_GENERAL = -100,
		FAIL_TOO_LONG = -2,
		FAIL_INVALID_CHAR = -1,
		PASS = 0
	}

	public enum RegexValidateType
	{
		RI_FORMULA			= 0,
		RI_EXCEL_ADDR		= 1,
		RI_VAR_NAME_LABEL	= 2,
		RI_VAR_NAME_OTHER	= 3
	}

	public struct Var_Id
	{
		public Var_Id(  string id, RegexValidateType validateType, char prefix, char sufix = (char) 0)
		{
			this.prefix = prefix;
			this.suffix = sufix;
			this.validateType = validateType;

			Id = id;
		}

		public char this[bool b]
		{
			get
			{
				if (b) return prefix;
				return suffix;
			}
		}

		public char this[int idx]
		{
			get
			{
				if (idx == 0 || idx % 2 == 0) return prefix;

				return suffix;
			}
		}

		private readonly RegexValidateType validateType;
		private readonly char prefix;
		private readonly char suffix;


		public string Prefix => prefix.ToString();
		public char cPrefix => prefix;

		public string PrefixStr => prefix.ToString();

		public int PrefixLen => Prefix.Length;

		public string Suffix => suffix.ToString();
		public char cSuffix => suffix;

		public int SuffixLen => Suffix.Length;

		public RegexValidateType ValidateType => validateType;

		public string Id { get; private set; }
	}


	public class ProcessFormulaSupport
	{
		private const char ID_STR_pfx = '{';
		private const char ID_STR_sfx = '}';

		private  readonly Var_Id EXCEL_ADDR     = new Var_Id("ca",
			RegexValidateType.RI_EXCEL_ADDR, '[', ']'); // CA cell address

		private  readonly Var_Id SYST_VAR       = new Var_Id("sv",
			RegexValidateType.RI_VAR_NAME_OTHER, '$'); // SA

		private  readonly Var_Id REVIT_PARAM    = new Var_Id("rp",
			RegexValidateType.RI_VAR_NAME_OTHER, '#'); // RP

		private  readonly Var_Id PROJ_PARAM     = new Var_Id("pp",
			RegexValidateType.RI_VAR_NAME_OTHER, '%'); // PP

		private  readonly Var_Id GLOBAL_PARAM   = new Var_Id("gp",
			RegexValidateType.RI_VAR_NAME_OTHER, '!'); // GP

		private  readonly Var_Id LABEL_NAME     = new Var_Id("ln",
			RegexValidateType.RI_VAR_NAME_LABEL, '@'); // LN

		public static Var_Id[] varIds;

		private  char[] varIdsCodes;

		private  LabelNameValidation labelNameValidate;
		private  ParamNameValidation paramNameValidate;
		private  ExcelAddrValidation excelNameValidate;

		internal  ParseUtil util = new ParseUtil();

		public ProcessFormulaSupport()
		{
			setvarId();
			assignValidate();
		}

		internal  ParseUtil Util => util;

		internal  void Clear()
		{
			TokenList = null;
		}

		private  void setvarId()
		{
			varIds = new []
			{
				EXCEL_ADDR, SYST_VAR, REVIT_PARAM, PROJ_PARAM, GLOBAL_PARAM, LABEL_NAME
			};

			varIdsCodes = new char[varIds.Length];

			for (var i = 0; i < varIds.Length; i++)
			{
				varIdsCodes[i] = varIds[i][true];
			}
		}

		private void assignValidate()
		{
			List<KeyValuePair<int, TestType>> testTypes = new List<KeyValuePair<int, TestType>>()
			{
				{new KeyValuePair<int, TestType>(1, TestType.ID_FIRST_CHAR)},
				{new KeyValuePair<int, TestType>(-1, TestType.ID_REMAIN_CHARS)}
			};

			labelNameValidate = new LabelNameValidation(20, testTypes);

			testTypes = new List<KeyValuePair<int, TestType>>()
			{
				{new KeyValuePair<int, TestType>(1, TestType.PARAM_FIRST_CHAR)},
				{new KeyValuePair<int, TestType>(-1, TestType.PARAM_REMAIN_CHARS)}
			};
			paramNameValidate = new ParamNameValidation(32, testTypes);

			excelNameValidate = new ExcelAddrValidation();
		}

		internal  List<KeyValuePair<string, string>> TokenList { get; private set; }

		internal  bool FormulaParse(string formula)
		{
			TokenList = tokenizeFormula(formula);

			if (TokenList.Count == 0) return false;

			return true;
		}

		// get the complete token list
		private  List<KeyValuePair<string, string>> tokenizeFormula(string formula)
		{
			// get collection of regex matches
			MatchCollection mc = ParseRegexSupport.MatchFormula(formula);

			List<KeyValuePair<string, string>> tokenList = new List<KeyValuePair<string, string>>();

			foreach (Match m in mc)
			{
				for (var i = 1; i < m.Groups.Count; i++)
				{
					Group g = m.Groups[i];

					if (g.Success && !string.IsNullOrWhiteSpace(g.Value))
					{
						tokenList.Add(new KeyValuePair<string, string>(g.Name, g.Value));
					}
				}
			}

			return tokenList;
		}

		public  List<ValuePair<string, string>> GetKeyVar(string formula)
		{
			string f = formula.Trim();

			List<ValuePair<string, string>> bothSides = new List<ValuePair<string, string>>();

			Clear();

			if (!FormulaParse(f)) return null;

			for (var j = 0; j < TokenList.Count; j++)
			{
				KeyValuePair<string, string> kvp = TokenList[j];

				if (kvp.Key.Equals("code"))
				{
					Debug.WriteLine("\ngot code| " + kvp.Value + " matches?| " + TokenList[j].Value);
					Debug.WriteLine("search idx| ");

					int idx = findIdx(kvp.Value[0]);

					Debug.WriteLine("\nidx| " + idx);

					if (idx >= 0)
					{
						j++;
						Debug.WriteLine("add kvp| id| " + varIds[idx].Id + "  value| " + TokenList[j].Value);
						bothSides.Add(new ValuePair<string, string>(varIds[idx].Id, TokenList[j].Value));
					}
				}
			}

			return bothSides;
		}

		private  int findIdx(char c)
		{
			// char c = (char) value[0];
			// Debug.Write("look for| " + c + " |");

			for (var i = 0; i < varIds.Length; i++)
			{
				// Debug.Write(" vs " + varIds[i].cPrefix);

				if (c == varIds[i].cPrefix ) return i;
			}

			return -1;
		}

		public bool GetKeyVars2(out List<ValuePair<int, string>> keyVars)
		{
			// scan tokenlist and extract key vars
			// and categorize the key var
			// leftside (=) right side
			// if more than (2) key vars, fail & return

			keyVars = new List<ValuePair<int, string>>();

			for (var j = 0; j < TokenList.Count; j++)
			{
				KeyValuePair<string, string> kvp = TokenList[j];

				if (!kvp.Key.Equals("keyvar")) continue;

				// kvp.value is the keyvar including its preface code
				// determine which keyvar

				int idx = findIdx(kvp.Value[0]);

				if (idx < 0) return false;

				keyVars.Add(new ValuePair<int, string>(idx, kvp.Value.Substring(1)));

				if (keyVars.Count > 2) return false;
			}

			return true;
		}

		public bool GetKeyVars3(out ValuePair<int, string> keyVar)
		{
			bool gotValue = false;
			// scan tokenlist and extract key vars
			// and categorize the key var
			// leftside (=) right side
			// if more than (2) key vars, fail & return

			keyVar = new ValuePair<int, string>();

			for (var j = 0; j < TokenList.Count; j++)
			{
				KeyValuePair<string, string> kvp = TokenList[j];

				if (kvp.Key.Equals("E")) continue;
				if (!kvp.Key.Equals("keyvar") || gotValue) return false;

				// kvp.value is the keyvar including its preface code
				// determine which keyvar

				int idx = findIdx(kvp.Value[0]);

				// Debug.Write("\n");

				if (idx < 0) return false;

				keyVar = new ValuePair<int, string>(idx, 
					kvp.Value);

				keyVar.Value = keyVar.Value.Substring(varIds[idx].PrefixLen,
					keyVar.Value.Length - varIds[idx].PrefixLen - varIds[idx].SuffixLen);

				gotValue = true;
			}

			return true;
		}

		public Tuple<int, char, TestType, TestStatusCode> ValidateKeyVar(ValuePair<int, string> keyVar)
		{
			switch (varIds[keyVar.Key].ValidateType)
			{

			case RegexValidateType.RI_EXCEL_ADDR:
				{
					return excelNameValidate.Validate(keyVar.Value);
				}
			case RegexValidateType.RI_VAR_NAME_LABEL:
				{
					return labelNameValidate.Validate(keyVar.Value);
				}
			case RegexValidateType.RI_VAR_NAME_OTHER:
				{
					return paramNameValidate.Validate(keyVar.Value);
				}
			}

			return null;
		}


		public  void Tests()
		{
			string[] test = new string[10];

			test[0] = "aAcdef";
			test[1] = "a12cdef";
			test[2] = "abc123AaAdef";
			test[3] = "1abcdef";
			test[4] = "@abcdef";
			test[5] = "ab@cdef";


			for (var i = 0; i < test.Length; i++)
			{
				if (test[i] == null) continue;

				Tuple<int, char, TestType, TestStatusCode> result
					= labelNameValidate.Validate(test[i]);

				if (result.Item4 == TestStatusCode.PASS)
				{
					Debug.WriteLine("testing| " + test[i] + " passed");
				}
				else
				{
					Debug.WriteLine("testing| " + test[i]
						+ " failed| >" + result.Item2 + "<"
						+ " | " +  result.Item3.ToString()
						+ " | " + result.Item4.ToString());
				}
			}

			Debug.WriteLine("done");
		}
	}
}