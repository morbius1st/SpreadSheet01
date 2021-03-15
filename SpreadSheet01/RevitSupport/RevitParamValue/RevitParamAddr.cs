// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitParamAddr.cs
// Created:      2021-02-22 (9:53 PM)

namespace SpreadSheet01.RevitSupport
{
/*	public class RevitParamAddr : ARevitParam
	{
		public RevitParamAddr(string value, ParamDesc paramDesc)
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
				this.dynValue.Value = value;

				// bool result = ExcelAssist.ParseExcelAddress(value, out row, out col);
				//
				// if (!result)
				// {
				// 	ErrorCode = RevitCellErrorCode.PARAM_VALUE_BAD_ADDR_CS001105;
				// }
			}
		}
	}*/
}