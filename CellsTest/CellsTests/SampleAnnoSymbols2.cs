// Solution:     SpreadSheet01
// // projname: CellsTest// File:             SampleAnnoSymbols.cs
// Created:      2021-03-03 (10:13 PM)

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitParamValue;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using static SpreadSheet01.RevitSupport.RevitParamManagement.RevitParamManager;
using SpreadSheet01.RevitSupport.RevitParamManagement;
using UtilityLibrary;

namespace CellsTest.CellsTests
{
	public class SampleAnnoSymbols : INotifyPropertyChanged
	{

		private ChartFamily chart;
		private CellFamily cell;

		private int symbolIdx = 0;
		private int chartIdx = 0;

		private bool initialized;


		public const int NUM_CHARTS = 5;
		public const int MAX_NUM_CELLS = 10;
		public const int NUM_SAMPLE_CELLS = 6;
		public const int BAD_CELL_IDX = NUM_SAMPLE_CELLS - 1;

		public static string[] CELL_FAMILY_NAMES = new []
		{
			CELL_FAMILY_NAME, CELL_FAMILY_NAME + " 2", "Cell Family Name 3", "Cell Family Name 4", "Cell Family Name 5", "Cell Family Name (Bad)"
		};
		// private AnnotationSymbol[] ChartSymbols { get; set; }
		// private AnnotationSymbol[] Symbols { get; set; }

		private AnnotationSymbol chartSymbol;

		public ICollection<Element> ChartElements { get; set; }
		// public ICollection<Element>[] CellElements { get; set; }

		public AnnotationSymbol[] SampleCells { get; set; } = new AnnotationSymbol[NUM_SAMPLE_CELLS];

		public Dictionary<string, ICollection<Element>> CellSyms { get; private set; }


		public AnnotationSymbol this[int idx] => SampleCells[idx];



		public void Process(string chartName)
		{
			if (initialized) return;
			

			// ChartFamily chart;
			bool MaskedTextResultHint = RevitParamManager.GetChartFamily(chartName, out chart);

			if (!MaskedTextResultHint) return;

			cell = chart.CellFamily;

			if (cell == null) return;

			initialized = true;

			CellSyms = new Dictionary<string, ICollection<Element>>();

			makeSampleCells();
			// makeCellSyms();
			makeChartSyms();

			// OnPropertyChanged(nameof(ChartSymbols));
		}

		private void makeSampleCells()
		{
			AnnotationSymbol cellSym;

			chartIdx = 0;
			// CellSyms = new Dictionary<string, ICollection<Element>>();

			for (chartIdx = 0; chartIdx < NUM_SAMPLE_CELLS; chartIdx++)
			{
				// CellSyms.Add(CELL_FAMILY_NAMES[chartIdx], new Element[MAX_NUM_CELLS]);

				cellSym = new AnnotationSymbol("Cell| " + chartIdx.ToString("D2") ,CELL_FAMILY_NAMES[chartIdx]);
				cellSym.Parameters = new ParameterSet();

				switch (chartIdx)
				{
				case 0:
					{
						AddCell_0_0(cellSym);
						break;
					}
				case 1:
					{
						AddCell_0_1(cellSym);
						break;
					}
				case 2:
					{
						AddCell_1_0(cellSym);
						break;
					}
				case 3:
					{
						AddCell_1_1(cellSym);
						break;
					}
				case 4:
					{
						AddCell_1_2(cellSym);
						break;
					}
				default:
					{
						AddCellBad(cellSym);
						break;
					}
				}

				SampleCells[chartIdx] = cellSym;
			}
		}


		public void AddCellInstParam(AnnotationSymbol aSym, int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName = cell[ParamType.PT_INSTANCE,index].ParameterName;
			// string paramName = RevitParamManager.CellAllParams[index + adjIdx].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			addCellParam(aSym, paramName, index, type, strVal, dblVal, intVal );
		}

		public void AddCellTypeParam(AnnotationSymbol aSym,int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName = cell[ParamType.PT_TYPE,index].ParameterName;
			// string paramName = RevitParamManager.CellAllParams[index + adjIdx].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, type, strVal, dblVal, intVal);
			aSym.Symbol.Parameters.Add(p);
		}

