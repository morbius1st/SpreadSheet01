using System.Collections.Generic;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.RevitSupport.RevitCellParameters;


// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitCellItem.cs
// Created:      2021-02-17 (6:43 PM)

namespace SpreadSheet01.RevitSupport
{

	public enum ValueStatus
	{
		DONE = 0,
		RECALCULATE = 1,
		WRITE_TO_CELL = 2,
		WRITE_TO_EXCEL =3,
		WRITE_TO_BOTH = 4
	}

	public enum ParamGroupType
	{
		DATA,
		COLLECTION,
		LABEL
	}

	public enum ParamDataType
	{
		ERROR = -1,
		EMPTY = 0,
		GROUP,
		IGNORE,
		STRING,
		NUMBER,
		ADDRESS,
		RELATIVEADDRESS,
		BOOL,
		DATATYPE
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

		PARAM_INVALID_CS001100,       // as in, the parameter must have a value
		PARAM_VALUE_MISSING_CS001101, // as in, the parameter must have a value
		PARAM_MISSING_CS001102,       // as in, the family does not have the parameter
		PARAM_VALUE_NAN_CS001103,     // as in, not a number


		PARAM_INVALID_INDEX_CS001115, // as in, label index is no good

		INVALID_ANNO_SYM_CS001120,

		LOCATION_BAD_CS001120, // as in, coordinates are no good

		DUPLICATE_KEY_CS000I01,      // all programs - internal - duplicate key error
		INVALID_DATA_FORMAT_CS000I10 // as in, not a proper number to parse
	}


	public class RevitCellItem
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

		public ARevitParam[] CellValues = new ARevitParam[DataParamCount];

		private Dictionary<string, RevitParamLabel> textValues = new Dictionary<string, RevitParamLabel>();

		private List<RevitCellErrorCode> errors = new List<RevitCellErrorCode>();


		private ParamDataType cellParamDataType;
		private AnnotationSymbol annoSymbol;


		public RevitCellItem()
		{
			cellParamDataType = ParamDataType.EMPTY;

			HasError = false;
		}

		public ARevitParam this[int idx]
		{
			get => CellValues[idx];
			set => CellValues[idx] = value;
		}





		// information held in array

	#region held in array

		public string Name
		{
			get => CellValues[NameIdx].GetValue();
			set
			{
				RevitParamText rv = new RevitParamText(value, CellParams[NameIdx]);
				CellValues[NameIdx] = rv;
			}
		}

		public string CellAddr
		{
			get => CellValues[CellAddrIdx].GetValue();
			set
			{
				RevitParamText rv = new RevitParamText(value, CellParams[CellAddrIdx]);
				CellValues[CellAddrIdx] = rv;
			}
		}

		public bool DataIsToCell
		{
			get => CellValues[DataIsToCellIdx].GetValue();
			set
			{
				RevitParamBool rv = new RevitParamBool(value, CellParams[DataIsToCellIdx]);
				CellValues[DataIsToCellIdx] = rv;
			}
		}

		public bool? HasError
		{
			get => CellValues[HasErrorsIdx].GetValue();
			set
			{
				RevitParamBool rv = new RevitParamBool(value, CellParams[HasErrorsIdx]);
				CellValues[HasErrorsIdx] = rv;
			}
		}

	#endregion


	#region cell item data

		public ParamDataType CellParamDataType
		{
			get => cellParamDataType;
			set => cellParamDataType = value;
		}

		public AnnotationSymbol AnnoSymbol
		{
			get => annoSymbol;
			set => annoSymbol = value;
		}

		public RevitCellErrorCode Error
		{
			set
			{
				if (errors.Contains(value)) return;

				errors.Add(value);
				HasError = true;
			}
		}

	#endregion



		public bool Add(ParamDesc pd, Parameter param)
		{
			CellParamDataType = pd.DataType;

			if (pd.GroupType == ParamGroupType.LABEL)
			{
				return AddLabel(pd, param);
			}

			return AddInfo(pd, param);
		}

		private bool AddInfo(ParamDesc pd, Parameter param)
		{

			return true;
		}
		
		private bool AddLabel(ParamDesc pd, Parameter param)
		{

			return true;
		}





		// public void AddText(string value, string paramName, ParamDesc paramDesc)
		// {
		// 	int idx = paramDesc.Index;
		//
		// 	RevitParamLabel rt = (RevitParamLabel) CellValues[idx];
		//
		// 	if (rt == null)
		// 	{
		// 		rt = new RevitParamLabel(paramName, paramDesc);
		// 	}
		//
		// 	rt.AddText(value, paramName, paramDesc);
		//
		// 	CellValues[idx] = rt;
		// }


		public IEnumerable<RevitCellErrorCode> Errors
		{
			get
			{
				foreach (RevitCellErrorCode error in errors)
				{
					yield return error;
				}
			}
		}


		// public string Text
		// {
		// 	get => CellValues[TextIdx].GetValue();
		// 	set
		// 	{
		// 		// RevitParamLabel rv = new RevitParamLabel(value, CellParams[TextIdx]);
		// 		// CellValues[TextIdx] = rv;
		// 	}
		// }
		// public string CellAddrName
		// {
		// 	get => CellValues[CellAddrNameIdx].GetValue();
		// 	set
		// 	{
		// 		RevitParamText rv = new RevitParamText(value, CellParams[CellAddrNameIdx]);
		// 		CellValues[CellAddrNameIdx] = rv;
		// 	}
		// }
		//
		// public string Formula
		// {
		// 	get => CellValues[FormulaIdx].GetValue();
		// 	set
		// 	{
		// 		RevitParamText rv = new RevitParamText(value, CellParams[FormulaIdx]);
		// 		CellValues[FormulaIdx] = rv;
		// 	}
		// }
		//
		// public double ValueAsNumber
		// {
		// 	get => CellValues[ValueAsNumberIdx].GetValue();
		// 	set
		// 	{
		// 		RevitParamNumber rv = new RevitParamNumber(value, CellParams[ValueAsNumberIdx]);
		// 		CellValues[ValueAsNumberIdx] = rv;
		// 	}
		// }
		//
		// public string ResultAsString
		// {
		// 	get => CellValues[CalcResultsAsTextIdx].GetValue();
		// 	set
		// 	{
		// 		RevitParamText rv = new RevitParamText(value, CellParams[CalcResultsAsTextIdx]);
		// 		CellValues[CalcResultsAsTextIdx] = rv;
		// 	}
		// }
		//
		// public double ResultAsNumber
		// {
		// 	get => CellValues[CalcResultsAsNumberIdx].GetValue();
		// 	set
		// 	{
		// 		RevitParamNumber rv = new RevitParamNumber(value, CellParams[CalcResultsAsNumberIdx]);
		// 		CellValues[CalcResultsAsNumberIdx] = rv;
		// 	}
		// }
		//
		// public string GlobalParamName
		// {
		// 	get => CellValues[GlobalParamNameIdx].GetValue();
		// 	set
		// 	{
		// 		RevitParamText rv = new RevitParamText(value, CellParams[GlobalParamNameIdx]);
		// 		CellValues[GlobalParamNameIdx] = rv;
		// 	}
		// }


		public override string ToString()
		{
			return Name + " <|> " + CellAddr + " <|> " + (errors[0].ToString() ?? "No Errors");
		}




	}
}