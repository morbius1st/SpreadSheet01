// Solution:     SpreadSheet01
// // projname: CellsTest// File:             charValidation.cs
// Created:      2021-03-20 (2:53 PM)

using System;

namespace SharedCode.FormulaSupport.ParseSupport
{
	internal class charValidation
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