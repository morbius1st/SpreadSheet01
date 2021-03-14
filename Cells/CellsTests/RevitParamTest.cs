using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using Cells.Windows;
using SpreadSheet01.RevitSupport;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitParamValue;
using SpreadSheet01.RevitSupport.RevitParamInfo;
using static SpreadSheet01.RevitSupport.RevitParamValue.ParamType;
using static SpreadSheet01.RevitSupport.RevitCellsManagement.RevitParamManager;


// Solution:     SpreadSheet01
// Project:       Tests
// File:             RevitParamTest.cs
// Created:      2021-02-26 (10:02 PM)


namespace Cells.CellsTests
{
	public class RevitParamTest : INotifyPropertyChanged
	{
		private SampleAnnoSymbols aSyms;

		// public RevitAnnoSyms AnnoSyms { get; private set; } = new RevitAnnoSyms();

		public RevitCatagorizeParam catParam;

		public void Process()
		{
			// chart params created
			ChartFamily a = ChartParams;

			// test - get the name
			ParamDesc pd = ChartParams[INSTANCE, NameIdx];
			string name = pd.ParameterName;

			// cell params created
			CellFamily b = CellParams;

			// test - get the name
			pd = CellParams[INSTANCE, NameIdx];

			// get the parameter name
			name = pd.ParameterName;

			// list the parameters
			listParams();


		}

		// public void Process2()
		// {
		// 	aSyms = new SampleAnnoSymbols();
		// 	aSyms.Process();
		//
		// 	MainWindow.WriteLineTab("start process symbols");
		//
		// 	listSymbols(aSyms.Symbols);
		//
		// 	foreach (AnnotationSymbol annoSym in aSyms.Symbols)
		// 	{
		// 		MainWindow.WriteLineTab("\n");
		// 		MainWindow.WriteLineTab("process symbol| " + annoSym.Name);
		//
		// 		RevitCellSym rvtCellSym = catParam.catagorizeAnnoSymParams(annoSym);
		// 		
		// 		rvtCellSym.AnnoSymbol = annoSym;
		//
		// 		string key = RevitParamUtil.MakeAnnoSymKey(rvtCellSym, 
		// 			(int) RevitParamManager.NameIdx,  (int) RevitParamManager.SeqIdx, false);
		//
		// 		MainWindow.WriteLineTab("   adding key| " + key);
		//
		// 		AnnoSyms.Add(key, rvtCellSym);
		// 	}
		//
		// 	OnPropertyChanged("AnnoSyms");
		// 	// AnnoSyms.UpdateProperties();
		// 	
		// 	// foreach (KeyValuePair<string, RevitCellSym> kvp in AnnoSyms.Containers)
		// 	// {
		// 	// 	foreach (ARevitParam param in kvp.Value.RevitParamList)
		// 	// 	{
		// 	// 		if (param != null)
		// 	// 		{
		// 	// 			param.UpdateProperties();
		// 	// 		}
		// 	// 	}
		// 	//
		// 	// 	kvp.Value.UpdateProperties();
		// 	// }
		//
		// 	// RevitAnnoSyms annoSymsEnd = AnnoSyms;
		//
		// 	MainWindow.WriteLineTab("process symbols complete");
		// 	MainWindow.WriteLineTab("\n");
		// }

//
// 		private RevitCellSym catagorizePAnnoSymParams(AnnotationSymbol aSym)
// 		{
// 			RevitCellSym ras = new RevitCellSym();
// 			ARevitParam rvtParam;
//
// 			int dataParamCount = 0;
// 			int labelParamCount = 0;
// 			int containerParamCount = 0;
//
// 			int labelId;
// 			bool isLabel;
// 			ParamDesc2 pd;
//
// 			foreach (Parameter param in aSym.GetOrderedParameters())
// 			{
// 				if (param.Definition.Type == ParamDataType.ERROR) break;
// 				if (param.Definition.Type == ParamDataType.EMPTY) continue;
//
// 				string paramName = RevitParamUtil.GetParamName(param.Definition.Name, out labelId, out isLabel);
//
// 				pd = RevitCellParameters.Match(paramName);
//
// 				switch (pd.Group)
// 				{
// 				case ParamGroup.DATA:
// 					{
// 						
// 						dataParamCount++;
// 						if (pd.DataType == ParamDataType.IGNORE) continue;
//
// 						rvtParam = catagorizeParameter(param, pd);
//
// 						ras.Add(pd.Index, rvtParam);
// 						break;
// 					}
// 				case ParamGroup.CONTAINER:
// 					{
// 						MainWindow.WriteLine("got container");
// 						containerParamCount++;
// 						if (pd.DataType == ParamDataType.IGNORE) continue;
// // todo: fix
// 						// RevitLabels labels = (RevitLabels) ras[LabelsIdx];
//
// 						// RevitLabel label = getLabel(labelId, labels);
// 						//
// 						// ARevitParam labelParam = catagorizeParameter(param, pd);
// 						//
// 						// label.Add(pd.Index, labelParam);
//
// 						// saveLabelParam(labelId, param, pd, labels);
//
// 						break;
// 					}
// 				case ParamGroup.LABEL_GRP:
// 					{
// 						labelParamCount++;
//
// 						if (labelId < 0 || pd.DataType == ParamDataType.IGNORE) continue;
// // todo: fix
// 						// RevitLabels labels = (RevitLabels) ras[LabelsIdx];
//
// 						// RevitLabel label = getLabel(labelId, labels);
// 						//
// 						// ARevitParam labelParam = catagorizeParameter(param, pd);
// 						//
// 						// label.Add(pd.Index, labelParam);
//
// 						// saveLabelParam(labelId, param, pd, labels);
//
// 						break;
// 					}
// 				}
// 			}
//
// 			return ras;
// 		}

