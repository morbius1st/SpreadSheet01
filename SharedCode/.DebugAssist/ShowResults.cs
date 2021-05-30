#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.ParseSupport;

#endregion

// username: jeffs
// created:  5/29/2021 6:26:42 AM

namespace SharedCode.DebugAssist
{
	public class ShowResults
	{
	#region private fields

		private ISendMessages win;

		private static readonly Lazy<ShowResults> instance =
			new Lazy<ShowResults>(()=> new ShowResults());

	#endregion

	#region ctor

		public ShowResults() { }

	#endregion

	#region public properties

		public static ShowResults Inst => instance.Value;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public void SetMessenger(ISendMessages win)
		{
			this.win = win;
		}

		private const int titleWidthD = -20;
		private const int fieldWidthD = -23;

		public void ShowParsePh1DataHeader()
		{
			win.WriteLineTab($"|{"Name"  , -5}\t|\t{"Value"  , -5}\t|\t{"position"  , -8}\t|\t{"length"  , -6}\t|\t{"isValDef?"   , -8}");
		}

		public void ShowParsePh1Data(ParsePh1Data pd1)
		{
			win.WriteLine("");
			ShowParsePh1DataHeader();
			win.WriteLineTab($"|{pd1.Name, -5}\t|\t{pd1.Value, -5}\t|\t{pd1.Position, -8}\t|\t{pd1.Length, -6}\t|\t{pd1.IsValueDef, -8}");
			win.WriteLine("");
			win.TabUp("@1");
			ShowParsVarDefs2D(pd1.Definition);
			win.TabDn("@1");
			win.WriteLine("");
		}



		public bool ShowParseGen(ParseGen pg)
		{
			if (pg == null) return false;

			win.WriteLineTab($"{"parse gen"  ,titleWidthD}| {pg.Description}");
			win.WriteLineTab($"{"val str"    ,titleWidthD}| {pg.ValueStr}");
			win.WriteLineTab($"{"group"      ,titleWidthD}| {pg.Group}");
			win.WriteLineTab($"{"id"         ,titleWidthD}| {pg.Id}");
			win.WriteLineTab($"{"val type"   ,titleWidthD}| {pg.ValueType}");
			win.WriteLineTab($"{"isgood"     ,titleWidthD}| {pg.IsGood}");
			win.WriteLineTab($"{"aDefBase count",titleWidthD}| {pg.aDefBase2?.Count ?? -1}");

			return true;
		}

		public void ShowParsVarDefs2D(ADefBase ab)
		{
			if (ab == null) return;

			if (ab is DefValue)
			{
				ShowParsVarDefs2D((DefValue) ab);
			} 
			else if (ab is DefVar)
			{
				ShowParsVarDefs2D((DefVar) ab);
			}
			else
			{
				win.WriteLineTab($"{"ADefBase",titleWidthD}| {ab.Description}");
				win.WriteLineTab($"{"val str" ,titleWidthD}| {ab.ValueStr}");
				win.WriteLineTab($"{"type"    ,titleWidthD}| {ab.GetType()}");
				win.WriteLineTab($"{"id"      ,titleWidthD}| {ab.Id}");
				win.WriteLineTab($"{"val type",titleWidthD}| {ab.ValueType}");
			}
		}

		public void ShowParsVarDefs2D(DefValue vd) 
		{
			if (vd == null) return;

			win.WriteLineTab($"{"value def" ,titleWidthD}| {vd.Description}");
			win.WriteLineTab($"{"val str"   ,titleWidthD}| {vd.ValueStr}");
			win.WriteLineTab($"{"val type"  ,titleWidthD}| {vd.ValueType}");
			win.WriteLineTab($"{"id"        ,titleWidthD}| {vd.Id}");
			win.WriteLineTab($"{"seq"       ,titleWidthD}| {vd.Seq}");
			win.WriteLineTab($"{"order"     ,titleWidthD}| {vd.Order}");
			win.WriteLineTab($"{"is numeric",titleWidthD}| {vd.IsNumeric}");
			win.WriteLineTab($"{"type"      ,titleWidthD}| {vd.GetType()}");

		}

		public void ShowParsVarDefs2D(DefVar pv)
		{
			if (pv == null) return;

			win.WriteLineTab($"{"parse var",titleWidthD}| {pv.Description}");
			win.WriteLineTab($"{"val str"  ,titleWidthD}| {pv.ValueStr}");
			win.WriteLineTab($"{"term str" ,titleWidthD}| {pv.TokenStrTerm}");
			win.WriteLineTab($"{"val type" ,titleWidthD}| {pv.ValueType}");
			win.WriteLineTab($"{"id"       ,titleWidthD}| {pv.Id}");
			win.WriteLineTab($"{"type"     ,titleWidthD}| {pv.GetType()}");
		}


	#endregion

	#region private methods

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
	}
}