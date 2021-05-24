#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

#endregion

// username: jeffs
// created:  5/21/2021 12:47:43 AM

namespace SharedCode.EquationSupport.TokenSupport.Values
{
	public class ValDouble : AValBase<double>
	{
	#region private fields

	#endregion

	#region ctor

		public ValDouble(string original, bool isValid)
		{
			Original = original;
			IsValid = isValid;
			ValueDef = SetValueDef();

			Amount = ConvertFromString(original);
		}

	#endregion

	#region public properties

		public override string Original { get; set; }
		public override double Amount { get; set; }

		public new static double DefaultAmt { get; } = double.MaxValue;
		public new static double InvalidAmt { get; } = double.MinValue;

		public new static ValDouble Default => new ValDouble(null, true) {Amount = DefaultAmt};
		public new static ValDouble Invalid => new ValDouble(null, false) {Amount = InvalidAmt};
		//
		// public override ValueDef ValueDef { get; set; }
		//
		// public override bool IsValid { get; set; }

	#endregion

	#region private properties

	#endregion

	#region public methods

		public override double AsDouble()
		{
			return Amount;
		}

		public override string AsString()
		{
			return Amount.ToString();
		}

		public override double ConvertFromString(string original)
		{
			if (original == null)
			{
				if (IsValid == true)
				{
					return DefaultAmt;
				}
				else
				{
					return InvalidAmt;
				}
			}


			double result;
			if (double.TryParse(original, out result))
			{
				result = InvalidAmt;
			}

			return result;
		}

		private ValueDef SetValueDef()
		{
			if (Original == null)
			{
				if (IsValid == true)
				{
					return VdefInst.Default;
				}
				else
				{
					return VdefInst.Invalid;
				}
			}

			return VdefInst[Vd_NumDouble];
		}

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is| " + nameof(ValDouble) + " (" + Amount + ")";
		}

	#endregion
	}
}