
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitParamManagement;
using UtilityLibrary;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class RevitParamUpdateType : ARevitParam
	{
		public RevitParamUpdateType(string value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;

			base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => dynValue.Value;

		public CellUpdateTypeCode UpdateType => (CellUpdateTypeCode) dynValue.Value;

		// provide a string
		private void set(string value)
		{
			gotValue = false;

			if (paramDesc.ReadReqmt == ParamReadReqmt.RD_VALUE_REQUIRED
				&& value.IsVoid() )
			{
				ErrorCodes = ErrorCodes.PARAM_VALUE_MISSING_CS001102;
				this.dynValue.Value = null;
			}
			else
			{
				// got a string to validate
				this.dynValue.Value = 
					CellUpdateTypes.I.GetTypeCode(value);

				if (this.dynValue.Value == CellUpdateTypeCode.INVALID)
				{
					ErrorCodes = ErrorCodes.PARAM_CHART_BAD_UPDATE_TYPE_CS001144;
				}
			}
		}

	}
}