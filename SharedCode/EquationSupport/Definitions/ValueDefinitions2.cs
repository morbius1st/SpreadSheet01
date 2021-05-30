#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedCode.EquationSupport.Definitions;
// using static SharedCode.EquationSupport.Definitions.TokenClass;
using static SharedCode.EquationSupport.Definitions.ValueType;
// using static SharedCode.EquationSupport.Definitions.ValueDataType;

#endregion

// user name: jeffs
// created:   5/17/2021 6:39:19 PM

namespace SharedCode.EquationSupport.Definitions
{
	public class ValueDefinitions2 : ADefinitionBase<ADefBase2>
	{
		private const int MAX_TOKENS = 50;

		private static readonly Lazy<ValueDefinitions2> instance =
		new Lazy<ValueDefinitions2>(()=> new ValueDefinitions2());

		static ValueDefinitions2() {Init(); }

		// private ValueDefinitions() : base() { }

		public static ValueDefinitions2 ValDefInst => instance.Value;

		public override ADefBase2 Invalid => 
			new DefValue("Invalid", null, VT_INVALID, (int) VT_INVALID, (int) VT_INVALID);
		public override ADefBase2 Default => 
			new DefValue("Default", null, VT_DEFAULT, (int) VT_DEFAULT, (int) VT_DEFAULT);

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
			idDefArray = new DefValue[MAX_TOKENS];

			int idx = 0;

			
			vType = VT_INVALID;
			Vd_Invalid = idx++;
			idDefArray[Vd_Invalid] = defineValue("Invalid"                               , "", VT_INVALID);
			
			vType = VT_DEFAULT;
			Vd_Assignment = idx++;
			idDefArray[Vd_Default] = defineValue("Default"                               , "", VT_DEFAULT);


			vType = VT_ASSIGNMENT;
			Vd_Assignment = idx++;
			idDefArray[Vd_Assignment] = defineValue("Assignment"                         , "=", VT_ASSIGNMENT);

			vType = VT_OP_LOGICAL;
			Vd_LogOr = idx++;
			idDefArray[Vd_LogOr]      = defineValue("Logical Or"                         , "<or>", VT_OP_LOGICAL );
			Vd_LogAnd = idx++;
			idDefArray[Vd_LogAnd]     = defineValue("Logical And"                        , "<and>", VT_OP_LOGICAL);
			Vd_LogEq = idx++;
			idDefArray[Vd_LogEq]      = defineValue("Logical Equality"                   , "==", VT_OP_LOGICAL   );
			Vd_LogInEq = idx++;
			idDefArray[Vd_LogInEq]    = defineValue("Logical Inequality"                 , "!=", VT_OP_LOGICAL   );
			Vd_LogNot = idx++;
			idDefArray[Vd_LogNot]    = defineValue("Logical Not"                         , "!", VT_OP_LOGICAL   );

			vType = VT_OP_RELATIONAL;
			Vd_RelLt = idx++;
			idDefArray[Vd_RelLt]      = defineValue("Relational Less Than"               , "<", VT_OP_RELATIONAL );
			Vd_RelLtEq = idx++;
			idDefArray[Vd_RelLtEq]    = defineValue("Relational Less Than or Equal"      , "<=", VT_OP_RELATIONAL);
			Vd_RelGt = idx++;
			idDefArray[Vd_RelGt]      = defineValue("Relational Greater Than"            , ">", VT_OP_RELATIONAL );
			Vd_RelGtEq = idx++;
			idDefArray[Vd_RelGtEq]    = defineValue("Relational Greater Than or Equal"   , ">=", VT_OP_RELATIONAL);

			vType = VT_OP_STRING;
			Vd_AddStr = idx++;
			idDefArray[Vd_AddStr]     = defineValue("String Addition"                    , "&", VT_OP_STRING);

			vType = VT_OP_ADDITIVE;
			Vd_MathAdd = idx++;
			idDefArray[Vd_MathAdd]    = defineValue("Additive Addition"                  , "+", VT_OP_ADDITIVE);
			Vd_MathSubt = idx++;
			idDefArray[Vd_MathSubt]   = defineValue("Additive Subtraction"               , "-", VT_OP_ADDITIVE);

			vType = VT_OP_MULTIPLICATIVE;
			Vd_MathMul = idx++;
			idDefArray[Vd_MathMul]    = defineValue("Multiplicative Multiply"            , "*", VT_OP_MULTIPLICATIVE);
			Vd_MathDiv = idx++;
			idDefArray[Vd_MathDiv]    = defineValue("Multiplicative Divide"              , "/", VT_OP_MULTIPLICATIVE);

