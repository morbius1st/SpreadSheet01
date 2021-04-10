using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SharedCode.RevitSupport.RevitParamManagement;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitParamManagement;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class RevitParamDefault : ARevitParam
	{
		public RevitParamDefault(dynamic dynValue)
		{
			this.dynValue = new DynamicValue(dynValue);
			// errors = new List<ErrorCodes>();
			paramDesc = ParamDesc.Empty;
			gotValue = dynValue != null;
			Assigned = gotValue;
		}

		public override dynamic GetValue() => dynValue;
	}

	public abstract class ARevitParam : INotifyPropertyChanged
	{
	#region private fields

		protected ErrorCodeList errorList = new ErrorCodeList();

		protected DynamicValue dynValue = new DynamicValue();

		// protected List<ErrorCodes> errors = new List<ErrorCodes>();

		protected ParamDesc paramDesc;

		protected bool gotValue;

	#endregion

	#region public properties

		public DynamicValue DynValue => dynValue;

		public ParamDesc ParamDesc
		{
			get => paramDesc;
			private set
			{
				paramDesc = value;
				OnPropertyChanged();
			}
		}

		public ErrorCodes ErrorCode
		{
			set => errorList.Add(value);
		}
		public IEnumerator<ErrorCodes> ErrorEnumerator => errorList.GetEnumerator();
		public  List<ErrorCodes> ErrorList => errorList.ErrorsList;
		public ErrorCodes GetError(int idx) => errorList[idx];
		public bool HasErrors => errorList.HasErrors;
		public void ResetErrors() => errorList.Reset();

		public bool Assigned { get; protected set; }

		public static ARevitParam Invalid
		{
			get
			{
				ARevitParam result = new RevitParamDefault(null);
				return result;
			}
		}

	#endregion

	#region public methods

		public abstract dynamic GetValue();

		public void UpdateProperties()
		{
			OnPropertyChanged(nameof(DynValue));
			OnPropertyChanged(nameof(ParamDesc));
		}

		public void SetValue<T>(T value)
		{
			if (gotValue)
			{
				gotValue = false;
				return;
			}

			dynValue.Value = value;

			gotValue = true;
			Assigned = true;

			OnPropertyChanged(nameof(DynValue));

		}

	#endregion

		// public IEnumerable<ErrorCodes> Errors()
		// {
		// 	if (errors == null) yield break;
		//
		// 	foreach (ErrorCodes error in errors)
		// 	{
		// 		yield return error;
		// 	}
		// }

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return	"<" + ParamDesc.ParameterName + ">|<" + (dynValue?.ToString() ?? "null value") + " >|< " + (HasErrors ? "Valid" : "Invalid") + ">";
		}


	}
}