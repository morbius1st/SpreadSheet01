namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class RevitParamNumber : ARevitParam
	{
		public RevitParamNumber(double? value, ParamDesc paramDesc)
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
				ErrorCode = RevitCellErrorCode.PARAM_VALUE_NAN_CS001103;
				this.value = double.NaN;
			}
			else
			{
				this.value = value;
			}
		}
	}
}