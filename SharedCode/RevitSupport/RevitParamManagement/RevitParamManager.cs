#region using

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamClass;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamType;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamReadReqmt;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamDataType;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamMode;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamExistReqmt;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamCat;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamSubCat;

#endregion

// username: jeffs
// created:  3/3/2021 7:51:06 PM

namespace SpreadSheet01.RevitSupport.RevitParamManagement
{
	public class RevitParamManager
	{
		public const int SHORT_NAME_LEN = 8;
		public const int MAX_LABELS_PER_CELL = 12;


	#region private fields

		public static string CHART_FAMILY_NAME = "CellSheetData";
		// public static string CHART_FAMILY_NAME = "SpreadSheetData";

		public static string CELL_FAMILY_NAME = "CellData";

		private static Families families;

		private static bool isConfigured;
		
	#endregion

	#region ctor

		static RevitParamManager()
		{
			configure();
		}

		private static void configure()
		{
			families = new Families(ParamCat.CT_ANNOTATION, ParamSubCat.SC_GENERIC_ANNOTATION);

			ChartFamily chart = defineChartParameters(CHART_FAMILY_NAME);

			defineCellParameters(chart, CELL_FAMILY_NAME);

			families.Add(chart);

			// test for a second chart + cells
			// chart = defineChartParameters(CHART_FAMILY_NAME + "_test");
			//
			// defineCellParameters(chart, CELL_FAMILY_NAME + "_test");
			//
			// families.Add(chart);

			isConfigured = true;

		}

	#endregion

	#region public properties

		public static bool IsConfigured => isConfigured;

		public static Families ChartFamilies => families;

	
	#region common instance params

		public static int CommonTypeParamTotal			= 0;
		public static readonly int IntNameIdx			= CommonTypeParamTotal++;
		public static readonly int DevelopIdx			= CommonTypeParamTotal++;


		public static int CommonInstParams		        = 0;

		// common indices
		public static readonly int NameIdx				= CommonInstParams++;
		public static readonly int SeqIdx				= CommonInstParams++;
		public static readonly int Descdx				= CommonInstParams++;

	#endregion

		public static int ChartInstanceParamTotal		= CommonInstParams;

		public static int CellBasicParamTotal			= CommonInstParams;

	#region cell specific instance params

	#region cell basic instance params

		// cell indices (cell basic)
		public static readonly int HasErrorsIdx         = CellBasicParamTotal++;
		// end of cell basic list

	#endregion

	#region cell label instance params

		public static int CellLabelParamTotal			= 0;

		// cell label indices (cell label)
		public static readonly int LblLabelIdx          = CellLabelParamTotal++;
		public static readonly int LblNameIdx           = CellLabelParamTotal++;
		public static readonly int LblFormulaIdx        = CellLabelParamTotal++;
		public static readonly int LblDataTypeIdx       = CellLabelParamTotal++;
		public static readonly int LblFormatInfoIdx     = CellLabelParamTotal++;

		public static readonly int LblIgnoreIdx         = CellLabelParamTotal++;
		// end of cell label list

	#endregion

		public static readonly int CellInstanceParamTotal = CellBasicParamTotal + CellLabelParamTotal;
		public static readonly int CellTypeParamCount = CommonTypeParamTotal;

		public static int CellInternalParamCount	        = 0; // temp number until real internal params are created
		public static readonly int CellInternalTempIdx		= CellInternalParamCount++;

	#endregion

	#region chart specific instance params

		// chart indices
		public static readonly int ChartFilePathIdx			= ChartInstanceParamTotal++;
		public static readonly int ChartWorkSheetIdx		= ChartInstanceParamTotal++;
		public static readonly int ChartCellFamilyNameIdx	= ChartInstanceParamTotal++;
		public static readonly int ChartUpdateTypeIdx		= ChartInstanceParamTotal++;

		public static readonly int ChartHasErrorsIdx		= ChartInstanceParamTotal++;
		// end of chart list

		public static readonly int ChartTypeParamCount = CommonTypeParamTotal;

		public static int ChartInternalParamCount	        = 0; // temp number until real internal params are created
		public static readonly int ChartInternalTempIdx		= ChartInternalParamCount++;

	#endregion

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static bool GetChartFamily(string chartFamilyName, out ChartFamily chart)
		{
			ChartFamily fam;

			bool result = families.FamilyTypes.TryGetValue(chartFamilyName, out fam);

			if (!result)
			{
				chart = ChartFamily.Invalid;

			} else
			{
				chart = fam;
			}

			return result;
		}

	#endregion

	#region private methods

		private static void defineParameter(Family f, ParamDesc pd)
		{
			pd.ShortName = RevitParamSupport.GetShortName(pd.ParameterName);

			f.AddParam(pd);
		}

