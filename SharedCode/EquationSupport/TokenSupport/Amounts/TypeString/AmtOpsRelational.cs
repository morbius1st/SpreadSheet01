using System.Reflection;
using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtOpAdd.cs
// Created:      2021-05-24 (10:18 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtOpsRelational : AmtTypeString
	{
		public AmtOpsRelational(string original) : base(original) { }
	}

	// public class AmtOpMathRelLt : AmtTypeString
	// {
	// 	public AmtOpMathRelLt(string original) : base(Vd_RelLt, original) { }
	// }
	//
	// public class AmtOpMathRelLtEq : AmtTypeString
	// {
	// 	public AmtOpMathRelLtEq(string original) : base(Vd_RelLtEq, original) { }
	// }
	//
	// public class AmtOpMathRelGt : AmtTypeString
	// {
	// 	public AmtOpMathRelGt(string original) : base(Vd_RelGt, original) { }
	// }
	//
	// public class AmtOpMathRelGtEq : AmtTypeString
	// {
	// 	public AmtOpMathRelGtEq(string original) : base(Vd_RelGtEq, original) { }
	// }

}