#region + Using Directives
#endregion

// user name: jeffs
// created:   2/23/2021 6:39:44 PM

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using SpreadSheet01.RevitSupport;
using UtilityLibrary;
using static SpreadSheet01.RevitSupport.RevitCellParameters;

namespace Autodesk.Revit.DB
{
	public class SampleAnnoSymbols
	{
		public AnnotationSymbol[] Symbol {get; set; }

		private int symbolIdx = 0;

		public void Process()
		{
			Symbol = new AnnotationSymbol[3];

			symbolIdx = 0;
			AddSymbol1();

			symbolIdx = 1;
			AddSymbol2();

			symbolIdx = 2;
			AddSymbol3();

		}

		public void AddParameter(int index, string strVal, double dblVal = 0.0, int intVal = 0, string name = "")
		{
			string paramName = CellParams[NameIdx].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, strVal, dblVal, intVal);
			Symbol[symbolIdx].parameters.Add(p);
		}

		private void AddSymbol1()
		{
			AddParameter(NameIdx, "Myname1");
			AddParameter(SeqIdx, "1");
			AddParameter(CellAddrIdx, "@A2");
			AddParameter(FormattingInfoIdx, "format info");
			AddParameter(GraphicType, "graphic type");
			AddParameter(DataIsToCellIdx, "", 0.0, 1);
			AddParameter(HasErrorsIdx, "", 0.0, 1);
			AddParameter(DataVisible, "", 0.0, 1);

			AddParameter(LabelIdx, "", 0.0, 0, "Label #A");
			AddParameter(lblRelAddrIdx , "(1,1)", 0.0, 0, "#A");
			AddParameter(lblDataTypeIdx, "text", 0.0, 0, "#A");
			AddParameter(lblFormulaIdx , "", 0.0, 0, "#A");
			AddParameter(lblIgnoreIdx  , "", 0.0, 0, "#A");
			AddParameter(lblAsLengthIdx, "", 0.0, 0, "#A");
			AddParameter(lblAsNumberIdx, "", 0.0, 0, "#A");
			AddParameter(lblAsYesNoIdx , "", 0.0, 0, "#A");
		}

		private void AddSymbol2()
		{
			AddParameter(NameIdx, "Myname2");
			AddParameter(SeqIdx, "1");
			AddParameter(CellAddrIdx, "@A4");
			AddParameter(FormattingInfoIdx, "format info");
			AddParameter(GraphicType, "graphic type");
			AddParameter(DataIsToCellIdx, "", 0.0, 1);
			AddParameter(HasErrorsIdx, "", 0.0, 1);
			AddParameter(DataVisible, "", 0.0, 1);

			AddParameter(LabelIdx, "", 0.0, 0, "Label #A");
			AddParameter(lblRelAddrIdx , "(1,1)", 0.0, 0, "#A");
			AddParameter(lblDataTypeIdx, "text", 0.0, 0, "#A");
			AddParameter(lblFormulaIdx , "", 0.0, 0, "#A");
			AddParameter(lblIgnoreIdx  , "", 0.0, 0, "#A");
			AddParameter(lblAsLengthIdx, "", 0.0, 0, "#A");
			AddParameter(lblAsNumberIdx, "", 0.0, 0, "#A");
			AddParameter(lblAsYesNoIdx , "", 0.0, 0, "#A");
		}

		private void AddSymbol3()
		{
			AddParameter(NameIdx, "Myname3");
			AddParameter(SeqIdx, "1");
			AddParameter(CellAddrIdx, "@A6");
			AddParameter(FormattingInfoIdx, "format info");
			AddParameter(GraphicType, "graphic type");
			AddParameter(DataIsToCellIdx, "", 0.0, 1);
			AddParameter(HasErrorsIdx, "", 0.0, 1);
			AddParameter(DataVisible, "", 0.0, 1);

			AddParameter(LabelIdx, "", 0.0, 0, "Label #A");
			AddParameter(lblRelAddrIdx , "(1,1)", 0.0, 0, "#A");
			AddParameter(lblDataTypeIdx, "text", 0.0, 0, "#A");
			AddParameter(lblFormulaIdx , "", 0.0, 0, "#A");
			AddParameter(lblIgnoreIdx  , "", 0.0, 0, "#A");
			AddParameter(lblAsLengthIdx, "", 0.0, 0, "#A");
			AddParameter(lblAsNumberIdx, "", 0.0, 0, "#A");
			AddParameter(lblAsYesNoIdx , "", 0.0, 0, "#A");
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

		public int ElementId
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
		private double asNumber;
		private int asInteger;


		public Definition Definition { get; set; }

		public string AsString () =>  asString;
		public double AsNumber () =>  asNumber;
		public int AsInteger () =>  asInteger;

		public Parameter(string name, string strVal, double dblVal, int intVal)
		{
			Definition = new Definition() {Name = name };
			asString = strVal;
			asNumber = dblVal;
			asInteger = intVal;
		}


	}

	public class Definition
	{
		public string Name { get; set; }
	}


}
