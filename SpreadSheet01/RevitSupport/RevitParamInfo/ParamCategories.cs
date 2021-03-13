#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   3/7/2021 4:03:16 PM

namespace SpreadSheet01.RevitSupport.RevitParamInfo
{
	public enum FamilyType
	{
		CHART,
		CELL
	}

	public enum FamilyClassificationType 
	{
		CATEGORY,
		SUBCATEGORY,
		FAMILYTYPE
	}

	public enum ParamCat
	{
		CT_ANNOTATION,
		CT_WALLS
	}

	public enum ParamSubCat
	{
		SC_GENERIC_ANNOTATION,
		SC_CURTAIN_WALLS
	}
	

	// public class ParamCats
	// {
	//
	// 	public class SC_GENERIC_ANNOTATION : PC_ANNOTATION
	// 	{
	//
	// 	}
	//
	// 	public class SC_ROOM_TAG
	// 	{
	//
	// 	}
	//
	//
	// 	public class PC_ANNOTATION
	// 	{
	//
	// 	}
	//
	//
	//
	//
	// 	public class PC_WALLS
	// 	{
	// 		public ParamCat Category = ParamCat.CT_WALLS;
	//
	// 	}
	//
	// 	public class SC_CURTAIN_WALLS : PC_WALLS
	// 	{
	// 		public ParamCat Category = ParamCat.CT_WALLS;
	// 	}
	//
	//
	// }




}