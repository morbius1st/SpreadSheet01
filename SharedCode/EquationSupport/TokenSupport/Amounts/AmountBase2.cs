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

using ValueType = SharedCode.EquationSupport.Definitions.ValueType;

#endregion

// username: jeffs
// created:  5/22/2021 6:41:52 AM

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public abstract class IAmtBase2
	{
		public string Original { get; set; }             // the original value
		public ValueDef ValueDef { get; protected set; } // the definition for the value
		public static int ValueDefIdx { get; protected set; }   // index to the specific value definition

		// from ValueDef
		public string ValueString => ValueDef.ValueStr;
		public ValueType DataType => ValueDef.ValueType; // the type of the value
		public int Id => ValueDef.Id;                    // number of the definition order
		public int Seq => ValueDef.Seq;                  // the sequence number within a value def group
		public int Order => ValueDef.Order;              // the order of operation / precedence order / higher gets done first
		public string Description => ValueDef.Description;

		public bool IsValid { get; protected set; }      // whether this item is valid

		public ValueDef SetValueDef(string original, int idx)
		{
			if (original == null) return VdefInst.Invalid;

			ValueDefinitions a = VdefInst;

			return VdefInst[idx];
		}

		public abstract string AsString();

		// depending on the value definition
		public virtual bool AsBool() => DefaultBool;
		public virtual double AsDouble() =>DefaultDouble;
		public virtual int AsInteger() =>DefaultInt;
		public virtual object AsObject() =>DefaultObj;
		public virtual UoM AsUnit() => DefaultUnit;

		public static bool   DefaultBool = false;
		public static double DefaultDouble = double.PositiveInfinity;
		public static int    DefaultInt = int.MinValue;
		public static object DefaultObj = null;
		public static UoM    DefaultUnit = null;

		public static bool   InvalidBool = false;
		public static double InvalidDouble = double.NegativeInfinity;
		public static int    InvalidInt = int.MaxValue;
		public static object InvalidObj = null;
		public static UoM    InvalidUnit = null;
	}

	public abstract class IAmtTypeSpecific2<T> : IAmtBase2
	{
		public IAmtTypeSpecific2() {}

		public IAmtTypeSpecific2(string original)
		{
			Original = original;
			Amount = SetAmount(original);
			ValueDef = SetValueDef(original, ValueDefIdx);
		}

		protected static U Make<U>(T amount, bool valid, ValueDef valDef)
			where U:  IAmtTypeSpecific2<T>, new()
		{
			U ai = new U();
			ai.Original = null;
			ai.Amount = amount;
			ai.IsValid = valid;
			ai.ValueDef = valDef;

			return ai;
		}

		public T Amount { get; protected set; } // the original value converted to its native value

		public abstract T ConvertFromString(string original); // the conversion method

		public T SetAmount(string original)
		{
			return ConvertFromString(original);
		}

		public override string AsString()
		{
			return Amount.ToString();
		}
	}

	public class AmtInteger2 : IAmtTypeSpecific2<int>
	{
		static AmtInteger2()
		{
			ValueDefIdx = Vd_NumInt;

			Default = Make<AmtInteger2>(DefaultInt, false, VdefInst[ValueDefIdx]);
			Invalid = Make<AmtInteger2>(InvalidInt, false, VdefInst[ValueDefIdx]);
		}

		public AmtInteger2() { } 

		public AmtInteger2(string original) : base(original) { }

		public override int AsInteger() => Amount;

		public static AmtInteger2 Default { get; }

		public static AmtInteger2 Invalid { get; }

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
			return "This is| " + nameof(AmtInteger2) + " (" + AsString() + ")";
		}
	}

	public class AmtDouble2 : IAmtTypeSpecific2<double>
	{
		static AmtDouble2()
		{
			ValueDefIdx = Vd_NumDouble;
			Default =
				Make<AmtDouble2>(DefaultInt, false, VdefInst[ValueDefIdx]);
			Invalid =
				Make<AmtDouble2>(InvalidInt, false, VdefInst[ValueDefIdx]);
		}
		public AmtDouble2() { }

		public AmtDouble2(string original) : base(original) { }

		public override double AsDouble() => Amount;

		public static AmtDouble2 Default { get; }

		public static AmtDouble2 Invalid { get; }

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
			return "This is| " + nameof(AmtDouble2) + " (" + AsString() + ")";
		}
	}

}