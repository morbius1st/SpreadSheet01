

// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitParamSupport.cs
// Created:      2021-02-26 (9:46 PM)


using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
#region enums

	public enum ParamType
	{
		INTERNAL,
		TYPE,
		INSTANCE,
		LABEL
	}



	public enum ParamExistReqmt
	{
		PARAM_MUST_EXIST,
		PARAM_OPTIONAL
	}

	public enum ParamReadReqmt
	{
		READ_VALUE_IGNORE,
		READ_VALUE_OPTIONAL,
		READ_VALUE_REQUIRED,
		READ_VALUE_SET_REQUIRED,
		READ_VALUE_REQD_IF_NUMBER
	}

	public enum ParamMode
	{

		NOT_USED,
		CALCULATED,          // and written to parameter
		READ_FROM_PARAMETER, // and into collection only
		READ_FROM_EXCEL,     // and write to parameter only
		WRITE_TO_EXCEL,      // after being read from parameter
		INTERNAL
	}

	public enum ParamDataType
	{
		ERROR = -1,
		EMPTY = 0,
		IGNORE,
		LABEL_TITLE,
		TEXT,
		NUMBER,
		ADDRESS,
		RELATIVEADDRESS,
		BOOL,
		DATATYPE,
		FORMULA,
		FILE_PATH,
		UPDATE_TYPE,
		WORKSHEETNAME
	}

	// public enum ParamGroup
	// {
	// 	DATA,
	// 	LABEL_GRP,
	// 	CONTAINER,
	// }

	public enum ValueStatus
	{
		DONE = 0,
		RECALCULATE = 1,
		WRITE_TO_CELL = 2,
		WRITE_TO_EXCEL = 3,
		WRITE_TO_BOTH = 4
	}

	public enum CellUpdateTypeCode
	{
		STANDARD,
		CONSTANT,
		ON_DEMAND,
		BEFORE_PLOT,
		ALL,
		INVALID
	}



	public class CellUpdateTypes
	{
		private static CellUpdateTypes me = new CellUpdateTypes();

		private const int SUBSTRLEN = 2;

		private CellUpdateTypes()
		{
			for (var i = 0; i < updateTypes.GetLength(0); i++)
			{
				updateTypes[i, 1] = updateTypes[i, 0].Substring(0, SUBSTRLEN);
			}
		}

		public static CellUpdateTypes I => me;

		private string[,] updateTypes  = 
		{
			{"Always", ""}, {"Constant",""}, {"On Demand",""}, {"Before Plotting", ""}
		};

		public string this[CellUpdateTypeCode idx] => updateTypes[(int) idx, 0];

		public CellUpdateTypeCode GetTypeCode(string test)
		{
			CellUpdateTypeCode result = CellUpdateTypeCode.STANDARD;

			string compare = test.Substring(0, SUBSTRLEN);

			for (int i = 0; i < updateTypes.GetLength(0); i++)
			{
				if (updateTypes[i, 1].Equals(compare)) break;

				result++;
			}

			return result;
		}
	}


	
	public enum RevitCellErrorCode
	{
		NO_ERROR = 0,
		ADDRESS_RANGE,
		ADDRESS_BAD,
		DATA_FLOW_CONFLICT,
		FORMULA_ERROR,

		PARAM_INVALID_CS001100,					// as in, the parameter must have a value
		PARAM_MISSING_CS001101,					// as in, the family does not have the parameter

		PARAM_VALUE_MISSING_CS001102,			// as in, the parameter must have a value
		PARAM_VALUE_NAN_CS001103,				// as in, not a number
		PARAM_VALUE_BAD_REL_ADDR_CS001104,		// relative address is no good
		PARAM_VALUE_BAD_ADDR_CS001105,			// address is no good
		PARAM_VALUE_BAD_FORMULA_CS001106,		// address is no good

		PARAM_CHART_PARAM_HAS_ERROR_CS001135,
		PARAM_CHART_MUST_EXIST_MISSING_CS001138,
		PARAM_CHART_INVALID_PROG_GRP_CS001140,
		PARAM_CHART_BAD_FILE_PATH_CS001142,
		PARAM_CHART_BAD_UPDATE_TYPE_CS001144,
		PARAM_CHART_WKSHT_MISSING_CS001146,

		PARAM_INVALID_INDEX_CS001135, // as in, label index is no good

		INVALID_ANNO_SYM_CS001140,

		LOCATION_BAD_CS001150, // as in, coordinates are no good

		NO_CELLS_FOUND_CS100200, // no cells of a specific family type were found

		DUPLICATE_KEY_CS000I01,      // all programs - internal - duplicate key error
		INVALID_DATA_FORMAT_CS000I10 // as in, not a proper number to parse
	}


#endregion


}