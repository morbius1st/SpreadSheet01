﻿#region using

using System;
using System.Collections.Generic;
using System.Windows.Controls;
using SharedCode.EquationSupport.TokenSupport;

// using static SharedCode.EquationSupport.Definitions.TokenClassGroups;
// using static SharedCode.EquationSupport.Definitions.TokenClassUnit;
using static SharedCode.EquationSupport.Definitions.ValueClass;
using static SharedCode.EquationSupport.Definitions.ValueClassOp;
using static SharedCode.EquationSupport.Definitions.ValueClassNumber;
using static SharedCode.EquationSupport.Definitions.ValueClassId;
using static SharedCode.EquationSupport.Definitions.ValueClassIdVar;
using static SharedCode.EquationSupport.Definitions.ValueClassIdFunct;
using static SharedCode.EquationSupport.Definitions.ValueClassUnit;
using static SharedCode.EquationSupport.Definitions.ValueClassUnitSys;
using static SharedCode.EquationSupport.Definitions.ValueClassGroup;

using static SharedCode.EquationSupport.Definitions.ValueDefinitions;

#endregion

// username: jeffs
// created:  5/17/2021 6:41:39 PM

namespace SharedCode.EquationSupport.Definitions
{
	public enum ValueType
	{
		VT_INVALID                = -1,
		VT_DEFAULT                = 0,
		VT_ASSIGNMENT             = (int) VC_ASSIGNMENT,
		VT_OPERATOR               = (int) VC_OPERATOR,
		
			VT_OP_LOGICAL         = (int) VT_OPERATOR + (int) VCO_LOGICAL,
			VT_OP_RELATIONAL      = (int) VT_OPERATOR + (int) VCO_RELATIONAL,
			VT_OP_STRING          = (int) VT_OPERATOR + (int) VCO_STRING,
			VT_OP_ADDITIVE        = (int) VT_OPERATOR + (int) VCO_ADDITIVE,
			VT_OP_MULTIPLICATIVE  = (int) VT_OPERATOR + (int) VCO_MULTIPLICATIVE,
			VT_OP_URINARY         = (int) VT_OPERATOR + (int) VCO_URINARY,
	
		VT_STRING                 = (int) VC_STRING,

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
			VT_ID_VARIABLE        = (int) VT_IDENTIFIER  + (int) VCI_VARIABLE,
				VT_ID_VAR_KEY     = (int) VT_ID_VARIABLE + (int) VCIV_KEY,
				VT_ID_VAR_VAR     = (int) VT_ID_VARIABLE + (int) VCIV_VARIABLE,
			VT_ID_FUNCTION        = (int) VT_IDENTIFIER  + (int) VCI_FUNCTION,
				VT_ID_FUN_INT     = (int) VT_ID_FUNCTION + (int) VCIF_INTERNAL,
				VT_ID_FUN_LIB     = (int) VT_ID_FUNCTION + (int) VCIF_LIBRARY,
				VT_ID_FUN_USR     = (int) VT_ID_FUNCTION + (int) VCIF_USER,
		VT_GROUPING               = (int) VC_GROUPING,
			VT_GP_REF             = (int) VC_GROUPING + (int) VCG_REF,
			VT_GP_BEG             = (int) VC_GROUPING + (int) VCG_BEG,
			VT_GP_END             = (int) VC_GROUPING + (int) VCG_END,
	}

	public enum ValueClass
	{
		VC_ASSIGNMENT      = 1,
		VC_OPERATOR        = 100,
		VC_STRING          = 500,
		VC_BOOLEAN         = 800,
		VC_NUMBER          = 1000,
		VC_UNIT            = 1100,
		VC_IDENTIFIER      = 10000,
		VC_GROUPING        = 90000
	}

	public enum ValueClassOp
	{
		VCO_LOGICAL        = 1,
		VCO_RELATIONAL     = 11,
		VCO_STRING         = 21,
		VCO_ADDITIVE       = 31,
		VCO_MULTIPLICATIVE = 41,
		VCO_URINARY        = 51,
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

	public enum ValueClassIdFunct
	{
		VCIF_INTERNAL     = 1,
		VCIF_LIBRARY      = 11,
		VCIF_USER         = 21,
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
		VCG_BEG          = 21,
		VCG_END          = 41,
	}

	public enum ParseGroupGeneral
	{
		PGG_INVALID      = -1,
		PGG_DEFAULT      = 0,
		PGG_ASSIGNMENT   = 10,
		PGG_OPERATOR     = 20,
		PGG_STRING       = 30,
		PGG_BOOLEAN      = 40,
		PGG_NUMBER       = 50,
		PGG_UNIT         = 60,
		PGG_FUNCTION     = 70,
		PGG_VARIABLE     = 80,
		PGG_GROUP_REF    = 100,
		PGG_GROUPING     = 110,
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

