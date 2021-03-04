#region + Using Directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitChartInfo;
using SpreadSheet01.RevitSupport.RevitParamInfo;
using UtilityLibrary;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.RevitSupport.RevitParamInfo.RevitCellParameters;
using static SpreadSheet01.RevitSupport.RevitChartInfo.RevitChartParameters;

#endregion

// user name: jeffs
// created:   2/25/2021 6:52:29 PM
// spreadsheet01

namespace SpreadSheet01.RevitSupport
{
	/*
	 the primary collection of the parameters for all of the annotation symbols of a type
	AnnoSymbols
	+--> dictionary<string, RevitCellParams> {RevitContainer} :: ARevitParam
		+-> RevitCellParams
			+-> ARevitParam[] {CellItems}
				+-> RevitParamText (string / text) [ename to RevitParamText]
				+-> RevitParamNumber (double / number)
				+-> etc.
				+-> RevitValueCGroup (has a collection)
					+-> ARevitParam[] {AnnoSymParameters}


	objects:
	1.	dictionary<string, AnnoSymbol>	AnnoSymbols
		> is the collection of all anno symbols of associated with a single chart anno symbol
		> that is, a collection of cellItems [rename to AnnoSym]
		> key is sequence as 000000
	2. AnnoSym
		> is the collection of all of the parameters associated with a single anno symbol
		> except that, value parameters go into a single parameter that has a collection of these parameters
		> collection is: ARevitParam[] called cellItems [rename to AnnoSymParameters]
		> key is index as 00


	notes:
	1. adjust chart anno symbol to use multiple anno symbols
	*/

	public class RevitContainers<T> : ARevitParam, INotifyPropertyChanged
	{
		public Dictionary<string, T> Containers { get; private set; }

		public RevitContainers()
		{
			Containers = new Dictionary<string, T>();
		}

		public override dynamic GetValue() => null;

		public void Add(string key, T container) 
		{
			
			Containers.Add(key, container);
			OnPropertyChanged(nameof(Containers));
		}

		public void UpdateProperties()
		{
			OnPropertyChanged(nameof(Containers));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return "I am RevitContainers<T>| " + typeof(T).Name;
		}
	}

	public class RevitAnnoSyms : RevitContainers<RevitAnnoSym>
	{
		public override string ToString()
		{
			return "I am RevitAnnoSyms| ";
		}
	}

	public class RevitCharts : RevitContainers<RevitChartSym>
	{
		// a collection of Chart Families
		public override string ToString()
		{
			return "I am RevitCharts| ";
		}
	}

	public class RevitLabels : RevitContainers<RevitLabel>
	{
		public override string ToString()
		{
			return "I am RevitLabels| ";
		}
	}

	public class RevitContainer: INotifyPropertyChanged
	{
		public ARevitParam[] RevitParamList { get; set; }

		public void Add(int idx, ARevitParam content)
		{
			RevitParamList[idx] = content;
			OnPropertyChanged(nameof(RevitParamList));
		}

		public ARevitParam this[int idx]
		{
			get => RevitParamList[idx];
			set
			{
				RevitParamList[idx] = value;
				OnPropertyChanged(nameof(RevitParamList));
			}
		}

		public void UpdateProperties()
		{
			OnPropertyChanged(nameof(RevitParamList));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return "I am RevitContainer|";
		}
	}

	public class RevitAnnoSym : RevitContainer
	{
		public RevitAnnoSym()
		{
			RevitParamList = new ARevitParam[RevitCellParameters.ParamCounts[(int) ParamGroup.DATA]];

			RevitParamList[LabelsIdx] = new RevitLabels();
		}

		public AnnotationSymbol AnnoSymbol { get; set; }

		public Element RvtElement {get; set; }

		public override string ToString()
		{
			return "I am RevitAnnoSym| ";
		}
	}

	public class RevitLabel : RevitContainer
	{
		public RevitLabel()
		{
			RevitParamList = new ARevitParam[RevitCellParameters.ParamCounts[(int) ParamGroup.LABEL]];
		}

		public override string ToString()
		{
			return "I am RevitLabel| ";
		}
	}

	public class RevitChartSym : RevitContainer
	{
		public RevitChartSym()
		{
			RevitParamList = new ARevitParam[RevitChartParameters.ChartParamCounts[(int) ParamGroup.DATA]];
		}

		public AnnotationSymbol AnnoSymbol { get; set; }

		public Element RvtElement {get; set; }

		public override string ToString()
		{
			return "I am RevitChart| ";
		}
	}


	/*
	public class RevitContainerTest
	{
		private RevitAnnoSyms annoSyms = new RevitAnnoSyms();

		public void test2()
		{
			RevitAnnoSym sym = new RevitAnnoSym();

			string key = RevitParamUtil.MakeAnnoSymKey(sym, false);

			annoSyms.Add(key, sym);
		}


		private RevitAnnoSym makeSym()
		{
			RevitAnnoSym sym = null;

			// sym.ContentList.Add("asf", null);


			return sym;
		}

		private void addParams(RevitAnnoSym sym)
		{

			ParamDesc pd = RevitCellParameters.Match("Name");


			RevitParamBool rb = new RevitParamBool(true, pd);
		}
	}
	*/
}