		public void AddCellInternalParam(AnnotationSymbol aSym,int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName =  cell[ParamType.PT_INTERNAL,index].ParameterName;
			// string paramName = RevitParamManager.CellAllParams[index + adjIdx].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, type, strVal, dblVal, intVal, false);
			aSym.Parameters.Add(p);
		}
		
		public void addCellParam(AnnotationSymbol aSym, string name, int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0)
		{
			Parameter p = new Parameter(name, type, strVal, dblVal, intVal);
			aSym.Parameters.Add(p);
		}

		public void AddLabelParam(AnnotationSymbol aSym,int index, int adjIdx,
			ParamDataType type, string strVal, string name = null,  
			double dblVal = 0.0, int intVal = 0, bool isMainLabel = false,
			bool boolVal = false)
		{
			string paramName = cell[ParamType.PT_LABEL, index].ParameterName;

			if (isMainLabel)
			{
				paramName = name.IsVoid() ? paramName : paramName  + " " + name;
			}
			else
			{
				paramName = name.IsVoid() ? paramName : name + " " + paramName;
			}

			Parameter p = new Parameter(paramName, type, strVal, dblVal, intVal);
			aSym.Parameters.Add(p);
		}

		
		private void addCellBasicParams(AnnotationSymbol aSym, string seq, string name)
		{
			AddCellInstParam(aSym, SeqIdx        , ParamDataType.DT_TEXT         , seq);
			AddCellInstParam(aSym, NameIdx       , ParamDataType.DT_TEXT         , name + chartIdx.ToString("D2"));
			AddCellInstParam(aSym, Descdx        , ParamDataType.DT_TEXT         , "Description 0");
			// AddCellInstParam(aSym, HasErrorsIdx  , ParamDataType.DT_IGNORE       , "", 0.0, 1);
		}

		private void addLabelParams(AnnotationSymbol aSym, string labelIdx, string name, 
			string datatype, string formula, string format, bool makeBad = false)
		{
			// string labelName = chartIdx.ToString("D2") + name;

			AddLabelParam(aSym, LblLabelIdx      , 0, ParamDataType.DT_TEXT      , ""          , labelIdx, 0.0, 0, true);
			// AddLabelParam(aSym, LblNameIdx       , 0, ParamDataType.DT_TEXT      , labelName   , labelIdx);
			AddLabelParam(aSym, LblFormulaIdx    , 0, ParamDataType.DT_FORMULA   , formula	 , labelIdx);

			if (!makeBad)
			{
				AddLabelParam(aSym, LblDataTypeIdx   , 0, ParamDataType.DT_DATATYPE  , datatype	 , labelIdx);
			}

			AddLabelParam(aSym, LblFormatInfoIdx , 0, ParamDataType.DT_TEXT      , format      , labelIdx);
			AddLabelParam(aSym, LblIgnoreIdx     , 0, ParamDataType.DT_BOOL      , null		 , labelIdx, 0.0, 1);

		}

		private void addCommonCellParams(AnnotationSymbol aSym)
		{
			AddCellTypeParam(aSym, IntNameIdx              , ParamDataType.DT_TEXT, "");
			AddCellTypeParam(aSym, DevelopIdx              , ParamDataType.DT_TEXT, "CyberStudio");
			AddCellInternalParam(aSym, CellInternalTempIdx , ParamDataType.DT_TEXT, "");
		}


