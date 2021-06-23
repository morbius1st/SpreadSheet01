#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using CellsTest.Windows;
using SharedCode.DebugAssist;
using SharedCode.EquationSupport.TokenSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;
using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.Definitions.ValueDefs.FromString;
using SharedCode.EquationSupport.ParseSupport;
using static SharedCode.EquationSupport.Definitions.ValueType;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;
using ValueType = SharedCode.EquationSupport.Definitions.ValueType;

#endregion

// user name: jeffs
// created:   5/22/2021 6:37:28 PM

namespace CellsTest.CellsTests
{
	public class Tests04Amounts
	{
		private static MainWindow win;
		private ParseDefinitions pDefs = new ParseDefinitions();
		private ShowInfo show = ShowInfo.Inst;
		ParsePhase parse = new ParsePhase();

		public Tests04Amounts(MainWindow win1)
		{
			win = win1;
		}


		// test making tokens
		public void TokenAmtTest01()
		{
			win.WriteLine($"tokenAmtTest01|");

			AAmtBase[] aibs =
			{
				new AmtInvalid()             ,
				new AmtDefault()             ,
				// new AmtOpMathRelLt("<")      ,
				// new AmtOpMathRelLtEq("<=")   ,
				// new AmtOpMathRelGt(">")      ,
				// new AmtOpMathRelGtEq(">=")   ,


				// new AmtAssignment("=")       ,
				// new AmtInteger("1234")       ,
				// new AmtDouble("456.78")      ,
				// new AmtOpAdd("+")            ,
				// new AmtOpAdd("+")            ,
				// new AmtOpSub("-")            ,
				// new AmtGpRef("<gref>")       ,
				// new AmtGpBeg("(")            ,
				// new AmtGpEnd(")")            ,
			};

			foreach (AAmtBase aib in aibs)
			{
				ShowBaseAmtA(aib);
				win.WriteLine("");
			}

			Token[] tokens = new Token[aibs.Length];

			for (var i = 0; i < aibs.Length; i++)
			{
				tokens[i] = new Token(ValDefInst[Vd_Default], aibs[i], new ParseDataInfo(0, 0, 0));
			}

			int[] order = new int[0];
			List<Tuple<string[], int, int, bool>> tkDesc = show.TokenInfoList(out order);

			win.TabUp(1);
			show.ShowHeader(tkDesc, order);

			foreach (Token tk in tokens)
			{
				show.ShowToken2(tk, tkDesc, order);
				// win.WriteLine("");
			}
			win.TabDn(1);
		}

		public void TokenTest02()
		{
			// win.WriteLine("@2");
			win.WriteLine("token test 04-01");
			win.WriteLine("tokenizing| ");

			string[] test = new string[15];
			int k = 0;

			// test[k++] = "= 123 (456 + 789) + (321 + 654) + 987";
			// test[k++] = "= 123 (456 + (123 + 456) + 246) + 567";
			// test[k++] = "= 123 (456 + (123 + 456) + 246) + 567)";
			// test[k++] = "= 123 (456 + (123 + 456) + 246 + 567";
			// test[k++] = "=( 1 + (21 + 22 + (31 + 32 * (41 + 42) * (sign(51+52) ) ) * sign( alpha, beta, (1 + 2) ) ) + 2*3) + 4+5 + {[A1]} + ({!B1}) & \"text text text\"";
			// test[k++] = "= 123 (456 + (123 + (678 + (123 + (123 + 456 + 789) + (123 + 456) + (123 + (123 + 456) + 456)) + 456) + 246))";

			// test[k++] = " = 1.0 + 1/2" ;
			// test[k++] = " = 1'-6\" +2'";
			// test[k++] = " = ((-456.6 + 789.4) - (321 * +654) / 50) % 4^1/2";
			// test[k++] = "{@Label} = {[A1]} + {$SystemVar} + {#RevitParam} + {%ProjParam} + {!GlobalParam} + {@LabelName} - {variable}";
			// test[k++] = " = True <or> False <and> 6 < (3>7)";
			// test[k++] = " = 5 > (6 < 7 >= 8) <= 9 == 10 != 11";
			// test[k++] = "{@Label} = \"hello\" & \" goodbye \"+ if(4>5,\"yes\",\"no\")";


			test[k++] = "= <or> <and> == != ! < <= > >= & + - * / ^ & + -A \"hello\" True False 100 10.0 1/2 "
				+ "{[A1]} {$SystemVar} {#RevitParam} {%ProjParam} {!GlobalParam} {@LabelName} {variable}"
				+ "(A) A(1,2) 1'-0\"";



			for (int i = 0; i < k; i++)
			{
				// win.WriteLine("@3");
				if (test[i] == null) break;

				TestHeader01(i, test[i]);

				List<List<ParseData>> result = ParsePhaseOne(test[i]);

				// win.WriteLine("@4");
				ShowParseData(result);
				// continue;

				if (result == null)
				{
					win.WriteLine("tokenizing| failed");
					return;
				}

				win.WriteLine("tokenizing| success");

				// win.WriteLine("@5");

				Tokens tks = new Tokens("formula 1");
				bool answer = tks.Process(result);

				if (!answer)
				{
					win.WriteLine("parse / tokenize| failed");
					return;
				}

				win.WriteLine("parse / tokenize| success");

				// win.WriteLine("@6");
				ShowTokenList(tks);

				// List<Token> tokens = TokenizePhaseOne(result);
			}
		}

