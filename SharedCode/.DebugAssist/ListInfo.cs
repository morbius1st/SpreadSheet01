#region + Using Directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Microsoft.Office.Interop.Excel;
using RevitSupport.RevitChartManagement;
using SharedCode.RevitSupport.RevitManagement;
using SharedCode.RevitSupport.RevitParamManagement;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitParamManagement;
using SpreadSheet01.RevitSupport.RevitParamValue;
using UtilityLibrary;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamType;
using Parameter = Autodesk.Revit.DB.Parameter;
using static SpreadSheet01.RevitSupport.RevitParamManagement.RevitParamManager;


#if  NOREVIT
using CellsTest.Windows;
#endif


#endregion

// user name: jeffs
// created:   3/28/2021 1:38:12 PM

namespace SharedCode.DebugAssist
{
	public class ListInfo<T> where T : ISendMessages
	{
		private T win { get; set; }

		public ListInfo(T win)
		{
			this.win = win;
		}

		public RevitSystemManager RevitSystMgr { get; } = new RevitSystemManager();

	#if NOREVIT

		public void listLablesAndFormulas()
		{
			foreach (KeyValuePair<string, RevitLabel> formulasAllCellLabel in RevitSystMgr.Formulas.AllCellLabels)
			{
				
			}
			win.WriteLine("Get All Charts / List labels and formulas| start");
			bool result;

			// get all of the revit chart families and 
			// process each to get its parameters
			result = paramTestA();

			if (!result)
			{
				win.WriteLine("Charts has errors");
			}

			listLabsAndForms(RevitSystMgr.Charts);

			win.WriteLine("Get All Charts / List labels and formulas| end");
		}


		public void listErrors1()
		{
			win.WriteLine("Get All Charts / List errors| start");
			bool result;

			// get all of the revit chart families and 
			// process each to get its parameters
			result = paramTestA();

			if (!result)
			{
				win.WriteLine("Charts has errors");
			}

			listErrors(RevitSystMgr.Charts);

			win.WriteLine("Get all charts| end");
		}

		public void getParamsTest1()
		{
			win.WriteLine("Get all charts| start");
			bool result;

			// get all of the revit chart families and 
			// process each to get its parameters
			result = RevitSystMgr.CollectAndPreProcessCharts(CellUpdateTypeCode.STANDARD);

			if (!result) return;

			listCharts(RevitSystMgr.Charts);

			win.WriteLine("Get all charts| end");
		}

		public void getParamsTest2()
		{
			win.WriteLine("list all chart families (#1)| start");
			ChartFamily chart ;

			bool result = RevitParamManager.GetChartFamily(RevitParamManager.CHART_FAMILY_NAME, out chart);

			if (!result) return;

			listAllChartFamilies();
		}

		public void getParamsTest3()
		{
			win.WriteLine("Get and list all charts| start");

			paramTestA();

			paramTestB();
		}

		public void getParamsTest4a()
		{
			win.WriteLine("\nGet all charts (report 4a)| start");

			Stopwatch s = new Stopwatch();

			s.Start();
			paramTestA();
			s.Stop();

			win.WriteLine("Get all charts (report 4)| done");
			win.WriteLine("elapsed tile| " + s.ElapsedMilliseconds + "\n");
		}

		public void getParamsTest4b()
		{
			win.WriteLine("\nList all charts (report 4b)| start");
			Stopwatch s = new Stopwatch();

			s.Start();
			paramTestB();
			s.Stop();

			win.WriteLine("List all charts (report 4b)| done");
			win.WriteLine("elapsed tile| " + s.ElapsedMilliseconds + "\n");
		}


		private bool paramTestA()
		{
			bool result;

			// get all of the revit chart families and 
			// process each to get its parameters
			result = RevitSystMgr.CollectAndPreProcessCharts(CellUpdateTypeCode.STANDARD);

			if (!result)
			{
				win.WriteLine("collect charts failed");
				return false;
			}

			// RevitSystMgr.PreProcessCharts(CellUpdateTypeCode.STANDARD);

			if (RevitSystMgr.Charts.HasErrors)
			{
				win.WriteLine("process charts| Charts has errors");
				return false;
			}

			return result;
		}

		private void paramTestB()
		{
			listAllChartsInfo(RevitSystMgr.Charts);
		}

	#region sample data

		// public void listSample(AnnotationSymbol[] chartSymbols, AnnotationSymbol[] symbols)
		// public void listSample(ICollection<Element>chartSymbols, ICollection<Element>[] symbols)
		public void listSample(ICollection<Element>chartSymbols, Dictionary<string, ICollection<Element>> symbols)
		{
			
			MainWindow.myTrace.TraceInformation("listSample| Start");
			win.showTabId = false;
			win.TabClr("");
			win.WriteLineTab("listing #1");
			win.WriteLineTab("list chart symbols");


			win.TabUp("list symbols start");

			{
				listSymbols(chartSymbols);
			}

			win.TabDn("list symbols end");

			win.WriteLine("");

			win.WriteLineTab("list symbols");


			win.TabUp("list cell symbols start");

			{
				foreach (KeyValuePair<string, ICollection<Element>> kvp in symbols)
				{
					if (kvp.Value == null || kvp.Value.Count == 0) continue;

					listSymbols(kvp.Value);
				}

				// foreach (ICollection<Element> symCollection in symbols)
				// {
				// 	if (symCollection == null || symCollection.Count == 0) continue;
				//
				// 	listSymbols(symCollection);
				// }
			}

			win.TabDn("list symbols end");

			win.ShowMessage();

			MainWindow.myTrace.TraceInformation("listSample| End");
			// MainWindow.myTrace.Flush();

		}

	#endregion

