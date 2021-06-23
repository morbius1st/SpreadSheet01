#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharedCode.EquationSupport.TokenSupport;
using static SharedCode.EquationSupport.Definitions.ValueDataGroup;
using static SharedCode.EquationSupport.Definitions.ValueType;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;
// using static SharedCode.EquationSupport.Definitions.VariableDefinitions;

#endregion

// user name: jeffs
// created:   5/17/2021 6:40:33 PM

namespace SharedCode.EquationSupport.Definitions
{
	public class ParseDefinitions : ADefinitionBase<ParseDef>
	{
		private const int MAX_TOKENS = 25;

		private static readonly Lazy<ParseDefinitions> instance =
			new Lazy<ParseDefinitions>(() => new ParseDefinitions());

		static ParseDefinitions()
		{
			Init();
		}

		public static ParseDefinitions PgDefInst => instance.Value;

		public override ParseDef Invalid => new ParseDef("Invalid", null, VT_INVALID, null, false);

		// public override ParseGen Default => new ParseGen("Default", null, VT_DEFAULT, PGG_DEFAULT, (int) PGG_DEFAULT, false);
		public override ParseDef Default => idDefArray[0];

		public static AValDefBase                                                                                    Classify(string test, string value)
		{

			for (int i = 0; i < idDefArray.Length; i++)
			{
				ParseDef pg = idDefArray[i];

				if (pg == null) continue;

				if (!pg.Equals(test) || !pg.IsGood) continue;

				for (int j = 0; j < pg.ValDefs.Count; j++)
				{
					if (pg.ValDefs[j].Equals(value))
					{
						// string t = pg.ValDefs[j].GetType().Name;
						// Token tk = pg.ValDefs[j].MakeToken(value, 0, 0, 0);
						return pg.ValDefs[j];
					}
				}
			}

			return (AValDefBase)ADefBase.Invalid;
		}


		public static int Pgd_Word;
		public static int Pgd_Assignment;
		public static int Pgd_Op;
		public static int Pgd_Neg;
		public static int Pgd_Text;
		public static int Pgd_Bool;
		public static int Pgd_NumInt;
		public static int Pgd_NumDbl;
		public static int Pgd_NumFract;
		public static int Pgd_NumUnitLen;
		public static int Pgd_Funct;
		public static int Pgd_FunctArgSep;
		public static int Pgd_VarKeyXl;
		public static int Pgd_VarKey;
		public static int Pgd_Var;
		public static int Pgd_GrpRef;
		public static int Pgd_GrpBeg;
		public static int Pgd_GrpEnd;
		public static int Pgd_FunctGrpBeg;
		public static int Pgd_FunctGrpEnd;
		public static int Pgd_PrnBeg;
		public static int Pgd_PrnEnd;
		public static int Pgd_Invalid;
		public static int Pgd_Default;

		private static int SetValue(int idx, ParseDef pg)
		{
			idDefArray[idx] = pg;

			return idx;
		}


