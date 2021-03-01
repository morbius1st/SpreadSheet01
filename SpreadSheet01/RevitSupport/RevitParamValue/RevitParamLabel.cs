using UtilityLibrary;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class RevitParamLabel: ARevitParam
	{
		public RevitParamLabel(string value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;

			base.SetValue(value);
			set(value);
		}

		public string LabelValueName { get; set; }

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