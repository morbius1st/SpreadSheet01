#region using

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
	public class RevitManager
	{
	#region private fields

		private const string ROOT_TRANSACTION_NAME = "Transaction Name";

		private const string KEY_ADDR_BEGIN = "〖";
		private const string KEY_ADDR_END   = "〗";
		private const string KEY_IDX_BEGIN  = "《";
		private const string KEY_IDX_END    = "》";

		private ChartList chartList = new ChartList();

		private Dictionary<string, RevitCellParams> CellItemsBySeqName { get; set; }
		private Dictionary<string, RevitCellParams> CellItemsByNameSeq { get; set; }

		private ICollection<Element> chartFamilies;
		private ICollection<Element> cellFamilies;

		private List<RevitCellParams> errorList = new List<RevitCellParams>();

	#endregion

	#region ctor

		public RevitManager() { }

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

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

			string chartPath = chartList.Charts[whichChart].Path;
			string chartWorkSheet = chartList.Charts[whichChart].WorkSheet;

			if (chartPath.IsVoid() || chartWorkSheet.IsVoid())
			{
				td = new TaskDialog("Spread Sheets");
				td.MainContent = "Incomplete Chart Information";
				td.MainIcon = TaskDialogIcon.TaskDialogIconError;
				td.CommonButtons = TaskDialogCommonButtons.Ok;
				td.Show();

				return null;
			}

			return chartList.Charts[whichChart].FamilyTypeName;
		}

		// public bool GetCells2(Document doc, string familyTypeName)
		// {
		// 	// select all matching cells
		// 	cellFamilies =
		// 		SelectbyCatAndMatchStringParameter(doc, BuiltInCategory.OST_GenericAnnotation, familyTypeName);
		//
		// 	if (cellFamilies == null && cellFamilies.Count == 0)
		// 	{
		// 		errorNoCells(familyTypeName);
		// 		return false;
		// 	}
		//
		// 	string key;
		// 	RevitCellParams cp;
		//
		// 	CellItemsBySeqName = new Dictionary<string, RevitCellParams>();
		//
		// 	foreach (Element cellFamily in cellFamilies)
		// 	{
		// 		int[] fails = new int[ReadParamFromFamily.Length];
		// 		int failIdx = 0;
		//
		// 		ci = new RevitCellParams();
		//
		// 		cp.AnnoSymbol = cellFamily as AnnotationSymbol;
		//
		// 		IList<Parameter> parameters = cellFamily.GetOrderedParameters();
		//
		// 		// FamilyInstance fi = cellFamily as FamilyInstance;
		// 		// AnnotationSymbol asx = cellFamily as AnnotationSymbol;
		// 		//
		// 		// ICollection<ElementId> se = fi.GetSubComponentIds();
		// 		// ICollection<Subelement> ee = fi.GetSubelements();
		// 		// IList<Subelement> sx = fi.Symbol.GetSubelements();
		//
		//
		// 		for (var i = 0; i < ReadParamFromFamily.GetLength(0); i++)
		// 		{
		// 			int idx = ReadParamFromFamily[i];
		//
		// 			try
		// 			{
		// 				DataType paramDataType = CellParamInfo[idx].DataType;
		// 				string paramName = CellParamInfo[idx].ParameterName;
		//
		//
		// 				switch (paramDataType)
		// 				{
		// 				case DataType.TEXT:
		// 					{
		// 						break;
		// 					}
		// 				case DataType.TEXT:
		// 					{
		// 						ci[idx] =
		// 							cellFamily.LookupParameter(paramName)?.AsString() ?? "";
		// 						cp.CellParamDataType = DataType.TEXT;
		//
		// 						break;
		// 					}
		// 				case DataType.NUMBER:
		// 					{
		// 						ci[idx] =
		// 							cellFamily.LookupParameter(paramName)?.AsDouble() ?? 0.0;
		// 						cp.CellParamDataType = DataType.NUMBER;
		//
		// 						break;
		// 					}
		// 				case DataType.BOOL:
		// 					{
		// 						ci[idx] =
		// 							(cellFamily.LookupParameter(paramName)?.AsInteger() ?? 1) == 1;
		// 						cp.CellParamDataType = DataType.BOOL;
		//
		// 						break;
		// 					}
		// 				default:
		// 					{
		// 						cp.CellParamDataType = DataType.ERROR;
		//
		// 						break;
		// 					}
		// 				}
		// 			}
		// 			catch
		// 			{
		// 				fails[failIdx++] = i;
		// 			}
		// 		}
		//
		// 		if (failIdx > 0) cp.HasError = true;
		//
		// 		// KEY_ADDR_BEGIN
		// 		// 	KEY_ADDR_END  
		// 		// KEY_IDX_BEGIN 
		// 		// KEY_IDX_END   
		//
		// 		key = ci[RevitCellParams.D_CELL_ADDR_NAME];
		//
		// 		key = key.IsVoid() ? ci[RevitCellParams.D_CELL_ADDR] : key;
		//
		// 		if (!key.IsVoid())
		// 		{
		// 			string testKey = KEY_ADDR_BEGIN + key + KEY_ADDR_END ;
		// 			bool gotKey = false;
		//
		// 			for (int i = 0; i < 100; i++)
		// 			{
		// 				key = testKey +  KEY_IDX_BEGIN + i.ToString("D3") + KEY_IDX_END;
		//
		// 				if (!CellItemsBySeqName.ContainsKey(key))
		// 				{
		// 					gotKey = true;
		// 					break;
		// 				}
		// 			}
		//
		// 			if (cp.Name.IsVoid()) cp.Name = key;
		//
		// 			if (gotKey) CellItemsBySeqName.Add(key, ci);
		// 		}
		// 	}
		//
		// 	return true;
		// }

		public bool GetCells(Document doc, string familyTypeName)
		{
			cellFamilies =
				SelectbyCatAndMatchStringParameter(doc, BuiltInCategory.OST_GenericAnnotation, familyTypeName);

			if (cellFamilies == null && cellFamilies.Count == 0)
			{
				errorNoCells(familyTypeName);
				return false;
			}

			RevitCellParams cp;

			CellItemsBySeqName = new Dictionary<string, RevitCellParams>();
			CellItemsByNameSeq = new Dictionary<string, RevitCellParams>();

			foreach (Element cellFamily in cellFamilies)
			{
				cp = categorizeCellParameters(cellFamily as AnnotationSymbol);

				if (cp.HasError == true)
				{
					errorList.Add(cp);
					continue;
				}

				string keySeqName = makeKey(cp, true);
				string keyNameSeq = makeKey(cp, false);

				CellItemsBySeqName.Add(keySeqName, cp);
				CellItemsByNameSeq.Add(keyNameSeq, cp);
			}

			return errorList.Count == 0;
		}

		// public bool GetCells2(Document doc, string familyTypeName)
		// {
		// 	// select all matching cells
		// 	cellFamilies =
		// 		SelectbyCatAndMatchStringParameter(doc, BuiltInCategory.OST_GenericAnnotation, familyTypeName);
		//
		// 	if (cellFamilies == null && cellFamilies.Count == 0)
		// 	{
		// 		errorNoCells(familyTypeName);
		// 		return false;
		// 	}
		//
		// 	string key;
		// 	RevitCellParams cp;
		//
		// 	CellItemsBySeqName = new Dictionary<string, RevitCellParams>();
		//
		// 	foreach (Element e in cellFamilies)
		// 	{
		// 		try
		// 		{
		// 			ci = new RevitCellParams();
		//
		// 			cp.AnnoSymbol = e;
		// 			cp.Name = (e.LookupParameter(RevitCellParams.CellItemIds[RevitCellParams.D_NAME])).AsString();
		// 			cp.CellAddrName = (e.LookupParameter(RevitCellParams.CellItemIds[RevitCellParams.D_CELL_ADDR_NAME])).AsString();
		//
		// 			if (cp.CellAddrName.IsVoid())
		// 			{
		// 				cp.CellAddr = (e.LookupParameter(RevitCellParams.CellItemIds[RevitCellParams.D_CELL_ADDR])).AsString();
		// 			}
		//
		// 			cp.Formula = (e.LookupParameter(RevitCellParams.CellItemIds[RevitCellParams.D_FORMULA])).AsString();
		// 			cp.ResultAsNumber = (e.LookupParameter(RevitCellParams.CellItemIds[RevitCellParams.D_CALC_RESULTS_AS_NUMBER]))
		// 			.AsDouble();
		// 			cp.ResultAsString = (e.LookupParameter(RevitCellParams.CellItemIds[RevitCellParams.D_CALC_RESULTS_AS_TEXT]))
		// 			.AsString();
		//
		// 			key = getKey(cp.Name, cp.CellAddrName, cp.CellAddr);
		//
		// 			CellItemsBySeqName.Add(key, ci);
		// 		}
		// 		catch { }
		// 	}
		//
		// 	return true;
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
			else if (paramCount != RevitCellParameters.ParamCounts[(int) ParamGroupType.DATA])
			{
				cp.CellParamDataType = ParamDataType.ERROR;
				cp.Error = RevitCellErrorCode.PARAM_MISSING_CS001102;
			}

			Debug.WriteLine("has error D?|" + cp.HasError);

			return cp;
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

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is RevitManager";
		}

	#endregion
	}

	public class ChartList
	{
		public List<RevitChartItem> Charts { get; } = new List<RevitChartItem>();

		public void Add(RevitChartItem revitChart)
		{
			Charts.Add(revitChart);
		}
	}
}