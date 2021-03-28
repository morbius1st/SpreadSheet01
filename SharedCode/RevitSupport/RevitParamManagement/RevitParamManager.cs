#region using

using System;
using System.Collections.Generic;
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


	#region private fields

		public static string CHART_FAMILY_NAME = "CellSheetData";
		// public static string CHART_FAMILY_NAME = "SpreadSheetData";

		public static string CELL_FAMILY_NAME = "CellData";

		private static Families families = new Families();

		private static bool isConfigured;

	#endregion

	#region ctor

		static RevitParamManager()
		{
			defineChartParameters();
			defineCellParameters();
		}

	#endregion

	#region public properties

		// public static int MustExistChartInstance => ChartParams.MustExistCount(INSTANCE);
		// public static int MustExistChartType => ChartParams.MustExistCount(TYPE);
		//
		// public static int MustExistCellInstance => CellParams.MustExistCount(INSTANCE);
		// public static int MustExistCellType => CellParams.MustExistCount(TYPE);
		// public static int MustExistCellLabel => CellParams.MustExistCount(LABEL);

		public static bool IsConfigured => isConfigured;

		public static ChartFamily ChartFam => 
			(ChartFamily) families.FamilyTypes[CHART_FAMILY_NAME];

		public static CellFamily CellFam => 
			(CellFamily) families.FamilyTypes[CELL_FAMILY_NAME];


	#region common instance params

		public static int CommonInstParams		        = 0;

		// common indices
		public static readonly int NameIdx				= CommonInstParams++;
		public static readonly int SeqIdx				= CommonInstParams++;
		public static readonly int Descdx				= CommonInstParams++;
		public static readonly int IntNameIdx			= CommonInstParams++;
		public static readonly int DevelopIdx			= CommonInstParams++;

	#endregion

		public static int ChartTotalParamCount			= CommonInstParams;

		public static int CellBasicTotalParamCount			= CommonInstParams;

	#region cell specific instance params

	#region cell basic instance params

		// cell indices (cell basic)
		public static readonly int HasErrorsIdx         = CellBasicTotalParamCount++;
		// end of cell basic list

	#endregion

	#region cell label instance params

		public static int CellLabelTotalParamCount			= 0;

		// cell label indices (cell label)
		public static readonly int LblLabelIdx          = CellLabelTotalParamCount++;
		public static readonly int LblNameIdx           = CellLabelTotalParamCount++;
		public static readonly int LblFormulaIdx        = CellLabelTotalParamCount++;
		public static readonly int LblDataTypeIdx       = CellLabelTotalParamCount++;
		public static readonly int LblFormatInfoIdx     = CellLabelTotalParamCount++;
		public static readonly int LblIgnoreIdx         = CellLabelTotalParamCount++;
		// end of cell label list

	#endregion

		public static readonly int CellTotalParamCount = CellBasicTotalParamCount + CellLabelTotalParamCount;

	#endregion

	#region chart specific instance params

		// chart indices
		public static readonly int ChartFilePathIdx		= ChartTotalParamCount++;
		public static readonly int ChartWorkSheetIdx	= ChartTotalParamCount++;
		public static readonly int ChartCellFamilyNameIdx	= ChartTotalParamCount++;
		public static readonly int ChartUpdateTypeIdx	= ChartTotalParamCount++;
		public static readonly int ChartHasErrorsIdx	= ChartTotalParamCount++;
		// end of chart list

	#endregion


		
	#region chart internal params

		public static int ChartInternalParamCount	        = 0; // temp number until real internal params are created
		public static readonly int ChartInternalTempIdx		= ChartInternalParamCount++;

		public static int CellInternalParamCount	        = 0; // temp number until real internal params are created
		public static readonly int CellInternalTempIdx		= CellInternalParamCount++;

	#endregion

		// // start of cell internal parameters
		// public static int CI_ParamCount		= 0;
		//
		// public static readonly int CI_CurrError			= CI_ParamCount++;


	#endregion

	#region private properties

	#endregion

	#region public methods


		public static string GetShortName(string name)
		{
			return name.Substring(0, Math.Min(name.Length, SHORT_NAME_LEN));
		}

		public static ParamDesc ChartParam(int idx)
		{
			return ChartFam.Params[idx];
		}

		public static ParamDesc ChartInternalParam(int idx)
		{
			return ChartFam.InternalParams[idx];
		}



		public static ParamDesc CellParam(int idx)
		{
			return CellFam.Params[idx];
		}
		
		public static ParamDesc CellInternalParam(int idx)
		{
			return CellFam.InternalParams[idx];
		}




		public static ParamDesc CellLabelParam(int idx)
		{
			return CellFam.LabelParams[idx];
		}



		// public void SetShortName(string parameterName)
		// {
		// 	shortName = GetShortName(parameterName, Family.SHORT_NAME_LEN);
		// }
		// public static ParamDesc ChartInstParam(int idx)
		// {
		// 	return ChartParams.InstanceParams[idx];
		// }
		// 		
		// public static ParamDesc ChartTypeParam(int idx)
		// {
		// 	return ChartParams.TypeParams[idx];
		// }
		// public static ParamDesc MatchChartInstance(string paramName)
		// {
		// 	return ChartParams.Match(INSTANCE, paramName);
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

	#endregion

	#region private methods

		private static void defineParameter(Family f, ParamDesc pd)
		{
			pd.ShortName = GetShortName(pd.ParameterName);

			f.AddParam(pd);
		}



		private static void defineChartParameters()
		{
			if (isConfigured) return;

			Family f = new ChartFamily(CHART_FAMILY_NAME,
				CT_ANNOTATION, SC_GENERIC_ANNOTATION);

			f.ConfigureLists(new []{5, ChartTotalParamCount});

			// 0
			defineParameter(f     , new ParamDesc("Name"                , "",
				NameIdx           , PC_CHART                            , PT_PARAM, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			// 1
			defineParameter(f     , new ParamDesc("Sequence"            , "",
				SeqIdx            , PC_CHART                            , PT_PARAM, EX_PARAM_OPTIONAL, DT_TEXT, RDVALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 2
			defineParameter(f     , new ParamDesc("Description"         , "",
				Descdx            , PC_CHART                            , PT_PARAM, EX_PARAM_OPTIONAL, DT_TEXT, RDVALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 3
			defineParameter(f     , new ParamDesc("InternalName"        , "",
				IntNameIdx        , PC_CHART                            , PT_PARAM, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_IGNORE, PM_READ_FROM_FAMILY));
			// 4
			defineParameter(f     , new ParamDesc("Developer"           , "",
				DevelopIdx        , PC_CHART                            , PT_PARAM, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_IGNORE, PM_READ_FROM_FAMILY));
			// 5
			defineParameter(f     , new ParamDesc("Excel File Path"     , "",
				ChartFilePathIdx  , PC_CHART                            , PT_PARAM, EX_PARAM_MUST_EXIST, DT_FILE_PATH, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			// 6
			defineParameter(f     , new ParamDesc("Excel WorkSheet Name", "",
				ChartWorkSheetIdx , PC_CHART                            , PT_PARAM, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			// 7
			defineParameter(f     , new ParamDesc("Cell Family Name"    , "",
				ChartCellFamilyNameIdx, PC_CHART                        , PT_PARAM, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			// 8
			defineParameter(f     , new ParamDesc("Update Scheme"       , "",
				ChartUpdateTypeIdx, PC_CHART                            , PT_PARAM, EX_PARAM_OPTIONAL, DT_UPDATE_TYPE, RDVALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 9
			defineParameter(f     , new ParamDesc("Cells With Errors"   , "",
				ChartHasErrorsIdx , PC_CHART                            , PT_PARAM, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_IGNORE, PM_CALCULATED));

			// 0
			defineParameter(f     , new ParamDesc("Temp Internal"       , "",
				ChartInternalTempIdx, PC_CHART                          , PT_INTERNAL, EX_PARAM_INTERNAL, DT_TEXT, RD_VALUE_IGNORE, PM_INTERNAL));



			families.Add(f);
		}

		private static void defineCellParameters()
		{
			if (isConfigured) return;

			Family f = new CellFamily(CELL_FAMILY_NAME,
				CT_ANNOTATION, SC_GENERIC_ANNOTATION);

			f.ConfigureLists(new [] {5, CellBasicTotalParamCount, CellLabelTotalParamCount});

			//0
			defineParameter(f , new ParamDesc("Cell Name"   , "", 
				NameIdx       , PC_CELL                    , PT_PARAM, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			//1
			defineParameter(f , new ParamDesc("Sequence"    , "", 
				SeqIdx        , PC_CELL                    , PT_PARAM, EX_PARAM_OPTIONAL, DT_TEXT, RDVALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 2
			defineParameter(f , new ParamDesc("Description" , "", 
				Descdx        , PC_CELL                    , PT_PARAM, EX_PARAM_OPTIONAL, DT_TEXT, RDVALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 3
			defineParameter(f , new ParamDesc("InternalName", "", 
				IntNameIdx    , PC_CELL                    , PT_PARAM, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_IGNORE, PM_READ_FROM_FAMILY));
			// 4
			defineParameter(f , new ParamDesc("Developer"   , "", 
				DevelopIdx    , PC_CELL                    , PT_PARAM, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_IGNORE, PM_READ_FROM_FAMILY));
			// 5
			defineParameter(f , new ParamDesc("Has Error"   , "", 
				HasErrorsIdx  , PC_CELL                    , PT_PARAM, EX_PARAM_MUST_EXIST, DT_BOOL, RD_VALUE_IGNORE, PM_CALCULATED));

			// 0
			defineParameter(f , new ParamDesc("Temp Internal", "",
				CellInternalTempIdx, PC_CELL               , PT_INTERNAL, EX_PARAM_INTERNAL, DT_TEXT, RD_VALUE_IGNORE, PM_INTERNAL));

			defineCellLabelParameters(f);

			families.Add(f);
		}
		
		private static void defineCellLabelParameters(Family f)
		{
			if (isConfigured) return;

			// 0
			defineParameter(f   , new ParamDesc("Label"          , "", 
				LblLabelIdx     , PC_LABEL                       , PT_LABEL, EX_PARAM_MUST_EXIST, DT_LABEL_TITLE, RD_VALUE_REQUIRED, PM_READ_FROM_EXCEL));
			// 1
			defineParameter(f   , new ParamDesc("Label Name"     , "", 
				LblNameIdx      , PC_LABEL                       , PT_LABEL, EX_PARAM_MUST_EXIST, DT_TEXT, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			// 2
			defineParameter(f   , new ParamDesc("Formula"        , "", 
				LblFormulaIdx   , PC_LABEL                       , PT_LABEL, EX_PARAM_MUST_EXIST, DT_FORMULA, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			// 3
			defineParameter(f   , new ParamDesc("Data Type"      , "", 
				LblDataTypeIdx  , PC_LABEL                       , PT_LABEL, EX_PARAM_MUST_EXIST, DT_DATATYPE, RDVALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 4
			defineParameter(f   , new ParamDesc("Formatting Info", "", 
				LblFormatInfoIdx, PC_LABEL                       , PT_LABEL, EX_PARAM_OPTIONAL, DT_TEXT, RDVALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 5
			defineParameter(f   , new ParamDesc("Ignore"         , "", 
				LblIgnoreIdx    , PC_LABEL                       , PT_LABEL, EX_PARAM_OPTIONAL, DT_BOOL, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
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