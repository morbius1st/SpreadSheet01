using EquationSupport.Definitions.AmountDefs;
using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

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
			// ValueDefIdx = ValueDefinitions.Vd_NumInt;

			Default = Make<AmtInteger>(DefaultInt, false, (DefNumInt) ValueDefinitions.ValDefInst[Vd_NumInt]);
			Invalid = Make<AmtInteger>(InvalidInt, false, (DefNumInt) ValueDefinitions.ValDefInst[Vd_NumInt]);
		}

		public AmtInteger() { } 

		public AmtInteger(string original) : base(Vd_NumInt, original) { }

		public override int AsInteger() => Amount;

		public static AmtInteger Default { get; }

		public static AmtInteger Invalid { get; }

		public override int ConvertFromString(string original, out bool isValid)
		{
			isValid = false;

			int result;

			if (original == null)
			{
				return InvalidInt;
			}

			if (!int.TryParse(original, out result))
			{
				result = InvalidInt;
			}

			isValid = true;

			return result;
		}

		public override string ToString()
		{
			return "This is| " + nameof(AmtInteger) + " (" + AsString() + ")";
		}
	}
}