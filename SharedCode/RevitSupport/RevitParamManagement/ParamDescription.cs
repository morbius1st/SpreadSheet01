#region using directives

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using UtilityLibrary;

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
			Exist = paramExist;
			ReadReqmt = paramReadReqmt;
			Mode = paramMode;

			// MakeParam = makeParam;
		}

	#endregion

	#region public properties

		public static ParamDesc Empty => new ParamDesc("", "", -1, 
			ParamClass.PC_INTERNAL, ParamType.PT_INTERNAL, ParamExistReqmt.EX_PARAM_MUST_EXIST, 
			ParamDataType.DT_IGNORE, ParamReadReqmt.RD_VALUE_IGNORE, ParamMode.PM_NOT_USED);


		public string ParameterName {
			get => parameterName;

			private set
			{
				parameterName = value;
				OnPropertyChanged();
			}
		}
		public string ShortName	        { get; set; }
		public int Index                { get; protected set; }
		
		public ParamClass ParamClass    { get; protected set; }
		public ParamType ParamType           { get; protected set; }
		public ParamDataType DataType   { get; protected set; }
		public ParamExistReqmt Exist    { get; protected set; }
		public ParamReadReqmt ReadReqmt { get; protected set; }
		public ParamMode Mode           { get; protected set; }

		// public RevitCatagorizeParam.MakeParamDelegate MakeParam { get; private set; }


		// public ParamGroup Group         { get; protected set; }

	#endregion

	#region private properties

	#endregion


	#region public methods

		// public void InvokeDelegate(Parameter param)
		// {
		// 	Debug.WriteLine("got invoke");
		//
		// 	if (MakeParam != null)
		// 	{
		// 		MakeParam.Invoke(param, this);
		// 	}
		// }



		// public bool Match(string testShortName)
		// {
		// 	if (testShortName.IsVoid()) return false;
		//
		// 	return testShortName.Equals(shortName);
		// }

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