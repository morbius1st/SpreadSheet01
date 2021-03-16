// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             DynamicValue.cs
// Created:      2021-03-03 (6:46 PM)

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class DynamicValue : INotifyPropertyChanged
	{
		private dynamic dynamicValue; // base value, as provided // as a number from excel
		private string formatString;  // 
		private string preFormatted;  // as provided directly from excel

		public DynamicValue(dynamic value)
		{
			dynamicValue = value;
		}

		public DynamicValue() { }

		public dynamic Value
		{
			get => dynamicValue;
			set
			{
				dynamicValue = value;
				OnPropertyChanged();
			}
		}

		public string AsPreFormatted() => preFormatted;

		public string AsFormatted() => dynamicValue?.ToString(formatString);

		public string AsString() => dynamicValue?.ToString() ?? null;

		public double AsDouble() => (dynamicValue ?? null) == typeof(double) ? dynamicValue : Double.NaN;

		public int AsInteger() => (dynamicValue ?? null) == typeof(int) ? dynamicValue : Int32.MaxValue;

		public bool AsBool() => (dynamicValue ?? null) == typeof(bool) ? dynamicValue : false;

		public Type BaseType() => dynamicValue?.GetType() ?? null;

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return "DynamicValue| " + dynamicValue.ToString();
		}
	}
}