#region + Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;
using SharedCode.RevitSupport.RevitParamManagement;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitParamManagement;
using SpreadSheet01.RevitSupport.RevitParamValue;
using UtilityLibrary;
using static SpreadSheet01.RevitSupport.RevitParamManagement.RevitParamManager;

using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamType;

//using static SharedCode.RevitSupport.RevitParamManagement.ErrorCodeList2;
#endregion

// user name: jeffs
// created:   2/25/2021 6:52:29 PM
// spreadsheet01

namespace SpreadSheet01.RevitSupport.RevitCellsManagement
{
#region Containers

	public abstract class RevitContainers<TV> : /* ARevitParam, */ INotifyPropertyChanged
	{
		private ErrorCodeList errorList = new ErrorCodeList();

		protected RevitContainers()
		{
			Containers = new Dictionary<string, TV>();
		}

		// protected List<ErrorCodes> errors = new List<ErrorCodes>();

		public List<ErrorCodes> ErrorCodeList => errorList.ErrorsList;

		public ErrorCodes ErrorCode
		{
			set { errorList.Add(value); }
		}

		public void ResetErrors() => errorList.Reset();

		public bool HasErrors => errorList.HasErrors;

		protected Dictionary<string, TV> Containers { get; set; }

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

		public abstract int NumberOfLists { get; protected set; }

		public T[][] RevitParamLists { get; set; }

		public T[] this[ParamType type] 
		{
			get => RevitParamLists[(int) type];
			set
			{
				int t = (int) type;

				RevitParamLists[(int) type] = value;
			}
		}

		public T this[ParamType type, int idx] => RevitParamLists[(int) type][idx];

		public void Add(ParamType type, int idx, T content)
		{
			RevitParamLists[(int) type][idx] = content;
		}

		// public ErrorCodes ErrorCode
		// {
		// 	set { base.ErrorCode = value; }
		// }
		//
		// public List<ErrorCodes> ErrorCodeList
		// {
		// 	get => base.ErrorList;
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
		public RevitCharts(string name)
		{
			Name = name;
		}

		public string Name { get; private set;}

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

		// public bool CellHasError { get; set; }

		public CellUpdateTypeCode UpdateType => RevitChartData.UpdateType;

		public bool Add(RevitCellData revitCellData)
		{
			int ext = 0;
			bool result;
			string key;

			do
			{
				key = RevitParamUtil.MakeCellKey( revitCellData.Sequence, revitCellData.Name, ext);
				result = Containers.ContainsKey(key);

				ext++;
				if (ext > 50)
				{
					result = true;
					break;
				}
			}
			while (result);

			if (result) return false;

			result = base.Add(key, revitCellData);

			if (!result)
			{
//				ErrCodeList.Add(this, ErrorCodes.DUPLICATE_KEY_CS000I01_1);
				revitCellData.ErrorCode = ErrorCodes.DUPLICATE_KEY_CS000I01_1;
				return false;
			}

			addCellLabels(revitCellData);

			return true;
		}

