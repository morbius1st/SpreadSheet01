#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.RevitSupport.RevitCellParameters;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  2/21/2021 6:04:19 AM

namespace Tests.CellsTests
{
	public class ParseTextParameter
	{
	#region private fields

		private Dictionary<string, RevitValueText> textValues = new Dictionary<string, RevitValueText>();

		private Dictionary<string, RevitCellItem> CellItemsBySeqName;
		private Dictionary<string, RevitCellItem> CellItemsByNameSeq;

		private int maxColWidth;

	#endregion

	#region ctor

		// public ParseTextParameter() { }

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void Process()
		{
			// test1();
			test2();

		}

	#endregion

	#region private methods

		private class OrderedParams
		{
			public string[] Value { get; private set; }

			public OrderedParams(string[] paramList)
			{
				Value = paramList;
			}
		}

		private void test2()
		{
			Console.WriteLine("begin text param tests");

			string[] actualParamNames = new []
			{
				"Name", "Sequence", "Text", "Excel Cell Name", "Excel Cell Address",
				"Formula", "Value as Number", "Calculation Results Text", 
				"Calculation Results Number", "Value Formatting Information", 
				"Global Parameter Name", "Data Direction Is To This Cell",
				"Has Error"
			};

			string[] readParamNames = new []
			{
				"Name", "Sequence", "Text (1,1)",  "Text (3,1)", "Text (3)", "Excel Cell Name", "Excel Cell Address",
				"Formula", "Value as Number", "Calculation Results Text", 
				"Calculation Results Number", "Value Formatting Information", 
				"Global Parameter Name", "Data Direction Is To This Cell",
				"Has Error"
			};

			foreach (string pname in actualParamNames)
			{
				if (pname.Length > maxColWidth) maxColWidth = pname.Length;
			}


			List<OrderedParams> paramList = new List<OrderedParams>()
			{
				//                                                                                                | cell |                 | calc | calc |
				//                         name   | seq  | text        | text        | text        | cell name    | addr | formula | val # | tx   | #    | format | global | dir | err
				new OrderedParams(new [] {"Name 1", "1", "Ex Text 1",  "Ex Text 2",  "Ex Text 3",  "Cell name 1", "",    "",       "",     "",    "",    "",      "",      "",   ""}),
				new OrderedParams(new [] {"Name 2", "2", "Text (a)",   "Text (b)",   "Text (c)",   "Cell name 1", "",    "",       "",     "",    "",    "",      "",      "",   "" }),
				new OrderedParams(new [] {"Name 3", "2", "Text (A)",   "Text (B)",   "Text (C)",   "",            "R1C1","",       "",     "",    "",    "",      "",      "",   "" }),
			};

			Console.WriteLine("\nbegin get cells");

			GetCells(readParamNames, paramList);

			listCellItems(actualParamNames);

		}

		private void listCellItems(string[] paramNames)
		{
			Console.WriteLine("\nbegin listing");

			ParamDesc pd;

			foreach (KeyValuePair<string, RevitCellItem> kvp in CellItemsBySeqName)
			{
				Console.WriteLine("\n  key|" + kvp.Key);
				Console.WriteLine(" name|" + kvp.Value.Name);

				if (kvp.Value.HasError.Value)
				{
					foreach (RevitCellErrorCode errorCode in kvp.Value.Errors)
					{
						Console.Write("error| " + errorCode.ToString());
					}

					Console.Write("\n");
				}
				
				for (var i = 0; i < paramNames.Length; i++)
				{
					string name = paramNames[i];

					pd = Match(name);

					int idx = pd.Index;

					if (pd.DataType != ParamDataType.TEXT)
					{
						string text = kvp.Value[idx]?.GetValue().ToString();

						text = text.IsVoid() ? "is void" : text;

						Console.WriteLine((name.PadRight(maxColWidth + 3) + "| .......  " + text));
					}
					else
					{
						
						RevitValueText text1 = (RevitValueText) kvp.Value[idx];

						Console.Write("read text1| " + text1.GetValue()
							+ " param name| " + text1.ParamDesc.ParameterName
							 );

						foreach (RevitCellErrorCode errCode in text1.Errors())
						{
							Console.Write(
								" error| " + errCode.ToString());
						}

						Console.Write("\n");

						foreach (RevitTextData text2 in ((RevitValueText) kvp.Value[idx]).TextValues())
						{
							Console.Write("read text2| "
								+ "   val| " + text2.GetValue()
								+ "   key| " + text2.GetKey()
								+ "   row| " + text2.Row 
								+ "   col| " + text2.Col
								+ "   pos| " + text2.Position
								+ "   seq| " + text2.Seq
								+ " valid| " + text2.IsValid
								+ " pname| " + text2.ParamDesc.ParameterName
								);
							foreach (RevitCellErrorCode errCode in text2.Errors())
							{
								Console.Write(
									" error| " + errCode.ToString());
							}

							Console.Write("\n");

							Console.WriteLine("  new text| " + text2.NewValue);
							
							Console.Write("\n");
						}
					}
				}
			}
		}