	#region private methods

		private void ShowParseData(List<List<ParseData>> dataList)
		{
			int[] order;

			List<Tuple<string[], int, int, bool>> infoList =
				show.ParseDataInfoList(out order);

			win.WriteLineTab("parse data list");

			win.TabUp(1);
			win.TabUp(3);

			show.ShowHeader(infoList, order);
			win.TabDn(3);

			for (var i = 0; i < dataList.Count; i++)
			{
				List<ParseData> pdList = dataList[i];

				win.WriteLineTab($"record {i:D}");
				win.TabUp(2);
				
				foreach (ParseData pd in pdList)
				{
					show.ShowParseData(pd, infoList, order);
				}
				win.TabDn(2);
			}
			win.TabDn(1);



		}

		private void TestHeader01(int i, string test)
		{
			win.Write("\n");
			win.WriteLine("testing| " + i + "| length| " + test.Length);
			win.Write("\n");

			win.WriteLine("0         1         2         3         4         5         6         7         8         9        10");
			win.WriteLine("0123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|123456789|");
			win.WriteLine(test);
			win.Write("\n");
		}


		private List<List<ParseData>> ParsePhaseOne(string eq)
		{
			parse = new ParsePhase();
			bool result = parse.Parse4(eq);
			return result ? parse.ParseList : null;
		}

		private void ShowTokenList(Tokens tks)
		{
			int[] order = new int[0];

			win.WriteLine("ShowTokenList|");

			List<Tuple<string[], int, int, bool>> tkDesc = show.TokenInfoList(out order);

			// order = new [] {1, 2, 3, 4, 5, 6};

			for (var i = 0; i < tks.TokenList.Count; i++)
			{
				List<Token> tkList = tks.TokenList[i];

				win.WriteLineTab($"Record| {i:D}  Level| {tkList[0].Level}");

				win.TabUp(1);
				show.ShowHeader(tkDesc, order);

				foreach (Token t in tkList)
				{
					show.ShowToken2(t, tkDesc, order);
				}

				win.TabDn(2);
				win.WriteLine("");
			}
		}

		private const int titleWidthA = -13;
		private const int fieldWidthA = -23;

		private void ShowBaseAmtA(AAmtBase aib2)
		{
			win.WriteLine("ShowBaseAmtA");

			win.WriteLine($"{"original" ,titleWidthA}| {aib2.Original   , fieldWidthA}| ");
			win.WriteLine($"{"as string",titleWidthA}| {aib2.AsString() , fieldWidthA}| ");
			// if (aib2.ValDef.IsNumeric) ShowBaseAmountA(aib2);
			// win.WriteLine($"{"data type",titleWidthA}| {aib2.ValueType   , fieldWidthA}| ");
			// win.WriteLine($"{"desc"     ,titleWidthA}| {aib2.Description, fieldWidthA}| ");
			// win.WriteLine($"{"order"    ,titleWidthA}| {aib2.Order      , fieldWidthA}| order of operation");
			// // win.WriteLine($"{"seq"      ,titleWidthA}| {aib2.Seq        , fieldWidthA}| sequence number within a value def group");
			// win.WriteLine($"{"id"       ,titleWidthA}| {aib2.Id         , fieldWidthA}| generic sequence / id number");
		}

