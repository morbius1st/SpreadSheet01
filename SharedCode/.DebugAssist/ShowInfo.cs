#region using

using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.ParseSupport;
using SharedCode.EquationSupport.TokenSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;
using UtilityLibrary;
using ValueType = SharedCode.EquationSupport.Definitions.ValueType;

#endregion

// username: jeffs
// created:  5/29/2021 6:26:42 AM

namespace SharedCode.DebugAssist
{
	public class ShowInfo
	{
	#region private fields

		private ISendMessages win;

		private static readonly Lazy<ShowInfo> instance =
			new Lazy<ShowInfo>(() => new ShowInfo());

	#endregion

	#region ctor

		public ShowInfo() { }

	#endregion

	#region public properties

		public static ShowInfo Inst => instance.Value;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void SetMessenger(ISendMessages win)
		{
			this.win = win;
		}

		private void ShowData(List<string> dataList, List<Tuple<string[], int, int, bool>> infoList, int[] order)
		{
			win.WriteTab("");

			for (int i = 0; i < order.Length; i++)
			{
				if (infoList[i].Item4) continue;

				int j = order[i];
				int k = infoList[j].Item2 >= 0 ? infoList[j].Item2 : -infoList[j].Item2;

				win.Write(Truncate(dataList[j], infoList[j].Item2).PadRight(k));
				win.Write(" ".Repeat(GAP));
			}

			win.WriteLine("");
		}

		public void ShowHeader(List<Tuple<string[], int, int, bool>> infoList, int[] order)
		{
			List<string> dataList;

			for (int i = 0; i < 2; i++)
			{
				dataList = new List<string>();

				for (int j = 0; j < infoList.Count; j++)
				{
					dataList.Add(infoList[j].Item1[i]);
				}

				ShowData(dataList, infoList, order);
			}

			dataList = new List<string>();

			for (int j = 0; j < infoList.Count; j++)
			{
				int k = infoList[j].Item2 >= 0 ? infoList[j].Item2 : -infoList[j].Item2;
				dataList.Add("-".Repeat(k));
			}

			ShowData(dataList, infoList, order);
		}


	#region Show Token Info

		public void ShowToken2(Token t, List<Tuple<string[], int, int, bool>> tkd, int[] order)
		{
			if (t == null)
			{
				win.WriteLineTab("Token is null");
				return;
			}

			//         title   title    
			//         top row bot row value   width
			List<string> tkl = InitList(tkd.Count);

			tkl[0] = t.Level.ToString("D");

			if (t.IsRefIdx)
			{
				tkl[1] = $"[{t.RefIdx.ToString("D")}]";
			}
			else
			{
				tkl[1] = $"({t.Position.ToString("D")})";
			}

			tkl[2] = t.Length.ToString("D");
			tkl[3] = t.ValDef.ValueType.ToString();
			tkl[4] = "**";
			tkl[5] = t.AmountBase.IsValid.ToString();
			tkl[6] = AsAmount(t);
			tkl[7] = t.AmountBase.AsString();
			tkl[8] = t.AmountBase.GetType().Name;
			tkl[9] = AsAmount(t).GetType().Name;
			tkl[10] = "**";
			tkl[11] = t.ValDef.ValueStr;
			tkl[12] = t.ValDef.Order.ToString("D");
			tkl[13] = t.ValDef.Index.ToString("D");
			tkl[14] = t.ValDef.Description;
			tkl[15] = t.ValDef.ValueType.ToString();
			tkl[16] = t.ValDef.DataGroup.ToString();
			tkl[17] = t.ValDef.IsNumeric.ToString();
			tkl[18] = t.ValDef.GetType().Name;
			tkl[19] = t.AmountBase.ToString();

			
			ShowData(tkl, tkd, order);
		}

		private List<string> InitList(int qty)
		{
			List<string> dataList = new List<string>();

			for (int i = 0; i < qty; i++)
			{
				dataList.Add("");
			}

			return dataList;
		}

