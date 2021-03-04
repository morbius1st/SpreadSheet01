#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using UtilityLibrary;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.RevitSupport.RevitCellParameters;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamReadReqmt;

#endregion

// username: jeffs
// created:  2/14/2021 10:56:46 PM

namespace SpreadSheet01.RevitSupport
{
	public class ChartList
	{
		public List<RevitChartItem> Charts { get; } = new List<RevitChartItem>();

		public void Add(RevitChartItem revitChart)
		{
			Charts.Add(revitChart);
		}
	}


	public class RevitManager : INotifyPropertyChanged
	{
	#region private fields

		private const string ROOT_TRANSACTION_NAME = "Transaction Name";

		private const string KEY_ADDR_BEGIN = "〖";
		private const string KEY_ADDR_END   = "〗";
		private const string KEY_IDX_BEGIN  = "《";
		private const string KEY_IDX_END    = "》";

		private Dictionary<string, RevitCellParams> CellItemsBySeqName { get; set; }
		private Dictionary<string, RevitCellParams> CellItemsByNameSeq { get; set; }

		private ChartList chartList;

		private ICollection<Element> chartFamilies;
		private ICollection<Element> cellFamilies;

		private RevitChartItem selectedChart;

		private List<RevitCellParams> errorList;

		private RevitAnnoSyms annoSyms;


	#endregion

	#region ctor

		public RevitManager()
		{
			chartList = new ChartList();
			errorList = new List<RevitCellParams>();
			annoSyms = new RevitAnnoSyms();
		}

	#endregion

	#region public properties

		public ChartList ChartList => chartList;

		public RevitChartItem SelectedChart => selectedChart;

		public bool GotCellFamilies { get; private set; }

		public bool GotChart => selectedChart.IsValid;

		public RevitAnnoSyms Symbols => annoSyms;

	#endregion

	#region private properties

	#endregion

	#region public methods


		public void GetCharts()
		{
			TaskDialog td;

			chartFamilies = FindGenericAnnotationByName(RevitDoc.Doc, "SpreadSheetData");

			if (chartFamilies == null && chartFamilies.Count == 0)
			{
				td = new TaskDialog("Spread Sheets");
				td.MainContent = "No charts found";
				td.MainIcon = TaskDialogIcon.TaskDialogIconError;
				td.CommonButtons = TaskDialogCommonButtons.Ok;
				td.Show();

				return;
			}

			getCharts(chartFamilies);

			OnPropertyChanged(nameof(ChartList));
			
		}



		public string getChart(Document doc, int whichChart = 0)
		{

			TaskDialog td;

			chartFamilies = FindGenericAnnotationByName(doc, "SpreadSheetData");

			if (chartFamilies == null && chartFamilies.Count == 0)
			{
				td = new TaskDialog("Spread Sheets");
				td.MainContent = "No charts found";
				td.MainIcon = TaskDialogIcon.TaskDialogIconError;
				td.CommonButtons = TaskDialogCommonButtons.Ok;
				td.Show();

				return null;
			}

			getCharts(chartFamilies);

			string chartPath = chartList.Charts[whichChart].ChartPath;
			string chartWorkSheet = chartList.Charts[whichChart].ChartWorkSheet;

			if (chartPath.IsVoid() || chartWorkSheet.IsVoid())
			{
				td = new TaskDialog("Spread Sheets");
				td.MainContent = "Incomplete Chart Information";
				td.MainIcon = TaskDialogIcon.TaskDialogIconError;
				td.CommonButtons = TaskDialogCommonButtons.Ok;
				td.Show();

				return null;
			}

			selectedChart = chartList.Charts[whichChart];

			return chartList.Charts[whichChart].ChartFamilyTypeName;
		}

		// create a list of annotation families of the proper type
		public bool GetCellFamilies(Document doc, string familyTypeName)
		{
			cellFamilies =
				SelectbyCatAndMatchStringParameter(doc,
					BuiltInCategory.OST_GenericAnnotation, familyTypeName);

			if (cellFamilies == null || cellFamilies.Count == 0)
			{
				errorNoCells(familyTypeName);
				return false;
			}

			GotCellFamilies = true;

			return true;
		}

		public bool GetAllCellParameters()
		{
			if (!GotCellFamilies) return false;

			foreach (Element el in cellFamilies)
			{
				AnnotationSymbol annoSym = el as AnnotationSymbol;

				RevitAnnoSym rvtAnnoSym = catagorizeAnnoSymParams(annoSym);

				rvtAnnoSym.RvtElement = el;
				rvtAnnoSym.AnnoSymbol = annoSym;

				string key = RevitValueSupport.MakeAnnoSymKey(rvtAnnoSym, false);

				annoSyms.Add(key, rvtAnnoSym);
			}

			return true;
		}



