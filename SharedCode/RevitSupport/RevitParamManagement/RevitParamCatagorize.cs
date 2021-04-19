using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.DB;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using Family = SpreadSheet01.RevitSupport.RevitCellsManagement.Family;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.Management.ErrorCodes;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamType;

//using static SharedCode.RevitSupport.RevitParamManagement.ErrorCodeList2;


// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitCatagorizeParam.cs
// Created:      2021-03-03 (7:31 PM)


namespace SpreadSheet01.RevitSupport.RevitParamManagement
{
	public class RevitParamCatagorize
	{
		private int mustExistParamCount = 0;

		public RevitChartData CatagorizeChartSymParams(Element elChart,
			ChartFamily chartFamily)
		{
			dataParamCount = 0;
			mustExistParamCount = 0;

		#if REVIT
			IList<Parameter> p = elChart.GetOrderedParameters();
			ParameterMap pm = elChart.ParametersMap;
			ParameterSet ps = elChart.Parameters;
		#endif

			RevitChartData rcd = new RevitChartData(chartFamily);

			rcd.RevitElement = elChart;
			rcd.AnnoSymbol = (AnnotationSymbol) elChart;

			foreach (Parameter param in elChart.GetOrderedParameters())
			{
				catagorizeChartSymParam(rcd, param, chartFamily, PT_INST_OR_INTL);
			}

			foreach (Parameter param in ((AnnotationSymbol) elChart).Symbol.GetOrderedParameters())
			{
				catagorizeChartSymParam(rcd, param, chartFamily, PT_TYPE);
			}

			rcd.ValidateMustExist();

			return rcd;
		}

		private void validateChartSymParams(RevitChartData rcd, int mustExistParamCount)
		{
			rcd.ValidateMustExist();
		}

		private void catagorizeChartSymParam(RevitChartData rcd,
			Parameter param, ChartFamily chartFamily, ParamType type)
		{
			ARevitParam rvtParam;
			ParamDesc pd;

			string paramName = param.Definition.Name;

			bool result = chartFamily.Match2(paramName, type, out pd);

			if (!result || pd == null || pd.Mode == ParamMode.PM_NOT_USED) return;

			if (pd.Exist == ParamExistReqmt.EX_PARAM_MUST_EXIST) mustExistParamCount++;

			switch (pd.ParamType)
			{
			case ParamType.PT_INSTANCE:
			case ParamType.PT_INTERNAL:
			case ParamType.PT_TYPE:
				{
					dataParamCount++;
					if (pd.DataType == ParamDataType.DT_IGNORE) return;
					rvtParam = catagorizeParameter(param, pd);
					rcd.Add(pd.ParamType, pd.Index, rvtParam);
					break;
				}
			default:
				{
					rvtParam = ARevitParam.Invalid;
//					ErrCodeList.Add(this, ErrorCodes.CHT_INVALID_PARAM_TYPE_CS001143);
					rvtParam.ErrorCode = RCHT_INVALID_PARAM_TYPE_CS001143;
					rcd.Add(PT_INSTANCE, pd.Index, rvtParam);
					break;
				}
			}

			if (rvtParam.HasErrors)
			{
				// Debug.WriteLine("is not valid");
//				ErrCodeList.Add(this, ErrorCodes.CHT_PARAM_HAS_ERROR_CS001137);
				rcd.ErrorCode = RCHT_PARAM_HAS_ERROR_CS001137;
			}
		}

		private int[] labelParamCount;
		private int dataParamCount = 0;

		public RevitCellData catagorizeCellParams(AnnotationSymbol aSym, CellFamily cellFamily)
		{
			dataParamCount = 0;
			labelParamCount = new int[RevitParamManager.MAX_LABELS_PER_CELL];

			RevitCellData rcd = new RevitCellData(cellFamily);

		#if REVIT
			IList<Parameter> p = aSym.GetOrderedParameters();
			ParameterMap pm = aSym.ParametersMap;
			ParameterSet ps = aSym.Parameters;
		#endif


			foreach (Parameter param in aSym.GetOrderedParameters())
			{
				catagorizeCellParam(rcd, param, cellFamily, PT_INST_OR_INTL);
			}

			foreach (Parameter param in  aSym.Symbol.GetOrderedParameters())
			{
				catagorizeCellParam(rcd, param, cellFamily, PT_TYPE);
			}

			validateRcdMustExist(rcd);

			return rcd;
		}

		private void validateRcdMustExist(RevitCellData rcd)
		{
			rcd.ValidateMustExist();
			rcd.ValidateLabelMustExist();
		}


