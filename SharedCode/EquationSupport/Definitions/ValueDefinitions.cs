#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueType;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;

#endregion

// user name: jeffs
// created:   5/17/2021 6:39:19 PM

namespace SharedCode.EquationSupport.Definitions
{
	public class ValueDefinitionsx : ADefinitionBase<ValDef>
	{
		private const int MAX_TOKENS = 50;

		private static readonly Lazy<ValueDefinitionsx> instance =
		new Lazy<ValueDefinitionsx>(()=> new ValueDefinitionsx());

		static ValueDefinitionsx() {Init(); }

		// private ValueDefinitions() : base() { }

		public static ValueDefinitionsx ValDefInst => instance.Value;

		public override ValDef Invalid => 
			new ValDef(-1, "Invalid", null, VT_INVALID, VDG_INVALID, (int) VT_INVALID);
		public override ValDef Default => 
			new ValDef(0, "Default", null, VT_DEFAULT, VDG_DEFAULT, (int) VT_DEFAULT);

		// public DefValue[] VDefDefs => idDefArray;
		//
		// public DefValue this[int idx] => VDefDefs[idx];

		// public string ValueStr(int idx) => VDefDefs[idx].ValueStr;

		// protected override void Initialize() { }

		public static int Vd_Invalid;
		public static int Vd_Default;

		// assignment
		public static int Vd_Assignment;

		// operator - logical
		public static int Vd_LogOr;
		public static int Vd_LogAnd;
		public static int Vd_LogEq;
		public static int Vd_LogInEq;
		public static int Vd_LogNot;
		// operator - relational
		public static int Vd_RelLt;
		public static int Vd_RelLtEq;
		public static int Vd_RelGt;
		public static int Vd_RelGtEq;
		// operator - string
		public static int Vd_AddStr;
		// operator - additive
		public static int Vd_MathAdd;
		public static int Vd_MathSubt;
		// operator - multiply
		public static int Vd_MathMul;
		public static int Vd_MathDiv;
		// operator - urniary
		public static int Vd_MathMod;
		public static int Vd_MathNeg;

		// string
		public static int Vd_String;

		// boolean
		public static int Vd_BoolTrue;
		public static int Vd_BoolFalse;

		// number - integer
		public static int Vd_NumInt;
		// number - double
		public static int Vd_NumDouble;
		// number - fract
		public static int Vd_NumFract;

		// unit - len
		public static int Vd_NumUntLenImp;
		public static int Vd_NumUntLenMet;

		// unit - area
		public static int Vd_NumUntAreaImp;
		public static int Vd_NumUntAreaMet;

		// unit - vol
		public static int Vd_NumUntVolImp;
		public static int Vd_NumUntVolMet;

		// identifier - function
		public static int Vd_FunBltIn;
		public static int Vd_FunLib;
		public static int Vd_FunUsr;
		// identifier - variable - key
		public static int Vd_VarKey;
		// identifier - variable
		public static int Vd_Varible;

		// grouping
		public static int Vd_GrpRef;
		public static int Vd_GrpBeg;
		public static int Vd_GrpEnd;
		
