﻿
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitParamManagement;
//using static SharedCode.RevitSupport.RevitParamManagement.ErrorCodeList2;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class RevitParamNumber : ARevitParam
	{
		public RevitParamNumber(double value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;
			base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => dynValue.AsDouble();

		private void set(double value)
		{
			gotValue = false;

			if (paramDesc.ReadReqmt == ParamReadReqmt.RD_VALUE_REQUIRED
				|| paramDesc.ReadReqmt == ParamReadReqmt.RD_VALUE_REQD_IF_NUMBER
				|| double.IsNaN(value))
			{
//				ErrCodeList.Add(this, ErrorCodes.CEL_VALUE_NAN_CS001103);
				ErrorCode = ErrorCodes.CEL_VALUE_NAN_CS001103;
				this.dynValue.Value = double.NaN;
			}
			else
			{
				this.dynValue.Value = value;
			}
		}
	}
}