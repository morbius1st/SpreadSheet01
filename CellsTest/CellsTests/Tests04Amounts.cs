#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellsTest.Windows;
using SharedCode.EquationSupport.TokenSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;
using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ValueType;

using static SharedCode.EquationSupport.Definitions.ValueDefinitions;
#endregion

// user name: jeffs
// created:   5/22/2021 6:37:28 PM

namespace CellsTest.CellsTests
{
	public class Tests04Amounts
	{
		private static MainWindow win;

		public Tests04Amounts(MainWindow win1)
		{
			win = win1;
		}

		internal void valueDefTest02a() 
		{
			win.WriteLine($"valueDefTest01|");

			ShowValueDefB(Vd_NumInt);
		}

		private const int titleWidthB = -13;
		private const int fieldWidthB = -33;
		private void ShowValueDefB(int idx)
		{
			ValueDef vDef = VdefInst[idx];

			win.WriteLine($"{"ShowValueDefA|",titleWidthB} for {idx:D}");
			win.WriteLine($"{"nameOf",titleWidthB} for {nameof(vDef)}");
			win.WriteLine($"{"value string",titleWidthB} for {vDef.ValueStr}");
			win.WriteLine($"{"description",titleWidthB} for {vDef.Description}");

		}


		internal void tokenAmtTest01()
		{
			win.WriteLine($"tokenAmtTest01|");

			AmtInteger2 ai2 = new AmtInteger2("1234");
			AmtDouble2 ad2 = new AmtDouble2("456.78");

			IAmtBase2[] aib = new IAmtBase2[]
			{
				ai2,
				ad2,
			};

			foreach (IAmtBase2 ai in aib)
			{
				ShowBaseAmtA(ai);
				win.WriteLine("");
			}

			TokenAmt2[] ta2s = new []
			{
				new TokenAmt2(aib[0]),
				new TokenAmt2(aib[1]),
				// new TokenAmt2(ai2s[2]),
			};
			
			foreach (TokenAmt2 ta2 in ta2s)
			{
				ShowTknA(ta2);
				win.WriteLine("");
			}

			// AmtInteger2[] ai2s = new []
			// {
			// 	new AmtInteger2("1234"),
			// 	new AmtInteger2("456"),
			// 	// AmtInteger2.Default,
			// 	// AmtInteger2.Invalid,
			// };
			//
			// foreach (AmtInteger2 ai in ai2s)
			// {
			// 	ShowAmtA(ai);
			// 	win.WriteLine("");
			// }


			// TokenAmt2[] ta2s = new []
			// {
			// 	new TokenAmt2(ai2s[0]),
			// 	new TokenAmt2(ai2s[1]),
			// 	// new TokenAmt2(ai2s[2]),
			// };
			//
			// foreach (TokenAmt2 ta2 in ta2s)
			// {
			// 	ShowTknA(ta2);
			// 	win.WriteLine("");
			// }
		}

		private const int titleWidthA = -13;
		private const int fieldWidthA = -23;

		private void ShowBaseAmtA(IAmtBase2 aib2)
		{

			win.WriteLine("ShowBaseAmtA");

			win.WriteLine($"{"original" ,titleWidthA}| {aib2.Original   , fieldWidthA}| ");
			win.WriteLine($"{"as string",titleWidthA}| {aib2.AsString() , fieldWidthA}| ");
			ShowBaseAmountA(aib2);
			win.WriteLine($"{"data type",titleWidthA}| {aib2.DataType   , fieldWidthA}| ");
			win.WriteLine($"{"desc"     ,titleWidthA}| {aib2.Description, fieldWidthA}| ");
			win.WriteLine($"{"order"    ,titleWidthA}| {aib2.Order      , fieldWidthA}| order of operation");
			win.WriteLine($"{"seq"      ,titleWidthA}| {aib2.Seq        , fieldWidthA}| sequence number within a value def group");
			win.WriteLine($"{"id"       ,titleWidthA}| {aib2.Id         , fieldWidthA}| generic sequence / id number");
		}

		private void ShowBaseAmountA(IAmtBase2 ai2)
		{


			switch (ai2.DataType)
			{
			case VT_NUM_INTEGER:
				{
					win.WriteLine($"{"as integer"   ,titleWidthA}| {ai2.AsInteger() , fieldWidthA}| ");
					break;
				}
			case VT_NUM_DOUBLE:
				{
					win.WriteLine($"{"as double"   ,titleWidthA}| {ai2.AsDouble() , fieldWidthA}| ");
					break;
				}
			case VT_STRING:
				{
					win.WriteLine($"{"as string"   ,titleWidthA}| {ai2.AsString() , fieldWidthA}| ");
					break;
				}
			default:
				{
					win.WriteLine("is unknown data type");
					break;
				}
			}
		}

		private void ShowAmtA(AmtInteger2 ai2)
		{

			win.WriteLine("ShowAmtA");

			win.WriteLine($"{"amount"   ,titleWidthA}| {ai2.Amount     , fieldWidthA}| ");
			win.WriteLine($"{"original" ,titleWidthA}| {ai2.Original   , fieldWidthA}| ");
			win.WriteLine($"{"as string",titleWidthA}| {ai2.AsString() , fieldWidthA}| ");
			win.WriteLine($"{"as int"   ,titleWidthA}| {ai2.AsInteger(), fieldWidthA}| ");
			win.WriteLine($"{"data type",titleWidthA}| {ai2.DataType   , fieldWidthA}| ");
			win.WriteLine($"{"desc"     ,titleWidthA}| {ai2.Description, fieldWidthA}| ");
			win.WriteLine($"{"order"    ,titleWidthA}| {ai2.Order      , fieldWidthA}| order of operation");
			win.WriteLine($"{"seq"      ,titleWidthA}| {ai2.Seq        , fieldWidthA}| sequence number within a value def group");
			win.WriteLine($"{"id"       ,titleWidthA}| {ai2.Id         , fieldWidthA}| generic sequence / id number");
		}

		private void ShowTknA(TokenAmt2 ta2)
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
					win.WriteLine($"{"as integer"   ,titleWidthA}| {ta2.AmountBase.AsInteger() , fieldWidthA}| ");
					break;
				}
			case VT_NUM_DOUBLE:
				{
					win.WriteLine($"{"as double"   ,titleWidthA}| {ta2.AmountBase.AsDouble() , fieldWidthA}| ");
					break;
				}
			case VT_STRING:
				{
					win.WriteLine($"{"as string"   ,titleWidthA}| {ta2.AmountBase.AsString() , fieldWidthA}| ");
					break;
				}
			default:
				{
					win.WriteLine("is unknown data type");
					break;
				}
			}
		}
	}
}
