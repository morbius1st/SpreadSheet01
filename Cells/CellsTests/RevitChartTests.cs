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
using SpreadSheet01.RevitSupport.RevitCellsManagement;

#endregion

// user name: jeffs
// created:   3/2/2021 10:19:34 PM

namespace Cells.CellsTests
{
	public class RevitChartTests
	{
		private SampleAnnoSymbols aSyms;

		// public RevitAnnoSyms Charts { get; private set; } = new RevitAnnoSyms();

		public void Process()
		{
			aSyms = new SampleAnnoSymbols();

			getChartSymbols(aSyms);

			listSymbols(aSyms.Charts);

		}

		private void getChartSymbols(SampleAnnoSymbols aSyms)
		{
			aSyms.Process();

			listSymbols(aSyms.Charts);

			AnnotationSymbol[] a = aSyms.Charts;
		}

		private void listSymbols(AnnotationSymbol[] annoSyms)
		{
			MainWindow.WriteLineTab("\nList symbols");

			foreach (AnnotationSymbol symbol in annoSyms)
			{
				MainWindow.WriteLineTab("\nsymbols| " + symbol.Name);
				MainWindow.WriteLineTab("parameters| count| " + symbol.parameters.Count);

				for (var i = 0; i < symbol.parameters.Count; i++)
				{
					MainWindow.WriteTab("   type| ");
					MainWindow.WriteTab(symbol.parameters[i].Definition.Type.ToString());

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


					MainWindow.WriteTab("   val| >");
					MainWindow.WriteTab(result);

					MainWindow.WriteTab("<   name| ");
					MainWindow.WriteLineTab(symbol.parameters[i].Definition.Name);
				}

				MainWindow.WriteLineTab("\nComplete\n");
			}
		}




	}
}
