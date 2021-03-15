

// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitCellParameters.cs
// Created:      2021-02-20 (5:36 AM)


namespace SpreadSheet01.RevitSupport
{

/*
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
		public static readonly int DataIsToCellIdx           = ParamCounts[(int) DATA]++; // get - read from family
		public static readonly int HasErrorsIdx              = ParamCounts[(int) DATA]++; // set - created
		public static readonly int LabelsIdx                 = ParamCounts[(int) DATA]++; // ignore

		// public static readonly int DataVisibleIdx            = ParamCounts[(int) DATA]++; // ignore
		// public static readonly int GraphicType               = ParamCounts[(int) DATA]++; // ignore

		// public static readonly int
			// LabelsIdx                 = ParamCounts[(int) DATA]; // the collection of label items

		// related to a label

		// public static int CollectGroupCount = 0;

		public static readonly int LabelIdx                  = ParamCounts[(int) LABEL_GRP]++; // 

		// public static int LabelParamCount = 0;

		public static readonly int lblRelAddrIdx             = ParamCounts[(int) LABEL_GRP]++; // 
		public static readonly int lblDataTypeIdx            = ParamCounts[(int) LABEL_GRP]++; // 
		public static readonly int lblFormulaIdx             = ParamCounts[(int) LABEL_GRP]++; // 
		public static readonly int lblIgnoreIdx              = ParamCounts[(int) LABEL_GRP]++; // 
		public static readonly int lblAsLengthIdx            = ParamCounts[(int) LABEL_GRP]++; // 
		public static readonly int lblAsNumberIdx            = ParamCounts[(int) LABEL_GRP]++; // 
		public static readonly int lblAsYesNoIdx             = ParamCounts[(int) LABEL_GRP]++; // 


		// public static readonly int TextIdx                   = DataParamCount++; // set - read from excel (special)
		// public static readonly int FormulaIdx                = DataParamCount++; // get - read from family
		// public static readonly int ValueAsNumberIdx          = DataParamCount++; // set - read from excel
		// public static readonly int CalcResultsAsTextIdx      = DataParamCount++; // set - created
		// public static readonly int CalcResultsAsNumberIdx    = DataParamCount++; // set - created
		// public static readonly int GlobalParamNameIdx        = DataParamCount++; // get - read from family

		public static SortedDictionary<string, int> CellParamIndex { get; private set; }
		public static ParamDesc2[] CellAllParams { get; private set; }

		static RevitCellParameters()
		{
			assignParameters();
			listParameterDefs();
		}

		public static ParamDesc2 Match(string name)
		{
			string shortName = ParamDesc2.GetShortNameSimple(name);

			ParamDesc2 pd = null;
			int idx;

			bool result = CellParamIndex.TryGetValue(shortName, out idx);

			if (result)
			{
				pd = CellAllParams[idx];
			}

			return pd;
		}

		private static void assignParameter(int idx, string shortName, ParamDesc2 pd)
		{
			Console.WriteLine("  add parameter| (" + idx + ") "+ shortName);

			CellParamIndex.Add(shortName, idx);
			CellAllParams[idx] = pd;
		}

		private static void assignParameters()
		{
			int adj1 = ParamCounts[(int) DATA];
			int adj2 = adj1 + ParamCounts[(int) CONTAINER];

			AllParamDefCount = adj2 + ParamCounts[(int) LABEL_GRP];


			AllParamCount = AllParamDefCount + ParamCounts[(int) LABEL_GRP];

			CellParamIndex = new SortedDictionary<string, int>();

			// CellAllParams = new ParamDesc[ParamCounts[(int) DATA]];
			CellAllParams = new ParamDesc2[AllParamCount];

			ParamDesc2 pd;

			// data parameters
			// 0
			pd = new ParamDesc2("Name"                          , NameIdx          , 0,    
				DATA, PARAM_MUST_EXIST, TEXT, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 1
			pd = new ParamDesc2("Sequence"                      , SeqIdx           , 0,    
				DATA, PARAM_OPTIONAL, TEXT, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 2
			pd = new ParamDesc2("Excel Cell Address"            , CellAddrIdx      , 0,    
				DATA, PARAM_MUST_EXIST, ADDRESS, READ_VALUE_SET_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 3
			pd = new ParamDesc2("Value Formatting Information"  , FormattingInfoIdx, 0,    
				DATA, PARAM_OPTIONAL, TEXT, READ_VALUE_OPTIONAL, CALCULATED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 5
			pd = new ParamDesc2("Data Direction Is To This Cell", DataIsToCellIdx  , 0,    
				DATA, PARAM_OPTIONAL, BOOL, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// 6
			pd = new ParamDesc2("Has Error"                     , HasErrorsIdx     , 0,    
				DATA, PARAM_MUST_EXIST, BOOL, READ_VALUE_IGNORE, CALCULATED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);


			// // 4
			// pd = new ParamDesc("Cell Graphic Type"             , GraphicType      , 0,    
			// 	DATA, PARAM_OPTIONAL, IGNORE, READ_VALUE_IGNORE, NOT_USED);
			// assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// // 7
			// pd = new ParamDesc("Cell Data Visible"             , DataVisibleIdx   , 0,    
			// 	DATA, PARAM_OPTIONAL, IGNORE, READ_VALUE_IGNORE, NOT_USED);
			// assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);




			// // 8 - this parameter holds all of the labels which holds all of the label parameters
			pd = new ParamDesc2("Labels"                        , LabelsIdx        , 0,    
				CONTAINER, PARAM_MUST_EXIST, IGNORE, READ_VALUE_IGNORE, NOT_USED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);



			// A
			pd = new ParamDesc2("Label"                         , LabelIdx         , adj2, 
				LABEL_GRP, 
				PARAM_MUST_EXIST, LABEL_TITLE, READ_VALUE_REQUIRED, READ_FROM_EXCEL);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);

			// the parameters associated with a label
			// A
			pd = new ParamDesc2("Relative Address"              , lblRelAddrIdx    , adj2, 
				LABEL_GRP, 
				PARAM_MUST_EXIST, RELATIVEADDRESS, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// B
			// C
			pd = new ParamDesc2("Formula"                       , lblFormulaIdx    , adj2, 
				LABEL_GRP, 
				PARAM_OPTIONAL, FORMULA, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			pd = new ParamDesc2("Data Type"                     , lblDataTypeIdx   , adj2, 
				LABEL_GRP, 
				PARAM_MUST_EXIST, DATATYPE, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// D
			pd = new ParamDesc2("Ignore"                        , lblIgnoreIdx     , adj2, 
				LABEL_GRP, 
				PARAM_OPTIONAL, BOOL, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// E
			pd = new ParamDesc2("As Length"                     , lblAsLengthIdx   , adj2, 
				LABEL_GRP, 
				PARAM_OPTIONAL, NUMBER, READ_VALUE_IGNORE, CALCULATED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// F
			pd = new ParamDesc2("As Number"                     , lblAsNumberIdx   , adj2, 
				LABEL_GRP, 
				PARAM_OPTIONAL, NUMBER, READ_VALUE_IGNORE, CALCULATED);
			assignParameter(pd.ParamIndex                      , pd.ShortName     , pd);
			// G
			pd = new ParamDesc2("As Yes-No"                     , lblAsYesNoIdx    , adj2, 
				LABEL_GRP, 
				PARAM_OPTIONAL, NUMBER, READ_VALUE_IGNORE, CALCULATED);
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
	*/
}