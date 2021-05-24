#region using

using System;
using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

#endregion

// username: jeffs
// created:  5/21/2021 12:47:43 AM

namespace SharedCode.EquationSupport.TokenSupport.Values
{

	public abstract class ValGrps : AValBase<string>
	{

	#region private fields

		protected abstract int ValDefIdx { get; }

	#endregion

	#region ctor

		protected ValGrps(string original, bool isValid)
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

	public class ValGrpRef : ValOps
	{
	#region private fields

	#endregion

	#region ctor

		public ValGrpRef(string original, bool isValid) : base(original, isValid) { }

	#endregion

	#region public properties

		public new static ValGrpRef Default => new ValGrpRef(null, true) {Amount = DefaultAmt};
		public new static ValGrpRef Invalid => new ValGrpRef(null, false) {Amount = InvalidAmt};

	#endregion
	#region private properties

		protected override int ValDefIdx { get; } = Vd_GrpRef;

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is| " + nameof(ValGrpRef);
		}

	#endregion
	}
	
	public class ValGrpBeg : ValOps
	{
	#region private fields

	#endregion

	#region ctor

		public ValGrpBeg(string original, bool isValid) : base(original, isValid) { }

	#endregion

	#region public properties

		public new static ValGrpBeg Default => new ValGrpBeg(null, true) {Amount = DefaultAmt};
		public new static ValGrpBeg Invalid => new ValGrpBeg(null, false) {Amount = InvalidAmt};

	#endregion
	#region private properties

		protected override int ValDefIdx { get; } = Vd_GrpBeg;

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is| " + nameof(ValGrpBeg);
		}

	#endregion
	}

}