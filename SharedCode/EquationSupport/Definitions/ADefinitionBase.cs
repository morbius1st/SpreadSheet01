﻿#region using

using System;
using System.Windows.Controls;
using SharedCode.EquationSupport.TokenSupport.Amounts;

// using static SharedCode.EquationSupport.Definitions.TokenClassGroups;
// using static SharedCode.EquationSupport.Definitions.TokenClassUnit;
using static SharedCode.EquationSupport.Definitions.ValueClass;
using static SharedCode.EquationSupport.Definitions.ValueClassOp;
using static SharedCode.EquationSupport.Definitions.ValueClassNumber;
using static SharedCode.EquationSupport.Definitions.ValueClassId;
using static SharedCode.EquationSupport.Definitions.ValueClassIdVar;
using static SharedCode.EquationSupport.Definitions.ValueClassIdFun;
using static SharedCode.EquationSupport.Definitions.ValueClassUnit;
using static SharedCode.EquationSupport.Definitions.ValueClassUnitSys;
using static SharedCode.EquationSupport.Definitions.ValueClassGroup;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;

#endregion

// username: jeffs
// created:  5/17/2021 6:41:39 PM

namespace SharedCode.EquationSupport.Definitions
{
	public enum ValueType
	{
		VT_INVALID                = (int) VC_INVALID,
		VT_DEFAULT                = (int) VC_DEFAULT,
		VT_ASSIGNMENT             = (int) VC_ASSIGNMENT,
		VT_OPERATOR               = (int) VC_OPERATOR,
		
			VT_OP_LOGICALMATH     = (int) VT_OPERATOR + (int) VCO_LOGMATH,  // ie. <and> <or>
			VT_OP_LOGICALSTR      = (int) VT_OPERATOR + (int) VCO_LOGSTR,   // ie. <s_eq>, <s_ne> 
			VT_OP_RELATIONALMATH  = (int) VT_OPERATOR + (int) VCO_RELMATH,  // ie. <, <=, >, >- 
			VT_OP_RELATIONALSTR   = (int) VT_OPERATOR + (int) VCO_RELSTR,   // ie. starts with, ends with
			VT_OP_TEXT            = (int) VT_OPERATOR + (int) VCO_TEXT,     // ie. concatenate, left, right, mid
			VT_OP_ADDITIVE        = (int) VT_OPERATOR + (int) VCO_ADDITIVE,
			VT_OP_MULTIPLICATIVE  = (int) VT_OPERATOR + (int) VCO_MULTIPLICATIVE,
			VT_OP_URINARY         = (int) VT_OPERATOR + (int) VCO_URINARY,
	
		VT_STRING                 = (int) VC_STRING,
		VT_TEXT                   = (int) VC_TEXT,

// everything from here
		VT_BOOLEAN                = (int) VC_BOOLEAN,
		VT_NUMBER                 = (int) VC_NUMBER,
			VT_NUM_INTEGER        = (int) VT_NUMBER + (int) VCN_INTEGER,
			VT_NUM_DOUBLE         = (int) VT_NUMBER + (int) VCN_DOUBLE,
			VT_NUM_FRACTION       = (int) VT_NUMBER + (int) VCN_FRACTION,
		VT_UNIT                   = (int) VC_UNIT,
			VT_UN_LEN             = (int) VT_UNIT    + (int) VCU_LENGTH,
				VT_UN_LEN_IMP     = (int) VT_UN_LEN  + (int) VCUS_IMPERIAL,
				VT_UN_LEN_MET     = (int) VT_UN_LEN  + (int) VCUS_METRIC,
			VT_UN_AREA            = (int) VT_UNIT    + (int) VCU_AREA,
				VT_UN_AREA_IMP    = (int) VT_UN_AREA + (int) VCUS_IMPERIAL,
				VT_UN_AREA_MET    = (int) VT_UN_AREA + (int) VCUS_METRIC,
			VT_UN_VOL             = (int) VT_UNIT    + (int) VCU_VOLUME,
				VT_UN_VOL_IMP     = (int) VT_UN_VOL  + (int) VCUS_IMPERIAL,
				VT_UN_VOL_MET     = (int) VT_UN_VOL  + (int) VCUS_METRIC,

// to here is numeric
		VT_IDENTIFIER             = (int) VC_IDENTIFIER,
			VT_ID_VARIABLE        = (int) VT_IDENTIFIER + VCI_VARIABLE,
				VT_ID_VAR_KEY     = (int) VT_ID_VARIABLE + (int) VCIV_KEY,
				VT_ID_VAR_VAR     = (int) VT_ID_VARIABLE + (int) VCIV_VARIABLE,
			VT_ID_FUNCTION        = (int) VT_IDENTIFIER + VCI_FUNCTION,
				VT_ID_FUN_TXT     = (int) VT_ID_FUNCTION + (int) VCIF_TEXT,
				VT_ID_FUN_BOOL    = (int) VT_ID_FUNCTION + (int) VCIF_BOOLEAN,
				VT_ID_FUN_INT     = (int) VT_ID_FUNCTION + (int) VCIF_NUM_INT,
				VT_ID_FUN_DBL     = (int) VT_ID_FUNCTION + (int) VCIF_NUM_DBL,
		VT_GROUPING               = (int) VC_GROUPING,
			VT_GP_REF             = (int) VC_GROUPING + (int) VCG_REF,
			VT_GP_BEG             = (int) VC_GROUPING + (int) VCG_BEG,
			VT_GP_END             = (int) VC_GROUPING + (int) VCG_END,
			VT_PRN_BEG            = (int) VC_GROUPING + (int) VCG_PRN_BEG,
			VT_PRN_END            = (int) VC_GROUPING + (int) VCG_PRN_END,
			VT_GP_ARG_SEP         = (int) VC_GROUPING + (int) VCG_ARG_SEP,
		VT_MAX                    = (int) VC_MAX, 
	}