		// private void listSymbols(AnnotationSymbol[] Symbols)
		private void listSymbols(ICollection<Element> Symbols)
		{
			foreach (AnnotationSymbol aSym in Symbols)
			{
				if (aSym == null) continue;

				win.WriteLine("");
				win.WriteLine("");

				win.WriteLineTab("list symbol|");
				win.TabUp("list symbol start");
				{
					win.WriteLineTab("symbol|            name| >" + aSym.Name + "<");
					win.WriteLineTab("family symbol|     name| >" + aSym.Symbol.Name + "<");
					win.WriteLineTab("family symbol| fam name| >" + aSym.Symbol.FamilyName + "<");

					win.WriteLine("");
					win.WriteLineTab("List all params");

					win.WriteLine("");
					win.TabUp("list params start");
					{
						win.WriteLineTab("list of instance params");
						listParams(aSym.GetOrderedParameters());

						win.WriteLine("");
						win.WriteLineTab("list of type params");
						listParams(aSym.Symbol.GetOrderedParameters());
					}
					win.TabDn("list all params end");
				}
				win.TabDn("list symbol end");
			}
		}

		private void listParams(IList<Parameter> paramsList)
		{
			win.WriteLine("");
			win.TabUp("list params start");

			win.WriteLineTab("|param| def| name|".PadRight(25) + "|type".PadRight(18) + "|value as string" );
			win.WriteLineTab("|----------------------".PadRight(25) + "|--------------".PadRight(18) + "|--------------------" );

			foreach (Parameter param in paramsList)
			{
				if (!param.UserModifiable)
				{
					win.WriteLineTab("| >  internal param <" );
				}


				win.WriteTab("| >" + (param.Definition.Name + "<").PadRight(22) );
				win.Write("| " + param.Definition.Type.ToString().PadRight(15));

				string value = param.AsString();

				win.WriteLine(" | " + (value.IsVoid() ? "* is empty *" : value));
			}

			win.TabDn("list params start");
		}

	#region listAllChartFamilies

		public void listAllChartFamilies()
		{
			win.showTabId = false;

			win.TabClr("");
			win.WriteLineTab("listing #3");
			win.WriteLineTab("list chart families");

			win.TabUp("list all charts start");
			{
				foreach (KeyValuePair<string, ChartFamily> kvp in RevitParamManager.ChartFamilies.FamilyTypes)
				{
					win.Write("\n");
					win.WriteLineTab("list chart| key| " + kvp.Key);

					listAChart(kvp.Value as ChartFamily);
				}
			}
			win.TabDn("list all charts end");

			win.ShowMessage();
		}

	#endregion

		public void listAChart(ChartFamily chartFam)
		{
			win.Write("\n");
			win.WriteLineTab("list a chart| familyName| " + chartFam.FamilyName);

			win.TabUp("list chart start");

			{
				win.WriteLine("");
				listOneFamily(chartFam, "chart");
				listMustExist(chartFam, "chart");
				listParams(chartFam, "chart");
				listCells(chartFam);
			}
			win.TabDn("list chart end");
		}

		private void listCells(ChartFamily chart)
		{
			win.Write("\n");
			win.WriteLineTab("list cell| ");

			win.TabUp("list cell start");

			// foreach (KeyValuePair<string, CellFamily> kvp in chart.CellFamilies)
			// {
			// Family cell = kvp.Value;
			Family cell = chart.CellFamily;

			// win.Write("\n");
			// win.WriteLineTab("list a cell| key| " + kvp.Key);

			// win.TabUp("list cell start");
			// {
			listOneFamily(cell, "cell");
			listMustExist(cell, "cell");
			listParams(cell, "cell");
			// }
			// win.TabDn("list cell end");
			// }
			win.TabDn("list cell end");
		}

		private void listOneFamily(Family chart, string who)
		{
			win.WriteLineTab(who + "| num of lists| " + chart.NumberOfLists);
			win.WriteLineTab(who + "| family nanme| " + chart.FamilyName);
			win.WriteLineTab(who + "|     category| " + chart.Category);
			win.WriteLineTab(who + "|      sub-cat| " + chart.SubCategory);
		}

		private void listMustExist(Family chart, string who)
		{
			win.WriteLine("");
			win.WriteLineTab(who + "| must exist list| " );

			win.TabUp("list must exist start");
			{
				for (var i = 0; i < chart.ParamMustExistCount.Length; i++)
				{
					win.WriteLineTab("family| list item| " + chart.ParamMustExistCount[i]);
				}
			}
			win.TabDn("list must exist end");
		}

		public void listParams(Family fam, string who)
		{
			win.WriteLine("");
			win.WriteLineTab(who + "| list parameter descriptions");

			int i = 0;
			int j = 0;

			win.TabUp("start list chart params");
			{
				foreach (ParamDesc[] paramDescs in fam.ParameterDescriptions)
				{
					ParamType pt = (ParamType) i++;

					win.WriteLine("");
					win.WriteLineTab("list parameters of type| " + pt.ToString() + " start");

					win.TabUp("params start");
					{
						win.WriteLine("");
						listParamHeader();
						foreach (ParamDesc pd in paramDescs)
						{
							listOneParamDesc(pd);
						}

						win.WriteLine("");
					}
					win.TabDn("params end");
				}

				win.WriteLine("");

				win.TabDn("end");

				i++;
			}
		}

		private int[] phw1 = new [] {23, 12, 12, 18, 12, 16, 22, 24 };

