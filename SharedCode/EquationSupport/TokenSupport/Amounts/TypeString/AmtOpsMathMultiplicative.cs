using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtOpAdd.cs
// Created:      2021-05-24 (10:18 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{

	public class AmtOpsMathMultiplicative : AmtTypeString
	{
		public AmtOpsMathMultiplicative(string original) : base(original) { }
	}

	// public class AmtOpAdd : AmtTypeString
	// {
	// 	public AmtOpAdd(string original) : base(Vd_MathAdd, original) { }
	// }
	//
	// public class AmtOpSub : AmtTypeString
	// {
	// 	public AmtOpSub(string original) : base(Vd_MathSubt, original) { }
	// }

}