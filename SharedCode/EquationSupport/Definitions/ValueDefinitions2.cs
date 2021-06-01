#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EquationSupport.Definitions.AmountDefs;
using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueType;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;

#endregion

// user name: jeffs
// created:   5/17/2021 6:39:19 PM

namespace SharedCode.EquationSupport.Definitions
{
	public class ValueDefinitions : ADefinitionBase<ADefBase2>
	{
		private const int MAX_TOKENS = 50;

		private static readonly Lazy<ValueDefinitions> instance =
		new Lazy<ValueDefinitions>(()=> new ValueDefinitions());

		static ValueDefinitions() {Init(); }

		// private ValueDefinitions() : base() { }

		public static ValueDefinitions ValDefInst => instance.Value;

		public override ADefBase2 Invalid => 
			new ValDef(-1, "Invalid", null, VT_INVALID, VDG_INVALID, (int) VT_INVALID);
		public override ADefBase2 Default => 
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
			idDefArray = new ADefBase2[MAX_TOKENS];

			int idx = 0;

			
			vType = VT_INVALID;
			Vd_Invalid = idx++;
			idDefArray[Vd_Invalid] = defineValue(Vd_Invalid, "Invalid"                               , "", VT_INVALID, VDG_INVALID);
			
			vType = VT_DEFAULT;
			Vd_Assignment = idx++;
			idDefArray[Vd_Default] = defineValue(Vd_Default, "Default"                               , "", VT_DEFAULT, VDG_DEFAULT);


			vType = VT_ASSIGNMENT;
			Vd_Assignment = idx++;
			// idDefArray[Vd_Assignment] = defineValue("Assignment"                         , "=", VT_ASSIGNMENT);
			idDefArray[Vd_Assignment] = new DefAssignment(Vd_Assignment, "Assignment", "=", VT_ASSIGNMENT, 1);

			vType = VT_OP_LOGICAL;
			Vd_LogOr = idx++;
			idDefArray[Vd_LogOr]      = defineValue(Vd_LogOr, "Logical Or"                         , "<or>", VT_OP_LOGICAL, VDG_TEXT );
			Vd_LogAnd = idx++;
			idDefArray[Vd_LogAnd]     = defineValue(Vd_LogAnd, "Logical And"                        , "<and>", VT_OP_LOGICAL, VDG_TEXT);
			Vd_LogEq = idx++;
			idDefArray[Vd_LogEq]      = defineValue(Vd_LogEq, "Logical Equality"                   , "==", VT_OP_LOGICAL, VDG_TEXT   );
			Vd_LogInEq = idx++;
			idDefArray[Vd_LogInEq]    = defineValue(Vd_LogInEq, "Logical Inequality"                 , "!=", VT_OP_LOGICAL, VDG_TEXT   );
			Vd_LogNot = idx++;
			idDefArray[Vd_LogNot]    = defineValue(Vd_LogNot, "Logical Not"                         , "!", VT_OP_LOGICAL, VDG_TEXT   );

			vType = VT_OP_RELATIONAL;
			Vd_RelLt = idx++;
			idDefArray[Vd_RelLt]      = defineValue(Vd_RelLt, "Relational Less Than"               , "<", VT_OP_RELATIONAL, VDG_TEXT );
			Vd_RelLtEq = idx++;
			idDefArray[Vd_RelLtEq]    = defineValue(Vd_RelLtEq, "Relational Less Than or Equal"      , "<=", VT_OP_RELATIONAL, VDG_TEXT);
			Vd_RelGt = idx++;
			idDefArray[Vd_RelGt]      = defineValue(Vd_RelGt, "Relational Greater Than"            , ">", VT_OP_RELATIONAL, VDG_TEXT );
			Vd_RelGtEq = idx++;
			idDefArray[Vd_RelGtEq]    = defineValue(Vd_RelGtEq, "Relational Greater Than or Equal"   , ">=", VT_OP_RELATIONAL, VDG_TEXT);

