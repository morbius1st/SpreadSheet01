using System;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitParamManagement;
//using static SharedCode.RevitSupport.RevitParamManagement.ErrorCodeList2;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class RevitParamInteger : ARevitParam
	{
		public RevitParamInteger(int value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;
			// base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => dynValue.AsDouble();

		private void set(int value)
		{
			gotValue = false;

			if (paramDesc.ReadReqmt == ParamReadReqmt.RD_VALUE_REQUIRED
				|| paramDesc.ReadReqmt == ParamReadReqmt.RD_VALUE_REQD_IF_NUMBER)
			{
//				ErrCodeList.Add(this, ErrorCodes.CEL_VALUE_NAN_CS001103);
				ErrorCode = ErrorCodes.CEL_VALUE_NAN_CS001103;
				this.dynValue.Value = Int32.MinValue;
			}
			else
			{
				this.dynValue.Value = value;
				gotValue = true;
			}
		}
	}
}