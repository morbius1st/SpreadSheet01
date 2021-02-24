// Solution:     SpreadSheet01
// Project:       Tests
// File:             DynamicTests.cs
// Created:      2021-02-21 (6:35 AM)

using System;

namespace Tests
{
	class DynamicTests
	{
		public void Process()
		{
			Test1();
		}

		private void Test2()
		{

		}


	#region dynamic test 1

		private void Test1()
		{
			ADynamicValue2[] values = new ADynamicValue2[20];

			int idx = 0;

			string t = "string";
			Console.WriteLine("setting a string");
			DynamicValue2Text dt = new DynamicValue2Text(t);
			values[idx++] = dt;

			double d = 1.0;
			Console.WriteLine("setting a double");
			DynamicValue2Double dd = new DynamicValue2Double(d);
			values[idx++] = dd;

			int b = 1;
			Console.WriteLine("setting a bool");
			DynamicValue2Bool db = new DynamicValue2Bool(b == 1 ? true : false);
			values[idx++] = db;

			int i = 1;
			Console.WriteLine("setting a int");
			DynamicValue2<int> di = new DynamicValue2<int>(i);
			values[idx++] = di;

			string s = "string 2";
			Console.WriteLine("setting a string 2");
			DynamicValue2String ds = new DynamicValue2String(s);
			values[idx++] = ds;


			

			Console.WriteLine("setting a string as null");
			DynamicValue2Text dtn = new DynamicValue2Text(null);
			values[idx++] = dtn;

			Console.WriteLine("setting a double as null");
			DynamicValue2Double ddn = new DynamicValue2Double(null);
			values[idx++] = ddn;
			
			Console.WriteLine("setting a bool as null");
			DynamicValue2Bool dbn = new DynamicValue2Bool(null);
			values[idx++] = dbn;

			int? ixn = null;
			Console.WriteLine("setting a int as null");
			DynamicValue2<int?> din = new DynamicValue2<int?>(ixn);
			values[idx++] = din;




			list(values);
		}

		private void list(ADynamicValue2[] values)
		{
			int i = 0;

			foreach (ADynamicValue2 value in values)
			{
				if (!(value?.Assigned ?? false)) continue;
				Console.WriteLine("idx| " + i++ + " value| " + (value.Get?.ToString() ?? "is null") 
					+ "  type| " + (value.Get?.GetType() ?? "null type"));
			}
		}

	#endregion

	}
}