			vType = VT_OP_STRING;
			Vd_AddStr = idx++;
			idDefArray[Vd_AddStr]     = defineValue(Vd_AddStr, "String Addition"                    , "&", VT_OP_STRING, VDG_TEXT);

			vType = VT_OP_ADDITIVE;
			// Vd_MathAdd = idx++;
			// idDefArray[Vd_MathAdd]    = new DefOpMathAdd(Vd_MathAdd, "Additive Addition", "+", VT_OP_ADDITIVE, 1);


			Vd_MathAdd = setValue(new DefOpMathAdd(idx++, "Additive Addition", "+", VT_OP_ADDITIVE, 1));

			Vd_MathSubt = idx++;
			idDefArray[Vd_MathSubt]    = new DefOpMathSubt(Vd_MathSubt, "Additive Subtraction", "-", VT_OP_ADDITIVE, 2);
			

			vType = VT_OP_MULTIPLICATIVE;
			Vd_MathMul = idx++;
			idDefArray[Vd_MathMul]    = defineValue(Vd_MathMul, "Multiplicative Multiply"            , "*", VT_OP_MULTIPLICATIVE, VDG_TEXT);
			Vd_MathDiv = idx++;
			idDefArray[Vd_MathDiv]    = defineValue(Vd_MathDiv, "Multiplicative Divide"              , "/", VT_OP_MULTIPLICATIVE, VDG_TEXT);

			vType = VT_OP_URINARY;
			Vd_MathMod = idx++;
			idDefArray[Vd_MathMod]    = defineValue(Vd_MathMod, "Urinary Modulus"                    , "%", VT_OP_URINARY, VDG_TEXT);
			Vd_MathNeg = idx++;
			idDefArray[Vd_MathNeg]    = defineValue(Vd_MathNeg, "Urinary Negative"                   , "-", VT_OP_URINARY, VDG_TEXT);



			vType = VT_STRING;
			Vd_String = idx++;
			idDefArray[Vd_String]    = defineValue(Vd_String, "String"                              , "", VT_STRING, VDG_STRING);


			vType = VT_BOOLEAN;
			Vd_BoolTrue = idx++;
			idDefArray[Vd_BoolTrue]    = defineValue(Vd_BoolTrue, "Boolean True"                      , "True", VT_BOOLEAN, VDG_BOOLEAN, true);
			Vd_BoolFalse = idx++;
			idDefArray[Vd_BoolFalse]    = defineValue(Vd_BoolFalse, "Boolean False"                    , "False", VT_BOOLEAN, VDG_BOOLEAN, true);


			vType = VT_NUMBER;
			Vd_NumInt = idx++;
			// idDefArray[Vd_NumInt]     = defineValue("Number Integer"                     , "", VT_NUM_INTEGER, true);
			idDefArray[Vd_NumInt]     = new DefNumInt(Vd_NumInt, "Number Integer"                     , "", VT_NUM_INTEGER, 1, true);
			Vd_NumDouble = idx++;
			idDefArray[Vd_NumDouble]  = defineValue(Vd_NumDouble, "Number Double"                      , "", VT_NUM_DOUBLE, VDG_NUM_DBL, true);
			Vd_NumFract = idx++;
			idDefArray[Vd_NumFract]   = defineValue(Vd_NumFract, "Number Fraction"                    , "", VT_NUM_FRACTION, VDG_NUM_FRACT, true);


			vType = VT_UNIT;

			Vd_NumUntLenImp = setValue(idx++, "Unit Length Imperial", "", VT_UN_LEN_IMP, true);

				Vd_NumUntLenImp = idx++;
			idDefArray[Vd_NumUntLenImp]  = defineValue(Vd_NumUntLenImp, "Unit Length Imperial"            , "", VT_UN_LEN_IMP, VDG_UNIT, true);
			Vd_NumUntLenMet = idx++;
			idDefArray[Vd_NumUntLenMet]  = defineValue(Vd_NumUntLenMet, "Unit Length Metric"              , "", VT_UN_LEN_MET, VDG_UNIT, true);

