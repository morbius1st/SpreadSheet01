#region + Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   3/20/2021 7:57:37 AM

namespace SharedCode.FormulaSupport
{
	public static class ParseFormula
	{
		private static Regex[] RE;

		private enum TestType
		{
			ID_FIRST_CHAR,
			ID_REMAIN_CHARS
		}

		private static charValidation idFirstChar;
		private static charValidation idRemainder;

		// static char[] invalidRevitNameChars = new [] {'\\',':','{','}', '[', ']', '|', ';', '<', '>', '?', '`', '~' }; //@"\:{}[]|;<>?`~";
		static string invalidRevitNameChars = @"\:{}[]|;<>?`~";

		static char[] lcLetters = new char[] {'a', 'z'};
		static char[] ucLetters = new char[] {'A', 'Z'};
		static char[] digits     = new char[] {'0', '9'};

		static char[][] letterChars = new char[][]
		{
			lcLetters, ucLetters
		};

		static char[][] wordChars = new char[][]
		{
			lcLetters, ucLetters, digits
		};



		private static idValidation labelNameValidate;


		static ParseFormula()
		{
			idFirstChar = new charValidation(null, letterChars, invalidRevitNameChars);
			idRemainder= new charValidation(null, wordChars, invalidRevitNameChars);

			List<KeyValuePair<int, TestType>> labelNameList = new List<KeyValuePair<int, TestType>>()
			{
				{new KeyValuePair<int, TestType>(1, TestType.ID_FIRST_CHAR) },
				{new KeyValuePair<int, TestType>(-1, TestType.ID_REMAIN_CHARS) }
			};


			labelNameValidate = new idValidation(20, labelNameList);

			RE = new Regex[5];

			// extract all special variables
			RE[0] = new Regex(
				@"(?<E>=)|(?<eq>.*?)(?<v>\{((?<code>[\$\#\%\!\@])(?<name>.*?)|(?<name>(?<code>\[).*?\]))\})|(?<eq>.+$)",
				RegexOptions.Compiled | RegexOptions.ExplicitCapture);

			// validate an excell address
			RE[1] = new Regex(@"(?<name>(?i:([a-w]?[a-z]{1,2}|x[a-f][a-d])\d{1,7}))",
				RegexOptions.Compiled | RegexOptions.ExplicitCapture);

			// a cell, label name
			RE[2] = new Regex(@"(?<name>[a-zA-Z][a-zA-Z0-9]{1,23})",
				RegexOptions.Compiled | RegexOptions.ExplicitCapture);

			// an other name (parameter name)
			RE[3] = new Regex(@"(?<name>[a-zA-Z][a-zA-Z0-9]{1,23})",
				RegexOptions.Compiled | RegexOptions.ExplicitCapture);
		}

		public static void Tests()
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
				int result = labelNameValidate.Validate(test[0]);

				if (result > 0)
				{
					Debug.Write("testing| " + test[i] + " passed");
				} else
				{
					Debug.Write("testing| " + test[i] + " failed");
				}
			}

			Debug.WriteLine("done");
		}



		private class idValidation
		{
			private List<KeyValuePair<int, TestType>> testTypes;

			private int maxLen;

			private int currListPos = 0;

			public idValidation(int maxLen, List<KeyValuePair<int, TestType>> testTypes)
			{
				this.maxLen = maxLen;
				this.testTypes = testTypes;
			}

			// validate the charters in the test string
			// return
			// -1 if too long
			// 0 if Ok;
			// +# if fail; # == position failure
			public int Validate(string test)
			{
				int t = 0;
				int subLen = 0;

				bool result;

				char[] c = test.ToCharArray();

				if (c.Length > maxLen) return -1;

				for (var i = 0; i < c.Length; i++)
				{
					if (! validate(c[i], testTypes[t].Value)) return i;

					if (testTypes[t].Key >= 0 && ++subLen > testTypes[t].Key )
					{
						subLen = 0;
						t++;
					}
				}

				return 0;
			}


			private bool validate(char c, TestType type)
			{
				switch (type)
				{
				case TestType.ID_FIRST_CHAR:
					{
						idFirstChar.ValidateChar(c);
						break;
					}
				case TestType.ID_REMAIN_CHARS:
					{
						idRemainder.ValidateChar(c);
						break;
					}
				}

				return true;
			}

		}

		private class charValidation
		{
			private char[][] allowedRanges;

			private string allowedChars;
			private string disallowedChars;

			private bool disallowedOverlapsAllowed;

			private bool hasRanges;
			private bool hasAllowed;
			private bool hasDisallowed;


			public charValidation(string oKchars, char[][] ranges = null, string badChars = null, bool disallowedOverlapsAllowed = false)
			{
				allowedRanges = ranges;
				allowedChars = oKchars;
				disallowedChars = badChars;

				hasRanges = allowedRanges != null;
				hasAllowed = allowedChars != null;

				if (disallowedChars == null)
				{
					hasDisallowed = false;
					this.disallowedOverlapsAllowed = false;
				}
				else
				{
					hasDisallowed = true;
					this.disallowedOverlapsAllowed = disallowedOverlapsAllowed;
				}

				if (!hasRanges && !hasAllowed) throw new ArgumentException("allowed characters missing");

			}

			public bool ValidateChar(char c)
			{
				bool result = false;

				// determine if within the allowe ranges
				if (hasRanges)
				{
					result = validateRange(c);
				}

				if (hasAllowed && !result)
				{
					result = allowedChars.IndexOf(c) != -1;
				}

				if (!result) return false;

				// however, for flexibility, determine if is a disallowed char
				// when overlap is allowed
				if (disallowedOverlapsAllowed)
				{
					result = disallowedChars.IndexOf(c) == -1;
				}

				return result;
			}

			private bool validateRange(char c)
			{
				bool result = false;

				for (var j = 0; j < allowedRanges.GetLength(0); j++)
				{
					if (c >= allowedRanges[j][0] && c <= allowedRanges[j][1])
					{
						result = true;
						break;
					}
				}

				return result;
			}
		}
	}
}