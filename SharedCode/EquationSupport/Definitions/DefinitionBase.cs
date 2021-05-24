#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
		VT_BOOLEAN                = (int) VC_BOOLEAN,
		VT_NUMBER                 = (int) VC_NUMBER,
			VT_NUM_INTEGER        = (int) VC_NUMBER + (int) VCN_INTEGER,
			VT_NUM_DOUBLE         = (int) VC_NUMBER + (int) VCN_DOUBLE,
			VT_NUM_FRACTION       = (int) VC_NUMBER + (int) VCN_FRACTION,
		VT_UNIT                   = (int) VC_UNIT,
			VT_UN_LEN             = (int) VCU_LENGTH,
				VT_UN_LEN_IMP     = (int) VCU_LENGTH + (int) VCUS_IMPERIAL,
				VT_UN_LEN_MET     = (int) VCU_LENGTH + (int) VCUS_METRIC,
			VT_UN_AREA            = (int) VCU_AREA,
				VT_UN_AREA_IMP    = (int) VCU_AREA + (int) VCUS_IMPERIAL,
				VT_UN_AREA_MET    = (int) VCU_AREA + (int) VCUS_METRIC,
			VT_UN_VOL             = (int) VCU_VOLUME,
				VT_UN_VOL_IMP     = (int) VCU_VOLUME + (int) VCUS_IMPERIAL,
				VT_UN_VOL_MET     = (int) VCU_VOLUME + (int) VCUS_METRIC,
		VT_IDENTIFIER             = (int) VC_IDENTIFIER,
			VT_ID_VARIABLE        = (int) VC_IDENTIFIER + (int) VCI_VARIABLE,
				VT_ID_VAR_KEY     = (int) VC_IDENTIFIER + (int) VCI_VARIABLE + (int) VCIV_KEY,
				VT_ID_VAR_VAR     = (int) VC_IDENTIFIER + (int) VCI_VARIABLE + (int) VCIV_VARIABLE,
			VT_ID_FUNCTION        = (int) VC_IDENTIFIER + (int) VCI_FUNCTION,
				VT_ID_FUN_INT     = (int) VC_IDENTIFIER + (int) VCI_FUNCTION + (int) VCIF_INTERNAL,
				VT_ID_FUN_LIB     = (int) VC_IDENTIFIER + (int) VCI_FUNCTION + (int) VCIF_LIBRARY,
				VT_ID_FUN_USR     = (int) VC_IDENTIFIER + (int) VCI_FUNCTION + (int) VCIF_USER,
		VT_GROUPING               = (int) VC_GROUPING,
			VT_GP_REF             = (int) VC_GROUPING + (int) VCG_REF,
			VT_GP_BEG             = (int) VC_GROUPING + (int) VCG_BEG,
			VT_GP_END             = (int) VC_GROUPING + (int) VCG_END,
	}

	//
	// public enum ValueDataType
	// {
	// 	VDT_UNASSIGNED     = -1,
	// 	VDT_OBJECT         = 0,
	// 	VDT_ASSIGNMENT     ,
	// 	VDT_OPERATOR       ,
	// 	VDT_STRING         ,
	// 	VDT_BOOL           ,
	// 	VDT_NUM_INTEGER    ,
	// 	VDT_NUM_DOUBLE     ,
	// 	VDT_NUM_UNIT       
	// }

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
		VCO_RELATIONAL     = 10,
		VCO_STRING         = 20,
		VCO_ADDITIVE       = 30,
		VCO_MULTIPLICATIVE = 40,
		VCO_URINARY        = 50,
	}

	public enum ValueClassNumber
	{
		VCN_INTEGER        = 1,
		VCN_DOUBLE         = 10,
		VCN_FRACTION       = 20,
	}

	public enum ValueClassId
	{
		VCI_VARIABLE       = 1,
		VCI_FUNCTION       = 100,
	}

	public enum ValueClassIdVar
	{
		VCIV_KEY           = 1,
		VCIV_VARIABLE      = 10,
	}

	public enum ValueClassIdFunct
	{
		VCIF_INTERNAL     = 1,
		VCIF_LIBRARY      = 10,
		VCIF_USER         = 20,
	}


	public enum ValueClassUnit
	{
		VCU_LENGTH		  = 1,
		VCU_AREA          = 200,
		VCU_VOLUME        = 400,

	}

	public enum ValueClassUnitSys
	{
		VCUS_IMPERIAL     = 1,
		VCUS_METRIC       = 2000,
		VCUS_OTHER        = 5000,
	}

	public enum ValueClassGroup
	{
		VCG_REF		     = 1,
		VCG_BEG          = 20,
		VCG_END          = 40,
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

	public abstract class ADefBase
	{
		public string Description { get; private set; }      // general description of the token
		public string ValueStr { get; private set; }         // the actual token value - i.e. "v1" or "+"
		public ValueType ValueType { get; private set; }     // the type of value held
		public int Id { get; private set; }                  // a numeric id
		
		public ADefBase() { }

		public ADefBase(string description, string valueStr, 
			ValueType valType, int id)
		{
			Description = description;
			ValueStr = valueStr;
			ValueType = valType;
			Id = id;
		}
	}

	// tokens associated with an equation operation
	public class ValueDef : ADefBase
	{
		public int Seq { get; private set; }   // the sequence number in a group
		public int Order { get; private set; } // order of operation - higher gets done first

		public ValueDef() { }

		public ValueDef(string description, string valueStr, ValueType valType, 
			int id, int seq, int order) : base(description, valueStr, valType, id)
		{
			// DataType = dataType;
			Seq = seq;
			Order = order;
		}
	}

	// tokens associated with the initial equation parse
	public class ParseGen : ADefBase
	{
		public ParseGroupGeneral Group { get; private set; }	// functional grouping
		public bool IsGood { get; private set; } // indicates token is not valid

		public ParseGen() { }

		public ParseGen(string description, string valueStr, ValueType valType,
			ParseGroupGeneral group, int id, bool isGood = true) : base(description, valueStr, valType, id)
		{
			Group = group;
			IsGood = isGood;
		}
	}
	
	// tokens associated with the initial equation parse
	public class ParseVar : ADefBase
	{
		public string TokenStrTerm { get; private set; }
		public ParseGroupVar Group { get; private set; }	// functional grouping

		public ParseVar() { }

		public ParseVar(string description, string valueStr,  string tokenStrTerm, 
			ValueType valType, ParseGroupVar group, 
			int id) : base(description, valueStr, valType, id)
		{
			TokenStrTerm = tokenStrTerm;
			Group = group;
		}
	}

	public abstract class DefinitionBase<T> where T : ADefBase, new()
	{
	#region private fields

		protected static T[] idDefArray;

		protected static int count;

	#endregion

	#region ctor
		
		public DefinitionBase()
		{
			// Initialize();
		}

	#endregion

	#region public properties

		public abstract T Invalid { get; }
		public abstract T Default { get; }

		public int Count => count;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public T Classify(string test)
		{
			for (int i = 0; i < count; i++)
			{
				if (idDefArray[i].ValueStr.Equals(test))
				{
					return idDefArray[i];
				}
			}

			return null;
		}

	#endregion

	#region private methods

		// protected abstract void Initialize();

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is| " + nameof(DefinitionBase);
		}

	#endregion
	}
}