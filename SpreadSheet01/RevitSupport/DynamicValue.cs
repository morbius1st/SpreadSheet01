﻿// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             DynamicValue.cs
// Created:      2021-03-03 (6:46 PM)

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SpreadSheet01.RevitSupport
{
	public class DynamicValue : INotifyPropertyChanged
	{
		private dynamic dynamicValue; // base value, as provided // as a number from excel
		private string formatString;  // 
		private string preFormatted;  // as provided directly from excel

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

		public string AsFormatted() => dynamicValue.ToString(formatString);

		public string AsString() => dynamicValue.ToString();

		public double AsDouble() => dynamicValue == typeof(double) ? dynamicValue : Double.NaN;

		public double AsInteger() => dynamicValue == typeof(int) ? dynamicValue : Int32.MaxValue;

		public double AsBool() => dynamicValue == typeof(bool) ? dynamicValue : false;

		public Type BaseType() => dynamicValue.GetType();


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return "this is DynamicValue";
		}
	}
}