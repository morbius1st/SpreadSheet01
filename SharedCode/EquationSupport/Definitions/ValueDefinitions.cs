#region + Using Directives

using System;
using SharedCode.EquationSupport.Definitions.ValueDefs.FromBase;
using SharedCode.EquationSupport.Definitions.ValueDefs.FromString;
using SharedCode.EquationSupport.Definitions.ValueDefs.FromVar;
using static SharedCode.EquationSupport.Definitions.ValueType;
using static SharedCode.EquationSupport.Definitions.ParseGroupVar;

#endregion

// user name: jeffs
// created:   5/17/2021 6:39:19 PM

namespace SharedCode.EquationSupport.Definitions
{
	public class ValueDefinitions : ADefinitionBase<AValDefBase>
	{
		private const int MAX_TOKENS = 70;
		// private static ValueType vType;

		private static readonly Lazy<ValueDefinitions> instance =
		new Lazy<ValueDefinitions>(() => new ValueDefinitions());

		static ValueDefinitions() { Init(); }

		// private ValueDefinitions() : base() { }

		public static ValueDefinitions ValDefInst => instance.Value;

		public override AValDefBase Invalid =>
			new ValDefInvalid(-1, "Invalid", null, VT_INVALID, (int)VT_INVALID);
		public override AValDefBase Default =>
			new ValDefDefault(0, "Default", null, VT_DEFAULT, (int)VT_DEFAULT);

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
		public static int Vd_AddText;
		// operator - additive
		public static int Vd_MathAdd;
		public static int Vd_MathSubt;
		// operator - multiply
		public static int Vd_MathMul;
		public static int Vd_MathDiv;
		public static int Vd_MathPwr;
		public static int Vd_MathMod;
		// operator - urniary
		public static int Vd_MathNeg;

		// string
		public static int Vd_Text;

		// boolean
		public static int Vd_BoolTrue;
		public static int Vd_BoolFalse;
		public static int Vd_BoolNull;

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
		public static int Vd_FunStrText;
		public static int Vd_FunBoolean;
		public static int Vd_FunInteger;
		public static int Vd_FunDouble;



		// identifier - variable - key
		public static int Vd_VarXlAddr;
		public static int Vd_VarSysVar;
		public static int Vd_VarRvtParam;
		public static int Vd_VarPrjParam;
		public static int Vd_VarGblParam;
		public static int Vd_VarLblName;
		public static int Vd_VarKey;

		// identifier - variable
		public static int Vd_Varible;

		// grouping
		public static int Vd_GrpRef;
		public static int Vd_GrpBeg;
		public static int Vd_GrpEnd;
		public static int Vd_FnGrpBeg;
		public static int Vd_FnGrpEnd;
		public static int Vd_PrnBeg;
		public static int Vd_PrnEnd;
		public static int Vd_FunctArgSep;

