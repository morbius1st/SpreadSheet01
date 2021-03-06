#region + Using Directives

using System.Collections.Generic;
using System.Diagnostics;
using SpreadSheet01.RevitSupport;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamReadReqmt;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamDataType;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamMode;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamGroup;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamExistReqmt;

using SpreadSheet01.RevitSupport.RevitParamInfo;
#endregion

// user name: jeffs
// created:   3/1/2021 10:42:06 PM

namespace SpreadSheet01.RevitSupport.RevitChartInfo
{

	public class RevitChartParameters
	{
		public static int[] ChartParamCounts = new int[3];
		public static int AllChartParamCount;
		public static int MustExistCount;


		public static readonly int ChartNameIdx                   = ChartParamCounts[(int) DATA]++;
		public static readonly int ChartDescIdx                   = ChartParamCounts[(int) DATA]++;
		public static readonly int ChartSeqIdx                    = ChartParamCounts[(int) DATA]++; // need to add
		public static readonly int ChartFilePathIdx	              = ChartParamCounts[(int) DATA]++; 
		public static readonly int ChartWorkSheetIdx	          = ChartParamCounts[(int) DATA]++;
		public static readonly int ChartFamilyNameIdx	          = ChartParamCounts[(int) DATA]++;
		public static readonly int ChartUpdateTypeIdx	          = ChartParamCounts[(int) DATA]++;

		public static readonly int ChartHasErrorsIdx              = ChartParamCounts[(int) DATA]++;

		public static SortedDictionary<string, int> ChartParamIndex { get; private set; }
		public static List<ParamDesc> ChartAllParams { get; private set; }

		public RevitChartParameters()
		{
			
			assignParameters();
		}

		public static ParamDesc Match(string name)
		{
			string shortName = ParamDesc.GetShortNameSimple(name);

			ParamDesc pd = null;
			int idx;

			bool result = ChartParamIndex.TryGetValue(shortName, out idx);

			if (result)
			{
				pd = ChartAllParams[idx];
			}

			return pd;
		}

		private static void configureChartList()
		{
			ChartAllParams = new List<ParamDesc>();

			for (int i = 0; i < AllChartParamCount; i++)
			{
				ChartAllParams.Add(ParamDesc.Empty);
			}
		}

		private static void assignParameter(int idx, string shortName, ParamDesc pd)
		{
			Debug.WriteLine("  add parameter| (" + idx + ") "+ shortName);

			ChartParamIndex.Add(shortName, idx);
			ChartAllParams[idx] = pd;

			if (pd.Exist == PARAM_MUST_EXIST ) MustExistCount++;
		}


		private static void assignParameters()
		{
			AllChartParamCount = ChartParamCounts[(int) DATA];

			ChartParamIndex= new SortedDictionary<string, int>();
			configureChartList();

			ParamDesc pd;

			// data parameters
			// 0
			pd = new ParamDesc("Name"                  , ChartNameIdx     , 0,    
				DATA, PARAM_MUST_EXIST, TEXT, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex              , pd.ShortName     , pd);
			// 1
			pd = new ParamDesc("Description"           , ChartDescIdx     , 0,    
				DATA, PARAM_OPTIONAL, TEXT, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex              , pd.ShortName     , pd);
			// 2
			pd = new ParamDesc("Sequence"              , ChartSeqIdx      , 0,    
				DATA, PARAM_OPTIONAL, TEXT, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex              , pd.ShortName     , pd);
			// 3
			pd = new ParamDesc("Excel File Path"       , ChartFilePathIdx , 0,    
				DATA, PARAM_MUST_EXIST, FILE_PATH, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex              , pd.ShortName     , pd);
			// 4
			pd = new ParamDesc("Excel WorkSheet Name"  , ChartWorkSheetIdx, 0,    
				DATA, PARAM_MUST_EXIST, TEXT, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex              , pd.ShortName     , pd);
			// 5
			pd = new ParamDesc("Cell Family Name"      , ChartFamilyNameIdx, 0,    
				DATA, PARAM_MUST_EXIST, TEXT, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex              , pd.ShortName     , pd);
			// 6
			pd = new ParamDesc("Update Scheme"         , ChartUpdateTypeIdx, 0,    
				DATA, PARAM_OPTIONAL, UPDATE_TYPE, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex              , pd.ShortName     , pd);
			// 7
			pd = new ParamDesc("Cells With Errors"     , ChartHasErrorsIdx, 0,    
				DATA, PARAM_MUST_EXIST, IGNORE, READ_VALUE_IGNORE, NOT_USED);
			assignParameter(pd.ParamIndex              , pd.ShortName     , pd);


		}
	}
}