		public List<Tuple<string[], int, int, bool>> TokenInfoList(out int[] order)
		{
			//         title   title    
			//         top row bot row value   width
			List<Tuple<string[], int, int, bool>> tkl =
				new List<Tuple<string[], int, int, bool>>();


			AddToTokenInfoList(tkl, ""          , "lvl"          , 03, 00);
			AddToTokenInfoList(tkl, "(pos)"     , "[ref]"        , 05, 01);
			AddToTokenInfoList(tkl, ""          , "len"          , 03, 02);
			AddToTokenInfoList(tkl, ""          , "val type"     , 25, 03);
																	 
			AddToTokenInfoList(tkl, "frm"       , "amt"          , 03, 04);
																	 
			AddToTokenInfoList(tkl, ""          , "valid"        , 05, 05);
			AddToTokenInfoList(tkl, "as"        , "amt"          , 20, 07);
			AddToTokenInfoList(tkl, "as"        , "string"       , 20, 08);
			AddToTokenInfoList(tkl, ""          , "type"         , 18, 09);
			AddToTokenInfoList(tkl, "as amt"    , "type"         , 08, 06);
																	 
			AddToTokenInfoList(tkl, "frm"       , "valdef"       , 06, 11);
			AddToTokenInfoList(tkl, "val"       , "str"          , 07, 12);
			AddToTokenInfoList(tkl, ""          , "order"        , 07, 13);
			AddToTokenInfoList(tkl, ""          , "idx"          , 03, 14);
			AddToTokenInfoList(tkl, ""          , "val desc"     , 25, 15);
			AddToTokenInfoList(tkl, ""          , "val type"     , 25, 16);
			AddToTokenInfoList(tkl, ""          , "data group"   , 12, 17);
			AddToTokenInfoList(tkl, "is"        , "numeric?"     , 08, 18);
			AddToTokenInfoList(tkl, ""          , "type"         ,-15, 19);
			
			AddToTokenInfoList(tkl, "amt"       , "to string"    , 40, 10);

			order = GetOrder(tkl);

			return tkl;
		}

		private void AddToTokenInfoList(List<Tuple<string[], int, int, bool>> TokenList,
			string tt, string bt, int w, int o, bool hide = false)
		{
			TokenList.Add(new Tuple<string[], int, int, bool>(new string[] {tt, bt}, w, o, hide));
		}

	#endregion

	#region show parse data

		public void ShowParseData(ParseData pd, List<Tuple<string[], int, int, bool>> infoList, int[] order)
		{
			if (pd == null)
			{
				win.WriteLineTab("Token is null");
				return;
			}

			//         title   title    
			//         top row bot row value   width
			List<string> dataList = new List<string>();
			List<string> defDataList = DefinitionDataList(pd.Definition);

			string ri = $"[{pd.RefIdx:D}]";
			string pi = $"({pd.Position:D})";

			string idx = pd.GotRefIdx ? 
				$"{ri,-5} {pd.GotRefIdx}" : 
				$"{pi,-6} {pd.GotRefIdx,7}";

			dataList.Add( $"{pd.Level.ToString("D")}");
			dataList.Add( $"{pd.Name}");
			dataList.Add( $"{pd.Value}");
			dataList.Add( $"{idx}");
			dataList.Add( $"{pd.Length}");
			dataList.Add( $"{pd.IsValueDef}");
			dataList.Add( $"{pd.GetType().Name}");
			dataList.Add( "**");

			dataList.AddRange(defDataList);

			ShowData(dataList, infoList, order);
		}

