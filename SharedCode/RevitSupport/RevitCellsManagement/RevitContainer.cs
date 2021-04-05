#region + Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitParamManagement;
using SpreadSheet01.RevitSupport.RevitParamValue;
using UtilityLibrary;
using static SpreadSheet01.RevitSupport.RevitParamManagement.RevitParamManager;

using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamType;
#endregion

// user name: jeffs
// created:   2/25/2021 6:52:29 PM
// spreadsheet01

namespace SpreadSheet01.RevitSupport.RevitCellsManagement
{
#region Containers

	public abstract class RevitContainers<TV> : /* ARevitParam, */ INotifyPropertyChanged
	{
		protected RevitContainers()
		{
			Containers = new Dictionary<string, TV>();
		}

		protected List<ErrorCodes> errors = new List<ErrorCodes>();

		protected Dictionary<string, TV> Containers { get; set; }

		public List<ErrorCodes> ErrorCodeList
		{
			get => errors;
		}

		public ErrorCodes ErrorCode
		{
			set { errors.Add(value); }
		}

		public void ResetErrors() => errors = new List<ErrorCodes>();

		public bool HasErrors => errors.Count > 0;

		protected bool Add(string key, TV container)
		{
			int i = 1;
			string k = key;
			bool result1;
			bool result2 = true;

			do
			{
				try
				{
					Containers.Add(k, container);
					result1 = true;
				}
				catch
				{
					result1 = false;
					result2 = false;

					k = key + "." + i++.ToString("D2");
				}
			}
			while (!result1);

			OnPropertyChanged(nameof(Containers));

			return result2;
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
			return "I am RevitContainers<TV>| <" + typeof(TV).Name ;
		}
	}

	public abstract class RevitContainer<T> : ARevitParam, INotifyPropertyChanged
	{
		public RevitContainer()
		{
			RevitParamLists = new T[NumberOfLists][];
		}

		public int NumberOfLists { get; protected set; }

		public T[][] RevitParamLists { get; set; }

		public T[] this[ParamType type] 
		{
			get => RevitParamLists[ (int) type];
			set => RevitParamLists[ (int) type] = value;
		}

		public T this[ParamType type, int idx] => RevitParamLists[(int) type][idx];


		public void Add(ParamType type, int idx, T content)
		{
			RevitParamLists[(int) type][idx] = content;
		}




		// public T[] RevitParamListBasic { get; set; }

		// public T this[int idx]
		// {
		// 	get
		// 	{
		// 		if (RevitParamListBasic == null
		// 			|| RevitParamListBasic[idx] == null
		// 			) return default(T);
		//
		// 		return RevitParamListBasic[idx];
		// 	}
		// 	set
		// 	{
		// 		RevitParamListBasic[idx] = value;
		// 		OnPropertyChanged(nameof(RevitParamListBasic));
		// 	}
		// }


		public bool HasErrors => errors.Count > 0;

		public ErrorCodes ErrorCode
		{
			set { base.ErrorCode = value; }
		}

		public List<ErrorCodes> ErrorCodeList
		{
			get => base.ErrorCodeList;
		}




		// public void Add(int idx, T content)
		// {
		// 	RevitParamListBasic[idx] = content;
		// 	OnPropertyChanged(nameof(RevitParamListBasic));
		// }

		public abstract override dynamic GetValue();

		public void UpdateProperties()
		{
			OnPropertyChanged(nameof(RevitParamLists));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return "I am RevitContainer| " + this[ParamType.PT_INSTANCE, NameIdx];
		}
	}

	// root with a collection of Chart Families
	public class RevitCharts : RevitContainers<RevitChart>
	{
		public Dictionary<string, RevitChart> ListOfCharts => Containers;

		public bool Add(string key, RevitChart container)
		{
			return base.Add(key, container);
		}

		public override string ToString()
		{
			return "I am RevitCharts| " /*+ GetValue() ?? "no value"*/;
		}
	}

	// one chart family with a collection of cell families
	public class RevitChart : RevitContainers<RevitCellData>
	{
		// key is sequence number first then name in the format of
		// 《    #》{name}
		public RevitChart()
		{

			// ChartFamily = chartFamily;
			ListOfCellSyms = new Dictionary<string, RevitCellData>();
			AllCellLabels = new SortedDictionary<string, RevitLabel>();
		}

