// Solution:     SpreadSheet01
// Project:       Tests
// File:             RevitParamTest.cs
// Created:      2021-02-26 (10:02 PM)

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Configuration;
using System.Text.RegularExpressions;
using Autodesk.Revit.DB;
using Cells.CellsTests;
using SpreadSheet01.RevitSupport;
using SpreadSheet01.RevitSupport.RevitParamValue;
using SpreadSheet01.RevitSupport.RevitParamInfo;
using static SpreadSheet01.RevitSupport.RevitParamInfo.RevitCellParameters;

namespace Tests.CellsTests
{
	public class RevitParamTest
	{
		private SampleAnnoSymbols aSyms;

		private RevitAnnoSyms annoSyms = new RevitAnnoSyms();

		public void Process()
		{

			aSyms = new SampleAnnoSymbols();
			aSyms.Process();

			Console.WriteLine("start process symbols");

			listSymbols(aSyms.Symbols);

			foreach (AnnotationSymbol annoSym in aSyms.Symbols)
			{
				Console.WriteLine("\n");
				Console.WriteLine("process symbol| " + annoSym.Name);

				RevitAnnoSym rvtAnnoSym = catagorizePAnnoSymParams(annoSym, ParamClass.LABEL);

				rvtAnnoSym.AnnoSymbol = annoSym;

				string key = RevitParamUtil.MakeAnnoSymKey(rvtAnnoSym, 
					(int) RevitCellParameters.NameIdx, (int) RevitCellParameters.SeqIdx, false);

				Console.WriteLine("   adding key| " + key);

				annoSyms.Add(key, rvtAnnoSym);
			}

			RevitAnnoSyms annoSymsEnd = annoSyms;

			Console.WriteLine("process symbols complete");
			Console.WriteLine("\n");
		}


		private RevitAnnoSym catagorizePAnnoSymParams(AnnotationSymbol aSym, ParamClass paramClass)
		{
			RevitAnnoSym ras = new RevitAnnoSym();
			ARevitParam rvtParam;

			int dataParamCount = 0;
			int labelParamCount = 0;
			int containerParamCount = 0;

			int labelId;
			bool isLabel;
			ParamDesc pd;

			foreach (Parameter param in aSym.GetOrderedParameters())
			{
				if (param.Definition.Type == ParamDataType.ERROR) break;
				if (param.Definition.Type == ParamDataType.EMPTY) continue;

				string paramName = RevitParamUtil.GetParamName(param.Definition.Name, paramClass, out labelId, out isLabel);

				pd = RevitCellParameters.Match(paramName);

				switch (pd.Group)
				{
				case ParamGroup.DATA:
					{
						
						dataParamCount++;
						if (pd.DataType == ParamDataType.IGNORE) continue;

						rvtParam = catagorizeParameter(param, pd);

						ras.Add(pd.Index, rvtParam);
						break;
					}
				case ParamGroup.CONTAINER:
					{
						Debug.WriteLine("got container");
						containerParamCount++;
						if (pd.DataType == ParamDataType.IGNORE) continue;

						RevitLabels labels = (RevitLabels) ras[LabelsIdx];

						// RevitLabel label = getLabel(labelId, labels);
						//
						// ARevitParam labelParam = catagorizeParameter(param, pd);
						//
						// label.Add(pd.Index, labelParam);

						saveLabelParam(labelId, param, pd, labels);

						break;
					}
				case ParamGroup.LABEL:
					{
						labelParamCount++;

						if (labelId < 0 || pd.DataType == ParamDataType.IGNORE) continue;

						RevitLabels labels = (RevitLabels) ras[LabelsIdx];

						// RevitLabel label = getLabel(labelId, labels);
						//
						// ARevitParam labelParam = catagorizeParameter(param, pd);
						//
						// label.Add(pd.Index, labelParam);

						saveLabelParam(labelId, param, pd, labels);

						break;
					}
				}
			}

			return ras;
		}