		private void listParamHeader()
		{
			// // line 1
			// win.WriteTab("|".PadRight(32));
			// win.Write(" | parent".PadRight(16));
			// // win.Write(" | cell".PadRight(15));
			// win.Write("\n");

			int i = 0;


			// line 2
			win.WriteTab("|idx");
			win.Write(" | name"       .PadRight(phw1[i++]+2, '-'));
			win.Write(" | short-name" .PadRight(phw1[i++]+2, '-'));
			// win.Write(" | dt type"    .PadRight(phw1[i++]+2, '-'));
			win.Write(" | rt type"    .PadRight(phw1[i++]+2, '-'));
			win.Write(" | sub type"   .PadRight(phw1[i++]+2, '-'));
			win.Write(" | pm class"   .PadRight(phw1[i++]+2, '-'));
			win.Write(" | pm type"    .PadRight(phw1[i++]+2, '-'));
			win.Write(" | rd req"     .PadRight(phw1[i++]+2, '-'));
			win.Write(" | mode"       .PadRight(phw1[i++]+2, '-'));
			win.Write(" | exist -----------------");
			win.Write("\n");

			// line 3
			// win.WriteLineTab("| "+"-".Repeat(160));
		}


		private void listOneParamDesc(ParamDesc pd)
		{
			int i = 0;

			win.WriteTab("| " + $"{pd.Index, 2:#0} ");
			win.Write   ("| " + pd.ParameterName  .PadRight(phw1[i++]));
			win.Write   ("| " + pd.ShortName      .PadRight(phw1[i++]));
			// win.Write   ("| " + pd.DataType       .ToString().PadRight(phw1[i++]));
			win.Write   ("| " + pd.RootType       .ToString().PadRight(phw1[i++]));
			win.Write   ("| " + pd.SubType        .ToString().PadRight(phw1[i++]));
			win.Write   ("| " + pd.ParamClass     .ToString().PadRight(phw1[i++]));
			win.Write   ("| " + pd.ParamType      .ToString().PadRight(phw1[i++]));
			win.Write   ("| " + pd.ReadReqmt      .ToString().PadRight(phw1[i++]));
			win.Write   ("| " + pd.Mode           .ToString().PadRight(phw1[i++]));
			win.Write   ("| " + pd.Exist          .ToString());
			win.Write("\n");
		}

	#endif

	#region list labels and formulas

		public void listLabsAndForms(RevitCharts charts)
		{
			win.showTabId = false;

			win.TabClr("");
			win.WriteLineTab("listing #7");
			win.WriteLineTab("list labels and formulas");

			win.TabUp("start");
			{
				listLabsAndFormsSummary(charts);
			}
			win.TabDn("end");

			win.ShowMessage();
		}

	#endregion

		private void listLabsAndFormsSummary(RevitCharts charts)
		{
			win.WriteLine("");
			win.WriteTab("summary for| charts| " + charts.Name);
			win.Write("  # charts| " + charts.ListOfCharts.Count.ToString("#0"));
			win.Write("\n");

			foreach (KeyValuePair<string, RevitChart> kvp1 in charts.ListOfCharts)
			{
				RevitChart chart = kvp1.Value;

				listLnfChart(chart);
			}

			win.WriteLine("");
			win.WriteLine("");
			win.WriteTab("summary for| all labels| " + charts.Name);
			win.WriteLine("");

			win.TabUp("labels summary 2");
			{
				listLabelHeader();
				listLabels(charts);
			}
			win.TabDn("labels summary2 ");
		}

		private void listLnfChart(RevitChart chart)
		{
			win.WriteLine("");
			win.WriteTab("summary for| chart| " + chart.Name);
			win.Write("  # cells| " + chart.ListOfCellSyms.Count.ToString("#0"));
			win.Write("  # labels| " + chart.AllCellLabels.Count);
			win.WriteLine("  # errors| " + chart.ErrorCodeList.Count);

			if (!listErrors(chart.ErrorCodeList))
			{
				win.WriteLineTab("chart errors| NONE");
			}

			if (!listErrors(chart.RevitChartData.ErrorCodeList))
			{
				win.WriteLineTab("chart data errors| NONE");
			}

			listAllParamErrors(chart.RevitChartData.RevitParamLists);

			foreach (KeyValuePair<string, RevitCellData> kvp in chart.ListOfCellSyms)
			{
				RevitCellData rcd = kvp.Value;
				if (!listErrors(rcd.ErrorCodeList))
				{
					win.WriteLineTab("cell data errors| NONE");
				}

				listAllParamErrors(rcd.RevitParamLists);
			}

			win.WriteLine("");
			win.TabUp("chart excel info");
			{
				listChartExcelInfo(chart);
			}
			win.TabDn("chart excel info");

			// win.WriteLine("");
			win.TabUp("labels summary");
			{
				listLnf(chart.AllCellLabels);
			}
			win.TabDn("labels summary");

		}

		private void listChartExcelInfo(RevitChart chart)
		{
			// win.WriteLine("");
			win.WriteLineTab("chart excel info| ");

			win.TabUp("excel info");
			{
				win.WriteLineTab("excel info| file path|" + chart.FilePath);
				win.WriteTab("excel info| worksheet| " +
					(chart.RevitChartData[PT_INSTANCE][RevitParamManager.ChartWorkSheetIdx]?.DynValue.AsString() ?? "is null").PadRight(12));
				win.WriteLine("exists| " + chart.Exists.ToString().PadRight(8));
			}
			win.TabDn("chart ");
		}

		private void listLabels(RevitCharts charts)
		{
			int idx = 0;
			// foreach (KeyValuePair<string, RevitLabel> kvp in charts.FormulaManager.AllCellLabels)

			foreach (KeyValuePair<string, RevitLabel> kvp in RevitSystMgr.Formulas.AllCellLabels)
			{
				RevitLabel label = kvp.Value;

				listLabel(label, idx++);
			}
		}

