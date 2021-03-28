using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using Family = SpreadSheet01.RevitSupport.RevitCellsManagement.Family;
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
			RevitChartData rcd = new RevitChartData();
			ARevitParam rvtParam;

			int dataParamCount = 0;
			int mustExistParamCount = 0;

			ParamDesc pd;

			rcd.RevitElement = elChart;
			rcd.AnnoSymbol = (AnnotationSymbol) elChart;

		#if REVIT
			IList<Parameter> p = elChart.GetOrderedParameters();
			ParameterMap pm = elChart.ParametersMap;
			ParameterSet ps = elChart.Parameters;
		#endif


			foreach (Parameter param in elChart.GetOrderedParameters())
			{
				string paramName = param.Definition.Name;

				pd = Family.Match(paramName, ParamClass.PC_CHART);
				// pd = RevitChartParameters.Match(paramName);

				if (pd == null || pd.Mode == ParamMode.PM_NOT_USED) continue;

				Debug.WriteLine("pd=" + pd.ParameterName + " (" + paramName + ") "
					+ " must exist? " + (pd.Exist == ParamExistReqmt.EX_PARAM_MUST_EXIST).ToString());


				if (pd.Exist == ParamExistReqmt.EX_PARAM_MUST_EXIST) mustExistParamCount++;

				switch (pd.Type)
				{
				case ParamType.PT_PARAM:
					{
						dataParamCount++;
						if (pd.DataType == ParamDataType.DT_IGNORE) continue;

						rvtParam = catagorizeParameter(param, pd);

						rcd.Add(pd.Index, rvtParam);
						break;
					}

				case ParamType.PT_INTERNAL:
					{
						dataParamCount++;
						if (pd.DataType == ParamDataType.DT_IGNORE) continue;

						rvtParam = catagorizeParameter(param, pd);

						rcd.AddInternal(pd.Index, rvtParam);

						break;
					}

				default:
					{
						rvtParam = ARevitParam.Invalid;
						rvtParam.ErrorCode = PARAM_CHART_INVALID_PROG_GRP_CS001140;
						rcd.Add(pd.Index, rvtParam);

						break;
					}
				}

				if (!rvtParam.IsValid) rcd.ErrorCode = PARAM_CHART_PARAM_HAS_ERROR_CS001135;
			}

			validateChartSymParams(rcd, mustExistParamCount, mustExistParamCount);

			return rcd;
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


		public RevitCellData catagorizeCellParams(AnnotationSymbol aSym)
		{
			RevitCellData rcd = new RevitCellData();
			ARevitParam rvtParam;

			int dataParamCount = 0;
			int[] labelParamCount = new int[12];


			int labelId;
			bool isLabel;
			ParamDesc pd;

			foreach (Parameter param in aSym.GetOrderedParameters())
			{
				string paramName = RevitParamUtil.GetRootName(param.Definition.Name,
					out labelId, out isLabel);


				pd = Family.Match(paramName, ParamClass.PC_CELL);
				//
				// if (isLabel)
				// {
				// 	pd = RevitParamManager.MatchCellLabel(paramName);
				// }
				// else
				// {
				// 	pd = RevitParamManager.MatchCellInstance(paramName);
				// }

				if (pd == null) continue;

				switch (pd.Type)
				{
				case ParamType.PT_PARAM:
					{
						dataParamCount++;
						if (pd.DataType == ParamDataType.DT_IGNORE) continue;

						rvtParam = catagorizeParameter(param, pd);

						rcd.Add(pd.Index, rvtParam);
						break;
					}
				case ParamType.PT_LABEL:
					{
						labelParamCount[labelId]++;
						if (pd.DataType == ParamDataType.DT_IGNORE) continue;

						RevitLabel label = saveLabelParam(labelId, param, pd, rcd);

						if (pd.Index == RevitParamManager.LblNameIdx)
						{
							rcd.AddLabelRef(label);
						}

						break;
					}
				case ParamType.PT_INTERNAL:
					{
						dataParamCount++;
						if (pd.DataType == ParamDataType.DT_IGNORE) continue;

						rvtParam = catagorizeParameter(param, pd);

						rcd.AddInternal(pd.Index, rvtParam);
						break;
					}
				}
			}

			return rcd;
		}


		private RevitLabel saveLabelParam(int labelId, Parameter param, ParamDesc pd, RevitCellData revitCellData)
		{
			Dictionary<string, RevitLabel> labels = revitCellData.ListOfLabels;

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

	}
}