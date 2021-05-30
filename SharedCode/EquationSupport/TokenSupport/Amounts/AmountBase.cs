﻿#region using
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
	public abstract class AAmtBase
	{
		public string Original { get; set; }             // the original value
		public DefValue ValueDef { get; protected set; } // the definition for the value
		public static int ValueDefIdx { get; protected set; }   // index to the specific value definition

		// from ValueDef
		public string ValueString => ValueDef.ValueStr;
		public ValueType DataType => ValueDef.ValueType; // the type of the value
		public int Id => ValueDef.Id;                    // number of the definition order
		public int Seq => ValueDef.Seq;                  // the sequence number within a value def group
		public int Order => ValueDef.Order;              // the order of operation / precedence order / higher gets done first
		public string Description => ValueDef.Description;

		public bool IsValid { get; protected set; }      // whether this item is valid

		public DefValue SetValueDef(string original, int idx)
		{
			if (original == null) return ValDefInst.Invalid;

			ValueDefinitions a = ValDefInst;

			return ValDefInst[idx];
		}

		public static AAmtBase Invalid => (AAmtBase) new AmtInvalid();

		public abstract string AsString();

		// depending on the value definition
		public virtual bool AsBool() => DefaultBool;
		public virtual double AsDouble() =>DefaultDouble;
		public virtual int AsInteger() =>DefaultInt;
		public virtual object AsObject() =>DefaultObj;
		public virtual UoM AsUnit() => DefaultUnit;

		public static bool   DefaultBool = false;
		public static string DefaultString = string.Empty;
		public static double DefaultDouble = double.PositiveInfinity;
		public static int    DefaultInt = int.MinValue;
		public static object DefaultObj = null;
		public static UoM    DefaultUnit = null;

		public static bool   InvalidBool = false;
		public static string InvalidString = null;
		public static double InvalidDouble = double.NegativeInfinity;
		public static int    InvalidInt = int.MaxValue;
		public static object InvalidObj = null;
		public static UoM    InvalidUnit = null;
	}

	public abstract class AAmtTypeSpecific<T> : AAmtBase
	{
		public AAmtTypeSpecific() {}

		public AAmtTypeSpecific(string original)
		{
			Original = original;
			Amount = SetAmount(original);
			ValueDef = SetValueDef(original, ValueDefIdx);
		}

		protected static U Make<U>(T amount, bool valid, DefValue valDef)
			where U:  AAmtTypeSpecific<T>, new()
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
}