		// private void ShowBaseAmountA(AAmtBase ai2)
		// {
		// 	switch (ai2.ValueType)
		// 	{
		// 	case VT_NUM_INTEGER:
		// 		{
		// 			win.WriteLine($"{"is integer"   ,titleWidthA}| {ai2.AsInteger() , fieldWidthA}| ");
		// 			break;
		// 		}
		// 	case VT_NUM_DOUBLE:
		// 		{
		// 			win.WriteLine($"{"is double"   ,titleWidthA}| {ai2.AsDouble() , fieldWidthA}| ");
		// 			break;
		// 		}
		// 	case VT_STRING:
		// 		{
		// 			win.WriteLine($"{"is string"   ,titleWidthA}| {ai2.AsString() , fieldWidthA}| ");
		// 			break;
		// 		}
		// 	default:
		// 		{
		// 			win.WriteLine("is unknown data type");
		// 			break;
		// 		}
		// 	}
		// }

		// show parse defs
		// internal void ShowParseGenDefs01a()
		// {
		// 	win.showTabId = false;
		//
		// 	win.WriteLine($"ShowParseDefs01a|");
		//
		// 	win.WriteLine("");
		// 	ShowParseGenDefs1D(pDefs.Invalid);
		//
		// 	win.WriteLine("");
		// 	ShowParseGenDefs1D(pDefs.Default);
		//
		// 	for (var i = 0; i < pDefs.Count; i++)
		// 	{
		// 		win.WriteLine("");
		// 		win.WriteLine($"{"ShowParseGenDefs",titleWidthD}| for {i:D}");
		// 		ShowParseGenDefs1D(pDefs[i]);
		// 	}
		// }

		// public void valueDefTest01b()
		// {
		// 	win.WriteLine($"valueDefTest01b|");
		//
		// 	ShowValueDefB(Vd_NumInt);
		// 	ShowValueDefB(Vd_NumDouble);
		// 	ShowValueDefB(Vd_NumFract);
		// 	ShowValueDefB(Vd_NumUntLenImp);
		//
		// 	ShowValueTypesEnumB();
		// 	ShowValueTypesC();
		// }

		// private const int titleWidthD = -20;
		// private const int fieldWidthD = -23;

		// private void ShowParseGenDefs1D(ParseGen pg)
		// {
		// 	if (!show.ShowParseGen(pg)) return;
		//
		// 	if (pg.aDefBase2 == null
		// 		|| pg.aDefBase2.Count == 0
		// 		) return;
		//
		// 	win.TabUp(1);
		// 	foreach (ADefBase2 aDef in pg.aDefBase2)
		// 	{
		// 		win.WriteLine("");
		// 		// ShowParsVarDefs2D(aDef);
		//
		// 		show.ShowParsVarDefs2D(aDef);
		// 	}
		//
		// 	win.TabDn(1);
		// 	win.WriteLine("");
		// }

		// private const int titleWidthB = -14;
		// private const int fieldWidthB = -23;

		// private void ShowValueDefB(int idx)
		// {
		// 	// ValueDefinitions a = VdefInst;
		// 	ADefBase2 vDef = (ADefBase2)ValDefInst[idx];
		//
		// 	if (vDef == null) return;
		//
		// 	win.WriteLine("");
		// 	win.WriteLine($"{"ShowValueDefB|",titleWidthB}| for {idx:D}");
		// 	win.WriteLine($"{"nameOf",titleWidthB}| {nameof(vDef)}");
		// 	win.WriteLine($"{"value string",titleWidthB}| {vDef.ValueStr}");
		// 	win.WriteLine($"{"numeric?",titleWidthB}| {vDef.IsNumeric}");
		// 	win.WriteLine($"{"description",titleWidthB}| {vDef.Description}");
		// 	win.WriteLine($"{"order",titleWidthB}| {vDef.Order}");
		// 	// win.WriteLine($"{"seq",titleWidthB}| {vDef.Seq}");
		// 	win.WriteLine($"{"id",titleWidthB}| {vDef.Id}");
		// }
		//
		// private void ShowValueTypesEnumB()
		// {
		// 	win.WriteLine("");
		// 	win.WriteLine("ValueTypesEnum|");
		// 	foreach (object vt in Enum.GetValues(typeof(ValueType)))
		// 	{
		// 		win.WriteLine($"{"value type",titleWidthB}| {vt.ToString(),-20} | {(int)vt:D}");
		// 	}
		// }

		// private const int titleWidthC = -13;
		// private const int fieldWidthC = -33;

