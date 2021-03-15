using System.Text.RegularExpressions;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using UtilityLibrary;

namespace SpreadSheet01.RevitSupport.RevitParamInfo
{
	public static class RevitParamUtil
	{
		private const string KEY_IDX_BEGIN  = "《";
		private const string KEY_IDX_END    = "》";

		private static int annoSymUniqueIdx = 0;

		public static string MakeLabelKey(int paramIdx)
		{
			return KEY_IDX_BEGIN + paramIdx.ToString("D2") + KEY_IDX_END;
		}

		public static string MakeSeqNameKey(string nameIn, string seqIn)
		{
			string seq = KEY_IDX_BEGIN + $"{(seqIn.IsVoid() ? "ZZZZZ" : seqIn),8}" + KEY_IDX_END;

			string name = nameIn.IsVoid() ? "un-named" : nameIn;

			return seq + name;
		}


		public static string MakeAnnoSymKey(IAnnoSymContainer aSym, int nameIdex, int seqIndex, bool asSeqName = true)
		{
			string seq = aSym[seqIndex].GetValue();

			seq = KEY_IDX_BEGIN + $"{(seq.IsVoid() ? "ZZZZZ" : seq),8}" + KEY_IDX_END;

			string name = aSym[nameIdex].GetValue();
			name = name.IsVoid() ? "un-named" : name;

			string eid = aSym.AnnoSymbol?.Id.ToString() ?? "Null Symbol " + annoSymUniqueIdx++.ToString("D7");

			if (asSeqName) return seq + name + eid;

			return name + seq + eid;

		}

		public static string LABEL_ID_PREFIX = "#";

		public static string GetRootName(string name, out int id, out bool isLabel)
		{
			id = -1;
			isLabel = false;

			if (name.IsVoid()) return "";

			string test = name.Trim();

			// Regex rx = new Regex(@"(?<=^\#\d|^\#\d\d)(\s+)(.*[^\s])|^(.*)(?=\s\#\d{1,2}\s*$)");
			Regex rx = new Regex(@"((?>^(?<name>.*)(?=\s\#(?<digits>\d{1,2})\s*$).*)|(?>(?>^\s*\#(?<digits>\d{1,2})\s+)(?<name>.*[^\s]))|(?<name>.*[^\s]))",
				RegexOptions.ExplicitCapture);
			Match m = rx.Match(name);

			if (!m.Success) return name;

			if (m.Groups.Count > 1)
			{
				bool result = int.TryParse(m.Groups["digits"].Value, out id);

				if (!result)
				{
					id = -1;
				}
				else
				{
					isLabel = true;
				}
			}

			return m.Groups["name"].Value;

		}
	}
}