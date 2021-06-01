﻿using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtOpAdd.cs
// Created:      2021-05-24 (10:18 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtOpAdd : AAmtTypeString
	{
		static AmtOpAdd()
		{
			// ValueDefIdx = ValueDefinitions.Vd_MathAdd;
		}

		public AmtOpAdd(string original) : base(Vd_MathAdd, original) { }

		// public override string AsString() => Amount;
		//
		// public override string ConvertFromString(string original)
		// {
		// 	return original;
		// }

		public override string ToString()
		{
			return "This is| " + nameof(AmtOpAdd) + " (" + AsString() + ")";
		}
	}
}