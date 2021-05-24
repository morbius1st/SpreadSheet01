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
	public class ValInteger : AValBase<int>
	{
	#region ctor

		public ValInteger(string original, bool isValid)
		{
			Original = original.Trim();
			IsValid = isValid;
			ValueDef = SetValueDef();

			Amount = ConvertFromString(original);
		}

	#endregion

	#region public properties

		public override string Original { get; set; }
		public override int Amount { get; set; }

		public new static  int DefaultAmt { get; } = int.MaxValue;
		public new static  int InvalidAmt { get; } = int.MinValue;

		public new static ValInteger Default => new ValInteger(null, true) {Amount = DefaultAmt};
		public new static ValInteger Invalid => new ValInteger(null, false) {Amount = InvalidAmt};

	#endregion

	#region public methods

		public override int AsInteger()
		{
			return Amount;
		}

		public override string AsString()
		{
			return Amount.ToString();
		}

		public override int ConvertFromString(string original)
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

			int result;
			if (int.TryParse(original, out result))
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

			return VdefInst[Vd_NumInt];
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is| " + nameof(ValInteger) + " (" + Amount + ")";
		}

	#endregion
	}
}