using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtDouble.cs
// Created:      2021-05-24 (10:19 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtDouble : AAmtTypeSpecific<double>
	{
		public static ADefBase2 ValueDef2 { get; protected set; } // the definition for the value

		static AmtDouble()
		{
			ValueDef2 = ValDefInst[Vd_NumDouble];

			// ValueDefIdx = ValueDefinitions.Vd_NumDouble;
			Default =
				Make<AmtDouble>(DefaultInt, false, (ValDef) ValDefInst[Vd_NumDouble]);
			Invalid =
				Make<AmtDouble>(InvalidInt, false, (ValDef) ValDefInst[Vd_NumDouble]);
		}

		public AmtDouble() { }

		public AmtDouble(string original) : base(Vd_NumDouble, original) { }

		public override double AsDouble() => Amount;

		public static AmtDouble Default { get; }

		public static AmtDouble Invalid { get; }

		public override double ConvertFromString(string original, out bool isValid)
		{
			isValid = false;

			double result;

			if (original == null)
			{
				return InvalidInt;
			}

			if (!double.TryParse(original, out result))
			{
				result = InvalidInt;
			}

			isValid = true;
			return result;
		}

		public override string ToString()
		{
			return "This is| " + nameof(AmtDouble) + " (" + AsString() + ")";
		}
	}
}