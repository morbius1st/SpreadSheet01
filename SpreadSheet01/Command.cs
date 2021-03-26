#region using

using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using SpreadSheet01.Windows;

#endregion

// projname: SpreadSheet01
// itemname: Command
// username: jeffs
// created:  2/13/2021 2:44:52 PM

namespace SpreadSheet01
{
	public static class RevitDoc
	{
		public static Document Doc { get; set; }
	}



	/// <summary>
	/// revit command entry point.  this command will automatically update all
	/// cells according to its "update type"
	/// </summary>

	[Transaction(TransactionMode.Manual)]
	public class Command : IExternalCommand
	{
	#region fields

		private const string ROOT_TRANSACTION_NAME = "Transaction Name";

		// private ChartList chartList = new ChartList();

	#endregion

	#region entry point: Execute

		public Result Execute(
			ExternalCommandData commandData, ref string message, ElementSet elements)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Application app = uiapp.Application;
			Document doc = uidoc.Document;

			RevitDoc.Doc = doc;

			// Access current selection
			Selection sel = uidoc.Selection;

			Result result;

			SelectCharts charts = new SelectCharts(app, doc);

			charts.ShowDialog();


/*			// // Modify document within a transaction
			using (Transaction tx = new Transaction(doc))
			{
				tx.Start(ROOT_TRANSACTION_NAME);

				result = rt.TestSpreadSheet1(doc);

				// ExcelExchange exe = new ExcelExchange();

				// exe.UpdateValues(doc,
				// 	chartPath, chartWorkSheet, cells);

				tx.Commit();
			}
*/

			// result = rt.TestSpreadSheet1(doc);

			// Review r = new Review();
			//
			// r.ShowDialog();

			return Result.Succeeded;
		}

	#endregion

	#region public methods



	#endregion

	#region private methods

