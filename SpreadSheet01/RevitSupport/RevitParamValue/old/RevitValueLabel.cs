using System.Collections.Generic;
using System.Runtime.InteropServices;
using Autodesk.Revit.DB.DirectContext3D;
using UtilityLibrary;

using static SpreadSheet01.RevitSupport.RevitCellParameters;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{

	public static class RevitValueSupport
	{
		public static string LABEL_ID_PREFIX = "#";

		// provide a "clean" parameter name
		// provide label parameter id (as out)
		// provide bool if the info provided is for the label
		public static string GetLabelName(string paramName, out int id, out bool isLabel)
		{
			int idx1;
			int idx2;

			string name;

			isLabel = false;

			id = GetLabelIndex(paramName, out idx1, out idx2);

			if (id < 0) return null;

			if (idx1 == 0)
			{
				name = paramName.Substring(idx2 + 1).Trim();
				isLabel = true;
			}
			else
			{
				name = paramName.Substring(0, idx1).Trim();
			}

			return name;
		}


		public static int GetLabelIndex(string paramName, out int idx1, out int idx2)
		{
			idx2 = -1;
			idx1 = paramName.IndexOf(LABEL_ID_PREFIX);

			if (idx1 < 0) return -1;

			idx2 = paramName.IndexOf(' ', idx1 + 1);

			idx2 = idx2 > 0 ? idx2 - idx1 : paramName.Length - idx1;

			bool result;
			int index;

			result = int.TryParse(paramName.Substring(idx1, idx2), out index);

			return result ? index : -1;
		}

		public static string MakeLabelKey(int paramIdx)
		{
			return "[" + paramIdx.ToString("D5") + "]";
		}
	}



	
	
	// each of the title #idx parameters are stored in the dictionary;
	/*
	process:
	0. pre-processer must parse name and provide
	1. provide label info - in any order
	2. provide label name, value, index, paramdesc
	3. if the label (pd.Index == LabelIdx)
		a. update base class info
		b. set id and name - do not read the value
		c. return
	4. if a sub-parameter
		a. create a new "ARevitParam" per specific data type;
		b. set id and name
		c. set the value when applies
		d. create the sub-parameter as a "LabelParameter"
		e. create the key
		f. save in dictionary
		g. return

	*/

	// this a class placed into the cell item list
	// each of the title #idx parameters are stored in the dictionary;
	// except for the primary category
	public class RevitParamLabel : ARevitParam
	{
		private Dictionary<string, LabelParameter> LabelParams = new Dictionary<string, LabelParameter>();

		// the label #idx is stored here
		private int id = 0;
		private string name;

		// provide information from family / Revit
		// info provided: the family's parameter name (sans id), its value, the id value, and the associated paramDesc
		public RevitParamLabel(string paramName, string value, int paramId, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;

			if (id == 0) id = paramId;

			configure(paramName, value, paramId);
		}

		public int Id => id;
		public string Key => "#" + id.ToString("D5");

		public override dynamic GetValue() => (string) value;

		// process a parameter name
		// return the "clean" parameter name
		private void configure(string paramName, string value, int paramId)
		{
			if (paramDesc.Index == LabelIdx)
			{
				// got the primary parameter;
				name = paramName;
				this.value = null;
			}
			else
			{
				classifyLabelParameter(paramName, value, paramId);
			}

		}


		private void classifyLabelParameter(string paramName, string value, int paramId)
		{
			switch (paramDesc.DataType)
			{
			case ParamDataType.STRING:
				{
					LabelParameterString ps = new LabelParameterString(value, paramDesc);
					LabelParams.Add(RevitValueSupport.MakeLabelKey(paramId), ps);
					break;
				}
			case ParamDataType.RELATIVEADDRESS:
				{
					LabelParameterString ps = new LabelParameterString(value, paramDesc);
					LabelParams.Add(RevitValueSupport.MakeLabelKey(paramId), ps);
					break;
				}
			case ParamDataType.DATATYPE:
				{
					LabelParameterString ps = new LabelParameterString(value, paramDesc);
					LabelParams.Add(RevitValueSupport.MakeLabelKey(paramId), ps);
					break;
				}
			case ParamDataType.BOOL:
				{
					LabelParameterString ps = new LabelParameterString(value, paramDesc);
					LabelParams.Add(RevitValueSupport.MakeLabelKey(paramId), ps);
					break;
				}
			case ParamDataType.NUMBER:
				{
					LabelParameterString ps = new LabelParameterString(value, paramDesc);
					LabelParams.Add(RevitValueSupport.MakeLabelKey(paramId), ps);
					break;
				}
			}
		}




	}

	public abstract class LabelParameter : ARevitParam
	{
		public LabelParameter()
		{
			// base.SetValue(value);
			// set(value);
		}

		public override dynamic GetValue() => (string) value;

		// protected abstract void set(string value);
		// {
		// 	gotValue = false;
		//
		// 	if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_REQUIRED
		// 		&& value.IsVoid() )
		// 	{
		// 		ErrorCode = RevitCellErrorCode.PARAM_VALUE_MISSING_CS001101;
		// 		this.value = null;
		// 	}
		// 	else
		// 	{
		// 		this.value = value;
		// 	}
		// }

	}

	
	public class LabelParameterString : LabelParameter
	{
		public LabelParameterString(string value, ParamDesc paramDesc) 
		{
			this.paramDesc = paramDesc;

			base.SetValue(value);

			// base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => (string) value;

		protected void set(string value)
		{
			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_IGNORE) return;

			// to prevent loop
			base.gotValue = false;
		
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
	
	public class LabelParameterNumber : LabelParameter
	{
		public LabelParameterNumber(double value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;

			base.SetValue(value);

			// base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => (double) value;

		protected void set(double value)
		{
			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_IGNORE) return;

			// to prevent loop
			base.gotValue = false;
		
			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_REQUIRED
				&& double.IsNaN(value))
			{
				ErrorCode = RevitCellErrorCode.PARAM_INVALID_CS001100;
				this.value = null;
			}
			else
			{
				this.value = value;
			}
		}
	}
		
	public class LabelParameterDataType : LabelParameter
	{
		public LabelParameterDataType(ParamDataType value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;

			base.SetValue(value);

			// base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => (double) value;

		protected void set(ParamDataType value)
		{
			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_IGNORE) return;

			// to prevent loop
			base.gotValue = false;
		
			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_REQUIRED)
			{
				ErrorCode = RevitCellErrorCode.PARAM_INVALID_CS001100;
				this.value = null;
			}
			else
			{
				this.value = value;
			}
		}
	}


}