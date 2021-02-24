#region using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Autodesk.Revit.DB;
using Microsoft.Office.Interop.Excel;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  2/14/2021 2:00:03 PM

namespace SpreadSheet01.ExcelSupport
{
	public class ExcelManager : INotifyPropertyChanged
	{
	#region private fields

		private Application excel;
		private Workbook excelFile;
		private Worksheet excelWS;

	#endregion

	#region ctor

		public ExcelManager() { }

	#endregion

	#region public properties

		public bool OpenExcelFile(string path)
		{
			try
			{
				excelFile = null;
				excelWS = null;

				DirectoryInfo di = new DirectoryInfo(path);

				excel = new Application();
				excelFile = excel.Workbooks.Open(di.FullName, false, true);
			}
			catch
			{
				return false;
			}

			return true;
		}

		public bool OpenExcelWorkSheet(string workSheetName)
		{
			if (excelFile == null) return false;

			try
			{
				excelWS = null;

				excelWS = (Worksheet) excelFile.Worksheets[workSheetName];
			}
			catch
			{
				return false;
			}

			return true;
		}

		public bool GetValue(string cellName, out string value)
		{
			value = null;

			Type t;
			

			if (excelWS == null) return false;

			try
			{
				// Range r = excelWS.Cells[cellName];
				Range r = excelWS.Evaluate(cellName);
				t = r.Value2.GetType();
				value = r.NumberFormat;

				

				// this gets the excel cell value as formatted text
				value = r.Text;

			}
			catch
			{
				return false;
			}

			return true;
		}
		
		public bool SetValue(string cellName, string value)
		{

			if (excelWS == null) return false;

			try
			{
				Range r = excelWS.Cells[cellName];

				r.Value = value;
			}
			catch
			{
				return false;
			}

			return true;
		}


	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ExcelManager";
		}

	#endregion
	}
}