		public List<Tuple<string[], int, int, bool>> ParseDataInfoList(out int[] order)
		{
			List<Tuple<string[], int, int, bool>> infoList = new List<Tuple<string[], int, int, bool>>();
			List<Tuple<string[], int, int, bool>> defInfoList = DefinitionInfoList();

			AddToDefInfoList(infoList, ""        , "lvl"            , 03, 00);
			AddToDefInfoList(infoList, ""        , "name"           , 08, 01);
			AddToDefInfoList(infoList, ""        , "value"          , 15, 02);
			AddToDefInfoList(infoList, "(pos) /" , "[ref] (is ref)" , 18, 03);
			AddToDefInfoList(infoList, ""        , "len"            , 05, 04);
			AddToDefInfoList(infoList, "is"      , "val def"        , 07, 05);
			AddToDefInfoList(infoList, ""        , "type name"      , 15, 06);
			AddToDefInfoList(infoList, "frm"     , "def"            , 03, 07);

			int offset = infoList.Count;

			int[] order1 = GetOrder(infoList);
			int[] order2 = GetOrder(defInfoList);
			AdjOrder(order2, 0, offset);

			order = new int[order1.Length + order2.Length];
			order1.CopyTo(order,0);
			order2.CopyTo(order, order1.Length);

			infoList.AddRange(defInfoList);

			return infoList;
		}

		private void AdjOrder(int[] order, int start, int offset)
		{
			for (var i = start; i < order.Length; i++)
			{
				order[i] += offset;
			}
		}


		private List<string> DefinitionDataList(AValDefBase def)
		{
			List<string> dataList = new List<string>();

			dataList.Add( $"{def.Description}");
			dataList.Add( $"{def.ValueStr}");
			dataList.Add( $"{def.IsNumeric}");
			dataList.Add( $"{def.Order}");
			dataList.Add( $"{def.DataGroup}");
			dataList.Add( $"{def.ValueType}");
			dataList.Add( $"{def.Index}");
			dataList.Add( $"{def.Id}");
			dataList.Add( $"{def.GetType().Name}");

			return dataList;
		}

		private List<Tuple<string[], int, int, bool>> DefinitionInfoList()
		{
			List<Tuple<string[], int, int, bool>> infoList = new List<Tuple<string[], int, int, bool>>();

			AddToDefInfoList(infoList, ""   , "Desc"      , 25, 00);
			AddToDefInfoList(infoList, "val", "str"       , 15, 01);
			AddToDefInfoList(infoList, "is" , "numeric?"  , 08, 02);
			AddToDefInfoList(infoList, ""   , "order"     , 07, 03);
			AddToDefInfoList(infoList, ""   , "data group", 12, 04);
			AddToDefInfoList(infoList, ""   , "value type", 25, 05);
			AddToDefInfoList(infoList, ""   , "idx"       , 03, 06);
			AddToDefInfoList(infoList, ""   , "id"        , 03, 07);
			AddToDefInfoList(infoList, ""   , "type name" ,-25, 08);

			return infoList;
		}

		private void AddToDefInfoList(List<Tuple<string[], int, int, bool>> infoList,
			string tt, string bt, int w, int o, bool hide = false)
		{
			infoList.Add(new Tuple<string[], int, int, bool>(new string[] {tt, bt}, w, o, hide));
		}

	#endregion
	
	#region show val / var defs

		public void ShowValDef(AValDefBase vd, 
			List<Tuple<string[], int, int, bool>> infoList, int[] order)
		{
			List<string> dataList = new List<string>();

			dataList.Add( $"{vd.Description}"); // 0
			dataList.Add( $"{vd.ValueStr}"); // 1
			dataList.Add( $"{vd.Id}"); // 2
			dataList.Add( $"{vd.ValueType}"); // 3
			dataList.Add( $"{vd.DataGroup}"); // 4
			dataList.Add( $"{vd.Order}"); // 5
			dataList.Add( $"{vd.Index}"); // 6
			dataList.Add( $"{vd.IsNumeric}"); // 7
			dataList.Add( $"{vd.GetType().Name}"); // 8
			dataList.Add( $"{vd.ToString()}"); // 9
			dataList.Add( $"{vd.MakeAmt("")?.GetType().Name ?? "is null"}"); // 10

			ShowData(dataList, infoList, order);
		}

