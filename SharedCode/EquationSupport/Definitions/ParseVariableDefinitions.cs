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
	public class ParseVariableDefinitions  : DefinitionBase<ParseVar>
	{
		private const int MAX_TOKENS = 20;

		static ParseVariableDefinitions() { Init(); }

		public override ParseVar Invalid => new ParseVar("Invalid", null, null, VT_INVALID,    PGV_INVALID, (int) PGV_DEFAULT);
		public override ParseVar Default => new ParseVar("Default", null, null, VT_DEFAULT, PGV_DEFAULT, (int) PGV_DEFAULT);

		// protected override void Initialize() { }

		private static void Init()
		{
			idDefArray = new ParseVar[MAX_TOKENS];

			int idx = 0;
			int id = 1;

			idDefArray[idx++] = new ParseVar("Excel Address"    , "{[", "]}" , VT_STRING, PGV_EXCL_ADDR, id++);
			idDefArray[idx++] = new ParseVar("System Variable"  , "{$", "}"  , VT_STRING, PGV_SYS_VAR,   id++);
			idDefArray[idx++] = new ParseVar("Revit Parameter"  , "{#", "}"  , VT_STRING, PGV_RVT_PARAM, id++);
			idDefArray[idx++] = new ParseVar("Project Parameter", "{%", "}"  , VT_STRING, PGV_PRJ_PARAM, id++);
			idDefArray[idx++] = new ParseVar("Global Parameter" , "{!", "}"  , VT_STRING, PGV_GBL_PARAM, id++);
			idDefArray[idx++] = new ParseVar("Label Name"       , "{@", "}"  , VT_STRING, PGV_LBL_NAME,  id++);

			count = idx;
		}
	}
}
