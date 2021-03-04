using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpreadSheet01.RevitSupport.RevitParamValue;
using UtilityLibrary;

// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitCellParameters.cs
// Created:      2021-02-20 (5:36 AM)


namespace SpreadSheet01.RevitSupport.RevitParamInfo
{
	public class ParamDesc : INotifyPropertyChanged
	{
		private const string TEXT_SHORT_NAME = "Text";
		private string parameterName;

		private string shortName;

		public ParamDesc(string paramName, int index,
			int indexAdjust,
			ParamGroup paramGroup,
			ParamDataType dataType,
			ParamReadReqmt paramReadReqmt,
			ParamMode paramMode)
		{
			Index = index;
			ParameterName = paramName;
			shortName = GetShortName(paramName);

			DataType = dataType;
			ReadReqmt = paramReadReqmt;
			Group = paramGroup;
			Mode = paramMode;
			ParamIndex = index + indexAdjust;
		}

		public static ParamDesc Empty => new ParamDesc("", -1, -1, ParamGroup.DATA, ParamDataType.IGNORE, ParamReadReqmt.READ_VALUE_IGNORE, ParamMode.NOT_USED);

		public string ParameterName {
			get => parameterName;

			private set
			{
				parameterName = value;
				OnPropertyChanged();
			}
		}

		public string ShortName => shortName;
		public int Index { get; private set; }

		public int ParamIndex { get; private set; }


		public ParamDataType DataType { get; private set; }
		public ParamReadReqmt ReadReqmt { get; private set; }
		public ParamMode Mode { get; private set; }
		public ParamGroup Group { get; private set; }

		public static string GetShortNameSimple(string name)
		{
			return name.Substring(0, Math.Min(name.Length, 8));
		}
		
		public static string GetShortName(string name)
		{
			if (name.IsVoid()) return "";
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
			else if (pos1 > 0)
			{
				test = name.Substring(0, pos1 - 1);
			}

			return name.Substring(0, Math.Min(name.Length, 8));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return Index.ToString("##0") + " <|> "+ DataType.ToString() + " <|> "+ ShortName + " <|> " + ParameterName;
		}
	}

	public class RevitCellParameters
	{
		// public const int PARAM_IDX = 0;
		// public const int PARAM_TYPE = 1;

		public static int[] ParamCounts = new int[3];
		public static int AllParamDefCount;
		public static int AllParamCount;

		// public static int DataParamCount = 0;
		//
		public static readonly int NameIdx                   = ParamCounts[(int) ParamGroup.DATA]++; // set - created
		public static readonly int SeqIdx                    = ParamCounts[(int) ParamGroup.DATA]++; // get - read from family
		public static readonly int CellAddrIdx               = ParamCounts[(int) ParamGroup.DATA]++; // get - read from family
		public static readonly int FormattingInfoIdx         = ParamCounts[(int) ParamGroup.DATA]++; // get - read from family
		public static readonly int GraphicType               = ParamCounts[(int) ParamGroup.DATA]++; // ignore
		public static readonly int DataIsToCellIdx           = ParamCounts[(int) ParamGroup.DATA]++; // get - read from family
		public static readonly int HasErrorsIdx              = ParamCounts[(int) ParamGroup.DATA]++; // set - created
		public static readonly int DataVisibleIdx            = ParamCounts[(int) ParamGroup.DATA]++; // ignore
		public static readonly int LabelsIdx                 = ParamCounts[(int) ParamGroup.DATA]++; // ignore

		// public static readonly int
			// LabelsIdx                 = ParamCounts[(int) DATA]; // the collection of label items

		// related to a label

		// public static int CollectGroupCount = 0;

		public static readonly int LabelIdx                  = ParamCounts[(int) ParamGroup.LABEL]++; // 

		// public static int LabelParamCount = 0;

		public static readonly int lblRelAddrIdx             = ParamCounts[(int) ParamGroup.LABEL]++; // 
		public static readonly int lblDataTypeIdx            = ParamCounts[(int) ParamGroup.LABEL]++; // 
		public static readonly int lblFormulaIdx             = ParamCounts[(int) ParamGroup.LABEL]++; // 
		public static readonly int lblIgnoreIdx              = ParamCounts[(int) ParamGroup.LABEL]++; // 
		public static readonly int lblAsLengthIdx            = ParamCounts[(int) ParamGroup.LABEL]++; // 
		public static readonly int lblAsNumberIdx            = ParamCounts[(int) ParamGroup.LABEL]++; // 
		public static readonly int lblAsYesNoIdx             = ParamCounts[(int) ParamGroup.LABEL]++; // 


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
			listParameterDefs();
		}

		public static ParamDesc Match(string name)
		{
			string shortName = ParamDesc.GetShortNameSimple(name);

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
			Console.WriteLine("  add parameter| (" + idx + ") "+ shortName);

			CellParamIndex.Add(shortName, idx);
			CellAllParams[idx] = pd;
		}

