// Solution:     SpreadSheet01
// Project:       Cells
// File:             RevitCellErrorCode.cs
// Created:      2021-03-14 (6:38 PM)

namespace SpreadSheet01.Management
{
	public enum ErrorCodes
	{
		NO_ERROR = 0,
		ADDRESS_RANGE,
		ADDRESS_BAD,
		DATA_FLOW_CONFLICT,
		FORMULA_ERROR,

		PARAM_INVALID_CS001100, // as in, the parameter must have a value
		PARAM_MISSING_CS001101, // as in, the family does not have the parameter

		PARAM_VALUE_MISSING_CS001102,      // as in, the parameter must have a value
		PARAM_VALUE_NAN_CS001103,          // as in, not a number
		PARAM_VALUE_BAD_REL_ADDR_CS001104, // relative address is no good
		PARAM_VALUE_BAD_ADDR_CS001105,     // address is no good
		PARAM_VALUE_BAD_FORMULA_CS001106,  // address is no good

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
}