// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitSelectSupport.cs
// Created:      2021-03-03 (11:18 PM)

using System.Collections.Generic;
using Autodesk.Revit.DB;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitCellsManagement;

namespace SpreadSheet01.RevitSupport.RevitSelectionSupport
{
	public class RevitSelectSupport
	{
		private ManagementSupport mgmtSupport = new ManagementSupport();


		public ICollection<Element> FindGenericAnnotationByName(Document doc, string typeName)
		{
			ParameterValueProvider provider =
				new ParameterValueProvider(new ElementId((int)BuiltInParameter.ALL_MODEL_FAMILY_NAME));

			string stringRuleValue = typeName;

			FilterStringRuleEvaluator sre = new FilterStringEquals();

			FilterRule rule = new FilterStringRule(provider, sre, stringRuleValue, true);

			ElementParameterFilter filter = new ElementParameterFilter(rule);

			FilteredElementCollector col
				= new FilteredElementCollector(doc)
				.OfCategory(BuiltInCategory.OST_GenericAnnotation).WhereElementIsNotElementType().WherePasses(filter);

			ICollection<Element> charts = col.ToElements();

			return charts;
		}





		public ICollection<Element> GetCellFamilies(Document doc, string familyTypeName)
		{
			return this.SelectbyCatAndMatchStringParameter(doc,
				BuiltInCategory.OST_GenericAnnotation, familyTypeName);

		}


