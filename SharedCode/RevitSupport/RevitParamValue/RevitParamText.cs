﻿
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitParamManagement;
//using static SharedCode.RevitSupport.RevitParamManagement.ErrorCodeList2;
using UtilityLibrary;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class RevitParamText : ARevitParam
	{
		public RevitParamText(string value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;

			// base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => dynValue.AsString();

		private void set(string value)
		{
			gotValue = false;

			if (paramDesc.ReadReqmt == ParamReadReqmt.RD_VALUE_REQUIRED
				&& value.IsVoid() )
			{
//				ErrCodeList.Add(this, ErrorCodes.CEL_VALUE_MISSING_CS001102);
				ErrorCode = ErrorCodes.CEL_VALUE_MISSING_CS001102;
				this.dynValue.Value = null;
			}
			else
			{
				this.dynValue.Value = value;
			}
		}
	}
}