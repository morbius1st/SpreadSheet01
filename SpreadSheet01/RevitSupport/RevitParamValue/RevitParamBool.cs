using SpreadSheet01.RevitSupport.RevitParamInfo;

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

			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_REQUIRED
				&& !value.HasValue
				)
			{
				ErrorCode = RevitCellErrorCode.PARAM_VALUE_MISSING_CS001102;
				this.dynValue.Value = null;
			}
			else
			{
				this.dynValue.Value = value;
			}
		}
	}
}