#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   3/21/2021 6:24:52 AM

namespace SharedCode.FormulaSupport.ParseSupport
{
	public static class ParseRegexSupport
	{
		private static Regex[] RE;

		static ParseRegexSupport()
		{
			RE = new Regex[5];

			// extract all special variables
			// RE[(int) RegexItemCode.RI_FORMULA] = new Regex(
			// 	@"(?<E>=)|(?<eq>.*?)(?<v>\{((?<code>[\$\#\%\!\@])(?<name>.*?)|(?<name>(?<code>\[).*?\]))\})|(?<eq>.+$)",
			// 	RegexOptions.Compiled | RegexOptions.ExplicitCapture);

			// extract all special variables
			RE[(int) RegexValidateType.RI_FORMULA] = new Regex(
				@"\s*(?<E>=?)(?<eq>.*?)\{(?:(?<keyvar>[\$\#\%\!\@].+?)|(?<keyvar>\[.+?\]))\}|\s*(?<E>=?)(?<eq>.+$)",
				RegexOptions.Compiled | RegexOptions.ExplicitCapture);

			// validate an excell address
			RE[(int) RegexValidateType.RI_EXCEL_ADDR] = new Regex(@"^(?<name>(?i:([a-w]?[a-z]{1,2}|x[a-f][a-d])\d{1,7}))$",
				RegexOptions.Compiled | RegexOptions.ExplicitCapture);

			// a cell, label name
			RE[(int) RegexValidateType.RI_VAR_NAME_LABEL] = new Regex(@"(?<name>[a-zA-Z][a-zA-Z0-9]{1,23})",
				RegexOptions.Compiled | RegexOptions.ExplicitCapture);

			// an other name (parameter name)
			RE[(int) RegexValidateType.RI_VAR_NAME_OTHER] = new Regex(@"(?<name>[a-zA-Z][a-zA-Z0-9]{1,23})",
				RegexOptions.Compiled | RegexOptions.ExplicitCapture);
		}

		public static MatchCollection MatchFormula(string formula)
		{
			return RE[0].Matches(formula);
		}

		internal static Match ValidateExcelAddr(string address)
		{
			return RE[(int) RegexValidateType.RI_EXCEL_ADDR].Match(address);
		}
		
		internal static Match ValidateLabelName(string name)
		{
			return RE[(int) RegexValidateType.RI_VAR_NAME_LABEL].Match(name);
		}
				
		internal static Match ValidateOtherName(string name)
		{
			return RE[(int) RegexValidateType.RI_VAR_NAME_OTHER].Match(name);
		}

	}
}