		private void GetCells(string[] paramNames, List<OrderedParams> paramList)
		{
			RevitCellItem ci;

			List<RevitCellItem> errorList = new List<RevitCellItem>();

			CellItemsBySeqName = new Dictionary<string, RevitCellItem>();
			CellItemsByNameSeq = new Dictionary<string, RevitCellItem>();

			foreach (OrderedParams orderedParams in paramList)
			{
				ci = catCell(paramNames, orderedParams);

				if (ci.HasError.Value)
				{
					errorList.Add(ci);
					continue;
				}

				string keySeqName = makeKey(ci, true);
				string keyNameSeq = makeKey(ci, false);

				CellItemsBySeqName.Add(keySeqName, ci);
				CellItemsByNameSeq.Add(keyNameSeq, ci);

			}
		}

		private RevitCellItem catCell(string[] parameters, OrderedParams op)
		{
			RevitCellItem ci = new RevitCellItem();
			ParamDesc pd;

			ci.AnnoSymbol = new AnnotationSymbol();

			int paramCount = 1;
			int textCount = 0;

			for (int i = 0; i < parameters.Length; i++)
			{
				string paramName = parameters[i];
				pd = Match(paramName);

				if (pd == null) continue;

				ci.CellParamDataType = pd.DataType;

				int idx = pd.Index;

				switch (pd.DataType)
				{
				case ParamDataType.STRING:
					{
						RevitValueString rs = new RevitValueString(op.Value[i], pd);
						ci[idx] = rs;
						paramCount++;
						break;
					}

				case ParamDataType.TEXT:
					{
						ci.AddText(op.Value[i], paramName, pd);
						textCount++;
						break;
					}

				case ParamDataType.NUMBER:
					{
						double info;
						bool result = double.TryParse(op.Value[i], out info);
						if (!result)
						{
							ci[idx] = new RevitValueNumber(Double.NaN, pd);
							ci[idx].ErrorCode = RevitCellErrorCode.INVALID_DATA_FORMAT_CS000I10;
							continue;
						}

						RevitValueNumber rn = new RevitValueNumber(info, pd);
						ci[idx] = rn;
						paramCount++;
						break;
					}

				case ParamDataType.BOOL:
					{
						int info;
						bool result = int.TryParse(op.Value[i], out info);

						if (!result)
						{
							ci[idx] = new RevitValueBool(false, pd);
							ci[idx].ErrorCode = RevitCellErrorCode.INVALID_DATA_FORMAT_CS000I10;
							continue;
						}

						RevitValueBool rb = new RevitValueBool(info == 1, pd);
						ci[idx] = rb;
						paramCount++;
						break;
					}
				}
			}
			return ci;
		}


/*
		private void test1()
		{
			Console.WriteLine("begin text parse tests");

			string[] testNames = new string[]
			{
				"Text (1,1)", "Text (2,1)", "Text (1,2)",
				"Text (3)", "text", "Text (x)", "Text",
				"Text (13,1)", "Text (1,13)", "Text (4,5)",
				"Text (1,1)"
			};

			string key;
			RevitValueText value;
			ParamDesc pd;

			foreach (string name in testNames)
			{
				Console.WriteLine("\ntesting| " + name);
				pd = RevitCellParameters.Match(name);
				value = new RevitValueText(name, name, pd);
				key = value.GetKey();

				// possible conditions
				// 1 all good - need to just add the key and proceed
				// 2. basically good but duplicate key
				// 3. invalid value for some reason
				// for 2 & 3
				// make an error key, try to add, adjust, try to add

				for (int i = 0; i < RevitValueText.MAX_CELLS; i++)
				{
					if (!textValues.ContainsKey(key))
					{
						textValues.Add(key, value);
						break;
					}

					value.SetDuplicateKey();
					key = value.GetKey();
				}

				Console.WriteLine("  value added| "
					+ " row| " + value.Row 
					+ " col| " + value.Col
					+ " pos| " + value.Position
					+ " seq| " + value.Seq
					+ " key| " + value.GetKey()
					+ " valid| " + value.IsValid
					+ " val| " + value.GetValue()
					);

				if (!value.IsValid)
				{
					Console.WriteLine("  err codes| ");
					foreach (RevitCellErrorCode errCode in value.Errors())
					{
						Console.WriteLine("   err code|" + errCode.ToString());
					}
				}

			}

		}
*/
		private string makeKey(RevitCellItem ci, bool asSeqName)
		{
			string seq = ((string) ci[SeqIdx].GetValue());
			seq = seq.IsVoid() ? "ZZZZZZ" : seq;
			seq = $"{seq,-8}";

			string name = ci.Name.IsVoid() ? "Un-named" : ci.Name;

			string eid = ci.AnnoSymbol.ElementId < 0 ? "null element" : ci.AnnoSymbol.ToString();

			if (asSeqName) return seq + name + eid;

			return name + seq + eid;
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ParseTextParameter";
		}

	#endregion
	}
}