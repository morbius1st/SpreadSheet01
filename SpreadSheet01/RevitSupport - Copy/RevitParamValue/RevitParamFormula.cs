﻿using System;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitParamManagement;

using UtilityLibrary;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
/*
	={[A8]}  = excel cell address
	={$name} = system variable	
	={#name} = revit variable	
	={%name} = project parameter
	={!name} = global parameter
	={@name} = label name

*/
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
				ErrorCodes = ErrorCodes.PARAM_VALUE_MISSING_CS001102;
				this.dynValue.Value = null;
			}
			else
			{
				value = value.Trim();

				if (!value.StartsWith("="))
				{
					ErrorCodes = ErrorCodes.PARAM_VALUE_BAD_FORMULA_CS001106;
				}

				this.dynValue.Value = value;
			}
		}

		public string AsString => GetValue();
		public double AsDouble => dynValue.AsDouble();
		public bool AsBool => dynValue.AsBool();
		public int AsInteger => dynValue.AsInteger();
		public dynamic AsValue => dynValue.Value;

		public Type GetType => dynValue.BaseType();

		public bool Evaluate()
		{
			return true;
		}


		public override string ToString()
		{
			return "I am " + nameof(RevitParamFormula) + "| " + GetValue();
		}
	}
}