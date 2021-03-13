#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using UtilityLibrary;
using SpreadSheet01.RevitSupport.RevitParamInfo;
using SpreadSheet01.RevitSupport.RevitParamValue;
#if NOREVIT
using Cells.Windows;
using Cells.CellsTests;
using SpreadSheet01.RevitSupport.RevitChartInfo;
#endif

#endregion

// username: jeffs
// created:  3/1/2021 10:29:39 PM

namespace SpreadSheet01.RevitSupport
{
	public class RevitChartManager : INotifyPropertyChanged
	{
	#region private fields

		private static string CHART_FAMILY_NAME = "SpreadSheetData";

		private const string KEY_IDX_BEGIN  = "《";
		private const string KEY_IDX_END    = "》";

		// collection of all revit charts  
		// this holds a collection of individual charts
		public RevitCharts Charts { get; private set; } = new RevitCharts();

		private readonly RevitCatagorizeParam revitCat = new RevitCatagorizeParam();

		private int errorIdx;

		// private RevitParamManager paramMgr;
		//
		// private List<RevitChartSym> errorSyms = new List<RevitChartSym>();
		//
		// private static int annoSymUniqueIdx = 0;


	#endregion

	#region ctor

		public RevitChartManager()
		{
			Reset();

			// paramMgr = new RevitParamManager();
		}

	#endregion

	#region public properties

		public static string RevitChartFamilyName => CHART_FAMILY_NAME;


	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool GetCurrentCharts()
		{
			ICollection<Element> chartFamilies;

			// step 1 - select all of the chart cells - place them into: 'chartFamilies'
			chartFamilies = findAllChartFamilies(CHART_FAMILY_NAME);

			// step 2 - process 'chartFamilies' and process all of the parameters
			getChartParams(chartFamilies);

			return chartFamilies != null && chartFamilies.Count > 0;
		}


		// #if NOREVIT
		// 	public string MakeChartSymKey(RevitChartSym cSym, bool asSeqName = false)
		// 	{
		// 		string seq = cSym[ChartSeqIdx].GetValue();
		//
		// 		seq = KEY_IDX_BEGIN + $"{(seq.IsVoid() ? "ZZZZZ" : seq),8}" + KEY_IDX_END;
		//
		// 		string name = cSym[ChartNameIdx].GetValue();
		// 		name = name.IsVoid() ? "un-named" : name;
		//
		// 		string eid = cSym.AnnoSymbol?.Id.ToString() ?? "Null Symbol " + annoSymUniqueIdx++.ToString("D7");
		//
		// 		if (asSeqName) return seq + name + eid;
		//
		// 		return name + seq + eid;
		// 	}
		// #endif

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
				RevitChartSym chartSym = revitCat.CatagorizeChartSymParams(el);

				chartSym.RvtElement = el;
				chartSym.AnnoSymbol = (AnnotationSymbol) el;

				string key;

				if (!chartSym.IsValid)
				{
					key = "*** error *** (" + (++errorIdx).ToString("D3") + ")";
				}
				else
				{
					// fixed this
					// 	why 8 parameters
					key = RevitParamUtil.MakeAnnoSymKey(chartSym,
						(int) RevitParamManager.NameIdx, (int) RevitParamManager.SeqIdx);
				}

				RevitChart chart = new RevitChart();

				chart.RvtChartSym = chartSym;

				Charts.Add(key, chart);
			}
		#endif
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

		public void listCharts()
		{
			MainWindow.TabClr();
			MainWindow.WriteLineTab("list charts");

			listOneRevitParam(Charts);

			foreach (KeyValuePair<string, RevitChart> kvp in Charts.Containers)
			{
				MainWindow.TabUp();

				MainWindow.WriteLineTab("\n*** list one chart");
				MainWindow.WriteLineTab("key 1  | " + kvp.Key);
				RevitChart chart = kvp.Value;

				listOneRevitParam(chart);

				MainWindow.WriteLineTab("charts| containers.count| " + chart.Containers.Count);
				MainWindow.WriteTab("\n");

				MainWindow.WriteLineTab("  *** list one family");

				foreach (ARevitParam p in chart.RvtChartSym.RevitParamList)
				{
					MainWindow.TabUp();

					MainWindow.WriteLineTab("*** root paramdesc| " + p.ParamDesc.ParameterName);
					listOneRevitParam(p);

					MainWindow.TabDn();
				}

				MainWindow.TabDn();
			}
		}

