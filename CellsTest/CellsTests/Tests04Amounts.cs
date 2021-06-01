#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellsTest.Windows;
using SharedCode.DebugAssist;
using SharedCode.EquationSupport.TokenSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;
using SharedCode.EquationSupport.Definitions;
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
		private ParseGeneralDefinitions pDefs = new ParseGeneralDefinitions();
		private ShowInfo show = ShowInfo.Inst;

		public Tests04Amounts(MainWindow win1)
		{
			win = win1;
		}

		// show parse defs
		internal void ShowParseGenDefs01a()
		{
			win.showTabId = false;

			win.WriteLine($"ShowParseDefs01a|");

			win.WriteLine("");
			ShowParseGenDefs1D(pDefs.Invalid);

			win.WriteLine("");
			ShowParseGenDefs1D(pDefs.Default);

			for (var i = 0; i < pDefs.Count; i++)
			{
				win.WriteLine("");
				win.WriteLine($"{"ShowParseGenDefs",titleWidthD}| for {i:D}");
				ShowParseGenDefs1D(pDefs[i]);
			}
		}

		// show value defs
		internal void valueDefTest01b()
		{
			win.WriteLine($"valueDefTest02b|");

			ShowValueDefB(Vd_NumInt);
			ShowValueDefB(Vd_NumDouble);
			ShowValueDefB(Vd_NumFract);
			ShowValueDefB(Vd_NumUntLenImp);

			ShowValueTypesEnumB();
			ShowValueTypesC();
		}

		// test making tokens
		internal void tokenAmtTest01()
		{
			win.WriteLine($"tokenAmtTest01|");

			AmtInteger ai = new AmtInteger("1234");
			AmtDouble ad = new AmtDouble("456.78");
			AmtOpAdd aoa = new AmtOpAdd("+");
			AmtGpRef agr = new AmtGpRef("");
			AmtGpBeg agb = new AmtGpBeg("(");
			AmtGpEnd age = new AmtGpEnd(")");

			AAmtBase[] aibs = new AAmtBase[]
			{
				ai,
				ad,
				aoa,
				agr,
				agb,
				age,
			};

			foreach (AAmtBase aib in aibs)
			{
				ShowBaseAmtA(aib);
				win.WriteLine("");
			}

			Token[] tokens = new []
			{
				new Token(aibs[0], 0, 0),
				new Token(aibs[1], 0, 0),
				new Token(aibs[2], 0, 0),
				new Token(aibs[3], 0, 0),
				new Token(aibs[4], 0, 0),
				new Token(aibs[5], 0, 0),
			};

			foreach (Token tk in tokens)
			{
				ShowTknA(tk);
				win.WriteLine("");
			}
		}

		public bool ShowParseGen(ParseGen pg)
		{
			if (pg == null) return false;

			win.WriteLineTab($"{"val str",titleWidthD}| {pg.ValueStr}");
			win.WriteLineTab($"{"id",titleWidthD}| {pg.Id}");
			win.WriteLineTab($"{"val type",titleWidthD}| {pg.ValueType}");
			win.WriteLineTab($"{"description",titleWidthD}| {pg.Description}");
			win.WriteLineTab($"{"isgood",titleWidthD}| {pg.IsGood}");

			return true;
		}

	#region private methods

		private const int titleWidthD = -20;
		private const int fieldWidthD = -23;

		private void ShowParseGenDefs1D(ParseGen pg)
		{
			if (!ShowParseGen(pg)) return;

			if (pg.aDefBase2 == null
				|| pg.aDefBase2.Count == 0
				) return;

			win.TabUp(1);
			foreach (ADefBase2 aDef in pg.aDefBase2)
			{
				win.WriteLine("");
				// ShowParsVarDefs2D(aDef);

				show.ShowParsVarDefs2D(aDef);
			}

			win.TabDn(1);
			win.WriteLine("");
		}


		// private void ShowParsVarDefs2D<T>(T pv) where T : ADefBase
		// {
		// 	win.WriteLineTab($"{"val str",titleWidthD}| {pv.ValueStr}");
		// 	win.WriteLineTab($"{"type",titleWidthD}| {pv.GetType()}");
		// 	win.WriteLineTab($"{"id",titleWidthD}| {pv.Id}");
		// 	win.WriteLineTab($"{"val type",titleWidthD}| {pv.ValueType}");
		// 	win.WriteLineTab($"{"description",titleWidthD}| {pv.Description}");
		// }

		private const int titleWidthB = -14;
		private const int fieldWidthB = -23;

		private void ShowValueDefB(int idx)
		{
			// ValueDefinitions a = VdefInst;
			ValDef vDef = (ValDef) ValDefInst[idx];

			if (vDef == null) return;

			win.WriteLine("");
			win.WriteLine($"{"ShowValueDefB|",titleWidthB}| for {idx:D}");
			win.WriteLine($"{"nameOf",titleWidthB}| {nameof(vDef)}");
			win.WriteLine($"{"value string",titleWidthB}| {vDef.ValueStr}");
			win.WriteLine($"{"numeric?",titleWidthB}| {vDef.IsNumeric}");
			win.WriteLine($"{"description",titleWidthB}| {vDef.Description}");
			win.WriteLine($"{"order",titleWidthB}| {vDef.Order}");
			// win.WriteLine($"{"seq",titleWidthB}| {vDef.Seq}");
			win.WriteLine($"{"id",titleWidthB}| {vDef.Id}");
		}

		private void ShowValueTypesEnumB()
		{
			win.WriteLine("");
			win.WriteLine("ValueTypesEnum|");
			foreach (object vt in Enum.GetValues(typeof(ValueType)))
			{
				win.WriteLine($"{"value type", titleWidthB}| {vt.ToString(),-20} | {(int) vt:D}");
			}
		}

		private const int titleWidthC = -13;
		private const int fieldWidthC = -33;

		private void ShowValueTypesC()
		{
			ValueDefinitions a = ValDefInst;

			win.WriteLine("");
			win.WriteLine("ValueTypes|");
			for (var i = 0; i < ValDefInst.Count; i++)
			{
				ValDef vDef = (ValDef) ValDefInst[i];

				if (vDef == null) continue;

				win.Write($"{"value type", titleWidthC}| {vDef.Description,fieldWidthC} |");
				win.WriteLine($"{(int) vDef.ValueType,-6:D}| numeric| {vDef.IsNumeric}");
			}
		}

		private const int titleWidthA = -13;
		private const int fieldWidthA = -23;

		private void ShowBaseAmtA(AAmtBase aib2)
		{
			win.WriteLine("ShowBaseAmtA");

			win.WriteLine($"{"original" ,titleWidthA}| {aib2.Original   , fieldWidthA}| ");
			win.WriteLine($"{"as string",titleWidthA}| {aib2.AsString() , fieldWidthA}| ");
			if (aib2.ValueDef.IsNumeric) ShowBaseAmountA(aib2);
			win.WriteLine($"{"data type",titleWidthA}| {aib2.DataType   , fieldWidthA}| ");
			win.WriteLine($"{"desc"     ,titleWidthA}| {aib2.Description, fieldWidthA}| ");
			win.WriteLine($"{"order"    ,titleWidthA}| {aib2.Order      , fieldWidthA}| order of operation");
			// win.WriteLine($"{"seq"      ,titleWidthA}| {aib2.Seq        , fieldWidthA}| sequence number within a value def group");
			win.WriteLine($"{"id"       ,titleWidthA}| {aib2.Id         , fieldWidthA}| generic sequence / id number");
		}

		private void ShowBaseAmountA(AAmtBase ai2)
		{
			switch (ai2.DataType)
			{
			case VT_NUM_INTEGER:
				{
					win.WriteLine($"{"is integer"   ,titleWidthA}| {ai2.AsInteger() , fieldWidthA}| ");
					break;
				}
			case VT_NUM_DOUBLE:
				{
					win.WriteLine($"{"is double"   ,titleWidthA}| {ai2.AsDouble() , fieldWidthA}| ");
					break;
				}
			case VT_STRING:
				{
					win.WriteLine($"{"is string"   ,titleWidthA}| {ai2.AsString() , fieldWidthA}| ");
					break;
				}
			default:
				{
					win.WriteLine("is unknown data type");
					break;
				}
			}
		}

		private void ShowAmtA(AmtInteger ai2)
		{
			win.WriteLine("ShowAmtA");

			win.WriteLine($"{"amount"   ,titleWidthA}| {ai2.Amount     , fieldWidthA}| ");
			win.WriteLine($"{"original" ,titleWidthA}| {ai2.Original   , fieldWidthA}| ");
			win.WriteLine($"{"as string",titleWidthA}| {ai2.AsString() , fieldWidthA}| ");
			win.WriteLine($"{"as int"   ,titleWidthA}| {ai2.AsInteger(), fieldWidthA}| ");
			win.WriteLine($"{"data type",titleWidthA}| {ai2.DataType   , fieldWidthA}| ");
			win.WriteLine($"{"desc"     ,titleWidthA}| {ai2.Description, fieldWidthA}| ");
			win.WriteLine($"{"order"    ,titleWidthA}| {ai2.Order      , fieldWidthA}| order of operation");
			// win.WriteLine($"{"seq"      ,titleWidthA}| {ai2.Seq        , fieldWidthA}| sequence number within a value def group");
			win.WriteLine($"{"id"       ,titleWidthA}| {ai2.Id         , fieldWidthA}| generic sequence / id number");
		}

		private void ShowTknA(Token ta2)
		{
			win.WriteLine("ShowTknA");

			win.WriteLine($"{"original"    ,titleWidthA}| {ta2.AmountBase.Original    , fieldWidthA}| ");
			win.WriteLine($"{"type"        ,titleWidthA}| {ta2.GetType().Name         , fieldWidthA}| ");
			win.WriteLine($"{"data type"   ,titleWidthA}| {ta2.DataType               , fieldWidthA}| ");
			win.WriteLine($"{"description" ,titleWidthA}| {ta2.AmountBase.Description , fieldWidthA}| ");
			win.WriteLine($"{"to string"   ,titleWidthA}| {ta2.ToString()             , fieldWidthA}| ");
			win.WriteLine("as?|");

			switch (ta2.DataType)
			{
			case VT_NUM_INTEGER:
				{
					win.WriteLine($"{"is integer"   ,titleWidthA}| {ta2.AmountBase.AsInteger() , fieldWidthA}| ");
					break;
				}
			case VT_NUM_DOUBLE:
				{
					win.WriteLine($"{"is double"   ,titleWidthA}| {ta2.AmountBase.AsDouble() , fieldWidthA}| ");
					break;
				}
			case VT_OP_URINARY:
			case VT_OP_LOGICAL:
			case VT_OP_MULTIPLICATIVE:
			case VT_OP_RELATIONAL:
			case VT_OP_STRING:
			case VT_OP_ADDITIVE:
			case VT_GP_REF:
			case VT_GP_BEG:
			case VT_GP_END:
			case VT_STRING:
				{
					win.WriteLine($"{"is string"   ,titleWidthA}| {ta2.AmountBase.AsString() , fieldWidthA}| ");
					break;
				}
			default:
				{
					win.WriteLine("is unknown data type");
					break;
				}
			}
		}

	#endregion
	}
}