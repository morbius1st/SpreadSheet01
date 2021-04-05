// Solution:     SpreadSheet01
// // projname: CellsTest// File:             RevitCellErrorCode.cs
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

	#region errors 100 to 124 / cell parameter errors


		PARAM_VALUE_MISSING_CS001102,      // as in, the parameter must have a value
		PARAM_VALUE_NAN_CS001103,          // as in, not a number
		PARAM_VALUE_BAD_REL_ADDR_CS001104, // relative address is no good
		PARAM_VALUE_BAD_ADDR_CS001105,     // address is no good
		PARAM_VALUE_BAD_FORMULA_CS001106,  // formula bad
		PARAM_HAS_ERROR_CS001107,          // general
		PARAM_INVALID_TYPE_CS001113,

		
		PARAM_INVALID_INDEX_CS001134, // as in, label index is no good
	#endregion

	#region errors 125 to 134 / cell parameter label errors

		LABEL_INVALID_CS001125,

		

	#endregion
	#region errors 135 to 170 / RevitChart

		CHART_CELL_FAMILY_MISSING_CS001136,
		CHART_PARAM_HAS_ERROR_CS001137,
		CHART_MUST_EXIST_MISSING_CS001138,
		CHART_BAD_FILE_PATH_CS001142,
		CHART_INVALID_PARAM_TYPE_CS001143,	// pram type not matched
		CHART_BAD_UPDATE_TYPE_CS001144,
		CHART_WKSHT_MISSING_CS001146,

	#endregion

		NO_CELLS_FOUND_CS100200, // no cells of a specific family type were found



		DUPLICATE_KEY_CS000I01,      // all programs - internal - duplicate key error - general

		DUPLICATE_KEY_CS000I01_1,    // all programs - internal - duplicate key error - location 1
		DUPLICATE_KEY_CS000I01_2,    // all programs - internal - duplicate key error - location 2
		DUPLICATE_KEY_CS000I01_3,    // all programs - internal - duplicate key error - location 3
		DUPLICATE_KEY_CS000I01_4,    // all programs - internal - duplicate key error - location 4
		DUPLICATE_KEY_CS000I01_5,    // all programs - internal - duplicate key error - location 5

		INVALID_DATA_FORMAT_CS000I10, // as in, not a proper number to parse
	}
}