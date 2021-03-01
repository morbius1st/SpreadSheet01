#region + Using Directives

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using UtilityLibrary;
using static SpreadSheet01.RevitSupport.RevitCellParameters;

#endregion

// user name: jeffs
// created:   2/25/2021 6:52:29 PM
// tests

namespace SpreadSheet01.RevitSupport.RevitParamValue
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



	public class RevitContainerTest
	{
		private RevitAnnoSyms annoSyms = new RevitAnnoSyms();

		private ObservableCollection<RevitAnnoSyms> sx = new ObservableCollection<RevitAnnoSyms>();

		public void test2()
		{
			ARevitParam px = new RevitParamAddr("asdf", null);
			
			RevitAnnoSym rx = new RevitAnnoSym();
			rx.Add(1, px);
			
			RevitAnnoSyms ras = new RevitAnnoSyms();
			ras.Add("asdf", rx);

			RevitAnnoSym sym = new RevitAnnoSym();
			
			string key = RevitValueSupport.MakeAnnoSymKey(sym, false);
			
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

	public class RevitContainers<T> : ARevitParam, INotifyPropertyChanged
	{

		public ObservableDictionary<string, T> Containers { get; set; }

		// public List<T>  ContainerList { get; private set; }  
		// 	= new List<T>();

		public RevitContainers()
		{
			Containers = new ObservableDictionary<string, T>();
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

	public class RevitLabels : RevitContainers<RevitLabel>
	{

		public override string ToString()
		{
			return "I am RevitLabels| ";
		}
	}



	public class RevitContainer : INotifyPropertyChanged
	{
		// public Dictionary<string, T>  ContentList { get; private set; }  =
		// 	new Dictionary<string, T>();

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

}
