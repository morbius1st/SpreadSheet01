// Solution:     SpreadSheet01
// Project:       Cells
// File:             SampleAnnoSymbols.cs
// Created:      2021-03-03 (10:13 PM)

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitChartInfo;
using SpreadSheet01.RevitSupport.RevitParamInfo;
using SpreadSheet01.RevitSupport.RevitParamValue;
using UtilityLibrary;

namespace Cells.CellsTests
{
	public class SampleAnnoSymbols : INotifyPropertyChanged
	{
		public AnnotationSymbol[] Charts {get; set; }

		public AnnotationSymbol[] Symbols {get; set; }

		public RevitCellParameters rcp = new RevitCellParameters();
		public RevitChartParameters rcpx = new RevitChartParameters();

		private int symbolIdx = 0;

		public void Process()
		{
			makeAnnoSyms();
			makeChartSyms();

			OnPropertyChanged(nameof(Charts));
			OnPropertyChanged(nameof(Symbols));

		}

		private void makeChartSyms()
		{
			Charts = new AnnotationSymbol[5];

			symbolIdx = 0;
			Charts[symbolIdx] = new AnnotationSymbol();
			Charts[symbolIdx].parameters = new List<Parameter>();
			Charts[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			AddChart1();

			symbolIdx = 1;
			Charts[symbolIdx] = new AnnotationSymbol();
			Charts[symbolIdx].parameters = new List<Parameter>();
			Charts[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			AddChart2();

			symbolIdx = 2;
			Charts[symbolIdx] = new AnnotationSymbol();
			Charts[symbolIdx].parameters = new List<Parameter>();
			Charts[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			AddChart3();

			symbolIdx = 3;
			Charts[symbolIdx] = new AnnotationSymbol();
			Charts[symbolIdx].parameters = new List<Parameter>();
			Charts[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			AddChart4();

			symbolIdx = 4;
			Charts[symbolIdx] = new AnnotationSymbol();
			Charts[symbolIdx].parameters = new List<Parameter>();
			Charts[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			AddChart5();
		}


		private void makeAnnoSyms()
		{
			Symbols = new AnnotationSymbol[3];

			symbolIdx = 0;
			Symbols[symbolIdx] = new AnnotationSymbol();
			Symbols[symbolIdx].parameters = new List<Parameter>();
			Symbols[symbolIdx].Name = "Symbol| " + symbolIdx.ToString("D2");
			AddSymbol1();

			symbolIdx = 1;
			Symbols[symbolIdx] = new AnnotationSymbol();
			Symbols[symbolIdx].parameters = new List<Parameter>();
			Symbols[symbolIdx].Name = "Symbol| " + symbolIdx.ToString("D2");
			AddSymbol2();

			symbolIdx = 2;
			Symbols[symbolIdx] = new AnnotationSymbol();
			Symbols[symbolIdx].parameters = new List<Parameter>();
			Symbols[symbolIdx].Name = "Symbol| " + symbolIdx.ToString("D2");
			AddSymbol3();
		}




		public void AddParameter( int index, int adjIdx,
			ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName = RevitCellParameters.CellAllParams[index + adjIdx].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, type, strVal, dblVal, intVal);
			Symbols[symbolIdx].parameters.Add(p);
		}

		private void AddSymbol1()
		{
			int adj1 = RevitCellParameters.ParamCounts[(int) ParamGroup.DATA];
			int adj2 = adj1 + RevitCellParameters.ParamCounts[(int) ParamGroup.CONTAINER];
			int adj3 = adj2 + RevitCellParameters.ParamCounts[(int) ParamGroup.LABEL];
			int adj4 = adj3 + RevitCellParameters.ParamCounts[(int) ParamGroup.LABEL];


			AddParameter(RevitCellParameters.NameIdx          , 0, ParamDataType.TEXT, "Myname1");
			AddParameter(RevitCellParameters.SeqIdx           , 0, ParamDataType.TEXT, "1");
			AddParameter(RevitCellParameters.CellAddrIdx      , 0, ParamDataType.TEXT, "@A2");
			AddParameter(RevitCellParameters.FormattingInfoIdx, 0, ParamDataType.TEXT, "format info");
			AddParameter(RevitCellParameters.GraphicType      , 0, ParamDataType.IGNORE, "graphic type");
			AddParameter(RevitCellParameters.DataIsToCellIdx  , 0, ParamDataType.BOOL  , "", 0.0, 1);
			AddParameter(RevitCellParameters.HasErrorsIdx     , 0, ParamDataType.IGNORE, "", 0.0, 1);
			AddParameter(RevitCellParameters.DataVisibleIdx   , 0, ParamDataType.IGNORE, "", 0.0, 1);

			AddParameter(RevitCellParameters.LabelsIdx        , 0, ParamDataType.EMPTY, "", 0.0, 0, "All Labels");

			AddParameter(RevitCellParameters.LabelIdx         , adj2, ParamDataType.TEXT, "", 0.0, 0, "Label #1 info 1");

			AddParameter(RevitCellParameters.lblRelAddrIdx    , adj2, ParamDataType.RELATIVEADDRESS, "(1,1)"   , 0.0, 0, "#1");
			AddParameter(RevitCellParameters.lblDataTypeIdx   , adj2, ParamDataType.DATATYPE       , "length"  , 0.0, 0, "#1");
			AddParameter(RevitCellParameters.lblFormulaIdx    , adj2, ParamDataType.FORMULA        , "=A+B"    , 0.0, 0, "#1");
			AddParameter(RevitCellParameters.lblIgnoreIdx     , adj2, ParamDataType.BOOL           , null      , 0.0, 1, "#1");
			AddParameter(RevitCellParameters.lblAsLengthIdx   , adj2, ParamDataType.IGNORE         , "A11.3"   , 0.0, 0, "#1");
			AddParameter(RevitCellParameters.lblAsNumberIdx   , adj2, ParamDataType.IGNORE         , "A11.4"   , 0.0, 0, "#1");
			AddParameter(RevitCellParameters.lblAsYesNoIdx    , adj2, ParamDataType.IGNORE         , "A11.5"   , 0.0, 0, "#1");

			AddParameter(RevitCellParameters.LabelIdx         , adj2, ParamDataType.TEXT, "", 0.0, 0, "Label #2 info");

			AddParameter(RevitCellParameters.lblRelAddrIdx    , adj2, ParamDataType.RELATIVEADDRESS, "(1,1)"   , 0.0, 0, "#2");
			AddParameter(RevitCellParameters.lblDataTypeIdx   , adj2, ParamDataType.DATATYPE       , "text"    , 0.0, 0, "#2");
			AddParameter(RevitCellParameters.lblFormulaIdx    , adj2, ParamDataType.FORMULA        , "=C+D"    , 0.0, 0, "#2");
			AddParameter(RevitCellParameters.lblIgnoreIdx     , adj2, ParamDataType.BOOL           , null      , 0.0, 1, "#2");
			AddParameter(RevitCellParameters.lblAsLengthIdx   , adj2, ParamDataType.IGNORE         , "A12.3"   , 0.0, 0, "#2");
			AddParameter(RevitCellParameters.lblAsNumberIdx   , adj2, ParamDataType.IGNORE         , "A12.4"   , 0.0, 0, "#2");
			AddParameter(RevitCellParameters.lblAsYesNoIdx    , adj2, ParamDataType.IGNORE         , "A12.5"   , 0.0, 0, "#2");

		}

		private void AddSymbol2()
		{
			int adj1 = RevitCellParameters.ParamCounts[(int) ParamGroup.DATA];
			int adj2 = adj1 + RevitCellParameters.ParamCounts[(int) ParamGroup.CONTAINER];

			AddParameter(RevitCellParameters.NameIdx          , 0, ParamDataType.TEXT, "Myname2");
			AddParameter(RevitCellParameters.SeqIdx           , 0, ParamDataType.TEXT, "1");
			AddParameter(RevitCellParameters.CellAddrIdx      , 0, ParamDataType.TEXT, "@A4");
			AddParameter(RevitCellParameters.FormattingInfoIdx, 0, ParamDataType.TEXT, "format info");
			AddParameter(RevitCellParameters.GraphicType      , 0, ParamDataType.IGNORE, "graphic type");
			AddParameter(RevitCellParameters.DataIsToCellIdx  , 0, ParamDataType.BOOL  , "", 0.0, 1);
			AddParameter(RevitCellParameters.HasErrorsIdx     , 0, ParamDataType.IGNORE, "", 0.0, 1);
			AddParameter(RevitCellParameters.DataVisibleIdx   , 0, ParamDataType.IGNORE, "", 0.0, 1);

			AddParameter(RevitCellParameters.LabelsIdx        , 0, ParamDataType.EMPTY, "", 0.0, 0, "All Labels");

			AddParameter(RevitCellParameters.LabelIdx         , adj2, ParamDataType.TEXT, "", 0.0, 0, name: "Label #1 info 2");

			AddParameter(RevitCellParameters.lblRelAddrIdx    , adj2, ParamDataType.RELATIVEADDRESS, "(1,2)"   , 0.0, 0, "#1");
			AddParameter(RevitCellParameters.lblDataTypeIdx   , adj2, ParamDataType.DATATYPE       , "length"  , 0.0, 0, "#1");
			AddParameter(RevitCellParameters.lblFormulaIdx    , adj2, ParamDataType.FORMULA        , "=E+F"    , 0.0, 0, "#1");
			AddParameter(RevitCellParameters.lblIgnoreIdx     , adj2, ParamDataType.BOOL           , null      , 0.0, 1, "#1");
			AddParameter(RevitCellParameters.lblAsLengthIdx   , adj2, ParamDataType.IGNORE         , "A2.3"    , 0.0, 0, "#1");
			AddParameter(RevitCellParameters.lblAsNumberIdx   , adj2, ParamDataType.IGNORE         , "A2.4"    , 0.0, 0, "#1");
			AddParameter(RevitCellParameters.lblAsYesNoIdx    , adj2, ParamDataType.IGNORE         , "A2.5"    , 0.0, 0, "#1");

			AddParameter(RevitCellParameters.LabelIdx         , 0, ParamDataType.ERROR, "", 0.0, 0, "end of list");
		}

		private void AddSymbol3()
		{
			int adj1 = RevitCellParameters.ParamCounts[(int) ParamGroup.DATA];
			int adj2 = adj1 + RevitCellParameters.ParamCounts[(int) ParamGroup.CONTAINER];

			AddParameter(RevitCellParameters.NameIdx          , 0, ParamDataType.TEXT, "Myname3");
			AddParameter(RevitCellParameters.SeqIdx           , 0, ParamDataType.TEXT, "1");
			AddParameter(RevitCellParameters.CellAddrIdx      , 0, ParamDataType.TEXT, "@A6");
			AddParameter(RevitCellParameters.FormattingInfoIdx, 0, ParamDataType.TEXT, "format info");
			AddParameter(RevitCellParameters.GraphicType      , 0, ParamDataType.IGNORE, "graphic type");
			AddParameter(RevitCellParameters.DataIsToCellIdx  , 0, ParamDataType.BOOL  , "", 0.0, 1);
			AddParameter(RevitCellParameters.HasErrorsIdx     , 0, ParamDataType.IGNORE, "", 0.0, 1);
			AddParameter(RevitCellParameters.DataVisibleIdx   , 0, ParamDataType.IGNORE, "", 0.0, 1);

			AddParameter(RevitCellParameters.LabelsIdx        , 0, ParamDataType.EMPTY, "", 0.0, 0, "All Labels");

			AddParameter(RevitCellParameters.LabelIdx         , adj2, ParamDataType.TEXT, "", 0.0, 0, "Label #1 info 3");
			AddParameter(RevitCellParameters.lblRelAddrIdx    , adj2, ParamDataType.RELATIVEADDRESS, "(1,3)"   , 0.0, 0, "#1");
			AddParameter(RevitCellParameters.lblDataTypeIdx   , adj2, ParamDataType.DATATYPE       , "length"  , 0.0, 0, "#1");
			AddParameter(RevitCellParameters.lblFormulaIdx    , adj2, ParamDataType.FORMULA        , "=G+H"    , 0.0, 0, "#1");
			AddParameter(RevitCellParameters.lblIgnoreIdx     , adj2, ParamDataType.BOOL           , null      , 0.0, 1, "#1");
			AddParameter(RevitCellParameters.lblAsLengthIdx   , adj2, ParamDataType.IGNORE         , "A3.3"    , 0.0, 0, "#1");
			AddParameter(RevitCellParameters.lblAsNumberIdx   , adj2, ParamDataType.IGNORE         , "A3.4"    , 0.0, 0, "#1");
			AddParameter(RevitCellParameters.lblAsYesNoIdx    , adj2, ParamDataType.IGNORE         , "A3.5"    , 0.0, 0, "#1");

			AddParameter(RevitCellParameters.LabelIdx         , 0, ParamDataType.ERROR, "", 0.0, 0, "end of list");
		}

		public void AddChart( int index, int adjIdx,
			ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName = RevitChartParameters.ChartAllParams[index + adjIdx].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, type, strVal, dblVal, intVal);
			Charts[symbolIdx].parameters.Add(p);
		}

		private void AddChart1()
		{

			AddChart(RevitChartParameters.ChartNameIdx        , 0, ParamDataType.TEXT, "Myname1");
			AddChart(RevitChartParameters.ChartDescIdx        , 0, ParamDataType.TEXT, "Description 1");
			AddChart(RevitChartParameters.ChartSeqIdx         , 0, ParamDataType.TEXT, "1");
			AddChart(RevitChartParameters.ChartFilePathIdx    , 0, ParamDataType.TEXT, @".\CsSampleChart_01_02.xlsx");
			AddChart(RevitChartParameters.ChartWorkSheetIdx   , 0, ParamDataType.TEXT, "CsSheet 1");
			AddChart(RevitChartParameters.ChartFamilyNameIdx  , 0, ParamDataType.TEXT, "CsCellFamily01");
			AddChart(RevitChartParameters.ChartUpdateTypeIdx  , 0, ParamDataType.UPDATE_TYPE, "Alyways");
			AddChart(RevitChartParameters.ChartHasErrorsIdx   , 0, ParamDataType.IGNORE, "");
		}

		private void AddChart2()
		{

			AddChart(RevitChartParameters.ChartNameIdx        , 0, ParamDataType.TEXT, "Myname2");
			AddChart(RevitChartParameters.ChartDescIdx        , 0, ParamDataType.TEXT, "Description 2");
			AddChart(RevitChartParameters.ChartSeqIdx         , 0, ParamDataType.TEXT, "2");
			AddChart(RevitChartParameters.ChartFilePathIdx    , 0, ParamDataType.TEXT, @".\CsSampleChart_01_02.xlsx");
			AddChart(RevitChartParameters.ChartWorkSheetIdx   , 0, ParamDataType.TEXT, "CsSheet 2");
			AddChart(RevitChartParameters.ChartFamilyNameIdx  , 0, ParamDataType.TEXT, "CsCellFamily02");
			AddChart(RevitChartParameters.ChartUpdateTypeIdx  , 0, ParamDataType.UPDATE_TYPE, "Alyways");
			AddChart(RevitChartParameters.ChartHasErrorsIdx   , 0, ParamDataType.IGNORE, "");
		}

		private void AddChart3()
		{

			AddChart(RevitChartParameters.ChartNameIdx        , 0, ParamDataType.TEXT, "Myname3");
			AddChart(RevitChartParameters.ChartDescIdx        , 0, ParamDataType.TEXT, "Description 3");
			AddChart(RevitChartParameters.ChartSeqIdx         , 0, ParamDataType.TEXT, "3");
			AddChart(RevitChartParameters.ChartFilePathIdx    , 0, ParamDataType.TEXT, @".\CsSampleChart_03_04.xlsx");
			AddChart(RevitChartParameters.ChartWorkSheetIdx   , 0, ParamDataType.TEXT, "CsSheet 1");
			AddChart(RevitChartParameters.ChartFamilyNameIdx  , 0, ParamDataType.TEXT, "CsCellFamily03");
			AddChart(RevitChartParameters.ChartUpdateTypeIdx  , 0, ParamDataType.UPDATE_TYPE, "Alyways");
			AddChart(RevitChartParameters.ChartHasErrorsIdx   , 0, ParamDataType.IGNORE, "");
		}
		
		private void AddChart4()
		{

			AddChart(RevitChartParameters.ChartNameIdx        , 0, ParamDataType.TEXT, "Myname4");
			AddChart(RevitChartParameters.ChartDescIdx        , 0, ParamDataType.TEXT, "Description 4 (not found worksheet)");
			AddChart(RevitChartParameters.ChartSeqIdx         , 0, ParamDataType.TEXT, "4");
			AddChart(RevitChartParameters.ChartFilePathIdx    , 0, ParamDataType.TEXT, @".\CsSampleChart_03_04.xlsx");
			AddChart(RevitChartParameters.ChartWorkSheetIdx   , 0, ParamDataType.TEXT, "CsSheet 2");
			AddChart(RevitChartParameters.ChartFamilyNameIdx  , 0, ParamDataType.TEXT, "CsCellFamily04");
			AddChart(RevitChartParameters.ChartUpdateTypeIdx  , 0, ParamDataType.UPDATE_TYPE, "Alyways");
			AddChart(RevitChartParameters.ChartHasErrorsIdx   , 0, ParamDataType.IGNORE, "");
		}
		
		private void AddChart5()
		{

			AddChart(RevitChartParameters.ChartNameIdx        , 0, ParamDataType.TEXT, "Myname5");
			AddChart(RevitChartParameters.ChartDescIdx        , 0, ParamDataType.TEXT, "Description 5 (not found chart)");
			AddChart(RevitChartParameters.ChartSeqIdx         , 0, ParamDataType.TEXT, "5");
			AddChart(RevitChartParameters.ChartFilePathIdx    , 0, ParamDataType.TEXT, @".\CsSampleChart_04.xlsx");
			AddChart(RevitChartParameters.ChartWorkSheetIdx   , 0, ParamDataType.TEXT, "CsSheet 1");
			AddChart(RevitChartParameters.ChartFamilyNameIdx  , 0, ParamDataType.TEXT, "CsCellFamily05");
			AddChart(RevitChartParameters.ChartUpdateTypeIdx  , 0, ParamDataType.UPDATE_TYPE, "Alyways");
			AddChart(RevitChartParameters.ChartHasErrorsIdx   , 0, ParamDataType.IGNORE, "");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	}
}