	public enum ValueClass
	{
		VC_INVALID         = -1,
		VC_DEFAULT         = 0,
		VC_ASSIGNMENT      = 10,
		VC_OPERATOR        = 100,
		VC_STRING          = 400,
		VC_TEXT            = 600,
		VC_BOOLEAN         = 800,
		VC_NUMBER          = 1000,
		VC_UNIT            = 1100,
		VC_IDENTIFIER      = 10000,
		VC_GROUPING        = 90000,
		VC_MAX             = 99999
	}

	public enum ValueClassOp
	{
		VCO_LOGMATH        = 1,
		VCO_LOGSTR         = 51,
		VCO_RELMATH		   = 101,
		VCO_RELSTR         = 151,
		VCO_TEXT           = 201,
		VCO_ADDITIVE       = 251,
		VCO_MULTIPLICATIVE = 301,
		VCO_URINARY        = 351,
	}

	public enum ValueClassNumber
	{
		VCN_INTEGER        = 1,
		VCN_DOUBLE         = 11,
		VCN_FRACTION       = 21,
	}

	public enum ValueClassId
	{
		VCI_VARIABLE       = 1,
		VCI_FUNCTION       = 50001,
	}

	public enum ValueClassIdVar
	{
		VCIV_KEY           = 1,
		VCIV_VARIABLE      = 101,
	}

	public enum ValueClassIdFun
	{
		VCIF_TEXT         = VDG_TEXT + 1,
		VCIF_BOOLEAN      = VDG_BOOLEAN + 1,
		VCIF_NUM_INT      = VDG_NUM_INT + 1,
		VCIF_NUM_DBL      = VDG_NUM_DBL + 1,
	}

	public enum ValueClassUnit
	{
		VCU_LENGTH		  = 1,
		VCU_AREA          = 301,
		VCU_VOLUME        = 601,
	}

	public enum ValueClassUnitSys
	{
		VCUS_IMPERIAL     = 1,
		VCUS_METRIC       = 101,
		VCUS_OTHER        = 201,
	}

	public enum ValueClassGroup
	{
		VCG_REF		     = 1,
		VCG_BEG		     = 2,
		VCG_END		     = 3,
		VCG_PRN_BEG      = 22,
		VCG_PRN_END      = 23,
		VCG_ARG_SEP      = 51,
	}

	public enum ValueDataGroup
	{
		VDG_INVALID      = -1,
		VDG_DEFAULT      = 0,

		VDG_STRING       = 10,
		VDG_TEXT         = 20,
		VDG_BOOLEAN      = 30,
		VDG_NUM_INT      = 40,
		VDG_NUM_DBL      = 50,
		VDG_DATETIME     = 60,
		// VDG_NUM_FRACT    = 60,
		// VDG_FUNCT        = 2000,
		// VDG_VAR          = 3000,

		VDG_UNIT         = 100,
		VDG_OBJECT       = 1000,
	}

	public enum ParseGroupVar
	{
		PGV_INVALID     = -1,
		PGV_DEFAULT     = 0,
		PGV_EXCL_ADDR   = 10,
		PGV_SYS_VAR     = 20,
		PGV_RVT_PARAM   = 30,
		PGV_PRJ_PARAM   = 40,
		PGV_GBL_PARAM   = 50,
		PGV_LBL_NAME    = 60,
	}
	
	public abstract class ADefinitionBase<T> where T : ADefBase //, new()
	{
	#region private fields

		protected static T[] idDefArray;

		protected static int count;

	#endregion

	#region public properties

		public abstract T Invalid { get; }
		public abstract T Default { get; }

		public int Count => count;

		// public AValDefBase this[int idx] => idDefArray[idx];
		public T this[int idx] => idDefArray[idx];

		public T[] Definitions => idDefArray;

	#endregion

	#region public methods

		public static T Classify(string test)
		{
			for (int i = 0; i < count; i++)
			{
				if (idDefArray[i] == null) continue;

				if (idDefArray[i].Equals(test))
				{
					return idDefArray[i];
				}
			}

			return null;
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is| " + nameof(DefinitionBase);
		}

	#endregion
	}

}