		// private void makeSampleCells2()
		// {
		// 	int idx = BAD_CELL_IDX;
		// 	AnnotationSymbol cellSym = new AnnotationSymbol("Cell| " + idx.ToString("D2"), CELL_FAMILY_NAMES[idx]);
		// 	cellSym.Parameters = new ParameterSet();
		// 	AddCellBad(cellSym);
		// 	SampleCells[idx] = cellSym;
		//
		// 	idx = 0;
		// 	cellSym = new AnnotationSymbol("Cell| " + idx.ToString("D2"), CELL_FAMILY_NAMES[idx]);
		// 	cellSym.Parameters = new ParameterSet();
		// 	AddCell_0_0(cellSym);
		// 	SampleCells[idx] = cellSym;
		// 	
		// 	idx = 1;
		// 	cellSym = new AnnotationSymbol("Cell| " + idx.ToString("D2"), CELL_FAMILY_NAMES[idx]);
		// 	cellSym.Parameters = new ParameterSet();
		// 	AddCell_0_1(cellSym);
		// 	SampleCells[idx] = cellSym;
		// 				
		// 	idx = 2;
		// 	cellSym = new AnnotationSymbol("Cell| " + idx.ToString("D2"), CELL_FAMILY_NAMES[idx]);
		// 	cellSym.Parameters = new ParameterSet();
		// 	AddCell_1_0(cellSym);
		// 	SampleCells[idx] = cellSym;
		// 	
		// 	idx = 3;
		// 	cellSym = new AnnotationSymbol("Cell| " + idx.ToString("D2"), CELL_FAMILY_NAMES[idx]);
		// 	cellSym.Parameters = new ParameterSet();
		// 	AddCell_1_1(cellSym);
		// 	SampleCells[idx] = cellSym;
		// 	
		// 	idx = 4;
		// 	cellSym = new AnnotationSymbol("Cell| " + idx.ToString("D2"), CELL_FAMILY_NAMES[idx]);
		// 	cellSym.Parameters = new ParameterSet();
		// 	AddCell_1_2(cellSym);
		// 	SampleCells[idx] = cellSym;
		// }

				
		// this is an error cell - duplicate name
		// also, label 1 has "make bad" set to true
		private void AddCellBad(AnnotationSymbol aSym)
		{
			if (chartIdx == 0) return;

			addCellBasicParams(aSym, "A.A", "MyCellname2.");

			// AddCellInstParam(SeqIdx           , ParamDataType.DT_TEXT, "1");
			// AddCellInstParam(NameIdx          , ParamDataType.DT_TEXT, "MyCellname2." + chartIdx.ToString("D2"));
			// AddCellInstParam(HasErrorsIdx     , ParamDataType.DT_IGNORE, "", 0.0, 1);

			addLabelParams(aSym, "#1", "MyLabelname -2-1", "text", "={#SheetName}", "", true);
			addLabelParams(aSym, "#2", "MyLabelname -2-2", "text", "={#SheetNumber}", "");
			addLabelParams(aSym, "#3", "MyLabelname -2-3", "text", "={$Date(yyyy-mm-dd)}", "");

			addCommonCellParams(aSym);
		}


		// cell 0_0 & cell 0_1 - associate with the same chart
		// so must have the same number of labels
		// also associated with cell bad (same number of labels)
		private void AddCell_0_0(AnnotationSymbol aSym)
		{
			addCellBasicParams(aSym, "0.0", "MyCellname1.");

			// AddCellInstParam(SeqIdx        , ParamDataType.DT_TEXT         , "0");
			// AddCellInstParam(NameIdx       , ParamDataType.DT_TEXT         , "MyCellname1." + chartIdx.ToString("D2"));
			// AddCellInstParam(Descdx        , ParamDataType.DT_TEXT         , "Description 0");
			// AddCellInstParam(HasErrorsIdx  , ParamDataType.DT_IGNORE       , ""                  , 0.0, 1);

			addLabelParams(aSym, "#1", "MyLabelname -0-1", "length", "={[A1]}", "#,###");
			addLabelParams(aSym, "#2", "MyLabelname -0-2", "length", "={[A2]}", "#,##0");
			addLabelParams(aSym, "#3", "MyLabelname -0-3", "text"  , "={[A3]}", "");

			addCommonCellParams(aSym);
		}

