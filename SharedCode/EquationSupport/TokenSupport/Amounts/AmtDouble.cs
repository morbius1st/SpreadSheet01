using SharedCode.EquationSupport.Definitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtDouble.cs
// Created:      2021-05-24 (10:19 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtDouble : AAmtTypeSpecific<double>
	{
		static AmtDouble()
		{
			ValueDefIdx = ValueDefinitions.Vd_NumDouble;
			Default =
				Make<AmtDouble>(DefaultInt, false, ValueDefinitions.ValDefInst[ValueDefIdx]);
			Invalid =
				Make<AmtDouble>(InvalidInt, false, ValueDefinitions.ValDefInst[ValueDefIdx]);
		}

		public AmtDouble() { }

		public AmtDouble(string original) : base(original) { }

		public override double AsDouble() => Amount;

		public static AmtDouble Default { get; }

		public static AmtDouble Invalid { get; }

		public override double ConvertFromString(string original)
		{
			double result;

			if (original == null)
			{
				return InvalidInt;
			}

			if (!double.TryParse(original, out result))
			{
				result = InvalidInt;
			}

			return result;
		}

		public override string ToString()
		{
			return "This is| " + nameof(AmtDouble) + " (" + AsString() + ")";
		}
	}
}