		private static void Init()
		{
			idDefArray = new ValDef[MAX_TOKENS];

			int idx = 0;

			
			vType = VT_INVALID;
			Vd_Invalid = idx++;
			idDefArray[Vd_Invalid] = defineValue("Invalid"                               , "", VT_INVALID, VDG_INVALID);
			
			vType = VT_DEFAULT;
			Vd_Assignment = idx++;
			idDefArray[Vd_Default] = defineValue("Default"                               , "", VT_DEFAULT, VDG_DEFAULT);


			vType = VT_ASSIGNMENT;
			Vd_Assignment = idx++;
			idDefArray[Vd_Assignment] = defineValue("Assignment"                         , "=", VT_ASSIGNMENT, VDG_TEXT);

			vType = VT_OP_LOGICAL;
			Vd_LogOr = idx++;
			idDefArray[Vd_LogOr]      = defineValue("Logical Or"                         , "<or>", VT_OP_LOGICAL, VDG_TEXT );
			Vd_LogAnd = idx++;
			idDefArray[Vd_LogAnd]     = defineValue("Logical And"                        , "<and>", VT_OP_LOGICAL, VDG_TEXT);
			Vd_LogEq = idx++;
			idDefArray[Vd_LogEq]      = defineValue("Logical Equality"                   , "==", VT_OP_LOGICAL, VDG_TEXT   );
			Vd_LogInEq = idx++;
			idDefArray[Vd_LogInEq]    = defineValue("Logical Inequality"                 , "!=", VT_OP_LOGICAL, VDG_TEXT   );
			Vd_LogNot = idx++;
			idDefArray[Vd_LogNot]    = defineValue("Logical Not"                         , "!", VT_OP_LOGICAL, VDG_TEXT   );

			vType = VT_OP_RELATIONAL;
			Vd_RelLt = idx++;
			idDefArray[Vd_RelLt]      = defineValue("Relational Less Than"               , "<", VT_OP_RELATIONAL, VDG_TEXT );
			Vd_RelLtEq = idx++;
			idDefArray[Vd_RelLtEq]    = defineValue("Relational Less Than or Equal"      , "<=", VT_OP_RELATIONAL, VDG_TEXT);
			Vd_RelGt = idx++;
			idDefArray[Vd_RelGt]      = defineValue("Relational Greater Than"            , ">", VT_OP_RELATIONAL, VDG_TEXT );
			Vd_RelGtEq = idx++;
			idDefArray[Vd_RelGtEq]    = defineValue("Relational Greater Than or Equal"   , ">=", VT_OP_RELATIONAL, VDG_TEXT);

			vType = VT_OP_STRING;
			Vd_AddStr = idx++;
			idDefArray[Vd_AddStr]     = defineValue("String Addition"                    , "&", VT_OP_STRING, VDG_TEXT);

			vType = VT_OP_ADDITIVE;
			Vd_MathAdd = idx++;
			idDefArray[Vd_MathAdd]    = defineValue("Additive Addition"                  , "+", VT_OP_ADDITIVE, VDG_TEXT);
			Vd_MathSubt = idx++;
			idDefArray[Vd_MathSubt]   = defineValue("Additive Subtraction"               , "-", VT_OP_ADDITIVE, VDG_TEXT);

			vType = VT_OP_MULTIPLICATIVE;
			Vd_MathMul = idx++;
			idDefArray[Vd_MathMul]    = defineValue("Multiplicative Multiply"            , "*", VT_OP_MULTIPLICATIVE, VDG_TEXT);
			Vd_MathDiv = idx++;
			idDefArray[Vd_MathDiv]    = defineValue("Multiplicative Divide"              , "/", VT_OP_MULTIPLICATIVE, VDG_TEXT);

			vType = VT_OP_URINARY;
			Vd_MathMod = idx++;
			idDefArray[Vd_MathMod]    = defineValue("Urinary Modulus"                    , "%", VT_OP_URINARY, VDG_TEXT);
			Vd_MathNeg = idx++;
			idDefArray[Vd_MathNeg]    = defineValue("Urinary Negative"                   , "-", VT_OP_URINARY, VDG_TEXT);



			vType = VT_STRING;
			Vd_String = idx++;
			idDefArray[Vd_String]    = defineValue("String"                              , "", VT_STRING, VDG_STRING);


			vType = VT_BOOLEAN;
			Vd_BoolTrue = idx++;
			idDefArray[Vd_BoolTrue]    = defineValue("Boolean True"                      , "True", VT_BOOLEAN, VDG_BOOLEAN, true);
			Vd_BoolFalse = idx++;
			idDefArray[Vd_BoolFalse]    = defineValue("Boolean False"                    , "False", VT_BOOLEAN, VDG_BOOLEAN, true);


			vType = VT_NUMBER;
			Vd_NumInt = idx++;
			idDefArray[Vd_NumInt]     = defineValue("Number Integer"                     , "", VT_NUM_INTEGER, VDG_NUM_INT, true);
			Vd_NumDouble = idx++;
			idDefArray[Vd_NumDouble]  = defineValue("Number Double"                      , "", VT_NUM_DOUBLE, VDG_NUM_DBL, true);
			Vd_NumFract = idx++;
			idDefArray[Vd_NumFract]   = defineValue("Number Fraction"                    , "", VT_NUM_FRACTION, VDG_NUM_FRACT, true);


			vType = VT_UNIT;
			Vd_NumUntLenImp = idx++;
			idDefArray[Vd_NumUntLenImp]  = defineValue("Unit Length Imperial"            , "", VT_UN_LEN_IMP, VDG_UNIT, true);
			Vd_NumUntLenMet = idx++;
			idDefArray[Vd_NumUntLenMet]  = defineValue("Unit Length Metric"              , "", VT_UN_LEN_MET, VDG_UNIT, true);

			Vd_NumUntAreaImp = idx++;
			idDefArray[Vd_NumUntAreaImp]  = defineValue("Unit Area Imperial"             , "", VT_UN_AREA_IMP, VDG_UNIT, true);
			Vd_NumUntAreaMet = idx++;
			idDefArray[Vd_NumUntAreaMet]  = defineValue("Unit Area Metric"               , "", VT_UN_AREA_MET, VDG_UNIT, true);

			Vd_NumUntVolImp = idx++;
			idDefArray[Vd_NumUntVolImp]  = defineValue("Unit Vol Imperial"               , "", VT_UN_VOL_IMP, VDG_UNIT, true);
			Vd_NumUntVolMet = idx++;
			idDefArray[Vd_NumUntVolMet]  = defineValue("Unit Vol Metric"                 , "", VT_UN_VOL_MET, VDG_UNIT, true);


			vType = VT_ID_VARIABLE;
			Vd_VarKey = idx++;
			idDefArray[Vd_VarKey]     = defineValue("Key Variable"                       , "{", VT_ID_VAR_KEY, VDG_VAR);
			Vd_Varible = idx++;
			idDefArray[Vd_Varible]     = defineValue("Variable"                           , "{", VT_ID_VAR_VAR, VDG_VAR);


			vType = VT_ID_FUNCTION;
			Vd_FunBltIn = idx++;
			idDefArray[Vd_FunBltIn]   = defineValue("Function Builtin"                   , "", VT_ID_FUN_INT, VDG_FUNCT);
			Vd_FunLib = idx++;
			idDefArray[Vd_FunLib]     = defineValue("Function Library"                   , "", VT_ID_FUN_LIB, VDG_FUNCT);
			Vd_FunUsr = idx++;
			idDefArray[Vd_FunUsr]     = defineValue("Function User"                      , "", VT_ID_FUN_USR, VDG_FUNCT);


			vType = VT_GROUPING;
			Vd_GrpRef = idx++;
			idDefArray[Vd_GrpRef]     = defineValue("Group Reference"                    , "", VT_GP_REF, VDG_TEXT);
			Vd_GrpBeg = idx++;
			idDefArray[Vd_GrpBeg]     = defineValue("Parenthesis Begin"                  , "(", VT_GP_BEG, VDG_TEXT);
			Vd_GrpEnd = idx++;
			idDefArray[Vd_GrpEnd]     = defineValue("Parenthesis End"                    , ")", VT_GP_END, VDG_TEXT);


			count = idx;
		}

		// private static int setValue(int idx, string desc, string tokenStr, ValueType valType, bool isNumeric = false)
		// {
		// 	idDefArray[idx] = defineValue(desc, tokenStr, valType, isNumeric);
		//
		// 	return idx;
		// }

		private static int id = 0;
		private static int seq = 0;
		private static ValueType vType;
		private static ValueType vTypePrior = VT_DEFAULT;

		// private static DefValue defineValue(int index, string desc, string tokenStr, ValueType valType, bool isNumeric = false)
		private static ValDef defineValue(   string desc, string tokenStr, ValueType valType, ValueDataGroup group, bool isNumeric = false)
		{
			if (vType != vTypePrior)
			{
				seq = 0;
				vTypePrior = vType;
			}

			return new ValDef(0, desc, tokenStr, valType, group, id++, isNumeric);
		}
	}
}