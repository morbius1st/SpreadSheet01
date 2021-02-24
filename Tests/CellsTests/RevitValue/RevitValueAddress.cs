#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityLibrary;

#endregion

// user name: jeffs
// created:   2/21/2021 11:32:38 PM

namespace Tests.CellsTests.RevitValue
{
	// public class RevitValueAddress : ARevitValue
	// {
	// 	private Dictionary<string, RevitAddressData> addressValues = new Dictionary<string, RevitAddressData>();
	//
	// 	public RevitValueAddress(string paramName, ParamDesc paramDesc)
	// 	{
	// 		this.paramDesc = paramDesc;
	// 		base.SetValue("");
	// 		set(paramDesc.ParameterName);
	// 	}
	//
	// 	public override dynamic GetValue() => value;
	//
	// 	private void set(string value)
	// 	{
	// 		gotValue = false;
	// 		this.value = "";
	//
	// 		RevitAddressData ad = new RevitAddressData(value, paramDesc);
	//
	// 	}
	//
	// 	public IEnumerable<RevitAddressData> TextValues()
	// 	{
	// 		foreach (KeyValuePair<string, RevitAddressData> kvp in addressValues)
	// 		{
	// 			yield return kvp.Value;
	// 		}
	// 	}
	// }
	//
	// public class RevitAddressData
	// {
	// 	private string excelCellAddress;
	//
	// 	public RevitAddressData(string value)
	// 	{
	// 		set(value);
	// 	}
	//
	// 	public string GetValue() => excelCellAddress;
	//
	// 	public bool IsValid => !excelCellAddress.IsVoid();
	//
	// 	private void set(string address)
	// 	{
	// 		excelCellAddress = address;
	// 	}
	//
	// 	// private string parseRC(string rc)
	// 	// {
	// 	// 	string addr = rc.ToUpper();
	// 	// 	string r;
	// 	// 	string c;
	// 	//
	// 	// 	int row = -1;
	// 	// 	int col = -1;
	// 	//
	// 	// 	bool result;
	// 	//
	// 	// 	if (addr.StartsWith("R"))
	// 	// 	{
	// 	// 		int posC = addr.IndexOf('C');
	// 	//
	// 	// 		if (posC > 1 & addr.Length >= 4)
	// 	// 		{
	// 	// 			r = addr.Substring(1, posC - 1);
	// 	//
	// 	// 			result = Int32.TryParse(r, out row);
	// 	//
	// 	// 			if (result)
	// 	// 			{
	// 	// 				c = addr.Substring(posC + 1);
	// 	//
	// 	// 				result = Int32.TryParse(c, out col);
	// 	//
	// 	// 				if (result)
	// 	// 				{
	// 	// 					return convertRC(row, col);
	// 	// 				}
	// 	// 			}
	// 	// 		}
	// 	// 	}
	// 	//
	// 	// 	return null;
	// 	// }
	// 	//
	// 	// private string convertRC(int row, int col)
	// 	// {
	// 	// 	string r = row.ToString();
	// 	// 	string c = ((char) col % 26).ToString();
	// 	//
	// 	// 	if (col > 26)
	// 	// 	{
	// 	// 		c = ((char) ( (col / 26)+ 65)).ToString() + c;
	// 	// 	}
	// 	//
	// 	// 	return c + r;
	// 	// }
	//
	// }
}

