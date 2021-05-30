#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.Definitions.ParseGroupVar;
using static SharedCode.EquationSupport.Definitions.ValueType;


#endregion

// user name: jeffs
// created:   5/17/2021 6:40:59 PM

namespace SharedCode.EquationSupport.Definitions
{
	public class VariableDefinitions  : DefinitionBase<ParseVar>
	{
		private const int MAX_TOKENS = 10;

		private static readonly Lazy<VariableDefinitions> instance =
			new Lazy<VariableDefinitions>(()=> new VariableDefinitions());

		static VariableDefinitions() { Init(); }

		public static VariableDefinitions PvDefInst => instance.Value;

		public ParseVar[] PvDefs => idDefArray;

		public ParseVar this[int idx] => PvDefs[idx];

		public string ValueStr(int idx) => PvDefs[idx].ValueStr;

		public override ParseVar Invalid => new ParseVar("Invalid", null, null, VT_INVALID,    PGV_INVALID, (int) PGV_DEFAULT);
		public override ParseVar Default => new ParseVar("Default", null, null, VT_DEFAULT, PGV_DEFAULT, (int) PGV_DEFAULT);

		public static int Pvd_XcellAddr;
		public static int Pvd_SysVar;
		public static int Pvd_RvtParam;
		public static int Pvd_PrjParam;
		public static int Pvd_GblParam;
		public static int Pvd_LabelName;

		private static void Init()
		{
			idDefArray = new ParseVar[MAX_TOKENS];

			int idx = 0;
			int id = 1;

			Pvd_XcellAddr = idx++;
			idDefArray[Pvd_XcellAddr] = new ParseVar("Excel Address"    , "{[", "]}" , VT_STRING, PGV_EXCL_ADDR, id++);

			Pvd_SysVar = idx++;
			idDefArray[Pvd_SysVar] = new ParseVar("System Variable"  , "{$", "}"  , VT_STRING, PGV_SYS_VAR,   id++);

			Pvd_RvtParam = idx++;
			idDefArray[Pvd_RvtParam] = new ParseVar("Revit Parameter"  , "{#", "}"  , VT_STRING, PGV_RVT_PARAM, id++);

			Pvd_PrjParam = idx++;
			idDefArray[Pvd_PrjParam] = new ParseVar("Project Parameter", "{%", "}"  , VT_STRING, PGV_PRJ_PARAM, id++);

			Pvd_GblParam = idx++;
			idDefArray[Pvd_GblParam] = new ParseVar("Global Parameter" , "{!", "}"  , VT_STRING, PGV_GBL_PARAM, id++);

			Pvd_LabelName = idx++;
			idDefArray[Pvd_LabelName] = new ParseVar("Label Name"       , "{@", "}"  , VT_STRING, PGV_LBL_NAME,  id++);

			count = idx;
		}
	}
}
