using SharedCode.EquationSupport.EqSupport.ValueSupport;
using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtDouble.cs
// Created:      2021-05-24 (10:19 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtUnit : AAmtTypeSpecific<double>
	{
		// the underlying data type
		public override ValueDataGroup DataGroup => ValueDataGroup.VDG_UNIT;

		public static AValDefBase ValDef { get; protected set; } // the definition for the value

		static AmtUnit()
		{
			ValDef = ValDefInst[Vd_NumUntLenImp];
		}

		public AmtUnit() { }

		public AmtUnit(string original) : base(original) { }

		public override double AsDouble() => Amount;

		public override double ConvertFromString(string original, out bool isValid)
		{
			isValid = false;

			double result;

			if (original == null)
			{
				return NumSupport.InvalidDouble;
			}

			if (!double.TryParse(original, out result))
			{
				result = NumSupport.InvalidDouble;
			}

			isValid = true;
			return result;
		}

		public override string ToString()
		{
			return "This is| " + this.GetType().Name + " (" + AsString() + ")";
		}
	}

	// public class AmtUnit : AmtTypeString
	// {
	// 	public AmtUnit(string original) : base(original) { }
	// }
}