		private void listLnf(Dictionary<string, RevitLabel> labels)
		{
			if (labels.Count == 0) return;

			win.WriteLineTab("labels list| ");

			win.TabUp("list labels");
			{
				int i = 0;

				foreach (KeyValuePair<string, RevitLabel> kvp in labels)
				{
					RevitLabel label = kvp.Value;

					listLabel(label, i++);
					

					// // win.WriteTab("idx| " + i++.ToString("{3:##0}"));
					// win.WriteTab("idx| " + $"{i++,2:#0}");
					// win.Write(" key| >" +  (kvp.Key + "<").PadRight(28));
					// win.Write(" chart seq| >" +  (label.ParentChart.Sequence + "<").PadRight(12));
					// win.Write(" cell data seq| >" +  (label.RevitCellData.Sequence + "<").PadRight(12));
					// win.Write(" cellName| " + label.RevitCellData.Name.PadRight(20));
					// win.Write(" label| " +  label.Label.PadRight(10));
					// win.Write(" id| " +  $"{label.LabelId,2:#0}");
					// win.Write(" errors| " +  label.HasErrors.ToString().PadRight(7));
					// win.Write(" formula| " +  label.Formula);
					// win.Write("\n");
				}
			}
			win.WriteLine("");

			win.TabDn("list labels");
		}

		// private int[] phw2 = new [10] {26, 16, 21, 12, 5};
		private int[] phw2 = new int[20];

		private void listLabelHeader()
		{
			int i = 0;

			win.WriteTab("|".PadRight(32));
			win.Write(" | parent".PadRight(16));
			// win.Write(" | cell".PadRight(15));
			win.Write("\n");

			win.WriteTab("|idx");

			phw2[i] = 26;
			win.Write("| key ".PadRight(phw2[i++] + 2,'-'));

			phw2[i] = 16;
			win.Write(" | chart name ".PadRight(phw2[i++] + 2, '-'));

			phw2[i] = 21;
			win.Write(" | cell data name ".PadRight(phw2[i++] + 2,'-'));

			phw2[i] = 14;
			win.Write(" | label Name".PadRight(phw2[i++] + 2,'-'));

			phw2[i] = 5;
			win.Write(" | id ".PadRight(phw2[i++] + 2, '-'));

			phw2[i] = 20;
			win.Write(" | formula status".PadRight(phw2[i++] + 2,'-'));

			win.Write(" | formula ---------------------------");
			win.Write("\n");

			win.WriteLineTab("|"+"-".Repeat(134));
		}

		private void listLabel(RevitLabel label, int j)
		{
			int i = 0;

			string name;

			win.WriteTab("| " + $"{j,2:#0}");
			win.Write("| >" +  (label.InternalKey + "<").PadRight(phw2[i++]));
			// win.Write(" chart seq| >" +  (label.ParentChart.Sequence + "<").PadRight(12));
			// win.Write(" cell data seq| >" +  (label.RevitCellData.Sequence + "<").PadRight(12));
			// win.Write(" cellName| " + label.RevitCellData.Name.PadRight(20));

			name = (label.ParentChart.HasErrors ? "*" : "") + label.ParentChart.Name;

			win.Write("| " +  name.PadRight(phw2[i++]));

			name = (label.RevitCellData.HasErrors ? "*" : "") + label.RevitCellData.Name;

			win.Write("| " +  name.PadRight(phw2[i++]));

			name = (label.HasErrors ? "*" : "") + label.Label;

			win.Write("| " +  $"{label.LabelId,3:#0}  ");

			win.Write("| " +  name.PadRight(phw2[i++]));

			win.Write("| " +  label.FormulaStatus.ToString().PadRight(phw2[i++]));

			// win.Write("| " +  label.HasErrors.ToString().PadRight(7));
			win.Write("| " +  label.Formula);
			win.Write("\n");
		}


		private void listAllParamErrors(ARevitParam[][] paramLists)
		{
			if (!hasErrors(paramLists)) return;
			 
			win.TabUp("errors lists| list all param errors");

			win.WriteLineTab("List all param errors| ");
			{
				int i = 0;

				foreach (ARevitParam[] paramList in paramLists)
				{
					if (paramList == null || !hasErrors(paramList)) continue;

					win.WriteLineTab("errors for| " + ((ParamType) i++).ToString().PadRight(12));

					win.TabUp("list param errors");
					{
						int j = 0;
						foreach (ARevitParam p in paramList)
						{
							if (p != null) listErrors(p.ErrorCodeList);
						}
					}
					win.TabDn("list param errors");
				}
			}
			win.TabDn("list all param errors");
		}

		private bool hasErrors(ARevitParam[][] paramLists)
		{
			foreach (ARevitParam[] paramList in paramLists)
			{
				if (paramList == null) continue;

				if (hasErrors(paramList)) return true;
			}
			return false;
		}
		
		private bool hasErrors(ARevitParam[] paramList)
		{
			foreach (ARevitParam p in paramList)
			{
				if (p != null)
				{
					if (p.ErrorCodeList.Count > 0 ) return true;
				}
			}

			return false;
		}
	#region list errors

		public void listErrors(RevitCharts Charts)
		{
			// List<ErrorCodeListMember> codes = ErrorCodeList2.ErrCodeList.Errors;


			win.showTabId = false;

			win.TabClr("");
			win.WriteLineTab("listing #1");
			win.WriteLineTab("list errors");

			win.TabUp("start");
			{
				listChartsSummary(Charts);
			}
			win.TabDn("end");

			win.ShowMessage();
		}

	#endregion