		public Dictionary<string, RevitCellData> ListOfCellSyms
		{
			get => Containers;
			set => Containers = value;
		}

		public SortedDictionary<string, RevitLabel> AllCellLabels { get; private set; }
			= new SortedDictionary<string, RevitLabel>();

		public ARevitParam this[int idx] => RevitChartData[PT_INSTANCE, idx];

		// parameter information for the chart symbol
		public RevitChartData RevitChartData { get; set; }

		
		

		public string Name {
			get
			{
				ARevitParam r = this[NameIdx];
				if (r == null) return null;
				return r.GetValue();
			}
	}
		public string Sequence => this[SeqIdx].GetValue();
		public string Description => this[Descdx].GetValue();
		public string IntName => this[IntNameIdx].GetValue();
		public string Developer => this[DevelopIdx].GetValue();
		public string FilePath => ((RevitParamFilePath) this[ChartFilePathIdx]).FullFilePath;
		public bool Exists => ((RevitParamFilePath) this[ChartFilePathIdx]).Exists;
		public string WorkSheet => this[ChartWorkSheetIdx].GetValue();

		public string CellFamilyName => this[ChartCellFamilyNameIdx].GetValue();
		public bool CellHasError { get; set; }

		public CellUpdateTypeCode UpdateType => RevitChartData.UpdateType;

		public bool HasErrors => ErrorCodeList.Count > 0;

		public ErrorCodes ErrorCode
		{
			set
			{
				errors.Add(value);
				// RevitChartData.ErrorCode = value;
			}
		}

		public List<ErrorCodes> ErrorCodeList
		{
			// get => RevitChartData.ErrorCodeList;
			get => errors;
		}

		public bool Add(RevitCellData revitCellData)
		{
			bool result = base.Add(makeCellSeqNameKey(revitCellData), revitCellData);

			if (!result)
			{
				revitCellData.ErrorCode = ErrorCodes.DUPLICATE_KEY_CS000I01_1;
				return false;
			}

			addCellLabels(revitCellData);

			return true;
		}

		public IEnumerable<ErrorCodes> Errors()
		{
			return errors;
		}

		public void ResetErrors() => RevitChartData.ResetErrors();

		public string makeCellSeqNameKey(RevitCellData revitCellData)
		{
			return RevitParamUtil.MakeSeqNameKey(revitCellData.Name, revitCellData.Sequence);
		}

		private bool addCellLabels(RevitCellData revitCellData)
		{
			bool result = false;

			foreach (KeyValuePair<string, RevitLabel> kvp in revitCellData.CellLabels)
			{
				try
				{
					kvp.Value.ParentChart = this;
					AllCellLabels.Add(kvp.Key, kvp.Value);
				}
				catch
				{
					result = false;
				}
			}

			return result;
		}

		public static RevitChart Invalid()
		{
			RevitChart cht = new RevitChart();
			cht.RevitChartData = new RevitChartData(ChartFamily.Invalid);
			cht.RevitChartData.ErrorCode = ErrorCodes.INVALID_DATA_FORMAT_CS000I10;
			cht.ErrorCode = ErrorCodes.INVALID_DATA_FORMAT_CS000I10;
			return cht;
		}

		public override string ToString()
		{
			return "I am RevitChart| " + Name;
		}
	}

	public class RevitChartData : RevitContainer<ARevitParam>, IAnnoSymContainer<ARevitParam>
	{
		// public ARevitParam[] RevitParamListType { get; set; }
		public ARevitParam[] revitParamListInternal { get; set; }

		public override dynamic GetValue()
		{
			return null;
		}

		public RevitChartData(ChartFamily chartFam)
		{
			ChartFamily = chartFam;



			NumberOfLists = 3;

			this[PT_INSTANCE] = new ARevitParam[chartFam.ParamCounts[(int) PT_INSTANCE]];
			// this[PT_INSTANCE] = new ARevitParam[ChartInstanceParamTotal];
			// RevitParamListBasic = new ARevitParam[ChartInstanceParamTotal];

			this[PT_TYPE] = new ARevitParam[chartFam.ParamCounts[(int) PT_TYPE]];
			// RevitParamListType = new ARevitParam[ChartTypeParamCount];

			revitParamListInternal = new ARevitParam[ChartInternalParamCount];
		}