		public List<Tuple<string[], int, int, bool>> ValDefInfoList(out int[] order)
		{
			List<Tuple<string[], int, int, bool>> infoList = new List<Tuple<string[], int, int, bool>>();

			AddToDefInfoList(infoList, ""        , "description"   , 25, 00);
			AddToDefInfoList(infoList, "val def" , "val str"       , 17, 01);
			AddToDefInfoList(infoList, ""        , "id"            , 05, 02);
			AddToDefInfoList(infoList, ""        , "val type"      , 20, 03);
			AddToDefInfoList(infoList, ""        , "data group"    , 15, 04);
			AddToDefInfoList(infoList, ""        , "order"         , 05, 05);
			AddToDefInfoList(infoList, ""        , "idx"           , 05, 06);
			AddToDefInfoList(infoList, "is"      , "numeric?"      , 08, 07);
			AddToDefInfoList(infoList, ""        , "type name"     , 20, 08);
			AddToDefInfoList(infoList, ""        , "to string"     , 40, 09);
			AddToDefInfoList(infoList, ""        , "makeAmt()"     ,-20, 10);

			order = GetOrder(infoList);

			return infoList;
		}

		public void ShowVarDef(AVarDef vd, 
			List<Tuple<string[], int, int, bool>> infoList, int[] order)
		{
			List<string> dataList = new List<string>();

			dataList.Add( $"{vd.Description}"); // 0
			dataList.Add( $"{vd.ValueStr}"); // 1
			dataList.Add( $"{vd.TokenStrTerm}"); // 2
			dataList.Add( $"{vd.Id}"); // 3
			dataList.Add( $"{vd.ValueType}"); // 4
			dataList.Add( $"{vd.DataGroup}"); // 5
			dataList.Add( $"{vd.Order}"); // 6
			dataList.Add( $"{vd.Index}"); // 7
			dataList.Add( $"{vd.IsNumeric}"); // 8
			dataList.Add( $"{vd.GetType().Name}"); // 9
			dataList.Add( $"{vd.ToString()}"); // 10
			dataList.Add( $"{vd.MakeAmt("")?.GetType().Name ?? "is null"}"); // 11

			ShowData(dataList, infoList, order);
		}

		public List<Tuple<string[], int, int, bool>> VarDefInfoList(out int[] order)
		{
			List<Tuple<string[], int, int, bool>> infoList = new List<Tuple<string[], int, int, bool>>();

			AddToDefInfoList(infoList, ""        , "description"   , 25, 00);
			AddToDefInfoList(infoList, "var def" , "val str"       , 09, 01);
			AddToDefInfoList(infoList, "token"   , "str"           , 05, 02);
			AddToDefInfoList(infoList, ""        , "id"            , 05, 03);
			AddToDefInfoList(infoList, ""        , "val type"      , 20, 04);
			AddToDefInfoList(infoList, ""        , "data group"    , 15, 05);
			AddToDefInfoList(infoList, ""        , "order"         , 05, 06);
			AddToDefInfoList(infoList, ""        , "idx"           , 05, 07);
			AddToDefInfoList(infoList, "is"      , "numeric?"      , 08, 08);
			AddToDefInfoList(infoList, ""        , "type name"     , 20, 09);
			AddToDefInfoList(infoList, ""        , "to string"     , 40, 10);
			AddToDefInfoList(infoList, ""        , "makeAmt()"     ,-20, 11);

			order = GetOrder(infoList);

			return infoList;
		}

	#endregion

	#region Show Parse Gen

		private const int PG_COL_A = 25;
		private const int PG_COL_C1 = 9;
		private const int PG_COL_E = 5;
		private const int PG_COL_F = 14;
		private const int PG_COL_X1 = 10;
		private const int PG_COL_X2 = 5;

