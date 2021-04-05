#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   3/28/2021 2:46:58 PM

namespace SharedCode.DebugAssist
{

	public interface ISendMessages
	{
		bool showTabId { get; set; }
		void listTabId();
		void TabUp(string id = "");
		void TabDn(string id = "");
		void TabClr(string id = "");
		void TabSet(int t);
		void WriteLineTab(string msg);
		void WriteTab(string msg);
		void WriteLine(string msg);
		void Write(string msg);

	}

}
