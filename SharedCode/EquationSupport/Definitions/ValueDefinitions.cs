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
	public class ValueDefinitions : DefinitionBase<ValueDef>
	{
		private const int MAX_TOKENS = 40;

		private static readonly Lazy<ValueDefinitions> instance =
		new Lazy<ValueDefinitions>(()=> new ValueDefinitions());

		static ValueDefinitions() {Init(); }

		private ValueDefinitions() : base() { }

		public static ValueDefinitions VdefInst => instance.Value;

		public override ValueDef Invalid => 
			new ValueDef("Invalid", null, VT_INVALID, (int) VT_INVALID, (int) VT_INVALID, -1);
		public override ValueDef Default => 
			new ValueDef("Default", null, VT_DEFAULT, (int) VT_DEFAULT, (int) VT_DEFAULT, -1);

		public ValueDef[] VDefDefs => idDefArray;

		public ValueDef this[int idx] => VDefDefs[idx];

		public string ValueStr(int idx) => VDefDefs[idx].ValueStr;

		// protected override void Initialize() { }

		public static int Vd_Assignment;
		public static int Vd_LogOr;
		public static int Vd_LogAnd;
		public static int Vd_LogEq;
		public static int Vd_LogInEq;
		public static int Vd_RelLt;
		public static int Vd_RelLtEq;
		public static int Vd_RelGt;
		public static int Vd_RelGtEq;
		public static int Vd_AddStr;
		public static int Vd_MathAdd;
		public static int Vd_MathSubt;
		public static int Vd_MathMul;
		public static int Vd_MathDiv;
		public static int Vd_MathMod;
		public static int Vd_MathNeg;
		public static int Vd_FunBltIn;
		public static int Vd_FunLib;
		public static int Vd_FunUsr;
		public static int Vd_VarKey;
		public static int Vd_Varible;
		public static int Vd_Boolean;
		public static int Vd_NumInt;
		public static int Vd_NumDouble;
		public static int Vd_NumFract;
		public static int Vd_NumUntImp;
		public static int Vd_NumUntMet;
		public static int Vd_GrpRef;
		public static int Vd_GrpBeg;
		public static int Vd_GrpEnd;
		
		private static void Init()
		{
			idDefArray = new ValueDef[MAX_TOKENS];

			int idx = 1;

			vType = VT_ASSIGNMENT;
			Vd_Assignment = idx++;
			idDefArray[Vd_Assignment] = DefineValue("Assignment"                      , "=");

			vType = VT_OP_LOGICAL;
			Vd_LogOr = idx++;
			idDefArray[Vd_LogOr]      = DefineValue("Logical Or"                      , "<or>" );
			Vd_LogAnd = idx++;
			idDefArray[Vd_LogAnd]     = DefineValue("Logical And"                     , "<and>");
			Vd_LogEq = idx++;
			idDefArray[Vd_LogEq]      = DefineValue("Logical Equality"                , "=="   );
			Vd_LogInEq = idx++;
			idDefArray[Vd_LogInEq]    = DefineValue("Logical Inequality"              , "!="   );

			vType = VT_OP_RELATIONAL;
			Vd_RelLt = idx++;
			idDefArray[Vd_RelLt]      = DefineValue("Relational Less Than"            , "<" );
			Vd_RelLtEq = idx++;
			idDefArray[Vd_RelLtEq]    = DefineValue("Relational Less Than or Equal"   , "<=");
			Vd_RelGt = idx++;
			idDefArray[Vd_RelGt]      = DefineValue("Relational Greater Than"         , ">" );
			Vd_RelGtEq = idx++;
			idDefArray[Vd_RelGtEq]    = DefineValue("Relational Greater Than or Equal", ">=");

			vType = VT_OP_STRING;
			Vd_AddStr = idx++;
			idDefArray[Vd_AddStr]     = DefineValue("String Addition"                 , "&");

			vType = VT_OP_ADDITIVE;
			Vd_MathAdd = idx++;
			idDefArray[Vd_MathAdd]    = DefineValue("Additive Addition"               , "+");
			Vd_MathSubt = idx++;
			idDefArray[Vd_MathSubt]   = DefineValue("Additive Subtraction"            , "-");

			vType = VT_OP_MULTIPLICATIVE;
			Vd_MathMul = idx++;
			idDefArray[Vd_MathMul]    = DefineValue("Multiplicative Multiply"         , "*");
			Vd_MathDiv = idx++;
			idDefArray[Vd_MathDiv]    = DefineValue("Multiplicative Divide"           , "/");

			vType = VT_OP_URINARY;
			Vd_MathMod = idx++;
			idDefArray[Vd_MathMod]    = DefineValue("Urinary Modulus"                 , "%");
			Vd_MathNeg = idx++;
			idDefArray[Vd_MathNeg]    = DefineValue("Urinary Negative"                , "-");

			vType = VT_BOOLEAN;
			Vd_Boolean = idx++;
			idDefArray[Vd_Boolean]    = DefineValue("Boolean"                         , "");

			vType = VT_NUM_INTEGER;
			Vd_NumInt = idx++;
			idDefArray[Vd_NumInt]     = DefineValue("Number Integer"                  , "");

			vType = VT_NUM_DOUBLE;
			Vd_NumDouble = idx++;
			idDefArray[Vd_NumDouble]  = DefineValue("Number Double"                   , "");

			vType = VT_NUM_FRACTION;
			Vd_NumFract = idx++;
			idDefArray[Vd_NumFract]   = DefineValue("Number Fraction"                 , "");

			vType = VT_UN_LEN_IMP;
			Vd_NumUntImp = idx++;
			idDefArray[Vd_NumUntImp]  = DefineValue("Unit Length Imperial"            , "");

			vType = VT_UN_LEN_MET;
			Vd_NumUntMet = idx++;
			idDefArray[Vd_NumUntMet]  = DefineValue("Unit Length Metric"              , "");

			vType = VT_ID_FUN_INT;
			Vd_FunBltIn = idx++;
			idDefArray[Vd_FunBltIn]   = DefineValue("Function Builtin"                , "");

			vType = VT_ID_FUN_LIB;
			Vd_FunLib = idx++;
			idDefArray[Vd_FunLib]     = DefineValue("Function Library"                , "");

			vType = VT_ID_FUN_USR;
			Vd_FunUsr = idx++;
			idDefArray[Vd_FunUsr]     = DefineValue("Function User"                   , "");

			vType = VT_ID_VAR_KEY;
			Vd_VarKey = idx++;
			idDefArray[Vd_VarKey]     = DefineValue("Key Variable"                    , "{");

			vType = VT_GROUPING;
			Vd_GrpRef = idx++;
			idDefArray[Vd_GrpRef]     = DefineValue("Group Reference"                 , "");
			Vd_GrpBeg = idx++;
			idDefArray[Vd_GrpBeg]     = DefineValue("Parenthesis Begin"               , "(");
			Vd_GrpEnd = idx++;
			idDefArray[Vd_GrpEnd]     = DefineValue("Parenthesis End"                 , ")");


			count = idx;
		}

		private static int id = 0;
		private static int seq = 0;
		private static ValueType vType;
		private static ValueType vTypePrior = VT_DEFAULT;

		private static ValueDef DefineValue(string desc, string tokenStr)
		{
			if (vType != vTypePrior)
			{
				seq = 0;
			}

			return new ValueDef(desc, tokenStr, vType, id++, seq, (int) vType + seq++);
		}
	}
}