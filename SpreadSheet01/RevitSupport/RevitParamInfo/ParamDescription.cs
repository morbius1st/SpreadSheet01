#region using directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitParamValue;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  3/7/2021 6:33:54 AM

namespace SpreadSheet01.RevitSupport.RevitParamInfo
{
	public class ParamDesc : INotifyPropertyChanged
	{
	#region private fields

		private string parameterName;
		private string shortName;

	#endregion

	#region ctor

		protected ParamDesc() {}

		public ParamDesc(string paramName,
			int index,
			int shortNameLen,
			ParamType paramType,
			ParamExistReqmt paramExist,
			ParamDataType dataType,
			ParamReadReqmt paramReadReqmt,
			ParamMode paramMode,
			RevitCatagorizeParam.MakeParamDelegate makeParam = null)
		{
			Index = index;
			ParameterName = paramName;
			ShortNameLen = shortNameLen;
			shortName = GetShortName(paramName, shortNameLen);

			Type = paramType;
			DataType = dataType;
			Exist = paramExist;
			ReadReqmt = paramReadReqmt;
			Mode = paramMode;

			MakeParam = makeParam;
		}

	#endregion

	#region public properties

		public static ParamDesc Empty => new ParamDesc("", -1, 8,
			ParamType.INTERNAL, ParamExistReqmt.PARAM_MUST_EXIST, 
			ParamDataType.IGNORE, ParamReadReqmt.READ_VALUE_IGNORE, 
			ParamMode.NOT_USED);


		public string ParameterName {
			get => parameterName;

			private set
			{
				parameterName = value;
				OnPropertyChanged();
			}
		}
		public string ShortName => shortName;
		public int Index                { get; protected set; }
		public int ShortNameLen         { get; set; }
		public ParamType Type           { get; protected set; }
		public ParamDataType DataType   { get; protected set; }
		public ParamExistReqmt Exist    { get; protected set; }
		public ParamReadReqmt ReadReqmt { get; protected set; }
		public ParamMode Mode           { get; protected set; }

		public RevitCatagorizeParam.MakeParamDelegate MakeParam { get; private set; }


		// public ParamGroup Group         { get; protected set; }

	#endregion

	#region private properties

	#endregion


	#region public methods

		public void InvokeDelegate(Parameter param)
		{
			Debug.WriteLine("got invoke");

			if (MakeParam != null)
			{
				MakeParam.Invoke(param, this);
			}
		}

		public void SetShortName()
		{
			shortName = GetShortName(parameterName, ShortNameLen);
		}

		public static string GetShortName(string name, int shortNameLen)
		{
			return name.Substring(0, Math.Min(name.Length, shortNameLen));
		}

		public bool Match(string testShortName)
		{
			if (testShortName.IsVoid()) return false;

			return testShortName.Equals(shortName);
		}

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "ParamDesc| " + ParameterName;
		}

	#endregion



		
		// public string GetRootName(string name)
		// {
		// 	if (name.IsVoid()) return "";
		//
		// 	string test = name.Trim();
		//
		// 	// Regex rx = new Regex(@"(?<=^\#\d|^\#\d\d)(\s+)(.*[^\s])|^(.*)(?=\s\#\d{1,2}\s*$)");
		// 	Regex rx = new Regex(@"((?>^(?<name>.*)(?=\s\#(?<digits>\d{1,2})\s*$).*)|(?>(?>^\s*\#(?<digits>\d{1,2})\s+)(?<name>.*[^\s]))|(?<name>.*[^\s]))", RegexOptions.ExplicitCapture);
		// 	Match m = rx.Match(name);
		//
		// 	if (!m.Success) return name;
		//
		// 	return m.Groups["name"].Value;
		//
		// 	// int pos1 = name.IndexOf('#');
		// 	// int pos2 = name.IndexOf(' ');
		// 	//
		// 	// if (pos1 == -1 || pos2 == -1) return name;
		// 	//
		// 	// if (pos1 < pos2)
		// 	// {
		// 	// 	test = test.Substring(pos2 + 1).Trim();
		// 	// } 
		// 	// else
		// 	// {
		// 	// 	test = test.Substring(0, pos2).Trim();
		// 	// }
		// 	//
		// 	// return test;
		// }

		// public string GetShortName(string rootName)
		// {
		// 	return GetShortName(rootName, ShortNameLen);
		// }

	}
}