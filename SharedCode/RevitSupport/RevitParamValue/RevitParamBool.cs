using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitParamManagement;
//using static SharedCode.RevitSupport.RevitParamManagement.ErrorCodeList2;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class RevitParamBool : ARevitParam
	{
		public RevitParamBool(bool? value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;
			base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => (bool?) dynValue.Value;

		private void set(bool? value)
		{
			gotValue = false;

			if (paramDesc.ReadReqmt == ParamReadReqmt.RD_VALUE_REQUIRED
				&& !value.HasValue
				)
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