		private void listChartsSummary(RevitCharts charts)
		{
			win.WriteLine("");
			win.WriteTab("summary for| charts| " + charts.Name);
			win.Write("  # charts| " + charts.ListOfCharts.Count.ToString("#0"));
			win.WriteLine("  has errors| " + charts.HasErrors + " qty| " + charts.ErrorCodeList.Count);

			win.TabUp("charts error summary");
			{
				listErrors(charts.ErrorCodeList);
			}
			win.TabDn("charts error summary");

			win.TabUp("list charts summary");
			{
				foreach (KeyValuePair<string, RevitChart> kvp1 in charts.ListOfCharts)
				{
					RevitChart chart = kvp1.Value;

					listChartErrors(chart);

					win.TabUp("chart error summary");
					{
						foreach (KeyValuePair<string, RevitCellData> kvp2 in chart.ListOfCellSyms)
						{
							RevitCellData rcd = kvp2.Value;

							listRevitCellDataErrors(rcd);

							win.TabUp("label error summary");
							{
								foreach (KeyValuePair<string, RevitLabel> kvp3 in rcd.ListOfLabels)
								{
									listLabelErrors(kvp3.Value);
								}
							}
							win.TabDn("label error summary");
						}
					}
					win.TabDn("chart error summary");
				}
			}
			win.TabDn("list chart summary");
		}

		private void listChartErrors(RevitChart chart)
		{
			win.WriteLine("");
			win.WriteTab("summary for| chart| " + chart.Name);
			win.Write("  # cells| " + chart.ListOfCellSyms.Count.ToString("#0"));
			win.WriteLine("  has errors| " + chart.HasErrors + " qty| " + chart.ErrorCodeList.Count);

			win.TabUp("chart error summary");
			{
				listErrors(chart.ErrorCodeList);
			}
			win.TabDn("chart error summary");
		}

		private void listRevitCellDataErrors(RevitCellData rcd)
		{
			win.WriteLine("");
			win.WriteTab("summary for| revitcelldata| " + rcd.Name);
			win.Write("  # labels| " + rcd.ListOfLabels.Count.ToString("#0"));
			win.WriteLine("  has errors| " + rcd.HasErrors + " qty| " + rcd.ErrorCodeList.Count);

			win.TabUp("rcd error summary");
			{
				listErrors(rcd.ErrorCodeList);
			}
			win.TabDn("rcd error summary");
		}

		private void listLabelErrors(RevitLabel label)
		{
			// win.WriteLine("");
			win.WriteTab("summary for| label| " + label.Label);
			win.WriteLine("  has errors| " + label.HasErrors + " qty| " + label.ErrorCodeList.Count);

			win.TabUp("label error summary");
			{
				if (!listErrors(label.ErrorCodeList))
				{
					win.WriteLineTab("label errors| NONE");
				}
			}
			win.TabDn("label error summary");
		}

		private bool listErrors(List<ErrorCodes> errorList)
		{
			if (errorList.Count == 0) return false;

			win.WriteLineTab("errors list| ");

			win.TabUp("list errors");
			{
				int i = 0;

				foreach (ErrorCodes ec in errorList)
				{
					listError(i, ec);
				}
			}
			// win.WriteLine("");

			win.TabDn("list errors");

			return true;
		}


		private void listError(int i, ErrorCodes ec)
		{
			win.WriteLineTab("idx| " + i++.ToString("#0")	+ " error| " + ec.ToString() );
		}

		// private void listChartErrors(RevitChart chart)
		// {
		// 	win.WriteLine("");
		// 	win.WriteLineTab("for chart| " + chart.Name);
		//
		// 	win.WriteLine("");
		// 	win.TabUp("list one chart errors");
		// 	{
		// 		win.WriteLineTab("chart| " + chart.Name + 
		// 			" got errors?| " + chart.HasErrors
		// 			+ " qty| " + chart.ErrorCodeList.Count);
		//
		//
		// 	}	
		// 	win.TabUp("list one chart errors");
		// }


	#region list charts

		public void listCharts(RevitCharts Charts)
		{
			win.showTabId = false;

			win.TabClr("");
			win.WriteLineTab("listing #4");
			win.WriteLineTab("list charts (list of {RevitChart}");

			win.TabUp("start");
			{
				foreach (KeyValuePair<string, RevitChart> kvp1 in Charts.ListOfCharts)
				{
					win.WriteLine("");
					win.WriteLineTab("for chart| " + kvp1.Key);

					win.TabUp("");
					{
						win.WriteLine("");
						listChart(kvp1.Value);
					}
					win.TabDn("");
				}
			}
			win.TabDn("end");

			win.ShowMessage();
		}

	#endregion

