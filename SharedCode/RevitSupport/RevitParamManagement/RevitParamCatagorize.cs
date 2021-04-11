using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.DB;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using Family = SpreadSheet01.RevitSupport.RevitCellsManagement.Family;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.Management.ErrorCodes;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamType;


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

			// ARevitParam rvtParam;
			// ParamDesc pd;

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

			validateChartSymParams(rcd, dataParamCount, mustExistParamCount);

			return rcd;
		}

		private void catagorizeChartSymParam(RevitChartData rcd,
			Parameter param, ChartFamily chartFamily, ParamType type)
		{
			ARevitParam rvtParam;
			ParamDesc pd;

			string paramName = param.Definition.Name;

			// Debug.WriteLine("\ncategorizing| " + paramName);

			// pd = Family.Match(paramName, ParamClass.PC_CHART);
			// bool result = chartFamily.Match(paramName, out pd, isType, false);
			bool result = chartFamily.Match2(paramName, type, out pd);

			if (!result || pd == null || pd.Mode == ParamMode.PM_NOT_USED) return;

			// Debug.WriteLine("got param desc| " + pd.ParameterName 
			// 	+ "  type| " + pd.ParamType + "  data type| " + pd.DataType);

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
			// case ParamType.PT_INTERNAL:
			// 	{
			// 		dataParamCount++;
			// 		if (pd.DataType == ParamDataType.DT_IGNORE) return;
			// 		rvtParam = catagorizeParameter(param, pd);
			// 		rcd.Add(pd.ParamType, pd.Index, rvtParam);
			// 		break;
			// 	}
			// case ParamType.PT_TYPE:
			// 	{
			// 		dataParamCount++;
			// 		if (pd.DataType == ParamDataType.DT_IGNORE) return;
			// 		rvtParam = catagorizeParameter(param, pd);
			// 		rcd.Add(pd.ParamType, pd.Index, rvtParam);
			// 		break;
			// 	}
			default:
				{
					rvtParam = ARevitParam.Invalid;
					rvtParam.ErrorCode = CHT_INVALID_PARAM_TYPE_CS001143;
					rcd.Add(PT_INSTANCE, pd.Index, rvtParam);
					break;
				}
			}

			if (rvtParam.HasErrors)
			{
				// Debug.WriteLine("is not valid");
				rcd.ErrorCode = CHT_PARAM_HAS_ERROR_CS001137;
			}

			// else
			// {
			// 	string value = rvtParam.DynValue.AsString();
			// 	// Debug.WriteLine("is valid?| " + rvtParam.IsValid.ToString() 
			// 	// 	+ "  value| " + value);
			// }
		}

		private void validateChartSymParams(RevitChartData rcs,
			int dataParamCount, int mustExistParamCount)
		{
			return;

			/*
				// if (mustExistParamCount != RevitChartParameters.MustExistCount)
				if (mustExistParamCount != RevitParamManager.MustExistChartInstance)
				{
					rcs.ErrorCodes = PARAM_CHART_MUST_EXIST_MISSING_CS001138;
				}
	
				if (!rcs.IsValid)
				{
					rcs.ErrorCodes = PARAM_CHART_PARAM_HAS_ERROR_CS001135;
				}
	
			*/
		}

		private int[] labelParamCount;
		private int dataParamCount = 0;

		public RevitCellData catagorizeCellParams(AnnotationSymbol aSym, CellFamily cellFamily)
		{
			dataParamCount = 0;
			labelParamCount = new int[RevitParamManager.MAX_LABELS_PER_CELL];

			RevitCellData rcd = new RevitCellData(cellFamily);

			// ARevitParam rvtParam;
			// int dataParamCount = 0;
			// int[] labelParamCount = new int[12];

			// int labelId;
			// bool isLabel;
			// ParamDesc pd;

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
					// RevitLabel label = saveLabelParam(labelId, param, pd, rcd);

					ARevitParam labelParam = catagorizeParameter(param, pd);

					// Debug.Write("@PT_Label| ");
					// Debug.Write(" rcd.name| " + rcd.Name.PadRight(18));
					// Debug.Write(" id| " + labelId.ToString("##0"));
					// Debug.Write(" idx| " + pd.Index.ToString("##0"));
					// Debug.Write(" name| " + pd.ParameterName.PadRight(18));
					// Debug.Write(" must exist| " + pd.Exist.ToString().PadRight(18));
					// Debug.WriteLine(" req'd| " + pd.IsRequired.ToString());

					bool answer = rcd.AddLabelParam(labelId, pd.Index, labelParam);

					// Debug.Write("\n");

					if (!answer)
					{
						rcd.ErrorCode = LBL_INVALID_CS001125;
					}

					return;
				}
			// case ParamType.PT_INTERNAL:
			// 	{
			// 		dataParamCount++;
			// 		if (pd.DataType == ParamDataType.DT_IGNORE) return;
			// 		rvtParam = catagorizeParameter(param, pd);
			// 		rcd.AddInternal(pd.Index, rvtParam);
			// 		break;
			// 	}
			// case ParamType.PT_TYPE:
			// 	{
			// 		dataParamCount++;
			// 		if (pd.DataType == ParamDataType.DT_IGNORE) return;
			// 		rvtParam = catagorizeParameter(param, pd);
			// 		rcd.AddType(pd.Index, rvtParam);
			// 		break;
			// 	}
			default:
				{
					rvtParam = ARevitParam.Invalid;
					rvtParam.ErrorCode = CEL_INVALID_TYPE_CS001113;
					rcd.Add(PT_INSTANCE, pd.Index, rvtParam);
					break;
				}
			}

			if (rvtParam.HasErrors)
			{
				rcd.ErrorCode = CEL_HAS_ERROR_CS001107;
			}
		}

		private void validateRcdMustExist(RevitCellData rcd)
		{
			int lblCountIdx = rcd.NumberOfLists;
			int maxLabels = rcd.ListOfLabels.Count + lblCountIdx;

			for (int i = 0; i < lblCountIdx - 1; i++)
			{
				if (rcd.CellFamily.ParamMustExistCount[i] != rcd.ReqdParamCount[i])
				{
					rcd.ErrorCode =	ErrorCodesAssist.EC_MustExist[i];
				}
			}

			for (int i = lblCountIdx; i < maxLabels; i++)
			{
				if (rcd.CellFamily.ParamMustExistCount[lblCountIdx - 1] != rcd.ReqdParamCount[i])
				{
					rcd.ErrorCode =	ErrorCodesAssist.EC_MustExist[lblCountIdx - 1];
				}
			}
		}


		// private RevitLabel saveLabelParam(int labelId, Parameter param, ParamDesc pd, 
		// 	RevitCellData rcd)
		// {
		// 	// Dictionary<string, RevitLabel> labels = revitCellData.ListOfLabels;
		//
		// 	RevitLabel label = getLabel(labelId, rcd.CellFamily.ParamCounts[(int) PT_LABEL],
		// 		rcd.ListOfLabels);
		//
		// 	ARevitParam labelParam = catagorizeParameter(param, pd);
		// 	
		// 	label.Add(PT_LABEL, pd.Index, labelParam);
		//
		// 	return label;
		// }
		//
		// private RevitLabel getLabel(int idx, int numOfParams,
		// 	SortedDictionary<string, RevitLabel> labels)
		// {
		// 	RevitLabel label;
		//
		// 	string key = RevitParamUtil.MakeLabelKey(idx);
		//
		// 	bool result = labels.TryGetValue(key, out label);
		//
		// 	if (!result)
		// 	{
		// 		label = new RevitLabel(idx, numOfParams);
		// 		labels.Add(key, label);
		// 	}
		//
		// 	return label;
		// }

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