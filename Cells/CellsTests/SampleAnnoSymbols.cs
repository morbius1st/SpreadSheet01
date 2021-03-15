// Solution:     SpreadSheet01
// Project:       Cells
// File:             SampleAnnoSymbols.cs
// Created:      2021-03-03 (10:13 PM)

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;

using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitParamInfo;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.RevitSupport.RevitCellsManagement.RevitParamManager;

using UtilityLibrary;

namespace Cells.CellsTests
{
	public class SampleAnnoSymbols : INotifyPropertyChanged
	{
		public ICollection<Element> ChartElements { get; set; }
		public ICollection<Element>[] CellElements { get; set; }

		public AnnotationSymbol[] Charts { get; set; }

		public AnnotationSymbol[] Symbols { get; set; }

		// public RevitParamManager rcp = new RevitParamManager();
		// public RevitParamManager rcpx = new RevitParamManager();

		private int symbolIdx = 0;
		private int chartIdx = 0;

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

			AddChart0();
			el = (Element) Charts[symbolIdx];
			ChartElements.Add(el);

			symbolIdx = 1;
			Charts[symbolIdx] = new AnnotationSymbol();
			Charts[symbolIdx].parameters = new List<Parameter>();
			Charts[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			AddChart1();
			el = (Element) Charts[symbolIdx];
			ChartElements.Add(el);

			symbolIdx = 2;
			Charts[symbolIdx] = new AnnotationSymbol();
			Charts[symbolIdx].parameters = new List<Parameter>();
			Charts[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			AddChart2();
			el = (Element) Charts[symbolIdx];
			ChartElements.Add(el);

			symbolIdx = 3;
			Charts[symbolIdx] = new AnnotationSymbol();
			Charts[symbolIdx].parameters = new List<Parameter>();
			Charts[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			AddChart3();
			el = (Element) Charts[symbolIdx];
			ChartElements.Add(el);

			symbolIdx = 4;
			Charts[symbolIdx] = new AnnotationSymbol();
			Charts[symbolIdx].parameters = new List<Parameter>();
			Charts[symbolIdx].Name = "Chart| " + symbolIdx.ToString("D2");
			AddChart4();
			el = (Element) Charts[symbolIdx];
			ChartElements.Add(el);
		}

		//
		// private void makeAnnoSyms()
		// {
		// 	Symbols = new AnnotationSymbol[4];
		// 	Element el;
		// 	CellElements = new List<Element>();
		//
		//
		// 	symbolIdx = 0;
		// 	Symbols[symbolIdx] = new AnnotationSymbol();
		// 	Symbols[symbolIdx].parameters = new List<Parameter>();
		// 	Symbols[symbolIdx].Name = "Symbol| " + symbolIdx.ToString("D2");
		// 	AddSymbol0();
		// 	el = (Element) Symbols[symbolIdx];
		// 	CellElements.Add(el);
		//
		// 	++symbolIdx;
		// 	Symbols[symbolIdx] = new AnnotationSymbol();
		// 	Symbols[symbolIdx].parameters = new List<Parameter>();
		// 	Symbols[symbolIdx].Name = "Symbol| " + symbolIdx.ToString("D2");
		// 	AddSymbol1();
		// 	el = (Element) Symbols[symbolIdx];
		// 	CellElements.Add(el);
		//
		// 	++symbolIdx;
		// 	Symbols[symbolIdx] = new AnnotationSymbol();
		// 	Symbols[symbolIdx].parameters = new List<Parameter>();
		// 	Symbols[symbolIdx].Name = "Symbol| " + symbolIdx.ToString("D2");
		// 	AddSymbol2();
		// 	el = (Element) Symbols[symbolIdx];
		// 	CellElements.Add(el);
		//
		// 	++symbolIdx;
		// 	Symbols[symbolIdx] = new AnnotationSymbol();
		// 	Symbols[symbolIdx].parameters = new List<Parameter>();
		// 	Symbols[symbolIdx].Name = "Symbol| " + symbolIdx.ToString("D2");
		// 	AddSymbol3();
		// 	el = (Element) Symbols[symbolIdx];
		// 	CellElements.Add(el);
		// }

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

			Symbols[idx] = new AnnotationSymbol();
			Symbols[idx].parameters = new List<Parameter>();
			Symbols[idx].Name = "Symbol| " + ix.ToString("D2") + idx.ToString("D2") ;

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


		public void AddInstParameter( int index, int adjIdx,
			ParamDataType type, string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName = CellInstParam(index).ParameterName;
			// string paramName = RevitParamManager.CellAllParams[index + adjIdx].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, type, strVal, dblVal, intVal);
			Symbols[symbolIdx].parameters.Add(p);
		}

		public void AddLabelParameter( int index, int adjIdx,
			ParamDataType type,
			string strVal,
			string name = null,  double dblVal = 0.0, int intVal = 0, bool isMainLabel = false,
			bool boolVal = false)
		{
			string paramName = CellLabelParam(index).ParameterName;

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

		private void AddSymbol0()
		{
			// int adj1 = RevitParamManager.ParamCounts[(int) ParamGroup.DATA];
			// int 0 = adj1 + RevitParamManager.ParamCounts[(int) ParamGroup.CONTAINER];
			// int adj3 = 0 + RevitParamManager.ParamCounts[(int) ParamGroup.LABEL_GRP];
			// int adj4 = adj3 + RevitParamManager.ParamCounts[(int) ParamGroup.LABEL_GRP];


			AddInstParameter(SeqIdx           , 0, ParamDataType.TEXT, "0");
			AddInstParameter(NameIdx          , 0, ParamDataType.TEXT, "MyCellname1"+ chartIdx.ToString("D2"));
			AddInstParameter(Descdx           , 0, ParamDataType.TEXT, "Description 0");
			
			AddInstParameter(HasErrorsIdx     , 0, ParamDataType.IGNORE, "", 0.0, 1);

			// AddParameter(RevitParamManager.GraphicType      , 0, ParamDataType.IGNORE, "graphic type");
			// AddParameter(RevitParamManager.DataVisibleIdx   , 0, ParamDataType.IGNORE, "", 0.0, 1);

			// AddParameter(RevitParamManager.LabelsIdx        , 0, ParamDataType.EMPTY, "", 0.0, 0, "All Labels");

			AddLabelParameter(LblLabelIdx         , 0, ParamDataType.TEXT        , "", "#1", 0.0, 0, true);

			// AddParameter(RevitParamManager.LblRelAddrIdx    , 0, ParamDataType.RELATIVEADDRESS, "(1,1)"   , 0.0, 0, "#1");
			AddLabelParameter(LblDataTypeIdx   , 0, ParamDataType.DATATYPE       , "length"			 , "#1");
			AddLabelParameter(LblNameIdx       , 0, ParamDataType.TEXT           , "MyLabelname 0-1 "+ chartIdx.ToString("D2")   , "#1");
			AddLabelParameter(LblFormulaIdx    , 0, ParamDataType.FORMULA        , "={[A1]}"			 , "#1");
			AddLabelParameter(LblIgnoreIdx     , 0, ParamDataType.BOOL           , null				 , "#1", 0.0, 1);
			AddLabelParameter(LblFormatInfoIdx , 0, ParamDataType.TEXT           , "#,###"			 , "#1");
			// AddLabelParameter(RevitParamManager.LblAsLengthIdx   , 0, ParamDataType.IGNORE         , "A11.3"       , 0.0, 0, "#1");
			// AddLabelParameter(RevitParamManager.LblAsNumberIdx   , 0, ParamDataType.IGNORE         , "A11.4"       , 0.0, 0, "#1");
			// AddLabelParameter(RevitParamManager.LblAsYesNoIdx    , 0, ParamDataType.IGNORE         , "A11.5"       , 0.0, 0, "#1");

			AddLabelParameter(LblLabelIdx         , 0, ParamDataType.TEXT, "", "#2", 0.0, 0, true);

			// AddParameter(RevitParamManager.LblRelAddrIdx    , 0, ParamDataType.RELATIVEADDRESS, "(1,1)"   , 0.0, 0, "#2");
			AddLabelParameter(LblNameIdx       , 0, ParamDataType.TEXT           , "MyLabelname 0-2 "+ chartIdx.ToString("D2")   , "#2");
			AddLabelParameter(LblDataTypeIdx   , 0, ParamDataType.DATATYPE       , "text"				 , "#2");
			AddLabelParameter(LblFormulaIdx    , 0, ParamDataType.FORMULA        , "={[A2]}"			 , "#2");
			AddLabelParameter(LblFormatInfoIdx , 0, ParamDataType.TEXT           , "#,###"			 , "#2");
			AddLabelParameter(LblIgnoreIdx     , 0, ParamDataType.BOOL           , null				 , "#2", 0.0, 1);
			// AddLabelParameter(RevitParamManager.LblAsLengthIdx   , 0, ParamDataType.IGNORE         , "A12.3"       , 0.0, 0, "#2");
			// AddLabelParameter(RevitParamManager.LblAsNumberIdx   , 0, ParamDataType.IGNORE         , "A12.4"       , 0.0, 0, "#2");
			// AddLabelParameter(RevitParamManager.LblAsYesNoIdx    , 0, ParamDataType.IGNORE         , "A12.5"       , 0.0, 0, "#2");
		}


		private void AddSymbol1()
		{
			// int adj1 = RevitParamManager.ParamCounts[(int) ParamGroup.DATA];
			// int 0 = adj1 + RevitParamManager.ParamCounts[(int) ParamGroup.CONTAINER];

			AddInstParameter(SeqIdx           , 0, ParamDataType.TEXT, "1");
			AddInstParameter(NameIdx          , 0, ParamDataType.TEXT, "MyCellname2"+ chartIdx.ToString("D2"));
			// AddParameter(RevitParamManager.CellAddrIdx      , 0, ParamDataType.TEXT, "@A4");
			AddInstParameter(HasErrorsIdx     , 0, ParamDataType.IGNORE, "", 0.0, 1);

			// AddParameter(RevitParamManager.GraphicType      , 0, ParamDataType.IGNORE, "graphic type");
			// AddParameter(RevitParamManager.DataVisibleIdx   , 0, ParamDataType.IGNORE, "", 0.0, 1);

			// AddParameter(RevitParamManager.LabelsIdx        , 0, ParamDataType.EMPTY, "", 0.0, 0, "All Labels");

			AddLabelParameter(LblLabelIdx         , 0, ParamDataType.TEXT        , "", "#1", 0.0, 0, true);

			// AddParameter(RevitParamManager.LblRelAddrIdx    , 0, ParamDataType.RELATIVEADDRESS, "(1,2)"   , 0.0, 0, "#1");
			AddLabelParameter(LblDataTypeIdx   , 0, ParamDataType.DATATYPE       , "length"			 , "#1");
			AddLabelParameter(LblNameIdx       , 0, ParamDataType.TEXT           , "MyLabelname 1-1 "+ chartIdx.ToString("D2")   , "#1");
			AddLabelParameter(LblFormulaIdx    , 0, ParamDataType.FORMULA        , "={#SheetName}"    , "#1");
			AddLabelParameter(LblFormatInfoIdx , 0, ParamDataType.TEXT           , "#,##0"			 , "#1");
			AddLabelParameter(LblIgnoreIdx     , 0, ParamDataType.BOOL           , null				 , "#1", 0.0, 1);
			// AddLabelParameter(RevitParamManager.LblAsLengthIdx   , 0, ParamDataType.IGNORE         , "A2.3"    , 0.0, 0, "#1");
			// AddLabelParameter(RevitParamManager.LblAsNumberIdx   , 0, ParamDataType.IGNORE         , "A2.4"    , 0.0, 0, "#1");
			// AddLabelParameter(RevitParamManager.LblAsYesNoIdx    , 0, ParamDataType.IGNORE         , "A2.5"    , 0.0, 0, "#1");

			// AddLabelParameter(RevitParamManager.LblLabelIdx       , 0, ParamDataType.ERROR, "", "end of list");
		}


		private void AddSymbol2()
		{
			// int adj1 = RevitParamManager.ParamCounts[(int) ParamGroup.DATA];
			// int 0 = adj1 + RevitParamManager.ParamCounts[(int) ParamGroup.CONTAINER];

			AddInstParameter(SeqIdx           , 0, ParamDataType.TEXT, "2");
			AddInstParameter(NameIdx          , 0, ParamDataType.TEXT, "MyCellname3"+ chartIdx.ToString("D2"));
			// AddParameter(RevitParamManager.CellAddrIdx      , 0, ParamDataType.TEXT, "@A6");
			AddInstParameter(HasErrorsIdx     , 0, ParamDataType.IGNORE, "", 0.0, 1);

			// AddParameter(RevitParamManager.GraphicType      , 0, ParamDataType.IGNORE, "graphic type");
			// AddParameter(RevitParamManager.DataVisibleIdx   , 0, ParamDataType.IGNORE, "", 0.0, 1);

			// AddParameter(RevitParamManager.LabelsIdx        , 0, ParamDataType.EMPTY, "", 0.0, 0, "All Labels");

			AddLabelParameter(LblLabelIdx         , 0, ParamDataType.TEXT        , "", "#1", 0.0, 0, true);

			// AddParameter(RevitParamManager.LblRelAddrIdx    , 0, ParamDataType.RELATIVEADDRESS, "(1,3)"   , 0.0, 0, "#1");
			AddLabelParameter(LblDataTypeIdx   , 0, ParamDataType.DATATYPE       , "length"           , "#1");
			AddLabelParameter(LblNameIdx       , 0, ParamDataType.TEXT           , "MyLabelname 2-1 "+ chartIdx.ToString("D2")  , "#1");
			AddLabelParameter(LblFormulaIdx    , 0, ParamDataType.FORMULA        , "={$Date(yyyy-mm-dd)}" , "#1");
			AddLabelParameter(LblIgnoreIdx     , 0, ParamDataType.BOOL           , null               , "#1", 0.0, 1);
			// AddLabelParameter(RevitParamManager.LblAsLengthIdx   , 0, ParamDataType.IGNORE         , "A3.3"    , 0.0, 0, "#1");
			// AddLabelParameter(RevitParamManager.LblAsNumberIdx   , 0, ParamDataType.IGNORE         , "A3.4"    , 0.0, 0, "#1");
			// AddLabelParameter(RevitParamManager.LblAsYesNoIdx    , 0, ParamDataType.IGNORE         , "A3.5"    , 0.0, 0, "#1");

			// AddLabelParameter(RevitParamManager.LblLabelIdx         , 0, ParamDataType.ERROR, "", "end of list");
		}


		private void AddSymbol3()
		{
			// int adj1 = RevitParamManager.ParamCounts[(int) ParamGroup.DATA];
			// int 0 = adj1 + RevitParamManager.ParamCounts[(int) ParamGroup.CONTAINER];

			AddInstParameter(SeqIdx           , 0, ParamDataType.TEXT, "3");
			AddInstParameter(NameIdx          , 0, ParamDataType.TEXT, "MyCellname4"+ chartIdx.ToString("D2"));
			// AddParameter(RevitParamManager.CellAddrIdx      , 0, ParamDataType.TEXT, "@A4");
			AddInstParameter(HasErrorsIdx     , 0, ParamDataType.IGNORE, "", 0.0, 1);

			// AddParameter(RevitParamManager.GraphicType      , 0, ParamDataType.IGNORE, "graphic type");
			// AddParameter(RevitParamManager.DataVisibleIdx   , 0, ParamDataType.IGNORE, "", 0.0, 1);

			// AddParameter(RevitParamManager.LabelsIdx        , 0, ParamDataType.EMPTY, "", 0.0, 0, "All Labels");

			AddLabelParameter(LblLabelIdx         , 0, ParamDataType.TEXT        , "", "#1", 0.0, 0, true);

			// AddParameter(RevitParamManager.LblRelAddrIdx    , 0, ParamDataType.RELATIVEADDRESS, "(1,2)"   , 0.0, 0, "#1");
			AddLabelParameter(LblDataTypeIdx   , 0, ParamDataType.DATATYPE       , "length"	        , "#1");
			AddLabelParameter(LblFormulaIdx    , 0, ParamDataType.FORMULA        , "={[A5]}"	        , "#1");
			AddLabelParameter(LblNameIdx       , 0, ParamDataType.TEXT           , "MyLabelname 3-1 "+ chartIdx.ToString("D2")  , "#1");
			AddLabelParameter(LblIgnoreIdx     , 0, ParamDataType.BOOL           , null		        , "#1", 0.0, 1);
			// AddLabelParameter(RevitParamManager.LblAsLengthIdx   , 0, ParamDataType.IGNORE         , "A2.3"    , 0.0, 0, "#1");
			// AddLabelParameter(RevitParamManager.LblAsNumberIdx   , 0, ParamDataType.IGNORE         , "A2.4"    , 0.0, 0, "#1");
			// AddLabelParameter(RevitParamManager.LblAsYesNoIdx    , 0, ParamDataType.IGNORE         , "A2.5"    , 0.0, 0, "#1");

			// AddLabelParameter(RevitParamManager.LblLabelIdx       , 0, ParamDataType.ERROR, "", "end of list");
		}


		public void AddChart( int index, int adjIdx,
			ParamDataType type,
			string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null)
		{
			string paramName = ChartInstParam(index).ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, type, strVal, dblVal, intVal);
			Charts[symbolIdx].parameters.Add(p);
		}

		private void AddChart0()
		{
			AddChart(SeqIdx              , 0, ParamDataType.TEXT, "0");
			AddChart(NameIdx             , 0, ParamDataType.TEXT, "MyChartname0");
			AddChart(Descdx              , 0, ParamDataType.TEXT, "Description 0");
			AddChart(ChartFilePathIdx    , 0, ParamDataType.TEXT, @".\CsSampleChart_01_02.xlsx");
			AddChart(ChartWorkSheetIdx   , 0, ParamDataType.TEXT, "CsSheet 1");
			AddChart(ChartFamilyNameIdx  , 0, ParamDataType.TEXT, "CsCellFamily01");
			AddChart(ChartUpdateTypeIdx  , 0, ParamDataType.UPDATE_TYPE, "Alyways");
			AddChart(ChartHasErrorsIdx   , 0, ParamDataType.TEXT, "");
		}

		private void AddChart1()
		{
			AddChart(SeqIdx                 , 0, ParamDataType.TEXT, "1");
			AddChart(ChartFilePathIdx       , 0, ParamDataType.TEXT, @".\CsSampleChart_01_02.xlsx");
			AddChart(NameIdx                , 0, ParamDataType.TEXT, "MyChartname1");
			AddChart(ChartWorkSheetIdx      , 0, ParamDataType.TEXT, "CsSheet 2");
			AddChart(ChartFamilyNameIdx     , 0, ParamDataType.TEXT, "CsCellFamily02");
			AddChart(ChartUpdateTypeIdx     , 0, ParamDataType.UPDATE_TYPE, "Alyways");
			AddChart(ChartHasErrorsIdx      , 0, ParamDataType.TEXT, "");
		}

		private void AddChart2()
		{
			AddChart(SeqIdx                 , 0, ParamDataType.TEXT, "2");
			AddChart(ChartFilePathIdx       , 0, ParamDataType.TEXT, @".\CsSampleChart_03_04.xlsx");
			AddChart(ChartWorkSheetIdx      , 0, ParamDataType.TEXT, "CsSheet 1");
			AddChart(NameIdx                , 0, ParamDataType.TEXT, "MyChartname2");
			AddChart(ChartFamilyNameIdx     , 0, ParamDataType.TEXT, "CsCellFamily03");
			AddChart(ChartUpdateTypeIdx     , 0, ParamDataType.UPDATE_TYPE, "Alyways");
			AddChart(ChartHasErrorsIdx      , 0, ParamDataType.TEXT, "");
		}

		private void AddChart3()
		{
			AddChart(NameIdx                , 0, ParamDataType.TEXT, "MyChartname3");
			// AddChart(RevitParamManager.ChartDescIdx        , 0, ParamDataType.TEXT, "Description 4 (not found worksheet)");
			AddChart(SeqIdx                 , 0, ParamDataType.TEXT, "3");
			AddChart(ChartFilePathIdx       , 0, ParamDataType.TEXT, @".\CsSampleChart_03_04.xlsx");
			AddChart(ChartWorkSheetIdx      , 0, ParamDataType.TEXT, "CsSheet 2");
			AddChart(ChartFamilyNameIdx     , 0, ParamDataType.TEXT, "CsCellFamily04");
			AddChart(ChartUpdateTypeIdx     , 0, ParamDataType.UPDATE_TYPE, "Alyways");
			AddChart(ChartHasErrorsIdx      , 0, ParamDataType.TEXT, "");
		}

		private void AddChart4()
		{
			AddChart(NameIdx                , 0, ParamDataType.TEXT, "MyChartname4");
			// AddChart(RevitParamManager.ChartDescIdx        , 0, ParamDataType.TEXT, "Description 5 (not found chart)");
			AddChart(SeqIdx                 , 0, ParamDataType.TEXT, "4");
			AddChart(ChartFilePathIdx       , 0, ParamDataType.TEXT, @".\CsSampleChart_04.xlsx");
			AddChart(ChartWorkSheetIdx      , 0, ParamDataType.TEXT, "CsSheet 1");
			AddChart(ChartFamilyNameIdx     , 0, ParamDataType.TEXT, "CsCellFamily05");
			AddChart(ChartUpdateTypeIdx     , 0, ParamDataType.UPDATE_TYPE, "Alyways");
			AddChart(ChartHasErrorsIdx      , 0, ParamDataType.TEXT, "");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}