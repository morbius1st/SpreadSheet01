#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitParamManagement;
using SpreadSheet01.RevitSupport.RevitParamValue;
using SpreadSheet01.RevitSupport.RevitSelectionSupport;
using UtilityLibrary;
#if NOREVIT
using Cells.Windows;
using Cells.CellsTests;


#endif

#endregion

// username: jeffs
// created:  3/1/2021 10:29:39 PM

namespace SpreadSheet01.RevitSupport.RevitCellsManagement
{
	public class RevitCellSystManager : INotifyPropertyChanged
	{
	#region private fields

		private static string CHART_FAMILY_NAME = "SpreadSheetData";

		private const string ROOT_TRANSACTION_NAME = "Transaction Name";

		private const string KEY_IDX_BEGIN  = "《";
		private const string KEY_IDX_END    = "》";

		// collection of all revit charts  
		// this holds a collection of individual charts
		public RevitCharts Charts { get; private set; } = new RevitCharts();

		private readonly RevitParamCatagorize revitCat;
		private RevitSelectSupport rvtSelect;

		private int errorIdx;

	#endregion

	#region ctor

		public RevitCellSystManager()
		{
			revitCat = new RevitParamCatagorize();
			rvtSelect = new RevitSelectSupport();

			Reset();
		}

	#endregion

	#region public properties

		public static string RevitChartFamilyName => CHART_FAMILY_NAME;

		public RevitSelectSupport RvtSelect
		{
			get { return rvtSelect; }
		}


	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool ProcessCharts(RevitCharts Charts, CellUpdateTypeCode which)
		{
			int fail = 0;
			// process all charts and add to list
			foreach (KeyValuePair<string, RevitChart> kvp in Charts.ListOfCharts)
			{
				// chart has all parameters retreived
				RevitChart chart = kvp.Value;

				if (which != CellUpdateTypeCode.ALL &&
					kvp.Value.UpdateType != which) continue;

				processOneChart(kvp.Value);
			}

			return true;
		}


		public bool GetCurrentCharts()
		{
			ICollection<Element> chartFamilies;

			// step 1 - select all of the chart cells - place them into: 'chartFamilies'
			chartFamilies = findAllChartFamilies(CHART_FAMILY_NAME);

			// step 2 - process 'chartFamilies' and process all of the parameters
			getChartParams(chartFamilies);

			return chartFamilies != null && chartFamilies.Count > 0;
		}

	#endregion

	#region private methods

		private ICollection<Element> findAllChartFamilies(string chartFamilyName)
		{
		#if NOREVIT

			SampleAnnoSymbols samples = new SampleAnnoSymbols();

			samples.Process();

			return samples.ChartElements;
		#endif

		#if REVIT
			return null;

		#endif
		}

		private void getChartParams(ICollection<Element> chartFamilies)
		{
		#if NOREVIT
			foreach (Element el in chartFamilies)
			{
				RevitChartData chartData = revitCat.CatagorizeChartSymParams(el);

				chartData.RevitElement = el;
				chartData.AnnoSymbol = (AnnotationSymbol) el;

				string key;

				if (!chartData.IsValid)
				{
					key = "*** error *** (" + (++errorIdx).ToString("D3") + ")";
				}
				else
				{
					// fixed this
					// 	why 8 parameters
					key = RevitParamUtil.MakeAnnoSymKey(chartData,
						(int) RevitParamManager.NameIdx, (int) RevitParamManager.SeqIdx);
				}

				RevitChart chart = new RevitChart();

				chart.RevitChartData = chartData;

				Charts.Add(key, chart);
			}
		#endif
		}

		// provide the list of cell families
		// process a chart and get the parameters for a family
		private bool processOneChart(RevitChart chart)
		{
			string cellFamilyTypeName = chart.RevitChartData.GetValue();

		#if REVIT
			ICollection<Element> cellElements
				= RvtSelect.GetCellFamilies(RevitDoc.Doc, cellFamilyTypeName);
		#endif

		#if NOREVIT
			int i = Int32.Parse(chart[RevitParamManager.SeqIdx].GetValue());

			ICollection<Element> cellElements
				= RvtSelect.GetCellFamilies(RevitDoc.Doc, cellFamilyTypeName, i);

		#endif


			if (cellElements == null || cellElements.Count == 0) return false;

			chart.ListOfCellSyms = new Dictionary<string, RevitCell>();

			foreach (Element cell in cellElements)
			{
				RevitCell rvtCell = processCellFamily2(cell);

				chart.Add(rvtCell);
			}

			return true;
		}


		private RevitCell processCellFamily2(Element el)
		{
			RevitCell rvtCell = revitCat.catagorizeCellParams(el as AnnotationSymbol);

			if (!rvtCell.IsValid) return null;

			return rvtCell;
		}



		private void Reset()
		{
			Charts = new RevitCharts();
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is RevitChartManager";
		}

	#endregion

	#if NOREVIT

		public void listCharts2()
		{
			int tabid = 0;
			MainWindow.showTabId = false;

			MainWindow.TabClr(tabid);

			MainWindow.WriteLineTab("list charts (list of {RevitChart}");

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
					MainWindow.WriteLineTab("Chart| label cell shortcuts| ([CellLabels])");

					MainWindow.TabUp(13);
					{
						{
							foreach (KeyValuePair<string, RevitLabel> kvp5 in kvp1.Value.AllCellLabels)
							{
								MainWindow.WriteTab("kvp5|   key| " + kvp5.Key.PadRight(15));
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
							foreach (KeyValuePair<string, RevitCell> kvp2 in kvp1.Value.ListOfCellSyms)

						#region Cell

							{
								RevitCell revitCell = kvp2.Value;


								// list a single family
								MainWindow.Write("\n");
								MainWindow.WriteLineTab("Cell| " + revitCell.ToString());

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
												foreach (KeyValuePair<string, RevitLabel> kvp4 in kvp2.Value.CellLabels)
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
#endif
	}
}