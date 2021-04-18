#region + Using Directives

using Autodesk.Revit.UI;

#endregion

// user name: jeffs
// created:   3/6/2021 9:51:59 AM

namespace SpreadSheet01.Management
{
	public class ManagementSupport
	{

		public void ErrorNoCellsFound(string familyTypeName)
		{
			TaskDialog td = new TaskDialog("Spread Sheet Cells for| " + familyTypeName);
			td.MainContent = "No cells found";
			td.MainIcon = TaskDialogIcon.TaskDialogIconError;
			td.CommonButtons = TaskDialogCommonButtons.Ok;
			td.Show();
		}

		public void ErrorNoChartsFound(string msg)
		{
			TaskDialog td = new TaskDialog("Update Cells");
			td.MainContent = "Chart cells have not been found| " + msg;
			td.MainInstruction = "The revit model appears to have no Chart cells placed.\nThe Chart cells provide the critical necessary\n"
				+ "information used to update the data cells.\n\nPlease add and configure Chart cells and try again." ;
			td.MainIcon = TaskDialogIcon.TaskDialogIconError;
			td.CommonButtons = TaskDialogCommonButtons.Ok;
			td.Show();
		}

		public void ErrorChartErrors(string msg)
		{
			TaskDialog td = new TaskDialog("Chart Collection Errors");
			td.MainContent = "When collecting Charts, errors were discovered |\n" + msg;
			td.MainInstruction = "The Charts / Cells system appears to have some errors.\nThe errors must be corrected before proceeding" ;
			td.MainIcon = TaskDialogIcon.TaskDialogIconError;
			td.CommonButtons = TaskDialogCommonButtons.Ok;
			td.Show();
		}
	}
}
