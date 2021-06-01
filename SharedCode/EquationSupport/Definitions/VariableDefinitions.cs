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
	public class VariableDefinitions  : ADefinitionBase<VarDef>
	{
		private const int MAX_TOKENS = 10;

		private static readonly Lazy<VariableDefinitions> instance =
			new Lazy<VariableDefinitions>(()=> new VariableDefinitions());

		static VariableDefinitions() { Init(); }

		public static VariableDefinitions VarDefInst => instance.Value;

		// public DefVar[] PvDefs => idDefArray;

		// public DefVar this[int idx] => PvDefs[idx];

		// public string ValueStr(int idx) => idDefArray[idx].ValueStr;

		public override VarDef Invalid => new VarDef(-1, "Invalid", null, null, VT_INVALID, PGV_INVALID,/* -1,*/ -1);
		public override VarDef Default => new VarDef(0, "Default", null, null, VT_DEFAULT, PGV_DEFAULT, /*0,*/ 0);

		public static int Pvd_XcellAddr;
		public static int Pvd_SysVar;
		public static int Pvd_RvtParam;
		public static int Pvd_PrjParam;
		public static int Pvd_GblParam;
		public static int Pvd_LabelName;

		private static void Init()
		{
			idDefArray = new VarDef[MAX_TOKENS];

			int idx = 0;
			int seq = 1;

			Pvd_XcellAddr =
				MakeDefVar(idx++,/* seq++,*/ "Excel Address"    , "{[", "]}", PGV_EXCL_ADDR);
			Pvd_SysVar =
				MakeDefVar(idx++,/* seq++,*/ "System Variable"  , "{$", "}", PGV_SYS_VAR);
			Pvd_RvtParam =
				MakeDefVar(idx++, /*seq++,*/ "Revit Parameter"  , "{#", "}", PGV_RVT_PARAM);
			Pvd_PrjParam =
				MakeDefVar(idx++,/* seq++,*/ "Project Parameter", "{%", "}", PGV_PRJ_PARAM);
			Pvd_GblParam =
				MakeDefVar(idx++, /*seq++,*/ "Global Parameter" , "{!", "}", PGV_GBL_PARAM);
			Pvd_LabelName =
				MakeDefVar(idx++, /*seq++,*/ "Label Name"       , "{@", "}", PGV_LBL_NAME);


			// Pvd_XcellAddr = idx++;
			// idDefArray[Pvd_XcellAddr] = new DefVar("Excel Address"    , "{[", "]}" , VT_STRING, PGV_EXCL_ADDR, seq, (int) VT_ID_VAR_KEY + seq++);
			//
			// Pvd_SysVar = idx++;
			// idDefArray[Pvd_SysVar] = new DefVar("System Variable"  , "{$", "}"  , VT_STRING, PGV_SYS_VAR, seq++, 0);
			//
			// Pvd_RvtParam = idx++;
			// idDefArray[Pvd_RvtParam] = new DefVar("Revit Parameter"  , "{#", "}"  , VT_STRING, PGV_RVT_PARAM, seq++, 0);
			//
			// Pvd_PrjParam = idx++;
			// idDefArray[Pvd_PrjParam] = new DefVar("Project Parameter", "{%", "}"  , VT_STRING, PGV_PRJ_PARAM, seq++, 0);
			//
			// Pvd_GblParam = idx++;
			// idDefArray[Pvd_GblParam] = new DefVar("Global Parameter" , "{!", "}"  , VT_STRING, PGV_GBL_PARAM, seq++, 0);
			//
			// Pvd_LabelName = idx++;
			// idDefArray[Pvd_LabelName] = new DefVar("Label Name"       , "{@", "}"  , VT_STRING, PGV_LBL_NAME, seq++, 0);

			count = idx;
		}

		private static int MakeDefVar(int idx, /*int seq,*/ string desc, string val, string term, ParseGroupVar pgv)
		{

			idDefArray[idx] = new VarDef(idx, desc, val, term, VT_STRING, pgv,/* seq,*/ (int) VT_ID_VAR_KEY/* + seq*/);

			return idx;
		}

	}

	
}
