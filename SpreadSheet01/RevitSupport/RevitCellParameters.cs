using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamReadReqmt;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamDataType;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamMode;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamGroup;

// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitCellParameters.cs
// Created:      2021-02-20 (5:36 AM)


namespace SpreadSheet01.RevitSupport
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
			return name.Substring(0, Math.Min(name.Length, 6));
		}
		
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
			else if (pos1 > 0)
			{
				test = name.Substring(0, pos1 - 1);
			}

			return name.Substring(0, Math.Min(name.Length, 6));
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
		public static readonly int NameIdx                   = ParamCounts[(int) DATA]++; // set - created
		public static readonly int SeqIdx                    = ParamCounts[(int) DATA]++; // get - read from family
		public static readonly int CellAddrIdx               = ParamCounts[(int) DATA]++; // get - read from family
		public static readonly int FormattingInfoIdx         = ParamCounts[(int) DATA]++; // get - read from family
		public static readonly int GraphicType               = ParamCounts[(int) DATA]++; // ignore
		public static readonly int DataIsToCellIdx           = ParamCounts[(int) DATA]++; // get - read from family
		public static readonly int HasErrorsIdx              = ParamCounts[(int) DATA]++; // set - created
		public static readonly int DataVisibleIdx            = ParamCounts[(int) DATA]++; // ignore
		public static readonly int LabelsIdx                 = ParamCounts[(int) DATA]++; // ignore

		// public static readonly int
			// LabelsIdx                 = ParamCounts[(int) DATA]; // the collection of label items

		// related to a label

		// public static int CollectGroupCount = 0;

		public static readonly int LabelIdx                  = ParamCounts[(int) LABEL]++; // 

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
			int adj2 = adj1 + ParamCounts[(int) CONTAINER];

			AllParamDefCount = adj2 + ParamCounts[(int) LABEL];


			AllParamCount = AllParamDefCount + ParamCounts[(int) LABEL];

			CellParamIndex = new SortedDictionary<string, int>();

#pragma warning disable CS0219 // The variable 'd' is assigned but its value is never used
			int d = (int) DATA;
#pragma warning restore CS0219 // The variable 'd' is assigned but its value is never used
			int x = ParamCounts[(int) DATA];

			// CellAllParams = new ParamDesc[ParamCounts[(int) DATA]];
			CellAllParams = new ParamDesc[AllParamCount];

			ParamDesc pd;

			// data parameters
			// 0
			pd = new ParamDesc("Name"                          , NameIdx          , 0,    DATA, TEXT, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 1
			pd = new ParamDesc("Sequence"                      , SeqIdx           , 0,    DATA, TEXT, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 2
			pd = new ParamDesc("Excel Cell Address"            , CellAddrIdx      , 0,    DATA, ADDRESS, READ_VALUE_SET_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 3
			pd = new ParamDesc("Value Formatting Information"  , FormattingInfoIdx, 0,    DATA, TEXT, READ_VALUE_OPTIONAL, CALCULATED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 4
			pd = new ParamDesc("Cell Graphic Type"             , GraphicType      , 0,    DATA, IGNORE, READ_VALUE_IGNORE, NOT_USED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 5
			pd = new ParamDesc("Data Direction Is To This Cell", DataIsToCellIdx  , 0,    DATA, BOOL, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 6
			pd = new ParamDesc("Has Error"                     , HasErrorsIdx     , 0,    DATA, BOOL, READ_VALUE_IGNORE, CALCULATED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 7
			pd = new ParamDesc("Cell Data Visible"             , DataVisibleIdx   , 0,    DATA, IGNORE, READ_VALUE_IGNORE, NOT_USED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);


			// // 8 - this parameter holds all of the labels which holds all of the label parameters
			pd = new ParamDesc("Labels"                        , LabelsIdx        , 0,    CONTAINER, IGNORE, READ_VALUE_IGNORE, NOT_USED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);

			// A
			pd = new ParamDesc("Label"                         , LabelIdx         , adj2, LABEL, LABEL_TITLE, READ_VALUE_REQUIRED, READ_FROM_EXCEL);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);

			// the parameters associated with a label
			// A
			pd = new ParamDesc("Relative Address"              , lblRelAddrIdx    , adj2, LABEL, RELATIVEADDRESS, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// B
			pd = new ParamDesc("Data Type"                     , lblDataTypeIdx   , adj2, LABEL, DATATYPE, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// C
			pd = new ParamDesc("Formula"                       , lblFormulaIdx    , adj2, LABEL, FORMULA, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// D
			pd = new ParamDesc("Ignore"                        , lblIgnoreIdx     , adj2, LABEL, BOOL, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// E
			pd = new ParamDesc("As Length"                     , lblAsLengthIdx   , adj2, LABEL, NUMBER, READ_VALUE_IGNORE, CALCULATED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// F
			pd = new ParamDesc("As Number"                     , lblAsNumberIdx   , adj2, LABEL, NUMBER, READ_VALUE_IGNORE, CALCULATED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// G
			pd = new ParamDesc("As Yes-No"                     , lblAsYesNoIdx    , adj2, LABEL, NUMBER, READ_VALUE_IGNORE, CALCULATED);
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