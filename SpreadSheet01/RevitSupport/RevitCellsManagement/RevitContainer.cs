#region + Using Directives

// using SpreadSheet01.RevitSupport.RevitChartInfo;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitChartInfo;
using SpreadSheet01.RevitSupport.RevitParamInfo;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.RevitSupport.RevitParamInfo.RevitCellParameters;
// using static SpreadSheet01.RevitSupport.RevitChartInfo.RevitChartParameters;

#endregion

// user name: jeffs
// created:   2/25/2021 6:52:29 PM
// spreadsheet01

namespace SpreadSheet01.RevitSupport.RevitCellsManagement
{
	/*
		chart symbols {RevitCharts}  [Containers]  dictionary<string, chart symbol> 
		+-> chart symbol  {RevitChart}  [Containers] dictionary<string, cell family> 
			+-> cell family  {RevitCellsym}  [Container] ARevitParam[]
				+-> parameters
	*/


#region Containers

	public class RevitContainers<T> : ARevitParam, INotifyPropertyChanged
	{
		public Dictionary<string, T> Containers { get; set; }

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

	public class RevitAnnoSyms : RevitContainers<RevitCellSym>
	{
		public override string ToString()
		{
			return "I am RevitAnnoSyms| " + GetValue();
		}
	}

	public class RevitLabels : RevitContainers<RevitLabel>
	{
		public override string ToString()
		{
			return "I am RevitLabels| " + GetValue();
		}
	}

	// root with a collection of Chart Families
	public class RevitCharts : RevitContainers<RevitChart>
	{
		public override string ToString()
		{
			return "I am RevitCharts| " + GetValue();
		}
	}

	// one chart family with a collection of cell families
	public class RevitChart : RevitContainers<RevitCellSym>
	{
		// parameter information for the chart symbol
		public RevitChartSym RvtChartSym { get; set; }

		public override string ToString()
		{
			return "I am RevitChart| " + GetValue();
		}
	}

#endregion


#region Container

	public abstract class RevitContainer: ARevitParam, INotifyPropertyChanged
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

	public class RevitLabel : RevitContainer
	{

		public RevitLabel()
		{
			RevitParamList = new ARevitParam[ParamCounts[(int) ParamGroup.LABEL]];
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

		ARevitParam this[int idx] {get; set; }
	}

	public class RevitChartSym : RevitContainer, IAnnoSymContainer
	{
		public override dynamic GetValue()
		{
			return null;
		}

		public RevitChartSym()
		{
			RevitParamList = new ARevitParam[RevitChartParameters.ChartParamCounts[(int) ParamGroup.DATA]];
		}

		public AnnotationSymbol AnnoSymbol { get; set; }

		public AnnotationSymbol ChartSymbol => AnnoSymbol;

		public Element RvtElement {get; set; }

		public Element ChartElement => RvtElement;

		public ARevitParam[] ChartParameters => RevitParamList;

		public override string ToString()
		{
			return "I am RevitChart| " + AnnoSymbol.Name;
		}
	}
	
	public class RevitCellSym : RevitContainer, IAnnoSymContainer
	{
		public override dynamic GetValue()
		{
			return null;
		}
		
		public RevitCellSym()
		{
			RevitParamList = new ARevitParam[ParamCounts[(int) ParamGroup.DATA]];

			RevitParamList[LabelsIdx] = new RevitLabels();
		}

		public ARevitParam[] CellParameters => RevitParamList;

		public AnnotationSymbol AnnoSymbol { get; set; }

		public AnnotationSymbol CellSymbol => AnnoSymbol;

		public Element RvtElement {get; set; }

		public Element CellElement => RvtElement;

		public override string ToString()
		{
			return "I am RevitCell| " + this[RevitCellParameters.NameIdx];
		}
	}

#endregion
}