			vType = VT_OP_URINARY;
			Vd_MathMod = idx++;
			idDefArray[Vd_MathMod]    = defineValue("Urinary Modulus"                    , "%", VT_OP_URINARY);
			Vd_MathNeg = idx++;
			idDefArray[Vd_MathNeg]    = defineValue("Urinary Negative"                   , "-", VT_OP_URINARY);



			vType = VT_STRING;
			Vd_String = idx++;
			idDefArray[Vd_String]    = defineValue("String"                              , "", VT_STRING, true);


			vType = VT_BOOLEAN;
			Vd_BoolTrue = idx++;
			idDefArray[Vd_BoolTrue]    = defineValue("Boolean True"                      , "True", VT_BOOLEAN, true);
			Vd_BoolFalse = idx++;
			idDefArray[Vd_BoolFalse]    = defineValue("Boolean False"                    , "False", VT_BOOLEAN, true);


			vType = VT_NUMBER;
			Vd_NumInt = idx++;
			idDefArray[Vd_NumInt]     = defineValue("Number Integer"                     , "", VT_NUM_INTEGER, true);
			Vd_NumDouble = idx++;
			idDefArray[Vd_NumDouble]  = defineValue("Number Double"                      , "", VT_NUM_DOUBLE, true);
			Vd_NumFract = idx++;
			idDefArray[Vd_NumFract]   = defineValue("Number Fraction"                    , "", VT_NUM_FRACTION, true);


			vType = VT_UNIT;

			Vd_NumUntLenImp = setValue(idx++, "Unit Length Imperial", "", VT_UN_LEN_IMP, true);

				Vd_NumUntLenImp = idx++;
			idDefArray[Vd_NumUntLenImp]  = defineValue("Unit Length Imperial"            , "", VT_UN_LEN_IMP, true);
			Vd_NumUntLenMet = idx++;
			idDefArray[Vd_NumUntLenMet]  = defineValue("Unit Length Metric"              , "", VT_UN_LEN_MET, true);

			Vd_NumUntAreaImp = idx++;
			idDefArray[Vd_NumUntAreaImp]  = defineValue("Unit Area Imperial"             , "", VT_UN_AREA_IMP, true);
			Vd_NumUntAreaMet = idx++;
			idDefArray[Vd_NumUntAreaMet]  = defineValue("Unit Area Metric"               , "", VT_UN_AREA_MET, true);

			Vd_NumUntVolImp = idx++;
			idDefArray[Vd_NumUntVolImp]  = defineValue("Unit Vol Imperial"               , "", VT_UN_VOL_IMP, true);
			Vd_NumUntVolMet = idx++;
			idDefArray[Vd_NumUntVolMet]  = defineValue("Unit Vol Metric"                 , "", VT_UN_VOL_MET, true);


			vType = VT_ID_VARIABLE;
			Vd_VarKey = idx++;
			idDefArray[Vd_VarKey]     = defineValue("Key Variable"                       , "{", VT_ID_VAR_KEY);
			Vd_Varible = idx++;
			idDefArray[Vd_Varible]     = defineValue("Variable"                           , "{", VT_ID_VAR_VAR);


			vType = VT_ID_FUNCTION;
			Vd_FunBltIn = idx++;
			idDefArray[Vd_FunBltIn]   = defineValue("Function Builtin"                   , "", VT_ID_FUN_INT);
			Vd_FunLib = idx++;
			idDefArray[Vd_FunLib]     = defineValue("Function Library"                   , "", VT_ID_FUN_LIB);
			Vd_FunUsr = idx++;
			idDefArray[Vd_FunUsr]     = defineValue("Function User"                      , "", VT_ID_FUN_USR);


			vType = VT_GROUPING;
			Vd_GrpRef = idx++;
			idDefArray[Vd_GrpRef]     = defineValue("Group Reference"                    , "", VT_GP_REF);
			Vd_GrpBeg = idx++;
			idDefArray[Vd_GrpBeg]     = defineValue("Parenthesis Begin"                  , "(", VT_GP_BEG);
			Vd_GrpEnd = idx++;
			idDefArray[Vd_GrpEnd]     = defineValue("Parenthesis End"                    , ")", VT_GP_END);


			count = idx;
		}

		private static int setValue(int idx, string desc, string tokenStr, ValueType valType, bool isNumeric = false)
		{
			idDefArray[idx] = defineValue(desc, tokenStr, valType, isNumeric);

			return idx;
		}

		private static int id = 0;
		private static int seq = 0;
		private static ValueType vType;
		private static ValueType vTypePrior = VT_DEFAULT;

		private static DefValue defineValue(string desc, string tokenStr, ValueType valType, bool isNumeric = false)
		{
			if (vType != vTypePrior)
			{
				seq = 0;
				vTypePrior = vType;
			}

			return new DefValue(desc, tokenStr, valType, id++, seq, isNumeric);
		}
	}
}