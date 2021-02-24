#region using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using SpreadSheet01.ExcelSupport;
using UtilityLibrary;
using SpreadSheet01.RevitSupport;
using SpreadSheet01.RevitSupport.RevitParamValue;

using static SpreadSheet01.RevitSupport.RevitCellParameters;
using static SpreadSheet01.RevitSupport.RevitCellItem;
using static SpreadSheet01.RevitSupport.ParamReadReqmt;


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

		private Dictionary<string, RevitCellItem> CellItemsBySeqName { get; set; }
		private Dictionary<string, RevitCellItem> CellItemsByNameSeq { get; set; }

		private ICollection<Element> chartFamilies;
		private ICollection<Element> cellFamilies;

		private List<RevitCellItem> errorList = new List<RevitCellItem>();

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
		// 	RevitCellItem ci;
		//
		// 	CellItemsBySeqName = new Dictionary<string, RevitCellItem>();
		//
		// 	foreach (Element cellFamily in cellFamilies)
		// 	{
		// 		int[] fails = new int[ReadParamFromFamily.Length];
		// 		int failIdx = 0;
		//
		// 		ci = new RevitCellItem();
		//
		// 		ci.AnnoSymbol = cellFamily as AnnotationSymbol;
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
		// 				case DataType.STRING:
		// 					{
		// 						ci[idx] =
		// 							cellFamily.LookupParameter(paramName)?.AsString() ?? "";
		// 						ci.CellParamDataType = DataType.STRING;
		//
		// 						break;
		// 					}
		// 				case DataType.NUMBER:
		// 					{
		// 						ci[idx] =
		// 							cellFamily.LookupParameter(paramName)?.AsDouble() ?? 0.0;
		// 						ci.CellParamDataType = DataType.NUMBER;
		//
		// 						break;
		// 					}
		// 				case DataType.BOOL:
		// 					{
		// 						ci[idx] =
		// 							(cellFamily.LookupParameter(paramName)?.AsInteger() ?? 1) == 1;
		// 						ci.CellParamDataType = DataType.BOOL;
		//
		// 						break;
		// 					}
		// 				default:
		// 					{
		// 						ci.CellParamDataType = DataType.ERROR;
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
		// 		if (failIdx > 0) ci.HasError = true;
		//
		// 		// KEY_ADDR_BEGIN
		// 		// 	KEY_ADDR_END  
		// 		// KEY_IDX_BEGIN 
		// 		// KEY_IDX_END   
		//
		// 		key = ci[RevitCellItem.D_CELL_ADDR_NAME];
		//
		// 		key = key.IsVoid() ? ci[RevitCellItem.D_CELL_ADDR] : key;
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
		// 			if (ci.Name.IsVoid()) ci.Name = key;
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

			RevitCellItem ci;

			CellItemsBySeqName = new Dictionary<string, RevitCellItem>();
			CellItemsByNameSeq = new Dictionary<string, RevitCellItem>();

			foreach (Element cellFamily in cellFamilies)
			{
				ci = categorizeCellParameters(cellFamily as AnnotationSymbol);

				if (ci.HasError == true)
				{
					errorList.Add(ci);
					continue;
				}

				string keySeqName = makeKey(ci, true);
				string keyNameSeq = makeKey(ci, false);

				CellItemsBySeqName.Add(keySeqName, ci);
				CellItemsByNameSeq.Add(keyNameSeq, ci);
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
		// 	RevitCellItem ci;
		//
		// 	CellItemsBySeqName = new Dictionary<string, RevitCellItem>();
		//
		// 	foreach (Element e in cellFamilies)
		// 	{
		// 		try
		// 		{
		// 			ci = new RevitCellItem();
		//
		// 			ci.AnnoSymbol = e;
		// 			ci.Name = (e.LookupParameter(RevitCellItem.CellItemIds[RevitCellItem.D_NAME])).AsString();
		// 			ci.CellAddrName = (e.LookupParameter(RevitCellItem.CellItemIds[RevitCellItem.D_CELL_ADDR_NAME])).AsString();
		//
		// 			if (ci.CellAddrName.IsVoid())
		// 			{
		// 				ci.CellAddr = (e.LookupParameter(RevitCellItem.CellItemIds[RevitCellItem.D_CELL_ADDR])).AsString();
		// 			}
		//
		// 			ci.Formula = (e.LookupParameter(RevitCellItem.CellItemIds[RevitCellItem.D_FORMULA])).AsString();
		// 			ci.ResultAsNumber = (e.LookupParameter(RevitCellItem.CellItemIds[RevitCellItem.D_CALC_RESULTS_AS_NUMBER]))
		// 			.AsDouble();
		// 			ci.ResultAsString = (e.LookupParameter(RevitCellItem.CellItemIds[RevitCellItem.D_CALC_RESULTS_AS_TEXT]))
		// 			.AsString();
		//
		// 			key = getKey(ci.Name, ci.CellAddrName, ci.CellAddr);
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
			// 				ci.Chart[kvp.Value] = p.AsString();
			// 				found++;
			// 			}
			// 		}
			//
			// 		if (found == RevitChartItem.ItemIdCount) break;
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

		private string makeKey(RevitCellItem ci, bool asSeqName)
		{
			string seq = ((string) ci[SeqIdx].GetValue());
			seq = seq.IsVoid() ? "ZZZZZZ" : seq;
			seq = KEY_IDX_BEGIN + $"{seq,8}" + KEY_IDX_END;

			string name = ci.Name.IsVoid() ? "Un-named " : ci.Name + " " ;

			string eid = ci.AnnoSymbol == null ? "null element" : ci.AnnoSymbol.Id.ToString();


			if (asSeqName) return seq + name + eid;

			return name + seq + eid;
		}

		private RevitCellItem categorizeCellParameters(AnnotationSymbol annoSym)
		{
			RevitCellItem ci = new RevitCellItem();

			if (annoSym == null)
			{
				ci.Error = RevitCellErrorCode.INVALID_ANNO_SYM_CS001111;
				return ci;
			}

			ci.AnnoSymbol = annoSym;

			IList<Parameter> parameters = annoSym.GetOrderedParameters();

			int paramCount = 1;
			int textCount = 0;

			foreach (Parameter param in parameters)
			{
				string name = param.Definition.Name;

				Debug.WriteLine("process parameter| " + name);

				ParamDesc pd = RevitCellParameters.Match(name);

				Debug.WriteLine("has error A?|" + ci.HasError);

				if (pd == null)
				{
					continue;
				}

				ci.CellParamDataType = pd.DataType;

				int idx = pd.Index;

				Debug.WriteLine("has error B?|" + ci.HasError);

				switch (pd.DataType)
				{
				case ParamDataType.STRING:
					{
						RevitValueString rs = 
							new RevitValueString(
								pd.ReadReqmt == READ_VALUE_IGNORE ? "" : param.AsString(), pd);
						ci[idx] = rs;
						paramCount++;
						break;
					}

				case ParamDataType.TEXT:
					{ 
						ci.AddText(param.AsString(), name, pd);
						textCount++;
						break;
					}

				case ParamDataType.ADDRESS:
					{ 
						RevitValueAddr ra = new RevitValueAddr(param.AsString(), pd);
						ci[idx] = ra;
						paramCount++;
						break;
					}

				case ParamDataType.NUMBER:
					{
						RevitValueNumber rn = new RevitValueNumber(
							pd.ReadReqmt == READ_VALUE_IGNORE ? Double.NaN : param.AsDouble(), pd);
						ci[idx] = rn;
						paramCount++;
						break;
					}

				case ParamDataType.BOOL:
					{
						RevitValueBool rb = new RevitValueBool(
							(pd.ReadReqmt == READ_VALUE_IGNORE ? (bool?) null : param.AsInteger() == 1), pd);
						ci[idx] = rb;
						paramCount++;
						break;
					}

				case ParamDataType.IGNORE:
					{
						// RevitValueString rs = new RevitValueString("Not Used", pd);
						// ci[idx] = rs;
						paramCount++;
						break;
					}
				}
				Debug.WriteLine("has error C?|" + ci.HasError);
			}

			if (textCount == 0)
			{
				ci.CellParamDataType = ParamDataType.ERROR;
				ci.Error = RevitCellErrorCode.PARAM_VALUE_MISSING_CS001101;
			}
			else if (paramCount != RevitCellParameters.ItemIdCount)
			{
				ci.CellParamDataType = ParamDataType.ERROR;
				ci.Error = RevitCellErrorCode.PARAM_MISSING_CS001102;
			}

			Debug.WriteLine("has error D?|" + ci.HasError);

			return ci;
		}


		private void getCharts(ICollection<Element> charts)
		{
			foreach (Element e in charts)
			{
				int found = 0;

				RevitChartItem ci = new RevitChartItem();

				foreach (Parameter p in e.ParametersMap)
				{
					string name = p.Definition.Name;

					foreach (KeyValuePair<string, int> kvp in RevitChartItem.ChartItemIds)
					{
						if (name.Equals(kvp.Key))
						{
							ci.Chart[kvp.Value] = p.AsString();
							found++;
						}
					}

					if (found == RevitChartItem.ItemIdCount) break;
				}

				if (ci != null) chartList.Add(ci);
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