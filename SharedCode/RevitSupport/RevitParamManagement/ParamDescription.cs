#region using directives

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamClass;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamType;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamRootDataType;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamSubDataType;


using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamExistReqmt;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamDataType;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamReadReqmt;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamMode;

#endregion

// username: jeffs
// created:  3/7/2021 6:33:54 AM

namespace SpreadSheet01.RevitSupport.RevitParamManagement
{
	public class ParamDesc : INotifyPropertyChanged
	{
	#region private fields

		private string parameterName;

	#endregion

	#region ctor

		public ParamDesc(string paramName,
			string shortName,
			int index,
			ParamClass paramClass,
			ParamType paramType,
			ParamExistReqmt paramExist,
			ParamDataType dataType,
			ParamRootDataType rootType, 
			ParamSubDataType subType,
			ParamReadReqmt paramReadReqmt,
			ParamMode paramMode)
			// RevitCatagorizeParam.MakeParamDelegate makeParam = null)
		{
			Index = index;
			ParameterName = paramName;
			ShortName = shortName;
			ParamClass = paramClass;
			ParamType = paramType;
			DataType = dataType;
			RootType = rootType;
			SubType = subType;
			Exist = paramExist;
			ReadReqmt = paramReadReqmt;
			Mode = paramMode;

			// MakeParam = makeParam;
		}

	#endregion

	#region public properties

		public static ParamDesc Empty => new ParamDesc("", "", -1, PC_INTERNAL, PT_INTERNAL, 
			EX_PARAM_MUST_EXIST, DT_IGNORE, RT_INVALID, ST_INVALID, RD_VALUE_IGNORE, PM_NOT_USED);


		public string ParameterName {
			get => parameterName;

			private set
			{
				parameterName = value;
				OnPropertyChanged();
			}
		}
		public string ShortName	          { get; set; }
		public int Index                  { get; protected set; }
										  
		public ParamClass ParamClass      { get; protected set; }
		public ParamType ParamType        { get; protected set; }
		public ParamDataType DataType     { get; protected set; }
		public ParamRootDataType RootType { get; protected set; }
		public ParamSubDataType SubType   { get; protected set; }
		public ParamExistReqmt Exist      { get; protected set; }
		public ParamReadReqmt ReadReqmt   { get; protected set; }
		public ParamMode Mode             { get; protected set; }

		public bool IsRequired => Exist == ParamExistReqmt.EX_PARAM_MUST_EXIST;

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

	}
}