		private void listOneRevitParam(ARevitParam p)
		{
			if (p.DynValue.Value == null) return;

			MainWindow.TabUp();

			MainWindow.WriteLineTab("p| paramdesc.paramname| " + p.ParamDesc.ParameterName);
			MainWindow.WriteLineTab("p| DynValue.value| " + p.DynValue.Value?.ToString() ?? "is null");
			// MainWindow.WriteLine("p| Assigned| " + p.Assigned);
			// MainWindow.WriteLine("p| isvalid| " + p.IsValid);

			MainWindow.TabDn();
		}

		public void listCharts2()
		{
			int tabid = 0;

			MainWindow.TabClr(tabid);
			
			MainWindow.WriteLineTab("list charts");

			MainWindow.TabUp(++tabid);

			// start with a list of charts
			foreach (KeyValuePair<string, RevitChart> kvp1 in Charts.Containers)
			{
				// process a single chart
				MainWindow.Write("\n");
				MainWindow.WriteLineTab("list a chart| " + kvp1.Key);
				
				MainWindow.TabUp(++tabid);
				{
					MainWindow.WriteLineTab("updateType| " + kvp1.Value.RvtChartSym.UpdateType);
					MainWindow.WriteLineTab("chart parameters| ");

					MainWindow.TabUp(++tabid);

					// list a single chart's parameters
					foreach (ARevitParam p1 in kvp1.Value.RvtChartSym.RevitParamList)
					{
						MainWindow.WriteLineTab("p1| " + p1.ParamDesc.ParameterName.PadRight(15)
							+ "  value| " + p1.GetValue() );
					}
					MainWindow.TabDn(tabid--);

					MainWindow.Write("\n");
					MainWindow.WriteLineTab("families| ");

					MainWindow.TabUp(++tabid);

					// list the collection of cell families
					foreach (KeyValuePair<string, RevitCellSym> kvp2 in kvp1.Value.Containers)
					{
						// list a single family
						MainWindow.Write("\n");
						MainWindow.WriteLineTab("family key| " + kvp2.Key);

						MainWindow.WriteLineTab("family parameters| value| " + kvp2.Value.GetValue() ?? " is null");

						MainWindow.TabUp(++tabid);

						foreach (ARevitParam p2 in kvp2.Value.RevitParamList)
						{
							MainWindow.WriteLineTab("p2| " + p2.ParamDesc.ParameterName.PadRight(15)
								+ "  value| " + p2.GetValue() );
						}

						MainWindow.TabDn(tabid--);

						MainWindow.Write("\n");

						MainWindow.WriteLineTab("labels| ");

						MainWindow.TabUp(++tabid);

						MainWindow.WriteLineTab("labels value| " + kvp2.Value.LabelList.DynValue.Value?.ToString() ?? "is null");

						foreach (KeyValuePair<string, RevitLabel> kvp3 in kvp2.Value.LabelList.Containers)
						{
							MainWindow.WriteLineTab("label key| " + kvp3.Key);
							MainWindow.WriteLineTab("label parameters| value| " + kvp3.Value.GetValue() ?? "is null");

							MainWindow.TabUp(++tabid);

							ARevitParam p3;

							for (var i = 0; i < kvp3.Value.RevitParamList.Length; i++)
							{
								MainWindow.WriteTab("p3| " + i + "| ");

								p3 = kvp3.Value.RevitParamList[i];

								if (p3 == null)
								{
									MainWindow.WriteLine("is null\n");
								} 
								else
								{
									MainWindow.WriteLine(p3.ParamDesc.ParameterName.PadRight(15)
										+ "  value| " + p3.GetValue() );
								}
							}

							MainWindow.TabDn(tabid--);
						}
						MainWindow.TabDn(tabid--);
					}
					MainWindow.TabDn(tabid--);
				}
				MainWindow.TabDn(tabid--);
			}
			MainWindow.TabClr();
		}


#endif


	}
}