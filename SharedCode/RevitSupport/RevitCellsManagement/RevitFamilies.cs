// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             Families.cs
// Created:      2021-03-11 (8:32 PM)

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitParamManagement;

using static SpreadSheet01.RevitSupport.RevitParamManagement.RevitParamManager;

namespace SpreadSheet01.RevitSupport.RevitCellsManagement
{
	public class Families
	{
		// holds a dictionary of families of a category (is a list of families) (e.g. generic annotation, room tag)
		// Dictionary<string, Families>
		// string is the category i.e. annotation
		public Dictionary<string, ChartFamily> FamilyTypes { get; set; } = new Dictionary<string, ChartFamily>();

		public Families() { }

		public Families(ParamCat cat, ParamSubCat sCat)
		{
			Category = cat;
			SubCategory = sCat;
			FamilyTypes = new Dictionary<string, ChartFamily>();
		}

		public ParamCat Category { get; private set; }
		public ParamSubCat SubCategory { get; private set; }

		public void Add(ChartFamily family)
		{
			if (!FamilyTypes.ContainsKey(family.FamilyName))
			{
				FamilyTypes.Add(family.FamilyName, family);
			}
		}

		public bool Exists(ChartFamily family)
		{
			return FamilyTypes.ContainsKey(family.FamilyName);
		}

		public ChartFamily GetChartFamily(string chartFamilyName)
		{
			ChartFamily chart;

			bool result = FamilyTypes.TryGetValue(chartFamilyName, out chart);

			if (!result) return ChartFamily.Invalid;

			return chart;
		}

	}

	public struct ParamStatusData
	{
		public ParamDesc ParamDesc { get; }

		public string ShortName => ParamDesc.ShortName;
		public ParamType Type => ParamDesc.ParamType;
		public int Index => ParamDesc.Index;
		public bool IsReqd { get; set; }
		public bool IsFound { get; set; }
		public ErrorCodes Error { get; set; }

		public ParamStatusData(ParamDesc paramDesc)
		{
			ParamDesc = paramDesc;
			IsReqd = false;
			IsFound = false;
			Error = ErrorCodes.EC_NO_ERROR;
		}
	}


	public abstract class Family  : INotifyPropertyChanged
	{

	#region private fields

		protected ParamDesc[][] ParamDescLists { get; set; }

		protected int[] paramCounts;

	#endregion

	#region ctor

		// static Family()
		// {
		//
		//
		// 	// statusList = new SortedDictionary<string, ParamStatusData>[2];
		// 	//
		// 	// statusList[(int) ParamClass.PC_CHART] = new SortedDictionary<string, ParamStatusData>();
		// 	//
		// 	// statusList[(int) ParamClass.PC_CELL] = new SortedDictionary<string, ParamStatusData>();
		// 	//
		// 	//
		// 	// paramStatusList = new ParamStatusData[3][];
		// 	// paramStatusList[(int) ParamClass.PC_CHART] = new ParamStatusData[RevitParamManager.ChartParamTotal];
		// 	// paramStatusList[(int) ParamClass.PC_CELL] = new ParamStatusData[RevitParamManager.CellBasicParamTotal];
		// 	// paramStatusList[(int) ParamClass.PC_LABEL] = new ParamStatusData[RevitParamManager.CellLabelParamTotal];
		//
		// }

		public Family(string familyName, ParamCat cat, ParamSubCat sCat, 
			int[] paramCounts)
		{

			// shortNameLengths = new int[listCount];

			FamilyName = familyName;
			Category = cat;
			SubCategory = sCat;
			this.paramCounts = paramCounts;
			ParamDescLists = new ParamDesc[NumberOfLists][];
			// ShortNameMatchList = new Tuple<string, int>[NumberOfLists][];

			// process each param type
			for (int i = 0; i < NumberOfLists; i++)
			{
				ParamDescLists[i] = new ParamDesc[paramCounts[i]];
				// ShortNameMatchList[i] = new Tuple<string, int>[paramCounts[i]];
			}

			ParamMustExistCount = new int[NumberOfLists];

		}

	#endregion

	#region indexer

		public ParamDesc this[ParamType type, int idx]
		{
			get { return ParamDescLists[(int) type][idx]; }
		}
		
		public ParamDesc this[int type, int idx]
		{
			get { return ParamDescLists[type][idx]; }
		}

	#endregion

	#region public properties

		public string FamilyName { get; private set; }

		public abstract int NumberOfLists { get; protected set; }

		public ParamDesc[][] ParameterDescriptions => ParamDescLists;

		public int[] ParamMustExistCount {get; protected set; }