		public void ShowParsGenHeader()
		{
			// win.WriteTab("");
			// win.Write($"{""                 , PG_COL_BX}");
			// win.Write($"{"val"              , PG_COL_C1}");
			// win.Write("\n");

			win.WriteTab("");
			win.Write($"{"**parse gen desc", -PG_COL_A} ");
			win.Write($"{"val str"         , -PG_COL_C1} ");
			win.Write($"{"id"              , -PG_COL_E} ");
			win.Write($"{"val type"        , -PG_COL_F} ");
			win.Write($"{"good?"           , -PG_COL_X1} ");
			win.Write($"{"count"           , -PG_COL_X2} ");
			win.Write("\n");

			win.WriteTab("");
			win.Write($"{"-".Repeat(PG_COL_A)} ");
			win.Write($"{"-".Repeat(PG_COL_C1)} ");
			win.Write($"{"-".Repeat(PG_COL_E)} ");
			win.Write($"{"-".Repeat(PG_COL_F)} ");
			win.Write($"{"-".Repeat(PG_COL_X1)} ");
			win.Write($"{"-".Repeat(PG_COL_X2)} ");
			win.Write("\n");
		}

		public bool ShowParseGen(ParseDef pg)
		{
			if (pg == null) return false;

			win.WriteTab("");
			win.Write($"{pg.Description, -PG_COL_A} ");
			win.Write($"{pg.ValueStr   , -PG_COL_C1} ");
			win.Write($"{pg.Id         , -PG_COL_E} ");
			win.Write($"{pg.ValueType  , -PG_COL_F} ");
			win.Write($"{pg.IsGood     , -PG_COL_X1} ");

			win.Write($"{pg.ValDefs?.Count ?? -1, -PG_COL_X2} ");

			win.Write("\n");

			return true;
		}

	#endregion

	#region show val-var defs

		private int GotValDef = 0;

		private int priorDef = -1;

		// public void ShowValDefs2D(AValDefBase ab)
		// {
		// 	if (ab == null) return;
		//
		// 	if (ab is ValDef)
		// 	{
		// 		if (GotValDef != 1)
		// 		{
		// 			GotValDef = 1;
		// 			ShowParsValDefsHeader();
		// 		}
		//
		// 		ShowParsVarDef((ValDef) ab);
		// 	}
		// 	else if (ab is VarDef)
		// 	{
		// 		if (GotValDef != -11)
		// 		{
		// 			GotValDef = -11;
		// 			ShowParsVarDefsHeader();
		// 		}
		//
		// 		ShowParsVarDef((VarDef) ab);
		// 		// ShowParsVarDefs2Dx((VarDef) ab);
		// 	}
		// 	else
		// 	{
		// 		// win.WriteLineTab($"** ADefBase2 *****************");
		// 		if (GotValDef != 1)
		// 		{
		// 			GotValDef = 1;
		// 			ShowParsValDefsHeader();
		// 		}
		//
		// 		ShowParsVarDef(ab);
		// 		// ShowParsValDefs2Dx(ab);
		// 	}
		// }

		private const int DEF_COL_A = 35;
		private const int DEF_COL_B = 20;
		private const int DEF_COL_BX = DEF_COL_A + DEF_COL_B;
		private const int DEF_COL_C1 = 11;
		private const int DEF_COL_C2 = 5;
		private const int DEF_COL_D = 5;
		private const int DEF_COL_E = 5;
		private const int DEF_COL_F = 33;
		private const int DEF_COL_F1 = 22;
		private const int DEF_COL_F2 = 10;
		private const int DEF_COL_G = 8;
		private const int DEF_COL_H = 10;
		private const int DEF_COL_I = 8;
		private const int DEF_COL_J = 33;
		private const int DEF_COL_J1 = 22;
		private const int DEF_COL_J2 = 10;
		private const int DEF_COL_X1 = 8;
		private const int DEF_COL_X2 = 15;

