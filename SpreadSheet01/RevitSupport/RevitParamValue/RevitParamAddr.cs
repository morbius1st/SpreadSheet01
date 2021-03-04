// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitParamAddr.cs
// Created:      2021-02-22 (9:53 PM)

using SpreadSheet01.RevitSupport.RevitParamInfo;
using UtilityLibrary;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class RevitParamAddr : ARevitParam
	{
		public RevitParamAddr(string value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;

			base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => (string) dynValue.Value;

		private void set(string value)
		{
			gotValue = false;

			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_REQUIRED
				&& value.IsVoid() )
			{
				ErrorCode = RevitCellErrorCode.PARAM_VALUE_MISSING_CS001101;
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
	}
}