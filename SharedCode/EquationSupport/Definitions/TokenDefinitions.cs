#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.TokenClass;

#endregion

// user name: jeffs
// created:   5/17/2021 6:39:19 PM

namespace SharedCode.EquationSupport.Definitions
{
	public class ValueDefinitions : DefinitionBase<TokenDef>
	{
		private const int MAX_TOKENS = 40;

		private static readonly Lazy<ValueDefinitions> instance =
		new Lazy<ValueDefinitions>(()=> new ValueDefinitions());

		private ValueDefinitions() {}

		public static ValueDefinitions TdInst => instance.Value;

		public override TokenDef Invalid => new TokenDef("Invalid", null, -1, 0, -1);

		public TokenDef[] TokenDefs => tokenArray;

		protected override void Initialize()
		{
			tokenArray = new TokenDef[MAX_TOKENS];

			Init();
		}

		public int Td_Assignment;
		public int Td_LogOr;
		public int Td_LogAnd;
		public int Td_LogEq;
		public int Td_LogInEq;
		public int Td_RelLt;
		public int Td_RelLtEq;
		public int Td_RelGt;
		public int Td_RelGtEq;
		public int Td_AddStr;
		public int Td_MathAdd;
		public int Td_MathSubt;
		public int Td_MathMul;
		public int Td_MathDiv;
		public int Td_MathMod;
		public int Td_MathNeg;
		public int Td_FunBltIn;
		public int Td_FunLib;
		public int Td_FunUsr;
		public int Td_VarKey;
		public int Td_Varible;
		public int Td_Boolean;
		public int Td_NumInt;
		public int Td_NumDouble;
		public int Td_NumFract;
		public int Td_NumUntImp;
		public int Td_NumUntMet;
		public int Td_GrpBeg;
		public int Td_GrpEnd;
		

		private void Init()
		{
			int idx = 0;

			tClass = TC_ASSIGNMENT;
			Td_Assignment = idx++;
			tokenArray[Td_Assignment] = DefineToken("Assignment"                      , "=");

			tClass = TC_OP_LOGICAL;
			Td_LogOr = idx++;
			tokenArray[Td_LogOr]      = DefineToken("Logical Or"                      , "<or>" );
			Td_LogAnd = idx++;
			tokenArray[Td_LogAnd]     = DefineToken("Logical And"                     , "<and>");
			Td_LogEq = idx++;
			tokenArray[Td_LogEq]      = DefineToken("Logical Equality"                , "=="   );
			Td_LogInEq = idx++;
			tokenArray[Td_LogInEq]    = DefineToken("Logical Inequality"              , "!="   );

			tClass = TC_OP_RELATIONAL;
			Td_RelLt = idx++;
			tokenArray[Td_RelLt]      = DefineToken("Relational Less Than"            , "<" );
			Td_RelLtEq = idx++;
			tokenArray[Td_RelLtEq]    = DefineToken("Relational Less Than or Equal"   , "<=");
			Td_RelGt = idx++;
			tokenArray[Td_RelGt]      = DefineToken("Relational Greater Than"         , ">" );
			Td_RelGtEq = idx++;
			tokenArray[Td_RelGtEq]    = DefineToken("Relational Greater Than or Equal", ">=");

			tClass = TC_OP_STRING;
			Td_AddStr = idx++;
			tokenArray[Td_AddStr]     = DefineToken("String Addition"                 , "&");

			tClass = TC_OP_ADDITIVE;
			Td_MathAdd = idx++;
			tokenArray[Td_MathAdd]    = DefineToken("Additive Addition"               , "+");
			Td_MathSubt = idx++;
			tokenArray[Td_MathSubt]   = DefineToken("Additive Subtraction"            , "-");

			tClass = TC_OP_MULTIPLICATIVE;
			Td_MathMul = idx++;
			tokenArray[Td_MathMul]    = DefineToken("Multiplicative Multiply"         , "*");
			Td_MathDiv = idx++;
			tokenArray[Td_MathDiv]    = DefineToken("Multiplicative Divide"           , "/");

			tClass = TC_OP_URINARY;
			Td_MathMod = idx++;
			tokenArray[Td_MathMod]    = DefineToken("Urinary Modulus"                 , "%");
			Td_MathNeg = idx++;
			tokenArray[Td_MathNeg]    = DefineToken("Urinary Negative"                , "-");

			tClass = TC_FUNCT_BUILTIN;
			Td_FunBltIn = idx++;
			tokenArray[Td_FunBltIn]   = DefineToken("Function Builtin"                , "");

			tClass = TC_FUNCT_LIBRARY;
			Td_FunLib = idx++;
			tokenArray[Td_FunLib]     = DefineToken("Function Library"                , "");

			tClass = TC_FUNCT_USER;
			Td_FunUsr = idx++;
			tokenArray[Td_FunUsr]     = DefineToken("Function User"                   , "");

			tClass = TC_VAR_KEY;
			Td_VarKey = idx++;
			tokenArray[Td_VarKey]     = DefineToken("Key Variable"                    , "{");

			tClass = TC_BOOLEAN;
			Td_Boolean = idx++;
			tokenArray[Td_Boolean]    = DefineToken("Boolean"                         , "");

			tClass = TC_NUM_INT;
			Td_NumInt = idx++;
			tokenArray[Td_NumInt]     = DefineToken("Number Integer"                  , "");

			tClass = TC_NUM_DOUBLE;
			Td_NumDouble = idx++;
			tokenArray[Td_NumDouble]  = DefineToken("Number Double"                   , "");

			tClass = TC_NUM_FRACTION;
			Td_NumFract = idx++;
			tokenArray[Td_NumFract]   = DefineToken("Number Fraction"                 , "");

			tClass = TC_UNIT_LEN_IMP;
			Td_NumUntImp = idx++;
			tokenArray[Td_NumUntImp]  = DefineToken("Unit Length Imperial"            , "");

			tClass = TC_UNIT_LEN_METRIC;
			Td_NumUntMet = idx++;
			tokenArray[Td_NumUntMet]  = DefineToken("Unit Length Metric"              , "");

			tClass = TC_GROUPING;
			Td_GrpBeg = idx++;
			tokenArray[Td_GrpBeg]     = DefineToken("Parenthesis Begin"               , "(");
			Td_GrpEnd = idx++;
			tokenArray[Td_GrpEnd]     = DefineToken("Parenthesis End"                 , ")");


			count = idx;
		}

		private int id = 0;
		private int seq = 0;
		private TokenClass tClass;
		private TokenClass tClassPrior = TC_UNASSIGNED;

		private TokenDef DefineToken(string desc, string tokenStr)
		{
			if (tClass != tClassPrior)
			{
				seq = 0;
			}

			return new TokenDef(desc, tokenStr, id++, seq, (int) tClass + seq++);
		}
	}
}