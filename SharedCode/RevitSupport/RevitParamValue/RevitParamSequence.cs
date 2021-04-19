
using System.Windows.Forms.VisualStyles;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitParamManagement;
//using static SharedCode.RevitSupport.RevitParamManagement.ErrorCodeList2;
using UtilityLibrary;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{

	public class RevitParamSequence : ARevitParam
	{
		public const char SEQ_DIVIDER  = '.';
		public const int MAX_SUFFIX = 2;
		public const int MAX_PREFIX = 4;

		private struct seqValue
		{
			public string prefix { get; private set; }
			public string suffix { get; private set; }

			public seqValue(string seq)
			{
				prefix = "";
				suffix = "";
				string s = seq;

				if (seq.IsVoid() || seq.Equals(SEQ_DIVIDER.ToString()))
				{
					s = "!";
				}
				
				s = s.Trim();

				string[] sq = s.Split(new []{SEQ_DIVIDER}, 2);

				prefix = formatPrefix(sq);
				suffix = formatSuffix(sq);

			}

			public string AsString()
			{
				return prefix + SEQ_DIVIDER + suffix;
			}

			private string formatPrefix(string[] sq)
			{
				string s = sq[0];

				if (preFormat(s, MAX_PREFIX, out s))
				{
					return s;
				}

				return sq[0].PadLeft(MAX_PREFIX);
			}

			private string formatSuffix(string[] sq)
			{
				string s;

				if (sq.Length == 1)
				{
					s = "";
				}
				else
				{
					s = sq[1];
				}

				if (preFormat(s, MAX_SUFFIX, out s))
				{
					return s;
				}

				return s + " ".Repeat(MAX_SUFFIX - s.Length);
			}

			private bool preFormat(string s, int max, out string result)
			{
				result = s;

				if (string.IsNullOrWhiteSpace(s))
				{
					result = " ".Repeat(max);
					return true;
				}

				if (s.Length >= max)
				{
					result = s.Substring(0, max);
					return true;
				}

				return false;
			}

		}

		public RevitParamSequence(string value, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;

			// base.SetValue(value);
			set(value);
		}

		public override dynamic GetValue() => dynValue.AsString();

		public new string AsString()
		{
			return ((seqValue) dynValue.Value).AsString();
		}


		private void set(string value)
		{
			gotValue = true;

			this.dynValue.Value =  new seqValue(value);
		}
	}
}