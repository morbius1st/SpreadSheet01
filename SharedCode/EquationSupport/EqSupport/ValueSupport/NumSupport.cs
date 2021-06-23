#region using

using SharedCode.EquationSupport.TokenSupport.Amounts;

#endregion

// username: jeffs
// created:  6/18/2021 7:14:38 AM

namespace SharedCode.EquationSupport.EqSupport.ValueSupport
{
	public static class NumSupport
	{
	#region private fields

	#endregion

	#region public fields

		public static string DefaultString = string.Empty;
		public static bool?  DefaultBool = false;
		public static double DefaultDouble = double.PositiveInfinity;
		public static int    DefaultInt = int.MinValue;
		public static object DefaultObj = null;
		public static UoM    DefaultUnit = null;

		public static bool?  InvalidBool = null;
		public static string InvalidString = null;
		public static double InvalidDouble = double.NegativeInfinity;
		public static int    InvalidInt = int.MaxValue;
		public static object InvalidObj = null;
		public static UoM    InvalidUnit = null;

		public static string FmtDouble = "F4";
		public static string FmtInteger = "D4";

	#endregion

	#region ctor

		// public NumSupport() { } 

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public static string ToString()
		{
			return "this is NumSupport";
		}

	#endregion


	}
}