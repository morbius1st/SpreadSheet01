#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RevitSupport.RevitChartManagement;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitParamManagement;

#endregion

// username: jeffs
// created:  4/17/2021 6:05:16 AM

namespace SharedCode.RevitSupport.RevitManagement
{
	public class RevitSystemManager
	{
	#region private fields

		private RevitChartManager chartMgr;
		private ManagementSupport mgmtSupport;

	#endregion

	#region ctor

		public RevitSystemManager()
		{
			chartMgr = new RevitChartManager();
			mgmtSupport = new ManagementSupport();

		}

	#endregion

	#region public properties

		public RevitCharts Charts => chartMgr.Charts;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool CollectCharts(CellUpdateTypeCode which)
		{
			if (!collectCharts(which))
			{
				showChartErrors();
				return false;
			}

			return true;
		}


	#endregion

	#region private methods


		private bool collectCharts(CellUpdateTypeCode which)
		{
			if (!chartMgr.CollectAllCharts()) return false;

			chartMgr.PreProcessCharts(which);

			if (chartMgr.Charts.HasErrors) return false;

			return true;
		}

		private void showChartErrors()
		{
			StringBuilder sb = new StringBuilder();

			foreach (ErrorCodes ec in chartMgr.Charts.ErrorCodeList)
			{
				sb.AppendLine(ec.ToString());
			}

			mgmtSupport.ErrorChartErrors(sb.ToString());
		}


	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is RevitSystemManager";
		}

	#endregion
	}
}