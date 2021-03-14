#region using

using SpreadSheet01.RevitSupport.RevitParamInfo;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamReadReqmt;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamDataType;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamMode;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamType;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamExistReqmt;
using static SpreadSheet01.RevitSupport.RevitParamInfo.ParamCat;
using static SpreadSheet01.RevitSupport.RevitParamInfo.ParamSubCat;

#endregion

// username: jeffs
// created:  3/3/2021 7:51:06 PM

namespace SpreadSheet01.RevitSupport.RevitCellsManagement
{

	public class RevitParamManager
	{
	#region private fields

		public static string CELL_FAMILY_NAME = "CellData";
		public static string CHART_FAMILY_NAME = RevitCellSystManager.RevitChartFamilyName;

		private static Families families = new Families();

		private static bool isConfigured;

	#endregion

	#region ctor

		static RevitParamManager()
		{
			defineChartParameters();
			defineCellBasicParameters();
		}

	#endregion

	#region public properties

		public static int MustExistChartInstance => ChartParams.MustExistCount(INSTANCE);
		public static int MustExistCellInstance => CellParams.MustExistCount(INSTANCE);

		public static bool IsConfigured => isConfigured;

		public static ChartFamily ChartParams => 
			(ChartFamily) families.FamilyTypes[CHART_FAMILY_NAME];

		public static CellFamily CellParams => 
			(CellFamily) families.FamilyTypes[CELL_FAMILY_NAME];


		public static int ChartMustExist = 0;
		public static int CellMustExist = 0;

		public static int CommonParameters		        = 0;

		// common indices
		public static readonly int NameIdx				= CommonParameters++;
		public static readonly int SeqIdx				= CommonParameters++;

		public static int AllChartParamCount			= CommonParameters;

		// chart indices
		public static readonly int ChartFilePathIdx		= AllChartParamCount++;
		public static readonly int ChartWorkSheetIdx	= AllChartParamCount++;
		public static readonly int ChartFamilyNameIdx	= AllChartParamCount++;
		public static readonly int ChartUpdateTypeIdx	= AllChartParamCount++;
		public static readonly int ChartHasErrorsIdx	= AllChartParamCount++;
		// end of list

		public static int CellBasicParamCount			= CommonParameters;

		// cell indices (basic)
		public static readonly int HasErrorsIdx         = CellBasicParamCount++;
		// public static readonly int LabelsIdx            = CellBasicParamCount++;
		// end of list

		public static int CellLabelParamCount			= 0;

		// cell label indices (label)
		public static readonly int LblLabelIdx          = CellLabelParamCount++;
		public static readonly int LblNameIdx           = CellLabelParamCount++;
		public static readonly int LblFormulaIdx        = CellLabelParamCount++;
		public static readonly int LblDataTypeIdx       = CellLabelParamCount++;
		public static readonly int LblFormatInfoIdx     = CellLabelParamCount++;
		public static readonly int LblIgnoreIdx         = CellLabelParamCount++;
		// public static readonly int LblAsLengthIdx       = CellLabelParamCount++;
		// public static readonly int LblAsNumberIdx       = CellLabelParamCount++; 
		// public static readonly int LblAsYesNoIdx        = CellLabelParamCount++; 

		// public static int AllCellParamCount				= CellBasicParamCount + CellLabelParamCount;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static ParamDesc ChartInstParam(int idx)
		{
			return ChartParams.InstanceParams[idx];
		}

		public static ParamDesc MatchChartInstance(string paramName)
		{
			return ChartParams.Match(INSTANCE, paramName);
		}



		public static ParamDesc CellInstParam(int idx)
		{
			return CellParams.InstanceParams[idx];
		}
		
		public static ParamDesc CellLabelParam(int idx)
		{
			return CellParams.LabelParams[idx];
		}


		public static ParamDesc MatchCellInstance(string paramName)
		{
			return CellParams.Match(INSTANCE, paramName);
		}

		public static ParamDesc MatchCellLabel(string paramName)
		{
			return CellParams.Match(LABEL, paramName);
		}






	#endregion

	#region private methods

