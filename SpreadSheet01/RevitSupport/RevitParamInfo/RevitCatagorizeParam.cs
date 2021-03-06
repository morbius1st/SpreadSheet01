﻿using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitChartInfo;
using SpreadSheet01.RevitSupport.RevitParamInfo;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.RevitSupport.RevitParamValue.RevitCellErrorCode;



// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitCatagorizeParam.cs
// Created:      2021-03-03 (7:31 PM)


namespace SpreadSheet01.RevitSupport
{
	public class RevitCatagorizeParam
	{
		public RevitChartSym CatagorizeChartSymParams(Element elChart, ParamClass paramClass)
		{
			RevitChartSym rcs = new RevitChartSym();
			ARevitParam rvtParam;

			int dataParamCount = 0;
			int mustExistParamCount = 0;

			ParamDesc pd;

			foreach (Parameter param in elChart.GetOrderedParameters())
			{
				string paramName = param.Definition.Name;

				pd = RevitChartParameters.Match(paramName);

				if (pd == null) continue;

				if (pd.Exist == ParamExistReqmt.PARAM_MUST_EXIST) mustExistParamCount++;

				switch (pd.Group)
				{
				case ParamGroup.DATA:
					{
						dataParamCount++;
						if (pd.DataType == ParamDataType.IGNORE) continue;

						rvtParam = catagorizeParameter(param, pd);

						rcs.Add(pd.Index, rvtParam);
						break;
					}
				default:
					{
						rvtParam = ARevitParam.Invalid;
						rvtParam.ErrorCode = PARAM_CHART_INVALID_PROG_GRP_CS001140;
						rcs.Add(pd.Index, rvtParam);

						break;
					}
				}

				if (!rvtParam.IsValid) rcs.ErrorCode = PARAM_CHART_PARAM_HAS_ERROR_CS001135;

			}
			validateChartSymParams(rcs, mustExistParamCount, mustExistParamCount);

			return rcs;
		}

		private void validateChartSymParams(RevitChartSym rcs, 
			int dataParamCount, int mustExistParamCount)
		{
			if (mustExistParamCount != RevitChartParameters.MustExistCount)
			{
				rcs.ErrorCode = PARAM_CHART_MUST_EXIST_MISSING_CS001138;
			}

			if (!rcs.IsValid)
			{
				rcs.ErrorCode = PARAM_CHART_PARAM_HAS_ERROR_CS001135;
			}
		}




		public RevitAnnoSym catagorizeAnnoSymParams(AnnotationSymbol aSym, ParamClass paramClass)
		{
			RevitAnnoSym ras = new RevitAnnoSym();
			ARevitParam rvtParam;

			int dataParamCount = 0;
			int labelParamCount = 0;
			int containerParamCount = 0;

			int labelId;
			bool isLabel;
			ParamDesc pd;

			foreach (Parameter param in aSym.GetOrderedParameters())
			{
				string paramName = RevitParamUtil.GetParamName(param.Definition.Name, paramClass,
					out labelId, out isLabel);

				pd = RevitCellParameters.Match(paramName);

				if (pd == null) continue;

				switch (pd.Group)
				{
				case ParamGroup.DATA:
					{
						dataParamCount++;
						if (pd.DataType == ParamDataType.IGNORE) continue;

						rvtParam = catagorizeParameter(param, pd);

						ras.Add(pd.Index, rvtParam);
						break;
					}
				case ParamGroup.CONTAINER:
					{
						containerParamCount++;
						if (pd.DataType == ParamDataType.IGNORE) continue;

						RevitLabels labels = (RevitLabels) ras[RevitCellParameters.LabelsIdx];

						saveLabelParam(labelId, param, pd, labels);

						break;
					}
				case ParamGroup.LABEL:
					{
						labelParamCount++;
						if (labelId < 0 || pd.DataType == ParamDataType.IGNORE) continue;

						RevitLabels labels = (RevitLabels) ras[RevitCellParameters.LabelsIdx];

						saveLabelParam(labelId, param, pd, labels);

						break;
					}
				}
			}

			return ras;
		}

		private void saveLabelParam(int labelId, Parameter param, ParamDesc pd, RevitLabels labels)
		{
			RevitLabel label = getLabel(labelId, labels);

			ARevitParam labelParam = catagorizeParameter(param, pd);

			label.Add(pd.Index, labelParam);
		}

		private RevitLabel getLabel(int idx, RevitLabels labels)
		{
			RevitLabel label = null;

			string key = RevitParamUtil.MakeLabelKey(idx);

			bool result = labels.Containers.TryGetValue(key, out label);

			if (!result)
			{
				label = new RevitLabel();
				labels.Containers.Add(key, label);
			}

			return label;
		}

		private ARevitParam catagorizeParameter(Parameter param, ParamDesc pd, string name = "")
		{
			ARevitParam p = null;

			switch (pd.DataType)
			{
			case ParamDataType.BOOL:
				{
					p = new RevitParamBool(
						pd.ReadReqmt ==
						ParamReadReqmt.READ_VALUE_IGNORE
							? (bool?) false
							: param.AsInteger() == 1, pd);
					break;
				}
			case ParamDataType.NUMBER:
				{
					p = new RevitParamNumber(
						pd.ReadReqmt ==
						ParamReadReqmt.READ_VALUE_IGNORE
							? double.NaN
							: param.AsDouble(), pd);
					break;
				}
			case ParamDataType.TEXT:
				{
					p = new RevitParamText(pd.ReadReqmt ==
						ParamReadReqmt.READ_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			case ParamDataType.DATATYPE:
				{
					p = new RevitParamText(pd.ReadReqmt ==
						ParamReadReqmt.READ_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			case ParamDataType.ADDRESS:
				{
					p = new RevitParamText(pd.ReadReqmt ==
						ParamReadReqmt.READ_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			case ParamDataType.RELATIVEADDRESS:
				{
					p = new RevitParamRelativeAddr(pd.ReadReqmt ==
						ParamReadReqmt.READ_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			case ParamDataType.FILE_PATH:
				{
					p = new RevitParamFilePath(pd.ReadReqmt ==
						ParamReadReqmt.READ_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			case ParamDataType.UPDATE_TYPE:
				{
					p = new RevitParamUpdateType(pd.ReadReqmt ==
						ParamReadReqmt.READ_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			case ParamDataType.WORKSHEETNAME:
				{
					p = new RevitParamWkShtName(pd.ReadReqmt ==
						ParamReadReqmt.READ_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			case ParamDataType.LABEL_TITLE:
				{
					p = new RevitParamLabel("", pd);
					((RevitParamLabel) p).LabelValueName = param.Definition.Name;
					break;
				}
			}

			return p;
		}
	}
}