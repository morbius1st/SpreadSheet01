// Solution:     SpreadSheet01
// Project:       Tests
// File:             DynamicValue2.cs
// Created:      2021-02-21 (6:37 AM)

using System;

namespace Tests
{
	class DynamicValue2Bool : ADynamicValue2
	{
		public DynamicValue2Bool(bool? value)
		{
			base.Set(value); 
			Set(value);
		}

		private void Set(bool? value)
		{
			gotValue = false;
			Console.WriteLine("I got a bool");
		}

		public override dynamic Get => (bool?) value;
	}

	class DynamicValue2Double : ADynamicValue2
	{
		public DynamicValue2Double(double? value)
		{
			base.Set(value);
			Set(value);
		}

		private void Set(double? value)
		{
			gotValue = false;
			Console.WriteLine("I got a double");
		}

		public override dynamic Get => (double?) value;
	}

	class DynamicValue2Text : ADynamicValue2
	{
		public DynamicValue2Text(string value)
		{
			Console.WriteLine("   ctor T1");
			base.Set(value);
			Console.WriteLine("   ctor T5");
			Set(value);
			Console.WriteLine("   ctor T9");
		}

		private void Set(string value)
		{
			Console.WriteLine("   set T1");
			gotValue = false;
			Console.WriteLine("   set T9");
			Console.WriteLine("I got a string");
		}

		public override dynamic Get => (string) value;
	}

	class DynamicValue2String : DynamicValue2<string>
	{
		public DynamicValue2String(string value) : base(value)
		{
			Console.WriteLine("   ctor S1");

			this.Set(value);
			Console.WriteLine("   ctor S9");
		}

		public void Set(string value)
		{
			Console.WriteLine("   set S1");
			gotValue = false;
			Console.WriteLine("   set S9");
			Console.WriteLine("I got a string 2");
		}

		public override dynamic Get => (string) value;
	}

	class DynamicValue2<T> : ADynamicValue2
	{
		public DynamicValue2(T value)
		{
			Console.WriteLine("   ctor D1");
			base.Set(value);
			Console.WriteLine("   ctor D9");
		}

		public override dynamic Get => (T) value;
	}

	abstract class ADynamicValue2
	{
		protected dynamic value;
		protected bool gotValue;

		public abstract dynamic Get { get; }

		public void Set<T>(T value)
		{
			Console.WriteLine("   set<T> A1");

			if (gotValue)
			{
				gotValue = false;
				return;
			}

			this.value = value;
			Assigned = true;

			gotValue = true;

			Console.WriteLine("   set<T> A9");
			Console.WriteLine("I got a value");
		}

		public bool Assigned { get; set; }
	}
}