		public ChartFamily ChartFamily { get; private set; }
		public CellFamily CellFamily => ChartFamily.CellFamily;

		public string FamilyName => AnnoSymbol?.Symbol.FamilyName ?? null;
		public string Name => this[ParamType.PT_INSTANCE, NameIdx].GetValue();
		public string Sequence => this[ParamType.PT_INSTANCE, SeqIdx].GetValue();
		public string CellFamilyName => this[ParamType.PT_INSTANCE, ChartCellFamilyNameIdx].GetValue();

		public CellUpdateTypeCode UpdateType => ((RevitParamUpdateType) this[PT_INSTANCE, ChartUpdateTypeIdx]).UpdateType;

		public AnnotationSymbol AnnoSymbol { get; set; }

		public Element RevitElement { get; set; }

		public void initLists()
		{

		}


		public void AddInternal(int idx, ARevitParam content)
		{
			revitParamListInternal[idx] = content;
		}
		
		public void AddType(int idx, ARevitParam content)
		{
			this[PT_TYPE][idx] = content;
			// RevitParamListType[idx] = content;
		}

		public override string ToString()
		{
			return "I am " + nameof(RevitChartData) + "| " + AnnoSymbol.Name;
		}
	}

#endregion


#region Container

	public interface IAnnoSymContainer<T> where T : ARevitParam
	{
		string Name { get; }

		AnnotationSymbol AnnoSymbol { get; set; }

		// T[] RevitParamListBasic { get; set; }
		// T[] RevitParamListType { get; set; }
		// T[] revitParamListInternal { get; set; }

		T[][] RevitParamLists { get; set; }

		// T this[int idx] { get; set; }

		T this[ParamType type, int idx] {get;}
	}

	public class RevitCellData : RevitContainer<ARevitParam>, IAnnoSymContainer<ARevitParam>
	{
		public ARevitParam[] RevitParamListType { get; set; }
		public ARevitParam[] revitParamListInternal { get; set; }

		public RevitCellData(CellFamily cellFam)
		{
			CellFamily = cellFam;

			NumberOfLists = 4;

			this[PT_INSTANCE] = new ARevitParam[cellFam.ParamCounts[(int) PT_INSTANCE]];
			// this[PT_INSTANCE] = new ARevitParam[CellBasicParamTotal];
			// RevitParamListBasic = new ARevitParam[CellBasicParamTotal];
			
			this[PT_TYPE] = new ARevitParam[cellFam.ParamCounts[(int) PT_TYPE]];
			// RevitParamListType = new ARevitParam[CellTypeParamCount];

			revitParamListInternal = new ARevitParam[CellInternalParamCount];
		}

		// key is OK
		public SortedDictionary<string, RevitLabel>  CellLabels
			= new SortedDictionary<string, RevitLabel>();

		public Dictionary<string, RevitLabel> ListOfLabels { get; set; }
			= new Dictionary<string, RevitLabel>();

		public AnnotationSymbol AnnoSymbol { get; set; }

		public CellFamily CellFamily { get; set; }

		public string Name => this[PT_INSTANCE, NameIdx].GetValue();
		public string Sequence => this[ParamType.PT_INSTANCE, SeqIdx].GetValue();

		public void AddInternal(int idx, ARevitParam content)
		{
			revitParamListInternal[idx] = content;
		}

		public void AddType(int idx, ARevitParam content)
		{
			RevitParamListType[idx] = content;
		}

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
			string n = this[PT_INSTANCE, NameIdx].ToString();
			return "I am RevitCell| " + this[PT_INSTANCE, NameIdx] ?? "null name";
		}
	}

	public class RevitLabel : RevitContainer<ARevitParam>
	{
		// a single label 
		public string Name => this[ParamType.PT_INSTANCE, LblNameIdx].GetValue();
		public string Formula => this[ParamType.PT_INSTANCE, LblFormulaIdx].GetValue();

		public RevitLabel(int paramCount)
		{
			this[PT_INSTANCE]= new ARevitParam[paramCount];
			// RevitParamListBasic = new ARevitParam[CellLabelParamTotal];
		}

		public RevitChart ParentChart { get; set; }

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