		// private void GetInfo(Document doc)
		// {
		//
		// 	ParameterValueProvider provider =
		// 		new ParameterValueProvider(new ElementId((int) BuiltInParameter.ALL_MODEL_FAMILY_NAME));
		//
		// 	string stringRuleValue = "SpreadSheetCell-01";
		//
		// 	FilterStringRuleEvaluator sre = new FilterStringEquals();
		//
		// 	FilterRule rule = new FilterStringRule(provider, sre, stringRuleValue, true);
		//
		// 	ElementParameterFilter filter = new ElementParameterFilter(rule);
		//
		//
		// 	ElementId paramId = GetParamElemId(doc);
		//
		// 	
		//
		// 	ParameterValueProvider provider1 =
		// 		new ParameterValueProvider(paramId);
		//
		// 	string stringRuleValue1 = "Chart 01";
		//
		// 	FilterStringRuleEvaluator sre1 = new FilterStringEquals();
		//
		// 	FilterRule rule1 = new FilterStringRule(provider1, sre1, stringRuleValue1, true);
		//
		// 	ElementParameterFilter filter1 = new ElementParameterFilter(rule1);
		//
		//
		// 	LogicalAndFilter andFilter = new LogicalAndFilter(filter, filter1);
		//
		//
		//
		// 	FilteredElementCollector col
		// 		= new FilteredElementCollector(doc)
		// 		.OfCategory(BuiltInCategory.OST_GenericAnnotation).OfClass(typeof(FamilyInstance)).WherePasses(filter1);
		//
		//
		// 	// Filtered element collector is iterable
		// 	foreach (AnnoSymbol e in col)
		// 	{
		// 		Debug.WriteLine(e.Name + "  chart name| " 
		// 			+ e.ParametersMap.get_Item("ChartName").AsString() 
		// 			+ "  value| " + e.ParametersMap.get_Item("Value").AsString());
		// 	}
		// }
		//
		//
		// // get all family instances that
		// // are generic annotation and
		// // have the name provided
		// private ICollection<AnnoSymbol> FindGenericAnnotationByName(Document doc, string typeName)
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
		// 	ICollection<AnnoSymbol> charts = col.ToElements();
		//
		// 	foreach (AnnoSymbol e in charts)
		// 	{
		// 		int found = 0;
		//
		// 		RevitChartItem ci = new RevitChartItem();
		//
		// 		foreach (Parameter p in e.ParametersMap)
		// 		{
		// 			string name = p.Definition.Name;
		//
		// 			foreach (KeyValuePair<string, int> kvp in RevitChartItem.ChartItemIds)
		// 			{
		// 				if (name.Equals(kvp.Key))
		// 				{
		// 					cp.Chart[kvp.Value] = p.AsString();
		// 					found++;
		// 				}
		// 			}
		// 			if (found == RevitChartItem.DataParamCount) break;
		// 		}
		//
		// 		if (ci != null) chartList.Add(ci);
		// 	}
		//
		// 	return charts;
		// }
		//
		// private void ShowCells1(Document doc, ICollection<AnnoSymbol> cells)
		// {
		// 	string msg = "";
		// 	int idx = 0;
		//
		// 	foreach (AnnoSymbol e in cells)
		// 	{
		// 		msg += "\n";
		// 		msg += "element| " + ++idx;
		// 		msg += "\n";
		// 		msg += "name| " + e.Name;
		// 		msg += "\n";
		// 		
		// 		foreach (Parameter p in e.ParametersMap)
		// 		{
		// 			if (p.StorageType == StorageType.String)
		// 			{
		// 				msg += "parameter| name| " + p.Definition.Name + "  value| " + p.AsString();
		// 				msg += "\n";
		// 			}
		// 		}
		// 	}
		//
		// 	TaskDialog td = new TaskDialog("Cells");
		// 	td.MainContent = msg;
		//
		// 	td.Show();
		// }
		//
		// private void ShowCharts1(Document doc, ICollection<ElementId> charts)
		// {
		// 	string msg = "";
		// 	int idx = 0;
		//
		// 	foreach (ElementId e in charts)
		// 	{
		// 		AnnoSymbol el = doc.GetElement(e);
		//
		// 		msg += "\n";
		// 		msg += "element| " + ++idx;
		// 		msg += "\n";
		// 		msg += "name| " + el.Name;
		// 		msg += "\n";
		// 		
		// 		foreach (Parameter p in el.ParametersMap)
		// 		{
		// 			if (p.StorageType == StorageType.String)
		// 			{
		// 				msg += "parameter| name| " + p.Definition.Name + "  value| " + p.AsString();
		// 				msg += "\n";
		// 			}
		// 		}
		// 	}
		//
		// 	TaskDialog td = new TaskDialog("Charts");
		// 	td.MainContent = msg;
		//
		// 	td.Show();
		// }



		// find a family type with a parameter of a specific value: "SpreadSheetCell-01"
		// private ElementId GetParamElemId(Document doc)
		// {
		// 	ParameterValueProvider provider =
		// 		new ParameterValueProvider(new ElementId((int) BuiltInParameter.ALL_MODEL_FAMILY_NAME));
		//
		// 	string stringRuleValue = "SpreadSheetCell-01";
		//
		// 	FilterStringRuleEvaluator sre = new FilterStringEquals();
		//
		// 	FilterRule rule = new FilterStringRule(provider, sre, stringRuleValue, true);
		//
		// 	ElementParameterFilter filter = new ElementParameterFilter(rule);
		//
		// 	FilteredElementCollector col
		// 		= new FilteredElementCollector(doc)
		// 		.OfCategory(BuiltInCategory.OST_GenericAnnotation).WhereElementIsElementType().WherePasses(filter);
		//
		// 	ElementId id = ElementId.InvalidElementId;
		//
		// 	foreach (Parameter p in col.FirstElement().ParametersMap)
		// 	{
		// 		string name = p.Definition.Name;
		//
		// 		Debug.WriteLine("parameter name| " + name);
		//
		// 		if (name.Equals("ChartName"))
		// 		{
		// 			id = p.AsElementId();
		// 			Debug.WriteLine("*** found ***");
		// 		}
		// 	}
		//
		// 	return id;
		// }


	#endregion
	}

}