		// private void ShowValueTypesC()
		// {
		// 	ValueDefinitions a = ValDefInst;
		//
		// 	win.WriteLine("");
		// 	win.WriteLine("ValueTypes|");
		// 	for (var i = 0; i < ValDefInst.Count; i++)
		// 	{
		// 		ADefBase2 vDef = (ADefBase2)ValDefInst[i];
		//
		// 		if (vDef == null) continue;
		//
		// 		win.Write($"{"value def",titleWidthC}| {vDef.Description,fieldWidthC}");
		// 		win.Write($" numeric?| {vDef.IsNumeric,-8}");
		// 		win.Write($" val type| {vDef.ValueType,-8:D}");
		// 		win.Write($" order| {vDef.Order,-8:D}");
		// 		win.WriteLine($" str| {vDef.ValueStr}");
		// 	}
		// }


		// private void ShowAmtA(AmtInteger ai2)
		// {
		// 	win.WriteLine("ShowAmtA");
		//
		// 	win.WriteLine($"{"amount"   ,titleWidthA}| {ai2.Amount     , fieldWidthA}| ");
		// 	win.WriteLine($"{"original" ,titleWidthA}| {ai2.Original   , fieldWidthA}| ");
		// 	win.WriteLine($"{"as string",titleWidthA}| {ai2.AsString() , fieldWidthA}| ");
		// 	win.WriteLine($"{"as int"   ,titleWidthA}| {ai2.AsInteger(), fieldWidthA}| ");
		// 	win.WriteLine($"{"data type",titleWidthA}| {ai2.DataType   , fieldWidthA}| ");
		// 	win.WriteLine($"{"desc"     ,titleWidthA}| {ai2.Description, fieldWidthA}| ");
		// 	win.WriteLine($"{"order"    ,titleWidthA}| {ai2.Order      , fieldWidthA}| order of operation");
		// 	// win.WriteLine($"{"seq"      ,titleWidthA}| {ai2.Seq        , fieldWidthA}| sequence number within a value def group");
		// 	win.WriteLine($"{"id"       ,titleWidthA}| {ai2.Id         , fieldWidthA}| generic sequence / id number");
		// }


		// private void ShowParsVarDefs2D<T>(T pv) where T : ADefBase
		// {
		// 	win.WriteLineTab($"{"val str",titleWidthD}| {pv.ValueStr}");
		// 	win.WriteLineTab($"{"type",titleWidthD}| {pv.GetType()}");
		// 	win.WriteLineTab($"{"id",titleWidthD}| {pv.Id}");
		// 	win.WriteLineTab($"{"val type",titleWidthD}| {pv.ValueType}");
		// 	win.WriteLineTab($"{"description",titleWidthD}| {pv.Description}");
		// }

		// private void ShowTknA(Token ta2)
		// {
		// 	win.WriteLine("ShowTknA");
		//
		// 	win.WriteLine($"{"original"    ,titleWidthA}| {ta2.AmountBase.Original    , fieldWidthA}| ");
		// 	win.WriteLine($"{"type"        ,titleWidthA}| {ta2.GetType().Name         , fieldWidthA}| ");
		// 	win.WriteLine($"{"data type"   ,titleWidthA}| {ta2.DataType               , fieldWidthA}| ");
		// 	win.WriteLine($"{"description" ,titleWidthA}| {ta2.AmountBase.Description , fieldWidthA}| ");
		// 	win.WriteLine($"{"to string"   ,titleWidthA}| {ta2.ToString()             , fieldWidthA}| ");
		// 	win.WriteLine("as?|");
		//
		// 	switch (ta2.DataType)
		// 	{
		// 	case VT_NUM_INTEGER:
		// 		{
		// 			win.WriteLine($"{"is integer"   ,titleWidthA}| {ta2.AmountBase.AsInteger() , fieldWidthA}| ");
		// 			break;
		// 		}
		// 	case VT_NUM_DOUBLE:
		// 		{
		// 			win.WriteLine($"{"is double"   ,titleWidthA}| {ta2.AmountBase.AsDouble() , fieldWidthA}| ");
		// 			break;
		// 		}
		// 	case VT_OP_URINARY:
		// 	case VT_OP_LOGICALMATH:
		// 	case VT_OP_MULTIPLICATIVE:
		// 	case VT_OP_RELATIONALMATH:
		// 	case VT_OP_STRING:
		// 	case VT_OP_ADDITIVE:
		// 	case VT_GP_REF:
		// 	case VT_GP_BEG:
		// 	case VT_GP_END:
		// 	case VT_STRING:
		// 		{
		// 			win.WriteLine($"{"is string"   ,titleWidthA}| {ta2.AmountBase.AsString() , fieldWidthA}| ");
		// 			break;
		// 		}
		// 	default:
		// 		{
		// 			win.WriteLine("is unknown data type");
		// 			break;
		// 		}
		// 	}
		// }

	#endregion
	}
}