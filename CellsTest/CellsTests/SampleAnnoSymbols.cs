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
	public class SampleAnnoSymbols2 : INotifyPropertyChanged
	{
		public const int NUM_CHARTS = 5;
		public const int MAX_NUM_CELLS = 10;

		public static string[] CELL_FAMILY_NAMES = new []
		{
			CELL_FAMILY_NAME, CELL_FAMILY_NAME + " 2", "Cell Family Name 3", "Cell Family Name 4", "Cell Family Name 5"
		};

		public ICollection<Element> ChartElements { get; set; }
		public ICollection<Element>[] CellElements { get; set; }

		private AnnotationSymbol[] ChartSymbols { get; set; }
		private AnnotationSymbol[] Symbols { get; set; }


		public Dictionary<string, Element[]> CellSyms { get; private set; }


		private ChartFamily chart;
		private CellFamily cell;

		private int symbolIdx = 0;
		private int chartIdx = 0;

		private bool initialized;

		public void Process(string chartName)
		{
			if (initialized) return;
			initialized = true;

			// ChartFamily chart;
			bool MaskedTextResultHint = RevitParamManager.GetChartFamily(chartName, out chart);

			if (!MaskedTextResultHint) return;

			// this.chart = chart;
			// bool result = chart.GetCellFamily(CELL_FAMILY_NAME, out cell);

			cell = chart.CellFamily;

			if (cell == null) return;

			makeCellSyms();
			makeChartSyms();

			OnPropertyChanged(nameof(ChartSymbols));
			OnPropertyChanged(nameof(Symbols));
		}

		private void makeChartSyms()
		{
			chartIdx = 0;

			Element el;
			ChartElements = new List<Element>();
			ChartSymbols = new AnnotationSymbol[5];

			symbolIdx = 0;
			ChartSymbols[symbolIdx] 
				= new AnnotationSymbol("Chart| " + symbolIdx.ToString("D2"), CHART_FAMILY_NAME);
			// ChartSymbols[symbolIdx].Parameters = new List<Parameter>();
			ChartSymbols[symbolIdx].Parameters = new ParameterSet();
			AddChart0();
			el = (Element) ChartSymbols[symbolIdx];
			ChartElements.Add(el);

			symbolIdx = 1;
			ChartSymbols[symbolIdx] 
				= new AnnotationSymbol("Chart| " + symbolIdx.ToString("D2"), CHART_FAMILY_NAME);
			ChartSymbols[symbolIdx].Parameters = new ParameterSet();
			// ChartSymbols[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			AddChart1();
			el = (Element) ChartSymbols[symbolIdx];
			ChartElements.Add(el);

			// symbolIdx = 2;
			// ChartSymbols[symbolIdx] 
			// 	= new AnnotationSymbol("Chart| " + symbolIdx.ToString("D2"), CHART_FAMILY_NAME);
			// ChartSymbols[symbolIdx].parameters = new List<Parameter>();
			// // ChartSymbols[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			// AddChart2();
			// el = (Element) ChartSymbols[symbolIdx];
			// ChartElements.Add(el);

			// symbolIdx = 1;
			// ChartSymbols[symbolIdx] 
			// 	= new AnnotationSymbol("Chart| " + symbolIdx.ToString("D2"), CHART_FAMILY_NAME);
			// ChartSymbols[symbolIdx].parameters = new List<Parameter>();
			// // ChartSymbols[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			// AddChart3();
			// el = (Element) ChartSymbols[symbolIdx];
			// ChartElements.Add(el);

			symbolIdx = 2;
			ChartSymbols[symbolIdx] 
				= new AnnotationSymbol("Chart| " + symbolIdx.ToString("D2"), CHART_FAMILY_NAME);
			ChartSymbols[symbolIdx].Parameters = new ParameterSet();
			AddChart4();
			el = (Element) ChartSymbols[symbolIdx];
			ChartElements.Add(el);
		}

		private void makeCellElements()
		{
			AnnotationSymbol aSym;

			chartIdx = 0;
			CellSyms = new Dictionary<string, Element[]>();

			for (int i = 0; i < NUM_CHARTS; i++)
			{
				CellSyms.Add(CELL_FAMILY_NAMES[i], new Element[MAX_NUM_CELLS]);

				aSym = new AnnotationSymbol("Cell| " + i.ToString("D2") + 1.ToString("D2") ,CELL_FAMILY_NAMES[i]);
				aSym.Parameters = new ParameterSet();



			}

		}

		private void makeCellSyms()
		{
			int qty = 10;

			CellElements = new ICollection<Element>[qty];
			Symbols = new AnnotationSymbol[4];


			for (int j = 0; j < 3; j++)
			{
				chartIdx = j;

				CellElements[j] = new List<Element>();

				for (int i = 0; i < 4; i++)
				{
					symbolIdx = i;

					addCellSymbol(i, j);
				}
			}
		}

		private void addCellSymbol(int idx, int ix)
		{
			Symbols[idx] = null;

			Symbols[idx] = new AnnotationSymbol("Cell| " + ix.ToString("D2") + idx.ToString("D2") , CELL_FAMILY_NAME);
			// Symbols[idx].Parameters = new List<Parameter>();
			Symbols[idx].Parameters = new ParameterSet();
			// Symbols[idx].Name = "Symbol| " + ix.ToString("D2") + idx.ToString("D2") ;

			switch (idx)
			{
			case 0:
				{
					AddCell_0_0();
					break;
				}
			case 1:
				{
					AddCell_0_1();
					break;
				}
			case 2:
				{
					AddCellBad();
					break;
				}
			// case 3:
			// 	{
			// 		AddSymbol3();
			// 		break;
			// 	}
			}

			Element el = (Element) Symbols[idx];
			CellElements[ix].Add(el);
		}

		public void AddCellInstParam(int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName = cell[ParamType.PT_INSTANCE,index].ParameterName;
			// string paramName = RevitParamManager.CellAllParams[index + adjIdx].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			addCellParam(paramName, index, type, strVal, dblVal, intVal );
		}

		public void AddCellTypeParam(int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName = cell[ParamType.PT_TYPE,index].ParameterName;
			// string paramName = RevitParamManager.CellAllParams[index + adjIdx].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, type, strVal, dblVal, intVal);
			Symbols[symbolIdx].Symbol.Parameters.Add(p);
		}

		public void AddCellInternalParam(int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName =  cell[ParamType.PT_INTERNAL,index].ParameterName;
			// string paramName = RevitParamManager.CellAllParams[index + adjIdx].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, type, strVal, dblVal, intVal, false);
			Symbols[symbolIdx].Parameters.Add(p);
		}
		
		public void addCellParam(string name, int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0)
		{
			Parameter p = new Parameter(name, type, strVal, dblVal, intVal);
			Symbols[symbolIdx].Parameters.Add(p);
		}

		public void AddLabelParam(int index, int adjIdx,
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
			Symbols[symbolIdx].Parameters.Add(p);
		}

		
		private void addCellBasicParams(string seq, string name)
		{
			AddCellInstParam(SeqIdx        , ParamDataType.DT_TEXT         , seq);
			AddCellInstParam(NameIdx       , ParamDataType.DT_TEXT         , name + chartIdx.ToString("D2"));
			AddCellInstParam(Descdx        , ParamDataType.DT_TEXT         , "Description 0");
			AddCellInstParam(HasErrorsIdx  , ParamDataType.DT_IGNORE       , "", 0.0, 1);
		}

		private void addLabelParams(string labelIdx, string name, string datatype, string formula, string format, bool makeBad = false)
		{
			string labelName = chartIdx.ToString("D2") + name;

			AddLabelParam(LblLabelIdx      , 0, ParamDataType.DT_TEXT      , ""          , labelIdx, 0.0, 0, true);
			// AddLabelParam(LblNameIdx       , 0, ParamDataType.DT_TEXT      , labelName   , labelIdx);
			AddLabelParam(LblFormulaIdx    , 0, ParamDataType.DT_FORMULA   , formula	 , labelIdx);

			if (!makeBad)
			{
				AddLabelParam(LblDataTypeIdx   , 0, ParamDataType.DT_DATATYPE  , datatype	 , labelIdx);
			}

			AddLabelParam(LblFormatInfoIdx , 0, ParamDataType.DT_TEXT      , format      , labelIdx);
			AddLabelParam(LblIgnoreIdx     , 0, ParamDataType.DT_BOOL      , null		 , labelIdx, 0.0, 1);

		}

		private void addCommonCellParams()
		{
			AddCellTypeParam(IntNameIdx              , ParamDataType.DT_TEXT, "");
			AddCellTypeParam(DevelopIdx              , ParamDataType.DT_TEXT, "CyberStudio");
			AddCellInternalParam(CellInternalTempIdx , ParamDataType.DT_TEXT, "");
		}

		// cell 0_0 & cell 0_1 - associate with the same chart
		// so must have the same number of labels
		// also associated with cell bad (same number of labels)
		private void AddCell_0_0()
		{
			addCellBasicParams("0", "MyCellname1.");

			// AddCellInstParam(SeqIdx        , ParamDataType.DT_TEXT         , "0");
			// AddCellInstParam(NameIdx       , ParamDataType.DT_TEXT         , "MyCellname1." + chartIdx.ToString("D2"));
			// AddCellInstParam(Descdx        , ParamDataType.DT_TEXT         , "Description 0");
			// AddCellInstParam(HasErrorsIdx  , ParamDataType.DT_IGNORE       , ""                  , 0.0, 1);

			addLabelParams("#1", "MyLabelname -0-1", "length", "={[A1]}", "#,###");
			addLabelParams("#2", "MyLabelname -0-2", "length", "={[A2]}", "#,##0");
			addLabelParams("#3", "MyLabelname -0-3", "text"  , "={[A3]}", "");

			addCommonCellParams();
		}

		private void AddCell_0_1()
		{
			addCellBasicParams("1", "MyCellname2.");

			// AddCellInstParam(SeqIdx           , ParamDataType.DT_TEXT, "1");
			// AddCellInstParam(NameIdx          , ParamDataType.DT_TEXT, "MyCellname2." + chartIdx.ToString("D2"));
			// AddCellInstParam(HasErrorsIdx     , ParamDataType.DT_IGNORE, "", 0.0, 1);

			addLabelParams("#1", "MyLabelname -1-1", "text", "={#SheetName}", "");
			addLabelParams("#2", "MyLabelname -1-2", "text", "={#SheetNumber}", "");
			addLabelParams("#3", "MyLabelname -1-3", "text", "={$Date(yyyy-mm-dd)}", "");

			addCommonCellParams();
		}
		
		// this is an error cell - duplicate name
		// also, label 1 has "make bad" set to true
		private void AddCellBad()
		{
			if (chartIdx == 0) return;

			addCellBasicParams("1", "MyCellname2.");

			// AddCellInstParam(SeqIdx           , ParamDataType.DT_TEXT, "1");
			// AddCellInstParam(NameIdx          , ParamDataType.DT_TEXT, "MyCellname2." + chartIdx.ToString("D2"));
			// AddCellInstParam(HasErrorsIdx     , ParamDataType.DT_IGNORE, "", 0.0, 1);

			addLabelParams("#1", "MyLabelname -2-1", "text", "={#SheetName}", "", true);
			addLabelParams("#2", "MyLabelname -2-2", "text", "={#SheetNumber}", "");
			addLabelParams("#3", "MyLabelname -2-3", "text", "={$Date(yyyy-mm-dd)}", "");

			addCommonCellParams();
		}

		private void AddCell_1_0()
		{
			addCellBasicParams("0", "MyCellname_1_0.");

			addLabelParams("#1", "MyLabelname -0-1", "length", "={[A1]}", "#,###");
			addLabelParams("#2", "MyLabelname -0-2", "length", "={[A2]}", "#,###");
			addLabelParams("#3", "MyLabelname -0-3", "length", "={[A3]}", "#,###");
			addLabelParams("#4", "MyLabelname -0-4", "length", "={[A4]}", "#,###");

			addCommonCellParams();
		}
		
		private void AddCell_1_1()
		{
			addCellBasicParams("1", "MyCellname_1_1.");

			addLabelParams("#1", "MyLabelname -1-1", "text", "={#SheetName}", "#,###");
			addLabelParams("#2", "MyLabelname -1-4", "text", "={#SheetNumber}", "#,###");
			addLabelParams("#3", "MyLabelname -1-3", "text", "={#RevitFileName}", "#,###");
			addLabelParams("#4", "MyLabelname -1-4", "text", "={#SheetNumber}", "#,###");

			addCommonCellParams();
		}

		private void AddCell_1_2()
		{
			addCellBasicParams("3", "MyCellname_1_2.");

			addLabelParams("#1", "MyLabelname -2-1", "text", "={$Date(yyyy-mm-dd)}", "#,###");
			addLabelParams("#2", "MyLabelname -2-2", "text", "={%project number}", "#,###");
			addLabelParams("#3", "MyLabelname -2-3", "text", "={!global1}", "#,###");
			addLabelParams("#4", "MyLabelname -2-4", "text", "={@MyLabelname 1-0-1}", "#,###");

			addCommonCellParams();
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
			ChartSymbols[symbolIdx].Symbol.Parameters.Add(p);
		}

		public void AddChartInternalParam(int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName =  chart[ParamType.PT_INTERNAL,index].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, type, strVal, dblVal, intVal, false);
			ChartSymbols[symbolIdx].Parameters.Add(p);
		}

		private void addChartParam(string name, int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0)
		{
			Parameter p = new Parameter(name, type, strVal, dblVal, intVal);
			ChartSymbols[symbolIdx].Parameters.Add(p);
		}

		private void addCommonChartParams()
		{
			AddChartTypeParam(IntNameIdx          , ParamDataType.DT_TEXT, "");
			AddChartTypeParam(DevelopIdx          , ParamDataType.DT_TEXT, "CyberStudio");
			AddChartInternalParam(ChartInternalTempIdx, ParamDataType.DT_TEXT, "");

			// AddChartInternalParam(CellParamsClassIdx  , ParamDataType.DT_TEXT, CELL_FAMILY_NAME);
		}

		private void AddChart0()
		{
			

			AddChartInstParam(SeqIdx              , ParamDataType.DT_TEXT, "0");
			AddChartInstParam(NameIdx             , ParamDataType.DT_TEXT, "MyChartname0");
			AddChartInstParam(Descdx              , ParamDataType.DT_TEXT, "Description 0");
			AddChartInstParam(ChartFilePathIdx    , ParamDataType.DT_TEXT, @".\CsSampleChart_01_02.xlsx");
			AddChartInstParam(ChartWorkSheetIdx   , ParamDataType.DT_TEXT, "CsSheet 1");
			AddChartInstParam(ChartCellFamilyNameIdx , ParamDataType.DT_TEXT, CELL_FAMILY_NAME);
			AddChartInstParam(ChartUpdateTypeIdx  , ParamDataType.DT_UPDATE_TYPE, "Alyways");
			AddChartInstParam(ChartHasErrorsIdx   , ParamDataType.DT_TEXT, "");

			addCommonChartParams();
		}

		private void AddChart1()
		{
			AddChartInstParam(SeqIdx                 , ParamDataType.DT_TEXT, "1");
			AddChartInstParam(ChartFilePathIdx       , ParamDataType.DT_TEXT, @".\CsSampleChart_01_02.xlsx");
			AddChartInstParam(NameIdx                , ParamDataType.DT_TEXT, "MyChartname1");
			AddChartInstParam(ChartWorkSheetIdx      , ParamDataType.DT_TEXT, "CsSheet 2");
			AddChartInstParam(ChartCellFamilyNameIdx     , ParamDataType.DT_TEXT, "CsCellFamily02");
			AddChartInstParam(ChartUpdateTypeIdx     , ParamDataType.DT_UPDATE_TYPE, "Alyways");
			AddChartInstParam(ChartHasErrorsIdx      , ParamDataType.DT_TEXT, "");

			addCommonChartParams();
		}

		private void AddChart2()
		{
			AddChartInstParam(SeqIdx                 , ParamDataType.DT_TEXT, "2");
			AddChartInstParam(ChartFilePathIdx       , ParamDataType.DT_TEXT, @".\CsSampleChart_03_04.xlsx");
			AddChartInstParam(ChartWorkSheetIdx      , ParamDataType.DT_TEXT, "CsSheet 1");
			AddChartInstParam(NameIdx                , ParamDataType.DT_TEXT, "MyChartname2");
			AddChartInstParam(ChartCellFamilyNameIdx     , ParamDataType.DT_TEXT, "CsCellFamily03");
			AddChartInstParam(ChartUpdateTypeIdx     , ParamDataType.DT_UPDATE_TYPE, "Alyways");
			AddChartInstParam(ChartHasErrorsIdx      , ParamDataType.DT_TEXT, "");

			addCommonChartParams();
		}

		private void AddChart3()
		{
			AddChartInstParam(NameIdx                , ParamDataType.DT_TEXT, "MyChartname3");
			// AddChart(RevitParamManager.ChartDescIdx        , 0, ParamDataType.TEXT, "Description 4 (not found worksheet)");
			AddChartInstParam(SeqIdx                 , ParamDataType.DT_TEXT, "3");
			AddChartInstParam(ChartFilePathIdx       , ParamDataType.DT_TEXT, @".\CsSampleChart_03_04.xlsx");
			AddChartInstParam(ChartWorkSheetIdx      , ParamDataType.DT_TEXT, "CsSheet 2");
			AddChartInstParam(ChartCellFamilyNameIdx     , ParamDataType.DT_TEXT, "CsCellFamily04");
			AddChartInstParam(ChartUpdateTypeIdx     , ParamDataType.DT_UPDATE_TYPE, "Alyways");
			AddChartInstParam(ChartHasErrorsIdx      , ParamDataType.DT_TEXT, "");

			addCommonChartParams();
		}

		private void AddChart4()
		{
			AddChartInstParam(NameIdx                , ParamDataType.DT_TEXT, "MyChartname4");
			// AddChart(RevitParamManager.ChartDescIdx        , 0, ParamDataType.TEXT, "Description 5 (not found chart)");
			AddChartInstParam(SeqIdx                 , ParamDataType.DT_TEXT, "4");
			AddChartInstParam(ChartFilePathIdx       , ParamDataType.DT_TEXT, @".\CsSampleChart_04.xlsx");
			// AddChartInstParam(ChartWorkSheetIdx      , ParamDataType.DT_TEXT, "CsSheet 1");
			AddChartInstParam(ChartCellFamilyNameIdx     , ParamDataType.DT_TEXT, "CsCellFamily05");
			AddChartInstParam(ChartUpdateTypeIdx     , ParamDataType.DT_UPDATE_TYPE, "Alyways");
			AddChartInstParam(ChartHasErrorsIdx      , ParamDataType.DT_TEXT, "");

			addCommonChartParams();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}