	public abstract class ADefBase : IEquatable<string>
	{
		private static int id = 1;

		public string Description { get; private set; }  // general description of the token
		public string ValueStr { get; private set; }     // the actual token value - i.e. "v1" or "+"
		public ValueType ValueType { get; private set; } // the type of value held
		public int Id { get; private set; }              // a numeric id // sequential number

		public ADefBase() { }

		public ADefBase(string description, string valueStr,
			ValueType valType)
		{
			Description = description;
			ValueStr = valueStr;
			ValueType = valType;
			Id = id++;
		}

		public abstract bool Equals(string test);

		public static ADefBase Invalid => (ADefBase) ValDefInst[Vd_Invalid];
	}

	// tokens associated with the initial equation parse
	public class ParseGen : ADefBase
	{
		public List<ADefBase2> aDefBase2 = null;
		public ParseGroupGeneral Group { get; private set; } // functional grouping
		public bool IsGood { get; private set; }             // indicates token is not valid

		public ParseGen() { }

		public ParseGen(   string description, string valueStr, ValueType valType,
			ParseGroupGeneral @group, ADefBase2[] aDefs, bool isGood = true)
			: base(description, valueStr, valType)
		{
			Group = group;

			if (aDefs == null || aDefs.Length == 0)
			{
				IsGood = false;
			}
			else
			{
				IsGood = isGood;

				this.aDefBase2 = new List<ADefBase2>();

				foreach (ADefBase2 vd in aDefs)
				{
					if (vd == null) continue;
					this.aDefBase2.Add(vd);
				}
			}
		}

		public ADefBase2 Classify(string test)
		{
			foreach (ADefBase2 ab in aDefBase2)
			{
				if (ab.Equals(test)) return ab;
			}

			return (ADefBase2) ADefBase.Invalid;
		}

		public override bool Equals(string test)
		{
			return (ValueStr?.Equals(string.Empty) ?? false) || (ValueStr?.Equals(test) ?? false);
		}
	}

	public abstract class ADefBase2 : ADefBase
	{
		public int Seq { get; private set; }   // the sequence number in a group
		public int Order { get; private set; } // order of operation - higher gets done first
		public bool IsNumeric { get; private set; }

		public ADefBase2() { }

		protected ADefBase2(string description, 
			string valueStr, 
			ValueType valType,
			int seq, 
			int order, 
			bool isNumeric
			) : base(description, valueStr, valType)
		{
			Seq = seq;
			Order = order;
			IsNumeric = isNumeric;
		}

		public abstract Token MakeToken();

	}

	// tokens associated with an equation operation
	public class DefValue : ADefBase2
	{
		public DefValue() { }

		public DefValue(string description, string valueStr, ValueType valType, 
			int seq, int order, bool isNumeric = false) : base(description, valueStr, valType, seq, order, isNumeric) { }

		public override Token MakeToken()
		{
			return null;
		}

		public override bool Equals(string test)
		{
			return (ValueStr?.Equals(string.Empty) ?? false) || (ValueStr?.Equals(test) ?? false);
		}
	}

	// tokens associated with the initial equation parse
	public class DefVar : ADefBase2
	{
		private int valStrLen;
		private int tokStrTrmLen;

		public string TokenStrTerm { get; private set; }
		public ParseGroupVar Group { get; private set; } // functional grouping

		public DefVar() { }

		public DefVar(string description, string valueStr, string tokenStrTerm, ValueType valType, ParseGroupVar @group, 
			int seq, int order, bool isNumeric = false) : base(description, valueStr, valType, seq, order, isNumeric)
		{
			TokenStrTerm = tokenStrTerm;
			Group = group;

			valStrLen = valueStr.Length;
			tokStrTrmLen = TokenStrTerm.Length;
		}

		public override Token MakeToken()
		{
			return null;
		}

		public override bool Equals(string test)
		{
			if (ValueStr == null) return false;

			string prefix = test.Substring(0, valStrLen);
			string suffix = test.Substring(test.Length - tokStrTrmLen, tokStrTrmLen);

			return prefix.Equals(ValueStr) && suffix.Equals(TokenStrTerm);
		}
	}

	public abstract class ADefinitionBase<T> where T : ADefBase, new()
	{
	#region private fields

		protected static T[] idDefArray;

		protected static int count;

	#endregion

	#region public properties

		public abstract T Invalid { get; }
		public abstract T Default { get; }

		public int Count => count;

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