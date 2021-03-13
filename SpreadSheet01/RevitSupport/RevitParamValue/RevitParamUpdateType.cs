using SpreadSheet01.RevitSupport.RevitParamInfo;
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

			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_REQUIRED
				&& value.IsVoid() )
			{
				ErrorCode = RevitCellErrorCode.PARAM_VALUE_MISSING_CS001102;
				this.dynValue.Value = null;
			}
			else
			{
				// got a string to validate
				this.dynValue.Value = 
					CellUpdateTypes.I.GetTypeCode(value);

				if (this.dynValue.Value == CellUpdateTypeCode.INVALID)
				{
					ErrorCode = RevitCellErrorCode.PARAM_CHART_BAD_UPDATE_TYPE_CS001144;
				}
			}
		}

	}
}