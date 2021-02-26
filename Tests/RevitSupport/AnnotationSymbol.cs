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

		public void AddParameter(int index, int adjIdx, string strVal, double dblVal = 0.0, int intVal = 0,
			string name = null, string suffix = null)
		{
			string paramName = CellAllParams[NameIdx].ParameterName;
			paramName = name.IsVoid() ? paramName : name + " " + paramName;

			Parameter p = new Parameter(paramName, strVal, dblVal, intVal);
			Symbol[symbolIdx].parameters.Add(p);
		}

		private void AddSymbol1()
		{
			int adj1 = ParamCounts[(int) ParamGroupType.DATA];
			int adj2 = adj1 + ParamCounts[(int) ParamGroupType.COLLECTION];


			AddParameter(NameIdx, 0, "Myname1");
			AddParameter(SeqIdx, 0, "1");
			AddParameter(CellAddrIdx, 0, "@A2");
			AddParameter(FormattingInfoIdx, 0, "format info");
			AddParameter(GraphicType, 0, "graphic type");
			AddParameter(DataIsToCellIdx, 0, "", 0.0, 1);
			AddParameter(HasErrorsIdx, 0, "", 0.0, 1);
			AddParameter(DataVisibleIdx, 0, "", 0.0, 1);

			AddParameter(LabelIdx, adj1, "", 0.0, 0, "Label #1");
			AddParameter(lblRelAddrIdx, adj2 , "(1,1)", 0.0, 0, "#1");
			AddParameter(lblDataTypeIdx, adj2, "text", 0.0, 0, "#1");
			AddParameter(lblFormulaIdx, adj2, "", 0.0, 0, "#1");
			AddParameter(lblIgnoreIdx, adj2, "", 0.0, 0, "#1");
			AddParameter(lblAsLengthIdx, adj2, "", 0.0, 0, "#1");
			AddParameter(lblAsNumberIdx, adj2, "", 0.0, 0, "#1");
			AddParameter(lblAsYesNoIdx, adj2, "", 0.0, 0, "#1");
		}

		private void AddSymbol2()
		{
			int adj1 = ParamCounts[(int) ParamGroupType.DATA];
			int adj2 = adj1 + ParamCounts[(int) ParamGroupType.COLLECTION];

			AddParameter(NameIdx, 0, "Myname2");
			AddParameter(SeqIdx, 0, "1");
			AddParameter(CellAddrIdx, 0, "@A4");
			AddParameter(FormattingInfoIdx, 0, "format info");
			AddParameter(GraphicType, 0, "graphic type");
			AddParameter(DataIsToCellIdx, 0, "", 0.0, 1);
			AddParameter(HasErrorsIdx, 0, "", 0.0, 1);
			AddParameter(DataVisibleIdx, 0, "", 0.0, 1);

			AddParameter(LabelIdx, adj1, "", 0.0, 0, "Label #1");
			AddParameter(lblRelAddrIdx, adj2 , "(1,1)", 0.0, 0, "#1");
			AddParameter(lblDataTypeIdx, adj2, "text", 0.0, 0, "#1");
			AddParameter(lblFormulaIdx, adj2, "", 0.0, 0, "#1");
			AddParameter(lblIgnoreIdx, adj2, "", 0.0, 0, "#1");
			AddParameter(lblAsLengthIdx, adj2, "", 0.0, 0, "#1");
			AddParameter(lblAsNumberIdx, adj2, "", 0.0, 0, "#1");
			AddParameter(lblAsYesNoIdx, adj2, "", 0.0, 0, "#1");
		}

		private void AddSymbol3()
		{
			int adj1 = ParamCounts[(int) ParamGroupType.DATA];
			int adj2 = adj1 + ParamCounts[(int) ParamGroupType.COLLECTION];

			AddParameter(NameIdx, 0, "Myname3");
			AddParameter(SeqIdx, 0, "1");
			AddParameter(CellAddrIdx, 0, "@A6");
			AddParameter(FormattingInfoIdx, 0, "format info");
			AddParameter(GraphicType, 0, "graphic type");
			AddParameter(DataIsToCellIdx, 0, "", 0.0, 1);
			AddParameter(HasErrorsIdx, 0, "", 0.0, 1);
			AddParameter(DataVisibleIdx, 0, "", 0.0, 1);

			AddParameter(LabelIdx, adj1, "", 0.0, 0, "Label #1");
			AddParameter(lblRelAddrIdx, adj2 , "(1,1)", 0.0, 0, "#1");
			AddParameter(lblDataTypeIdx, adj2, "text", 0.0, 0, "#1");
			AddParameter(lblFormulaIdx, adj2, "", 0.0, 0, "#1");
			AddParameter(lblIgnoreIdx, adj2, "", 0.0, 0, "#1");
			AddParameter(lblAsLengthIdx, adj2, "", 0.0, 0, "#1");
			AddParameter(lblAsNumberIdx, adj2, "", 0.0, 0, "#1");
			AddParameter(lblAsYesNoIdx, adj2, "", 0.0, 0, "#1");
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
