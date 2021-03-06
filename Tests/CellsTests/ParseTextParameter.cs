/*

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

		private Dictionary<string, RevitCellParams> CellItemsBySeqName;
		private Dictionary<string, RevitCellParams> CellItemsByNameSeq;

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

		private void test1()
		{
			SampleAnnoSymbols sample = new SampleAnnoSymbols();
			sample.Process();

			RevitGroup rg0 = new RevitGroup();

			foreach (AnnotationSymbol annoSym in sample.Symbol)
			{
				RevitCellParams cp = addParameters(annoSym.parameters);

				string key = cp.Key;

				rg0.CellParams.Add(key, cp);
			}

		}

		private RevitCellParams addParameters(IList<Parameter> paramList)
		{
			RevitCellParams cp1A = new RevitCellParams();

			IList<Parameter[]> Label = new List<Parameter[]>();

			foreach (Parameter param in paramList)
			{
				ParamDesc pd = Match(param.Definition.Name);

				if (pd.Group == ParamGroup.DATA)
				{
					cp1A.CellValues[pd.ParamIndex] = new RevitParamText(param.AsString(), pd);
				}
				else
				{
					int pos1;
					int pos2;
					int idx =
						RevitParamUtil.GetLabelIndex(param.Definition.Name, 
							out pos1, out pos2);


				}


			}

			return cp1A;
		}




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
			RevitCellParams cp;

			List<RevitCellParams> errorList = new List<RevitCellParams>();

			CellItemsBySeqName = new Dictionary<string, RevitCellParams>();
			CellItemsByNameSeq = new Dictionary<string, RevitCellParams>();

			foreach (AnnotationSymbol annoSym in sample.Symbol)
			{

				cp = categorizeCellParameters(annoSym);

				if (cp.HasError == true)
				{
					errorList.Add(cp);
					continue;
				}

				string keySeqName = makeKey(cp, true);
				string keyNameSeq = makeKey(cp, false);

				CellItemsBySeqName.Add(keySeqName, cp);
				CellItemsByNameSeq.Add(keyNameSeq, cp);
			}
		}



		private RevitCellParams categorizeCellParameters(AnnotationSymbol annoSym)
		{
			RevitCellParams cp = new RevitCellParams();

			if (annoSym == null)
			{
				cp.Error = RevitCellErrorCode.INVALID_ANNO_SYM_CS001120;
				return cp;
			}

			cp.AnnoSymbol = annoSym;

			IList<Parameter> parameters = annoSym.GetOrderedParameters();

			int totalParams = parameters.Count;
			int paramCount = 0;
			int labelCount = 0;

			foreach (Parameter param in parameters)
			{
				string name = param.Definition.Name;

				ParamDesc pd = RevitCellParameters.Match(name);

				if (pd == null)  continue;

				// bool result = cp.Add(pd, param);
				//
				// if (result)
				// {
				// 	cp.Error = RevitCellErrorCode.INVALID_DATA_FORMAT_CS000I10;
				// }
				//
				// if (pd.Group ==	ParamGroup.DATA)
				// {
				// 	paramCount++;
				// }
				// else
				// {
				// 	labelCount++;
				// }
			}

			if (paramCount == 0 || paramCount != ParamCounts[(int) ParamGroup.DATA] ||
				labelCount.Equals(0) || (labelCount % ParamCounts[(int) ParamGroup.LABEL]) != 0 )
			{
				cp.Error = RevitCellErrorCode.PARAM_MISSING_CS001101;
			}

			return cp;
		}

		private void GetCells(string[] paramNames, List<OrderedParams> paramList)
		{
			RevitCellParams cp;

			List<RevitCellParams> errorList = new List<RevitCellParams>();

			CellItemsBySeqName = new Dictionary<string, RevitCellParams>();
			CellItemsByNameSeq = new Dictionary<string, RevitCellParams>();

			foreach (OrderedParams orderedParams in paramList)
			{
				cp = catCell(paramNames, orderedParams);

				if (cp.HasError.Value)
				{
					errorList.Add(cp);
					continue;
				}

				string keySeqName = makeKey(cp, true);
				string keyNameSeq = makeKey(cp, false);

				CellItemsBySeqName.Add(keySeqName, cp);
				CellItemsByNameSeq.Add(keyNameSeq, cp);

			}
		}

		private RevitCellParams catCell(string[] parameters, OrderedParams op)
		{
			RevitCellParams cp = new RevitCellParams();
			ParamDesc pd;

			cp.AnnoSymbol = new AnnotationSymbol();

			int paramCount = 1;
			int textCount = 0;

			for (int i = 0; i < parameters.Length; i++)
			{
				string paramName = parameters[i];
				pd = Match(paramName);

				if (pd == null) continue;

				cp.CellParamDataType = pd.DataType;

				int idx = pd.Index;

				switch (pd.DataType)
				{
				case ParamDataType.TEXT:
					{
						RevitParamText rt = new RevitParamText(op.Value[i], pd);
						cp[idx] = rt;
						paramCount++;
						break;
					}

				// case ParamDataType.TEXT:
				// 	{
				// 		cp.AddText(op.Value[i], paramName, pd);
				// 		textCount++;
				// 		break;
				// 	}

				case ParamDataType.NUMBER:
					{
						double info;
						bool result = double.TryParse(op.Value[i], out info);
						if (!result)
						{
							cp[idx] = new RevitParamNumber(Double.NaN, pd);
							cp[idx].ErrorCode = RevitCellErrorCode.INVALID_DATA_FORMAT_CS000I10;
							continue;
						}

						RevitParamNumber rn = new RevitParamNumber(info, pd);
						cp[idx] = rn;
						paramCount++;
						break;
					}

				case ParamDataType.BOOL:
					{
						int info;
						bool result = int.TryParse(op.Value[i], out info);

						if (!result)
						{
							cp[idx] = new RevitParamBool(false, pd);
							cp[idx].ErrorCode = RevitCellErrorCode.INVALID_DATA_FORMAT_CS000I10;
							continue;
						}

						RevitParamBool rb = new RevitParamBool(info == 1, pd);
						cp[idx] = rb;
						paramCount++;
						break;
					}
				}
			}
			return cp;
		}

		private void listCellItems(string[] paramNames)
		{
			Console.WriteLine("\nbegin listing");

			ParamDesc pd;

			foreach (KeyValuePair<string, RevitCellParams> kvp in CellItemsBySeqName)
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

					if (pd.Group != ParamGroup.LABEL)
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

					// 	foreach (RevitTextData text2 in ((RevitParamText) kvp.Value[idx]).TextValues())
					// 	{
					// 		Console.Write("read text2| "
					// 			+ "   val| " + text2.GetValue()
					// 			+ "   key| " + text2.GetKey()
					// 			+ "   row| " + text2.Row 
					// 			+ "   col| " + text2.Col
					// 			+ "   pos| " + text2.Position
					// 			+ "   seq| " + text2.Seq
					// 			+ " valid| " + text2.IsValid
					// 			+ " pname| " + text2.ParamDesc.ParameterName
					// 			);
					// 		foreach (RevitCellErrorCode errCode in text2.Errors())
					// 		{
					// 			Console.Write(
					// 				" error| " + errCode.ToString());
					// 		}
					//
					// 		Console.Write("\n");
					//
					// 		Console.WriteLine("  new text| " + text2.NewValue);
					// 		
					// 		Console.Write("\n");
					// 	}
					}
				}
			}
		}

		private string makeKey(RevitCellParams cp, bool asSeqName)
		{
			string seq = ((string) cp[SeqIdx].GetValue());
			seq = seq.IsVoid() ? "ZZZZZZ" : seq;
			seq = $"{seq,-8}";

			string name = cp.Name.IsVoid() ? "Un-named" : cp.Name;

			string eid = cp.AnnoSymbol.ElementId < 0 ? "null element" : cp.AnnoSymbol.ToString();

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

*/
