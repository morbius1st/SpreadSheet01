#region + Using Directives
using System;

using Microsoft.WindowsAPICodePack.Dialogs;

#endregion

// user name: jeffs
// created:   3/6/2021 10:19:54 AM

namespace SpreadSheet01.RevitSupport.RevitCellsManagement
{
	public class RevitManagementSupport
	{

		public void ErrorNoCellsFound(string familyTypeName)
		{
			TaskDialog td = new TaskDialog();
			td.Caption ="Spread Sheet Cells for| " + familyTypeName;
			td.InstructionText = "No Data cells were found| ";
			td.Icon = TaskDialogStandardIcon.Error;
			td.StandardButtons = TaskDialogStandardButtons.Ok;
			td.Show();
		}

		public void ErrorNoChartsFound(string msg)
		{
			TaskDialog td = new TaskDialog();
			td.Caption ="Update Cells";
			td.InstructionText = "Chart cells have not been found| " + msg;
			td.Icon = TaskDialogStandardIcon.Error;
			td.Text ="The revit model appears to have no Chart cells placed.\nThe Chart cells provide the critical necessary\n"
				+ "information used to update the data cells.\n\nPlease add and configure Chart cells and try again." ;
			td.StandardButtons = TaskDialogStandardButtons.Ok;
			td.Show();
		}
	}
}