		// private void saveLabelParam(int labelId, Parameter param, ParamDesc2 pd, RevitLabels labels)
		// {
		// 	RevitLabel label = getLabel(labelId, labels);
		//
		// 	ARevitParam labelParam = catagorizeParameter(param, pd);
		//
		// 	label.Add(pd.Index, labelParam);
		// }
		//
		// private RevitLabel getLabel(int idx, RevitLabels labels)
		// {
		// 	RevitLabel label = null;
		//
		// 	string key = RevitParamUtil.MakeLabelKey(idx);
		//
		// 	bool result = labels.Containers.TryGetValue(key, out label);
		//
		// 	if (!result)
		// 	{
		// 		label = new RevitLabel();
		// 		labels.Containers.Add(key, label);
		// 	}
		//
		// 	return label;
		// }

		// private ARevitParam catagorizeParameter(Parameter param, ParamDesc2 pd, string name = "")
		// {
		// 	ARevitParam p = null;
		//
		// 	switch (pd.DataType)
		// 	{
		// 	case ParamDataType.BOOL:
		// 		{
		// 			p = new RevitParamBool(
		// 				pd.ReadReqmt ==
		// 				ParamReadReqmt.READ_VALUE_IGNORE
		// 					? (bool?) false
		// 					: param.AsInteger() == 1, pd);
		// 			break;
		// 		}
		// 	case ParamDataType.NUMBER:
		// 		{
		// 			p = new RevitParamNumber(
		// 				pd.ReadReqmt ==
		// 				ParamReadReqmt.READ_VALUE_IGNORE
		// 					? double.NaN
		// 					: param.AsDouble(), pd);
		// 			break;
		// 		}
		// 	case ParamDataType.TEXT:
		// 		{
		// 			p = new RevitParamText(pd.ReadReqmt ==
		// 				ParamReadReqmt.READ_VALUE_IGNORE
		// 					? ""
		// 					: param.AsString(), pd);
		// 			break;
		// 		}
		// 	case ParamDataType.DATATYPE:
		// 		{
		// 			p = new RevitParamText(pd.ReadReqmt ==
		// 				ParamReadReqmt.READ_VALUE_IGNORE
		// 					? ""
		// 					: param.AsString(), pd);
		// 			break;
		// 		}
		// 	case ParamDataType.ADDRESS:
		// 		{
		// 			p = new RevitParamText(pd.ReadReqmt ==
		// 				ParamReadReqmt.READ_VALUE_IGNORE
		// 					? ""
		// 					: param.AsString(), pd);
		// 			break;
		// 		}
		// 	case ParamDataType.RELATIVEADDRESS:
		// 		{
		// 			p = new RevitParamText(pd.ReadReqmt ==
		// 				ParamReadReqmt.READ_VALUE_IGNORE
		// 					? ""
		// 					: param.AsString(), pd);
		// 			break;
		// 		}
		// 	}
		//
		// 	return p;
		// }


