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

			// foreach (Parameter param in elChart.GetOrderedParameters())
			// {
			// 	catagorizeChartSymParam(rcd, param, chartFamily, false);
			//
			// }

			foreach (Parameter param in ((AnnotationSymbol) elChart).Symbol.GetOrderedParameters())
			{
				catagorizeChartSymParam(rcd, param, chartFamily, true);
			}

			validateChartSymParams(rcd, dataParamCount, mustExistParamCount);

			return rcd;
		}


		private void catagorizeChartSymParam(RevitChartData rcd, 
			Parameter param, ChartFamily chartFamily, bool isType)
		{
			ARevitParam rvtParam;
			ParamDesc pd;

			string paramName = param.Definition.Name;

			// Debug.WriteLine("\ncategorizing| " + paramName);

			// pd = Family.Match(paramName, ParamClass.PC_CHART);
			bool result = chartFamily.Match(paramName, out pd, isType);

			if (!result || pd == null || pd.Mode == ParamMode.PM_NOT_USED) return;

			// Debug.WriteLine("got param desc| " + pd.ParameterName 
			// 	+ "  type| " + pd.ParamType + "  data type| " + pd.DataType);

			if (pd.Exist == ParamExistReqmt.EX_PARAM_MUST_EXIST) mustExistParamCount++;

			switch (pd.ParamType)
			{
			case ParamType.PT_INSTANCE:
				{
					dataParamCount++;
					if (pd.DataType == ParamDataType.DT_IGNORE) return;
					rvtParam = catagorizeParameter(param, pd);
					rcd.Add(PT_INSTANCE, pd.Index, rvtParam);
					break;
				}
			case ParamType.PT_INTERNAL:
				{
					dataParamCount++;
					if (pd.DataType == ParamDataType.DT_IGNORE) return;
					rvtParam = catagorizeParameter(param, pd);
					rcd.AddInternal(pd.Index, rvtParam);
					break;
				}
			case ParamType.PT_TYPE:
				{
					dataParamCount++;
					if (pd.DataType == ParamDataType.DT_IGNORE) return;
					rvtParam = catagorizeParameter(param, pd);
					rcd.AddType(pd.Index, rvtParam);
					break;
				}
			default:
				{
					rvtParam = ARevitParam.Invalid;
					rvtParam.ErrorCode = CHART_INVALID_PARAM_TYPE_CS001143;
					rcd.Add(PT_INSTANCE, pd.Index, rvtParam);
					break;
				}
			}

			if (!rvtParam.IsValid)
			{
				// Debug.WriteLine("is not valid");
				rcd.ErrorCode = CHART_PARAM_HAS_ERROR_CS001137;
			} else
			{
				string value = rvtParam.DynValue.AsString();
				// Debug.WriteLine("is valid?| " + rvtParam.IsValid.ToString() 
				// 	+ "  value| " + value);
			}
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
			labelParamCount = new int[12];

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
				catagorizeCellParam(rcd, param, cellFamily, false);

/*
				string paramName = RevitParamUtil.GetRootName(param.Definition.Name,
					out labelId, out isLabel);


				pd = Family.Match(paramName, ParamClass.PC_CELL);

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
*/
			}

			foreach (Parameter param in  aSym.Symbol.GetOrderedParameters())
			{
				catagorizeCellParam(rcd, param, cellFamily, true);
			}

			return rcd;
		}

		private void catagorizeCellParam(RevitCellData rcd, Parameter param, 
			CellFamily cellFamily, bool isType)
		{
			ARevitParam rvtParam = null;
			ParamDesc pd;

			int labelId;
			bool isLabel;

			string paramRootName = RevitParamUtil.GetRootName(param.Definition.Name,
				out labelId, out isLabel);

			ParamType pt = isLabel ? ParamType.PT_LABEL : ParamType.PT_INSTANCE;

			bool result = cellFamily.Match(paramRootName, out pd, isType);

			if (! result || pd == null || pd.Mode == ParamMode.PM_NOT_USED) return;
			
			switch (pd.ParamType)
			{
			case ParamType.PT_INSTANCE:
				{
					dataParamCount++;
					if (pd.DataType == ParamDataType.DT_IGNORE) return;

					rvtParam = catagorizeParameter(param, pd);

					rcd.Add(PT_INSTANCE, pd.Index, rvtParam);
					break;
				}
			case ParamType.PT_LABEL:
				{
					labelParamCount[labelId]++;
					if (pd.DataType == ParamDataType.DT_IGNORE) return;

					RevitLabel label = saveLabelParam(labelId, param, pd, rcd);

					if (pd.Index == RevitParamManager.LblNameIdx)
					{
						rcd.AddLabelRef(label);
					}

					if (!label.IsValid)
					{
						rcd.ErrorCode = LABEL_INVALID_CS001125;
					}
					return;
				}
			case ParamType.PT_INTERNAL:
				{
					dataParamCount++;
					if (pd.DataType == ParamDataType.DT_IGNORE) return;

					rvtParam = catagorizeParameter(param, pd);

					rcd.AddInternal(pd.Index, rvtParam);
					break;
				}
			case ParamType.PT_TYPE:
				{
					dataParamCount++;
					if (pd.DataType == ParamDataType.DT_IGNORE) return;

					rvtParam = catagorizeParameter(param, pd);

					rcd.AddType(pd.Index, rvtParam);
					break;
				}
			default:
				{
					rvtParam = ARevitParam.Invalid;
					rvtParam.ErrorCode = PARAM_INVALID_TYPE_CS001113;
					rcd.Add(PT_INSTANCE, pd.Index, rvtParam);

					break;
				}
			}

			if (!rvtParam.IsValid)
			{
				rcd.ErrorCode = PARAM_HAS_ERROR_CS001107;
			}
		}

		private RevitLabel saveLabelParam(int labelId, Parameter param, ParamDesc pd, 
			RevitCellData revitCellData)
		{
			Dictionary<string, RevitLabel> labels = revitCellData.ListOfLabels;

			RevitLabel label = getLabel(labelId, revitCellData.CellFamily.ParamCounts[(int) PT_LABEL],
				labels);

			ARevitParam labelParam = catagorizeParameter(param, pd);

			label.Add(PT_INSTANCE, pd.Index, labelParam);

			return label;
		}

		private RevitLabel getLabel(int idx, int numOfParams,
			Dictionary<string, RevitLabel> labels)
		{
			RevitLabel label;

			string key = RevitParamUtil.MakeLabelKey(idx);

			bool result = labels.TryGetValue(key, out label);

			if (!result)
			{
				label = new RevitLabel(numOfParams);
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