// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitParamAddr.cs
// Created:      2021-02-22 (9:53 PM)

using SpreadSheet01.ExcelSupport;
using UtilityLibrary;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class RevitParamRelativeAddr : ARevitParam
	{
		
		public int Row { get; private set; }
		public int Col { get; private set; }

		public RevitParamRelativeAddr(string value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;

			base.SetValue(value);
			set(value);
		}

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
				int row;
				int col;

				this.value = value;

				bool result = ExcelAssist.ParseRelativeAddress(value, out row, out col);

				Row = row;
				Col = col;

				if (!result)
				{
					ErrorCode = RevitCellErrorCode.PARAM_VALUE_BAD_REL_ADDR_CS001104;
				}

			}
		}
	}
}