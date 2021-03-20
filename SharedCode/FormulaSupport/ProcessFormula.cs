#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  3/15/2021 10:03:00 PM

namespace SharedCode.FormulaSupport
{
	public enum FormulaVariableType
	{
		UNASSIGNED			= -1,
		CELL_ADDRESS,		// [], <ca>
		SYST_VARIABLE,		// $, <sv>
		REVIT_PARAMETER,	// #, <rp>
		PROJECT_PARAMETER,	// %, <pp>
		GLOBAL_PARAMETER,	// !, <gp>
		LABEL_NAME			// @, <ln>
	}

/*
 final
@"(?<E>=)|(?<eq>.*?)(?<v>\{(\[(?<ca>(?i:[a-z]{1,3}[1-9]\d{0,6}))\]|\$(?<sv>.+?)|\@(?<ln>.+?)|\!(?<gp>.+?)|\%(?<pp>.+?)|\#(?<rp>.+?))\})|(?<eq>.+$)";
except gets a "correct" excel address
@"(?<E>=)|(?<eq>.*?)(?<v>\{(\[(?<ca>(([a-wA-W]{0,1}[a-zA-Z]{1,2}|[xX][a-eA-E][a-zA-Z]|[xX][fF][a-dA-D])[1-9]\d{0,6}))\]|\$(?<sv>.+?)|\@(?<ln>.+?)|\!(?<gp>.+?)|\%(?<pp>.+?)|\#(?<rp>.+?))\})|(?<eq>.+$)";
 */
	public class ProcessFormula
	{
		public struct Var_Id
		{
			public Var_Id(string id, char prefix, char sufix = (char) 0)
			{
				this.prefix = prefix;
				this.suffix = sufix;
				varType = FormulaVariableType.UNASSIGNED;

				Id = id;
			}

			public string this[bool b]
			{
				get
				{
					if (b) return char.ToString(prefix);
					return char.ToString(suffix);
				}
			}

			public char this[int idx]
			{
				get
				{
					if (idx < 0 || idx % 2 == 0) return prefix;

					return suffix;
				}
			}

			private readonly FormulaVariableType varType;
			private readonly char prefix;
			private readonly char suffix;
			

			public string Prefix => new string(new char[] {ID_STR_pfx, prefix});

			public string PrefixStr => prefix.ToString();

			public int PrefixLen => Prefix.Length;

			public string Suffix => new string(new char[] {suffix, ID_STR_sfx});

			public int SuffixLen => Suffix.Length;

			public string Id { get; private set; }
		}

	#region private fields

		private const char ID_STR_pfx = '{';
		private const char ID_STR_sfx = '}';

		private readonly Var_Id EXCEL_ADDR     = new Var_Id("ca", '[', ']');	// CA cell address
		private readonly Var_Id SYST_VAR       = new Var_Id("sv", '$');	// SA
		private readonly Var_Id REVIT_PARAM    = new Var_Id("rp", '#');	// RP
		private readonly Var_Id PROJ_PARAM     = new Var_Id("pp", '%');	// PP
		private readonly Var_Id GLOBAL_PARAM   = new Var_Id("gp", '!');	// GP
		private readonly Var_Id LABEL_NAME     = new Var_Id("ln", '@');	// LN


	#endregion

	#region ctor

		// public ProcessFormula()
		// {
		// }

	#endregion

	#region public properties

		// public int CatagorizeVariable(string variableString)
		// {
		// 	string testCode = variableString.Trim().Substring(0, 2);
		//
		// 	Var_Id? vId = GetCode(testCode);
		//
		// 	if (vId == null) return -1;
		// }


	#endregion

	#region private properties



	#endregion

	#region public methods

		public bool Process(string formula)
		{
			string f = formula.Trim();

			if (f[0] != '=') return false;


			return true;
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
			return "this is ProcessFormula";
		}

	#endregion
	}
}