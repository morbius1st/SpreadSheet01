﻿using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitParamManagement;
using SpreadSheet01.RevitSupport.RevitParamValue;
using UtilityLibrary;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class RevitParamWkShtName : ARevitParam
	{
		public RevitParamWkShtName(string value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;

			base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => (string) dynValue.Value;

		private void set(string value)
		{
			gotValue = false;

			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_REQUIRED
				&& value.IsVoid() )
			{
				ErrorCodes = ErrorCodes.PARAM_VALUE_MISSING_CS001102;
				this.dynValue.Value = null;
			}
			else
			{
				this.dynValue.Value = value;
			}
		}
	}
}