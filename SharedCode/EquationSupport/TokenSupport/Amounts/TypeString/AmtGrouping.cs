using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtGpRef.cs
// Created:      2021-05-24 (10:18 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtGpRef : AmtTypeString
	{
		public AmtGpRef(string original) : base(original) { }

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
			return "This is| " + nameof(AmtGpRef) + " (" + AsString() + ")";
		}
	}

	public class AmtGpBeg : AmtTypeString
	{
		public AmtGpBeg(string original) : base(original) { }

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

	public class AmtGpEnd : AmtTypeString
	{
		public AmtGpEnd(string original) : base(original) { }

		// public override string AsString() => Amount;
		//
		// public override string ConvertFromString(string original, out bool isValid)
		// {
		// 	isValid = true;
		// 	return original;
		// }

		public override string ToString()
		{
			return "This is| " + nameof(AmtGpEnd) + " (" + AsString() + ")";
		}
	}


}