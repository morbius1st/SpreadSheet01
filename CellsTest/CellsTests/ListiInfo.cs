#region + Using Directives

using Autodesk.Revit.DB;

#endregion

// user name: jeffs
// created:   3/28/2021 6:54:03 AM

namespace CellsTest.CellsTests
{

	// public class ListiInfo
	// {
	//
	// 	public void listx() {}
	//
	// 	public void listSample(AnnotationSymbol[] chartSymbols, AnnotationSymbol[] symbols) { }
	//
	//
	//
	// }

}


/*


		public void listCharts(RevitCharts Charts) 
		{
			
			MainWindow.showTabId = false;

			MainWindow.TabClr(0);

			MainWindow.WriteLineTab("list charts (list of {RevitChart}");

			MainWindow.TabUp("start");
			{
				foreach (KeyValuePair<string, RevitChart> kvp1 in Charts.ListOfCharts)
				{
					MainWindow.WriteLine("");
					MainWindow.WriteLineTab("for chart| " + kvp1.Key);

					MainWindow.TabUp("");
					{
						MainWindow.WriteLine("");
						listChart(kvp1.Value);
					}
					MainWindow.TabDn("");

				}
			}	
			MainWindow.TabDn("end");
		}

		private void listChart(RevitChart chart)
		{
			MainWindow.WriteLineTab("Chart|       name| " + chart.Name);
			MainWindow.WriteLineTab("Chart|   sequence| " + chart.Sequence);
			MainWindow.WriteLineTab("Chart|   filepath| " + chart.FilePath);
			MainWindow.WriteLineTab("Chart|     exists| " + chart.Exists);
			MainWindow.WriteLineTab("Chart|  worksheet| " + chart.WorkSheet);
			MainWindow.WriteLineTab("Chart| updateType| " + chart.UpdateType);
			MainWindow.WriteLine("");

			MainWindow.TabUp("revit chart data| start");
			{
				RevitChartData rcd = chart.RevitChartData;

				MainWindow.WriteLineTab("RevitChartData|       name| " + rcd.Name);
				MainWindow.WriteLineTab("RevitChartData|   dynvalue| " + rcd.DynValue.ToString());
				MainWindow.WriteLineTab("RevitChartData|   sequence| " + rcd.Sequence);
				MainWindow.WriteLineTab("RevitChartData| updatetype| " + rcd.UpdateType);
			}
			MainWindow.TabDn("revit chart data| start");

		}


		private void listOneChart(RevitChart chart)
		{
			MainWindow.WriteLineTab("Chart|       name| " + chart.Name);
			MainWindow.WriteLineTab("Chart|   sequence| " + chart.Sequence);
			MainWindow.WriteLineTab("Chart|   filepath| " + chart.FilePath);
			MainWindow.WriteLineTab("Chart|     exists| " + chart.Exists);
			MainWindow.WriteLineTab("Chart|  worksheet| " + chart.WorkSheet);
			MainWindow.WriteLineTab("Chart| updateType| " + chart.UpdateType);
			MainWindow.Write("\n");
			MainWindow.WriteLineTab("Chart| parameters| (RevitChartData)");

			MainWindow.TabUp("revit chart data| start");
			{
				RevitChartData rcd = chart.RevitChartData;

				MainWindow.WriteLineTab("RevitChart|       name| " + rcd.Name);
				MainWindow.WriteLineTab("RevitChart|   dynvalue| " + rcd.DynValue.ToString());
				MainWindow.WriteLineTab("RevitChart|   sequence| " + rcd.Sequence);
				MainWindow.WriteLineTab("RevitChart| updatetype| " + rcd.UpdateType);
				
				MainWindow.TabUp("revit chart data| error codes| start");
				{
					List<ErrorCodes> errors = rcd.ErrorCodeList;

					if (errors != null && errors.Count > 0)
					{
						foreach (ErrorCodes errorCodes in errors)
						{
							MainWindow.WriteLineTab("RevitChart| error code| " + errorCodes);
						}
					}
					else
					{
						MainWindow.WriteLineTab("RevitChart| error code| NO ERRORS");
					}
				}
				MainWindow.TabDn("revit chart data| error codes| end");


				MainWindow.TabUp("revit chart params| start");
				{
					string s = chart.RevitChartData.Name;
					MainWindow.WriteLineTab("ChartData| Name| " + (s.IsVoid() ? "is void" : s));

					s = chart.RevitChartData.Sequence;
					MainWindow.WriteLineTab("ChartData|  Seq| " + (s.IsVoid() ? "is void" : s));

					// list a single chart's parameters
					foreach (ARevitParam p1 in chart.RevitChartData.RevitParamList)
					{
						if (p1 == null)
						{
							MainWindow.WriteLineTab("p1| is null");
						}
						else
						{
							MainWindow.WriteLineTab("p1| " + p1.ParamDesc.ParameterName.PadRight(15)
								+ "  value| " + p1.GetValue() );
						}
					}
				}
				MainWindow.TabDn("revit chart params| end");
			}
			MainWindow.TabDn("revit chart data| end");
		}







		public void listAllChartsInfo(RevitCharts Charts)
		{
			int tabid = 0;
			MainWindow.showTabId = false;

			MainWindow.TabClr(tabid);

			MainWindow.WriteLineTab("list charts (list of {RevitChart and contained info}");

			MainWindow.TabUp(1);

			MainWindow.WriteLine("");
			MainWindow.WriteLineTab("Status lists");

			{
				MainWindow.WriteLine("");
				MainWindow.WriteLineTab("Chart Status list\n");

				MainWindow.TabUp(1);
				{
					ParamStatusData[] s1 = RevitParamManager.ChartFam.StatusData;
					listStatusData(s1);
				}

				MainWindow.TabDn(2);

				MainWindow.WriteLine("");
				MainWindow.WriteLineTab("Chart Status list internal\n");

				MainWindow.TabUp(1);
				{
					MainWindow.WriteLineTab("working\n");

				}
				MainWindow.TabDn(2);

				MainWindow.WriteLine("");
				MainWindow.WriteLineTab("Cell Status list: Basic\n");

				MainWindow.TabUp(3);
				{
					ParamStatusData[] s2 = RevitParamManager.CellFam.StatusDataBasic;
					listStatusData(s2);
				}
				MainWindow.TabDn(4);
		
				MainWindow.WriteLine("");
				MainWindow.WriteLineTab("Cell Status list: Label\n");

				MainWindow.TabUp(3);
				{
					ParamStatusData[] s3 = RevitParamManager.CellFam.StatusDataLabel;
					listStatusData(s3);
				}
				MainWindow.TabDn(4);

				MainWindow.WriteLine("");
			}

			MainWindow.TabDn(4);

			MainWindow.TabUp(5);

			// start with a list of charts
			foreach (KeyValuePair<string, RevitChart> kvp1 in Charts.ListOfCharts)

		#region Chart

			{
				RevitChart Chart = kvp1.Value;
				// Chart.ListOfCellSyms["asdf"].

				// process a single chart
				MainWindow.Write("\n");
				MainWindow.WriteLineTab("list a chart| " + kvp1.Key);

				MainWindow.TabUp(10);
				{
					RevitChart v = kvp1.Value;
					

					MainWindow.WriteLineTab("Chart|       name| " + kvp1.Value.Name);
					MainWindow.WriteLineTab("Chart|   sequence| " + kvp1.Value.Sequence);
					MainWindow.WriteLineTab("Chart|   filepath| " + kvp1.Value.FilePath);
					MainWindow.WriteLineTab("Chart|     exists| " + kvp1.Value.Exists);
					MainWindow.WriteLineTab("Chart|  worksheet| " + kvp1.Value.WorkSheet);
					MainWindow.WriteLineTab("Chart| updateType| " + kvp1.Value.UpdateType);
					MainWindow.Write("\n");
					MainWindow.WriteLineTab("Chart| parameters| (RevitChartData)");

					MainWindow.TabUp(11);
					{
						string s = kvp1.Value.RevitChartData.Name;
						MainWindow.WriteLineTab("ChartData| Name| " + (s.IsVoid() ? "is void" : s));

						s = kvp1.Value.RevitChartData.Sequence;
						MainWindow.WriteLineTab("ChartData|  Seq| " + (s.IsVoid() ? "is void" : s));

						// list a single chart's parameters
						foreach (ARevitParam p1 in kvp1.Value.RevitChartData.RevitParamList)
						{
							if (p1 == null)
							{
								MainWindow.WriteLineTab("p1| is null");
							}
							else
							{
								MainWindow.WriteLineTab("p1| " + p1.ParamDesc.ParameterName.PadRight(15)
									+ "  value| " + p1.GetValue() );
							}
						}
					}
					MainWindow.TabDn(12);

					MainWindow.Write("\n");
					MainWindow.WriteLineTab("Chart| label cell shortcuts| (AllCellLabels)");

					MainWindow.TabUp(13);
					{
						{
							foreach (KeyValuePair<string, RevitLabel> kvp5 in kvp1.Value.AllCellLabels)
							{
								MainWindow.WriteTab("kvp5|   key| >" + kvp5.Key.PadRight(15) +"<");
								MainWindow.WriteLine(" value| formula| " + kvp5.Value.Formula);
							}
						}
						// MainWindow.TabDn(17);

						// MainWindow.TabUp(20);
						// {

					#region Cells

						MainWindow.Write("\n");
						MainWindow.WriteLineTab("Cells| [ListOfCellSyms] ");

						MainWindow.TabUp(25);
						{
							// list the collection of cell families
							foreach (KeyValuePair<string, RevitCellData> kvp2 in kvp1.Value.ListOfCellSyms)

						#region Cell

							{
								RevitCellData revitCellData = kvp2.Value;


								// list a single family
								MainWindow.Write("\n");
								MainWindow.WriteLineTab("Cell| " + revitCellData.ToString());

								MainWindow.TabUp(30);
								{
									MainWindow.WriteLineTab("Cell| key       | " + kvp2.Key);
									MainWindow.WriteLineTab("Cell| parameters| value| " + kvp2.Value.GetValue() ?? " is null");

									MainWindow.TabUp(33);
									{
									#region Cell Parameters

										foreach (ARevitParam p2 in kvp2.Value.RevitParamList)
										{
											if (p2 == null)
											{
												MainWindow.WriteLineTab("p2| is null");
											}
											else
											{
												MainWindow.WriteLineTab("p2| " + p2.ParamDesc.ParameterName.PadRight(15)
													+ "  value| " + p2.GetValue() );
											}
										}

									#endregion

										MainWindow.Write("\n");
										MainWindow.WriteLineTab("labels| (RevitCell [kvp2])");

									#region Shortcuts to Labels

										MainWindow.TabUp(35);
										{
											MainWindow.Write("\n");
											MainWindow.WriteLineTab("Labels| label cell shortcuts| ([CellLabels])");

											MainWindow.TabUp(36);
											{
												foreach (KeyValuePair<string, RevitLabel> kvp4 in kvp2.Value.ListOfLabels)
												{
													MainWindow.WriteTab("kvp4|   key| " + kvp4.Key.PadRight(15));
													MainWindow.WriteLine(" value| formula| " + kvp4.Value.Formula);
												}
											}
											MainWindow.TabDn(37);
										}
										MainWindow.TabDn(38);

									#endregion

										MainWindow.TabUp(40);
										{
											// MainWindow.WriteLineTab("labels value| " + kvp2.Value.LabelList.DynValue.Value?.ToString() ?? "is null");

											foreach (KeyValuePair<string, RevitLabel> kvp3 in kvp2.Value.ListOfLabels)

										#region Label

											{
												MainWindow.Write("\n");
												MainWindow.WriteLineTab("label| (RevitLabel [kvp3]) " + kvp3.Value.ToString());

												MainWindow.TabUp(43);
												{
													MainWindow.WriteLineTab("label| key       | " + kvp3.Key);
													MainWindow.WriteLineTab("label| value     | " + kvp3.Value.GetValue() ?? "is null");
													MainWindow.Write("\n");
													MainWindow.WriteLineTab("label| parameters| ([RevitParamList])");

													MainWindow.TabUp(45);
													{
														ARevitParam p3;

													#region Label Parameters

														for (var i = 0; i < kvp3.Value.RevitParamList.Length; i++)
														{
															MainWindow.WriteTab("p3| " + i + "| ");

															p3 = kvp3.Value.RevitParamList[i];

															if (p3 == null)
															{
																MainWindow.WriteLine("is null / not used");
															}
															else
															{
																MainWindow.WriteLine(p3.ParamDesc.ParameterName.PadRight(15)
																	+ "  index| " + p3.ParamDesc.Index.ToString("D3")
																	+ "  value| " + p3.GetValue() );
															}
														}

													#endregion
													}
													MainWindow.TabDn(50);
												}
												MainWindow.TabDn(52);
											}

										#endregion

											MainWindow.TabDn(53);
										}
									}
									MainWindow.TabDn(55);
								}
								MainWindow.TabDn(57);
							}

						#endregion
						}

						MainWindow.TabDn(60);
					}

				#endregion


					MainWindow.TabDn(62);
					// }
					// MainWindow.TabDn(62);
				}
				MainWindow.TabDn(65);
			}

		#endregion

			MainWindow.TabClr(70);
			MainWindow.TabClr();
		}


				
		private void listStatusData(ParamStatusData[] psd)
		{
			foreach (ParamStatusData status in psd)
			{
				MainWindow.WriteTab("Chart status|" );
				MainWindow.Write(" shortname| " + status.ShortName.PadRight(10));
				MainWindow.Write(" type| " + status.Type.ToString().PadRight(12));
				MainWindow.Write(" index| " + status.Index.ToString("D3").PadRight(5));
				MainWindow.Write(" pd index| " + status.ParamDesc.Index.ToString("D3").PadRight(5));
				MainWindow.Write(" class| " + status.ParamDesc.ParamClass.ToString().PadRight(12));
				MainWindow.Write(" pd data type| " + status.ParamDesc.DataType.ToString().PadRight(15));
				MainWindow.Write(" error| " + status.Error);
				MainWindow.WriteLine("");
			}
		}



	}
*/

