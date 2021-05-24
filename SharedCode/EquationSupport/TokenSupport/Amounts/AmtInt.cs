// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AmtInt.cs
// Created:      2021-05-22 (7:38 PM)

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{

	internal class AmtInt : AAmtBase<AmtInteger, int>
	{
	#region ctor

		public AmtInt(string original) : base(original) { }

	#endregion

	#region public properties

		public AmtInteger AI => amt;

		public static AmtInt Default => (AmtInt) AAmtBase<AmtInteger, int>.Default;

		public static AmtInt Invalid => (AmtInt) AAmtBase<AmtInteger, int>.Invalid;

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "This is| " + nameof(AmtInt) + " (" + AsString() + ")";
		}

	#endregion
	}

	internal class AmtInteger : IAmtTypeSpecific<int>
	{
		public int Amount { get; set; }

		public int ValueDefIdx { get; protected set; }

		public int DefaultAmt { get; } = int.MaxValue;
		public int InvalidAmt { get; } = int.MinValue;

		public void SetAmount(string original)
		{
			Amount = ConvertFromString(original);
		}

		public int ConvertFromString(string original)
		{
			int result;

			if (original == null)
			{
				return InvalidAmt;
			}

			if (int.TryParse(original, out result))
			{
				result = InvalidAmt;
			}

			return result;
		}

		public string AsString()
		{
			return Amount.ToString();
		}

		public override string ToString()
		{
			return "This is| " + nameof(AmtInteger) + " (" + AsString() + ")";
		}
	}


}