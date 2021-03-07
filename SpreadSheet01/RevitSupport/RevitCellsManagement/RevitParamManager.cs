#region using

using SpreadSheet01.RevitSupport.RevitParamInfo;

#endregion

// username: jeffs
// created:  3/3/2021 7:51:06 PM

namespace SpreadSheet01.RevitSupport.RevitCellsManagement
{
	public enum ParamClass
	{
		CHART,
		LABEL
	}

	public class RevitParamManager
	{
	#region private fields
		
	#endregion

	#region ctor

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static ParamDesc Match(string fullName, ParamClass paramClass)
		{
			ParamDesc pd;

			switch (paramClass)
			{
			case ParamClass.CHART:
				{
					break;
				}
			case ParamClass.LABEL:
				{
					return RevitCellParameters.Match(fullName);
					break;
				}
			}

			return ParamDesc.Empty;
		}

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is RevitParamManager";
		}

	#endregion
	}
}