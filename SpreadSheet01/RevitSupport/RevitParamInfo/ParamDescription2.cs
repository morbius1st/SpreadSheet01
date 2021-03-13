// Solution:     SpreadSheet01
// Project:       Cells
// File:             ParamDescription.cs
// Created:      2021-03-07 (6:02 AM)

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpreadSheet01.RevitSupport.RevitParamValue;
using UtilityLibrary;

namespace SpreadSheet01.RevitSupport.RevitParamInfo
{
/*
	public class ParamDesc2 : INotifyPropertyChanged
	{
		private const string TEXT_SHORT_NAME = "Text";

		private string parameterName;
		private string shortName;

		public ParamDesc2(string paramName,
			int index,
			int indexAdjust,
			ParamGroup paramGroup,
			ParamExistReqmt paramExist,
			ParamDataType dataType,
			ParamReadReqmt paramReadReqmt,
			ParamMode paramMode)
		{
			Index = index;
			ParameterName = paramName;
			shortName = GetShortName(paramName);

			DataType = dataType;
			Exist = paramExist;
			ReadReqmt = paramReadReqmt;
			Group = paramGroup;
			Mode = paramMode;
			ParamIndex = index + indexAdjust;
		}

		public static ParamDesc2 Empty => new ParamDesc2("", -1, -1, 
			ParamGroup.DATA, ParamExistReqmt.PARAM_MUST_EXIST, ParamDataType.IGNORE, ParamReadReqmt.READ_VALUE_IGNORE, ParamMode.NOT_USED);

		public string ParameterName {
			get => parameterName;

			private set
			{
				parameterName = value;
				OnPropertyChanged();
			}
		}
		public string ShortName => shortName;
		public int Index { get; private set; }
		public int ParamIndex { get; private set; }
		public ParamDataType DataType { get; private set; }
		public ParamExistReqmt Exist { get; private set; }
		public ParamReadReqmt ReadReqmt { get; private set; }
		public ParamMode Mode { get; private set; }
		public ParamGroup Group { get; private set; }

		public static string GetShortNameSimple(string name)
		{
			return name.Substring(0, Math.Min(name.Length, 8));
		}
		
		public static string GetShortName(string name, int shortNameLen = 8)
		{
			if (name.IsVoid()) return "";
			string test = name;

			int pos1 = name.IndexOf('#');
			int pos2 = -1;

			if (pos1 == 0 )
			{
				pos2 = name.IndexOf(' ');

				if (pos2 > pos1 + 1)
				{
					test = name.Substring(pos2 + 1).Trim();
				}
			}
			else if (pos1 > 0)
			{
				test = name.Substring(0, pos1 - 1);
			}

			return name.Substring(0, Math.Min(name.Length, shortNameLen));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return Index.ToString("##0") + " <|> "+ DataType.ToString() + " <|> "+ ShortName + " <|> " + ParameterName;
		}
	}
*/
}