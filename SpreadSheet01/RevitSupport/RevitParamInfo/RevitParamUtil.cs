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

		public static string MakeAnnoSymKey(   IAnnoSymContainer aSym, int nameIdex, int seqIndex, bool asSeqName = true)
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

		// provide a "clean" parameter name
		// provide label parameter id (as out)
		// provide bool if the info provided is for the label
		public static string GetParamName(  string paramName, ParamClass paramClass, out int id, out bool isLabel)
		{
			int idx1;
			int idx2;

			string name;

			isLabel = false;

			id = GetLabelIndex(paramName, out idx1, out idx2);

			if (id < 0) return paramName;

			if (idx1 == 1)
			{
				name = paramName.Substring(idx2 + 1).Trim();
				isLabel = true;
			}
			else
			{
				name = paramName.Substring(0, idx1-1).Trim();
			}

			return name;
		}

		public static int GetLabelIndex(string paramName, out int idx1, out int idx2)
		{
			idx2 = -1;
			idx1 = paramName.IndexOf(LABEL_ID_PREFIX) + 1;

			if (idx1 <= 0) return -1;

			idx2 = paramName.IndexOf(' ', idx1);

			idx2 = idx2 > 0 ? idx2 - idx1 : paramName.Length - idx1;

			bool result;
			int index;
			string test = paramName.Substring(idx1, idx2);

			result = int.TryParse(paramName.Substring(idx1, idx2), out index);

			return result ? index : -1;
		}

	}
}