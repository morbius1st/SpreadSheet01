using System.Collections.Generic;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public abstract class ARevitParam
	{
		protected dynamic value;

		protected List<RevitCellErrorCode> errors;

		protected ParamDesc paramDesc;

		protected bool gotValue;

		public abstract dynamic GetValue();

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
			private set => paramDesc = value;
		}

		public bool IsValid => errors == null;

		public void SetValue<T>(T value)
		{
			if (gotValue)
			{
				gotValue = false;
				return;
			}

			gotValue = true;
			Assigned = true;

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

		public override string ToString()
		{
			return "<" + (value.ToString() ?? "null value") + " >|< " + (IsValid ? "Valid" : "Invalid") + ">";
		}
	}

	// generic string value
}