		// public bool GetCells(Document doc, string familyTypeName)
		// {
		// 	cellFamilies =
		// 		SelectbyCatAndMatchStringParameter(doc, BuiltInCategory.OST_GenericAnnotation, familyTypeName);
		//
		// 	if (cellFamilies == null && cellFamilies.Count == 0)
		// 	{
		// 		errorNoCells(familyTypeName);
		// 		return false;
		// 	}
		//
		// 	RevitCellParams cp;
		//
		// 	CellItemsBySeqName = new Dictionary<string, RevitCellParams>();
		// 	CellItemsByNameSeq = new Dictionary<string, RevitCellParams>();
		//
		// 	foreach (Element cellFamily in cellFamilies)
		// 	{
		// 		cp = categorizeCellParameters(cellFamily as AnnotationSymbol);
		//
		// 		if (cp.HasError == true)
		// 		{
		// 			errorList.Add(cp);
		// 			continue;
		// 		}
		//
		// 		string keySeqName = makeKey(cp, true);
		// 		string keyNameSeq = makeKey(cp, false);
		//
		// 		CellItemsBySeqName.Add(keySeqName, cp);
		// 		CellItemsByNameSeq.Add(keyNameSeq, cp);
		// 	}
		//
		// 	return errorList.Count == 0;
		// }


		// get all family instances that
		// are generic annotation and
		// have the name provided
		public ICollection<Element> FindGenericAnnotationByName(Document doc, string typeName)
		{
			ParameterValueProvider provider =
				new ParameterValueProvider(new ElementId((int) BuiltInParameter.ALL_MODEL_FAMILY_NAME));

			string stringRuleValue = typeName;

			FilterStringRuleEvaluator sre = new FilterStringEquals();

			FilterRule rule = new FilterStringRule(provider, sre, stringRuleValue, true);

			ElementParameterFilter filter = new ElementParameterFilter(rule);

			FilteredElementCollector col
				= new FilteredElementCollector(doc)
				.OfCategory(BuiltInCategory.OST_GenericAnnotation).WhereElementIsNotElementType().WherePasses(filter);

			ICollection<Element> charts = col.ToElements();

			// foreach (Element e in charts)
			// {
			// 	int found = 0;
			//
			// 	RevitChartItem ci = new RevitChartItem();
			//
			// 	foreach (Parameter p in e.ParametersMap)
			// 	{
			// 		string name = p.Definition.Name;
			//
			// 		foreach (KeyValuePair<string, int> kvp in RevitChartItem.ChartItemIds)
			// 		{
			// 			if (name.Equals(kvp.Key))
			// 			{
			// 				cp.Chart[kvp.Value] = p.AsString();
			// 				found++;
			// 			}
			// 		}
			//
			// 		if (found == RevitChartItem.DataParamCount) break;
			// 	}
			//
			// 	if (ci != null) chartList.Add(ci);
			// }

			return charts;
		}

		public ICollection<Element> SelectbyCatAndMatchStringParameter(Document doc, BuiltInCategory category,
			string matchString)
		{
			ParameterValueProvider provider =
				new ParameterValueProvider(new ElementId((int) BuiltInParameter.ALL_MODEL_FAMILY_NAME));

			string stringRuleValue = matchString;

			FilterStringRuleEvaluator sre = new FilterStringEquals();

			FilterRule rule = new FilterStringRule(provider, sre, stringRuleValue, true);

			ElementParameterFilter filter = new ElementParameterFilter(rule);

			FilteredElementCollector col
				= new FilteredElementCollector(doc)
				.OfCategory(category).WhereElementIsNotElementType().WherePasses(filter);

			return col.ToElements();
		}

	#endregion

	#region private methods

		private RevitAnnoSym catagorizeAnnoSymParams(AnnotationSymbol aSym)
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
				string paramName = RevitValueSupport.GetParamName(param.Definition.Name, out labelId, out isLabel);

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
						Debug.WriteLine("got container");
						containerParamCount++;
						if (pd.DataType == ParamDataType.IGNORE) continue;

						RevitLabels labels = (RevitLabels) ras[LabelsIdx];

						// RevitLabel label = getLabel(labelId, labels);
						//
						// ARevitParam labelParam = catagorizeParameter(param, pd);
						//
						// label.Add(pd.Index, labelParam);

						saveLabelParam(labelId, param, pd, labels);

						break;
					}
				case ParamGroup.LABEL:
					{
						labelParamCount++;

						if (labelId < 0 || pd.DataType == ParamDataType.IGNORE) continue;

						RevitLabels labels = (RevitLabels) ras[LabelsIdx];

						// RevitLabel label = getLabel(labelId, labels);
						//
						// ARevitParam labelParam = catagorizeParameter(param, pd);
						//
						// label.Add(pd.Index, labelParam);

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

			string key = RevitValueSupport.MakeLabelKey(idx);

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
			case ParamDataType.LABEL_TITLE:
				{
					p = new RevitParamLabel("", pd);
					((RevitParamLabel) p).LabelValueName = param.Definition.Name;
					break;
				}
			}

