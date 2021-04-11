#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitParamManagement;
using SpreadSheet01.RevitSupport.RevitParamValue;
using UtilityLibrary;

using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamType;

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

	#if NOREVIT



	#region sample data


		public void listSample(AnnotationSymbol[] chartSymbols, AnnotationSymbol[] symbols)
		{
			win.showTabId = false;
			win.TabClr("");

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
				listSymbols(symbols);
			}

			win.TabDn("list symbols end");

		}


		private void listSymbols(AnnotationSymbol[] Symbols)
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

			win.WriteLineTab("|param| def| name|".PadRight(25) + "|type".PadRight(18)+ "|value as string" );
			win.WriteLineTab("|----------------------".PadRight(25) + "|--------------".PadRight(18)+ "|--------------------" );

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
		
	#endregion


	#region Families (parameter descriptions)

		public void listAllChartFamilies()
		{
			win.showTabId = false;

			win.TabClr("");

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
		}

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

		private void listOneParamDesc(ParamDesc pd)
		{
			
			// win.WriteTab("  param desc| seq| " + seq.ToString("##0"));
			win.WriteTab("idx| " + pd.Index.ToString("##0"));
			win.Write(" name| " + pd.ParameterName.PadRight(22));
			win.Write(" sht-name| " + pd.ShortName.PadRight(10));
			win.Write(" dt type| " + pd.DataType.ToString().PadRight(15));
			win.Write(" pm class| " + pd.ParamClass.ToString().PadRight(10));
			win.Write(" pm type| " + pd.ParamType.ToString().PadRight(15));
			win.Write(" rd req| " + pd.ReadReqmt.ToString().PadRight(20));
			win.Write(" mode| " + pd.Mode.ToString().PadRight(22));
			win.WriteLine(" exist| " + pd.Exist);

		}
		
	#endregion

	#endif

	#region list charts

		public void listCharts(RevitCharts Charts)
		{
			win.showTabId = false;

			win.TabClr("");

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
		}

		private void listChart(RevitChart chart)
		{

			win.WriteLineTab("Chart|          name| " + chart.Name);
			win.WriteLineTab("Chart| cell fam name| " + chart.CellFamilyName);
			win.WriteLineTab("Chart|      sequence| " + chart.Sequence);
			win.WriteLineTab("Chart|      filepath| " + chart.FilePath);
			win.WriteLineTab("Chart|        exists| " + chart.Exists);
			win.WriteLineTab("Chart|     worksheet| " + chart.WorkSheet);
			win.WriteLineTab("Chart|    updateType| " + chart.UpdateType);
			win.WriteLineTab("Chart|     hasErrors| " + chart.HasErrors + " (" + chart.ErrorCodeList.Count + ")");
			win.WriteLineTab("Chart|   cell Errors| " + chart.CellHasError);

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
			win.WriteLineTab("R_ChartData|              name| " + rcd.Name);
			win.WriteLineTab("R_ChartData|          fam name| " + rcd.FamilyName);
			win.WriteLineTab("R_ChartData|     cell fam name| " + rcd.CellFamilyName);
			win.WriteLineTab("R_ChartData| anno sym fam name| " + rcd.AnnoSymbol.Symbol.FamilyName);
			win.WriteLineTab("R_ChartData|          dynvalue| " + rcd.DynValue.ToString());
			win.WriteLineTab("R_ChartData|          sequence| " + rcd.Sequence);
			win.WriteLineTab("R_ChartData|        updatetype| " + rcd.UpdateType);
		}

		private void listRevitChartParams2(ARevitParam[] paramList)
		{
			if (paramList == null || paramList.Length == 0)
			{
				win.WriteLineTab("list is null or empty");
				return;
			}

			win.WriteLineTab("|param| def| name|".PadRight(24) + "|errors?".PadRight(12)+ "|value as string" );
			win.WriteLineTab("|----------------------".PadRight(24) + "|-----".PadRight(12)+ "|--------------------------------------------" );

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
							win.Write("| " + kvp.Value.Name.PadRight(23));
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

	#endregion


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
										}win.TabDn("33");


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
													win.WriteLineTab("label| key       | " + kvp3.Key);
													win.WriteLineTab("label| value     | " + kvp3.Value.GetValue() ?? "is null");
													win.WriteLineTab("label| parent cht| " + kvp3.Value.ParentChart?.ToString() ?? "is null");
													win.WriteLineTab("label| excel file| " + kvp3.Value.ParentChart?.FilePath ?? "is null");
													win.WriteLineTab("label| excel wsht| " + kvp3.Value.ParentChart?.WorkSheet ?? "is null");
													win.WriteLineTab("label| has errors| " + kvp3.Value.HasErrors);


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
		}

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





		// private void listStatusData(ParamStatusData[] psd)
		// {
		// 	foreach (ParamStatusData status in psd)
		// 	{
		// 		win.WriteTab("Chart status|" );
		// 		win.Write(" shortname| " + status.ShortName.PadRight(10));
		// 		win.Write(" type| " + status.Type.ToString().PadRight(12));
		// 		win.Write(" index| " + status.Index.ToString("D3").PadRight(5));
		// 		win.Write(" pd index| " + status.ParamDesc.Index.ToString("D3").PadRight(5));
		// 		win.Write(" class| " + status.ParamDesc.ParamClass.ToString().PadRight(12));
		// 		win.Write(" pd data type| " + status.ParamDesc.DataType.ToString().PadRight(15));
		// 		win.Write(" error| " + status.Error);
		// 		win.WriteLine("");
		// 	}
		// }


	}
}