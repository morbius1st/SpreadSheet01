
using System.IO;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitParamManagement;
using UtilityLibrary;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class RevitParamFilePath : ARevitParam
	{
		private FilePath<FileNameSimple> excelFilPath;

		public RevitParamFilePath(string value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;

			base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => dynValue.AsString();

		public FilePath<FileNameSimple> FilePath => excelFilPath;

		public string FullFilePath => excelFilPath.FullFilePath;
		public bool Exists => excelFilPath.Exists;

		private void set(string value)
		{
			gotValue = false;

			if (paramDesc.ReadReqmt == ParamReadReqmt.RD_VALUE_REQUIRED
				&& value.IsVoid() )
			{
				ErrorCode = ErrorCodes.PARAM_VALUE_MISSING_CS001102;
				this.dynValue.Value = null;
				excelFilPath = FilePath<FileNameSimple>.Invalid;
			}
			else
			{
				
			#if NOREVIT

				value = Path.Combine(@"B:\Programming\VisualStudioProjects\RevitProjects\SpreadSheet\Revit", Path.GetFileName(value));
				
			#endif

				this.dynValue.Value = value;

				excelFilPath = new FilePath<FileNameSimple>(value);

				if (!excelFilPath.IsValid || !excelFilPath.IsFound)
				{
					ErrorCode = ErrorCodes.CHART_BAD_FILE_PATH_CS001142;
				}
			}
		}
	}
}