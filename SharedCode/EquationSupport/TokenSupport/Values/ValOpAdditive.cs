#region using

using System;
using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

#endregion

// username: jeffs
// created:  5/21/2021 12:47:43 AM

namespace SharedCode.EquationSupport.TokenSupport.Values
{
	public abstract class ValOps : AValBase<string>
	{

	#region private fields

		protected abstract int ValDefIdx { get; }

	#endregion

	#region ctor

		protected ValOps(string original, bool isValid)
		{
			Original = original.Trim();
			IsValid = isValid;
			ValueDef = SetValueDef();

			Amount = ConvertFromString(original);
		}

	#endregion

	#region public properties

		public override string Original { get; set; }
		public override string Amount { get; set; }

		public new static string DefaultAmt { get; } = String.Empty;
		public new static string InvalidAmt { get; } = null;

	#endregion

	#region public methods

		public override string AsString()
		{
			return Amount;
		}

		public override string ConvertFromString(string original)
		{
			if (original == null) return DefaultAmt;

			return original;
		}

	#endregion
	
	#region public methods
	
		protected ValueDef SetValueDef()
		{
			if (Original != null && Original.Equals(VdefInst.ValueStr(ValDefIdx)))
			{
				return VdefInst[ValDefIdx];
			}

			if (IsValid == true)
			{
				return VdefInst.Default;
			}

			return VdefInst.Invalid;
		}
	
	#endregion

	}

	public class ValOpAdd : ValOps
	{
	#region private fields

	#endregion

	#region ctor

		public ValOpAdd(string original, bool isValid) : base(original, isValid)
		{
			// Original = original.Trim();
			// IsValid = isValid;
			// ValueDef = SetValueDef();
			//
			// Amount = ConvertFromString(original);
		}

	#endregion

	#region public properties

		// public override string Original { get; set; }
		// public override string Amount { get; set; }
		//
		// public new static string DefaultAmt { get; } = String.Empty;
		// public new static string InvalidAmt { get; } = null;
		//
		public new static ValOpAdd Default => new ValOpAdd(null, true) {Amount = DefaultAmt};
		public new static ValOpAdd Invalid => new ValOpAdd(null, false) {Amount = InvalidAmt};

	#endregion

	#region private properties

		protected override int ValDefIdx { get; } = Vd_MathAdd;

	#endregion

	#region public methods

		// public override string AsString()
		// {
		// 	return Amount;
		// }
		//
		// public override string ConvertFromString(string original)
		// {
		// 	if (original == null) return DefaultAmt;
		//
		// 	return original;
		// }
		//
		// private ValueDef SetValueDef()
		// {
		// 	if (Original != null && Original.Equals(VdefInst.ValueStr(Vd_MathAdd)))
		// 	{
		// 		return VdefInst[Vd_MathAdd];
		// 	}
		//
		// 	if (IsValid == true)
		// 	{
		// 		return VdefInst.Default;
		// 	}
		//
		// 	return VdefInst.Invalid;
		// }

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
			return "this is| " + nameof(ValOpAdd) + " (" + Amount + ")";
		}

	#endregion
	}
	
	public class ValOpSubt : ValOps
	{
	#region ctor

		public ValOpSubt(string original, bool isValid) : base(original, isValid)
		{ }

	#endregion

	#region public properties		

		public new static ValOpSubt Default => new ValOpSubt(null, true) {Amount = DefaultAmt};
		public new static ValOpSubt Invalid => new ValOpSubt(null, false) {Amount = InvalidAmt};
		
	#endregion

	#region private properties

		protected override int ValDefIdx { get; } = Vd_MathSubt;

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is| " + nameof(ValOpSubt) + " (" + Amount + ")";
		}

	#endregion
	}
}