		private bool addCellLabels(RevitCellData revitCellData)
		{
			bool result = false;

			foreach (KeyValuePair<string, RevitLabel> kvp in revitCellData.ListOfLabels)
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
		public RevitChartData(ChartFamily chartFam)
		{
			ReqdParamCount = new int[NumberOfLists];

			ChartFamily = chartFam;

			this[PT_INSTANCE] = new ARevitParam[chartFam.ParamCounts[(int) PT_INSTANCE]];
			// this[PT_INSTANCE] = new ARevitParam[ChartInstanceParamTotal];
			// RevitParamListBasic = new ARevitParam[ChartInstanceParamTotal];

			this[PT_TYPE] = new ARevitParam[chartFam.ParamCounts[(int) PT_TYPE]];
			// RevitParamListType = new ARevitParam[ChartTypeParamCount];

			this[PT_INTERNAL] = new ARevitParam[chartFam.ParamCounts[(int) PT_INTERNAL]];
			// revitParamListInternal = new ARevitParam[ChartInternalParamCount];
		}

		public int[] ReqdParamCount {get; private set; }

		public override int NumberOfLists { get;  protected set; } = 3;

		public ChartFamily ChartFamily { get; private set; }
		public CellFamily CellFamily => ChartFamily.CellFamily;

		public string FamilyName => AnnoSymbol?.Symbol.FamilyName ?? null;
		public string Name => this[ParamType.PT_INSTANCE, NameIdx].GetValue();
		public string Sequence => this[ParamType.PT_INSTANCE, SeqIdx].GetValue();
		public string CellFamilyName => this[ParamType.PT_INSTANCE, ChartCellFamilyNameIdx].GetValue();

		public CellUpdateTypeCode UpdateType => ((RevitParamUpdateType) this[PT_INSTANCE, ChartUpdateTypeIdx]).UpdateType;

		public override dynamic GetValue()
		{
			return null;
		}

		public new void Add(ParamType type, int idx, ARevitParam content)
		{
			this[type][idx] = content;

			if (content.ParamDesc.IsRequired)
			{
				ReqdParamCount[(int) type]++;
			}
		}

		public AnnotationSymbol AnnoSymbol { get; set; }

		public Element RevitElement { get; set; }

		
		public void ValidateMustExist()
		{
			for (int i = 0; i < NumberOfLists; i++)
			{
				if (ChartFamily.ParamMustExistCount[i] != ReqdParamCount[i])
				{
//					ErrCodeList.Add(this, ErrorCodesAssist.EC_MustExist[(int) ParamClass.PC_CHART][i]);
					ErrorCode = ErrorCodesAssist.EC_MustExist[(int) ParamClass.PC_CHART][i];
				}

			}
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

		T this[ParamType type, int idx] {get;}
	}

	public class RevitCellData : RevitContainer<ARevitParam>, IAnnoSymContainer<ARevitParam>
	{
		// public ARevitParam[] RevitParamListType { get; set; }
		// public ARevitParam[] revitParamListInternal { get; set; }

		public RevitCellData(CellFamily cellFam)
		{
			CellFamily = cellFam;

			ReqdParamCount = new int[NumberOfLists + RevitParamManager.MAX_LABELS_PER_CELL];
			ListOfLabels = new SortedDictionary<string, RevitLabel>();

			this[PT_INSTANCE] = new ARevitParam[cellFam.ParamCounts[(int) PT_INSTANCE]];
			// this[PT_INSTANCE] = new ARevitParam[CellBasicParamTotal];
			// RevitParamListBasic = new ARevitParam[CellBasicParamTotal];
			
			this[PT_TYPE] = new ARevitParam[cellFam.ParamCounts[(int) PT_TYPE]];
			// RevitParamListType = new ARevitParam[CellTypeParamCount];

			this[PT_INTERNAL] = new ARevitParam[cellFam.ParamCounts[(int) PT_INTERNAL]];
			// revitParamListInternal = new ARevitParam[CellInternalParamCount];
		}

		public int[] ReqdParamCount {get; private set; }

		public override int NumberOfLists { get;  protected set; } = 4;

		// public SortedDictionary<string, RevitLabel>  CellLabels
		// 	= new SortedDictionary<string, RevitLabel>();

		public SortedDictionary<string, RevitLabel> ListOfLabels { get; set; }

		public AnnotationSymbol AnnoSymbol { get; set; }

		public CellFamily CellFamily { get; set; }

		public string Name => this[PT_INSTANCE, NameIdx].GetValue();
		public string Sequence => this[ParamType.PT_INSTANCE, SeqIdx].GetValue();

		public new void Add(ParamType type, int idx, ARevitParam content)
		{
			this[type][idx] = content;

			if (content.ParamDesc.IsRequired)
			{
				ReqdParamCount[(int) type]++;
			}
		}

		public override dynamic GetValue()
		{
			return null;
		}

		// public void AddLabelRef(RevitLabel label)
		// {
		// 	ListOfLabels.Add(label.Name, label);
		// }

		public bool AddLabelParam(int labelId, int idx, ARevitParam labelParam)
		{
			// Debug.Write("    @Add_Label| " + labelParam.ParamDesc.ParameterName.PadRight(18));

			RevitLabel label = getLabel(labelId);

			label.Add(PT_LABEL, idx, labelParam);

			// if (idx == RevitParamManager.LblNameIdx)
			// {
			// 	AddLabelRef(label);
			// }

			if (labelParam.ParamDesc.IsRequired)
			{
				// int i = (int) PT_LABEL + labelId;
				// Debug.Write(" is reqd| idx| " + i.ToString("##0"));

				ReqdParamCount[(int) PT_LABEL + labelId]++;
			}

			// Debug.Write("\n");

			return !label.HasErrors;
		}

		public void ValidateMustExist()
		{
			for (int i = 0; i < NumberOfLists - 1 ; i++)
			{
				if (CellFamily.ParamMustExistCount[i] != ReqdParamCount[i])
				{
//					ErrCodeList.Add(this, ErrorCodesAssist.EC_MustExist[(int) ParamClass.PC_CELL][i]);
					ErrorCode = ErrorCodesAssist.EC_MustExist[(int) ParamClass.PC_CELL][i];
					return;
				}
			}
		}

		public void ValidateLabelMustExist()
		{
			bool result;

			foreach (KeyValuePair<string, RevitLabel> kvp in ListOfLabels)
			{
				if (!kvp.Value.ValidateMustExist(CellFamily.ParamMustExistCount[NumberOfLists - 1]))
				{
//					ErrCodeList.Add(this, ErrorCodes.CEL_LABEL_PARAM_MISSING_CS001124);
					ErrorCode = ErrorCodes.CEL_LABEL_PARAM_MISSING_CS001124;
					return;
				}
			}
		}


		private RevitLabel getLabel(int labelId)
		{
			RevitLabel label;

			string key = RevitParamUtil.MakeLabelKey(Sequence, labelId);

			bool result = ListOfLabels.TryGetValue(key, out label);

			if (!result)
			{
				label = new RevitLabel(labelId, CellFamily.ParamCounts[(int) PT_LABEL]);
				ListOfLabels.Add(key, label);
			}

			return label;
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
		public string Name => this[PT_LABEL, LblNameIdx].GetValue();
		public string Formula => this[PT_LABEL, LblFormulaIdx].GetValue();

		public RevitLabel(int labelId, int paramCount)
		{
			LabelId = labelId;

			this[PT_LABEL] = new ARevitParam[paramCount];
			// RevitParamListBasic = new ARevitParam[CellLabelParamTotal];
		}

		public override int NumberOfLists { get;  protected set; } = 4;

		public int LabelId { get; private set; }

		public RevitChart ParentChart { get; set; }

		public bool ValidateMustExist(int mustExistReqd)
		{
			int mustExistAct = 0;

			foreach (ARevitParam p in this[PT_LABEL])
			{
				if (p == null)
				{
					continue;
				}

				if (p.ParamDesc.Exist == ParamExistReqmt.EX_PARAM_MUST_EXIST)
				{
					mustExistAct++;
				}
			}

			Debug.Write("label validate| name| " + Name.PadRight(18));
			Debug.Write(" act| " + mustExistAct.ToString("##0"));
			Debug.Write(" reqd| " + mustExistReqd.ToString("##0"));

			Debug.Write("\n" );

			if (mustExistAct != mustExistReqd)
			{
//				ErrCodeList.Add(this, ErrorCodes.LBL_PARAM_MISSING_CS001123);
				ErrorCode = ErrorCodes.LBL_PARAM_MISSING_CS001123;
				return false;
			}

			return true;
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