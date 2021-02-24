using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;
using Tests.CellsTests.RevitValue;
using UtilityLibrary;

using static Tests.CellsTests.RevitCellParameters;


// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitCellItem.cs
// Created:      2021-02-17 (6:43 PM)

namespace Tests.CellsTests
{
	
	public enum ValueStatus
	{
		DONE = 0,
		RECALCULATE = 1,
		WRITE_TO_CELL = 2,
		WRITE_TO_EXCEL =3,
		WRITE_TO_BOTH = 4
	}

	public enum ParamDataType
	{
		ERROR = -1,
		EMPTY = 0,
		IGNORE,
		STRING,
		NUMBER,
		ADDRESS,
		BOOL,
		TEXT
	}

	public enum ParamMode
	{
		NOT_USED,
		CALCULATED,            // and written to parameter
		READ_FROM_PARAMETER,   // and into collection only
		READ_FROM_EXCEL,       // and write to parameter only
		WRITE_TO_EXCEL,        // after being read from parameter
	}

	public enum ParamReadReqmt 
	{
		READ_VALUE_IGNORE,
		READ_VALUE_OPTIONAL,
		READ_VALUE_REQUIRED,
		READ_VALUE_SET_REQUIRED,
		READ_VALUE_REQD_IF_NUMBER
	}

	public enum RevitCellErrorCode
	{
		NO_ERROR = 0,
		ADDRESS_RANGE,
		ADDRESS_BAD,
		DATA_FLOW_CONFLICT,
		FORMULA_ERROR,

		PARAM_INVALID_CS001100,				// as in, the parameter must have a value
		PARAM_VALUE_MISSING_CS001101,		// as in, the parameter must have a value
		PARAM_MISSING_CS001102,				// as in, the family does not have the parameter
		PARAM_VALUE_NAN_CS001103,			// as in, not a number
		
		INVALID_ANNO_SYM_CS001111,

		LOCATION_BAD_CS001115,				// as in, coordinates are no good

		DUPLICATE_KEY_CS000I01,				// all programs - internal - duplicate key error
		INVALID_DATA_FORMAT_CS000I10		// as in, not a proper number to parse
	}


	public class RevitCellItem // : IEnumerable<RevitValueText>
	{
	#region family parameters

		// private string name;				// set - created
		// private string text;				// set - read from excel (special)
		// private string cellAddrName;		// get - read from family
		// private string cellAddr;			// get - read from family
		// private string formula;			// get - read from family
		// private double valueAsNumber;	// set - read from excel
		// private string resultAsString;	// set - created
		// private double resultAsNumber;	// set - created
		// private string formattingInfo;	// get - read from family
		// private string globalParamName;	// get - read from family
		// private bool dataIsToCell;		// get - read from family
		// private bool hasError;			// set - created

	#endregion

		// public dynamic[] CellValues = new dynamic[ItemIdCount];

		public ARevitValue[] CellValues = new ARevitValue[ItemIdCount];

		// private Dictionary<string, RevitValueText> textValues = new Dictionary<string, RevitValueText>();

		private List<RevitCellErrorCode> errors = new List<RevitCellErrorCode>();

		// private RevitCellErrorCode errorType;

		private ParamDataType cellParamDataType;

		private int annoSymbol;

		// static RevitCellItem()
		// {
		// 	ItemIdCount = CellParamInfo.Count;
		// }

		public RevitCellItem()
		{
			cellParamDataType = ParamDataType.EMPTY;
			HasError = false;
			ResultAsNumber = double.NaN;
			ValueAsNumber = double.NaN;
		}

		public ARevitValue this[int idx]
		{
			get => CellValues[idx];
			set => CellValues[idx] = value;
		}



		public string Name
		{
			get => CellValues[NameIdx].GetValue();
			set
			{
				RevitValueString rv = new RevitValueString(value, CellParams[NameIdx]);
				CellValues[NameIdx] = rv;
			}
		}

