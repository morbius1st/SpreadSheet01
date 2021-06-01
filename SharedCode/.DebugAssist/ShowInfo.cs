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
using SharedCode.EquationSupport.TokenSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;
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
			new Lazy<ShowInfo>(()=> new ShowInfo());

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

		private const int titleWidthD = -20;
		private const int fieldWidthD = -23;

		public void ShowParsePh1DataHeader()
		{
			win.WriteLineTab($"|{"Name"  , -5}\t|\t{"Value"  , -5}\t|\t{"position"  , -8}\t|\t{"length"  , -6}\t|\t{"isValDef?"   , -8}");
		}

		public void ShowParsePh1Data(ParsePhData pd1)
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
			win.WriteLineTab($"{"id"         ,titleWidthD}| {pg.Id}");
			win.WriteLineTab($"{"val type"   ,titleWidthD}| {pg.ValueType}");
			win.WriteLineTab($"{"isgood"     ,titleWidthD}| {pg.IsGood}");
			win.WriteLineTab($"{"aDefBase count",titleWidthD}| {pg.aDefBase2?.Count ?? -1}");

			return true;
		}

		public void ShowParsVarDefs2D(ADefBase2 ab)
		{
			if (ab == null) return;

			if (ab is ValDef)
			{
				ShowParsVarDefs2Dx((ValDef) ab);
			} 
			else if (ab is VarDef)
			{
				ShowParsVarDefs2Dx((VarDef) ab);
			}
			else
			{
				win.WriteLineTab($"** ADefBase2 *****************");
				ShowParsVarDefs2Dx(ab);
			}
		}

		public void ShowParsVarDefs2Dx(ADefBase2 vd) 
		{
			if (vd == null) return;

			win.WriteLineTab($"{"** value def" ,titleWidthD}| {vd.Description}");
			win.WriteLineTab($"{"type"      ,titleWidthD}| {vd.GetType()}");
			win.WriteLineTab($"{"val str"   ,titleWidthD}| {vd.ValueStr}");
			win.WriteLineTab($"{"val type"  ,titleWidthD}| {vd.ValueType} ({(int) vd.ValueType})");
			win.WriteLineTab($"{"id"        ,titleWidthD}| {vd.Id}");
			win.WriteLineTab($"{"order"     ,titleWidthD}| {vd.Order}");
			win.WriteLineTab($"{"is numeric",titleWidthD}| {vd.IsNumeric}");

			Token t = vd.MakeToken("", 0, 0);

			ShowToken(t, true);
		}

		public void ShowParsVarDefs2Dx(VarDef pv)
		{
			if (pv == null) return;

			win.WriteLineTab($"{"** parse var",titleWidthD}| {pv.Description}");
			win.WriteLineTab($"{"type"     ,titleWidthD}| {pv.GetType()}");
			win.WriteLineTab($"{"val str"  ,titleWidthD}| {pv.ValueStr}");
			win.WriteLineTab($"{"term str" ,titleWidthD}| {pv.TokenStrTerm}");
			win.WriteLineTab($"{"val type" ,titleWidthD}| {pv.ValueType}");
			win.WriteLineTab($"{"id"       ,titleWidthD}| {pv.Id}");
			win.WriteLineTab($"{"val type" ,titleWidthD}| {pv.Order}");
		}

		public void ShowToken(Token t, bool abbreviated)
		{
			if (t == null)
			{
				win.WriteLineTab($"{"token",titleWidthD}| is null");
				return;
			}

			win.WriteLineTab($"{"token/valid?",titleWidthD}| {t.AmountBase.IsValid}");

			if (abbreviated) return;

			win.WriteLineTab($"{"value str"    ,titleWidthD}| {t.AmountBase.ValueDef.ValueStr}");
			win.WriteLineTab($"{"as string"    ,titleWidthD}| {t.AmountBase.AsString()}");
			win.WriteLineTab($"{"as amt"       ,titleWidthD}| {AsAmount(t.AmountBase)}");
			win.WriteLineTab($"{"type"         ,titleWidthD}| {t.AmountBase.GetType().Name}");
			win.WriteLineTab($"{"position"     ,titleWidthD}| {t.Position}");
			win.WriteLineTab($"{"len"          ,titleWidthD}| {t.Length}");
			win.WriteLineTab($"{"data type"    ,titleWidthD}| {t.AmountBase.DataType}");
			win.WriteLineTab($"{"value desc"   ,titleWidthD}| {t.AmountBase.ValueDef.Description}");
			win.WriteLineTab($"{"data group"   ,titleWidthD}| {t.AmountBase.ValueDef.DataGroup}");
		}



	#endregion

	#region private methods

		private string AsAmount(AAmtBase aa)
		{
			string result = "";
			switch (aa.ValueDef.DataGroup)
			{
			case ValueDataGroup.VDG_BOOLEAN:
				{
					result = aa.AsBool().ToString();
					break;
				}
			case ValueDataGroup.VDG_FUNCT:
				{
					result = aa.ToString();
					break;
				}
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
			case ValueDataGroup.VDG_NUM_FRACT:
				{
					result = aa.AsDouble().ToString();
					break;
				}
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
			case ValueDataGroup.VDG_VAR:
				{
					result = aa.AsString();
					break;
				}
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