		// categorize a single parameter
		private void catagorizeCellParam(RevitCellData rcd, Parameter param,
			CellFamily cellFamily, ParamType type)
		{
			ARevitParam rvtParam = null;
			ParamDesc pd;

			int labelId;
			bool isLabel;

			string paramRootName = RevitParamUtil.GetRootName(param.Definition.Name,
				out labelId, out isLabel);

			// ParamType pt = isLabel ? PT_LABEL : type;
			type = isLabel ? PT_LABEL : type;

			bool result = cellFamily.Match2(paramRootName, type, out pd);

			if (! result || pd == null || pd.Mode == ParamMode.PM_NOT_USED) return;

			switch (pd.ParamType)
			{
			case ParamType.PT_INSTANCE:
			case ParamType.PT_INTERNAL:
			case ParamType.PT_TYPE:
				{
					dataParamCount++;
					if (pd.DataType == ParamDataType.DT_IGNORE) return;
					rvtParam = catagorizeParameter(param, pd);
					rcd.Add(pd.ParamType, pd.Index, rvtParam);
					break;
				}
			case ParamType.PT_LABEL:
				{
					labelParamCount[labelId]++;
					if (pd.DataType == ParamDataType.DT_IGNORE) return;

					ARevitParam labelParam = catagorizeParameter(param, pd);

					bool answer = rcd.AddLabelParam(labelId, pd.Index, labelParam);

					// Debug.Write("\n");

					if (!answer)
					{
//						ErrCodeList.Add(this, ErrorCodes.LBL_INVALID_CS001125);
						rcd.ErrorCode = LBL_INVALID_CS001125;
					}

					return;
				}

			default:
				{
					rvtParam = ARevitParam.Invalid;
//					ErrCodeList.Add(this, ErrorCodes.CEL_INVALID_TYPE_CS001113);
					rvtParam.ErrorCode = CEL_INVALID_TYPE_CS001113;
					rcd.Add(PT_INSTANCE, pd.Index, rvtParam);
					break;
				}
			}

			if (rvtParam.HasErrors)
			{
//				ErrCodeList.Add(this, ErrorCodes.CEL_HAS_ERROR_CS001107);
				rcd.ErrorCode = CEL_HAS_ERROR_CS001107;
			}
		}


		private ARevitParam catagorizeParameter(Parameter param, ParamDesc pd, string name = "")
		{
			ARevitParam p = null;

			// pd.InvokeDelegate(param);

			switch (pd.DataType)
			{
			case ParamDataType.DT_TEXT:
				{
					p = new RevitParamText(pd.ReadReqmt ==
						ParamReadReqmt.RD_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			case ParamDataType.DT_BOOL:
				{
					p = new RevitParamBool(
						pd.ReadReqmt ==
						ParamReadReqmt.RD_VALUE_IGNORE
							? (bool?) false
							: param.AsInteger() == 1, pd);
					break;
				}
			case ParamDataType.DT_NUMBER:
				{
					p = new RevitParamNumber(
						pd.ReadReqmt ==
						ParamReadReqmt.RD_VALUE_IGNORE
							? double.NaN
							: param.AsDouble(), pd);
					break;
				}
			case ParamDataType.DT_FORMULA:
				{
					p = new RevitParamFormula(pd.ReadReqmt ==
						ParamReadReqmt.RD_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			case ParamDataType.DT_DATATYPE:
				{
					p = new RevitParamText(pd.ReadReqmt ==
						ParamReadReqmt.RD_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			case ParamDataType.DT_FILE_PATH:
				{
					p = new RevitParamFilePath(pd.ReadReqmt ==
						ParamReadReqmt.RD_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			case ParamDataType.DT_UPDATE_TYPE:
				{
					p = new RevitParamUpdateType(pd.ReadReqmt ==
						ParamReadReqmt.RD_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			case ParamDataType.DT_WORKSHEETNAME:
				{
					p = new RevitParamWkShtName(pd.ReadReqmt ==
						ParamReadReqmt.RD_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			case ParamDataType.DT_LABEL_TITLE:
				{
					p = new RevitParamLabel("", pd);
					((RevitParamLabel) p).LabelValueName = param.Definition.Name;
					break;
				}
			case ParamDataType.DT_INTEGER:
				{
					p = new RevitParamNumber(
						pd.ReadReqmt ==
						ParamReadReqmt.RD_VALUE_IGNORE
							? double.NaN
							: param.AsDouble(), pd);
					break;
				}
			case ParamDataType.DT_SEQUENCE:
				{
					p = new RevitParamSequence(pd.ReadReqmt ==
						ParamReadReqmt.RD_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			}

			return p;
		}

		// public delegate ARevitParam MakeParamDelegate(Parameter param, ParamDesc pd);
		//
		//
		// MakeParamDelegate ParamBoolDelegate = ParamBool;
		//
		// public static ARevitParam ParamBool(Parameter param, ParamDesc pd)
		// {
		// 	Debug.WriteLine("got bool delegate");
		//
		// 	return null;
		// }
	}
}