			Vd_NumUntAreaImp = idx++;
			idDefArray[Vd_NumUntAreaImp]  = defineValue(Vd_NumUntAreaImp, "Unit Area Imperial"             , "", VT_UN_AREA_IMP, VDG_UNIT, true);
			Vd_NumUntAreaMet = idx++;
			idDefArray[Vd_NumUntAreaMet]  = defineValue(Vd_NumUntAreaMet, "Unit Area Metric"               , "", VT_UN_AREA_MET, VDG_UNIT, true);

			Vd_NumUntVolImp = idx++;
			idDefArray[Vd_NumUntVolImp]  = defineValue(Vd_NumUntVolImp, "Unit Vol Imperial"               , "", VT_UN_VOL_IMP, VDG_UNIT, true);
			Vd_NumUntVolMet = idx++;
			idDefArray[Vd_NumUntVolMet]  = defineValue(Vd_NumUntVolMet, "Unit Vol Metric"                 , "", VT_UN_VOL_MET, VDG_UNIT, true);


			vType = VT_ID_VARIABLE;
			Vd_VarKey = idx++;
			idDefArray[Vd_VarKey]     = defineValue(Vd_VarKey, "Key Variable"                       , "{", VT_ID_VAR_KEY, VDG_VAR);
			Vd_Varible = idx++;
			idDefArray[Vd_Varible]     = defineValue(Vd_Varible, "Variable"                           , "{", VT_ID_VAR_VAR, VDG_VAR);


			vType = VT_ID_FUNCTION;
			Vd_FunBltIn = idx++;
			idDefArray[Vd_FunBltIn]   = defineValue(Vd_FunBltIn, "Function Builtin"                   , "", VT_ID_FUN_INT, VDG_FUNCT);
			Vd_FunLib = idx++;
			idDefArray[Vd_FunLib]     = defineValue(Vd_FunLib, "Function Library"                   , "", VT_ID_FUN_LIB, VDG_FUNCT);
			Vd_FunUsr = idx++;
			idDefArray[Vd_FunUsr]     = defineValue(Vd_FunUsr, "Function User"                      , "", VT_ID_FUN_USR, VDG_FUNCT);


			vType = VT_GROUPING;
			Vd_GrpRef = idx++;
			idDefArray[Vd_GrpRef]     = new DefGrpRef(Vd_GrpRef, "Group Reference", "", VT_GP_REF, 1);
			Vd_GrpBeg = idx++;
			idDefArray[Vd_GrpBeg]     = new DefGrpBeg(Vd_GrpBeg, "Parenthesis Begin", "(", VT_GP_BEG, 2);
			Vd_GrpEnd = idx++;
			idDefArray[Vd_GrpEnd]     = new DefGrpEnd(Vd_GrpEnd, "Parenthesis End", ")", VT_GP_END, 3);


			count = idx;
		}


		private static int setValue(ADefBase2 ab2)
		{
			idDefArray[ab2.Index] = ab2;

			return ab2.Index;
		}


		private static int setValue(int idx, string desc, string tokenStr, ValueType valType, bool isNumeric = false)
		{
			idDefArray[idx] = defineValue(idx, desc, tokenStr, valType, VDG_TEXT, isNumeric);

			return idx;
		}

		private static int id = 0;
		private static int seq = 0;
		private static ValueType vType;
		private static ValueType vTypePrior = VT_DEFAULT;

		private static ADefBase2 defineValue(    int index, string desc, string tokenStr, ValueType valType, ValueDataGroup @group, bool isNumeric = false)
		{
			if (vType != vTypePrior)
			{
				seq = 0;
				vTypePrior = vType;
			}

			return new ValDef(index, desc, tokenStr, valType, group, id++,/* seq,*/ isNumeric);
		}
	}
}