
using System;
using System.Collections.Generic;

using static SpreadSheet01.RevitSupport.ParamReadReqmt;
using static SpreadSheet01.RevitSupport.ParamDataType;
using static SpreadSheet01.RevitSupport.ParamMode;

// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitCellParameters.cs
// Created:      2021-02-20 (5:36 AM)


namespace SpreadSheet01.RevitSupport
{
	
	public class ParamDesc
	{
		private const string TEXT_SHORT_NAME = "Text";

		private string shortName;

		public ParamDesc( string paramName, int index,
			ParamDataType dataType, ParamReadReqmt paramReadReqmt, 
			ParamMode paramMode)
		{
			Index = index;
			ParameterName = paramName;
			shortName = GetShortName(paramName);

			DataType = dataType;
			ReadReqmt = paramReadReqmt;

		}

		public string ParameterName { get; private set; }
		public string ShortName => shortName;
		public int Index { get; private set; }
		public ParamDataType DataType { get; private set; }
		public ParamReadReqmt ReadReqmt { get; private set; }
		public ParamMode Mode { get; private set; }

		public static string GetShortName(string name)
		{
			if (name.StartsWith(TEXT_SHORT_NAME))
			{
				return TEXT_SHORT_NAME;
			}

			return name.Substring(0, Math.Min(name.Length, 22));
		}
	}

	public class RevitCellParameters
	{
		public const int PARAM_IDX = 0;
		public const int PARAM_TYPE = 1;

		public static int ItemIdCount = 0;

		public static readonly int NameIdx                   = ItemIdCount++; // set - created
		public static readonly int SeqIdx                    = ItemIdCount++; // get - read from family
		public static readonly int TextIdx                   = ItemIdCount++; // set - read from excel (special)
		public static readonly int CellAddrIdx               = ItemIdCount++; // get - read from family
		public static readonly int FormulaIdx                = ItemIdCount++; // get - read from family
		public static readonly int ValueAsNumberIdx          = ItemIdCount++; // set - read from excel
		public static readonly int CalcResultsAsTextIdx      = ItemIdCount++; // set - created
		public static readonly int CalcResultsAsNumberIdx    = ItemIdCount++; // set - created
		public static readonly int FormattingInfoIdx         = ItemIdCount++; // get - read from family
		public static readonly int GlobalParamNameIdx        = ItemIdCount++; // get - read from family
		public static readonly int DataIsToCellIdx           = ItemIdCount++; // get - read from family
		public static readonly int HasErrorsIdx              = ItemIdCount++; // set - created

		public static readonly int DataVisible               = ItemIdCount++; // ignore
		public static readonly int GraphicType               = ItemIdCount++; // ignore

		// public static Dictionary<string, ParamDesc> CellParamInfo { get; private set; }

		public static SortedDictionary<string, int> CellParamIndex { get; private set; }
		public static ParamDesc[] CellParams { get; private set; }


		// public static int[] ReadParamFromFamily = new []
		// {
		// 	D_CELL_ADDR_NAME,
		// 	D_CELL_ADDR,
		// 	D_FORMULA,
		// 	D_VALUE_AS_NUMBER,
		// 	D_FORMATTING_INFO,
		// 	D_GLOBAL_PARAM_NAME,
		// 	D_DATA_IS_TO_CELL
		// };

		static RevitCellParameters()
		{
			assignParameters();
		}

		public static ParamDesc Match(string name)
		{
			string shortName = ParamDesc.GetShortName(name);

			ParamDesc pd = null;
			int idx;

			bool result = CellParamIndex.TryGetValue(shortName, out idx);

			if (result)
			{
				pd = CellParams[idx];
			}

			return pd;
		}

		private static void assignParameter(int idx, string shortName, ParamDesc pd)
		{
			CellParamIndex.Add(shortName, idx);
			CellParams[idx] = pd;
		}

		private static void assignParameters()
		{
			CellParamIndex = new SortedDictionary<string, int>();
			CellParams = new ParamDesc[ItemIdCount];

			ParamDesc pd;

			// 1
			pd = new ParamDesc("Name", NameIdx, STRING, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.Index, pd.ShortName, pd);

			// 2
			pd = new ParamDesc("Sequence", SeqIdx, STRING, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.Index, pd.ShortName, pd);

			// 3
			pd = new ParamDesc("Excel Cell Address", CellAddrIdx, STRING, READ_VALUE_SET_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.Index, pd.ShortName, pd);

			// 4
			pd = new ParamDesc("Formula", FormulaIdx, STRING, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.Index, pd.ShortName, pd);

			// 5
			pd = new ParamDesc("Value as Number", ValueAsNumberIdx, NUMBER, READ_VALUE_REQD_IF_NUMBER, READ_FROM_EXCEL);
			assignParameter(pd.Index, pd.ShortName, pd);

			// 6
			pd = new ParamDesc("Calculation Results Text", CalcResultsAsTextIdx, STRING, READ_VALUE_IGNORE, READ_FROM_PARAMETER);
			assignParameter(pd.Index, pd.ShortName, pd);

			// 7
			pd = new ParamDesc("Calculation Results Number", CalcResultsAsNumberIdx, NUMBER, READ_VALUE_IGNORE, CALCULATED);
			assignParameter(pd.Index, pd.ShortName, pd);

			// 8
			pd = new ParamDesc("Value Formatting Information", FormattingInfoIdx, STRING, READ_VALUE_OPTIONAL, CALCULATED);
			assignParameter(pd.Index, pd.ShortName, pd);

			// 9
			pd = new ParamDesc("Global Parameter Name", GlobalParamNameIdx, STRING, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.Index, pd.ShortName, pd);

			// 10
			pd = new ParamDesc("Data Direction Is To This Cell", DataIsToCellIdx, BOOL, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.Index, pd.ShortName, pd);

			// 11
			pd = new ParamDesc("Has Error", HasErrorsIdx, BOOL, READ_VALUE_IGNORE, CALCULATED);
			assignParameter(pd.Index, pd.ShortName, pd);

			// 12
			pd = new ParamDesc("Text", TextIdx, TEXT, READ_VALUE_REQUIRED, READ_FROM_EXCEL);
			assignParameter(pd.Index, pd.ShortName, pd);

			// 13
			pd = new ParamDesc("Cell Data Visible", DataVisible, IGNORE, READ_VALUE_IGNORE, NOT_USED);
			assignParameter(pd.Index, pd.ShortName, pd);

			// 14
			pd = new ParamDesc("Cell Graphic Type", GraphicType, IGNORE, READ_VALUE_IGNORE, NOT_USED);
			assignParameter(pd.Index, pd.ShortName, pd);
		}
	}

}