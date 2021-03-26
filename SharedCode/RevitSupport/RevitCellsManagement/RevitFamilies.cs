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

namespace SpreadSheet01.RevitSupport.RevitCellsManagement
{

	public class Families
	{
		// holds a dictionary of families of a category (is a list of families) (e.g. generic annotation, room tag)
		// Dictionary<string, Families>
		// string is the category i.e. annotation
		public Dictionary<string, Family> FamilyTypes { get; set; } = new Dictionary<string, Family>();

		public Families() { }

		public Families(ParamCat cat, ParamSubCat sCat)
		{
			Category = cat;
			SubCategory = sCat;
			FamilyTypes = new Dictionary<string, Family>();
		}

		public ParamCat Category { get; private set; }
		public ParamSubCat SubCategory { get; private set; }

		public void Add(Family family)
		{
			if (!FamilyTypes.ContainsKey(family.FamilyName))
			{
				FamilyTypes.Add(family.FamilyName, family);
			}
		}

		public bool Exists(Family family)
		{
			return FamilyTypes.ContainsKey(family.FamilyName);
		}
	}

	public struct ParamStatusData
	{
		public string ShortName { get; }

		public ParamDesc ParamDesc { get; }
		public ParamType Type { get; }
		public int Index { get; }
		public bool IsReqd { get; set;}
		public bool IsFound { get; set;}
		public ErrorCodes Error { get; set; }

		public ParamStatusData(  string shortName, ParamType type, ParamDesc paramDesc, int idx)
		{
			ShortName = shortName;
			ParamDesc = paramDesc;
			Type = type;
			Index = idx;
			IsReqd = false;
			IsFound = false;
			Error = ErrorCodes.NO_ERROR;
		}
	}


