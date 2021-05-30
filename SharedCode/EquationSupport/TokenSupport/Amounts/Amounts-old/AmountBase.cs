#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.TokenSupport.Values;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;
using ValueType = SharedCode.EquationSupport.Definitions.ValueType;

#endregion

// username: jeffs
// created:  5/22/2021 6:41:52 AM

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public interface IAmtBase
	{
		string Original { get; set; } // the original value
		ValueDef ValueDef { get; }    // the definition for the value
		ValueType DataType { get; }   // the type of the value

		// from ValueDef
		string ValueString { get; } // the value as a string
		int Id { get; }             // the Id of the value| simple numerical identifier
		int Seq { get; }            // a sequence number| varies per ValueDefGroup
		int Order { get; }          // the precedence for the value
		string Description { get; } // the description of the value

		bool IsValid { get; } // whether this item is valid

		// depending on the value definition
		string AsString(); // value as a string
		object AsObject(); // value as an object
		bool AsBool();     // value as a bool
		int AsInteger();   // value as an integer
		double AsDouble(); // value as a double
		UoM AsUnit();      // value as a unit of measure
	}

	public interface IAmtTypeSpecific<T>
	{
		int ValueDefIdx { get; } // index to the specific value definition

		T Amount { get; set; } // the original value converted to its native value

		T DefaultAmt { get; } // a default value
		T InvalidAmt { get; } // value when invalid

		T ConvertFromString(string original); // the conversion method

		string AsString();

		void SetAmount(string original);
	}

	public class AAmtBase<T, U> : IAmtBase where T : IAmtTypeSpecific<U>, new()
	{
		protected T amt;
		public AAmtBase() { }

		public AAmtBase(string original)
		{
			Original = original;

			amt = new T();
			amt.SetAmount(original);

			ValueDef = SetValueDef(original, amt.ValueDefIdx);
		}

		public string Original { get; set; }             // the original value
		public ValueDef ValueDef { get; protected set; } // the definition for the value
		public ValueType DataType => ValueDef.ValueType;

		// from ValueDef
		public string ValueString => ValueDef.ValueStr;
		public int Id => ValueDef.Id;       // number for the definition order
		public int Seq => ValueDef.Seq;     // the sequence number within a value def group
		public int Order => ValueDef.Order; // the order of operation / precedence order / higher gets done first
		public string Description => ValueDef.Description;

		public bool IsValid { get; private set; } // whether this item is valid

		private ValueDef SetValueDef(string original, int idx)
		{
			if (original == null) return VdefInst.Invalid;

			return VdefInst[idx];
		}

		public string AsString() => amt.AsString();

		// depending on the value definition
		public virtual bool AsBool()
		{
			return DefaultBool;
		} // value as a string

		public virtual double AsDouble()
		{
			return DefaultDouble;
		} // value as an object

		public virtual int AsInteger()
		{
			return DefaultInt;
		} // value as a bool

		public virtual object AsObject()
		{
			return DefaultObj;
		} // value as an integer

		public virtual UoM AsUnit()
		{
			return DefaultUnit;
		} // value as a unit

		public static bool DefaultBool = false;
		public static double DefaultDouble = double.NaN;
		public static int DefaultInt = int.MinValue;
		public static object DefaultObj = null;
		public static UoM DefaultUnit = null;

		public override string ToString()
		{
			return amt.ToString();
		}

		public static AAmtBase<T, U> Default
		{
			get
			{
				AAmtBase<T, U> aaBase = new AAmtBase<T, U>(null);
				aaBase.amt.Amount = aaBase.amt.DefaultAmt;
				return aaBase;
			}
		}

		public static AAmtBase<T, U> Invalid
		{
			get
			{
				AAmtBase<T, U> aaBase = new AAmtBase<T, U>(null);
				aaBase.amt.Amount = aaBase.amt.InvalidAmt;
				return aaBase;
			}
		}
	}

	internal class AmtDbl : AAmtBase<AmtDouble, int>
	{
	#region ctor

		public AmtDbl(string original) : base(original) { }

	#endregion

	#region public properties

		public AmtDouble AI => amt;

		public static AmtDbl Default => (AmtDbl) AAmtBase<AmtDouble, int>.Default;

		public static AmtDbl Invalid => (AmtDbl) AAmtBase<AmtDouble, int>.Invalid;

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "This is| " + nameof(AmtDouble) + " (" + AsString() + ")";
		}

	#endregion
	}

	internal class AmtDouble : IAmtTypeSpecific<int>
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
			return "This is| " + nameof(AmtDouble) + " (" + AsString() + ")";
		}
	}


}