// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitValueAddr.cs
// Created:      2021-02-22 (9:53 PM)

using UtilityLibrary;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
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
				ErrorCode = RevitCellErrorCode.PARAM_VALUE_MISSING_CS001101;
				this.value = null;
			}
			else
			{
				this.value = value;
			}
		}
	}
}