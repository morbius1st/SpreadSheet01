#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

		private Dictionary<string, RevitParamText> textValues = new Dictionary<string, RevitParamText>();

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

		private void test3()
		{
			SampleAnnoSymbols sample = new SampleAnnoSymbols();
			sample.Process();

			GetCells(sample);

		}


		private void GetCells(SampleAnnoSymbols sample)
		{
			RevitCellItem ci;

			List<RevitCellItem> errorList = new List<RevitCellItem>();

			CellItemsBySeqName = new Dictionary<string, RevitCellItem>();
			CellItemsByNameSeq = new Dictionary<string, RevitCellItem>();

			foreach (AnnotationSymbol annoSym in sample.Symbol)
			{

				ci = categorizeCellParameters(annoSym);

				if (ci.HasError == true)
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



		private RevitCellItem categorizeCellParameters(AnnotationSymbol annoSym)
		{
			RevitCellItem ci = new RevitCellItem();

			if (annoSym == null)
			{
				ci.Error = RevitCellErrorCode.INVALID_ANNO_SYM_CS001120;
				return ci;
			}

			ci.AnnoSymbol = annoSym;

			IList<Parameter> parameters = annoSym.GetOrderedParameters();

			int totalParams = parameters.Count;
			int paramCount = 0;
			double labelCount = 0;

			foreach (Parameter param in parameters)
			{
				string name = param.Definition.Name;

				ParamDesc pd = RevitCellParameters.Match(name);

				if (pd == null)  continue;

				bool result = ci.Add(pd, param);

				if (result)
				{
					ci.Error = RevitCellErrorCode.INVALID_DATA_FORMAT_CS000I10;
				}

				if (pd.MetaType == ParamMetaType.INFORMATION)
				{
					paramCount++;
				}
				else
				{
					if (pd.Index == LabelIdx)
					{
						labelCount += lblCountIncrement;
					}
					else
					{
						labelCount += lblCountItemIncrementValue;
					}
				}
			}

			if (paramCount == 0 || paramCount != ReqdParamCount ||
				labelCount.Equals(0) || 
				((labelCount * lblCountItemIncrementBasis) % lblCountItemIncrementBasis) != 0)
			{
				ci.Error = RevitCellErrorCode.PARAM_MISSING_CS001102;
			}

			return ci;
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
						RevitParamString rs = new RevitParamString(op.Value[i], pd);
						ci[idx] = rs;
						paramCount++;
						break;
					}

				// case ParamDataType.TEXT:
				// 	{
				// 		ci.AddText(op.Value[i], paramName, pd);
				// 		textCount++;
				// 		break;
				// 	}

				case ParamDataType.NUMBER:
					{
						double info;
						bool result = double.TryParse(op.Value[i], out info);
						if (!result)
						{
							ci[idx] = new RevitParamNumber(Double.NaN, pd);
							ci[idx].ErrorCode = RevitCellErrorCode.INVALID_DATA_FORMAT_CS000I10;
							continue;
						}

						RevitParamNumber rn = new RevitParamNumber(info, pd);
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
							ci[idx] = new RevitParamBool(false, pd);
							ci[idx].ErrorCode = RevitCellErrorCode.INVALID_DATA_FORMAT_CS000I10;
							continue;
						}

						RevitParamBool rb = new RevitParamBool(info == 1, pd);
						ci[idx] = rb;
						paramCount++;
						break;
					}
				}
			}
			return ci;
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

					if (pd.MetaType != ParamMetaType.LABEL)
					{
						string text = kvp.Value[idx]?.GetValue().ToString();

						text = text.IsVoid() ? "is void" : text;

						Console.WriteLine((name.PadRight(maxColWidth + 3) + "| .......  " + text));
					}
					else
					{
						
						RevitParamText text1 = (RevitParamText) kvp.Value[idx];

						Console.Write("read text1| " + text1.GetValue()
							+ " param name| " + text1.ParamDesc.ParameterName
							 );

						foreach (RevitCellErrorCode errCode in text1.Errors())
						{
							Console.Write(
								" error| " + errCode.ToString());
						}

						Console.Write("\n");

						foreach (RevitTextData text2 in ((RevitParamText) kvp.Value[idx]).TextValues())
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