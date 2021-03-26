﻿// Solution:     SpreadSheet01
// Project:       Cells
// File:             IdValidation.cs
// Created:      2021-03-20 (2:52 PM)

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SharedCode.FormulaSupport.ParseSupport
{
	internal class IdValidation : AStringValidate
	{

		public IdValidation(int maxLen, List<KeyValuePair<int, TestType>> testTypes)
		{
			base.maxLen = maxLen;
			base.testTypes = testTypes;
		}

		protected override bool validate(char c, TestType type)
		{
			switch (type)
			{
			case TestType.ID_FIRST_CHAR:
				{
					return ParseUtil.IdFirstChar.ValidateChar(c);
				}
			case TestType.ID_REMAIN_CHARS:
				{
					return ParseUtil.IdRemainder.ValidateChar(c);
				}
			}

			return false;
		}

	}
	
	internal class LabelNameValidation : AStringValidate
	{
		public LabelNameValidation(int maxLen, List<KeyValuePair<int, TestType>> testTypes)
		{
			base.maxLen = maxLen;
			base.testTypes = testTypes;
		}

		protected override bool validate(char c, TestType type)
		{
			switch (type)
			{
			case TestType.ID_FIRST_CHAR:
				{
					return ParseUtil.IdFirstChar.ValidateChar(c);
				}
			case TestType.ID_REMAIN_CHARS:
				{
					return ParseUtil.IdRemainder.ValidateChar(c);
				}
			}

			return false;
		}

	}
	
	internal class ParamNameValidation : AStringValidate
	{
		public ParamNameValidation(int maxLen, List<KeyValuePair<int, TestType>> testTypes)
		{
			base.maxLen = maxLen;
			base.testTypes = testTypes;
		}

		protected override bool validate(char c, TestType type)
		{
			switch (type)
			{
			case TestType.PARAM_FIRST_CHAR:
				{
					return ParseUtil.ParamNameFirstChar.ValidateChar(c);
				}
			case TestType.PARAM_REMAIN_CHARS:
				{
					return ParseUtil.ParamNameRemainder.ValidateChar(c);
				}
			}

			return false;
		}

	}
	
	internal class ExcelAddrValidation : AStringValidate
	{
		public Tuple<int, char, TestType, TestStatusCode> Validate(string test)
		{
			Match m = ParseRegexSupport.ValidateExcelAddr(test);

			if (!m.Success)
			{
				return GeneralFail;
			}

			return Success;
		}

		protected override bool validate(char c, TestType type)
		{
			return false;
		}

	}
}