	public abstract class Family  : INotifyPropertyChanged
		// : RevitContainer<ParamDesc>
	{
		// holds a single family info
		// holds the family's type parameters     (TypeParams)
		// holds the family's instance parameters (InstanceParams)
		// holds the family's internal parameters (InternalParams)

	#region private fields

		protected List<List<ParamDesc>> paramLists = new List<List<ParamDesc>>();

		//                                          param req'd + found +     + other error / code
		// meaning paramName Shortened-v |   type -v index -v   v       v     v 
		protected static SortedDictionary<string, ParamStatusData>[] statusList;

		protected int paramMustExistCount;

		protected ParamClass ParamClass { get; set; }

		// protected int[] shortNameLengths;

		// protected int[] mustExistParamCount;

		// private int typeShortNameLen = 8;
		// private int instShortNameLen = 8;
		// private int intShortNameLen = 8;

	#endregion

	#region ctor

		static Family()
		{
			statusList = new SortedDictionary<string, ParamStatusData>[2];

			statusList[(int) ParamClass.PC_CHART] = new SortedDictionary<string, ParamStatusData>();

			statusList[(int) ParamClass.PC_CELL] = new SortedDictionary<string, ParamStatusData>();
		}

		public Family(string familyName, ParamCat cat, ParamSubCat sCat)
		{
			// shortNameLengths = new int[listCount];


			FamilyName = familyName;
			Category = cat;
			SubCategory = sCat;

			for (int i = 0; i < listCount; i++)
			{
				paramLists.Add(new List<ParamDesc>());
			}
		}

	#endregion

	#region indexer

		public ParamDesc this[ParamType type, int idx]
		{
			get { return paramLists[(int) type][idx]; }
		}

	#endregion

	#region public properties

		public string FamilyName { get; private set; }

		public abstract int listCount { get; protected set; }

		public ParamSubCat SubCategory { get; protected set; }

		public ParamCat Category { get; protected set; }

	#endregion

		public int Index { get; set; }

	#region public methods

		public void ResetStatusDataList ()
		{
			foreach (KeyValuePair<string, ParamStatusData> kvp in statusList[(int) ParamClass])
			{
				ParamStatusData psd = kvp.Value;
				psd.IsFound = false;
				psd.Error = ErrorCodes.NO_ERROR;
			}
		}

		internal static ParamDesc Match(string paramName, ParamClass paramClass)
		{
			string shortName = RevitParamManager.GetShortName(paramName);

			ParamStatusData statData;

			bool found = statusList[(int) paramClass].TryGetValue(shortName, out statData);

			if (!found) return ParamDesc.Empty;
			//
			// ParamType type = statData.Type;
			//
			// int idx = statData.Index;
			//
			// return paramLists[(int) type][idx];

			return statData.ParamDesc;
		}

		// public dynamic GetClassification(FamilyClassificationType type)
		// {
		// 	if (type == FamilyClassificationType.CATEGORY) return Category;
		// 	if (type == FamilyClassificationType.SUBCATEGORY) return SubCategory;
		// 	if (type == FamilyClassificationType.FAMILYTYPE) return FamilyName;
		// 	return null;
		// }

		public List<ParamDesc> ParamList(ParamType type)
		{
			if ((int) type > listCount) return null;

			return paramLists[(int) type];
		}

		public void AddParam(ParamDesc pd)
		{
			pd.ShortName = RevitParamManager.GetShortName(pd.ParameterName);

			paramLists[(int) pd.Type][pd.Index] = pd;

			ParamStatusData statData = 
				new ParamStatusData(pd.ShortName, pd.Type, pd, pd.Index);


			statusList[(int) ParamClass].Add(pd.ShortName, statData);

			if (pd.Exist == ParamExistReqmt.EX_PARAM_MUST_EXIST)
			{
				paramMustExistCount++;

				statData.IsReqd = true;
			}
		}

		// public int ShortNameLength(ParamType type)
		// {
		// 	return shortNameLengths[(int) type];
		// }

		public void ConfigureLists(int[] counts)
		{
			// mustExistParamCount = new int[counts.Length];

			for (int i = 0; i < paramLists.Count; i++)
			{
				for (int j = 0; j < counts[i]; j++)
				{
					(paramLists[i]).Add(ParamDesc.Empty);
				}
			}
		}

	#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return "Family| Name| " + FamilyName;
		}
	}

	public class ChartFamily : Family
	{
		// public static new ParamClass ParamClass { get; protected set; } = ParamClass.CHART;

		public override int listCount { get; protected set; } = 3;

		public ChartFamily(string familyName, ParamCat cat, ParamSubCat sCat) : base(familyName, cat, sCat)
		{
			base.ParamClass= ParamClass.PC_CHART;
		}

		public  List<ParamDesc> InternalParams => paramLists[(int) ParamType.PT_INTERNAL];
		public  List<ParamDesc> TypeParams => paramLists[(int) ParamType.PT_TYPE];
		public  List<ParamDesc> InstanceParams => paramLists[(int) ParamType.PT_INSTANCE];

		public override string ToString()
		{
			return "ChartFamily| Name| " + FamilyName;
		}	
	}

	public class CellFamily : Family
	{
		// public static new ParamClass ParamClass { get; protected set; } = ParamClass.CELL;

		public override int listCount { get; protected set; } = 4;

		public CellFamily(string familyName, ParamCat cat, ParamSubCat sCat) : base(familyName, cat, sCat)
		{
			base.ParamClass= ParamClass.PC_CELL;
		}

		public  List<ParamDesc> InternalParams => paramLists[(int) ParamType.PT_INTERNAL];
		public  List<ParamDesc> TypeParams => paramLists[(int) ParamType.PT_TYPE];
		public  List<ParamDesc> InstanceParams => paramLists[(int) ParamType.PT_INSTANCE];
		public  List<ParamDesc> LabelParams => paramLists[(int) ParamType.PT_LABEL];


			public override string ToString()
		{
			return "CellFamily| Name| " + FamilyName;
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