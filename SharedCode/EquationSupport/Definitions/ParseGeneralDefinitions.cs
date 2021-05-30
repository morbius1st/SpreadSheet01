#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static SharedCode.EquationSupport.Definitions.ParseGroupGeneral;
using static SharedCode.EquationSupport.Definitions.ValueType;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;
using static SharedCode.EquationSupport.Definitions.VariableDefinitions;

#endregion

// user name: jeffs
// created:   5/17/2021 6:40:33 PM

namespace SharedCode.EquationSupport.Definitions
{
	public class ParseGeneralDefinitions : ADefinitionBase<ParseGen>
	{
		private const int MAX_TOKENS = 20;
		
		private static readonly Lazy<ParseGeneralDefinitions> instance =
			new Lazy<ParseGeneralDefinitions>(()=> new ParseGeneralDefinitions());

		static ParseGeneralDefinitions()
		{
			Init();
		}

		public static ParseGeneralDefinitions PgDefInst => instance.Value;

		public override ParseGen Invalid => new ParseGen("Invalid", null, VT_INVALID,    PGG_INVALID, null, false);

		// public override ParseGen Default => new ParseGen("Default", null, VT_DEFAULT, PGG_DEFAULT, (int) PGG_DEFAULT, false);
		public override ParseGen Default => idDefArray[0];

		public static ADefBase2 Classify(string test, string value)
		{

			for (int i = 0; i < idDefArray.Length; i++)
			{
				ParseGen pg = idDefArray[i];

				if (pg == null) continue;

				if (!pg.Equals(test) || !pg.IsGood) continue;

				for (int j = 0; j < pg.aDefBase2.Count; j++)
				{
					if (pg.aDefBase2[j].Equals(value)) return pg.aDefBase2[j];
				}
			}

			return (ADefBase2) ADefBase.Invalid;
		}

		private static void Init()
		{
			idDefArray = new ParseGen[MAX_TOKENS];

			int idx = 1;
			int id = 1;

			// ParseItem pi = new ParseItem(ValueDefinitions.VdefInst[1], "Assignment", "=", VT_STRING, PGG_ASSIGNMENT, id++);

			idDefArray[0] = new ParseGen("Default"                    , null  , VT_DEFAULT, PGG_DEFAULT  , null, false);
			idDefArray[idx++] = new ParseGen("Invalid"                , "x1"  , VT_STRING, PGG_INVALID   , null, false);
			idDefArray[idx++] = new ParseGen("Word"                   , "w1"  , VT_STRING, PGG_INVALID   , null, false);
			idDefArray[idx++] = new ParseGen("Assignment"             , "eq"   , VT_STRING, PGG_ASSIGNMENT ,
				new [] {ValDefInst[Vd_Assignment]} );

			idDefArray[idx++] = new ParseGen("Operator"               , "op1" , VT_STRING, PGG_OPERATOR  ,
				new []
				{
					ValDefInst[Vd_LogOr],
					ValDefInst[Vd_LogAnd],
					ValDefInst[Vd_LogEq],
					ValDefInst[Vd_LogInEq],
					ValDefInst[Vd_RelLt],
					ValDefInst[Vd_RelLtEq],
					ValDefInst[Vd_RelGt],
					ValDefInst[Vd_AddStr],
					ValDefInst[Vd_MathAdd],
					ValDefInst[Vd_MathSubt],
					ValDefInst[Vd_MathMul],
					ValDefInst[Vd_MathDiv],
					ValDefInst[Vd_MathMod],
					ValDefInst[Vd_MathNeg],
				}
				);

			idDefArray[idx++] = new ParseGen("String"                 , "s1"  , VT_STRING, PGG_STRING    ,
				new [] {ValDefInst[Vd_String]} );

			idDefArray[idx++] = new ParseGen("Boolean"                , "b1"   , VT_STRING, PGG_BOOLEAN   ,
				new []
				{
					ValDefInst[Vd_BoolTrue],
					ValDefInst[Vd_BoolFalse]
				}
				);

			idDefArray[idx++] = new ParseGen("Number Integer"         , "n1"  , VT_STRING, PGG_NUMBER    ,
				new [] {ValDefInst[Vd_NumInt]} );

			idDefArray[idx++] = new ParseGen("Number Double"          , "d1"  , VT_STRING, PGG_NUMBER    ,
				new [] {ValDefInst[Vd_NumDouble]} );

			idDefArray[idx++] = new ParseGen("Number Fraction"        , "fr1" , VT_STRING, PGG_NUMBER    ,
				new [] {ValDefInst[Vd_NumFract]} );

			idDefArray[idx++] = new ParseGen("Number Length"          , "l1"  , VT_STRING, PGG_UNIT      ,
				new [] {ValDefInst[Vd_NumUntLenImp]} );

			idDefArray[idx++] = new ParseGen("Function"               , "fn1" , VT_STRING, PGG_FUNCTION  ,
				new [] {ValDefInst[Vd_FunUsr]} );


			// int a = Pvd_XcellAddr;
			// VarDef c = VariableDefinitions.idDefArray[Pvd_XcellAddr];
			idDefArray[idx++] = new ParseGen("Variable Special"       , "v1"  , VT_STRING, PGG_VARIABLE  ,
				new []
				{
					VarDefInst[Pvd_XcellAddr],
				}
				);

			idDefArray[idx++] = new ParseGen("Variable Special"       , "v2"  , VT_STRING, PGG_VARIABLE  ,
				new []
				{
					VarDefInst[Pvd_SysVar],
					VarDefInst[Pvd_RvtParam],
					VarDefInst[Pvd_PrjParam],
					VarDefInst[Pvd_GblParam],
					VarDefInst[Pvd_LabelName],
				}
				);

			idDefArray[idx++] = new ParseGen("Variable Address"       , "v3"  , VT_STRING, PGG_VARIABLE  ,
				new [] {ValDefInst[Vd_Varible]} );

			idDefArray[idx++] = new ParseGen("Group Reference"        , "ref" , VT_STRING, PGG_GROUP_REF ,
				new [] {ValDefInst[Vd_GrpRef]} );

			idDefArray[idx++] = new ParseGen("Group Parenthesis Begin", "pb" , VT_STRING, PGG_GROUPING  ,
				new [] {ValDefInst[Vd_GrpBeg]} );

			idDefArray[idx++] = new ParseGen("Group Parenthesis End"  , "pe" , VT_STRING, PGG_GROUPING  ,
				new [] {ValDefInst[Vd_GrpEnd]} );

			count = idx;
		}
	}
}