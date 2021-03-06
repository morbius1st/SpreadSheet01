﻿using System.Text.RegularExpressions;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitParamValue;
using UtilityLibrary;

using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamType;

namespace SpreadSheet01.RevitSupport.RevitParamManagement
{
	public static class RevitParamUtil
	{
		private const string KEY_IDX_BEGIN  = "《";
		private const string KEY_IDX_END    = "》";

		private static int annoSymUniqueIdx = 0;

		public static string MakeLabelKey(string seqId, int paramIdx, int ext = 0)
		{
			// string seq = $"{(seqId.IsVoid() ? "ZZZZZ" : seqId), 8}";
			// string id = paramIdx.ToString("D2");
			// string extn = ext.ToString("D2");
			// return seq + "|" + id + "." + extn;

			string key = $"{(seqId.IsVoid() ? "ZZZZZ" : seqId), 8}.{ext:D2}|{paramIdx:D2}";

			return key;
		}

		public static string MakeCellKey( string seqIn, string nameIn, int ext)
		{
			string seq = $"{(seqIn.IsVoid() ? "ZZZZZ" : seqIn),8}.{ext:D2}|";

			string name = nameIn.IsVoid() ? "un-named" : nameIn;

			return seq + name;
		}


		public static string MakeAnnoSymKey(IAnnoSymContainer<ARevitParam> aSym, int nameIdex, int seqIndex, bool asSeqName = true)
		{
			string seq = aSym[PT_INSTANCE, seqIndex].GetValue();

			seq = $"{(seq.IsVoid() ? "ZZZZZ" : seq),8}" + "|";

			string name = aSym[PT_INSTANCE, nameIdex].GetValue();
			name = (name.IsVoid() ? "un-named" : name) + "|";

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