		private void listParams()
		{
			MainWindow.WriteLineTab("\nList param descriptions");

			MainWindow.WriteLineTab("\nList chart param descriptions");
			MainWindow.WriteTab("\n");

			listParamHeader(ChartParams);
			MainWindow.WriteTab("\n");
			
			foreach (ParamDesc pd in ChartParams.InstanceParams)
			{
				listAParam(pd);

			}
			
			MainWindow.WriteLineTab("\nList cell param descriptions");
			MainWindow.WriteTab("\n");

			listParamHeader(CellParams);
			MainWindow.WriteTab("\n");

			foreach (ParamDesc pd in CellParams.InstanceParams)
			{
				listAParam(pd);
			}
			MainWindow.WriteTab("\n");

			foreach (ParamDesc pd in CellParams.LabelParams)
			{
				listAParam(pd);
			}
		}

		private void listParamHeader(Family f)
		{
			MainWindow.WriteLineTab("fam name| " + f.FamilyName);
			MainWindow.WriteLineTab("cat     | "      + f.Category);
			MainWindow.WriteLineTab("subcat  | "   + f.SubCategory);
			MainWindow.WriteLineTab("type    | "     + f.GetType());
		}

		private void listAParam(ParamDesc pd)
		{
			MainWindow.WriteTab("name| " + pd.ParameterName.PadRight(32));
			MainWindow.WriteTab("Type| " + pd.DataType.ToString().PadRight(12));
			MainWindow.WriteTab("Mode| " + pd.Mode.ToString().PadRight(10));
			MainWindow.WriteTab("\n");
		}



		private void listSymbols(AnnotationSymbol[] annoSyms)
		{
			MainWindow.WriteLineTab("\nList symbols");

			foreach (AnnotationSymbol symbol in annoSyms)
			{
				MainWindow.WriteLineTab("\nsymbols| " + symbol.Name);
				MainWindow.WriteLineTab("parameters| count| " + symbol.parameters.Count);

				for (var i = 0; i < symbol.parameters.Count; i++)
				{
					MainWindow.WriteTab("   type| ");
					MainWindow.WriteTab(symbol.parameters[i].Definition.Type.ToString());

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


					MainWindow.WriteTab("   val| >");
					MainWindow.WriteTab(result);

					MainWindow.WriteTab("<   name| ");
					MainWindow.WriteLineTab(symbol.parameters[i].Definition.Name);
				}

				MainWindow.WriteLineTab("\nComplete\n");
			}
		}

		// private void listSymbols(RevitAnnoSyms annoSyms)
		// {
		// 	MainWindow.WriteLineTab("\nList symbols");
		//
		//
		// 	foreach (KeyValuePair<string, RevitCellSym> kvp in annoSyms.ListOfRevitCellSym)
		// 	{
		// 		RevitCellSym symbol = kvp.Value;
		//
		// 		MainWindow.WriteLineTab("\nsymbols| " + symbol.AnnoSymbol.Name + "  (" + kvp.Key + ")");
		// 		MainWindow.WriteLineTab("parameters| count| " + symbol.RevitParamList.Length);
		//
		// 		foreach (ARevitParam param in symbol.RevitParamList)
		// 		{
		// 			MainWindow.WriteTab("   ");
		// 			MainWindow.WriteTab(param.ParamDesc.Index.ToString("###"));
		// 			MainWindow.WriteTab("  val| ");
		// 			MainWindow.WriteTab(param.GetValue());
		// 			MainWindow.WriteTab("  name| ");
		// 			MainWindow.WriteTab(param.ParamDesc.ParameterName);
		// 		}
		//
		// 		MainWindow.WriteLineTab("\nComplete\n");
		// 	}
		// }

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}
	}
}