		private static void Init()
		{
			idDefArray = new AValDefBase[MAX_TOKENS];

			int idx = 0;

			// must include all valye types
			// vType            = VT_ASSIGNMENT;
			Vd_Assignment    = setValue(new ValDefAssignment(idx++    , "Assignment"                      , "="    , VT_ASSIGNMENT, 1));

			// vType            = VT_OP_LOGICALMATH;
			Vd_LogOr         = setValue(new ValDefOpMathLogOr(idx++   , "Logical Or"                      , "<or>" , VT_OP_LOGICALMATH, 1));
			Vd_LogAnd        = setValue(new ValDefOpMathLogAnd(idx++  , "Logical And"                     , "<and>", VT_OP_LOGICALMATH, 2));
			Vd_LogEq         = setValue(new ValDefOpMathLogEq(idx++   , "Logical Equality"                , "=="   , VT_OP_LOGICALMATH, 3));
			Vd_LogInEq       = setValue(new ValDefOpMathLogInEq(idx++ , "Logical Inequality"              , "!="   , VT_OP_LOGICALMATH, 4));
			Vd_LogNot        = setValue(new ValDefOpMathLogNot(idx++  , "Logical Not"                     , "!"    , VT_OP_LOGICALMATH, 5));

			// vType            = VT_OP_RELATIONALMATH;
			Vd_RelLt         = setValue(new ValDefOpMathRelLt(idx++   , "Relational Less Than"            , "<"    , VT_OP_RELATIONALMATH, 1));
			Vd_RelLtEq       = setValue(new ValDefOpMathRelLte(idx++  , "Relational Less Than or Equal"   , "<="   , VT_OP_RELATIONALMATH, 2));
			Vd_RelGt         = setValue(new ValDefOpMathRelGt(idx++   , "Relational Greater Than"         , ">"    , VT_OP_RELATIONALMATH, 3));
			Vd_RelGtEq       = setValue(new ValDefOpMathRelGte(idx++  , "Relational Greater Than or Equal", ">="   , VT_OP_RELATIONALMATH, 4));

			// vType            = VT_OP_TEXT;
			Vd_AddText       = setValue(new ValDefOpTextAdd(idx++      , "Text Addition"                 , "&"    , VT_OP_TEXT, 1));

			// vType            = VT_OP_ADDITIVE;
			Vd_MathAdd       = setValue(new ValDefOpMathAdd(idx++     , "Additive Addition"               , "+"    , VT_OP_ADDITIVE, 1));
			Vd_MathSubt      = setValue(new ValDefOpMathSubt(idx++    , "Additive Subtraction"            , "-"    , VT_OP_ADDITIVE, 2));

			// vType            = VT_OP_MULTIPLICATIVE;
			Vd_MathMul       = setValue(new ValDefOpMathMul(idx++     , "Multiplicative Multiply"         , "*"    , VT_OP_MULTIPLICATIVE, 1));
			Vd_MathDiv       = setValue(new ValDefOpMathDiv(idx++     , "Multiplicative Divide"           , "/"    , VT_OP_MULTIPLICATIVE, 2));
			Vd_MathPwr       = setValue(new ValDefOpMathPower(idx++   , "Multiplicative Power"            , "^"    , VT_OP_MULTIPLICATIVE, 3));
			Vd_MathMod       = setValue(new ValDefOpMathMod(idx++     , "Multiplicative Modulus"          , "%"    , VT_OP_MULTIPLICATIVE, 4));

			// vType            = VT_OP_URINARY;
			Vd_MathNeg       = setValue(new ValDefOpMathNeg(idx++     , "Urinary Negative"                , "-"    , VT_OP_URINARY, 1));

			// vType            = VT_TEXT;
			Vd_Text          = setValue(new ValDefText(idx++            , "Text"                           , ""     , VT_TEXT, 1));

			// vType            = VT_BOOLEAN;
			Vd_BoolTrue      = setValue(new ValDefNumBoolTrue(idx++   , "Boolean True"                    , "True" , VT_BOOLEAN, 1, true));
			Vd_BoolFalse     = setValue(new ValDefNumBoolFalse(idx++  , "Boolean False"                   , "False", VT_BOOLEAN, 2, true));
			Vd_BoolFalse     = setValue(new ValDefNumBoolNull(idx++  , "Boolean Null"                     , "bNull", VT_BOOLEAN, 3, true));

			// vType            = VT_NUMBER;
			Vd_NumInt        = setValue(new ValDefNumInt(idx++        , "Number Integer"                  , ""     , VT_NUM_INTEGER, 1, true));
			Vd_NumDouble     = setValue(new ValDefNumDouble(idx++     , "Number Double"                   , ""     , VT_NUM_DOUBLE, 2, true));
			Vd_NumFract      = setValue(new ValDefNumFract(idx++      , "Number Fraction"                 , ""     , VT_NUM_FRACTION, 3, true));
			
			// see VarDefinitions for variables
			// // vType            = VT_ID_VARIABLE;
			Vd_VarXlAddr     = setValue(new VarDefKeyXlAddr(idx++  , "Variable XlAddr"                 , "{[", "]}", VT_ID_VAR_KEY, PGV_EXCL_ADDR, 1));
			Vd_VarSysVar     = setValue(new VarDefKeySysVar(idx++  , "Variable System Var"             , "{$", "}",  VT_ID_VAR_KEY, PGV_SYS_VAR  , 2));
			Vd_VarRvtParam   = setValue(new VarDefKeyRvtParam(idx++, "Variable Revit Param"            , "{#", "}",  VT_ID_VAR_KEY, PGV_RVT_PARAM, 3));
			Vd_VarPrjParam   = setValue(new VarDefKeyPrjParam(idx++, "Variable Project Param"          , "{%", "}",  VT_ID_VAR_KEY, PGV_PRJ_PARAM, 4));
			Vd_VarGblParam   = setValue(new VarDefKeyGblParam(idx++, "Variable Global Param"           , "{!", "}",  VT_ID_VAR_KEY, PGV_GBL_PARAM, 5));
			Vd_VarLblName    = setValue(new VarDefKeyLblName(idx++ , "Variable Label Param"            , "{@", "}",  VT_ID_VAR_KEY, PGV_LBL_NAME , 6));

			// Vd_VarKey        = setValue(new DefVarKeyString(idx++, "Variable Cells"                    , "{" , "}" , VT_ID_VAR_KEY,  PGV_DEFAULT,10));

			Vd_Varible       = setValue(new ValDefVarStrText(idx++     , "Variable"                        , "{"    , VT_ID_VAR_VAR, 11));


			// vType            = VT_ID_FUNCTION;
			Vd_FunStrText    = setValue(new ValDefFunStrText(idx++     , "Functions Text"                 , ""     , VT_ID_FUN_TXT, 1));
			Vd_FunBoolean    = setValue(new ValDefFunBool(idx++     , "Functions Boolean"                 , ""     , VT_ID_FUN_BOOL,1));
			Vd_FunInteger    = setValue(new ValDefFunNumInt(idx++     , "Functions Integer"               , ""     , VT_ID_FUN_INT, 1));
			Vd_FunDouble     = setValue(new ValDefFunNumDbl(idx++     , "Functions Double"                , ""     , VT_ID_FUN_DBL, 1));

			// vType            = VT_GROUPING;
			Vd_GrpRef        = setValue(new ValDefGrpRef(idx++        , "Group Reference"                 , "<gpr>", VT_GP_REF, 1));
			Vd_GrpBeg        = setValue(new ValDefGrpBeg(idx++        , "Group Begin"                     , "<gpb>", VT_GP_BEG, 2));
			Vd_GrpEnd        = setValue(new ValDefGrpEnd(idx++        , "Group End"                       , "<gpe>", VT_GP_END, 3));
			Vd_FnGrpBeg      = setValue(new ValDefGrpBeg(idx++        , "Function Group Begin"            , "<fgb>", VT_GP_REF, 21));
			Vd_FnGrpEnd      = setValue(new ValDefGrpEnd(idx++        , "Function Group End"              , "<fge>", VT_GP_REF, 22));
			Vd_PrnBeg        = setValue(new ValDefGrpBeg(idx++        , "Parenthesis Begin"               , "("    , VT_PRN_BEG, 51));
			Vd_PrnEnd        = setValue(new ValDefGrpEnd(idx++        , "Parenthesis End"                 , ")"    , VT_PRN_END, 52));
			Vd_FunctArgSep   = setValue(new ValDefGrpArgSep(idx++     , "Argument Separator"              , ","    , VT_GP_ARG_SEP, 71));

			// vType            = VT_UNIT;
			Vd_NumUntLenImp  = setValue(new ValDefUnitLenImp(idx++    , "Unit Length Imperial"            , ""     , VT_UN_LEN_IMP, 1, true));
			Vd_NumUntLenMet  = setValue(new ValDefUnitLenMetric(idx++ , "Unit Length Metric"              , ""     , VT_UN_LEN_MET, 1, true));

			Vd_NumUntAreaImp = setValue(new ValDefUnitAreaImp(idx++   , "Unit Area Imperial"              , ""     , VT_UN_AREA_IMP, 1, true));
			Vd_NumUntAreaMet = setValue(new ValDefUnitAreaMetric(idx++, "Unit Area Metric"                , ""     , VT_UN_AREA_MET, 1, true));

			Vd_NumUntVolImp  = setValue(new ValDefUnitVolImp(idx++    , "Unit Vol Imperial"               , ""     , VT_UN_VOL_IMP, 1, true));
			Vd_NumUntVolMet  = setValue(new ValDefUnitVolMetric(idx++ , "Unit Vol Metric"                 , ""     , VT_UN_VOL_MET, 1, true));

			// vType            = VT_INVALID;
			Vd_Invalid       = setValue(new ValDefInvalid(idx++       , "Invalid"                         , ""     , VT_INVALID, 0));

			// vType            = VT_DEFAULT;
			Vd_Default       = setValue(new ValDefDefault(idx++       , "Default"                         , ""     , VT_DEFAULT, 0));

			count = idx;
		}

		private static int setValue(AValDefBase ab2)
		{
			idDefArray[ab2.Index] = ab2;

			return ab2.Index;
		}

		// private static int setValue(int idx, string desc, string tokenStr, ValueType valType, ValueDataGroup group, bool isNumeric = false)
		// {
		// 	idDefArray[idx] = defineValue(idx, desc, tokenStr, valType, group, isNumeric);
		//
		// 	return idx;
		// }

		// private static int id = 0;
		// private static int seq = 0;
		//
		// private static ValueType vTypePrior = VT_DEFAULT;
		//
		// private static AValDefBase defineValue(int index, string desc, string tokenStr, ValueType valType, ValueDataGroup @group, bool isNumeric = false)
		// {
		// 	if (vType != vTypePrior)
		// 	{
		// 		seq = 0;
		// 		vTypePrior = vType;
		// 	}
		//
		// 	return new AValDefBase(index, desc, tokenStr, valType, group, id++, isNumeric);
		// }
	}
}