		private void AddCell_0_1(AnnotationSymbol aSym)
		{
			addCellBasicParams(aSym, "1A.", "MyCellname2.");

			// AddCellInstParam(SeqIdx           , ParamDataType.DT_TEXT, "1");
			// AddCellInstParam(NameIdx          , ParamDataType.DT_TEXT, "MyCellname2." + chartIdx.ToString("D2"));
			// AddCellInstParam(HasErrorsIdx     , ParamDataType.DT_IGNORE, "", 0.0, 1);

			addLabelParams(aSym, "#1", "MyLabelname -1-1", "text", "={#SheetName}", "");
			addLabelParams(aSym, "#2", "MyLabelname -1-2", "text", "={#SheetNumber}", "");
			addLabelParams(aSym, "#3", "MyLabelname -1-3", "text", "={$Date(yyyy-mm-dd)}", "");

			addCommonCellParams(aSym);
		}

		private void AddCell_1_0(AnnotationSymbol aSym)
		{
			addCellBasicParams(aSym, ".A0", "MyCellname_1_0.");

			addLabelParams(aSym, "#1", "MyLabelname -0-1", "length", "={[A1]}", "#,###");
			addLabelParams(aSym, "#2", "MyLabelname -0-2", "length", "={[A2]}", "#,###");
			addLabelParams(aSym, "#3", "MyLabelname -0-3", "length", "={[A3]}", "#,###");
			addLabelParams(aSym, "#4", "MyLabelname -0-4", "length", "={[A4]}", "#,###");

			addCommonCellParams(aSym);
		}
		
		private void AddCell_1_1(AnnotationSymbol aSym)
		{
			addCellBasicParams(aSym, "xy.z", "MyCellname_1_1.");

			addLabelParams(aSym, "#1", "MyLabelname -1-1", "text", "={#SheetName}", "#,###");
			addLabelParams(aSym, "#2", "MyLabelname -1-4", "text", "={#SheetNumber}", "#,###");
			addLabelParams(aSym, "#3", "MyLabelname -1-3", "text", "={#RevitFileName}", "#,###");
			addLabelParams(aSym, "#4", "MyLabelname -1-4", "text", "={#SheetNumber}", "#,###");

			addCommonCellParams(aSym);
		}

		private void AddCell_1_2(AnnotationSymbol aSym)
		{
			addCellBasicParams(aSym, "alp.ha", "MyCellname_1_2.");

			addLabelParams(aSym, "#1", "MyLabelname -2-1", "text", "={$Date(yyyy-mm-dd)}", "#,###");
			addLabelParams(aSym, "#2", "MyLabelname -2-2", "text", "={%project number}", "#,###");
			addLabelParams(aSym, "#3", "MyLabelname -2-3", "text", "={!global1}", "#,###");
			addLabelParams(aSym, "#4", "MyLabelname -2-4", "text", "={@MyLabelname 1-0-1}", "#,###");

			addCommonCellParams(aSym);
		}

		private void makeChartSyms()
		{
			chartIdx = 0;

			Element el;
			ChartElements = new List<Element>();
			// ChartSymbols = new AnnotationSymbol[5];

			symbolIdx = 0;
			// ChartSymbols[symbolIdx]
			chartSymbol	= new AnnotationSymbol("Chart| " + symbolIdx.ToString("D2"), CHART_FAMILY_NAME);
			// ChartSymbols[symbolIdx].Parameters = new List<Parameter>();
			// ChartSymbols[symbolIdx].Parameters = new ParameterSet();
			chartSymbol.Parameters = new ParameterSet();
			AddChart0(symbolIdx);
			// el = (Element) ChartSymbols[symbolIdx];
			el = (Element) chartSymbol;
			ChartElements.Add(el);

			symbolIdx = 1;
			chartSymbol	= new AnnotationSymbol("Chart| " + symbolIdx.ToString("D2"), CHART_FAMILY_NAME);
			chartSymbol.Parameters = new ParameterSet();
			AddChart1(symbolIdx);
			el = (Element) chartSymbol;
			ChartElements.Add(el);
			
			symbolIdx = 2;
			chartSymbol	= new AnnotationSymbol("Chart| " + symbolIdx.ToString("D2"), CHART_FAMILY_NAME);
			chartSymbol.Parameters = new ParameterSet();
			AddChart2(symbolIdx);
			el = (Element) chartSymbol;
			ChartElements.Add(el);
			
			symbolIdx = 3;
			chartSymbol	= new AnnotationSymbol("Chart| " + symbolIdx.ToString("D2"), CHART_FAMILY_NAME);
			chartSymbol.Parameters = new ParameterSet();
			AddChart3(symbolIdx);
			el = (Element) chartSymbol;
			ChartElements.Add(el);

			symbolIdx = 4;
			// ChartSymbols[symbolIdx] 
			chartSymbol	= new AnnotationSymbol("Chart| " + symbolIdx.ToString("D2"), CHART_FAMILY_NAME);
			// ChartSymbols[symbolIdx].Parameters = new ParameterSet();
			chartSymbol.Parameters = new ParameterSet();
			AddChart4(symbolIdx);
			// el = (Element) ChartSymbols[symbolIdx];
			el = (Element) chartSymbol;
			ChartElements.Add(el);
		}

		
		private void configSampleCells(int idx, int[] sampleCellList)
		{
			Element[] el = new Element[sampleCellList.Length];

			for (var i = 0; i < sampleCellList.Length; i++)
			{
				el[i] = (Element) SampleCells[sampleCellList[i]];
			}

			CellSyms.Add(CELL_FAMILY_NAMES[idx], el);
		}