		public void ShowParsValDefsHeader()
		{
			if (priorDef != 1)
			{
				win.Write("\n");
				priorDef = 1;
			}

			win.WriteTab("");
			win.Write($"{"**val def desc", -DEF_COL_A} ");
			win.Write($"{"type"          , -DEF_COL_B} ");
			win.Write($"{"val str"       , -DEF_COL_C1 } ");
			win.Write($"{"id"            , -DEF_COL_E} ");
			win.Write($"{"val type"      , -DEF_COL_F} ");
			win.Write($"{"order"         , -DEF_COL_G} ");
			win.Write($"{"num?"          , -DEF_COL_I} ");
			win.Write($"{"Idx"           , -DEF_COL_X1} ");
			win.Write($"{"data grp"      , -DEF_COL_X2} ");
			win.Write("\n");

			win.WriteTab("");
			win.Write($"{"-".Repeat(DEF_COL_A)} ");
			win.Write($"{"-".Repeat(DEF_COL_B)} ");
			win.Write($"{"-".Repeat(DEF_COL_C1)} ");
			win.Write($"{"-".Repeat(DEF_COL_E)} ");
			win.Write($"{"-".Repeat(DEF_COL_F)} ");
			win.Write($"{"-".Repeat(DEF_COL_G)} ");
			win.Write($"{"-".Repeat(DEF_COL_I)} ");
			win.Write($"{"-".Repeat(DEF_COL_X1)} ");
			win.Write($"{"-".Repeat(DEF_COL_X2)} ");
			win.Write("\n");
		}

		public void ShowParsVarDef(AValDefBase vd)
		{
			if (vd == null) return;

			string num = $"[{(int) vd.ValueType}]";

			win.WriteTab("");
			win.Write($"{vd.Description   , -DEF_COL_A} ");
			win.Write($"{vd.GetType().Name, -DEF_COL_B} ");
			win.Write($"{vd.ValueStr      , -DEF_COL_C1} ");
			win.Write($"{vd.Id            , -DEF_COL_E} ");
			win.Write($"{vd.ValueType     , -DEF_COL_F1} {num,-DEF_COL_F2} ");
			win.Write($"{vd.Order         , -DEF_COL_G} ");
			win.Write($"{vd.IsNumeric     , -DEF_COL_I} ");
			win.Write($"{vd.Index         , -DEF_COL_X1} ");
			win.Write($"{vd.DataGroup     , -DEF_COL_X2} ");
			win.Write("\n");
			// Token t = vd.MakeToken("", 0, 0);

			// ShowToken(t, true);
		}

		// private void ShowParsVarDefsHeader()
		// {
		// 	if (priorDef != 2)
		// 	{
		// 		win.Write("\n");
		// 		priorDef = 2;
		// 	}
		//
		// 	win.WriteTab("");
		// 	win.Write($"{""               , -DEF_COL_A} ");
		// 	win.Write($"{""               , -DEF_COL_B} ");
		// 	win.Write($"{"val"            , -DEF_COL_C2} ");
		// 	win.Write($"{"term"           , -DEF_COL_D} ");
		// 	win.Write("\n");
		//
		// 	win.WriteTab("");
		// 	win.Write($"{"**var def desc", -DEF_COL_A} ");
		// 	win.Write($"{"type"          , -DEF_COL_B} ");
		// 	win.Write($"{"str"           , -DEF_COL_C2} ");
		// 	win.Write($"{"str"           , -DEF_COL_D} ");
		// 	win.Write($"{"id"            , -DEF_COL_E} ");
		// 	win.Write($"{"val type"      , -DEF_COL_F} ");
		// 	win.Write($"{"group"         , -DEF_COL_J} ");
		// 	win.Write($"{"order"         , -DEF_COL_G} ");
		// 	win.Write("\n");
		//
		// 	win.WriteTab("");
		// 	win.Write($"{"-".Repeat(DEF_COL_A)} ");
		// 	win.Write($"{"-".Repeat(DEF_COL_B)} ");
		// 	win.Write($"{"-".Repeat(DEF_COL_C2)} ");
		// 	win.Write($"{"-".Repeat(DEF_COL_D)} ");
		// 	win.Write($"{"-".Repeat(DEF_COL_E)} ");
		// 	win.Write($"{"-".Repeat(DEF_COL_F)} ");
		// 	win.Write($"{"-".Repeat(DEF_COL_J)} ");
		// 	win.Write($"{"-".Repeat(DEF_COL_G)} ");
		// 	win.Write("\n");
		// }
		//
		// public void ShowParsVarDef(VarDef pv)
		// {
		// 	if (pv == null) return;
		//
		// 	string num1 = $"[{(int) pv.ValueType}]";
		// 	string num2 = $"[{(int) pv.Group}]";
		//
		// 	win.WriteTab("");
		// 	win.Write($"{pv.Description , -DEF_COL_A} ");
		// 	win.Write($"{pv.GetType().Name, -DEF_COL_B} ");
		// 	win.Write($"{pv.ValueStr    , -DEF_COL_C2} ");
		// 	win.Write($"{pv.TokenStrTerm, -DEF_COL_D} ");
		// 	win.Write($"{pv.Id          , -DEF_COL_E} ");
		// 	win.Write($"{pv.ValueType   , -DEF_COL_F1} {num1, -DEF_COL_F2} ");
		// 	win.Write($"{pv.Group       , -DEF_COL_J1} {num2, -DEF_COL_J2} ");
		// 	win.Write($"{pv.Order       , -DEF_COL_G} ");
		// 	win.Write("\n");
		// }
	