		private static void Init()
		{
			idDefArray = new ParseDef[MAX_TOKENS];

			int idx = 0;

			Pgd_Assignment = SetValue(idx++, new ParseDef("Assignment", "eq", VT_STRING,
				new[] { ValDefInst[Vd_Assignment] }));

			Pgd_Op = SetValue(idx++, new ParseDef("Operator", "op1", VT_STRING,
				new[]
				{
					ValDefInst[Vd_LogOr],
					ValDefInst[Vd_LogAnd],
					ValDefInst[Vd_LogEq],
					ValDefInst[Vd_LogInEq],
					ValDefInst[Vd_RelLt],
					ValDefInst[Vd_RelLtEq],
					ValDefInst[Vd_RelGt],
					ValDefInst[Vd_AddText],
					ValDefInst[Vd_MathAdd],
					ValDefInst[Vd_MathSubt],
					ValDefInst[Vd_MathMul],
					ValDefInst[Vd_MathDiv],
					ValDefInst[Vd_MathPwr],
					ValDefInst[Vd_MathMod],
				}));

			Pgd_Neg = SetValue(idx++, new ParseDef("Negate", "ng", VT_STRING,
				new[] { ValDefInst[Vd_MathNeg] }));

			Pgd_Text = SetValue(idx++, new ParseDef("Text", "s1", VT_TEXT,
				new[] { ValDefInst[Vd_Text] }));

			Pgd_Bool = SetValue(idx++, new ParseDef("Boolean", "b1", VT_STRING,
				new[]
				{
					ValDefInst[Vd_BoolTrue],
					ValDefInst[Vd_BoolFalse]
				}));

			Pgd_NumInt = SetValue(idx++, new ParseDef("Number Integer", "n1", VT_STRING,
				new[] { ValDefInst[Vd_NumInt] }));

			Pgd_NumDbl = SetValue(idx++, new ParseDef("Number Double", "d1", VT_STRING,
				new[] { ValDefInst[Vd_NumDouble] }));

			Pgd_NumFract = SetValue(idx++, new ParseDef("Number Fraction", "fr1", VT_STRING,
				new[] { ValDefInst[Vd_NumFract] }));

			Pgd_NumUnitLen = SetValue(idx++, new ParseDef("Number Length", "l1", VT_STRING,
				new[] { ValDefInst[Vd_NumUntLenImp] }));

			Pgd_Funct = SetValue(idx++, new ParseDef("Function", "fn1", VT_STRING,
				new[] { ValDefInst[Vd_FunInteger] }));


			Pgd_VarKeyXl = SetValue(idx++, new ParseDef("Variable Special", "v1", VT_STRING,
				new[]
				{
					// VarDefInst[Pvd_XcellAddr],
					ValDefInst[Vd_VarXlAddr],
				}));

			Pgd_VarKey = SetValue(idx++, new ParseDef("Variable Special", "v2", VT_STRING,
				new[]
				{
					ValDefInst[Vd_VarSysVar],
					ValDefInst[Vd_VarRvtParam],
					ValDefInst[Vd_VarPrjParam],
					ValDefInst[Vd_VarGblParam],
					ValDefInst[Vd_VarLblName],

					// VarDefInst[Pvd_SysVar],
					// VarDefInst[Pvd_RvtParam],
					// VarDefInst[Pvd_PrjParam],
					// VarDefInst[Pvd_GblParam],
					// VarDefInst[Pvd_LabelName],
				}));

			Pgd_Var = SetValue(idx++, new ParseDef("Variable Address", "v3", VT_STRING,
					new[] { ValDefInst[Vd_Varible] }));

			Pgd_Word = SetValue(idx++, new ParseDef("Word", "w1", VT_STRING, null, false));

			Pgd_FunctArgSep = SetValue(idx++, new ParseDef("Function Arg Separator", "as1", VT_STRING,
				new[] { ValDefInst[Vd_FunctArgSep] }));

			Pgd_PrnBeg = SetValue(idx++, new ParseDef("Parenthesis Begin", "pb", VT_STRING,
				new[] { ValDefInst[Vd_PrnBeg] }));

			Pgd_PrnEnd = SetValue(idx++, new ParseDef("Parenthesis End", "pe", VT_STRING,
				new[] { ValDefInst[Vd_PrnEnd] }));



			Pgd_GrpRef = SetValue(idx++, new ParseDef("Group Reference", "grpref", VT_STRING,
				new[] { ValDefInst[Vd_GrpRef] }));

			Pgd_GrpBeg = SetValue(idx++, new ParseDef("Group Begin", "grpbeg", VT_STRING,
				new[] { ValDefInst[Vd_GrpBeg] }));

			Pgd_GrpEnd = SetValue(idx++, new ParseDef("Group End", "grpend", VT_STRING,
				new[] { ValDefInst[Vd_GrpEnd] }));

			Pgd_FunctGrpBeg = SetValue(idx++, new ParseDef("Function Group Begin", "fngrpbeg", VT_STRING,
				new[] { ValDefInst[Vd_FnGrpBeg] }));

			Pgd_FunctGrpEnd = SetValue(idx++, new ParseDef("Function Group End", "fngrpend", VT_STRING,
				new[] { ValDefInst[Vd_FnGrpEnd] }));


			Pgd_Invalid = SetValue(idx++, new ParseDef("Invalid", "x1", VT_STRING, null, false));
			Pgd_Default = SetValue(idx++, new ParseDef("Default", null, VT_DEFAULT, null, false));
			count = idx;
		}
	}
}