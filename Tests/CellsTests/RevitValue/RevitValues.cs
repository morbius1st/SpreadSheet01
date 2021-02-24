using System.Collections;
using System.Collections.Generic;
using Tests.CellsTests.RevitValue;
using UtilityLibrary;
using static Tests.CellsTests.RevitCellErrorCode;

// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitValueText.cs
// Created:      2021-02-20 (5:35 AM)


namespace Tests.CellsTests.RevitValue
{
	public abstract class ARevitValue
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
			return value.ToString() + " >|< " + (IsValid ? "Valid" : "Invalid");
		}
	}

	// generic string value
	public class RevitValueString : ARevitValue
	{
		public RevitValueString(string value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;

			base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => (string) value;

		private void set(string value)
		{
			gotValue = false;

			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_REQUIRED
				&& value.IsVoid() )
			{
				ErrorCode = PARAM_VALUE_MISSING_CS001101;
				this.value = null;
			}
			else
			{
				this.value = value;
			}
		}
	}
	
	// generic double value
	public class RevitValueNumber : ARevitValue
	{
		public RevitValueNumber(double? value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;
			base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => (double?) value;

		private void set(double? value)
		{
			gotValue = false;

			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_REQUIRED
				|| paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_REQD_IF_NUMBER
				&& (!value.HasValue
				|| (value.HasValue && double.IsNaN(value.Value))) 
				
				)
			{
				ErrorCode = PARAM_VALUE_NAN_CS001103;
				this.value = double.NaN;
			}
			else
			{
				this.value = value;
			}
		}
	}

	// generic bool value
	public class RevitValueBool : ARevitValue
	{
		public RevitValueBool(bool? value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;
			base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => (bool?) value;

		private void set(bool? value)
		{
			gotValue = false;

			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_REQUIRED
				&& !value.HasValue
				)
			{
				ErrorCode = PARAM_VALUE_MISSING_CS001101;
				this.value = null;
			}
			else
			{
				this.value = value;
			}
		}
	}

	
	// address value
	public class RevitValueAddr : ARevitValue
	{
		public RevitValueAddr(string value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;

			base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => (string) value;

		private void set(string value)
		{
			gotValue = false;

			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_REQUIRED
				&& value.IsVoid() )
			{
				ErrorCode = PARAM_VALUE_MISSING_CS001101;
				this.value = null;
			}
			else
			{
				this.value = value;
			}
		}
	}

}