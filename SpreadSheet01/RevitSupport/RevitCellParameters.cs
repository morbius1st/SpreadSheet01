
using System;
using System.Collections.Generic;

using static SpreadSheet01.RevitSupport.ParamReadReqmt;
using static SpreadSheet01.RevitSupport.ParamDataType;
using static SpreadSheet01.RevitSupport.ParamMode;
using static SpreadSheet01.RevitSupport.ParamGroupType;

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

		public ParamDesc(string paramName, int index,
			int indexAdjust,
			ParamGroupType paramGroupType,
			ParamDataType dataType,
			ParamReadReqmt paramReadReqmt,
			ParamMode paramMode)
		{
			Index = index;
			ParameterName = paramName;
			shortName = GetShortName(paramName);

			DataType = dataType;
			ReadReqmt = paramReadReqmt;
			GroupType = paramGroupType;
			Mode = paramMode;
			ParamIndex = index + indexAdjust;

		}

		public string ParameterName { get; private set; }
		public string ShortName => shortName;
		public int Index { get; private set; }

		public int ParamIndex { get; private set; }


		public ParamDataType DataType { get; private set; }
		public ParamReadReqmt ReadReqmt { get; private set; }
		public ParamMode Mode { get; private set; }
		public ParamGroupType GroupType { get; private set; }



		public static string GetShortName(string name)
		{
			string test = name;

			int pos1 = name.IndexOf('#');
			int pos2 = -1;

			if (pos1 == 0 )
			{
				pos2 = name.IndexOf(' ');

				if (pos2 > pos1 + 1)
				{
					test = name.Substring(pos2 + 1).Trim();
				}
			} 
			else if (pos1 == 6)
			{
				test = name.Substring(0, 5);
			}

			return name.Substring(0, Math.Min(name.Length, 9));
		}
	}

	public class RevitCellParameters
	{
		// public const int PARAM_IDX = 0;
		// public const int PARAM_TYPE = 1;

		public static int[] ParamCounts = new int[3];
		public static int AllParamCount;

		// public static int DataParamCount = 0;
		//
		public static readonly int NameIdx                   = ParamCounts[(int) DATA]++; // set - created
		public static readonly int SeqIdx                    = ParamCounts[(int) DATA]++; // get - read from family
		public static readonly int CellAddrIdx               = ParamCounts[(int) DATA]++; // get - read from family
		public static readonly int FormattingInfoIdx         = ParamCounts[(int) DATA]++; // get - read from family
		public static readonly int GraphicType               = ParamCounts[(int) DATA]++; // ignore
		public static readonly int DataIsToCellIdx           = ParamCounts[(int) DATA]++; // get - read from family
		public static readonly int HasErrorsIdx              = ParamCounts[(int) DATA]++; // set - created
		public static readonly int DataVisibleIdx            = ParamCounts[(int) DATA]++; // ignore
		public static readonly int LabelsIdx                 = ParamCounts[(int) DATA]++; // the collection of label items

		// related to a label

		// public static int CollectGroupCount = 0;

		public static readonly int LabelIdx                  = ParamCounts[(int) COLLECTION]++; // 

		// public static int LabelParamCount = 0;

		public static readonly int lblRelAddrIdx             = ParamCounts[(int) LABEL]++; // 
		public static readonly int lblDataTypeIdx            = ParamCounts[(int) LABEL]++; // 
		public static readonly int lblFormulaIdx             = ParamCounts[(int) LABEL]++; // 
		public static readonly int lblIgnoreIdx              = ParamCounts[(int) LABEL]++; // 
		public static readonly int lblAsLengthIdx            = ParamCounts[(int) LABEL]++; // 
		public static readonly int lblAsNumberIdx            = ParamCounts[(int) LABEL]++; // 
		public static readonly int lblAsYesNoIdx             = ParamCounts[(int) LABEL]++; // 


		// public static readonly int TextIdx                   = DataParamCount++; // set - read from excel (special)
		// public static readonly int FormulaIdx                = DataParamCount++; // get - read from family
		// public static readonly int ValueAsNumberIdx          = DataParamCount++; // set - read from excel
		// public static readonly int CalcResultsAsTextIdx      = DataParamCount++; // set - created
		// public static readonly int CalcResultsAsNumberIdx    = DataParamCount++; // set - created
		// public static readonly int GlobalParamNameIdx        = DataParamCount++; // get - read from family

		public static SortedDictionary<string, int> CellParamIndex { get; private set; }
		public static ParamDesc[] CellAllParams { get; private set; }

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
				pd = CellAllParams[idx];
			}

			return pd;
		}

		private static void assignParameter(int idx, string shortName, ParamDesc pd)
		{
			CellParamIndex.Add(shortName, idx);
			CellAllParams[idx] = pd;
		}

		private static void assignParameters()
		{
			int adj1 = ParamCounts[(int) ParamGroupType.DATA];
			int adj2 = adj1 + ParamCounts[(int) COLLECTION];
			AllParamCount = adj2 + ParamCounts[(int) LABEL];

			CellParamIndex = new SortedDictionary<string, int>();
			CellAllParams = new ParamDesc[ParamCounts[(int) DATA]];

			ParamDesc pd;

			// data parameters
			// 0
			pd = new ParamDesc("Name", NameIdx, 0, DATA, TEXT, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex, pd.ShortName, pd);
			// 1
			pd = new ParamDesc("Sequence", SeqIdx, 0, DATA, TEXT, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex, pd.ShortName, pd);
			// 2
			pd = new ParamDesc("Excel Cell Address", CellAddrIdx, 0, DATA, ADDRESS, READ_VALUE_SET_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex, pd.ShortName, pd);
			// 3
			pd = new ParamDesc("Value Formatting Information", FormattingInfoIdx, 0, DATA, TEXT, READ_VALUE_OPTIONAL, CALCULATED);
			assignParameter(pd.ParamIndex, pd.ShortName, pd);
			// 4
			pd = new ParamDesc("Cell Graphic Type", GraphicType, 0, DATA, IGNORE, READ_VALUE_IGNORE, NOT_USED);
			assignParameter(pd.ParamIndex, pd.ShortName, pd);
			// 5
			pd = new ParamDesc("Data Direction Is To This Cell", DataIsToCellIdx, 0, DATA, BOOL, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex, pd.ShortName, pd);
			// 6
			pd = new ParamDesc("Has Error", HasErrorsIdx, 0, DATA, BOOL, READ_VALUE_IGNORE, CALCULATED);
			assignParameter(pd.ParamIndex, pd.ShortName, pd);
			// 7
			pd = new ParamDesc("Cell Data Visible", DataVisibleIdx, 0, DATA, IGNORE, READ_VALUE_IGNORE, NOT_USED);
			assignParameter(pd.ParamIndex, pd.ShortName, pd);
			// 8 - this parameter holds all of the labels which holds all of the label parameters
			pd = new ParamDesc(null, LabelsIdx, 0, COLLECTION, GROUP, READ_VALUE_IGNORE, NOT_USED);
			assignParameter(pd.ParamIndex, pd.ShortName, pd);



			
			pd = new ParamDesc("Label", LabelIdx, adj1, LABEL, TEXT, READ_VALUE_REQUIRED, READ_FROM_EXCEL);
			assignParameter(pd.ParamIndex, pd.ShortName, pd);

			// the parameters associated with a label
			// A
			pd = new ParamDesc("Relative Address", lblRelAddrIdx, adj2, LABEL, RELATIVEADDRESS, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex, pd.ShortName, pd);
			// B
			pd = new ParamDesc("Data Type", lblDataTypeIdx, adj2, LABEL, DATATYPE, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex, pd.ShortName, pd);
			// C
			pd = new ParamDesc("Formula", lblFormulaIdx, adj2, LABEL, TEXT, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex, pd.ShortName, pd);
			// D
			pd = new ParamDesc("Ignore", lblIgnoreIdx, adj2, LABEL, BOOL, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex, pd.ShortName, pd);
			// E
			pd = new ParamDesc("As Length", lblAsLengthIdx, adj2, LABEL, NUMBER, READ_VALUE_IGNORE, CALCULATED);
			assignParameter(pd.ParamIndex, pd.ShortName, pd);
			// F
			pd = new ParamDesc("As Number", lblAsNumberIdx, adj2, LABEL, NUMBER, READ_VALUE_IGNORE, CALCULATED);
			assignParameter(pd.ParamIndex, pd.ShortName, pd);
			// G
			pd = new ParamDesc("As Yes-No", lblAsYesNoIdx, adj2, LABEL, NUMBER, READ_VALUE_IGNORE, CALCULATED);
			assignParameter(pd.ParamIndex, pd.ShortName, pd);

		}
	}

}