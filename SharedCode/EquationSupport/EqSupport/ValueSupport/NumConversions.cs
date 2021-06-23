#region using

using static SharedCode.EquationSupport.EqSupport.ValueSupport.NumSupport;

#endregion

// username: jeffs
// created:  6/18/2021 7:05:43 AM
using System;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;

namespace SharedCode.EquationSupport.EqSupport.ValueSupport
{
	public static class NumConversions
	{
	#region private fields

	#endregion

	#region ctor

		// public Conversions() { }

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public static bool FractToDouble(string fract, out double value)
		{
			if (string.IsNullOrWhiteSpace(fract))
			{
				value = InvalidDouble;

				return false;
			}

			Match m = Regex.Match(fract, @"(\d*)\s*(\d+)\/(\d*)");

			string num = m.Groups[1].Value;
			string numer = m.Groups[2].Value;
			string denom = m.Groups[3].Value;

			bool result;

			int wholeNum;
			int numerator;
			int denomerator = 1;

			try
			{
				int.TryParse(num, out wholeNum);
				
				result = int.TryParse(numer, out numerator) &&
					int.TryParse(denom, out denomerator);

				if (result)
				{
					value = wholeNum + (double) numerator / denomerator;

					return true;
				}


				// if (!string.IsNullOrWhiteSpace(num))
				// {
				// 	result = int.TryParse(num, out wholeNum)
				// }
				// else
				// {
				// 	wholeNum = 0;
				// }
				//
				// if (result)
				// {
				// 	result = int.TryParse(numer, out numerator);
				//
				// 	if (result)
				// 	{
				// 		result = int.TryParse(denom, out denomerator);
				//
				// 		if (result)
				// 		{
				// 			value = wholeNum + (double) numerator / denomerator;
				//
				// 			return true;
				// 		}
				// 	}
				// }
			}
			catch { }

			value = InvalidDouble;

			return false;
		}

		public static bool StringToDouble(string input, out double value)
		{
			if (!string.IsNullOrWhiteSpace(input))
			{
				try
				{
					if (double.TryParse(input, out value))
					{
						return true;
					}
				}
				catch { }
			}

			value = InvalidDouble;

			return false;
		}

		public static bool StringToInteger(string input, out int value)
		{
			if (!string.IsNullOrWhiteSpace(input))
			{
				try
				{
					if (int.TryParse(input, out value))
					{
						return true;
					}
				}
				catch { }
			}

			value = InvalidInt;

			return false;
		}

		public static bool StringToBoolean(string input, out bool? value)
		{
			bool val;

			if (!string.IsNullOrWhiteSpace(input))
			{
				try
				{
					if (bool.TryParse(input, out val))
					{
						value = val;
						return true;
					}
				}
				catch { }
			}

			value = InvalidBool;

			return false;
		}

		public static string DoubleToFractString(double num, int precision = 3, bool asPowerOfTwo = false)
		{
			if (num.Equals(InvalidDouble)) return "Invalid Fract";

			return null;
		}

		public static string DoubleToString(double num, string format = null)
		{
			if (num.Equals(InvalidDouble)) return "Invalid Dbl";
			return num.ToString(format ?? NumSupport.FmtDouble);
		}

		public static string IntToString(int num, string format = null)
		{
			if (num.Equals(InvalidInt)) return "Invalid Int";
			return num.ToString(format ?? NumSupport.FmtInteger);
		}

		public static string BoolToString(bool num)
		{
			if (num.Equals(InvalidBool)) return "Invalid Bool";
			return num.ToString();
		}

	#endregion

	#region private methods

		// private string FractAsPowerOfTwo(double num, int precision)
		// {
		// 	try
		// 	{
		//
		//
		//
		//
		// 	}
		// 	catch
		// 	{ }
		//
		// 	return null;
		// }

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public static string ToString()
		{
			return "this is Conversions";
		}

	#endregion
	}
}