	#endregion

	#endregion

	#region private methods

		private string AsAmount(Token t)
		{
			AAmtBase aa = t.AmountBase;

			string result = "";
			switch (t.ValDef.DataGroup)
			{
			case ValueDataGroup.VDG_BOOLEAN:
				{
					result = aa.AsBool().ToString();
					break;
				}
			// case ValueDataGroup.VDG_FUNCT:
			// 	{
			// 		result = aa.AsString();
			// 		break;
			// 	}
			case ValueDataGroup.VDG_NUM_DBL:
				{
					result = aa.AsDouble().ToString();
					break;
				}
			case ValueDataGroup.VDG_NUM_INT:
				{
					result = aa.AsInteger().ToString();
					break;
				}
			// case ValueDataGroup.VDG_NUM_FRACT:
			// 	{
			// 		result = aa.AsDouble().ToString();
			// 		break;
			// 	}
			case ValueDataGroup.VDG_STRING:
				{
					result = aa.AsString();
					break;
				}
			case ValueDataGroup.VDG_UNIT:
				{
					result = aa.AsUnit().ToString();
					break;
				}
			// case ValueDataGroup.VDG_VAR:
			// 	{
			// 		result = aa.AsString();
			// 		break;
			// 	}
			case ValueDataGroup.VDG_OBJECT:
				{
					result = aa.AsObject().ToString();
					break;
				}
		
			// case ValueDataGroup.VDG_INVALID:
			// case ValueDataGroup.VDG_DEFAULT:
			default:
				{
					result = aa.AsString();
					break;
				}
			}
		
			return result;
		}

		private const int SUFFIX = 3;
		private const int GAP = 3;

/*
l = 4
len = 5
suffix + gap = 6
length 

*/

		private string Truncate(string s, int l)
		{
			if (l < 0) return s;

			int len = s.Length;
			int div = l - SUFFIX - GAP - 1;

			if (len <= l) return s;
			if (len <= SUFFIX + GAP + 1 || div < 1) return s.Substring(0, l);


			return s.Substring(0, div) + " … " + s.Substring(len - SUFFIX, SUFFIX);
		}

		private int[] GetOrder(List<Tuple<string[], int, int, bool>> infoList)
		{
			int[] order = new int[infoList.Count];

			for (var i = 0; i < infoList.Count; i++)
			{
				// order[i] = infoList[i].Item3;
				order[infoList[i].Item3] = i;
			}

			return order;
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is ShowResults";
		}

	#endregion

