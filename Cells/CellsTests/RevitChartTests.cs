#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitParamValue;
using Cells.Windows;
using Cells.CellsTests;
using SpreadSheet01.RevitSupport;

#endregion

// user name: jeffs
// created:   3/2/2021 10:19:34 PM

namespace Cells.CellsTests
{
	public class RevitChartTests
	{
		private SampleAnnoSymbols aSyms;

		public RevitAnnoSyms Charts { get; private set; } = new RevitAnnoSyms();

		public void Process()
		{
			aSyms = new SampleAnnoSymbols();

			getChartSymbols(aSyms);



		}

		private void getChartSymbols(SampleAnnoSymbols aSyms)
		{
			aSyms.Process();

			listSymbols(aSyms.Charts);

			AnnotationSymbol[] a = aSyms.Charts;
		}

		private void listSymbols(AnnotationSymbol[] annoSyms)
		{
			MainWindow.WriteLine("\nList symbols");

			foreach (AnnotationSymbol symbol in annoSyms)
			{
				MainWindow.WriteLine("\nsymbols| " + symbol.Name);
				MainWindow.WriteLine("parameters| count| " + symbol.parameters.Count);

				for (var i = 0; i < symbol.parameters.Count; i++)
				{
					MainWindow.Write("   type| ");
					MainWindow.Write(symbol.parameters[i].Definition.Type.ToString());

					string result = "unknown";

					switch (symbol.parameters[i].Definition.Type)
					{
					case ParamDataType.TEXT:
						{
							result = symbol.parameters[i].AsString();
							break;
						}
					case ParamDataType.BOOL:
						{
							result = (symbol.parameters[i].AsInteger() == 1).ToString();
							break;
						}
					case ParamDataType.IGNORE:
						{
							result = "ignore";
							break;
						}
					}


					MainWindow.Write("   val| >");
					MainWindow.Write(result);

					MainWindow.Write("<   name| ");
					MainWindow.WriteLine(symbol.parameters[i].Definition.Name);
				}

				MainWindow.WriteLine("\nComplete\n");
			}
		}




	}
}
