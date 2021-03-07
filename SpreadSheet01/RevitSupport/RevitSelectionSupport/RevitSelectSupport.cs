// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitSelectSupport.cs
// Created:      2021-03-03 (11:18 PM)

using System.Collections.Generic;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitCellsManagement;

namespace SpreadSheet01.RevitSupport.RevitSelectionSupport
{
	public class RevitSelectSupport
	{
		private RevitManagementSupport mgmtSupport = new RevitManagementSupport();

		public ICollection<Element> GetCellFamilies(Document doc, string familyTypeName)
		{
			return this.SelectbyCatAndMatchStringParameter(doc,
				BuiltInCategory.OST_GenericAnnotation, familyTypeName);


		}



		// public bool GetCellFamilies(Document doc, string familyTypeName, RevitManager revitManager)
		// {
		// 	revitManager.CellFamilies = this.SelectbyCatAndMatchStringParameter(doc,
		// 		BuiltInCategory.OST_GenericAnnotation, familyTypeName);
		//
		// 	if (revitManager.CellFamilies == null || revitManager.CellFamilies.Count == 0)
		// 	{
		// 		mgmtSupport.ErrorNoCellsFound(familyTypeName);
		// 		return false;
		// 	}
		//
		// 	revitManager.GotCellFamilies = true;
		//
		// 	return true;
		// }

		// public ICollection<Element> FindGenericAnnotationByName(Document doc, string typeName)
		// {
		// 	ParameterValueProvider provider =
		// 		new ParameterValueProvider(new ElementId((int) BuiltInParameter.ALL_MODEL_FAMILY_NAME));
		//
		// 	string stringRuleValue = typeName;
		//
		// 	FilterStringRuleEvaluator sre = new FilterStringEquals();
		//
		// 	FilterRule rule = new FilterStringRule(provider, sre, stringRuleValue, true);
		//
		// 	ElementParameterFilter filter = new ElementParameterFilter(rule);
		//
		// 	FilteredElementCollector col
		// 		= new FilteredElementCollector(doc)
		// 		.OfCategory(BuiltInCategory.OST_GenericAnnotation).WhereElementIsNotElementType().WherePasses(filter);
		//
		// 	ICollection<Element> charts = col.ToElements();
		//
		// 	return charts;
		// }

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

	}
}