		public int[] ParamCounts => paramCounts;

		public ParamSubCat SubCategory { get; protected set; }

		public ParamCat Category { get; protected set; }

	#endregion

	#region non-public properties

		public abstract ParamClass ParamClass { get; set; }

	#endregion

	#region public methods

		public void AddParam(ParamDesc pd)
		{
			pd.ShortName = RevitParamSupport.GetShortName(pd.ParameterName);

			ParamDescLists[(int) pd.ParamType][pd.Index] = pd;
			// ShortNameMatchList[(int) pd.ParamType][pd.Index]
			// 	= new Tuple<string, int>(pd.ShortName, pd.Index);

			if (pd.Exist == ParamExistReqmt.EX_PARAM_MUST_EXIST)
			{
				ParamMustExistCount[(int) pd.ParamType]++;
			}
		}

		public bool Match2(string paramName, ParamType type, out ParamDesc pd)
		{
			bool result = false;
			pd = ParamDesc.Empty;
			string shortName = RevitParamSupport.GetShortName(paramName);

			if (type == ParamType.PT_INST_OR_INTL)
			{
				if (match(shortName, (int) ParamType.PT_INSTANCE, out pd))
				{
					result = true;
				} 
				else if (match(shortName, (int) ParamType.PT_INTERNAL, out pd))
				{
					result = true;
				}
			} 
			else
			{
				result = (match(shortName, (int) type, out pd));
			}

			return result;
		}





/*
		public bool Match(string paramName, out ParamDesc pd, bool isType, bool isLabel)
		{
			bool result = false;
			pd = ParamDesc.Empty;
			string shortName = RevitParamSupport.GetShortName(paramName);

			// if is type, test index 0 only
			// if not, test from 1 to num lists

			// int start = isType ? 0 : 1;
			// int end = isType ? 1 : NumberOfLists;
			//
			// for (int pt = start; pt < end; pt++)
			// {
			// 	if (match(shortName, pt, out pd))
			// 	{
			// 		result = true;
			// 		break;
			// 	}
			// }
			//
			//

			if (isType)
			{
				if (match(shortName, (int) ParamType.PT_TYPE, out pd))
				{
					result = true;
				}
			} 
			else if (isLabel)
			{
				if (match(shortName, (int) ParamType.PT_LABEL, out pd))
				{
					result = true;
				}
			}
			else
			{
				if (match(shortName, (int) ParamType.PT_INSTANCE, out pd))
				{
					result = true;
				} 
				else if (match(shortName, (int) ParamType.PT_INTERNAL, out pd))
				{
					result = true;
				}
			}

			return result;
		}
*/
		public bool Match(string paramName, ParamType pt, out ParamDesc pd)
		{
			pd = ParamDesc.Empty;
			string shortName = RevitParamSupport.GetShortName(paramName);

			return match(shortName, (int) pt, out pd);
		}

		private bool match(string shortName, int pt, out ParamDesc pd)
		{
			bool result = false;
			pd = ParamDesc.Empty;

			foreach (ParamDesc pdx in ParamDescLists[pt])
			{
				if (shortName.Equals(pdx.ShortName))
				{
					pd = pdx;
					result = true;
					break;
				}
			}
			return result;
		}

	#endregion

	#region private methods

		// private void resetParamStatusDataList(ParamStatusData[] statusList)
		// {
		// 	foreach (ParamStatusData paramStatus in statusList)
		// 	{
		// 		ParamStatusData psd = paramStatus;
		// 		psd.IsFound = false;
		// 		psd.Error = ErrorCodes.NO_ERROR;
		// 	}
		// }

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "Family| Name| " + FamilyName;
		}

