// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitParamInteger.cs
// Created:      2021-03-15 (12:36 AM)

using System;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitParamManagement;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class RevitParamInteger : ARevitParam
	{
		public RevitParamInteger(int value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;
			base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => dynValue.AsDouble();

		private void set(int value)
		{
			gotValue = false;

			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_REQUIRED
				|| paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_REQD_IF_NUMBER)
			{
				ErrorCodes = ErrorCodes.PARAM_VALUE_NAN_CS001103;
				this.dynValue.Value = Int32.MinValue;
			}
			else
			{
				this.dynValue.Value = value;
				gotValue = true;
			}
		}
	}
}