		// public string Text
		// {
		// 	get => CellValues[TextIdx].GetValue();
		// 	set
		// 	{
		// 		// RevitValueText rv = new RevitValueText(value, CellParams[TextIdx]);
		// 		// CellValues[TextIdx] = rv;
		// 	}
		// }

		// public string CellAddrName
		// {
		// 	get => CellValues[CellAddrNameIdx].GetValue();
		// 	set
		// 	{
		// 		RevitValueString rv = new RevitValueString(value, CellParams[CellAddrNameIdx]);
		// 		CellValues[CellAddrNameIdx] = rv;
		// 	}
		// }

		public string CellAddr
		{
			get => CellValues[CellAddrIdx].GetValue();
			set
			{
				RevitValueString rv = new RevitValueString(value, CellParams[CellAddrIdx]);
				CellValues[CellAddrIdx] = rv;
			}
		}

		public string Formula
		{
			get => CellValues[FormulaIdx].GetValue();
			set
			{
				RevitValueString rv = new RevitValueString(value, CellParams[FormulaIdx]);
				CellValues[FormulaIdx] = rv;
			}
		}

		public double ValueAsNumber
		{
			get => CellValues[ValueAsNumberIdx].GetValue();
			set
			{
				RevitValueNumber rv = new RevitValueNumber(value, CellParams[ValueAsNumberIdx]);
				CellValues[ValueAsNumberIdx] = rv;
			}
		}

		public string ResultAsString
		{
			get => CellValues[CalcResultsAsTextIdx].GetValue();
			set
			{
				RevitValueString rv = new RevitValueString(value, CellParams[CalcResultsAsTextIdx]);
				CellValues[CalcResultsAsTextIdx] = rv;
			}
		}

		public double ResultAsNumber
		{
			get => CellValues[CalcResultsAsNumberIdx].GetValue();
			set
			{
				RevitValueNumber rv = new RevitValueNumber(value, CellParams[CalcResultsAsNumberIdx]);
				CellValues[CalcResultsAsNumberIdx] = rv;
			}
		}

		public string GlobalParamName
		{
			get => CellValues[GlobalParamNameIdx].GetValue();
			set
			{
				RevitValueString rv = new RevitValueString(value, CellParams[GlobalParamNameIdx]);
				CellValues[GlobalParamNameIdx] = rv;
			}
		}

		public bool DataIsToCell
		{
			get => CellValues[DataIsToCellIdx].GetValue();
			set
			{
				RevitValueBool rv = new RevitValueBool(value, CellParams[DataIsToCellIdx]);
				CellValues[DataIsToCellIdx] = rv;
			}
		}

		public bool HasError
		{
			get => CellValues[HasErrorsIdx].GetValue();
			set
			{
				RevitValueBool rv = new RevitValueBool(value, CellParams[HasErrorsIdx]);
				CellValues[HasErrorsIdx] = rv;
			}
		}

		public ParamDataType CellParamDataType
		{
			get => cellParamDataType;
			set => cellParamDataType = value;
		}

		public int AnnoSymbol
		{
			get => annoSymbol;
			set => annoSymbol = value;
		}

		public RevitCellErrorCode Error
		{
			set
			{
				errors.Add(value);
				HasError = true;
			}
		}

		public IEnumerable<RevitCellErrorCode> Errors()
		{
			if (errors == null) yield break;

			foreach (RevitCellErrorCode error in errors)
			{
				yield return error;
			}
		}

		public void AddText(string value, string paramName, ParamDesc paramDesc)
		{
			int idx = paramDesc.Index;

			RevitValueText rt = (RevitValueText) CellValues[idx];

			if (rt == null)
			{
				rt = new RevitValueText(paramName, paramDesc);
			}

			rt.AddText(value, paramName, paramDesc);

			CellValues[idx] = rt;
		}

		public override string ToString()
		{
			return Name + " <|> " + CellParamDataType + " <|> " + (errors[0].ToString() ?? "No Errors");
		}
	}
}