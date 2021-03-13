#region using

using System;
using System.Collections.Generic;

using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitParamInfo;
using SpreadSheet01.RevitSupport.RevitParamValue;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  2/14/2021 2:39:25 PM

namespace SpreadSheet01.ExcelSupport
{
	public class ExcelExchange
	{
	#region private fields

		private const string CELL_NAME_STR = "Excel Cell Name";
		private const string UNIT_FORMAT_STR = "Unit Format";
		private const string VALUE = "Value";

		private RevitManager rvtMgr;

		private ExcelManager exMgr = new ExcelManager();

		private bool configured;



	#endregion

	#region ctor

		public ExcelExchange() { }

	#endregion

	#region public properties

		public bool Configured
		{
			get => configured;
			set => configured = value;
		}

	#endregion

	#region private properties

	#endregion

	#region public methods
//
// 		public bool UpdateValues(Document doc, string excelFilePath, 
// 			string excelWorkSheetName, 
// 			RevitAnnoSyms cells)
// 		{
// 			Configure(excelFilePath, excelWorkSheetName);
//
// 			if (!configured) return false;
//
// 			string cellName;
// 			string value;
// 			int fails = 0;
//
// 			string anchorCellName;
// 			string relativeAddr;
// 			string cellAddr;
//
//
// 			foreach (KeyValuePair<string, RevitCellSym> rvtAnnoSym in cells.Containers)
// 			{
// 				Element e = rvtAnnoSym.Value.RvtElement;
//
// 				anchorCellName = rvtAnnoSym.Value.RevitParamList[RevitCellParameters.CellAddrIdx].GetValue();
//
// 				RevitLabels labels = (RevitLabels) rvtAnnoSym.Value.RevitParamList[RevitCellParameters.LabelsIdx];
//
// 				foreach (KeyValuePair<string, RevitLabel> kvp in labels.Containers)
// 				{
// 					RevitLabel label = (RevitLabel) kvp.Value;
//
// 					RevitParamRelativeAddr relAddr = (RevitParamRelativeAddr) label[RevitCellParameters.lblRelAddrIdx];
//
// 					cellAddr = ExcelAssist.AddToCellAddr(anchorCellName, relAddr.Row, relAddr.Col);
//
// 					if (!exMgr.GetValue(cellAddr, out value))
// 					{
// 						fails++;
// 						value = "%ERROR%";
// 					}
//
// 					string labelValueTitle = ((RevitParamLabel) label[RevitCellParameters.LabelIdx]).LabelValueName;
//
// 					Parameter p = e.LookupParameter(labelValueTitle);
// 					p.Set(value);
// 				}
//
// 				exMgr.CloseExcelCloseFile();
// 			}
// /*
// 			foreach (Element e in cells)
// 			{
// 				try
// 				{
// 					cellName = e.ParametersMap.get_Item(CELL_NAME_STR).AsString();
//
// 					if (!exMgr.GetValue(cellName, out value))
// 					{
// 						fails++;
// 						value = "%error%";
// 					}
//
// 					Parameter p = e.LookupParameter(VALUE);
// 					p.Set(value);
// 				}
// 				catch 
// 				{
// 					continue;
// 				}
// 			}
// */
//
// 			return true;
// 		}
//




	#endregion

	#region private methods

		private void Configure(string excelFilePath, string excelWorkSheetName)
		{
			if (!exMgr.OpenExcelFile(excelFilePath)) return ; //false;

			if (!exMgr.OpenExcelWorkSheet(excelWorkSheetName)) return; // false;

			configured = true;
		}



	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ExcelExchange";
		}

	#endregion
	}




}