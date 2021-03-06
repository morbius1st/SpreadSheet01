﻿#region + Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitParamManagement;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.RevitSupport.RevitParamManagement.RevitParamManager;

#endregion

// user name: jeffs
// created:   2/25/2021 6:52:29 PM
// spreadsheet01

namespace SpreadSheet01.RevitSupport.RevitCellsManagement
{

#region Containers

	public abstract class RevitContainers<TU, TV> : /* ARevitParam, */ INotifyPropertyChanged
	{
		protected Dictionary<TU, TV> Containers { get; set; }

		public RevitContainers()
		{
			Containers = new Dictionary<TU, TV>();
		}

		// public override dynamic GetValue() => null;

		protected void Add(TU key, TV container)
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
			return "I am RevitContainers<TU,TV>| " + typeof(TU).Name + ">|<" + typeof(TU).Name ;
		}
	}

	public abstract class RevitContainer<T> :ARevitParam, INotifyPropertyChanged
	{
		public T[] RevitParamList { get; set; }

		public void Add(int idx, T content)
		{
			RevitParamList[idx] = content;
			OnPropertyChanged(nameof(RevitParamList));
		}

		public T this[int idx]
		{
			get => RevitParamList[idx];
			set
			{
				RevitParamList[idx] = value;
				OnPropertyChanged(nameof(RevitParamList));
			}
		}

		public abstract override dynamic GetValue();

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
			return "I am RevitContainer| " + this[NameIdx];
		}
	}

	// root with a collection of Chart Families
	public class RevitCharts : RevitContainers<string, RevitChart>
	{
		public Dictionary<string, RevitChart> ListOfCharts => Containers;

		public new void Add(string key, RevitChart container)
		{
			base.Add(key, container);
		}

		public override string ToString()
		{
			return "I am RevitCharts| " /*+ GetValue() ?? "no value"*/;
		}
	}

	// one chart family with a collection of cell families
	public class RevitChart : RevitContainers<string, RevitCell>
	{
		// key is sequence number first then name in the format of
		// 《    #》{name}
		public Dictionary<string, RevitCell> ListOfCellSyms {
			get => Containers;
			set => Containers = value;
		}
		public SortedDictionary<string, RevitLabel> AllCellLabels { get; private set; }
			= new SortedDictionary<string, RevitLabel>();

		public void Add(RevitCell cell)
		{
			base.Add(makeCellSeqNameKey(cell), cell);

			addCellLabels(cell);
		}

		// parameter information for the chart symbol
		public RevitChartData RevitChartData { get; set; }

		public string Name => this[NameIdx].GetValue();
		public string Sequence => this[SeqIdx].GetValue();
		public string FilePath => ((RevitParamFilePath) this[ChartFilePathIdx]).FullFilePath;
		public bool Exists => ((RevitParamFilePath) this[ChartFilePathIdx]).Exists;
		public string WorkSheet => this[ChartWorkSheetIdx].GetValue();
		public CellUpdateTypeCode UpdateType => RevitChartData.UpdateType;

		public ARevitParam this[int idx] =>RevitChartData.RevitParamList[idx];

		public string makeCellSeqNameKey(RevitCell cell)
		{
			return RevitParamUtil.MakeSeqNameKey(cell.Name, cell.Sequence);
		}
		
		private void addCellLabels(RevitCell cell)
		{
			foreach (KeyValuePair<string, RevitLabel> kvp in cell.CellLabels)
			{
				AllCellLabels.Add(kvp.Key, kvp.Value);
			}
		}

		public override string ToString()
		{
			return "I am RevitChart| " /*+ GetValue() ?? "no value"*/;
		}
	}

	public class RevitChartData : RevitContainer<ARevitParam>, IAnnoSymContainer
	{
		public override dynamic GetValue()
		{
			return null;
		}

		public RevitChartData()
		{
			RevitParamList = new ARevitParam[AllChartParamCount];
		}

		public string Name => this[NameIdx].GetValue();
		public string Sequence => this[SeqIdx].GetValue();
		public CellUpdateTypeCode UpdateType => ((RevitParamUpdateType) this[ChartUpdateTypeIdx]).UpdateType;


		public AnnotationSymbol AnnoSymbol { get; set; }

		public Element RevitElement { get; set; }

		// public ARevitParam[] ChartParameters => RevitParamList;

		public override string ToString()
		{
			return "I am " + nameof(RevitChartData) + "| " + AnnoSymbol.Name;
		}
	}

#endregion


#region Container


	public interface IAnnoSymContainer
	{
		string Name { get; }

		AnnotationSymbol AnnoSymbol { get; set; }

		ARevitParam[] RevitParamList { get; set; }

		ARevitParam this[int idx] { get; set; }
	}
	
	public class RevitCell : RevitContainer<ARevitParam>, IAnnoSymContainer
	{
		public RevitCell()
		{
			RevitParamList = new ARevitParam[CellBasicParamCount];
		}

		public Dictionary<string, RevitLabel> ListOfLabels { get; set; } 
			= new Dictionary<string, RevitLabel>();

		// key is OK
		public SortedDictionary<string, RevitLabel>  CellLabels 
			= new SortedDictionary<string, RevitLabel>();

		public AnnotationSymbol AnnoSymbol { get; set; }
		
		public Element RevitElement { get; set; }

		// public ARevitParam this[int idx] => RevitParamList[idx];

		public string Name => this[NameIdx].GetValue();
		public string Sequence => this[SeqIdx].GetValue();

		// public RevitLabels LabelList { get; private set; }
		// public AnnotationSymbol CellSymbol => AnnoSymbol;
		// public Element CellElement => RvtElement;
		// public ARevitParam[] CellParameters => RevitParamList;

		public void AddLabelRef(RevitLabel label)
		{
			CellLabels.Add(label.Name, label);
		}

		public override dynamic GetValue()
		{
			return null;
		}

		public override string ToString()
		{
			return "I am RevitCell| " + this[NameIdx] ?? "null name";
		}
	}

	public class RevitLabel : RevitContainer<ARevitParam>
	{
		// a single label 
		public string Name => this[LblNameIdx].GetValue();
		public string Formula => this[LblFormulaIdx].GetValue();

		public RevitLabel()
		{
			RevitParamList = new ARevitParam[CellLabelParamCount];
		}

		public override dynamic GetValue()
		{
			return null;
		}

		public override string ToString()
		{
			return "I am RevitLabel| " + Name;
		}
	}

#endregion

}