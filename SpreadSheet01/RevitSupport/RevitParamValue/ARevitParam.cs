using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UtilityLibrary;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public abstract class ARevitParam : INotifyPropertyChanged
	{
		protected dynamic value;

		protected List<RevitCellErrorCode> errors;

		protected ParamDesc paramDesc;

		protected bool gotValue;

		public abstract dynamic GetValue();

		public dynamic Value => value;

		public RevitCellErrorCode ErrorCode
		{
			set
			{
				if (errors == null) errors = new List<RevitCellErrorCode>();

				errors.Add(value);
			}
		}

		public bool Assigned { get; private set; }

		public ParamDesc ParamDesc
		{
			get => paramDesc;
			private set
			{
				paramDesc = value;
				OnPropertyChanged();
			}
		}

		public bool IsValid => errors == null;

		public void UpdateProperties()
		{
			OnPropertyChanged(nameof(Value));
			OnPropertyChanged(nameof(ParamDesc));
		}


		public void SetValue<T>(T value)
		{
			if (gotValue)
			{
				gotValue = false;
				return;
			}

			gotValue = true;
			Assigned = true;

			OnPropertyChanged(nameof(Value));

		}

		// return false if do not process
		public static bool ReadValue(ParamDesc paramDesc)
		{
			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_IGNORE) return false;

			return true;
		}

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
			return "<" + (value?.ToString() ?? "null value") + " >|< " + (IsValid ? "Valid" : "Invalid") + ">";
		}
	}

	// generic string value
}