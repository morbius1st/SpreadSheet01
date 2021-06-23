// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             AAmtTypeSpecific.cs
// Created:      2021-06-17 (6:31 PM)

using System;
using System.Runtime.Remoting.Messaging;
using SharedCode.EquationSupport.Definitions;

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{
	public abstract class AAmtTypeSpecific<T> : AAmtBase
	{
		public AAmtTypeSpecific() {}

		public AAmtTypeSpecific(string original)
		{
			bool isValid;

			Original = original;
			Amount = SetAmount(original, out isValid);
			// ValDef = SetValueDef(original, index);
			IsValid = isValid;
		}

		public T Amount { get; protected set; } // the original value converted to its native value

		public abstract T ConvertFromString(string original, out bool isValid);

		public T SetAmount(string original, out bool isValid)
		{
			return ConvertFromString(original, out isValid);
		}

		public override string AsString()
		{
			return Amount.ToString();
		}

		public override string ToString()
		{
			return $"this is| {this.GetType().Name} ({Amount})";
		}
	}
}