using System.IO;

using SpreadSheet01.RevitSupport.RevitParamInfo;
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

			if (paramDesc.ReadReqmt == ParamReadReqmt.READ_VALUE_REQUIRED
				&& value.IsVoid() )
			{
				ErrorCode = RevitCellErrorCode.PARAM_VALUE_MISSING_CS001102;
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
					ErrorCode = RevitCellErrorCode.PARAM_CHART_BAD_FILE_PATH_CS001142;
				}
			}
		}
	}
}