#region + Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitChartInfo;
using SpreadSheet01.RevitSupport.RevitParamInfo;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.RevitSupport.RevitCellsManagement.RevitParamManager;

#endregion

// user name: jeffs
// created:   2/25/2021 6:52:29 PM
// spreadsheet01

namespace SpreadSheet01.RevitSupport.RevitCellsManagement
{

#region Containers

	public abstract class RevitContainers<TU, TV> : ARevitParam, INotifyPropertyChanged
	{
		public Dictionary<TU, TV> Containers { get; set; }

		public RevitContainers()
		{
			Containers = new Dictionary<TU, TV>();
		}

		public override dynamic GetValue() => null;

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

	public class RevitAnnoSyms : RevitContainers<string, RevitCellSym>
	{

		public new void Add(string key, RevitCellSym container)
		{
			base.Add(key, container);
		}

		public override string ToString()
		{
			return "I am RevitAnnoSyms| " + GetValue();
		}
	}

	public class RevitLabels : RevitContainers<string, RevitLabel>
	{
		public new void Add(string key, RevitLabel container)
		{
			base.Add(key, container);
		}

		public override string ToString()
		{
			return "I am RevitLabels| " + GetValue();
		}
	}

	// root with a collection of Chart Families
	public class RevitCharts : RevitContainers<string, RevitChart>
	{
		public new void Add(string key, RevitChart container)
		{
			base.Add(key, container);
		}

		public override string ToString()
		{
			return "I am RevitCharts| " + GetValue() ?? "no value";
		}
	}

	// one chart family with a collection of cell families
	public class RevitChart : RevitContainers<string, RevitCellSym>
	{
		public new void Add(string key, RevitCellSym container)
		{
			base.Add(key, container);
		}

		// parameter information for the chart symbol
		public RevitChartSym RvtChartSym { get; set; }

		public CellUpdateTypeCode UpdateType => RvtChartSym.UpdateType;

		public override string ToString()
		{
			return "I am RevitChart| " + GetValue() ?? "no value";
		}
	}

	// parameter system

#endregion


#region Container

	public abstract class RevitContainer<T> : ARevitParam, INotifyPropertyChanged
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

	
	public class RevitLabel : RevitContainer<ARevitParam>
	{
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
			return "I am RevitLabel| " + this[NameIdx];
		}
	}

	public interface IAnnoSymContainer
	{
		AnnotationSymbol AnnoSymbol { get; set; }

		ARevitParam[] RevitParamList { get; set; }

		ARevitParam this[int idx] { get; set; }
	}

	public class RevitChartSym : RevitContainer<ARevitParam>, IAnnoSymContainer
	{
		public override dynamic GetValue()
		{
			return null;
		}

		public RevitChartSym()
		{
			RevitParamList = new ARevitParam[AllChartParamCount];
		}

		public CellUpdateTypeCode UpdateType => ((RevitParamUpdateType) RevitParamList[ChartUpdateTypeIdx]).UpdateType;

		public AnnotationSymbol AnnoSymbol { get; set; }

		public Element RvtElement { get; set; }

		// public ARevitParam[] ChartParameters => RevitParamList;

		public override string ToString()
		{
			return "I am RevitChart| " + AnnoSymbol.Name;
		}
	}

	public class RevitCellSym : RevitContainer<ARevitParam>, IAnnoSymContainer
	{
		public override dynamic GetValue()
		{
			return null;
		}

		public RevitCellSym()
		{
			RevitParamList = new ARevitParam[CellBasicParamCount];

			LabelList = new RevitLabels();
		}

		public RevitLabels LabelList { get; private set; }

		public AnnotationSymbol AnnoSymbol { get; set; }
		
		public Element RvtElement { get; set; }

		// public AnnotationSymbol CellSymbol => AnnoSymbol;
		// public Element CellElement => RvtElement;
		// public ARevitParam[] CellParameters => RevitParamList;

		public override string ToString()
		{
			return "I am RevitCellSym| " + this[NameIdx] ?? "null name";
		}
	}


#endregion

}