		private static void defineChartParameters()
		{
			if (isConfigured) return;

			Family f = new ChartFamily(CHART_FAMILY_NAME,
				CT_ANNOTATION, SC_GENERIC_ANNOTATION);

			int snLen = f.ShortNameLength(INSTANCE);

			f.ConfigureLists(new []{5, 5, AllChartParamCount});

			// 0
			f.AddParam(new ParamDesc("Name", 
				NameIdx, snLen, INSTANCE, PARAM_MUST_EXIST, TEXT, READ_VALUE_REQUIRED, READ_FROM_PARAMETER));
			// 1
			f.AddParam(new ParamDesc("Sequence", 
				SeqIdx, snLen, INSTANCE, PARAM_OPTIONAL, TEXT, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER));
			// 2
			f.AddParam(new ParamDesc("Excel File Path", 
				ChartFilePathIdx, snLen, INSTANCE, PARAM_MUST_EXIST, FILE_PATH, READ_VALUE_REQUIRED, READ_FROM_PARAMETER));
			// 3
			f.AddParam(new ParamDesc("Excel WorkSheet Name", 
				ChartWorkSheetIdx, snLen, INSTANCE, PARAM_MUST_EXIST, TEXT, READ_VALUE_REQUIRED, READ_FROM_PARAMETER));
			// 4
			f.AddParam(new ParamDesc("Cell Family Name", 
				ChartFamilyNameIdx, snLen, INSTANCE, PARAM_MUST_EXIST, TEXT, READ_VALUE_REQUIRED, READ_FROM_PARAMETER));
			// 5
			f.AddParam(new ParamDesc("Update Type", 
				ChartUpdateTypeIdx, snLen, INSTANCE, PARAM_OPTIONAL, UPDATE_TYPE, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER));
			// 6
			f.AddParam(new ParamDesc("Cells With Errors", 
				ChartHasErrorsIdx, snLen, INSTANCE, PARAM_MUST_EXIST, TEXT, READ_VALUE_IGNORE, CALCULATED));

			families.Add(f);
		}

		private static void defineCellBasicParameters()
		{
			if (isConfigured) return;

			Family f = new CellFamily(CELL_FAMILY_NAME,
				CT_ANNOTATION, SC_GENERIC_ANNOTATION);

			int snLen = f.ShortNameLength(LABEL);

			f.ConfigureLists(new [] {5, 5, CellBasicParamCount, CellLabelParamCount});

			f.AddParam(new ParamDesc("Name", 
				NameIdx, snLen, INSTANCE, PARAM_MUST_EXIST, TEXT, READ_VALUE_REQUIRED, READ_FROM_PARAMETER));

			f.AddParam(new ParamDesc("Sequence", 
				SeqIdx, snLen, INSTANCE, PARAM_OPTIONAL, TEXT, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER));

			f.AddParam(new ParamDesc("Has Error", 
				HasErrorsIdx , snLen, INSTANCE, PARAM_MUST_EXIST, BOOL, READ_VALUE_IGNORE, CALCULATED));

			defineCellLabelParameters(f);

			families.Add(f);
		}
		
		private static void defineCellLabelParameters(Family f)
		{
			if (isConfigured) return;

			int snLen = f.ShortNameLength(INSTANCE);

			f.AddParam(new ParamDesc("Label", 
				LblLabelIdx, snLen, LABEL, 
				PARAM_MUST_EXIST, LABEL_TITLE, READ_VALUE_REQUIRED, READ_FROM_EXCEL));

			f.AddParam(new ParamDesc("Name", 
				LblNameIdx, snLen, LABEL, 
				PARAM_MUST_EXIST, TEXT, READ_VALUE_REQUIRED, READ_FROM_PARAMETER));
			
			f.AddParam(new ParamDesc("Formula", 
				LblFormulaIdx, snLen, LABEL, 
				PARAM_MUST_EXIST, FORMULA, READ_VALUE_REQUIRED, READ_FROM_PARAMETER));
			
			f.AddParam(new ParamDesc("Data Type", 
				LblDataTypeIdx, snLen, LABEL, 
				PARAM_MUST_EXIST, DATATYPE, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER));
						
			f.AddParam(new ParamDesc("Formatting Info", 
				LblFormatInfoIdx, snLen, LABEL, 
				PARAM_OPTIONAL, TEXT, READ_VALUE_OPTIONAL, READ_FROM_PARAMETER));
			
			f.AddParam(new ParamDesc("Ignore", 
				LblIgnoreIdx, snLen, LABEL, 
				PARAM_OPTIONAL, BOOL, READ_VALUE_REQUIRED, READ_FROM_PARAMETER, RevitCatagorizeParam.ParamBool));
			
			// f.AddParam(new ParamDesc("As Length", 
			// 	LblAsLengthIdx, snLen, LABEL, 
			// 	PARAM_OPTIONAL, NUMBER, READ_VALUE_IGNORE, CALCULATED));
			//
			// f.AddParam(new ParamDesc("As Number", 
			// 	LblAsNumberIdx, snLen, LABEL, 
			// 	PARAM_OPTIONAL, NUMBER, READ_VALUE_IGNORE, CALCULATED));
			//
			// f.AddParam(new ParamDesc("As Yes-No", 
			// 	LblAsYesNoIdx, snLen, LABEL, 
			// 	PARAM_OPTIONAL, NUMBER, READ_VALUE_IGNORE, CALCULATED));
		}



	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is RevitParamManager";
		}

	#endregion

	}
}