using SharedCode.EquationSupport.Definitions;

// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtInteger.cs
// Created:      2021-05-24 (10:18 AM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public class AmtInteger : AAmtTypeSpecific<int>
	{
		static AmtInteger()
		{
			ValueDefIdx = ValueDefinitions.Vd_NumInt;

			Default = Make<AmtInteger>(DefaultInt, false, ValueDefinitions.ValDefInst[ValueDefIdx]);
			Invalid = Make<AmtInteger>(InvalidInt, false, ValueDefinitions.ValDefInst[ValueDefIdx]);
		}

		public AmtInteger() { } 

		public AmtInteger(string original) : base(original) { }

		public override int AsInteger() => Amount;

		public static AmtInteger Default { get; }

		public static AmtInteger Invalid { get; }

		public override int ConvertFromString(string original)
		{
			int result;

			if (original == null)
			{
				return InvalidInt;
			}

			if (!int.TryParse(original, out result))
			{
				result = InvalidInt;
			}

			return result;
		}

		public override string ToString()
		{
			return "This is| " + nameof(AmtInteger) + " (" + AsString() + ")";
		}
	}
}