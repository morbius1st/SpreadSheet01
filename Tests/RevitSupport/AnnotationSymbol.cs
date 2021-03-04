

#region + Using Directives
#endregion

// user name: jeffs
// created:   2/23/2021 6:39:44 PM

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using SpreadSheet01.RevitSupport;
using SpreadSheet01.RevitSupport.RevitParamValue;
using UtilityLibrary;

using static SpreadSheet01.RevitSupport.RevitParamInfo.RevitCellParameters;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamDataType;

namespace Autodesk.Revit.DB
{
	public class Element
	{

	}

	public class SampleAnnoSymbols
	{
		public AnnotationSymbol[] Symbols {get; set; }

		private int symbolIdx = 0;

		public void Process()
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
			string paramName = CellAllParams[index + adjIdx].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, type, strVal, dblVal, intVal);
			Symbols[symbolIdx].parameters.Add(p);
		}

		private void AddSymbol1()
		{
			int adj1 = ParamCounts[(int) ParamGroup.DATA];
			int adj2 = adj1 + ParamCounts[(int) ParamGroup.CONTAINER];
			int adj3 = adj2 + ParamCounts[(int) ParamGroup.LABEL];
			int adj4 = adj3 + ParamCounts[(int) ParamGroup.LABEL];


			AddParameter(NameIdx          , 0, TEXT, "Myname1");
			AddParameter(SeqIdx           , 0, TEXT, "1");
			AddParameter(CellAddrIdx      , 0, TEXT, "@A2");
			AddParameter(FormattingInfoIdx, 0, TEXT, "format info");
			AddParameter(GraphicType      , 0, IGNORE, "graphic type");
			AddParameter(DataIsToCellIdx  , 0, BOOL  , "", 0.0, 1);
			AddParameter(HasErrorsIdx     , 0, IGNORE, "", 0.0, 1);
			AddParameter(DataVisibleIdx   , 0, IGNORE, "", 0.0, 1);

			AddParameter(LabelsIdx        , 0, EMPTY, "", 0.0, 0, "All Labels");

			AddParameter(LabelIdx         , adj2, TEXT, "", 0.0, 0, "Label #1 info 1");

			AddParameter(lblRelAddrIdx    , adj2, RELATIVEADDRESS, "(1,1)"   , 0.0, 0, "#1");
			AddParameter(lblDataTypeIdx   , adj2, DATATYPE       , "length"  , 0.0, 0, "#1");
			AddParameter(lblFormulaIdx    , adj2, FORMULA        , "=A+B"    , 0.0, 0, "#1");
			AddParameter(lblIgnoreIdx     , adj2, BOOL           , null      , 0.0, 1, "#1");
			AddParameter(lblAsLengthIdx   , adj2, IGNORE         , "A11.3"   , 0.0, 0, "#1");
			AddParameter(lblAsNumberIdx   , adj2, IGNORE         , "A11.4"   , 0.0, 0, "#1");
			AddParameter(lblAsYesNoIdx    , adj2, IGNORE         , "A11.5"   , 0.0, 0, "#1");

			AddParameter(LabelIdx         , adj2, TEXT, "", 0.0, 0, "Label #2 info");

			AddParameter(lblRelAddrIdx    , adj2, RELATIVEADDRESS, "(1,1)"   , 0.0, 0, "#2");
			AddParameter(lblDataTypeIdx   , adj2, DATATYPE       , "text"    , 0.0, 0, "#2");
			AddParameter(lblFormulaIdx    , adj2, FORMULA        , "=C+D"    , 0.0, 0, "#2");
			AddParameter(lblIgnoreIdx     , adj2, BOOL           , null      , 0.0, 1, "#2");
			AddParameter(lblAsLengthIdx   , adj2, IGNORE         , "A12.3"   , 0.0, 0, "#2");
			AddParameter(lblAsNumberIdx   , adj2, IGNORE         , "A12.4"   , 0.0, 0, "#2");
			AddParameter(lblAsYesNoIdx    , adj2, IGNORE         , "A12.5"   , 0.0, 0, "#2");

		}

		private void AddSymbol2()
		{
			int adj1 = ParamCounts[(int) ParamGroup.DATA];
			int adj2 = adj1 + ParamCounts[(int) ParamGroup.CONTAINER];

			AddParameter(NameIdx          , 0, TEXT, "Myname2");
			AddParameter(SeqIdx           , 0, TEXT, "1");
			AddParameter(CellAddrIdx      , 0, TEXT, "@A4");
			AddParameter(FormattingInfoIdx, 0, TEXT, "format info");
			AddParameter(GraphicType      , 0, IGNORE, "graphic type");
			AddParameter(DataIsToCellIdx  , 0, BOOL  , "", 0.0, 1);
			AddParameter(HasErrorsIdx     , 0, IGNORE, "", 0.0, 1);
			AddParameter(DataVisibleIdx   , 0, IGNORE, "", 0.0, 1);

			AddParameter(LabelsIdx        , 0, EMPTY, "", 0.0, 0, "All Labels");

			AddParameter(LabelIdx         , adj2, TEXT, "", 0.0, 0, name: "Label #1 info 2");

			AddParameter(lblRelAddrIdx    , adj2, RELATIVEADDRESS, "(1,2)"   , 0.0, 0, "#1");
			AddParameter(lblDataTypeIdx   , adj2, DATATYPE       , "length"  , 0.0, 0, "#1");
			AddParameter(lblFormulaIdx    , adj2, FORMULA        , "=E+F"    , 0.0, 0, "#1");
			AddParameter(lblIgnoreIdx     , adj2, BOOL           , null      , 0.0, 1, "#1");
			AddParameter(lblAsLengthIdx   , adj2, IGNORE         , "A2.3"    , 0.0, 0, "#1");
			AddParameter(lblAsNumberIdx   , adj2, IGNORE         , "A2.4"    , 0.0, 0, "#1");
			AddParameter(lblAsYesNoIdx    , adj2, IGNORE         , "A2.5"    , 0.0, 0, "#1");

			AddParameter(LabelIdx         , 0, ERROR, "", 0.0, 0, "end of list");
		}

		private void AddSymbol3()
		{
			int adj1 = ParamCounts[(int) ParamGroup.DATA];
			int adj2 = adj1 + ParamCounts[(int) ParamGroup.CONTAINER];

			AddParameter(NameIdx          , 0, TEXT, "Myname3");
			AddParameter(SeqIdx           , 0, TEXT, "1");
			AddParameter(CellAddrIdx      , 0, TEXT, "@A6");
			AddParameter(FormattingInfoIdx, 0, TEXT, "format info");
			AddParameter(GraphicType      , 0, IGNORE, "graphic type");
			AddParameter(DataIsToCellIdx  , 0, BOOL  , "", 0.0, 1);
			AddParameter(HasErrorsIdx     , 0, IGNORE, "", 0.0, 1);
			AddParameter(DataVisibleIdx   , 0, IGNORE, "", 0.0, 1);

			AddParameter(LabelsIdx        , 0, EMPTY, "", 0.0, 0, "All Labels");

			AddParameter(LabelIdx         , adj2, TEXT, "", 0.0, 0, "Label #1 info 3");
			AddParameter(lblRelAddrIdx    , adj2, RELATIVEADDRESS, "(1,3)"   , 0.0, 0, "#1");
			AddParameter(lblDataTypeIdx   , adj2, DATATYPE       , "length"  , 0.0, 0, "#1");
			AddParameter(lblFormulaIdx    , adj2, FORMULA        , "=G+H"    , 0.0, 0, "#1");
			AddParameter(lblIgnoreIdx     , adj2, BOOL           , null      , 0.0, 1, "#1");
			AddParameter(lblAsLengthIdx   , adj2, IGNORE         , "A3.3"    , 0.0, 0, "#1");
			AddParameter(lblAsNumberIdx   , adj2, IGNORE         , "A3.4"    , 0.0, 0, "#1");
			AddParameter(lblAsYesNoIdx    , adj2, IGNORE         , "A3.5"    , 0.0, 0, "#1");

			AddParameter(LabelIdx         , 0, ERROR, "", 0.0, 0, "end of list");
		}

	}


	public class AnnotationSymbol
	{
		private static int id = 100000;
		private int elementId = -1;

		public IList<Parameter> parameters;

		public IList<Parameter> GetOrderedParameters()
		{
			return parameters;
		}

		public string Name { get; set; }

		public int Id
		{
			get
			{
				if (elementId == -1)
				{
					elementId = id++;
				}

				return elementId;
			}
		}
	}

	public class Parameter
	{
		private string asString;
		private double asDouble;
		private int asInteger;


		public Definition Definition { get; set; }

		public string AsString () =>  asString;
		public double AsDouble () =>  asDouble;
		public int AsInteger () =>  asInteger;

		public Parameter(   string name, ParamDataType type, 
			string strVal, double dblVal, int intVal)
		{
			Definition = new Definition() {Name = name, Type = type};
			asString = strVal;
			asDouble = dblVal;
			asInteger = intVal;
		}


	}

	public class Definition
	{
		public string Name { get; set; }
		public ParamDataType Type { get; set; }
	}


}
