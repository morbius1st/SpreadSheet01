#region using

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using CellsTest.CellsTests;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitParamManagement;
using UtilityLibrary;

#endregion

// projname: CellsTest
// itemname: MainWindow
// username: jeffs
// created:  2/28/2021 5:54:07 AM

namespace CellsTest.Windows
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
	#region private fields

		// public RevitParamTest RevitParamTests { get; } = new RevitParamTest();

		

		private ListiInfo listInfo = new ListiInfo();

		private static MainWindow me;

		private Tests01 tests01 = new Tests01();

	#endregion

	#region ctor

		public MainWindow()
		{
			me = this;

			InitializeComponent();
		}

	#endregion

	#region public properties

		public RevitSystemManager RevitSystMgr { get; } = new RevitSystemManager();

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

		public static  void TabUp(string id)
		{
			tabs++;
			tabId = id + " up  >>>>>";
			listTabId();
		}

		public static  void TabUp(int id = -1)
		{
			TabUp(id.ToString("D3"));
		}

		public static  void TabDn(string id)
		{
			tabs--;
			tabId = id + " dn  <<<<<<<";
			listTabId();
		}

		public static void TabDn(int id = -1)
		{
			TabDn(id.ToString("D3"));
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

			// tests01.splitTest5();
			// tests01.ProcessFormulaSupport.Tests();

			// tests01.ests();
			// tests01.tests2();
			// tests01.test3();

			// return;

			// test that the params have been defined
			// RevitParamTests.Process();

			// get all of the revit chart families and 
			// process each to get its parameters
			result = RevitSystMgr.CollectAllCharts();

			if (!result) return;

			RevitSystMgr.ProcessCharts(CellUpdateTypeCode.ALL);

			listInfo.listAllChartsInfo(RevitSystMgr.Charts);

			// proess labels

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