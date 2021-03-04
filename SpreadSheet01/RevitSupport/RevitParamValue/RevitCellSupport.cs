

// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitParamSupport.cs
// Created:      2021-02-26 (9:46 PM)




namespace SpreadSheet01.RevitSupport.RevitParamValue
{
#region enums

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
		UPDATE_TYPE
	}

	public enum ParamGroup
	{
		DATA,
		CONTAINER,
		LABEL,
		MUST_EXIST
	}

	public enum ValueStatus
	{
		DONE = 0,
		RECALCULATE = 1,
		WRITE_TO_CELL = 2,
		WRITE_TO_EXCEL = 3,
		WRITE_TO_BOTH = 4
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
		PARAM_VALUE_BAD_REL_ADDR_CS001104,	// relative address is no good
		PARAM_VALUE_BAD_ADDR_CS001105,	// address is no good


		PARAM_INVALID_INDEX_CS001115, // as in, label index is no good

		INVALID_ANNO_SYM_CS001120,

		LOCATION_BAD_CS001120, // as in, coordinates are no good

		DUPLICATE_KEY_CS000I01,      // all programs - internal - duplicate key error
		INVALID_DATA_FORMAT_CS000I10 // as in, not a proper number to parse
	}


#endregion


}