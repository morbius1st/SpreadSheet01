// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitParamSupport.cs
// Created:      2021-02-26 (9:46 PM)


using System;

namespace SpreadSheet01.RevitSupport.RevitParamManagement
{

	public static class RevitParamSupport
	{
		public const int PARAM_CLASS_COUNT = 2;
		public const int PARAM_TYPE_COUNT = 4;

		public static string GetShortName(string name)
		{
			return name.Substring(0, Math.Min(name.Length, RevitParamManager.SHORT_NAME_LEN));
		}
	}

#region enums

	public enum ParamClass
	{
		PC_INTERNAL = -1,
		PC_CHART = 0,
		PC_CELL,
	}

	public enum ParamType
	{
		PT_INST_OR_INTL = -1,
		PT_INSTANCE,
		PT_INTERNAL,
		PT_TYPE,
		PT_LABEL,
		PT_LABEL_INTERNAL,

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

	public enum ParamExistReqmt
	{
		EX_PARAM_MUST_EXIST,
		EX_PARAM_OPTIONAL,
		EX_PARAM_INTERNAL
	}

	public enum ParamReadReqmt
	{
		RD_VALUE_IGNORE,
		RD_VALUE_OPTIONAL,
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
		DT_WORKSHEETNAME,
		DT_SEQUENCE
	}

	public enum ParamRootDataType
	{
		RT_INVALID = -1,
		RT_IGNORE,
		RT_TEXT,
		RT_INTEGER,
		RT_DOUBLE,
		RT_BOOL
	}

	public enum ParamSubDataType
	{
		ST_INVALID = -1,
		ST_NONE = 0,
		ST_LABEL_TITLE,
		ST_DATATYPE,
		ST_FORMULA,
		ST_FILE_PATH,
		ST_UPDATE_TYPE,
		ST_WORKSHEETNAME,
		ST_SEQUENCE
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