		// public void ShowToken(Token t, bool abbreviated)
		// {
		// 	if (t == null)
		// 	{
		// 		win.WriteLineTab($"{"token"         ,titleWidthD}| is null");
		// 		return;
		// 	}
		//
		//
		// 	win.WriteLineTab($"{"t to string"       ,titleWidthD}| {t.ToString()}");
		// 	if (abbreviated) return;
		//
		// 	if (t.IsRefIdx)
		// 	{
		// 		win.WriteLineTab($"{"ref index"     ,titleWidthD}| {t.RefIdx}");
		// 	}
		// 	else
		// 	{
		// 		win.WriteLineTab($"{"position"      ,titleWidthD}| {t.Position}");
		// 	}
		//
		// 	win.WriteLineTab($"{"len"               ,titleWidthD}| {t.Length}");
		// 	win.WriteLineTab($"{"level"             ,titleWidthD}| {t.Level}");
		//
		// 	win.WriteLineTab($"*** Amount follows ***");
		// 	win.WriteLineTab($"{"token/valid?"      ,titleWidthD}| {t.AmountBase.IsValid}");
		// 	win.WriteLineTab($"{"as amt"            ,titleWidthD}| {AsAmount(t.AmountBase)}");
		// 	win.WriteLineTab($"{"as string"         ,titleWidthD}| {t.AmountBase.AsString()}");
		// 	win.WriteLineTab($"{"to string"         ,titleWidthD}| {t.AmountBase.ToString()}");
		// 	win.WriteLineTab($"{"type"              ,titleWidthD}| {t.AmountBase.GetType().Name}");
		// 	win.WriteLineTab($"{"as amt type"       ,titleWidthD}| {AsAmount(t.AmountBase).GetType()}");
		//
		// 	win.WriteLineTab($"*** value def follows ***");
		// 	win.WriteLineTab($"{"value str"         ,titleWidthD}| {t.AmountBase.ValueDef.ValueStr}");
		// 	win.WriteLineTab($"{"order"             ,titleWidthD}| {t.AmountBase.ValueDef.Order}");
		// 	win.WriteLineTab($"{"index"             ,titleWidthD}| {t.AmountBase.ValueDef.Index}");
		// 	win.WriteLineTab($"{"value desc"        ,titleWidthD}| {t.AmountBase.ValueDef.Description}");
		// 	win.WriteLineTab($"{"value type"        ,titleWidthD}| {t.AmountBase.ValueType}");
		// 	win.WriteLineTab($"{"data group"        ,titleWidthD}| {t.AmountBase.ValueDef.DataGroup}");
		// 	win.WriteLineTab($"{"is numeric?"       ,titleWidthD}| {t.AmountBase.ValueDef.IsNumeric}");
		// 	win.WriteLineTab($"{"vd to string"      ,titleWidthD}| {t.AmountBase.ValueDef.ToString()}");
		// }


		// public void ShowAToken2(Token t)
		// {
		// 	int[] order = new int[0];
		//
		// 	List<Tuple<string[], int>> tkDesc = ShowToken2List(out order);
		//
		// 	win.TabDn(1);
		// 	ShowHeader(tkDesc, order);
		// 	ShowToken2(t, tkDesc, order);
		// 	win.TabDn(1);
		// 	win.WriteLine("");
		// }

		
		// public void ShowParsePh1Data(ParseData pd1)
		// {
		// 	win.WriteLine("");
		// 	ShowParsePh1DataHeader();
		// 	win.WriteLineTab($"|{pd1.Name, -5}\t|\t{pd1.Value, -5}\t|\t{pd1.Position, -8}\t|\t{pd1.Length, -6}\t|\t{pd1.IsValueDef, -8}");
		// 	win.WriteLine("");
		// 	win.TabUp("@1");
		// 	ShowValDefs2D(pd1.Definition);
		// 	win.TabDn("@1");
		// 	win.WriteLine("");
		// }

		// private const int titleWidthD = -20;
		// private const int fieldWidthD = -23;
		//
		// public void ShowParsePh1DataHeader()
		// {
		// 	win.WriteLineTab($"|{"Name"  , -5}\t|\t{"Value"  , -5}\t|\t{"position"  , -8}\t|\t{"length"  , -6}\t|\t{"isValDef?"   , -8}");
		// }

	}
}