// Solution:     SpreadSheet01
// Project:       Tests
// File:             RevitParamTest.cs
// Created:      2021-02-26 (10:02 PM)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using Cells.Windows;
using SpreadSheet01.RevitSupport;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.RevitSupport.RevitCellParameters;

namespace Cells.CellsTests
{
	public class RevitParamTest : INotifyPropertyChanged
	{
		private SampleAnnoSymbols aSyms;

		public RevitAnnoSyms AnnoSyms { get; private set; } = new RevitAnnoSyms();

		public void Process()
		{
			aSyms = new SampleAnnoSymbols();
			aSyms.Process();

			MainWindow.WriteLine("start process symbols");

			listSymbols(aSyms.Symbols);

			foreach (AnnotationSymbol annoSym in aSyms.Symbols)
			{
				MainWindow.WriteLine("\n");
				MainWindow.WriteLine("process symbol| " + annoSym.Name);

				RevitAnnoSym rvtAnnoSym = catagorizePAnnoSymParams(annoSym);
				
				rvtAnnoSym.AnnoSymbol = annoSym;

				string key = RevitValueSupport.MakeAnnoSymKey(rvtAnnoSym, false);

				MainWindow.WriteLine("   adding key| " + key);

				AnnoSyms.Add(key, rvtAnnoSym);
			}

			OnPropertyChanged("AnnoSyms");
			AnnoSyms.UpdateProperties();
			
			foreach (KeyValuePair<string, RevitAnnoSym> kvp in AnnoSyms.Containers)
			{
				foreach (ARevitParam param in kvp.Value.RevitParamList)
				{
					if (param != null)
					{
						param.UpdateProperties();
					}
				}

				kvp.Value.UpdateProperties();
			}

			RevitAnnoSyms annoSymsEnd = AnnoSyms;

			MainWindow.WriteLine("process symbols complete");
			MainWindow.WriteLine("\n");
		}


		private RevitAnnoSym catagorizePAnnoSymParams(AnnotationSymbol aSym)
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

				string paramName = RevitValueSupport.GetParamName(param.Definition.Name, out labelId, out isLabel);

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
						MainWindow.WriteLine("got container");
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

			string key = RevitValueSupport.MakeLabelKey(idx);

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
							: param.AsNumber(), pd);
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
			MainWindow.WriteLine("\nList symbols");

			foreach (AnnotationSymbol symbol in annoSyms)
			{
				MainWindow.WriteLine("\nsymbols| " + symbol.Name);
				MainWindow.WriteLine("parameters| count| " + symbol.parameters.Count);

				for (var i = 0; i < symbol.parameters.Count; i++)
				{
					MainWindow.Write("   type| ");
					MainWindow.Write(symbol.parameters[i].Definition.Type.ToString());

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


					MainWindow.Write("   val| >");
					MainWindow.Write(result);

					MainWindow.Write("<   name| ");
					MainWindow.WriteLine(symbol.parameters[i].Definition.Name);
				}

				MainWindow.WriteLine("\nComplete\n");
			}
		}


		private void listSymbols(RevitAnnoSyms annoSyms)
		{
			MainWindow.WriteLine("\nList symbols");


			foreach (KeyValuePair<string, RevitAnnoSym> kvp in annoSyms.Containers)
			{
				RevitAnnoSym symbol = kvp.Value;

				MainWindow.WriteLine("\nsymbols| " + symbol.AnnoSymbol.Name + "  (" + kvp.Key + ")");
				MainWindow.WriteLine("parameters| count| " + symbol.RevitParamList.Length);

				foreach (ARevitParam param in symbol.RevitParamList)
				{
					MainWindow.Write("   ");
					MainWindow.Write(param.ParamDesc.Index.ToString("###"));
					MainWindow.Write("  val| ");
					MainWindow.Write(param.GetValue());
					MainWindow.Write("  name| ");
					MainWindow.Write(param.ParamDesc.ParameterName);
				}

				MainWindow.WriteLine("\nComplete\n");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}