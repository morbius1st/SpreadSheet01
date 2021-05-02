#region using
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

#endregion

// projname: CellsTest
// itemname: App
// username: jeffs
// created:  2/28/2021 5:54:07 AM

namespace CellsTest
{
	public partial class App : Application
	{
	
		private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			// Process unhandled exception

			// Prevent default unhandled exception processing
			e.Handled = true;

			// CellsTest.Windows.MainWindow.myTrace.Flush();
		}
	}
}
