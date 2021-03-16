// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitParamSupport.cs
// Created:      2021-02-26 (9:46 PM)


namespace SpreadSheet01.RevitSupport.RevitParamManagement
{
#region enums

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