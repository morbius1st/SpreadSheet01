using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.CellsTests;

// using Tests.CellsTests;

namespace Tests
{
	class Program
	{
		static Program p;

		static DynamicTests dt = new DynamicTests();

		private static RevitParamTest rpt = new RevitParamTest();


		// private static ParseTextParameter pt = new ParseTextParameter();

		static void Main(string[] args)
		{
			ConsoleKeyInfo c;

			p = new Program();

			do
			{
				// dt.Process();
				// pt.Process();
				rpt.Process();

				Console.Write("\nEnter r to repeat: ");
				c = Console.ReadKey(false);
				Console.Write("\n\n");
			}
			while (c.KeyChar == 'r');
		}

		public void RunTest()
		{
			dt.Process();
		}
	}

}