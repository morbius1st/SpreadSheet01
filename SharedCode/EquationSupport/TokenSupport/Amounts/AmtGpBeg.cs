using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtGpBeg.cs
// Created:      2021-05-24 (10:18 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtGpBeg : AAmtTypeString
	{



		static AmtGpBeg()
		{
			// ValueDefIdx = ValueDefinitions.Vd_GrpBeg;
		}

		public AmtGpBeg(string original) : base(Vd_GrpBeg, original) { }

		// public override string AsString() => Amount;
		//
		// public override string ConvertFromString(string original, out bool isValid)
		// {
		// 	isValid = true;
		//
		// 	return original;
		// }

		public override string ToString()
		{
			return "This is| " + nameof(AmtGpBeg) + " (" + AsString() + ")";
		}
	}
}