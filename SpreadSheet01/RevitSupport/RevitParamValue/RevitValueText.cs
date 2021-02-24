using System.Collections.Generic;

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	
	public class RevitValueText : ARevitValue
	{
		private Dictionary<string, RevitTextData> textValues = new Dictionary<string, RevitTextData>();

		public RevitValueText(string paramName, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;
			base.SetValue("");
			set(paramDesc.ParameterName);
		}

		public override dynamic GetValue() => value;

		public void AddText(string value, string paramName, ParamDesc paramDesc)
		{
			RevitTextData rt = new RevitTextData(value, paramName, paramDesc);

			if (!rt.IsValid)
			{
				ErrorCode = RevitCellErrorCode.PARAM_INVALID_CS001100;
			}

			string key = rt.GetKey();

			bool result = textValues.ContainsKey(key);

			if (result)
			{
				rt.ErrorCode = RevitCellErrorCode.DUPLICATE_KEY_CS000I01;

				ErrorCode = RevitCellErrorCode.DUPLICATE_KEY_CS000I01;

				do
				{
					key = rt.GetNextKey();
					result = textValues.ContainsKey(key);
				}
				while (result);
			}

			textValues.Add(key, rt);
		}

		private void set(string value)
		{
			gotValue = false;
			this.value = value;
		}

		public IEnumerable<RevitTextData> TextValues()
		{
			if (textValues == null) yield break;

			foreach (KeyValuePair<string, RevitTextData> kvp in textValues)
			{
				yield return kvp.Value;
			}
		}
	}

	public class RevitTextData : ARevitValue
	{
		public static int MAX_CELLS = 12;

		private const string ERROR_STRING = "Text Param Err ";

		private string seq;
		private int row = 1;
		private int col = 1;

		public RevitTextData(string origValue, string paramName, ParamDesc paramDesc)
		{
			this.paramDesc = paramDesc;
			this.value = origValue;
			Position = paramName;

			if (!IsValid) return;
		}

		public string NewValue {get; set; }

		public override dynamic GetValue() => (string) this.value;

		public string Seq
		{
			get => seq;
			private set => seq = value;
		}

		public int Row
		{
			get => row;
			set => row = value;
		}

		public int Col
		{
			get => col;
			set => col = value;
		}

		public string Position
		{
			get => "(" + row + "," + col + ")";
			set
			{
				if (!setPosition(value)) setInvalidPosition();
			}
		}

		private int errorSeq = 0;

		public string GetKey()
		{
			return row.ToString("D3") + col.ToString("D3");
		}

		public string GetNextKey()
		{
			return ERROR_STRING + (errorSeq++).ToString();
		}

		public void SetDuplicateKey()
		{
			if (errorSeq > 0) return;

			errorSeq = 1;
			ErrorCode = RevitCellErrorCode.DUPLICATE_KEY_CS000I01;
			SetInvalid();
		}

		// note that position must be set first
		private void setSeq()
		{
			Seq = (MAX_CELLS * col + row).ToString("D3");
		}

		private bool setPosition(string value)
		{
			int pos1 = value.IndexOf('('); // req'd
			int pos4 = value.IndexOf(')'); // req'd

			if (pos1 == -1 || pos4 == -1) return false;

			int pos2 = value.IndexOf(','); // may be missing

			int len1;
			int len2;


			if (pos2 < 0)
			{
				len1 = pos4 - pos1 - 1;
				len2 = -1;
			}
			else
			{
				len1 = pos2 - pos1 - 1;
				len2 = pos4 - pos2 - 1;
			}

			if (len1 == 0 || len2 == 0) return false;

			if (!int.TryParse(value.Substring(pos1 + 1, len1), out row)) return false;

			if (pos2 > 1)
			{
				if (!int.TryParse(value.Substring(pos2 + 1, len2), out col)) return false;
			}
			else
			{
				col = 1;
			}

			if (row <= 0 || col <= 0 || (row * col) > MAX_CELLS) return false;

			setSeq();

			return true;
		}

		private void setInvalidPosition()
		{
			ErrorCode = RevitCellErrorCode.LOCATION_BAD_CS001115;
			SetInvalid();
		}

		public void SetInvalid()
		{
			seq = ERROR_STRING;
			row = -1;
			col = -1;
		}

		public override string ToString()
		{
			return this.value + " [" + row + "," + col + "]";
		}
	}

}