// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitParamSupport.cs
// Created:      2021-02-26 (9:46 PM)


namespace SpreadSheet01.RevitSupport.RevitParamManagement
{
#region enums

	public enum ParamClass
	{
		PC_CHART,
		PC_CELL,
		PC_LABEL,
		PC_INTERNAL
	}

	public enum ParamCat
	{
		CT_ANNOTATION,
		CT_WALLS
	}

	public enum ParamSubCat
	{
		SC_GENERIC_ANNOTATION,
		SC_CURTAIN_WALLS
	}

	public enum ParamType
	{
		PT_INTERNAL,
		PT_PARAM,
		// PT_TYPE,
		// PT_INSTANCE,
		PT_LABEL
	}

	public enum ParamExistReqmt
	{
		EX_PARAM_MUST_EXIST,
		EX_PARAM_OPTIONAL,
		EX_PARAM_INTERNAL
	}

	public enum ParamReadReqmt
	{
		RD_VALUE_IGNORE,
		RDVALUE_OPTIONAL,
		RD_VALUE_REQUIRED,
		RD_VALUE_SET_REQUIRED,
		RD_VALUE_REQD_IF_NUMBER
	}

	public enum ParamMode
	{
		PM_NOT_USED,
		PM_CALCULATED,          // and written to parameter
		PM_READ_FROM_FAMILY, // and into collection only
		PM_READ_FROM_EXCEL,     // and write to parameter only
		PM_WRITE_TO_EXCEL,      // after being read from parameter
		PM_INTERNAL
	}

	public enum ParamDataType
	{
		DT_ERROR = -1,
		DT_EMPTY = 0,
		DT_IGNORE,
		DT_LABEL_TITLE,
		DT_TEXT,
		DT_INTEGER,
		DT_NUMBER,
		DT_BOOL,
		DT_DATATYPE,
		DT_FORMULA,
		DT_FILE_PATH,
		DT_UPDATE_TYPE,
		DT_WORKSHEETNAME
	}

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
			{"Always", ""}, {"Constant", ""}, {"On Demand", ""}, {"Before Plotting", ""}
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

#endregion


}