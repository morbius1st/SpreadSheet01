﻿// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             DynamicValue.cs
// Created:      2021-03-03 (6:46 PM)

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UtilityLibrary;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class DynamicValue : INotifyPropertyChanged
	{
		private dynamic dynamicValue; // base value, as provided // as a number from excel
#pragma warning disable CS0649 // Field 'DynamicValue.formatString' is never assigned to, and will always have its default value null
		private string formatString;  // 
#pragma warning restore CS0649 // Field 'DynamicValue.formatString' is never assigned to, and will always have its default value null
#pragma warning disable CS0649 // Field 'DynamicValue.preFormatted' is never assigned to, and will always have its default value null
		private string preFormatted;  // as provided directly from excel
#pragma warning restore CS0649 // Field 'DynamicValue.preFormatted' is never assigned to, and will always have its default value null

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
			string result =  dynamicValue == null ? "is null" : dynamicValue.ToString();
			result = result.IsVoid() ? "is empty" : result;
			return "DynamicValue| " + result;
		}
	}
}