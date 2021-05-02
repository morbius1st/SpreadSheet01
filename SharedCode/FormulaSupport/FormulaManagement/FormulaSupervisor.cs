#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using RevitSupport.RevitChartManagement;
using SpreadSheet01.RevitSupport.RevitCellsManagement;

#endregion

// username: jeffs
// created:  4/18/2021 9:17:22 PM

namespace SharedCode.FormulaSupport.FormulaManagement
{
	public class FormulaSupervisor
	{
	#region private fields

		private RevitChartManager _chartMgr;

	#endregion

	#region ctor

		// FormulaDirector		fmDir
		// FormulaSupervisor	fmSuper
		// FormulaNavigator		fmNav
		// FormulaPrincipal		fmPrinc
		// FormulaAdministrator	fmAdmin
		// FormulaLeader		fmLead

		public FormulaSupervisor(RevitChartManager chartMgr)
		{
			_chartMgr = chartMgr;
		}

	#endregion

	#region public properties

		public SortedDictionary<string, RevitLabel> AllCellLabels  { get; private set; }
			= new SortedDictionary<string, RevitLabel>();

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool Preprocess()
		{

			return true;
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
			return "this is FormulaManager";
		}

	#endregion
	}
}