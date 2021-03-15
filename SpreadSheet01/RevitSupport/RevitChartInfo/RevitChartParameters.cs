#region + Using Directives

#endregion

// user name: jeffs
// created:   3/1/2021 10:42:06 PM

namespace SpreadSheet01.RevitSupport
{


/*	public class RevitChartParameters
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
		public static List<ParamDesc2> ChartAllParams { get; private set; }

		public static bool IsConfigured {get; private set;}

		public RevitChartParameters()
		{
			
			assignParameters();
		}

		public static ParamDesc2 Match(string name)
		{
			string shortName = ParamDesc2.GetShortNameSimple(name);

			ParamDesc2 pd = null;
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
			ChartAllParams = new List<ParamDesc2>();

			for (int i = 0; i < AllChartParamCount; i++)
			{
				ChartAllParams.Add(ParamDesc2.Empty);
			}
		}

		private static void assignParameter(int idx, string shortName, ParamDesc2 pd)
		{
			Debug.WriteLine("  add parameter| (" + idx + ") "+ shortName);

			ChartParamIndex.Add(shortName, idx);
			ChartAllParams[idx] = pd;

			if (pd.Exist == PARAM_MUST_EXIST ) MustExistCount++;
		}


		private static void assignParameters()
		{
			if (IsConfigured) return;

			IsConfigured = true;

			MustExistCount = 0;

			AllChartParamCount = ChartParamCounts[(int) DATA];

			ChartParamIndex= new SortedDictionary<string, int>();
			configureChartList();

			ParamDesc2 pd;

			// data parameters
			// 0
			pd = new ParamDesc2("Name"                  , ChartNameIdx     , 0,    
				DATA, PARAM_MUST_EXIST, TEXT, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex              , pd.ShortName     , pd);
			// 1
			pd = new ParamDesc2("Description"           , ChartDescIdx     , 0,    
				DATA, PARAM_OPTIONAL, TEXT, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex              , pd.ShortName     , pd);
			// 2
			pd = new ParamDesc2("Sequence"              , ChartSeqIdx      , 0,    
				DATA, PARAM_OPTIONAL, TEXT, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex              , pd.ShortName     , pd);
			// 3
			pd = new ParamDesc2("Excel File Path"       , ChartFilePathIdx , 0,    
				DATA, PARAM_MUST_EXIST, FILE_PATH, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex              , pd.ShortName     , pd);
			// 4
			pd = new ParamDesc2("Excel WorkSheet Name"  , ChartWorkSheetIdx, 0,    
				DATA, PARAM_MUST_EXIST, TEXT, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex              , pd.ShortName     , pd);
			// 5
			pd = new ParamDesc2("Cell Family Name"      , ChartFamilyNameIdx, 0,    
				DATA, PARAM_MUST_EXIST, TEXT, READ_VALUE_REQUIRED, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex              , pd.ShortName     , pd);
			// 6
			pd = new ParamDesc2("Update Type"         , ChartUpdateTypeIdx, 0,    
				DATA, PARAM_OPTIONAL, UPDATE_TYPE, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER);
			assignParameter(pd.ParamIndex              , pd.ShortName     , pd);
			// 7
			pd = new ParamDesc2("Cells With Errors"     , ChartHasErrorsIdx, 0,    
				DATA, PARAM_MUST_EXIST, BOOL, READ_VALUE_IGNORE, NOT_USED);
			assignParameter(pd.ParamIndex              , pd.ShortName     , pd);


		}
	}
*/
}
