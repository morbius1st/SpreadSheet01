﻿#region + Using Directives
using System;
using Autodesk.Revit.UI;

#endregion

// user name: jeffs
// created:   3/6/2021 9:51:59 AM

namespace SpreadSheet01.RevitSupport.RevitCellsManagement
{
	public class RevitManagementSupport
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
	}
}