// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitChartItem.cs
// Created:      2021-02-17 (6:44 PM)

using System.Collections.Generic;

namespace SpreadSheet01.RevitSupport
{
	public class RevitChartItem
	{
		public static int ItemIdCount;

		public const int EXCEL_PATH = 0;
		public const int EXCEL_WORKSHEET = 1;
		public const int CELL_FAMILYTYPENAME = 2;

		public static Dictionary<string, int> ChartItemIds { get; }  = new Dictionary<string, int>(3)
		{
			{"Excel File",  EXCEL_PATH},
			{"Excel WorkSheet Name",  EXCEL_WORKSHEET},
			{"Cell Family Name",  CELL_FAMILYTYPENAME}
		};

		static RevitChartItem()
		{
			ItemIdCount = ChartItemIds.Count;
		}


		public string[] Chart { get; set; }  = new string[3];

		public string Path
		{
			get => Chart[EXCEL_PATH];
			set => Chart[EXCEL_PATH] = value;
		}

		public string WorkSheet
		{
			get => Chart[EXCEL_WORKSHEET];
			set => Chart[EXCEL_WORKSHEET] = value;
		}

		public string FamilyTypeName
		{
			get => Chart[CELL_FAMILYTYPENAME];
			set => Chart[CELL_FAMILYTYPENAME] = value;
		}
	}
}