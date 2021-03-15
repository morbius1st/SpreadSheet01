#region using

#endregion

// username: jeffs
// created:  2/28/2021 1:58:41 PM

namespace SpreadSheet01
{
	public class ExcelAssist
	{
	#region private fields

	#endregion

	#region ctor

		public ExcelAssist() { }

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		// public static string AddToCellAddr(string cellAddr, int irow, int icol)
		// {
		// 	if (irow == 0 && icol == 0) return cellAddr;
		//
		// 	int row;
		// 	int col;
		//
		// 	bool result = ConvertExcelAddress(cellAddr, out row, out col);
		//
		// 	if (!result) return null;
		//
		// 	row += irow > 0 ? irow - 1 : 0;
		// 	col += icol > 0 ? icol - 1 : 0;
		//
		// 	result = validateExcelRowCol(row, col);
		//
		// 	if (!result) return null;
		//
		// 	string addr = ColIntToCellAddr(col) + row.ToString();
		//
		// 	return addr;
		// }



		// public static bool ConvertExcelAddress(string addr, out int row, out int col)
		// {
		// 	bool result = false;
		// 	row = 0;
		// 	col = 0;
		//
		// 	string test = addr.ToUpper().Trim();
		//
		// 	char rows;
		// 	char cols;
		//
		// 	bool numbers = false;
		//
		// 	char[] digits = test.ToCharArray();
		//
		// 	int len = digits.Length;
		//
		// 	for (var i = 0; i < digits.Length; i++)
		// 	{
		// 		if (!numbers)
		// 		{
		// 			if (!digits[i].IsUpperAlpha() || i + 1 == len) return false;
		//
		// 			col *= 26;
		// 			col += (digits[i] - 'A' + 1);
		//
		// 			if (digits[i + 1].IsNumber()) numbers = true;
		// 		}
		// 		else
		// 		{
		// 			if (!digits[i].IsNumber()) return false;
		//
		// 			row *= 10;
		// 			row += (digits[i] - '0');
		// 		}
		// 	}
		//
		// 	if (!validateExcelRowCol(row, col)) return false;
		//
		// 	return true;
		// }

		// private static bool validateExcelRowCol(int row, int col)
		// {
		// 	return (row > 0 && row <= 1048576 && col > 0 && col <= 16384);
		// }
		//
		// public static int CellAddrToColInt(char[] cols)
		// {
		// 	int col = 0;
		//
		// 	for (var i = 0; i < cols.Length; i++)
		// 	{
		// 		col *= 26;
		// 		col += (cols[i] - 'A' + 1);
		// 	}
		//
		// 	return col;
		// }
		//
		//
		// public static string ColIntToCellAddr(int col)
		// {
		//
		// 	int dividend = col;
		// 	string cols = String.Empty;
		// 	int modulo;
		//
		// 	while (dividend > 0)
		// 	{
		// 		modulo = (dividend - 1) % 26;
		// 		cols = Convert.ToChar(65+modulo).ToString() + cols;
		// 		dividend = (int) ((dividend - modulo) / 26);
		// 	}
		//
		// 	return cols;
		// }


		public static bool ParseRelativeAddress(string ra, out int row, out int col)
		{
			bool result = false;

			row = 0;
			col = 0;

			string test = ra.Trim();
			string rows;
			string cols;

			int pos1 = test.IndexOf('(');
			if (pos1 < 0) return false;

			int pos3 = test.IndexOf(')');
			if (pos3 < 0) return false;

			int pos2 = test.IndexOf(',');

			pos2 = pos2 > 0 ? pos2 : pos3;

			rows = test.Substring(pos1 + 1, pos2 - pos1 - 1);

			result = int.TryParse(rows, out row);

			if (!result)
			{
				return false;
			}

			if (pos2 == pos3)
			{
				col = 1;
				return true;
			}

			cols = test.Substring(pos2 + 1, pos3 - pos2 - 1);

			result = int.TryParse(cols, out col);

			if (!result)
			{
				return false;
			}

			return result;
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
			return "this is ExcelAssist";
		}

	#endregion
	}
}