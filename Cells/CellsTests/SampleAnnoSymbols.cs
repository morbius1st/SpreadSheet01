// Solution:     SpreadSheet01
// Project:       Cells
// File:             SampleAnnoSymbols.cs
// Created:      2021-03-03 (10:13 PM)

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitChartInfo;
using SpreadSheet01.RevitSupport.RevitParamInfo;
using SpreadSheet01.RevitSupport.RevitParamValue;
using UtilityLibrary;

namespace Cells.CellsTests
{
	public class SampleAnnoSymbols : INotifyPropertyChanged
	{
		public ICollection<Element> ChartElements { get; set; }
		public ICollection<Element> CellElements { get; set; }

		public AnnotationSymbol[] Charts {get; set; }

		public AnnotationSymbol[] Symbols {get; set; }

		// public RevitParamManager rcp = new RevitParamManager();
		// public RevitParamManager rcpx = new RevitParamManager();

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
			Element el;
			ChartElements = new List<Element>();
			Charts = new AnnotationSymbol[5];

			symbolIdx = 0;
			Charts[symbolIdx] = new AnnotationSymbol();
			Charts[symbolIdx].parameters = new List<Parameter>();
			Charts[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");

			AddChart1();
			el = (Element) Charts[symbolIdx];
			ChartElements.Add(el);

			symbolIdx = 1;
			Charts[symbolIdx] = new AnnotationSymbol();
			Charts[symbolIdx].parameters = new List<Parameter>();
			Charts[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			AddChart2();
			el = (Element) Charts[symbolIdx];
			ChartElements.Add(el);

			symbolIdx = 2;
			Charts[symbolIdx] = new AnnotationSymbol();
			Charts[symbolIdx].parameters = new List<Parameter>();
			Charts[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			AddChart3();
			el = (Element) Charts[symbolIdx];
			ChartElements.Add(el);

			symbolIdx = 3;
			Charts[symbolIdx] = new AnnotationSymbol();
			Charts[symbolIdx].parameters = new List<Parameter>();
			Charts[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			AddChart4();
			el = (Element) Charts[symbolIdx];
			ChartElements.Add(el);

			symbolIdx = 4;
			Charts[symbolIdx] = new AnnotationSymbol();
			Charts[symbolIdx].parameters = new List<Parameter>();
			Charts[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			AddChart5();
			el = (Element) Charts[symbolIdx];
			ChartElements.Add(el);
		}


		private void makeAnnoSyms()
		{
			Symbols = new AnnotationSymbol[3];
			Element el;
			CellElements = new List<Element>();


			symbolIdx = 0;
			Symbols[symbolIdx] = new AnnotationSymbol();
			Symbols[symbolIdx].parameters = new List<Parameter>();
			Symbols[symbolIdx].Name = "Symbol| " + symbolIdx.ToString("D2");
			AddSymbol1();
			el = (Element) Symbols[symbolIdx];
			CellElements.Add(el);

			symbolIdx = 1;
			Symbols[symbolIdx] = new AnnotationSymbol();
			Symbols[symbolIdx].parameters = new List<Parameter>();
			Symbols[symbolIdx].Name = "Symbol| " + symbolIdx.ToString("D2");
			AddSymbol2();
			el = (Element) Symbols[symbolIdx];
			CellElements.Add(el);

			symbolIdx = 2;
			Symbols[symbolIdx] = new AnnotationSymbol();
			Symbols[symbolIdx].parameters = new List<Parameter>();
			Symbols[symbolIdx].Name = "Symbol| " + symbolIdx.ToString("D2");
			AddSymbol3();
			el = (Element) Symbols[symbolIdx];
			CellElements.Add(el);
		}




		public void AddInstParameter( int index, int adjIdx,
			ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName = RevitParamManager.CellInstParam(index).ParameterName;
			// string paramName = RevitParamManager.CellAllParams[index + adjIdx].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, type, strVal, dblVal, intVal);
			Symbols[symbolIdx].parameters.Add(p);
		}

		public void AddLabelParameter( int index, int adjIdx,
			ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null, bool isMainLabel = false)
		{
			string paramName = RevitParamManager.CellLabelParam(index).ParameterName;

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

		private void AddSymbol1()
		{
			// int adj1 = RevitParamManager.ParamCounts[(int) ParamGroup.DATA];
			// int 0 = adj1 + RevitParamManager.ParamCounts[(int) ParamGroup.CONTAINER];
			// int adj3 = 0 + RevitParamManager.ParamCounts[(int) ParamGroup.LABEL_GRP];
			// int adj4 = adj3 + RevitParamManager.ParamCounts[(int) ParamGroup.LABEL_GRP];


			AddInstParameter(RevitParamManager.NameIdx          , 0, ParamDataType.TEXT, "Myname1");
			AddInstParameter(RevitParamManager.SeqIdx           , 0, ParamDataType.TEXT, "1");
			// AddParameter(RevitParamManager.CellAddrIdx      , 0, ParamDataType.TEXT, "@A2");
			AddInstParameter(RevitParamManager.HasErrorsIdx     , 0, ParamDataType.IGNORE, "", 0.0, 1);

			// AddParameter(RevitParamManager.GraphicType      , 0, ParamDataType.IGNORE, "graphic type");
			// AddParameter(RevitParamManager.DataVisibleIdx   , 0, ParamDataType.IGNORE, "", 0.0, 1);

			// AddParameter(RevitParamManager.LabelsIdx        , 0, ParamDataType.EMPTY, "", 0.0, 0, "All Labels");

			AddLabelParameter(RevitParamManager.LblLabelIdx         , 0, ParamDataType.TEXT, "", 0.0, 0, "#1", true);

			// AddParameter(RevitParamManager.LblRelAddrIdx    , 0, ParamDataType.RELATIVEADDRESS, "(1,1)"   , 0.0, 0, "#1");
			AddLabelParameter(RevitParamManager.LblNameIdx       , 0, ParamDataType.TEXT           , "Myname1-1 "  , 0.0, 0, "#1");
			AddLabelParameter(RevitParamManager.LblDataTypeIdx   , 0, ParamDataType.DATATYPE       , "length"      , 0.0, 0, "#1");
			AddLabelParameter(RevitParamManager.LblFormulaIdx    , 0, ParamDataType.FORMULA        , "=[A1]"       , 0.0, 0, "#1");
			AddLabelParameter(RevitParamManager.LblIgnoreIdx     , 0, ParamDataType.BOOL           , null          , 0.0, 1, "#1");
			AddLabelParameter(RevitParamManager.LblFormatInfoIdx , 0, ParamDataType.TEXT           , "#,###"       , 0.0, 0, "#1");
			// AddLabelParameter(RevitParamManager.LblAsLengthIdx   , 0, ParamDataType.IGNORE         , "A11.3"       , 0.0, 0, "#1");
			// AddLabelParameter(RevitParamManager.LblAsNumberIdx   , 0, ParamDataType.IGNORE         , "A11.4"       , 0.0, 0, "#1");
			// AddLabelParameter(RevitParamManager.LblAsYesNoIdx    , 0, ParamDataType.IGNORE         , "A11.5"       , 0.0, 0, "#1");

			AddLabelParameter(RevitParamManager.LblLabelIdx         , 0, ParamDataType.TEXT, "", 0.0, 0, "#2", true);

			// AddParameter(RevitParamManager.LblRelAddrIdx    , 0, ParamDataType.RELATIVEADDRESS, "(1,1)"   , 0.0, 0, "#2");
			AddLabelParameter(RevitParamManager.LblNameIdx       , 0, ParamDataType.TEXT           , "Myname1-2 "  , 0.0, 0, "#2");
			AddLabelParameter(RevitParamManager.LblDataTypeIdx   , 0, ParamDataType.DATATYPE       , "text"        , 0.0, 0, "#2");
			AddLabelParameter(RevitParamManager.LblFormulaIdx    , 0, ParamDataType.FORMULA        , "=[A2]"       , 0.0, 0, "#2");
			AddLabelParameter(RevitParamManager.LblFormatInfoIdx , 0, ParamDataType.TEXT           , "#,###"       , 0.0, 0, "#2");
			AddLabelParameter(RevitParamManager.LblIgnoreIdx     , 0, ParamDataType.BOOL           , null          , 0.0, 1, "#2");
			// AddLabelParameter(RevitParamManager.LblAsLengthIdx   , 0, ParamDataType.IGNORE         , "A12.3"       , 0.0, 0, "#2");
			// AddLabelParameter(RevitParamManager.LblAsNumberIdx   , 0, ParamDataType.IGNORE         , "A12.4"       , 0.0, 0, "#2");
			// AddLabelParameter(RevitParamManager.LblAsYesNoIdx    , 0, ParamDataType.IGNORE         , "A12.5"       , 0.0, 0, "#2");

		}


		private void AddSymbol2()
		{
			// int adj1 = RevitParamManager.ParamCounts[(int) ParamGroup.DATA];
			// int 0 = adj1 + RevitParamManager.ParamCounts[(int) ParamGroup.CONTAINER];

			AddInstParameter(RevitParamManager.NameIdx          , 0, ParamDataType.TEXT, "Myname2");
			AddInstParameter(RevitParamManager.SeqIdx           , 0, ParamDataType.TEXT, "1");
			// AddParameter(RevitParamManager.CellAddrIdx      , 0, ParamDataType.TEXT, "@A4");
			AddInstParameter(RevitParamManager.HasErrorsIdx     , 0, ParamDataType.IGNORE, "", 0.0, 1);

			// AddParameter(RevitParamManager.GraphicType      , 0, ParamDataType.IGNORE, "graphic type");
			// AddParameter(RevitParamManager.DataVisibleIdx   , 0, ParamDataType.IGNORE, "", 0.0, 1);

			// AddParameter(RevitParamManager.LabelsIdx        , 0, ParamDataType.EMPTY, "", 0.0, 0, "All Labels");

			AddLabelParameter(RevitParamManager.LblLabelIdx         , 0, ParamDataType.TEXT, "", 0.0, 0, "#1", true);

			// AddParameter(RevitParamManager.LblRelAddrIdx    , 0, ParamDataType.RELATIVEADDRESS, "(1,2)"   , 0.0, 0, "#1");
			AddLabelParameter(RevitParamManager.LblNameIdx       , 0, ParamDataType.TEXT           , "Myname2-1 "  , 0.0, 0, "#1");
			AddLabelParameter(RevitParamManager.LblDataTypeIdx   , 0, ParamDataType.DATATYPE       , "length"  , 0.0, 0, "#1");
			AddLabelParameter(RevitParamManager.LblFormulaIdx    , 0, ParamDataType.FORMULA        , "=E+F"    , 0.0, 0, "#1");
			AddLabelParameter(RevitParamManager.LblFormatInfoIdx , 0, ParamDataType.TEXT           , "#,##0"   , 0.0, 0, "#1");
			AddLabelParameter(RevitParamManager.LblIgnoreIdx     , 0, ParamDataType.BOOL           , null      , 0.0, 1, "#1");
			// AddLabelParameter(RevitParamManager.LblAsLengthIdx   , 0, ParamDataType.IGNORE         , "A2.3"    , 0.0, 0, "#1");
			// AddLabelParameter(RevitParamManager.LblAsNumberIdx   , 0, ParamDataType.IGNORE         , "A2.4"    , 0.0, 0, "#1");
			// AddLabelParameter(RevitParamManager.LblAsYesNoIdx    , 0, ParamDataType.IGNORE         , "A2.5"    , 0.0, 0, "#1");

			AddLabelParameter(RevitParamManager.LblLabelIdx       , 0, ParamDataType.ERROR, "", 0.0, 0, "end of list");
		}


		private void AddSymbol3()
		{
			// int adj1 = RevitParamManager.ParamCounts[(int) ParamGroup.DATA];
			// int 0 = adj1 + RevitParamManager.ParamCounts[(int) ParamGroup.CONTAINER];

			AddInstParameter(RevitParamManager.NameIdx          , 0, ParamDataType.TEXT, "Myname3");
			AddInstParameter(RevitParamManager.SeqIdx           , 0, ParamDataType.TEXT, "1");
			// AddParameter(RevitParamManager.CellAddrIdx      , 0, ParamDataType.TEXT, "@A6");
			AddInstParameter(RevitParamManager.HasErrorsIdx     , 0, ParamDataType.IGNORE, "", 0.0, 1);

			// AddParameter(RevitParamManager.GraphicType      , 0, ParamDataType.IGNORE, "graphic type");
			// AddParameter(RevitParamManager.DataVisibleIdx   , 0, ParamDataType.IGNORE, "", 0.0, 1);

			// AddParameter(RevitParamManager.LabelsIdx        , 0, ParamDataType.EMPTY, "", 0.0, 0, "All Labels");

			AddLabelParameter(RevitParamManager.LblLabelIdx         , 0, ParamDataType.TEXT, "", 0.0, 0, "#1", true);

			// AddParameter(RevitParamManager.LblRelAddrIdx    , 0, ParamDataType.RELATIVEADDRESS, "(1,3)"   , 0.0, 0, "#1");
			AddLabelParameter(RevitParamManager.LblNameIdx       , 0, ParamDataType.TEXT           , "Myname3-1 "  , 0.0, 0, "#1");
			AddLabelParameter(RevitParamManager.LblDataTypeIdx   , 0, ParamDataType.DATATYPE       , "length"  , 0.0, 0, "#1");
			AddLabelParameter(RevitParamManager.LblFormulaIdx    , 0, ParamDataType.FORMULA        , "=G+H"    , 0.0, 0, "#1");
			AddLabelParameter(RevitParamManager.LblIgnoreIdx     , 0, ParamDataType.BOOL           , null      , 0.0, 1, "#1");
			// AddLabelParameter(RevitParamManager.LblAsLengthIdx   , 0, ParamDataType.IGNORE         , "A3.3"    , 0.0, 0, "#1");
			// AddLabelParameter(RevitParamManager.LblAsNumberIdx   , 0, ParamDataType.IGNORE         , "A3.4"    , 0.0, 0, "#1");
			// AddLabelParameter(RevitParamManager.LblAsYesNoIdx    , 0, ParamDataType.IGNORE         , "A3.5"    , 0.0, 0, "#1");

			AddLabelParameter(RevitParamManager.LblLabelIdx         , 0, ParamDataType.ERROR, "", 0.0, 0, "end of list");
		}

		

		private void AddSymbol4()
		{
			// int adj1 = RevitParamManager.ParamCounts[(int) ParamGroup.DATA];
			// int 0 = adj1 + RevitParamManager.ParamCounts[(int) ParamGroup.CONTAINER];

			AddInstParameter(RevitParamManager.NameIdx          , 0, ParamDataType.TEXT, "Myname4");
			AddInstParameter(RevitParamManager.SeqIdx           , 0, ParamDataType.TEXT, "1");
			// AddParameter(RevitParamManager.CellAddrIdx      , 0, ParamDataType.TEXT, "@A4");
			AddInstParameter(RevitParamManager.HasErrorsIdx     , 0, ParamDataType.IGNORE, "", 0.0, 1);

			// AddParameter(RevitParamManager.GraphicType      , 0, ParamDataType.IGNORE, "graphic type");
			// AddParameter(RevitParamManager.DataVisibleIdx   , 0, ParamDataType.IGNORE, "", 0.0, 1);

			// AddParameter(RevitParamManager.LabelsIdx        , 0, ParamDataType.EMPTY, "", 0.0, 0, "All Labels");

			AddLabelParameter(RevitParamManager.LblLabelIdx         , 0, ParamDataType.TEXT, "", 0.0, 0, "#1", true);

			// AddParameter(RevitParamManager.LblRelAddrIdx    , 0, ParamDataType.RELATIVEADDRESS, "(1,2)"   , 0.0, 0, "#1");
			AddLabelParameter(RevitParamManager.LblNameIdx       , 0, ParamDataType.TEXT           , "Myname4-1 "  , 0.0, 0, "#1");
			AddLabelParameter(RevitParamManager.LblDataTypeIdx   , 0, ParamDataType.DATATYPE       , "length"  , 0.0, 0, "#1");
			AddLabelParameter(RevitParamManager.LblFormulaIdx    , 0, ParamDataType.FORMULA        , "=E+F"    , 0.0, 0, "#1");
			AddLabelParameter(RevitParamManager.LblIgnoreIdx     , 0, ParamDataType.BOOL           , null      , 0.0, 1, "#1");
			// AddLabelParameter(RevitParamManager.LblAsLengthIdx   , 0, ParamDataType.IGNORE         , "A2.3"    , 0.0, 0, "#1");
			// AddLabelParameter(RevitParamManager.LblAsNumberIdx   , 0, ParamDataType.IGNORE         , "A2.4"    , 0.0, 0, "#1");
			// AddLabelParameter(RevitParamManager.LblAsYesNoIdx    , 0, ParamDataType.IGNORE         , "A2.5"    , 0.0, 0, "#1");

			AddLabelParameter(RevitParamManager.LblLabelIdx       , 0, ParamDataType.ERROR, "", 0.0, 0, "end of list");
		}


		public void AddChart( int index, int adjIdx,
			ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName = RevitParamManager.ChartInstParam(index).ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, type, strVal, dblVal, intVal);
			Charts[symbolIdx].parameters.Add(p);
		}

		private void AddChart1()
		{

			AddChart(RevitParamManager.NameIdx             , 0, ParamDataType.TEXT, "Myname1");
			// AddChart(RevitParamManager.ChartDescIdx        , 0, ParamDataType.TEXT, "Description 1");
			AddChart(RevitParamManager.SeqIdx              , 0, ParamDataType.TEXT, "1");
			AddChart(RevitParamManager.ChartFilePathIdx    , 0, ParamDataType.TEXT, @".\CsSampleChart_01_02.xlsx");
			AddChart(RevitParamManager.ChartWorkSheetIdx   , 0, ParamDataType.TEXT, "CsSheet 1");
			AddChart(RevitParamManager.ChartFamilyNameIdx  , 0, ParamDataType.TEXT, "CsCellFamily01");
			AddChart(RevitParamManager.ChartUpdateTypeIdx  , 0, ParamDataType.UPDATE_TYPE, "Alyways");
			AddChart(RevitParamManager.ChartHasErrorsIdx   , 0, ParamDataType.TEXT, "");
		}

		private void AddChart2()
		{

			AddChart(RevitParamManager.NameIdx                , 0, ParamDataType.TEXT, "Myname2");
			// AddChart(RevitParamManager.ChartDescIdx        , 0, ParamDataType.TEXT, "Description 2");
			AddChart(RevitParamManager.SeqIdx                 , 0, ParamDataType.TEXT, "2");
			AddChart(RevitParamManager.ChartFilePathIdx       , 0, ParamDataType.TEXT, @".\CsSampleChart_01_02.xlsx");
			AddChart(RevitParamManager.ChartWorkSheetIdx      , 0, ParamDataType.TEXT, "CsSheet 2");
			AddChart(RevitParamManager.ChartFamilyNameIdx     , 0, ParamDataType.TEXT, "CsCellFamily02");
			AddChart(RevitParamManager.ChartUpdateTypeIdx     , 0, ParamDataType.UPDATE_TYPE, "Alyways");
			AddChart(RevitParamManager.ChartHasErrorsIdx      , 0, ParamDataType.TEXT, "");
		}

		private void AddChart3()
		{

			AddChart(RevitParamManager.NameIdx                , 0, ParamDataType.TEXT, "Myname3");
			// AddChart(RevitParamManager.ChartDescIdx        , 0, ParamDataType.TEXT, "Description 3");
			AddChart(RevitParamManager.SeqIdx                 , 0, ParamDataType.TEXT, "3");
			AddChart(RevitParamManager.ChartFilePathIdx       , 0, ParamDataType.TEXT, @".\CsSampleChart_03_04.xlsx");
			AddChart(RevitParamManager.ChartWorkSheetIdx      , 0, ParamDataType.TEXT, "CsSheet 1");
			AddChart(RevitParamManager.ChartFamilyNameIdx     , 0, ParamDataType.TEXT, "CsCellFamily03");
			AddChart(RevitParamManager.ChartUpdateTypeIdx     , 0, ParamDataType.UPDATE_TYPE, "Alyways");
			AddChart(RevitParamManager.ChartHasErrorsIdx      , 0, ParamDataType.TEXT, "");
		}
		
		private void AddChart4()
		{

			AddChart(RevitParamManager.NameIdx                , 0, ParamDataType.TEXT, "Myname4");
			// AddChart(RevitParamManager.ChartDescIdx        , 0, ParamDataType.TEXT, "Description 4 (not found worksheet)");
			AddChart(RevitParamManager.SeqIdx                 , 0, ParamDataType.TEXT, "4");
			AddChart(RevitParamManager.ChartFilePathIdx       , 0, ParamDataType.TEXT, @".\CsSampleChart_03_04.xlsx");
			AddChart(RevitParamManager.ChartWorkSheetIdx      , 0, ParamDataType.TEXT, "CsSheet 2");
			AddChart(RevitParamManager.ChartFamilyNameIdx     , 0, ParamDataType.TEXT, "CsCellFamily04");
			AddChart(RevitParamManager.ChartUpdateTypeIdx     , 0, ParamDataType.UPDATE_TYPE, "Alyways");
			AddChart(RevitParamManager.ChartHasErrorsIdx      , 0, ParamDataType.TEXT, "");
		}
		
		private void AddChart5()
		{

			AddChart(RevitParamManager.NameIdx                , 0, ParamDataType.TEXT, "Myname5");
			// AddChart(RevitParamManager.ChartDescIdx        , 0, ParamDataType.TEXT, "Description 5 (not found chart)");
			AddChart(RevitParamManager.SeqIdx                 , 0, ParamDataType.TEXT, "5");
			AddChart(RevitParamManager.ChartFilePathIdx       , 0, ParamDataType.TEXT, @".\CsSampleChart_04.xlsx");
			AddChart(RevitParamManager.ChartWorkSheetIdx      , 0, ParamDataType.TEXT, "CsSheet 1");
			AddChart(RevitParamManager.ChartFamilyNameIdx     , 0, ParamDataType.TEXT, "CsCellFamily05");
			AddChart(RevitParamManager.ChartUpdateTypeIdx     , 0, ParamDataType.UPDATE_TYPE, "Alyways");
			AddChart(RevitParamManager.ChartHasErrorsIdx      , 0, ParamDataType.TEXT, "");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	}
}