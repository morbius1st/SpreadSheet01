using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.Management.ErrorCodes;


// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitCatagorizeParam.cs
// Created:      2021-03-03 (7:31 PM)


namespace SpreadSheet01.RevitSupport.RevitParamManagement
{
	public class RevitParamCatagorize
	{
		public RevitChartData CatagorizeChartSymParams(Element elChart)
		{
			RevitChartData rcs = new RevitChartData();
			ARevitParam rvtParam;

			int dataParamCount = 0;
			int mustExistParamCount = 0;

			ParamDesc pd;

			rcs.RevitElement = elChart;
			rcs.AnnoSymbol = (AnnotationSymbol) elChart;

			foreach (Parameter param in elChart.GetOrderedParameters())
			{
				string paramName = param.Definition.Name;

				pd = RevitParamManager.MatchChartInstance(paramName);
				// pd = RevitChartParameters.Match(paramName);

				if (pd == null || pd.Mode == ParamMode.NOT_USED) continue;

				Debug.WriteLine("pd=" + pd.ParameterName + " (" + paramName + ") "
					+ " must exist? " + (pd.Exist == ParamExistReqmt.PARAM_MUST_EXIST).ToString());


				if (pd.Exist == ParamExistReqmt.PARAM_MUST_EXIST) mustExistParamCount++;

				switch (pd.Type)
				{
				case ParamType.INSTANCE:
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
						rvtParam.ErrorCodes = PARAM_CHART_INVALID_PROG_GRP_CS001140;
						rcs.Add(pd.Index, rvtParam);

						break;
					}
				}

				if (!rvtParam.IsValid) rcs.ErrorCodes = PARAM_CHART_PARAM_HAS_ERROR_CS001135;
			}

			validateChartSymParams(rcs, mustExistParamCount, mustExistParamCount);

			return rcs;
		}

		private void validateChartSymParams(RevitChartData rcs,
			int dataParamCount, int mustExistParamCount)
		{
			// if (mustExistParamCount != RevitChartParameters.MustExistCount)
			if (mustExistParamCount != RevitParamManager.MustExistChartInstance)
			{
				rcs.ErrorCodes = PARAM_CHART_MUST_EXIST_MISSING_CS001138;
			}

			if (!rcs.IsValid)
			{
				rcs.ErrorCodes = PARAM_CHART_PARAM_HAS_ERROR_CS001135;
			}
		}


		public RevitCell catagorizeCellParams(AnnotationSymbol aSym)
		{
			RevitCell rvtCell = new RevitCell();
			ARevitParam rvtParam;

			int dataParamCount = 0;
			int[] labelParamCount = new int[12];
			int containerParamCount = 0;

			int labelId;
			bool isLabel;
			ParamDesc pd;

			foreach (Parameter param in aSym.GetOrderedParameters())
			{
				string paramName = RevitParamUtil.GetRootName(param.Definition.Name,
					out labelId, out isLabel);

				if (isLabel)
				{
					pd = RevitParamManager.MatchCellLabel(paramName);
				}
				else
				{
					pd = RevitParamManager.MatchCellInstance(paramName);
				}

				if (pd == null) continue;

				switch (pd.Type)
				{
				case ParamType.INSTANCE:
					{
						dataParamCount++;
						if (pd.DataType == ParamDataType.IGNORE) continue;

						rvtParam = catagorizeParameter(param, pd);

						rvtCell.Add(pd.Index, rvtParam);
						break;
					}
				case ParamType.LABEL:
					{
						labelParamCount[labelId]++;
						if (pd.DataType == ParamDataType.IGNORE) continue;

						RevitLabel label = saveLabelParam(labelId, param, pd, rvtCell);

						if (pd.Index == RevitParamManager.LblNameIdx)
						{
							rvtCell.AddLabelRef(label);
						}

						break;
					}
				}
			}

			return rvtCell;
		}


		private RevitLabel saveLabelParam(int labelId, Parameter param, ParamDesc pd, RevitCell rvtCell)
		{
			Dictionary<string, RevitLabel> labels = rvtCell.ListOfLabels;

			RevitLabel label = getLabel(labelId, labels);

			ARevitParam labelParam = catagorizeParameter(param, pd);

			label.Add(pd.Index, labelParam);

			return label;
		}

		private RevitLabel getLabel(int idx, Dictionary<string, RevitLabel> labels)
		{
			RevitLabel label;

			string key = RevitParamUtil.MakeLabelKey(idx);

			bool result = labels.TryGetValue(key, out label);

			if (!result)
			{
				label = new RevitLabel();
				labels.Add(key, label);
			}

			return label;
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


		private ARevitParam catagorizeParameter(Parameter param, ParamDesc pd, string name = "")
		{
			ARevitParam p = null;

			// pd.InvokeDelegate(param);

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
			case ParamDataType.FORMULA:
				{
					p = new RevitParamFormula(pd.ReadReqmt ==
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
			case ParamDataType.INTEGER:
				{
					p = new RevitParamNumber(
						pd.ReadReqmt ==
						ParamReadReqmt.READ_VALUE_IGNORE
							? double.NaN
							: param.AsDouble(), pd);
					break;
				}
			}

			return p;
		}

	}
}