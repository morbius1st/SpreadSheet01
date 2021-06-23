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
	
	// public class VariableDefinitions  : ADefinitionBase<VarDef>
	// {
	// 	private const int MAX_TOKENS = 10;
	//
	// 	private static readonly Lazy<VariableDefinitions> instance =
	// 		new Lazy<VariableDefinitions>(()=> new VariableDefinitions());
	//
	// 	static VariableDefinitions() { Init(); }
	//
	// 	public static VariableDefinitions VarDefInst => instance.Value;
	//
	// 	// public DefVar[] PvDefs => idDefArray;
	//
	// 	// public DefVar this[int idx] => PvDefs[idx];
	//
	// 	// public string ValueStr(int idx) => idDefArray[idx].ValueStr;
	//
	// 	public override VarDef Invalid => new VarDef(-1, "Invalid", null, null, VT_INVALID, PGV_INVALID, -1);
	// 	public override VarDef Default => new VarDef(0, "Default", null, null, VT_DEFAULT, PGV_DEFAULT, 0);
	//
	// 	public static int Pvd_XcellAddr;
	// 	public static int Pvd_SysVar;
	// 	public static int Pvd_RvtParam;
	// 	public static int Pvd_PrjParam;
	// 	public static int Pvd_GblParam;
	// 	public static int Pvd_LabelName;
	//
	// 	private static void Init()
	// 	{
	// 		idDefArray = new VarDef[MAX_TOKENS];
	//
	// 		int idx = 0;
	// 		int seq = 1;
	//
	// 		Pvd_XcellAddr =
	// 			SetValue(new DefVarKeyXlAddr(idx++  , "Variable XlAddr"       , "{[", "]}", VT_ID_VAR_KEY, PGV_EXCL_ADDR, 1));
	// 		Pvd_SysVar =
	// 			SetValue(new DefVarKeySysVar(idx++  , "Variable System Var"   , "{$", "}",  VT_ID_VAR_KEY, PGV_SYS_VAR  , 2));
	// 		Pvd_RvtParam =
	// 			SetValue(new DefVarKeyRvtParam(idx++, "Variable Revit Param"  , "{#", "}",  VT_ID_VAR_KEY, PGV_RVT_PARAM, 3));
	// 		Pvd_PrjParam =
	// 			SetValue(new DefVarKeyPrjParam(idx++, "Variable Project Param", "{%", "}",  VT_ID_VAR_KEY, PGV_PRJ_PARAM, 4));
	// 		Pvd_GblParam =
	// 			SetValue(new DefVarKeyGblParam(idx++, "Variable Global Param" , "{!", "}",  VT_ID_VAR_KEY, PGV_GBL_PARAM, 5));
	// 		Pvd_LabelName =
	// 			SetValue(new DefVarKeyLblName(idx++ , "Variable Label Param"  , "{@", "}",  VT_ID_VAR_KEY, PGV_LBL_NAME , 6));
	//
	//
	//
	//
	//
	//
	// 		// Pvd_XcellAddr =
	// 		// 	SetValue(idx++,/* seq++,*/ "Excel Address"    , "{[", "]}", PGV_EXCL_ADDR);
	// 		// Pvd_SysVar =
	// 		// 	SetValue(idx++,/* seq++,*/ "System Variable"  , "{$", "}", PGV_SYS_VAR);
	// 		// Pvd_RvtParam =
	// 		// 	SetValue(idx++, /*seq++,*/ "Revit Parameter"  , "{#", "}", PGV_RVT_PARAM);
	// 		// Pvd_PrjParam =
	// 		// 	SetValue(idx++,/* seq++,*/ "Project Parameter", "{%", "}", PGV_PRJ_PARAM);
	// 		// Pvd_GblParam =
	// 		// 	SetValue(idx++, /*seq++,*/ "Global Parameter" , "{!", "}", PGV_GBL_PARAM);
	// 		// Pvd_LabelName =
	// 		// 	SetValue(idx++, /*seq++,*/ "Label Name"       , "{@", "}", PGV_LBL_NAME);
	//
	//
	// 		// Pvd_XcellAddr = idx++;
	// 		// idDefArray[Pvd_XcellAddr] = new DefVar("Excel Address"    , "{[", "]}" , VT_STRING, PGV_EXCL_ADDR, seq, (int) VT_ID_VAR_KEY + seq++);
	// 		//
	// 		// Pvd_SysVar = idx++;
	// 		// idDefArray[Pvd_SysVar] = new DefVar("System Variable"  , "{$", "}"  , VT_STRING, PGV_SYS_VAR, seq++, 0);
	// 		//
	// 		// Pvd_RvtParam = idx++;
	// 		// idDefArray[Pvd_RvtParam] = new DefVar("Revit Parameter"  , "{#", "}"  , VT_STRING, PGV_RVT_PARAM, seq++, 0);
	// 		//
	// 		// Pvd_PrjParam = idx++;
	// 		// idDefArray[Pvd_PrjParam] = new DefVar("Project Parameter", "{%", "}"  , VT_STRING, PGV_PRJ_PARAM, seq++, 0);
	// 		//
	// 		// Pvd_GblParam = idx++;
	// 		// idDefArray[Pvd_GblParam] = new DefVar("Global Parameter" , "{!", "}"  , VT_STRING, PGV_GBL_PARAM, seq++, 0);
	// 		//
	// 		// Pvd_LabelName = idx++;
	// 		// idDefArray[Pvd_LabelName] = new DefVar("Label Name"       , "{@", "}"  , VT_STRING, PGV_LBL_NAME, seq++, 0);
	//
	// 		count = idx;
	// 	}
	//
	// 	private static int SetValue(VarDef vd2)
	// 	{
	// 		idDefArray[vd2.Index] = vd2;
	//
	// 		return vd2.Index;
	// 	}
	//
	// 	private static int SetValue(int idx, /*int seq,*/ string desc, string val, string term, ParseGroupVar pgv)
	// 	{
	// 	
	// 		idDefArray[idx] = new VarDef(idx, desc, val, term, VT_STRING, pgv,/* seq,*/ (int) VT_ID_VAR_KEY/* + seq*/);
	// 	
	// 		return idx;
	// 	}
	//
	// }

	
}