		private void saveLabelParam(int labelId, Parameter param, ParamDesc pd, RevitLabels labels)
		{
			RevitLabel label = getLabel(labelId, labels);

			ARevitParam labelParam = catagorizeParameter(param, pd);

			label.Add(pd.Index, labelParam);
		}

		private RevitLabel getLabel(int idx, RevitLabels labels)
		{
			RevitLabel label = null;

			string key = RevitParamUtil.MakeLabelKey(idx);

			bool result = labels.Containers.TryGetValue(key, out label);

			if (!result)
			{
				label = new RevitLabel();
				labels.Containers.Add(key, label);
			}

			return label;
		}

		private ARevitParam catagorizeParameter(Parameter param, ParamDesc pd, string name = "")
		{
			ARevitParam p = null;

			switch (pd.DataType)
			{
			case ParamDataType.BOOL:
				{
					p = new RevitParamBool(
						pd.ReadReqmt ==
						ParamReadReqmt.READ_VALUE_IGNORE
							? (bool?) false
							: param.AsInteger() == 1, pd);
					break;
				}
			case ParamDataType.NUMBER:
				{
					p = new RevitParamNumber(
						pd.ReadReqmt ==
						ParamReadReqmt.READ_VALUE_IGNORE
							? double.NaN
							: param.AsDouble(), pd);
					break;
				}
			case ParamDataType.TEXT:
				{
					p = new RevitParamText(pd.ReadReqmt ==
						ParamReadReqmt.READ_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			case ParamDataType.DATATYPE:
				{
					p = new RevitParamText(pd.ReadReqmt ==
						ParamReadReqmt.READ_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			case ParamDataType.ADDRESS:
				{
					p = new RevitParamText(pd.ReadReqmt ==
						ParamReadReqmt.READ_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			case ParamDataType.RELATIVEADDRESS:
				{
					p = new RevitParamText(pd.ReadReqmt ==
						ParamReadReqmt.READ_VALUE_IGNORE
							? ""
							: param.AsString(), pd);
					break;
				}
			}

			return p;
		}

		private void listSymbols(AnnotationSymbol[] annoSyms)
		{
			Console.WriteLine("\nList symbols");

			foreach (AnnotationSymbol symbol in annoSyms)
			{
				Console.WriteLine("\nsymbols| " + symbol.Name);
				Console.WriteLine("parameters| count| " + symbol.parameters.Count);

				for (var i = 0; i < symbol.parameters.Count; i++)
				{
					Console.Write("   type| ");
					Console.Write(symbol.parameters[i].Definition.Type.ToString());

					string result = "unknown";

					switch (symbol.parameters[i].Definition.Type)
					{
					case ParamDataType.TEXT:
						{
							result = symbol.parameters[i].AsString();
							break;
						}
					case ParamDataType.BOOL:
						{
							result = (symbol.parameters[i].AsInteger() == 1).ToString();
							break;
						}
					case ParamDataType.IGNORE:
						{
							result = "ignore";
							break;
						}
					}


					Console.Write("   val| >");
					Console.Write(result);

					Console.Write("<   name| ");
					Console.WriteLine(symbol.parameters[i].Definition.Name);
				}

				Console.WriteLine("\nComplete\n");
			}
		}


		private void listSymbols(RevitAnnoSyms annoSyms)
		{
			Console.WriteLine("\nList symbols");


			foreach (KeyValuePair<string, RevitAnnoSym> kvp in annoSyms.Containers)
			{
				RevitAnnoSym symbol = kvp.Value;

				Console.WriteLine("\nsymbols| " + symbol.AnnoSymbol.Name + "  (" + kvp.Key + ")");
				Console.WriteLine("parameters| count| " + symbol.RevitParamList.Length);

				foreach (ARevitParam param in symbol.RevitParamList)
				{
					Console.Write("   ");
					Console.Write(param.ParamDesc.Index.ToString("###"));
					Console.Write("  val| ");
					Console.Write(param.GetValue());
					Console.Write("  name| ");
					Console.Write(param.ParamDesc.ParameterName);
				}

				Console.WriteLine("\nComplete\n");
			}
		}
	}
}