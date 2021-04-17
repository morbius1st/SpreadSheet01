using SpreadSheet01.RevitSupport.RevitParamManagement;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamType;

// Solution:     SpreadSheet01
// // projname: CellsTest// File:             RevitCellErrorCode.cs
// Created:      2021-03-14 (6:38 PM)


namespace SpreadSheet01.Management
{
	public static class ErrorCodesAssist
	{
		public static ErrorCodes[][] EC_MustExist;

		static ErrorCodesAssist()
		{
			EC_MustExist = new ErrorCodes[(int) RevitParamSupport.PARAM_CLASS_COUNT][];
			EC_MustExist[(int) ParamClass.PC_CELL] = new ErrorCodes[4];
			EC_MustExist[(int) ParamClass.PC_CHART] = new ErrorCodes[3];

			EC_MustExist[(int) ParamClass.PC_CELL][(int) PT_INSTANCE] 
				= ErrorCodes.CEL_INSTANCE_PARAM_MISSING_CS001122;
			EC_MustExist[(int) ParamClass.PC_CELL][(int) PT_INTERNAL] 
				= ErrorCodes.CEL_INTERNAL_PARAM_MISSING_CS001123;
			EC_MustExist[(int) ParamClass.PC_CELL][(int) PT_TYPE] 
				= ErrorCodes.CEL_TYPE_PARAM_MISSING_CS001121;
			EC_MustExist[(int) ParamClass.PC_CELL][(int) PT_LABEL] 
				= ErrorCodes.LBL_PARAM_MISSING_CS001123;

			EC_MustExist[(int) ParamClass.PC_CHART][(int) PT_TYPE] 
				= ErrorCodes.RCD_TYPE_PARAM_MISSING_CS001197;
			EC_MustExist[(int) ParamClass.PC_CHART][(int) PT_INSTANCE] 
				= ErrorCodes.RCD_INSTANCE_PARAM_MISSING_CS001198;
			EC_MustExist[(int) ParamClass.PC_CHART][(int) PT_INTERNAL] 
				= ErrorCodes.RCD_INTERNAL_PARAM_MISSING_CS001199;
		}
	}


	public enum ErrorCodes
	{
		EC_NO_ERROR = 0,
		EC_ADDRESS_RANGE,
		EC_ADDRESS_BAD,
		EC_DATA_FLOW_CONFLICT,
		EC_FORMULA_ERROR,

		EC_PARAM_INVALID_CS001100, // as in, the parameter must have a value

	#region errors 100 to 124 / cell parameter errors


		CEL_VALUE_MISSING_CS001102,      // as in, the parameter must have a value
		CEL_VALUE_NAN_CS001103,          // as in, not a number
		CEL_VALUE_BAD_REL_ADDR_CS001104, // relative address is no good
		CEL_VALUE_BAD_ADDR_CS001105,     // address is no good
		CEL_VALUE_BAD_FORMULA_CS001106,  // formula bad
		CEL_HAS_ERROR_CS001107,          // general
		CEL_INVALID_TYPE_CS001113,
		CEL_TYPE_PARAM_MISSING_CS001121,
		CEL_INSTANCE_PARAM_MISSING_CS001122,
		CEL_INTERNAL_PARAM_MISSING_CS001123,
		CEL_LABEL_PARAM_MISSING_CS001124,

		
		PARAM_INVALID_INDEX_CS001134, // as in, label index is no good
	#endregion

	#region errors 125 to 134 / cell parameter label errors

		LBL_INVALID_CS001125,
		LBL_PARAM_MISSING_CS001123,
		

	#endregion

	#region errors 135 to 169 / RevitChart

		CHT_CELL_HAS_ERROR_CS001135,
		CHT_FAIL_TO_ADD_CS001036,
		CHT_PARAM_HAS_ERROR_CS001137,
		CHT_RCD_HAS_ERRORS_CS001138,
		CHT_BAD_FILE_PATH_CS001142,
		CHT_INVALID_PARAM_TYPE_CS001143,	// pram type not matched
		CHT_BAD_UPDATE_TYPE_CS001144,
		CHT_PROCESS_CHART_FAILED_CS001145,

	#endregion

	#region errors 170 to 199 / RevitChartData

		RCD_TYPE_PARAM_MISSING_CS001197,
		RCD_INSTANCE_PARAM_MISSING_CS001198,
		RCD_INTERNAL_PARAM_MISSING_CS001199,

	#endregion

	#region errors 200 to 224 / formula

		CHTS_CHART_HAS_ERRORS_CS001200,

	#endregion


	#region errors 225 to 249 / formula

		FOR_FORMULA_ERROR_CS001200,

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