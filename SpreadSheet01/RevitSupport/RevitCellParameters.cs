
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

		public ParamDesc( string paramName, int index,
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

		}

		public string ParameterName { get; private set; }
		public string ShortName => shortName;
		public int Index { get; private set; }
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
		public const int PARAM_IDX = 0;
		public const int PARAM_TYPE = 1;

		public static int DataParamCount = 0;
		public static int LabelParamCount = 0;
		
		public static readonly int NameIdx                   = DataParamCount++; // set - created
		public static readonly int SeqIdx                    = DataParamCount++; // get - read from family
		public static readonly int CellAddrIdx               = DataParamCount++; // get - read from family
		public static readonly int FormattingInfoIdx         = DataParamCount++; // get - read from family
		public static readonly int GraphicType               = DataParamCount++; // ignore
		public static readonly int DataIsToCellIdx           = DataParamCount++; // get - read from family
		public static readonly int HasErrorsIdx              = DataParamCount++; // set - created
		public static readonly int DataVisibleIdx            = DataParamCount++; // ignore
		public static readonly int LabelsIdx                 = DataParamCount++; // the collection of label items

		// related to a label
		public static readonly int LabelIdx                  = LabelParamCount++; // 
		public static readonly int lblRelAddrIdx             = LabelParamCount++; // 
		public static readonly int lblDataTypeIdx            = LabelParamCount++; // 
		public static readonly int lblFormulaIdx             = LabelParamCount++; // 
		public static readonly int lblIgnoreIdx              = LabelParamCount++; // 
		public static readonly int lblAsLengthIdx            = LabelParamCount++; // 
		public static readonly int lblAsNumberIdx            = LabelParamCount++; // 
		public static readonly int lblAsYesNoIdx             = LabelParamCount++; // 


		// public static readonly int TextIdx                   = DataParamCount++; // set - read from excel (special)
		// public static readonly int FormulaIdx                = DataParamCount++; // get - read from family
		// public static readonly int ValueAsNumberIdx          = DataParamCount++; // set - read from excel
		// public static readonly int CalcResultsAsTextIdx      = DataParamCount++; // set - created
		// public static readonly int CalcResultsAsNumberIdx    = DataParamCount++; // set - created
		// public static readonly int GlobalParamNameIdx        = DataParamCount++; // get - read from family

		public static SortedDictionary<string, int> CellParamIndex { get; private set; }
		public static ParamDesc[] CellParams { get; private set; }

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
			CellParams = new ParamDesc[DataParamCount];

			ParamDesc pd;

			// 0
			pd = new ParamDesc("Name", NameIdx, DATA, STRING, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.Index, pd.ShortName, pd);
			// 1
			pd = new ParamDesc("Sequence", SeqIdx, DATA, STRING, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.Index, pd.ShortName, pd);
			// 2
			pd = new ParamDesc("Excel Cell Address", CellAddrIdx, DATA, ADDRESS, READ_VALUE_SET_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.Index, pd.ShortName, pd);
			// 3
			pd = new ParamDesc("Value Formatting Information", FormattingInfoIdx, DATA, STRING, READ_VALUE_OPTIONAL, CALCULATED);
			assignParameter(pd.Index, pd.ShortName, pd);
			// 4
			pd = new ParamDesc("Cell Graphic Type", GraphicType, DATA, IGNORE, READ_VALUE_IGNORE, NOT_USED);
			assignParameter(pd.Index, pd.ShortName, pd);
			// 5
			pd = new ParamDesc("Data Direction Is To This Cell", DataIsToCellIdx, DATA, BOOL, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.Index, pd.ShortName, pd);
			// 6
			pd = new ParamDesc("Has Error", HasErrorsIdx, DATA, BOOL, READ_VALUE_IGNORE, CALCULATED);
			assignParameter(pd.Index, pd.ShortName, pd);
			// 7
			pd = new ParamDesc("Cell Data Visible", DataVisibleIdx, DATA, IGNORE, READ_VALUE_IGNORE, NOT_USED);
			assignParameter(pd.Index, pd.ShortName, pd);
			// 8
			pd = new ParamDesc(null, LabelsIdx, COLLECTION, IGNORE, READ_VALUE_IGNORE, NOT_USED);
			assignParameter(pd.Index, pd.ShortName, pd);




			// A
			pd = new ParamDesc("Label", LabelIdx, LABEL, STRING, READ_VALUE_REQUIRED, READ_FROM_EXCEL);
			assignParameter(pd.Index, pd.ShortName, pd);

			// B
			pd = new ParamDesc("Relative Address", lblRelAddrIdx, LABEL, RELATIVEADDRESS, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.Index, pd.ShortName, pd);

			// C
			pd = new ParamDesc("Data Type", lblDataTypeIdx, LABEL, DATATYPE, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.Index, pd.ShortName, pd);

			// D
			pd = new ParamDesc("Formula", lblFormulaIdx, LABEL, STRING, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.Index, pd.ShortName, pd);

			// E
			pd = new ParamDesc("Ignore", lblIgnoreIdx, LABEL, BOOL, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.Index, pd.ShortName, pd);

			// F
			pd = new ParamDesc("As Length", lblAsLengthIdx, LABEL, NUMBER, READ_VALUE_IGNORE, CALCULATED);
			assignParameter(pd.Index, pd.ShortName, pd);

			// G
			pd = new ParamDesc("As Number", lblAsNumberIdx, LABEL, NUMBER, READ_VALUE_IGNORE, CALCULATED);
			assignParameter(pd.Index, pd.ShortName, pd);

			// H
			pd = new ParamDesc("As Yes-No", lblAsYesNoIdx, LABEL, NUMBER, READ_VALUE_IGNORE, CALCULATED);
			assignParameter(pd.Index, pd.ShortName, pd);


			// // 12
			// pd = new ParamDesc("Text", TextIdx, TEXT, READ_VALUE_REQUIRED, READ_FROM_EXCEL, INFORMATION);
			// assignParameter(pd.Index, pd.ShortName, pd);
			//
			// // 4
			// pd = new ParamDesc("Formula", FormulaIdx, STRING, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER, INFORMATION);
			// assignParameter(pd.Index, pd.ShortName, pd);
			//
			// // 5
			// pd = new ParamDesc("Value as Number", ValueAsNumberIdx, NUMBER, READ_VALUE_REQD_IF_NUMBER, READ_FROM_EXCEL, INFORMATION);
			// assignParameter(pd.Index, pd.ShortName, pd);
			//
			// // 6
			// pd = new ParamDesc("Calculation Results Text", CalcResultsAsTextIdx, STRING, READ_VALUE_IGNORE, READ_FROM_PARAMETER, INFORMATION);
			// assignParameter(pd.Index, pd.ShortName, pd);
			//
			// // 7
			// pd = new ParamDesc("Calculation Results Number", CalcResultsAsNumberIdx, NUMBER, READ_VALUE_IGNORE, CALCULATED, INFORMATION);
			// assignParameter(pd.Index, pd.ShortName, pd);
			// // 9
			// pd = new ParamDesc("Global Parameter Name", GlobalParamNameIdx, STRING, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER, INFORMATION);
			// assignParameter(pd.Index, pd.ShortName, pd);

		}
	}

}