		public void AddChartInstParam(int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName = chart[ParamType.PT_INSTANCE,index].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			addChartParam(paramName, index, type, strVal, dblVal, intVal );
		}

		public void AddChartTypeParam(int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName =  chart[ParamType.PT_TYPE,index].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, type, strVal, dblVal, intVal);
			// ChartSymbols[symbolIdx].Symbol.Parameters.Add(p);
			chartSymbol.Symbol.Parameters.Add(p);
		}

		public void AddChartInternalParam(int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName =  chart[ParamType.PT_INTERNAL,index].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, type, strVal, dblVal, intVal, false);
			// ChartSymbols[symbolIdx].Parameters.Add(p);
			chartSymbol.Parameters.Add(p);
		}

		private void addChartParam(string name, int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0)
		{
			Parameter p = new Parameter(name, type, strVal, dblVal, intVal);
			// ChartSymbols[symbolIdx].Parameters.Add(p);
			chartSymbol.Parameters.Add(p);
		}

		private void addCommonChartParams()
		{
			AddChartTypeParam(IntNameIdx          , ParamDataType.DT_TEXT, "");
			AddChartTypeParam(DevelopIdx          , ParamDataType.DT_TEXT, "CyberStudio");
			AddChartInternalParam(ChartInternalTempIdx, ParamDataType.DT_TEXT, "");

			// AddChartInternalParam(CellParamsClassIdx  , ParamDataType.DT_TEXT, CELL_FAMILY_NAME);
		}



		private void AddChart0(int idx)
		{
			AddChartInstParam(SeqIdx              , ParamDataType.DT_TEXT, ".10");
			AddChartInstParam(NameIdx             , ParamDataType.DT_TEXT, "MyChartname0");
			AddChartInstParam(Descdx              , ParamDataType.DT_TEXT, "Description 0");
			AddChartInstParam(ChartFilePathIdx    , ParamDataType.DT_TEXT, @".\CsSampleChart_01_02.xlsx");
			AddChartInstParam(ChartWorkSheetIdx   , ParamDataType.DT_TEXT, "CsSheet 1");
			AddChartInstParam(ChartCellFamilyNameIdx , ParamDataType.DT_TEXT, CELL_FAMILY_NAMES[idx]);
			AddChartInstParam(ChartUpdateTypeIdx  , ParamDataType.DT_UPDATE_TYPE, "Alyways");
			// AddChartInstParam(ChartHasErrorsIdx   , ParamDataType.DT_TEXT, "");

			addCommonChartParams();

			configSampleCells(idx, new [] {0, 1});
		}

		private void AddChart1(int idx)
		{
			AddChartInstParam(SeqIdx                 , ParamDataType.DT_TEXT, "1a.");
			AddChartInstParam(ChartFilePathIdx       , ParamDataType.DT_TEXT, @".\CsSampleChart_01_02.xlsx");
			AddChartInstParam(NameIdx                , ParamDataType.DT_TEXT, "MyChartname1");
			AddChartInstParam(ChartWorkSheetIdx      , ParamDataType.DT_TEXT, "CsSheet 2");
			AddChartInstParam(ChartCellFamilyNameIdx , ParamDataType.DT_TEXT, CELL_FAMILY_NAMES[idx]);
			AddChartInstParam(ChartUpdateTypeIdx     , ParamDataType.DT_UPDATE_TYPE, "Alyways");
			// AddChartInstParam(ChartHasErrorsIdx      , ParamDataType.DT_TEXT, "");

			addCommonChartParams();

			configSampleCells(idx, new [] {2, 3, 4});
		}

		private void AddChart2(int idx)
		{
			AddChartInstParam(SeqIdx                 , ParamDataType.DT_TEXT, "");
			AddChartInstParam(ChartFilePathIdx       , ParamDataType.DT_TEXT, @".\CsSampleChart_03_04.xlsx");
			AddChartInstParam(ChartWorkSheetIdx      , ParamDataType.DT_TEXT, "CsSheet 1");
			AddChartInstParam(NameIdx                , ParamDataType.DT_TEXT, "MyChartname2");
			AddChartInstParam(ChartCellFamilyNameIdx , ParamDataType.DT_TEXT, CELL_FAMILY_NAMES[idx]);
			AddChartInstParam(ChartUpdateTypeIdx     , ParamDataType.DT_UPDATE_TYPE, "Alyways");
			// AddChartInstParam(ChartHasErrorsIdx      , ParamDataType.DT_TEXT, "");

			addCommonChartParams();

			configSampleCells(idx, new [] {0, 1});
		}

		private void AddChart3(int idx)
		{
			AddChartInstParam(NameIdx                , ParamDataType.DT_TEXT, "MyChartname3");
			// AddChart(RevitParamManager.ChartDescIdx        , 0, ParamDataType.TEXT, "Description 4 (not found worksheet)");
			AddChartInstParam(SeqIdx                 , ParamDataType.DT_TEXT, ".");
			AddChartInstParam(ChartFilePathIdx       , ParamDataType.DT_TEXT, @".\CsSampleChart_03_04.xlsx");
			AddChartInstParam(ChartWorkSheetIdx      , ParamDataType.DT_TEXT, "CsSheet 2");
			AddChartInstParam(ChartCellFamilyNameIdx , ParamDataType.DT_TEXT, CELL_FAMILY_NAMES[idx]);
			AddChartInstParam(ChartUpdateTypeIdx     , ParamDataType.DT_UPDATE_TYPE, "Alyways");
			// AddChartInstParam(ChartHasErrorsIdx      , ParamDataType.DT_TEXT, "");

			addCommonChartParams();

			configSampleCells(idx, new [] {0, 1});
		}

		private void AddChart4(int idx)
		{
			AddChartInstParam(NameIdx                , ParamDataType.DT_TEXT, "MyChartname4");
			// AddChart(RevitParamManager.ChartDescIdx        , 0, ParamDataType.TEXT, "Description 5 (not found chart)");
			AddChartInstParam(SeqIdx                 , ParamDataType.DT_TEXT, "4x.yz");
			AddChartInstParam(ChartFilePathIdx       , ParamDataType.DT_TEXT, @".\CsSampleChart_04.xlsx");
			// AddChartInstParam(ChartWorkSheetIdx      , ParamDataType.DT_TEXT, "CsSheet 1");
			AddChartInstParam(ChartCellFamilyNameIdx , ParamDataType.DT_TEXT, CELL_FAMILY_NAMES[idx]);
			AddChartInstParam(ChartUpdateTypeIdx     , ParamDataType.DT_UPDATE_TYPE, "Alyways");
			// AddChartInstParam(ChartHasErrorsIdx      , ParamDataType.DT_TEXT, "");

			addCommonChartParams();

			configSampleCells(idx, new [] {1, 2, BAD_CELL_IDX});
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}