		private void listChart(RevitChart chart)
		{
			


			win.WriteLineTab("Chart|          name| " + chart[NameIdx]?.GetValue() ?? "is null");
			win.WriteLineTab("Chart| cell fam name| " + chart[ChartCellFamilyNameIdx]?.GetValue() ?? "is null");
			win.WriteLineTab("Chart|      sequence| " + chart[SeqIdx]?.GetValue() ?? "is null");
			win.WriteLineTab("Chart|      filepath| " + ((RevitParamFilePath) chart[ChartFilePathIdx])?.FilePath ?? "is null");
			win.WriteLineTab("Chart|        exists| " + ((RevitParamFilePath) chart[ChartFilePathIdx])?.Exists ?? "is null");
			win.WriteLineTab("Chart|     worksheet| " + chart[ChartWorkSheetIdx]?.GetValue() ?? "is null");
			win.WriteLineTab("Chart|    updateType| " + chart[ChartUpdateTypeIdx]?.GetValue() ?? "is null");
			win.WriteLineTab("Chart|     hasErrors| " + chart.HasErrors + " (" + chart.ErrorCodeList.Count + ")");

			win.WriteLine("");
			win.WriteLineTab("Chart params");

			win.TabUp("chart params start");
			{
				win.WriteLine("");
				win.WriteLineTab("basic params");

				win.TabUp("basic params start");
				listRevitChartParams2(chart.RevitChartData[PT_INSTANCE]);
				win.TabDn("basic params end");

				win.WriteLine("");
				win.WriteLineTab("type params");

				win.TabUp("type params start");
				listRevitChartParams2(chart.RevitChartData[PT_TYPE]);
				win.TabDn("type params end");

				win.WriteLine("");
				win.WriteLineTab("internal params");

				win.TabUp("internal params start");
				listRevitChartParams2(chart.RevitChartData[PT_INTERNAL]);
				win.TabDn("internal params end");
			}
			win.TabDn("chart params| end");


			win.WriteLine("");
			win.WriteLineTab("Chart errors");

			win.TabUp("chart errors start");
			{
				listRevitChartErrors(chart);
			}
			win.TabDn("chart errors| end");


			win.WriteLine("");
			win.WriteLineTab("RevitChartData");

			win.TabUp("revit chart data| start");
			{
				RevitChartData rcd = chart.RevitChartData;

				listRevitChartDataBasic(rcd);
			}
			win.TabDn("revit chart data| end");

			win.Write("\n");
			win.WriteLineTab("param| must exist list");

			win.TabUp("param must exist list| start");
			{
				for (var i = 0; i < chart.RevitChartData.ChartFamily.ParamMustExistCount.Length; i++)
				{
					win.WriteTab("PT | " + ((ParamType) i).ToString().PadRight(18));
					win.WriteLine("qty| " + chart.RevitChartData.ChartFamily.ParamMustExistCount[i].ToString("D3"));
				}
			}

			win.TabDn("param must exist list| end");

			win.Write("\n");
			win.WriteLineTab("param| does exist list");

			win.TabUp("params do exist list| start");
			{
				for (var i = 0; i < chart.RevitChartData.ReqdParamCount.Length; i++)
				{
					win.WriteTab("PT | " + ((ParamType) i).ToString().PadRight(18));
					win.WriteLine("qty| " + chart.RevitChartData.ReqdParamCount[i].ToString("D3"));
				}
			}
			win.TabDn("params do exist list| end");

			win.WriteLine("");
		}

		private void listRevitChartDataBasic(RevitChartData rcd)
		{
			win.WriteLineTab("R_ChartData|          sequence| " + rcd[SeqIdx]?.GetValue() ?? "is null");
			win.WriteLineTab("R_ChartData|              name| " + rcd[NameIdx]?.GetValue() ?? "is null");
			win.WriteLineTab("R_ChartData|          fam name| " + rcd.FamilyName);
			win.WriteLineTab("R_ChartData|     cell fam name| " + rcd[ChartCellFamilyNameIdx]?.GetValue() ?? "is null");
			win.WriteLineTab("R_ChartData| anno sym fam name| " + rcd.AnnoSymbol.Symbol.FamilyName);
			win.WriteLineTab("R_ChartData|          dynvalue| " + rcd.DynValue.ToString());
			win.WriteLineTab("R_ChartData|        updatetype| " + rcd[ChartUpdateTypeIdx]?.GetValue() ?? "is null");
		}

		private void listRevitChartParams2(ARevitParam[] paramList)
		{
			if (paramList == null || paramList.Length == 0)
			{
				win.WriteLineTab("list is null or empty");
				return;
			}

			win.WriteLineTab("|param| def| name|".PadRight(24) + "|errors?".PadRight(12) + "|value as string" );
			win.WriteLineTab("|----------------------".PadRight(24) + "|-----".PadRight(12) + "|--------------------------------------------" );

			foreach (ARevitParam p2 in paramList)
			{
				if (p2 == null)
				{
					win.WriteLineTab("|p2| is null");
					continue;
				}

				win.WriteTab("| " + p2.ParamDesc.ParameterName.PadRight(22));

				string value = p2.GetValue().ToString();

				if (value.IsVoid())
				{
					win.WriteLine("| value is void");
				}
				else
				{
					win.Write("| " + p2.HasErrors.ToString().PadRight(10));
					win.WriteLine("| " + p2.GetValue() );
				}
			}
		}

		private void listRevitChartParams(ARevitParam[] paramList)
		{
			if (paramList == null || paramList.Length == 0)
			{
				win.WriteLineTab("list is null or empty");
				return;
			}

			foreach (ARevitParam p2 in paramList)
			{
				if (p2 == null)
				{
					win.WriteLineTab("p2| is null");
				}
				else
				{
					win.WriteLineTab("p2| " + p2.ParamDesc.ParameterName.PadRight(25)
						+ "  value| " + p2.GetValue() );
				}
			}
		}

		private void listRevitChartErrors(RevitChart chart)
		{
			if (chart.ErrorCodeList.Count > 0)
			{
				foreach (ErrorCodes ec in chart.ErrorCodeList)
				{
					win.WriteLineTab("error| " + ec);
				}
			}
			else
			{
				win.WriteLineTab("errors| NONE");
			}
		}


		private void listAllChartCellsSyms(RevitChart chart)
		{
			win.TabUp("list cell syms");
			{
				win.WriteLineTab("list all cell syms ");

				win.TabUp("list cell syms");
				{
					if (chart.ListOfCellSyms.Count > 0)
					{
						win.WriteLineTab("|key".PadRight(30) + "|seq".PadRight(6) + "|name" );
						win.WriteLineTab("|-------------------------".PadRight(30) + "|---- " + "|---------------" );

						foreach (KeyValuePair<string, RevitCellData> kvp in chart.ListOfCellSyms)
						{
							win.WriteTab("| " + kvp.Key.PadRight(28));
							win.Write("| " + kvp.Value.Sequence.PadRight(4));
							win.WriteLine("| " + kvp.Value.Name);
						}
					}
					else
					{
						win.WriteLineTab("cell sym list is empty ");
					}
				}
				win.TabDn("list cell syms");
			}
			win.TabDn("list cell syms");
		}


