#region using

using System;
using System.Collections.Generic;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamReadReqmt;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamDataType;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamMode;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamType;
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

		public static string CELL_FAMILY_NAME = "CellData";
		public static string CHART_FAMILY_NAME = RevitCellSystManager.RevitChartFamilyName;

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

		public static Families Fams = families;

		public static ChartFamily ChartParams => 
			(ChartFamily) families.FamilyTypes[CHART_FAMILY_NAME];

		public static CellFamily CellParams => 
			(CellFamily) families.FamilyTypes[CELL_FAMILY_NAME];


	#region common instance params

		public static int CommonInstParams		        = 0;

		// common indices
		public static readonly int NameIdx				= CommonInstParams++;
		public static readonly int SeqIdx				= CommonInstParams++;
		public static readonly int Descdx				= CommonInstParams++;

	#endregion

		public static int ChartTotalInstParamCount			= CommonInstParams;

		public static int CellBasicInstParamCount			= CommonInstParams;

	#region cell specific instance params

	#region cell basic instance params

		// cell indices (cell basic)
		public static readonly int HasErrorsIdx         = CellBasicInstParamCount++;
		// end of cell basic list

	#endregion

	#region cell label instance params

		public static int CellLabelInstParamCount			= 0;

		// cell label indices (cell label)
		public static readonly int LblLabelIdx          = CellLabelInstParamCount++;
		public static readonly int LblNameIdx           = CellLabelInstParamCount++;
		public static readonly int LblFormulaIdx        = CellLabelInstParamCount++;
		public static readonly int LblDataTypeIdx       = CellLabelInstParamCount++;
		public static readonly int LblFormatInfoIdx     = CellLabelInstParamCount++;
		public static readonly int LblIgnoreIdx         = CellLabelInstParamCount++;
		// end of cell label list

	#endregion

		public static readonly int CellTotalParamCount = CellBasicInstParamCount + CellLabelInstParamCount;

	#endregion

	#region chart specific instance params

		// chart indices
		public static readonly int ChartFilePathIdx		= ChartTotalInstParamCount++;
		public static readonly int ChartWorkSheetIdx	= ChartTotalInstParamCount++;
		public static readonly int ChartFamilyNameIdx	= ChartTotalInstParamCount++;
		public static readonly int ChartUpdateTypeIdx	= ChartTotalInstParamCount++;
		public static readonly int ChartHasErrorsIdx	= ChartTotalInstParamCount++;
		// end of chart list

	#endregion


	#region common type params

		public static int CommonTypeParams	        = 0;

		public static readonly int IntNameIdx			= CommonTypeParams++;
		public static readonly int DevelopIdx			= CommonTypeParams++;

	#endregion

		
	#region chart internal params

		public static int ChartInternalParamCount	        = 0;

		public static readonly int CellParamsClassIdx		= ChartInternalParamCount++;


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


		// public void SetShortName(string parameterName)
		// {
		// 	shortName = GetShortName(parameterName, Family.SHORT_NAME_LEN);
		// }
		
		public static ParamDesc ChartInstParam(int idx)
		{
			return ChartParams.InstanceParams[idx];
		}
		
		// public static ParamDesc MatchChartInstance(string paramName)
		// {
		// 	return ChartParams.Match(INSTANCE, paramName);
		// }
		
		public static ParamDesc CellTypeParam(int idx)
		{
			return CellParams.TypeParams[idx];
		}
		
		public static ParamDesc CellInstParam(int idx)
		{
			return CellParams.InstanceParams[idx];
		}
		
		public static ParamDesc CellLabelParam(int idx)
		{
			return CellParams.LabelParams[idx];
		}
		
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

			f.ConfigureLists(new []{5, 5, ChartTotalInstParamCount});

			// 0
			defineParameter(f, new ParamDesc("Name", "", 
				NameIdx, PT_INSTANCE, EX_PARAM_MUST_EXIST, TEXT, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			// 1
			defineParameter(f, new ParamDesc("Sequence", "", 
				SeqIdx, PT_INSTANCE, EX_PARAM_OPTIONAL, TEXT, RDVALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 2
			defineParameter(f, new ParamDesc("Description", "", 
				Descdx, PT_INSTANCE, EX_PARAM_OPTIONAL, TEXT, RDVALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 3
			defineParameter(f, new ParamDesc("Excel File Path", "", 
				ChartFilePathIdx, PT_INSTANCE, EX_PARAM_MUST_EXIST, FILE_PATH, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			// 4
			defineParameter(f, new ParamDesc("Excel WorkSheet Name", "", 
				ChartWorkSheetIdx, PT_INSTANCE, EX_PARAM_MUST_EXIST, TEXT, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			// 5
			defineParameter(f, new ParamDesc("Cell Family Name", "", 
				ChartFamilyNameIdx, PT_INSTANCE, EX_PARAM_MUST_EXIST, TEXT, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			// 6
			defineParameter(f, new ParamDesc("Update Scheme", "", 
				ChartUpdateTypeIdx, PT_INSTANCE, EX_PARAM_OPTIONAL, UPDATE_TYPE, RDVALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 7
			defineParameter(f, new ParamDesc("Cells With Errors", "", 
				ChartHasErrorsIdx, PT_INSTANCE, EX_PARAM_MUST_EXIST, TEXT, RD_VALUE_IGNORE, PM_CALCULATED));


			// 0
			defineParameter(f, new ParamDesc("InternalName", "", 
				IntNameIdx, PT_TYPE, EX_PARAM_MUST_EXIST, TEXT, RD_VALUE_IGNORE, PM_READ_FROM_FAMILY));
			// 1
			defineParameter(f, new ParamDesc("Developer", "", 
				DevelopIdx, PT_TYPE, EX_PARAM_MUST_EXIST, TEXT, RD_VALUE_IGNORE, PM_READ_FROM_FAMILY));


			// 0
			defineParameter(f, new ParamDesc("Param Class Name", "", 
				CellParamsClassIdx, PT_INTERNAL, EX_PARAM_INTERNAL, TEXT, RD_VALUE_IGNORE, PM_INTERNAL));



			families.Add(f);
		}

		private static void defineCellParameters()
		{
			if (isConfigured) return;

			Family f = new CellFamily(CELL_FAMILY_NAME,
				CT_ANNOTATION, SC_GENERIC_ANNOTATION);

			f.ConfigureLists(new [] {5, 5, CellBasicInstParamCount, CellLabelInstParamCount});
			//0
			defineParameter(f, new ParamDesc("Cell Name", "", 
				NameIdx, PT_INSTANCE, EX_PARAM_MUST_EXIST, TEXT, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			//1
			defineParameter(f, new ParamDesc("Sequence", "", 
				SeqIdx, PT_INSTANCE, EX_PARAM_OPTIONAL, TEXT, RDVALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 2
			defineParameter(f, new ParamDesc("Description", "", 
				Descdx, PT_INSTANCE, EX_PARAM_OPTIONAL, TEXT, RDVALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			// 3
			defineParameter(f, new ParamDesc("Has Error", "", 
				HasErrorsIdx , PT_INSTANCE, EX_PARAM_MUST_EXIST, BOOL, RD_VALUE_IGNORE, PM_CALCULATED));

			// 0
			defineParameter(f, new ParamDesc("InternalName", "", 
				IntNameIdx, PT_TYPE, EX_PARAM_MUST_EXIST, TEXT, RD_VALUE_IGNORE, PM_READ_FROM_FAMILY));
			// 1
			defineParameter(f, new ParamDesc("Developer", "", 
				DevelopIdx, PT_TYPE, EX_PARAM_MUST_EXIST, TEXT, RD_VALUE_IGNORE, PM_READ_FROM_FAMILY));



			defineCellLabelParameters(f);

			families.Add(f);
		}
		
		private static void defineCellLabelParameters(Family f)
		{
			if (isConfigured) return;

			defineParameter(f, new ParamDesc("Label", "", 
				LblLabelIdx, PT_LABEL, 
				EX_PARAM_MUST_EXIST, LABEL_TITLE, RD_VALUE_REQUIRED, PM_READ_FROM_EXCEL));

			defineParameter(f, new ParamDesc("Label Name", "", 
				LblNameIdx, PT_LABEL, 
				EX_PARAM_MUST_EXIST, TEXT, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			
			defineParameter(f, new ParamDesc("Formula", "", 
				LblFormulaIdx, PT_LABEL, 
				EX_PARAM_MUST_EXIST, FORMULA, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
			
			defineParameter(f, new ParamDesc("Data Type", "", 
				LblDataTypeIdx, PT_LABEL, 
				EX_PARAM_MUST_EXIST, DATATYPE, RDVALUE_OPTIONAL, PM_READ_FROM_FAMILY));
						
			defineParameter(f, new ParamDesc("Formatting Info", "", 
				LblFormatInfoIdx, PT_LABEL, 
				EX_PARAM_OPTIONAL, TEXT, RDVALUE_OPTIONAL, PM_READ_FROM_FAMILY));
			
			defineParameter(f, new ParamDesc("Ignore", "", 
				LblIgnoreIdx, PT_LABEL, 
				EX_PARAM_OPTIONAL, BOOL, RD_VALUE_REQUIRED, PM_READ_FROM_FAMILY));
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