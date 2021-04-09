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
		public ICollection<Element> ChartElements { get; set; }
		public ICollection<Element>[] CellElements { get; set; }

		public AnnotationSymbol[] ChartSymbols { get; set; }

		public AnnotationSymbol[] Symbols { get; set; }

		private ChartFamily chart;
		private CellFamily cell;


		private int symbolIdx = 0;
		private int chartIdx = 0;

		public void Process(string chartName)
		{

			// ChartFamily chart;
			bool MaskedTextResultHint = RevitParamManager.GetChartFamily(chartName, out chart);

			if (!MaskedTextResultHint) return;

			// this.chart = chart;
			// bool result = chart.GetCellFamily(CELL_FAMILY_NAME, out cell);

			cell = chart.CellFamily;

			if (cell == null) return;

			makeAnnoSyms();
			makeChartSyms();

			OnPropertyChanged(nameof(ChartSymbols));
			OnPropertyChanged(nameof(Symbols));
		}

		private void makeChartSyms()
		{
			Element el;
			ChartElements = new List<Element>();
			ChartSymbols = new AnnotationSymbol[5];

			symbolIdx = 0;
			ChartSymbols[symbolIdx] 
				= new AnnotationSymbol("Chart| " + symbolIdx.ToString("D2"), CHART_FAMILY_NAME);
			ChartSymbols[symbolIdx].parameters = new List<Parameter>();
			AddChart0();
			el = (Element) ChartSymbols[symbolIdx];
			ChartElements.Add(el);

			symbolIdx = 1;
			ChartSymbols[symbolIdx] 
				= new AnnotationSymbol("Chart| " + symbolIdx.ToString("D2"), CHART_FAMILY_NAME);
			ChartSymbols[symbolIdx].parameters = new List<Parameter>();
			// ChartSymbols[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			AddChart1();
			el = (Element) ChartSymbols[symbolIdx];
			ChartElements.Add(el);

			symbolIdx = 2;
			ChartSymbols[symbolIdx] 
				= new AnnotationSymbol("Chart| " + symbolIdx.ToString("D2"), CHART_FAMILY_NAME);
			ChartSymbols[symbolIdx].parameters = new List<Parameter>();
			// ChartSymbols[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			AddChart2();
			el = (Element) ChartSymbols[symbolIdx];
			ChartElements.Add(el);

			symbolIdx = 3;
			ChartSymbols[symbolIdx] 
				= new AnnotationSymbol("Chart| " + symbolIdx.ToString("D2"), CHART_FAMILY_NAME);
			ChartSymbols[symbolIdx].parameters = new List<Parameter>();
			// ChartSymbols[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			AddChart3();
			el = (Element) ChartSymbols[symbolIdx];
			ChartElements.Add(el);

			// symbolIdx = 4;
			// ChartSymbols[symbolIdx] 
			// 	= new AnnotationSymbol("Chart| " + symbolIdx.ToString("D2"), CHART_FAMILY_NAME);
			// ChartSymbols[symbolIdx].parameters = new List<Parameter>();
			// // ChartSymbols[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			// AddChart4();
			// el = (Element) ChartSymbols[symbolIdx];
			// ChartElements.Add(el);
		}

		private void makeAnnoSyms()
		{
			CellElements = new ICollection<Element>[5];
			Symbols = new AnnotationSymbol[4];


			for (int j = 0; j < 3; j++)
			{
				chartIdx = j;

				CellElements[j] = new List<Element>();

				for (int i = 0; i < 4; i++)
				{
					symbolIdx = i;

					addSymbol(i, j);
				}
			}
		}

		private void addSymbol(int idx, int ix)
		{
			Symbols[idx] = null;

			Symbols[idx] = new AnnotationSymbol("Symbol| " + ix.ToString("D2") + idx.ToString("D2") , CELL_FAMILY_NAME);
			Symbols[idx].parameters = new List<Parameter>();
			// Symbols[idx].Name = "Symbol| " + ix.ToString("D2") + idx.ToString("D2") ;

			switch (idx)
			{
			case 0:
				{
					AddSymbol0();
					break;
				}
			case 1:
				{
					AddSymbol1();
					break;
				}
			case 2:
				{
					AddSymbol2();
					break;
				}
			case 3:
				{
					AddSymbol3();
					break;
				}
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
			Symbols[symbolIdx].Symbol.parameters.Add(p);
		}

		public void AddCellInternalParam(int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName =  cell[ParamType.PT_INTERNAL,index].ParameterName;
			// string paramName = RevitParamManager.CellAllParams[index + adjIdx].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, type, strVal, dblVal, intVal, false);
			Symbols[symbolIdx].parameters.Add(p);
		}


		public void addCellParam(string name, int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0)
		{
			Parameter p = new Parameter(name, type, strVal, dblVal, intVal);
			Symbols[symbolIdx].parameters.Add(p);
		}


		public void AddLabelParam( int index, int adjIdx,
			ParamDataType type,
			string strVal,
			string name = null,  double dblVal = 0.0, int intVal = 0, bool isMainLabel = false,
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
			Symbols[symbolIdx].parameters.Add(p);
		}

		private void addCommonCellParams()
		{
			AddCellTypeParam(IntNameIdx              , ParamDataType.DT_TEXT, "");
			AddCellTypeParam(DevelopIdx              , ParamDataType.DT_TEXT, "CyberStudio");
			AddCellInternalParam(CellInternalTempIdx , ParamDataType.DT_TEXT, "");
		}


		private void AddSymbol0()
		{
			AddCellInstParam(SeqIdx           , ParamDataType.DT_TEXT, "0");
			AddCellInstParam(NameIdx          , ParamDataType.DT_TEXT, "MyCellname1" + chartIdx.ToString("D2"));
			AddCellInstParam(Descdx           , ParamDataType.DT_TEXT, "Description 0");

			AddCellInstParam(HasErrorsIdx     , ParamDataType.DT_IGNORE, "", 0.0, 1);

			AddLabelParam(LblLabelIdx         , 0, ParamDataType.DT_TEXT        , "", "#1", 0.0, 0, true);

			AddLabelParam(LblDataTypeIdx   , 0, ParamDataType.DT_DATATYPE       , "length"			 , "#1");
			AddLabelParam(LblNameIdx       , 0, ParamDataType.DT_TEXT           , "MyLabelname 0-1 " + chartIdx.ToString("D2")   , "#1");
			AddLabelParam(LblFormulaIdx    , 0, ParamDataType.DT_FORMULA        , "={[A1]}"			 , "#1");
			AddLabelParam(LblIgnoreIdx     , 0, ParamDataType.DT_BOOL           , null				 , "#1", 0.0, 1);
			AddLabelParam(LblFormatInfoIdx , 0, ParamDataType.DT_TEXT           , "#,###"			 , "#1");

			AddLabelParam(LblLabelIdx         , 0, ParamDataType.DT_TEXT, "", "#2", 0.0, 0, true);

			AddLabelParam(LblNameIdx       , 0, ParamDataType.DT_TEXT           , "MyLabelname 0-2 " + chartIdx.ToString("D2")   , "#2");
			AddLabelParam(LblDataTypeIdx   , 0, ParamDataType.DT_DATATYPE       , "text"				 , "#2");
			AddLabelParam(LblFormulaIdx    , 0, ParamDataType.DT_FORMULA        , "={[A2]}"			 , "#2");
			AddLabelParam(LblFormatInfoIdx , 0, ParamDataType.DT_TEXT           , "#,###"			 , "#2");
			AddLabelParam(LblIgnoreIdx     , 0, ParamDataType.DT_BOOL           , null				 , "#2", 0.0, 1);

			addCommonCellParams();
		}


		private void AddSymbol1()
		{
			AddCellInstParam(SeqIdx           , ParamDataType.DT_TEXT, "1");
			AddCellInstParam(NameIdx          , ParamDataType.DT_TEXT, "MyCellname2" + chartIdx.ToString("D2"));

			AddCellInstParam(HasErrorsIdx     , ParamDataType.DT_IGNORE, "", 0.0, 1);

			AddLabelParam(LblLabelIdx         , 0, ParamDataType.DT_TEXT        , "", "#1", 0.0, 0, true);

			AddLabelParam(LblDataTypeIdx   , 0, ParamDataType.DT_DATATYPE       , "length"			 , "#1");
			AddLabelParam(LblNameIdx       , 0, ParamDataType.DT_TEXT           , "MyLabelname 1-1 " + chartIdx.ToString("D2")   , "#1");
			AddLabelParam(LblFormulaIdx    , 0, ParamDataType.DT_FORMULA        , "={#SheetName}"    , "#1");
			AddLabelParam(LblFormatInfoIdx , 0, ParamDataType.DT_TEXT           , "#,##0"			 , "#1");
			AddLabelParam(LblIgnoreIdx     , 0, ParamDataType.DT_BOOL           , null				 , "#1", 0.0, 1);

			addCommonCellParams();
		}


		private void AddSymbol2()
		{
			AddCellInstParam(SeqIdx           , ParamDataType.DT_TEXT, "2");
			AddCellInstParam(NameIdx          , ParamDataType.DT_TEXT, "MyCellname3" + chartIdx.ToString("D2"));
			AddCellInstParam(HasErrorsIdx     , ParamDataType.DT_IGNORE, "", 0.0, 1);

			AddLabelParam(LblLabelIdx         , 0, ParamDataType.DT_TEXT        , "", "#1", 0.0, 0, true);

			AddLabelParam(LblDataTypeIdx   , 0, ParamDataType.DT_DATATYPE       , "length"           , "#1");
			AddLabelParam(LblNameIdx       , 0, ParamDataType.DT_TEXT           , "MyLabelname 2-1 " + chartIdx.ToString("D2")  , "#1");
			AddLabelParam(LblFormulaIdx    , 0, ParamDataType.DT_FORMULA        , "={$Date(yyyy-mm-dd)}" , "#1");
			AddLabelParam(LblIgnoreIdx     , 0, ParamDataType.DT_BOOL           , null               , "#1", 0.0, 1);

			addCommonCellParams();
		}


		private void AddSymbol3()
		{
			AddCellInstParam(SeqIdx           , ParamDataType.DT_TEXT, "3");
			AddCellInstParam(NameIdx          , ParamDataType.DT_TEXT, "MyCellname4" + chartIdx.ToString("D2"));
			AddCellInstParam(HasErrorsIdx     , ParamDataType.DT_IGNORE, "", 0.0, 1);

			AddLabelParam(LblLabelIdx         , 0, ParamDataType.DT_TEXT        , "", "#1", 0.0, 0, true);

			AddLabelParam(LblDataTypeIdx   , 0, ParamDataType.DT_DATATYPE       , "length"	        , "#1");
			AddLabelParam(LblFormulaIdx    , 0, ParamDataType.DT_FORMULA        , "={[A5]}"	        , "#1");
			AddLabelParam(LblNameIdx       , 0, ParamDataType.DT_TEXT           , "MyLabelname 3-1 " + chartIdx.ToString("D2")  , "#1");
			AddLabelParam(LblIgnoreIdx     , 0, ParamDataType.DT_BOOL           , null		        , "#1", 0.0, 1);

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
			ChartSymbols[symbolIdx].Symbol.parameters.Add(p);
		}

		public void AddChartInternalParam(int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName =  chart[ParamType.PT_INTERNAL,index].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, type, strVal, dblVal, intVal, false);
			ChartSymbols[symbolIdx].parameters.Add(p);
		}

		private void addChartParam(string name, int index, ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0)
		{
			Parameter p = new Parameter(name, type, strVal, dblVal, intVal);
			ChartSymbols[symbolIdx].parameters.Add(p);
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
			AddChartInstParam(ChartWorkSheetIdx      , ParamDataType.DT_TEXT, "CsSheet 1");
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