			return p;
		}

		private void getCharts(ICollection<Element> charts)
		{
			foreach (Element e in charts)
			{
				int found = 0;

				RevitChartItem cp = new RevitChartItem();

				foreach (Parameter p in e.ParametersMap)
				{
					string name = p.Definition.Name;

					foreach (KeyValuePair<string, int> kvp in RevitChartItem.ChartItemIds)
					{
						if (name.Equals(kvp.Key))
						{
							cp.Chart[kvp.Value] = p.AsString();
							found++;
						}
					}

					if (found == RevitChartItem.ItemIdCount) break;
				}

				if (cp != null) chartList.Add(cp);
			}
		}

		private void errorNoCells(string familyTypeName)
		{
			TaskDialog td = new TaskDialog("Spread Sheet Cells for| " + familyTypeName);
			td.MainContent = "No cells found";
			td.MainIcon = TaskDialogIcon.TaskDialogIconError;
			td.CommonButtons = TaskDialogCommonButtons.Ok;
			td.Show();
		}

		/*
		private string getKey(string name, string cellName, string cellAddr)
		{
			string Key;

			if (!name.IsVoid())
			{
				Key = name;
			}
			else if (!cellName.IsVoid())
			{
				Key = cellName;
			}
			else
			{
				Key = cellAddr;
			}

			return Key;
		}

		private string makeKey(RevitCellParams cp, bool asSeqName)
		{
			string seq = ((string) cp[SeqIdx].GetValue());
			seq = seq.IsVoid() ? "ZZZZZZ" : seq;
			seq = KEY_IDX_BEGIN + $"{seq,8}" + KEY_IDX_END;

			string name = cp.Name.IsVoid() ? "Un-named " : cp.Name + " " ;

			string eid = cp.AnnoSymbol == null ? "null element" : cp.AnnoSymbol.Id.ToString();


			if (asSeqName) return seq + name + eid;

			return name + seq + eid;
		}

		private RevitCellParams categorizeCellParameters(AnnotationSymbol annoSym)
		{
			RevitCellParams cp = new RevitCellParams();

			if (annoSym == null)
			{
				cp.Error = RevitCellErrorCode.INVALID_ANNO_SYM_CS001120;
				return cp;
			}

			cp.AnnoSymbol = annoSym;

			IList<Parameter> parameters = annoSym.GetOrderedParameters();

			int paramCount = 1;
			int textCount = 0;

			foreach (Parameter param in parameters)
			{
				string name = param.Definition.Name;

				Debug.WriteLine("process parameter| " + name);

				ParamDesc pd = RevitCellParameters.Match(name);

				Debug.WriteLine("has error A?|" + cp.HasError);

				if (pd == null)
				{
					continue;
				}

				cp.CellParamDataType = pd.DataType;

				int idx = pd.Index;

				Debug.WriteLine("has error B?|" + cp.HasError);

				switch (pd.DataType)
				{
				case ParamDataType.TEXT:
					{
						RevitParamText rs =
							new RevitParamText(
								pd.ReadReqmt == READ_VALUE_IGNORE ? "" : param.AsString(), pd);
						cp[idx] = rs;
						paramCount++;
						break;
					}
				//
				// case ParamDataType.TEXT:
				// 	{ 
				// 		cp.AddText(param.AsString(), name, pd);
				// 		textCount++;
				// 		break;
				// 	}

				case ParamDataType.ADDRESS:
					{
						RevitParamAddr ra = new RevitParamAddr(param.AsString(), pd);
						cp[idx] = ra;
						paramCount++;
						break;
					}

				case ParamDataType.NUMBER:
					{
						RevitParamNumber rn = new RevitParamNumber(
							pd.ReadReqmt == READ_VALUE_IGNORE ? Double.NaN : param.AsDouble(), pd);
						cp[idx] = rn;
						paramCount++;
						break;
					}

				case ParamDataType.BOOL:
					{
						RevitParamBool rb = new RevitParamBool(
							(pd.ReadReqmt == READ_VALUE_IGNORE ? (bool?) null : param.AsInteger() == 1), pd);
						cp[idx] = rb;
						paramCount++;
						break;
					}

				case ParamDataType.IGNORE:
					{
						// RevitParamText rs = new RevitParamText("Not Used", pd);
						// ci[idx] = rs;
						paramCount++;
						break;
					}
				}

				Debug.WriteLine("has error C?|" + cp.HasError);
			}

			if (textCount == 0)
			{
				cp.CellParamDataType = ParamDataType.ERROR;
				cp.Error = RevitCellErrorCode.PARAM_VALUE_MISSING_CS001101;
			}
			else if (paramCount != RevitCellParameters.ParamCounts[(int) ParamGroup.DATA])
			{
				cp.CellParamDataType = ParamDataType.ERROR;
				cp.Error = RevitCellErrorCode.PARAM_MISSING_CS001102;
			}

			Debug.WriteLine("has error D?|" + cp.HasError);

			return cp;
		}

		*/

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is RevitManager";
		}

	#endregion
	}

}