		public ICollection<Element> SelectbyCatAndMatchStringParameter(Document doc, 
			BuiltInCategory category, string familyTypeName)
		{
			ParameterValueProvider provider =
				new ParameterValueProvider(new ElementId((int) BuiltInParameter.ALL_MODEL_FAMILY_NAME));

			string stringRuleValue = familyTypeName;

			FilterStringRuleEvaluator sre = new FilterStringEquals();

			FilterRule rule = new FilterStringRule(provider, sre, stringRuleValue, true);

			ElementParameterFilter filter = new ElementParameterFilter(rule);

			FilteredElementCollector col
				= new FilteredElementCollector(doc)
				.OfCategory(category).WhereElementIsNotElementType().WherePasses(filter);

			return col.ToElements();
		}




/*


		public bool GetCellFamilies(Document doc, string familyTypeName, RevitManager revitManager)
		{
			revitManager.CellFamilies = this.SelectbyCatAndMatchStringParameter(doc,
				BuiltInCategory.OST_GenericAnnotation, familyTypeName);

			if (revitManager.CellFamilies == null || revitManager.CellFamilies.Count == 0)
			{
				mgmtSupport.ErrorNoCellsFound(familyTypeName);
				return false;
			}

			revitManager.GotCellFamilies = true;

			return true;
		}



		public ICollection<Element> FindGenericAnnotationByName(Document doc, string typeName)
		{
			ParameterValueProvider provider =
				new ParameterValueProvider(new ElementId((int)BuiltInParameter.ALL_MODEL_FAMILY_NAME));

			string stringRuleValue = typeName;

			FilterStringRuleEvaluator sre = new FilterStringEquals();

			FilterRule rule = new FilterStringRule(provider, sre, stringRuleValue, true);

			ElementParameterFilter filter = new ElementParameterFilter(rule);

			FilteredElementCollector col
				= new FilteredElementCollector(doc)
				.OfCategory(BuiltInCategory.OST_GenericAnnotation).WhereElementIsNotElementType().WherePasses(filter);

			ICollection<Element> charts = col.ToElements();

			return charts;
		}



		private void GetInfo(Document doc)
		{

			ParameterValueProvider provider =
				new ParameterValueProvider(new ElementId((int)BuiltInParameter.ALL_MODEL_FAMILY_NAME));

			string stringRuleValue = "SpreadSheetCell-01";

			FilterStringRuleEvaluator sre = new FilterStringEquals();

			FilterRule rule = new FilterStringRule(provider, sre, stringRuleValue, true);

			ElementParameterFilter filter = new ElementParameterFilter(rule);


			ElementId paramId = GetParamElemId(doc);



			ParameterValueProvider provider1 =
				new ParameterValueProvider(paramId);

			string stringRuleValue1 = "Chart 01";

			FilterStringRuleEvaluator sre1 = new FilterStringEquals();

			FilterRule rule1 = new FilterStringRule(provider1, sre1, stringRuleValue1, true);

			ElementParameterFilter filter1 = new ElementParameterFilter(rule1);


			LogicalAndFilter andFilter = new LogicalAndFilter(filter, filter1);



			FilteredElementCollector col
				= new FilteredElementCollector(doc)
				.OfCategory(BuiltInCategory.OST_GenericAnnotation).OfClass(typeof(FamilyInstance)).WherePasses(filter1);


			// Filtered element collector is iterable
			foreach (AnnoSymbol e in col)
			{
				Debug.WriteLine(e.Name + "  chart name| "
					+ e.ParametersMap.get_Item("ChartName").AsString()
					+ "  value| " + e.ParametersMap.get_Item("Value").AsString());
			}
		}




		get all family instances that are generic annotation and have the name provided

		 private ICollection<AnnoSymbol> FindGenericAnnotationByName(Document doc, string typeName)
		{
			ParameterValueProvider provider =
				new ParameterValueProvider(new ElementId((int)BuiltInParameter.ALL_MODEL_FAMILY_NAME));

			string stringRuleValue = typeName;

			FilterStringRuleEvaluator sre = new FilterStringEquals();

			FilterRule rule = new FilterStringRule(provider, sre, stringRuleValue, true);

			ElementParameterFilter filter = new ElementParameterFilter(rule);

			FilteredElementCollector col
				= new FilteredElementCollector(doc)
				.OfCategory(BuiltInCategory.OST_GenericAnnotation).WhereElementIsNotElementType().WherePasses(filter);

			ICollection<AnnoSymbol> charts = col.ToElements();

			foreach (AnnoSymbol e in charts)
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
							cp.Chart[kvp.Value] = p.AsString();
							found++;
						}
					}
					if (found == RevitChartItem.DataParamCount) break;
				}

				if (ci != null) chartList.Add(ci);
			}

			return charts;
		}





		find a family type with a parameter of a specific value: "SpreadSheetCell-01"
		 private ElementId GetParamElemId(Document doc)
		{
			ParameterValueProvider provider =
				new ParameterValueProvider(new ElementId((int)BuiltInParameter.ALL_MODEL_FAMILY_NAME));

			string stringRuleValue = "SpreadSheetCell-01";

			FilterStringRuleEvaluator sre = new FilterStringEquals();

			FilterRule rule = new FilterStringRule(provider, sre, stringRuleValue, true);

			ElementParameterFilter filter = new ElementParameterFilter(rule);

			FilteredElementCollector col
				= new FilteredElementCollector(doc)
				.OfCategory(BuiltInCategory.OST_GenericAnnotation).WhereElementIsElementType().WherePasses(filter);

			ElementId id = ElementId.InvalidElementId;

			foreach (Parameter p in col.FirstElement().ParametersMap)
			{
				string name = p.Definition.Name;

				Debug.WriteLine("parameter name| " + name);

				if (name.Equals("ChartName"))
				{
					id = p.AsElementId();
					Debug.WriteLine("*** found ***");
				}
			}

			return id;
		}


		private void ShowCells1(Document doc, ICollection<AnnoSymbol> cells)
		{
			string msg = "";
			int idx = 0;

			foreach (AnnoSymbol e in cells)
			{
				msg += "\n";
				msg += "element| " + ++idx;
				msg += "\n";
				msg += "name| " + e.Name;
				msg += "\n";

				foreach (Parameter p in e.ParametersMap)
				{
					if (p.StorageType == StorageType.String)
					{
						msg += "parameter| name| " + p.Definition.Name + "  value| " + p.AsString();
						msg += "\n";
					}
				}
			}

			TaskDialog td = new TaskDialog("Cells");
			td.MainContent = msg;

			td.Show();
		}





		private void ShowCharts1(Document doc, ICollection<ElementId> charts)
		{
			string msg = "";
			int idx = 0;

			foreach (ElementId e in charts)
			{
				AnnoSymbol el = doc.GetElement(e);

				msg += "\n";
				msg += "element| " + ++idx;
				msg += "\n";
				msg += "name| " + el.Name;
				msg += "\n";

				foreach (Parameter p in el.ParametersMap)
				{
					if (p.StorageType == StorageType.String)
					{
						msg += "parameter| name| " + p.Definition.Name + "  value| " + p.AsString();
						msg += "\n";
					}
				}
			}

			TaskDialog td = new TaskDialog("Charts");
			td.MainContent = msg;

			td.Show();
		}

		*/

	}
}