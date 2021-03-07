using SpreadSheet01.RevitSupport.RevitParamInfo;
using UtilityLibrary;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class RevitParamFormula : ARevitParam
	{
		public RevitParamFormula(string value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;

			base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => dynValue.AsString();

		private void set(string value)
		{
			gotValue = false;

			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_REQUIRED
				&& value.IsVoid() )
			{
				ErrorCode = RevitCellErrorCode.PARAM_VALUE_MISSING_CS001102;
				this.dynValue.Value = null;
			}
			else
			{
				value = value.Trim();

				if (!value.StartsWith("="))
				{
					ErrorCode = RevitCellErrorCode.PARAM_VALUE_BAD_FORMULA_CS001106;
				}

				this.dynValue.Value = value;
			}
		}
	}
}