using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpreadSheet01.RevitSupport.RevitParamInfo;
using SpreadSheet01.RevitSupport.RevitParamValue;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class RevitParamDefault : ARevitParam
	{
		public RevitParamDefault(dynamic dynValue)
		{
			this.dynValue = new DynamicValue(dynValue);
			errors = new List<RevitCellErrorCode>();
			paramDesc = ParamDesc.Empty;
			gotValue = dynValue != null;
			Assigned = gotValue;
		}

		public override dynamic GetValue() => dynValue;
	}

	public abstract class ARevitParam : INotifyPropertyChanged
	{
	#region private fields

		protected DynamicValue dynValue = new DynamicValue();

		protected List<RevitCellErrorCode> errors;

		protected ParamDesc paramDesc;

		protected bool gotValue;

	#endregion

	#region public properties

		public abstract dynamic GetValue();

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
		
		public RevitCellErrorCode ErrorCode
		{
			set
			{
				if (errors == null) errors = new List<RevitCellErrorCode>();

				errors.Add(value);
			}
		}

		public bool Assigned { get; protected set; }

		public bool IsValid => errors == null || errors.Count == 0;

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

		public IEnumerable<RevitCellErrorCode> Errors()
		{
			if (errors == null) yield break;

			foreach (RevitCellErrorCode error in errors)
			{
				yield return error;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return	"<" + ParamDesc.ParameterName + ">|<" + (dynValue?.ToString() ?? "null value") + " >|< " + (IsValid ? "Valid" : "Invalid") + ">";
		}


	}
}