		private static ChartFamily defineChartParameters(string chartFamilyName)
		{
			if (isConfigured) return null;

			int[] paramCounts = new int[(int) RevitParamSupport.PARAM_TYPE_COUNT];
			paramCounts[(int) PT_INSTANCE] = ChartInstanceParamTotal;
			paramCounts[(int) PT_INTERNAL] = ChartInternalParamCount;
			paramCounts[(int) PT_TYPE] = ChartTypeParamCount;
			paramCounts[(int) PT_LABEL] = 0;

			ChartFamily f = new ChartFamily(chartFamilyName, CT_ANNOTATION, 
				SC_GENERIC_ANNOTATION, paramCounts);


			//
			// instance parameters
			//
			// 0
			defineParameter(f     , new ParamDesc("Name"                , "",
				NameIdx           , PC_CHART                            , PT_INSTANCE, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			// 1
			defineParameter(f     , new ParamDesc("Sequence"            , "",
				SeqIdx            , PC_CHART                            , PT_INSTANCE, EX_PARAM_OPTIONAL, DT_TEXT, RD_VALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 2
			defineParameter(f     , new ParamDesc("Description"         , "",
				Descdx            , PC_CHART                            , PT_INSTANCE, EX_PARAM_OPTIONAL, DT_TEXT, RD_VALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 3
			defineParameter(f     , new ParamDesc("Excel File Path"     , "",
				ChartFilePathIdx  , PC_CHART                            , PT_INSTANCE, EX_PARAM_MUST_EXIST, DT_FILE_PATH, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			// 4
			defineParameter(f     , new ParamDesc("Excel WorkSheet Name", "",
				ChartWorkSheetIdx , PC_CHART                            , PT_INSTANCE, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			// 5
			defineParameter(f     , new ParamDesc("Cell Family Name"    , "",
				ChartCellFamilyNameIdx, PC_CHART                        , PT_INSTANCE, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			// 6
			defineParameter(f     , new ParamDesc("Update Scheme"       , "",
				ChartUpdateTypeIdx, PC_CHART                            , PT_INSTANCE, EX_PARAM_OPTIONAL, DT_UPDATE_TYPE, RD_VALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 7
			defineParameter(f     , new ParamDesc("Cells With Errors"   , "",
				ChartHasErrorsIdx , PC_CHART                            , PT_INSTANCE, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_IGNORE, PM_CALCULATED));
			//
			// internal parameters
			//
			// 0
			defineParameter(f     , new ParamDesc("Temp Internal"       , "",
				ChartInternalTempIdx, PC_CHART                          , PT_INTERNAL, EX_PARAM_INTERNAL, DT_TEXT, RD_VALUE_IGNORE, PM_INTERNAL));

			//
			// type parameters
			//
			// 0
			defineParameter(f     , new ParamDesc("Internal Name"        , "",
				IntNameIdx        , PC_CHART                            , PT_TYPE, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 1
			defineParameter(f     , new ParamDesc("Developer"           , "",
				DevelopIdx        , PC_CHART                            , PT_TYPE, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));


			return f;
		}

		private static void defineCellParameters(ChartFamily chart, string cellFamilyName)
		{
			if (isConfigured) return;

			int[] paramCounts = new int[(int) RevitParamSupport.PARAM_TYPE_COUNT];
			paramCounts[(int) PT_INSTANCE] = CellBasicParamTotal;
			paramCounts[(int) PT_INTERNAL] = CellInternalParamCount;
			paramCounts[(int) PT_TYPE] = CellTypeParamCount;
			paramCounts[(int) PT_LABEL] = CellLabelParamTotal;

			CellFamily f = new CellFamily(cellFamilyName,
				CT_ANNOTATION, SC_GENERIC_ANNOTATION, paramCounts);

			// f.ConfigureLists(new [] {5, CellBasicParamTotal, CellLabelParamTotal});

			//0
			defineParameter(f , new ParamDesc("Cell Name"   , "",
				NameIdx       , PC_CELL                    , PT_INSTANCE, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			//1
			defineParameter(f , new ParamDesc("Sequence"    , "",
				SeqIdx        , PC_CELL                    , PT_INSTANCE, EX_PARAM_OPTIONAL, DT_TEXT, RD_VALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 2
			defineParameter(f , new ParamDesc("Description" , "",
				Descdx        , PC_CELL                    , PT_INSTANCE, EX_PARAM_OPTIONAL, DT_TEXT, RD_VALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 3
			defineParameter(f , new ParamDesc("Has Error"   , "",
				HasErrorsIdx  , PC_CELL                    , PT_INSTANCE, EX_PARAM_MUST_EXIST, DT_BOOL, RD_VALUE_IGNORE, PM_CALCULATED));

			// 0
			defineParameter(f , new ParamDesc("Temp Internal", "",
				CellInternalTempIdx, PC_CELL               , PT_INTERNAL, EX_PARAM_INTERNAL, DT_TEXT, RD_VALUE_IGNORE, PM_INTERNAL));

			// 0
			defineParameter(f , new ParamDesc("Internal Name", "",
				IntNameIdx    , PC_CELL                    , PT_TYPE, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 1
			defineParameter(f , new ParamDesc("Developer"   , "",
				DevelopIdx    , PC_CELL                    , PT_TYPE, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));


			defineCellLabelParameters(f);

			chart.CellFamily = f;
			// chart.AddCellFamily(f);
		}

		private static void defineCellLabelParameters(Family f)
		{
			if (isConfigured) return;

			// 0
			defineParameter(f   , new ParamDesc("Label"          , "",
				LblLabelIdx     , PC_CELL                       , PT_LABEL, EX_PARAM_MUST_EXIST, DT_LABEL_TITLE, RD_VALUE_REQUIRED, PM_READ_FROM_EXCEL));
			// 1
			defineParameter(f   , new ParamDesc("Label Name"     , "",
				LblNameIdx      , PC_CELL                       , PT_LABEL, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			// 2
			defineParameter(f   , new ParamDesc("Formula"        , "",
				LblFormulaIdx   , PC_CELL                       , PT_LABEL, EX_PARAM_MUST_EXIST, DT_FORMULA, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			// 3
			defineParameter(f   , new ParamDesc("Data Type"      , "",
				LblDataTypeIdx  , PC_CELL                       , PT_LABEL, EX_PARAM_MUST_EXIST, DT_DATATYPE, RD_VALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 4
			defineParameter(f   , new ParamDesc("Formatting Info", "",
				LblFormatInfoIdx, PC_CELL                       , PT_LABEL, EX_PARAM_OPTIONAL, DT_TEXT, RD_VALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 5
			defineParameter(f   , new ParamDesc("Ignore"         , "",
				LblIgnoreIdx    , PC_CELL                       , PT_LABEL, EX_PARAM_OPTIONAL, DT_BOOL, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
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


// public static int MustExistChartInstance => paramCounts.MustExistCount(INSTANCE);
// public static int MustExistChartType => paramCounts.MustExistCount(TYPE);
//
// public static int MustExistCellInstance => CellParams.MustExistCount(INSTANCE);
// public static int MustExistCellType => CellParams.MustExistCount(TYPE);
// public static int MustExistCellLabel => CellParams.MustExistCount(LABEL);

// public static ChartFamily ChartFam => (ChartFamily) families.FamilyTypes[CHART_FAMILY_NAME];
// public static CellFamily CellFam => (CellFamily) families.FamilyTypes[CELL_FAMILY_NAME];


		// public static CellFamily GetCellFamily(string cellFamilyName)
		// {
		// 	Family fam;
		//
		// 	bool result = getFamily(cellFamilyName, out fam);
		//
		// 	return fam as CellFamily;
		// }
		// public static CellFamily GetCellFamily(ChartFamily cf)
		// {
		// 	return GetCellFamily(cf.FamilyName);
		// }
		// private static bool getFamily(string familyName, out ChartFamily fam)
		// {
		// 	return families.FamilyTypes.TryGetValue(familyName, out fam);
		// }
		//
		// public static ParamDesc ChartParam(int idx)
		// {
		// 	return ChartFam[PT_INSTANCE, idx];
		// }
		//
		// public static ParamDesc ChartInternalParam(int idx)
		// {
		// 	return ChartFam[PT_INTERNAL, idx];
		// }
		//
		// public static ParamDesc CellParam(int idx)
		// {
		// 	return CellFam[PT_INSTANCE, idx];
		// }
		//
		// public static ParamDesc CellInternalParam(int idx)
		// {
		// 	return CellFam[PT_INTERNAL, idx];
		// }
		//
		// public static ParamDesc CellLabelParam(int idx)
		// {
		// 	return CellFam[PT_LABEL, idx];
		// }
		// public void SetShortName(string parameterName)
		// {
		// 	shortName = GetShortName(parameterName, Family.SHORT_NAME_LEN);
		// }
		// public static ParamDesc ChartInstParam(int idx)
		// {
		// 	return paramCounts.InstanceParams[idx];
		// }
		// 		
		// public static ParamDesc ChartTypeParam(int idx)
		// {
		// 	return paramCounts.TypeParams[idx];
		// }
		// public static ParamDesc MatchChartInstance(string paramName)
		// {
		// 	return paramCounts.Match(INSTANCE, paramName);
		// }
		// public static ParamDesc CellTypeParam(int idx)
		// {
		// 	return CellParams.TypeParams[idx];
		// }
		//
		// public static ParamDesc CellInstParam(int idx)
		// {
		// 	return CellParams.InstanceParams[idx];
		// }
		// public static ParamDesc MatchCellInstance(string paramName)
		// {
		// 	return CellParams.Match(INSTANCE, paramName);
		// }
		//
		// public static ParamDesc MatchCellLabel(string paramName)
		// {
		// 	return CellParams.Match(LABEL, paramName);
		// }