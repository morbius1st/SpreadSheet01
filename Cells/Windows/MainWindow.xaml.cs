#region using

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Cells.CellsTests;
using SpreadSheet01.RevitSupport;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitParamValue;
using UtilityLibrary;

#endregion

// projname: Cells
// itemname: MainWindow
// username: jeffs
// created:  2/28/2021 5:54:07 AM

namespace Cells.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
	#region private fields

		public RevitParamTest RevitParamTests { get; } = new RevitParamTest();

		public RevitCellSystManager RevitChartMgr { get; } = new RevitCellSystManager();

		public RevitManager RevitMgr { get; } = new RevitManager();

		public RevitChartTests RevitChartTests { get; } = new RevitChartTests();

		private static MainWindow me;

	#endregion

	#region ctor

		public MainWindow()
		{
			me = this;

			InitializeComponent();

		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

		private static int tabs = 0;
		private static string tabId = null;

		public static bool showTabId = true;

		private static void listTabId()
		{
			if (tabId != null && showTabId)
			{
				WriteLine(tabId);
			}
		}

		public static  void TabUp(int id = -1)
		{
			tabs++;
			tabId = id.ToString("D3") + " up";
			listTabId();
		}

		public static void TabDn(int id = -1)
		{
			tabs--;
			tabId = id.ToString("D3") + " dn";
			listTabId();
		}

		public static void TabClr(int id = -1)
		{
			tabs = 0;
			tabId = id.ToString("D3") + " clr";
			listTabId();
		}

		public static void TabSet(int t)
		{
			tabs = t;
		}

		public static void WriteLineTab(string msg)
		{
			WriteTab(msg + "\n");
		}

		public static void WriteTab(string msg)
		{
			me.TbxMsg.Text += 
				// (tabId > 0 ? tabId.ToString("D3") : "") + 
				(tabs > 0 ? "    ".Repeat(tabs) : "" ) + msg;
		}
		
		public static void WriteLine(string msg)
		{
			Write(msg + "\n");
		}

		public static void Write(string msg)
		{
			me.TbxMsg.Text += msg;
		}

	#endregion

	#region private methods

	#endregion

	#region event consuming

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@Debug");
		}
		
		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			WriteLineTab("Loaded...");

			bool result;

			// test that the params have been defined
			// RevitParamTests.Process();

			// get all of the revit chart families and 
			// process each to get its parameters
			result = RevitChartMgr.GetCurrentCharts();

			if (!result) return;

			// RevitChartMgr.listCharts();


			RevitMgr.ProcessCharts(RevitChartMgr.Charts, CellUpdateTypeCode.ALL);

			RevitChartMgr.listCharts2();

			// old - load the parameter descriptions
			// RevitChartTests.Process();

			// test the refit parameters
			// RevitParamTests.Process();


			// RevitMgr.ProcessAlwaysCharts();

			OnPropertyChange("RevitParamTests");
		}

	#endregion

	#region event publishing

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is MainWindow";
		}

	#endregion


	}
}