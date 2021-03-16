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

		public override dynamic GetValue() => (bool?) value;

		private void set(bool? value)
		{
			gotValue = false;

			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_REQUIRED
				&& !value.HasValue
				)
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