	#endregion
	}

	public class ChartFamily : Family
	{
		private static int seqChart = 0;

		// private Dictionary<string, CellFamily> cellFamilies = new Dictionary<string, CellFamily>();

		public override int NumberOfLists { get; protected set; } = 3;
		public override ParamClass ParamClass { get; set; }

		public ChartFamily(string familyName, ParamCat cat, 
			ParamSubCat sCat, int[] paramCounts) : base(familyName, cat, sCat, paramCounts)
		{
			ParamClass = ParamClass.PC_CHART;
		}

		// public Dictionary<string, CellFamily> CellFamilies => cellFamilies;

		public CellFamily CellFamily { get; set; }

		// public bool GetCellFamily(string cellFamilyName, out CellFamily cell)
		// {
		// 	bool results = cellFamilies.TryGetValue(cellFamilyName, out cell);
		//
		// 	if (results) return true;
		//
		// 	cell = CellFamily.Invalid;
		//
		// 	return results;
		// }

		// public void AddCellFamily(CellFamily cell)
		// {
		// 	bool result = cellFamilies.ContainsKey(cell.FamilyName);
		//
		// 	if (result) return;
		//
		// 	cellFamilies.Add(cell.FamilyName, cell);
		// }

		// public CellFamily CellFamily {get; set; }

		public override string ToString()
		{
			return "ChartFamily| Name| " + FamilyName;
		}

		public static ChartFamily Invalid
		{
			get
			{
				string name = "*** invalid *** (" + (++seqChart).ToString("D3") + ")";

				return invalid(name);
			}
		}

		public static ChartFamily invalid(string name)
		{
			return new ChartFamily(name, ParamCat.CT_ANNOTATION, 
				ParamSubCat.SC_GENERIC_ANNOTATION, new []{0,0,0,0});
		}

	}

	public class CellFamily : Family
	{
		private static int seqCell = 0;

		public override int NumberOfLists { get; protected set; } = 4;
		public override ParamClass ParamClass { get; set; }

		public CellFamily(string familyName, ParamCat cat, 
			ParamSubCat sCat, int[] paramCounts) : base(familyName, cat, sCat, paramCounts)
		{
			ParamClass = ParamClass.PC_CELL;
		}

		public override string ToString()
		{
			return "CellFamily| Name| " + FamilyName;
		}

		public static CellFamily Invalid
		{
			get
			{
				string name = "*** invalid *** (" + (++seqCell).ToString("D3") + ")";

				return CellFamily.invalid(name);
			}
		}

		public static CellFamily invalid(string name)
		{
			return new CellFamily(name, ParamCat.CT_ANNOTATION, 
				ParamSubCat.SC_GENERIC_ANNOTATION, new []{0,0,0,0});
		}

	}


	// public class FamilyCategories : RevitContainers<ParamCat, FamilyCategory>
	// {
	// 	// holds a dictionary of all family categories (is a list of cats) (e.g. annotation, and walls, and ...)
	// 	// Dictionary<string, Families>
	// 	// string is the category i.e. annotation
	//
	// 	public FamilyCategories()
	// 	{
	// 		dynValue.Value = "Collection of Family Categories";
	//
	// 		Containers = new Dictionary<ParamCat, FamilyCategory>();
	// 	}
	//
	// 	public Dictionary<ParamCat, FamilyCategory> FamilyCats => Containers;
	//
	// 	public void Add(Family family)
	// 	{
	// 		FamilyCategory cat;
	//
	// 		bool result = Containers.TryGetValue(family.Category, out cat);
	//
	// 		if (!result)
	// 		{
	// 			cat = new FamilyCategory(family.Category);
	// 			cat.DynValue.Value = family.Category.ToString();
	// 			Add(family.Category, cat);
	// 		}
	//
	// 		cat.Add(family);
	// 	}
	//
	// 	public bool Exists(Family family)
	// 	{
	// 		FamilyCategory cat;
	//
	// 		bool result = Containers.TryGetValue(family.Category, out cat);
	// 	
	// 		if (!result) return false;
	//
	// 		return cat.Exists(family);
	// 	}
	//
	// }
	//
	// public class FamilyCategory : RevitContainers<ParamSubCat, Families>
	// {
	// 	// holds a dictionary of a family category (is a list of subcats) (e.g. either annotation or walls)
	// 	// Dictionary<string, Families>
	// 	// string is the category i.e. annotation
	//
	// 	public FamilyCategory(ParamCat cat)
	// 	{
	// 		Category = cat;
	// 		Containers = new Dictionary<ParamSubCat, Families>();
	// 	}
	//
	// 	public Dictionary<ParamSubCat, Families> FamilyCat => Containers;
	//
	// 	public ParamCat Category {get; private set; }
	//
	// 	public void Add(Family family)
	// 	{
	// 		Families fams;
	//
	// 		bool result = Containers.TryGetValue(family.SubCategory, out fams);
	//
	// 		if (!result)
	// 		{
	// 			fams = new Families(family.Category, family.SubCategory);
	// 			family.DynValue.Value = family.SubCategory.ToString();
	//
	// 			Add(family.SubCategory, fams);
	// 		}
	// 	}
	//
	// 	public bool Exists(Family family)
	// 	{
	// 		Families fams;
	//
	// 		bool result = Containers.TryGetValue(family.SubCategory, out fams);
	// 	
	// 		if (!result) return false;
	//
	// 		return fams.Exists(family);
	// 	}
	//
	// }
}