		private void listAllChartLabels(RevitChart chart)
		{
			win.TabUp("list labels");
			{
				win.WriteLine("\n");
				win.WriteLineTab("list all labels ");

				win.TabUp("list labels");
				{
					if (chart.AllCellLabels.Count > 0)
					{
						win.WriteLineTab("|key".PadRight(30) + "|name".PadRight(25) + "|formula" );
						win.WriteLineTab("|----------------------".PadRight(30) + "|-------------------".PadRight(25) + "|--------------------" );

						foreach (KeyValuePair<string, RevitLabel> kvp in chart.AllCellLabels)
						{
							win.WriteTab("| " + kvp.Key.PadRight(28));
							win.Write("| " + kvp.Value.Label.PadRight(23));
							win.WriteLine("| " + kvp.Value.Formula);
						}
					}
					else
					{
						win.WriteLineTab("label list is empty ");
					}
				}
				win.TabDn("list labels");
			}
			win.TabDn("list labels");
		}


	#region listAllChartInfo

		public void listAllChartsInfo(RevitCharts Charts)
		{
			win.showTabId = false;

			win.TabClr("0");

			win.WriteLine("");
			win.WriteLineTab("listing #5");
			win.WriteLineTab("list charts (list of {RevitChart and contained info}");

			win.TabUp("5");

			win.WriteLine("");
			win.WriteLineTab("list all charts ");

			win.TabUp("6");
			{
				int j = 0;
				foreach (KeyValuePair<string, RevitChart> kvp1 in Charts.ListOfCharts)
				{
					win.WriteLine("");
					win.WriteLineTab("chart " + j++.ToString("#0") + "| " + getChartName(kvp1.Value).PadRight(20) + " errors?| " + kvp1.Value.HasErrors);
					listAllChartCellsSyms(kvp1.Value);
					listAllChartLabels(kvp1.Value);
				}
			}
			win.TabDn("7");


			// start with a list of charts
			foreach (KeyValuePair<string, RevitChart> kvp1 in Charts.ListOfCharts)

		#region Chart

			{
				RevitChart Chart = kvp1.Value;
				// Chart.ListOfCellSyms["asdf"].

				// process a single chart
				win.Write("\n");
				win.WriteLineTab("list a chart| " + kvp1.Key);


				win.Write("\n");
				win.TabUp("7");
				{
					win.WriteLineTab("list associated cells ");

					win.TabUp("8");
					{
						int j = 0;
						if (kvp1.Value.ListOfCellSyms.Count > 0)
						{
							foreach (KeyValuePair<string, RevitCellData> kvp2 in kvp1.Value.ListOfCellSyms)
							{
								win.WriteLineTab("cell " + j++.ToString("#0") + "| " + kvp2.Value.Name.PadRight(18) + " errors?| " + kvp2.Value.HasErrors);
							}
						}
						else
						{
							win.WriteLineTab("Cell| NONE");
						}
					}
					win.TabDn("9");

					win.Write("\n");

					// win.TabUp("10");
					{
						RevitChart v = kvp1.Value;

						listChart(kvp1.Value);

						// if (kvp1.Value.HasErrors)
						// {
						// 	win.Write("\n");
						// 	win.WriteLineTab("Chart| has errors, next chart");
						// }
						// else
						// {
						win.Write("\n");
						win.WriteLineTab("Chart| label cell shortcuts| (AllCellLabels)");

						win.TabUp("13");
						{
							{
								foreach (KeyValuePair<string, RevitLabel> kvp5 in kvp1.Value.AllCellLabels)
								{
									win.WriteTab("kvp5|   key| " + kvp5.Key.PadRight(18));
									// win.WriteTab("kvp5|   key| " + kvp5.Value.);
									win.WriteLine(" value| formula| " + kvp5.Value.Formula);
								}
							}
							// win.TabDn("17");

							// win.TabUp("20");
							// {

						#region Cells

							win.Write("\n");
							win.WriteLineTab("Cells #C| [RevitCellData : ListOfCellSyms] ");

							win.TabUp("25");
							{
								if (kvp1.Value.ListOfCellSyms.Count == 0)
								{
									win.WriteLineTab("List is empty");
								}

								// list the collection of cell families
								foreach (KeyValuePair<string, RevitCellData> kvp2 in kvp1.Value.ListOfCellSyms)

							#region Cell

								{
									RevitCellData revitCellData = kvp2.Value;

									// list a single family
									win.Write("\n");
									win.WriteLineTab("CellData| " + revitCellData.ToString());

									win.TabUp("30");
									{
										win.WriteLineTab("CellData| key       | " + kvp2.Key);
										win.WriteLineTab("CellData| has errors| " + kvp2.Value.HasErrors);
										win.WriteLineTab("CellData| parameters| value| " + kvp2.Value.GetValue() ?? " is null");

										win.Write("\n");
										win.WriteLineTab("CellData params");

										win.TabUp("33");
										{
										#region Cell Parameters

											win.Write("\n");

											foreach (ARevitParam p2 in kvp2.Value[PT_INSTANCE])
											{
												if (p2 == null)
												{
													win.WriteLineTab("p2| is null");
												}
												else
												{
													win.WriteLineTab("p2| " + p2.ParamDesc.ParameterName.PadRight(15)
														+ "  value| " + p2.GetValue() );
												}
											}

										#endregion
										}
										win.TabDn("33");


										if (kvp2.Value.HasErrors)
										{
											win.Write("\n");
											win.WriteLineTab("CellData errors| qty| " + kvp2.Value.ErrorCodeList.Count);

											win.TabUp("cellData errors");
											{
												for (var i = 0; i < kvp2.Value.ErrorCodeList.Count; i++)
												{
													win.WriteTab("idx | " + i.ToString().PadRight(18));
													win.WriteLine("error| " + kvp2.Value.ErrorCodeList[i]);
												}
											}
											win.TabDn("cellData errors");
										}

										win.Write("\n");
										win.WriteLineTab("Cell| must exist list");

										win.TabUp("Cell| must exist list");
										{
											for (var i = 0; i < kvp2.Value.CellFamily.ParamMustExistCount.Length; i++)
											{
												win.WriteTab("PT | " + ((ParamType) i).ToString().PadRight(18));
												win.WriteLine("qty| " + kvp2.Value.CellFamily.ParamMustExistCount[i].ToString("D3"));
											}
										}
										win.TabDn("Cell| must exist list");

										win.Write("\n");
										win.WriteLineTab("param| does exist list");

										win.TabUp("params do exist list| start");
										{
											for (var i = 0; i < kvp2.Value.NumberOfLists; i++)
											{
												win.WriteTab("PT | " + ((ParamType) i).ToString().PadRight(18));
												win.WriteLine("qty| " + kvp2.Value.ReqdParamCount[i].ToString("D3"));
											}

											win.Write("\n");

											for (var i = kvp2.Value.NumberOfLists; i < kvp2.Value.ReqdParamCount.Length; i++)
											{
												win.WriteTab("PT | " + ((ParamType) i).ToString().PadRight(18));
												win.WriteLine("qty| " + kvp2.Value.ReqdParamCount[i].ToString("D3"));
											}
										}
										win.TabDn("params do exist list| end");


										win.Write("\n");
										win.WriteLineTab("labels| (RevitCell [kvp2])");

									#region Shortcuts to Labels

										win.TabUp("35");
										{
											win.Write("\n");
											win.WriteLineTab("Labels| label cell shortcuts| ([CellLabels])");

											win.TabUp("36");
											{
												foreach (KeyValuePair<string, RevitLabel> kvp4 in kvp2.Value.ListOfLabels)
												{
													win.WriteTab("kvp4|   key| " + kvp4.Key.PadRight(15));
													win.WriteLine(" value| formula| " + kvp4.Value.Formula);
												}
											}
											win.TabDn("37");
										}
										win.TabDn("38");

									#endregion

										win.TabUp("40");
										{
											// win.WriteLineTab("labels value| " + kvp2.Value.LabelList.DynValue.Value?.ToString() ?? "is null");

											foreach (KeyValuePair<string, RevitLabel> kvp3 in kvp2.Value.ListOfLabels)

										#region Label

											{
												win.Write("\n");
												win.WriteLineTab("label| (RevitLabel [kvp3]) " + kvp3.Value.ToString());

												win.TabUp("43");
												{


													win.WriteLineTab("label| has errors| " + kvp3.Value.HasErrors);

													try
													{
														win.WriteLineTab("label| key       | " + kvp3.Key);
														win.WriteLineTab("label| value     | " + kvp3.Value.GetValue() ?? "is null");
														win.WriteLineTab("label| parent cht| " + kvp3.Value.ParentChart?.ToString() ?? "is null");
														win.WriteLineTab("label| excel file| " + kvp3.Value.ParentChart?.FilePath ?? "is null");
														// win.WriteLineTab("label| excel wsht| " + kvp3.Value.ParentChart?.WorkSheet ?? "is null");

														win.WriteLineTab("label| excel wsht| " + kvp3.Value.ParentChart[ChartWorkSheetIdx]?.GetValue() ?? "is null");


														if (kvp3.Value.HasErrors)
														{
															win.Write("\n");
															win.WriteLineTab("label errors| qty| " + kvp3.Value.ErrorCodeList.Count);

															win.TabUp("label errors");
															for (var i = 0; i < kvp3.Value.ErrorCodeList.Count; i++)
															{
																win.WriteTab("idx| " + i.ToString().PadRight(18));
																win.WriteLine(" error code| " + kvp3.Value.ErrorCodeList[i]);
															}

															win.TabDn("label errors");
														}


														win.Write("\n");
														win.WriteLineTab("label| parameters| ([RevitParamList])");

														win.TabUp("45");
														{
															ARevitParam p3;

														#region Label Parameters

															for (var i = 0; i < kvp3.Value[PT_LABEL].Length; i++)
															{
																win.WriteTab("p3| " + i + "| ");

																p3 = kvp3.Value[PT_LABEL][i];

																if (p3 == null)
																{
																	win.WriteLine("is null / not used");
																}
																else
																{
																	win.WriteLine(p3.ParamDesc.ParameterName.PadRight(15)
																		+ "  index| " + p3.ParamDesc.Index.ToString("D3")
																		+ "  value| " + p3.GetValue() );
																}
															}

														#endregion
														}
														win.TabDn("50");

													}
													catch
													{
														win.Write("\n");
														win.WriteLineTab("*** label| has errors| error found - remainder skipped");
														
													}


												}
												win.TabDn("52");
											}

										#endregion

											win.TabDn("53");
										}
									}
									win.TabDn("57");
								}

							#endregion
							}
							win.TabDn("60");

						#endregion
						}
						// }

						win.TabDn("62");
						// }
						// win.TabDn("62");
					}
					// win.TabDn("65");
				}
				win.TabDn("67");
			}

		#endregion

			win.TabClr("70");
			win.TabClr();

			win.ShowMessage();
		}

	#endregion

		private string getChartName(RevitChart chart)
		{
			string name;
			try
			{
				name = chart.Name;
			}
			catch
			{
				name = "* Error *| no name";
			}

			return name;
		}
	}
}