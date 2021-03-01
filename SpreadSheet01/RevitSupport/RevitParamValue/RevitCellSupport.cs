
using UtilityLibrary;

using static SpreadSheet01.RevitSupport.RevitCellParameters;

// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitParamSupport.cs
// Created:      2021-02-26 (9:46 PM)




namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public static class RevitValueSupport
	{
		private const string KEY_IDX_BEGIN  = "《";
		private const string KEY_IDX_END    = "》";

		private static int annoSymUniqueIdx = 0;

		public static string MakeLabelKey(int paramIdx)
		{
			return KEY_IDX_BEGIN + paramIdx.ToString("D2") + KEY_IDX_END;
		}

		public static string MakeAnnoSymKey(RevitAnnoSym aSym, bool asSeqName)
		{
			string seq = aSym[SeqIdx].GetValue();

			seq = KEY_IDX_BEGIN + $"{(seq.IsVoid() ? "ZZZZZ" : seq),8}" + KEY_IDX_END;

			string name = aSym[NameIdx].GetValue();
			name = name.IsVoid() ? "un-named" : name;

			string eid = aSym.AnnoSymbol?.Id.ToString() ?? "Null Symbol " + annoSymUniqueIdx++.ToString("D7");
			
			if (asSeqName) return seq + name + eid;

			return name + seq + eid;

		}

		public static string LABEL_ID_PREFIX = "#";

		// provide a "clean" parameter name
		// provide label parameter id (as out)
		// provide bool if the info provided is for the label
		public static string GetParamName(string paramName, out int id, out bool isLabel)
		{
			int idx1;
			int idx2;

			string name;

			isLabel = false;

			id = GetLabelIndex(paramName, out idx1, out idx2);

			if (id < 0) return paramName;

			if (idx1 == 1)
			{
				name = paramName.Substring(idx2 + 1).Trim();
				isLabel = true;
			}
			else
			{
				name = paramName.Substring(0, idx1-1).Trim();
			}

			return name;
		}

		public static int GetLabelIndex(string paramName, out int idx1, out int idx2)
		{
			idx2 = -1;
			idx1 = paramName.IndexOf(LABEL_ID_PREFIX) + 1;

			if (idx1 <= 0) return -1;

			idx2 = paramName.IndexOf(' ', idx1);

			idx2 = idx2 > 0 ? idx2 - idx1 : paramName.Length - idx1;

			bool result;
			int index;
			string test = paramName.Substring(idx1, idx2);

			result = int.TryParse(paramName.Substring(idx1, idx2), out index);

			return result ? index : -1;
		}

	}

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
		LABEL_TITLE,
		IGNORE,
		TEXT,
		NUMBER,
		ADDRESS,
		RELATIVEADDRESS,
		BOOL,
		DATATYPE,
		FORMULA
	}

	public enum ParamGroup
	{
		DATA,
		CONTAINER,
		LABEL
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