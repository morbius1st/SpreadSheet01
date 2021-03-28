// Solution:     SpreadSheet01
// // projname: CellsTest// File:             IStringValidate.cs
// Created:      2021-03-20 (2:53 PM)

using System;
using System.Collections.Generic;

namespace SharedCode.FormulaSupport.ParseSupport
{
	interface IStringValidate
	{
		Tuple<int, char, TestType, TestStatusCode> Validate(string test);
	}

	internal abstract class AStringValidate : IStringValidate
	{
		public static Tuple<int, char, TestType, TestStatusCode>
			Success => new Tuple<int, char, TestType, TestStatusCode>(0, (char) 0, TestType.NONE, TestStatusCode.PASS);

		public static Tuple<int, char, TestType, TestStatusCode>
			GeneralFail => new Tuple<int, char, TestType, TestStatusCode>(-1, (char) 0, TestType.NONE, TestStatusCode.FAIL_GENERAL);

		protected List<KeyValuePair<int, TestType>> testTypes;

		protected int maxLen;

		public virtual Tuple<int, char, TestType, TestStatusCode> Validate(string test)
		{
			int t = 0;
			int subLen = 0;

			bool result;

			char[] c = test.ToCharArray();

			if (c.Length > maxLen) return 
				new Tuple<int, char, TestType, TestStatusCode>(-1, (char) 0, TestType.NONE, TestStatusCode.FAIL_TOO_LONG);

			for (var i = 0; i < c.Length; i++)
			{
				if (! validate(c[i], testTypes[t].Value)) return 
					new Tuple<int, char, TestType, TestStatusCode>(i, c[i], testTypes[t].Value, TestStatusCode.FAIL_INVALID_CHAR);

				if (testTypes[t].Key >= 0 && ++subLen >= testTypes[t].Key )
				{
					subLen = 0;
					t++;
				}
			}

			return Success;
		}

		protected abstract bool validate(char c, TestType type);
	}

}