		private static void assignParameters()
		{
			int adj1 = ParamCounts[(int) ParamGroup.DATA];
			int adj2 = adj1 + ParamCounts[(int) ParamGroup.CONTAINER];

			AllParamDefCount = adj2 + ParamCounts[(int) ParamGroup.LABEL];


			AllParamCount = AllParamDefCount + ParamCounts[(int) ParamGroup.LABEL];

			CellParamIndex = new SortedDictionary<string, int>();

			// CellAllParams = new ParamDesc[ParamCounts[(int) DATA]];
			CellAllParams = new ParamDesc[AllParamCount];

			ParamDesc pd;

			// data parameters
			// 0
			pd = new ParamDesc("Name"                          , NameIdx          , 0,    ParamGroup.DATA, ParamDataType.TEXT, ParamReadReqmt.READ_VALUE_REQUIRED, ParamMode.READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 1
			pd = new ParamDesc("Sequence"                      , SeqIdx           , 0,    ParamGroup.DATA, ParamDataType.TEXT, ParamReadReqmt.READ_VALUE_OPTIONAL, ParamMode.READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 2
			pd = new ParamDesc("Excel Cell Address"            , CellAddrIdx      , 0,    ParamGroup.DATA, ParamDataType.ADDRESS, ParamReadReqmt.READ_VALUE_SET_REQUIRED, ParamMode.READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 3
			pd = new ParamDesc("Value Formatting Information"  , FormattingInfoIdx, 0,    ParamGroup.DATA, ParamDataType.TEXT, ParamReadReqmt.READ_VALUE_OPTIONAL, ParamMode.CALCULATED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 4
			pd = new ParamDesc("Cell Graphic Type"             , GraphicType      , 0,    ParamGroup.DATA, ParamDataType.IGNORE, ParamReadReqmt.READ_VALUE_IGNORE, ParamMode.NOT_USED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 5
			pd = new ParamDesc("Data Direction Is To This Cell", DataIsToCellIdx  , 0,    ParamGroup.DATA, ParamDataType.BOOL, ParamReadReqmt.READ_VALUE_OPTIONAL, ParamMode.READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 6
			pd = new ParamDesc("Has Error"                     , HasErrorsIdx     , 0,    ParamGroup.DATA, ParamDataType.BOOL, ParamReadReqmt.READ_VALUE_IGNORE, ParamMode.CALCULATED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 7
			pd = new ParamDesc("Cell Data Visible"             , DataVisibleIdx   , 0,    ParamGroup.DATA, ParamDataType.IGNORE, ParamReadReqmt.READ_VALUE_IGNORE, ParamMode.NOT_USED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);


			// // 8 - this parameter holds all of the labels which holds all of the label parameters
			pd = new ParamDesc("Labels"                        , LabelsIdx        , 0,    ParamGroup.CONTAINER, ParamDataType.IGNORE, ParamReadReqmt.READ_VALUE_IGNORE, ParamMode.NOT_USED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);

			// A
			pd = new ParamDesc("Label"                         , LabelIdx         , adj2, ParamGroup.LABEL, ParamDataType.LABEL_TITLE, ParamReadReqmt.READ_VALUE_REQUIRED, ParamMode.READ_FROM_EXCEL);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);

			// the parameters associated with a label
			// A
			pd = new ParamDesc("Relative Address"              , lblRelAddrIdx    , adj2, ParamGroup.LABEL, ParamDataType.RELATIVEADDRESS, ParamReadReqmt.READ_VALUE_OPTIONAL, ParamMode.READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// B
			pd = new ParamDesc("Data Type"                     , lblDataTypeIdx   , adj2, ParamGroup.LABEL, ParamDataType.DATATYPE, ParamReadReqmt.READ_VALUE_OPTIONAL, ParamMode.READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// C
			pd = new ParamDesc("Formula"                       , lblFormulaIdx    , adj2, ParamGroup.LABEL, ParamDataType.FORMULA, ParamReadReqmt.READ_VALUE_OPTIONAL, ParamMode.READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// D
			pd = new ParamDesc("Ignore"                        , lblIgnoreIdx     , adj2, ParamGroup.LABEL, ParamDataType.BOOL, ParamReadReqmt.READ_VALUE_REQUIRED, ParamMode.READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// E
			pd = new ParamDesc("As Length"                     , lblAsLengthIdx   , adj2, ParamGroup.LABEL, ParamDataType.NUMBER, ParamReadReqmt.READ_VALUE_IGNORE, ParamMode.CALCULATED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// F
			pd = new ParamDesc("As Number"                     , lblAsNumberIdx   , adj2, ParamGroup.LABEL, ParamDataType.NUMBER, ParamReadReqmt.READ_VALUE_IGNORE, ParamMode.CALCULATED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// G
			pd = new ParamDesc("As Yes-No"                     , lblAsYesNoIdx    , adj2, ParamGroup.LABEL, ParamDataType.NUMBER, ParamReadReqmt.READ_VALUE_IGNORE, ParamMode.CALCULATED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
		}

		private static void listParameterDefs()
		{
			Console.WriteLine("");
			Console.WriteLine("list parameter definitions start\n");
			Console.WriteLine("all count| " + AllParamCount);
			Console.WriteLine("def count| " + AllParamDefCount);
			Console.WriteLine("");

			for (var i = 0; i < AllParamDefCount; i++)
			{
				Console.Write("  idx| " + CellAllParams[i].Index.ToString("##0") + "| ");
				Console.Write("grp| " + CellAllParams[i].Group.ToString().PadRight(7) + "| ");
				Console.Write("typ| " + CellAllParams[i].DataType.ToString().PadRight(16) + "| ");
				Console.Write("mod| " + CellAllParams[i].Mode.ToString().PadRight(20) + "| ");
				Console.Write("shn| " + CellAllParams[i].ShortName.ToString().PadRight(8) + "| ");
				Console.WriteLine("nam| " + CellAllParams[i].ParameterName);

			}

			Console.WriteLine("list parameters definitions done\n");
		}
	}
}