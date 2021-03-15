// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitParamAddr.cs
// Created:      2021-02-22 (9:53 PM)

namespace SpreadSheet01.RevitSupport
{
/*	public class RevitParamRelativeAddr : ARevitParam
	{

		public int Row { get; private set; }
		public int Col { get; private set; }

		public RevitParamRelativeAddr(string value, ParamDesc paramDesc)
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
				int row;
				int col;

				this.dynValue.Value = value;

				bool result = ExcelAssist.ParseRelativeAddress(value, out row, out col);

				Row = row;
				Col = col;

				if (!result)
				{
					ErrorCode = RevitCellErrorCode.PARAM_VALUE_BAD_REL_ADDR_CS001104;
				}

			}
		}
	}*/
}