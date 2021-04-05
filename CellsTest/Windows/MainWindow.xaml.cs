#region using

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using CellsTest.CellsTests;
using SharedCode.DebugAssist;
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
	public partial class MainWindow : Window, INotifyPropertyChanged, ISendMessages
	{
	#region private fields

		// public RevitParamTest RevitParamTests { get; } = new RevitParamTest();

		// private ListiInfo listInfo = new ListiInfo();

		private SharedCode.DebugAssist.ListInfo<MainWindow> listInfo;

		// private static MainWindow me;

		private Tests01 tests01;

	#endregion

	#region ctor

		public MainWindow()
		{
			// me = this;

			InitializeComponent();

			listInfo = new ListInfo<MainWindow>(this);

			tests01 = new Tests01(this);
		}

	#endregion

	#region public properties

		public RevitSystemManager RevitSystMgr { get; } = new RevitSystemManager();

	#endregion

	#region private properties

	#endregion

	#region public methods

		private int tabs = 0;
		private string tabId = null;

		public bool showTabId { get; set; }  = true;

		public void listTabId()
		{
			if (tabId != null && showTabId)
			{
				WriteLine(tabId);
			}
		}

		public  void TabUp(string id)
		{
			tabs++;
			tabId = id + " up  >>>>>";
			listTabId();
		}

		public  void TabUp(int id = -1)
		{
			TabUp(id.ToString("D3"));
		}

		public  void TabDn(string id)
		{
			tabs--;
			tabId = id + " dn  <<<<<<<";
			listTabId();
		}

		public void TabDn(int id = -1)
		{
			TabDn(id.ToString("D3"));
		}

		public void TabClr(string id)
		{
			tabs = 0;
			tabId = id + " clr";
			listTabId();
		}

		public void TabSet(int t)
		{
			tabs = t;
		}

		public void WriteLineTab(string msg)
		{
			WriteTab(msg + "\n");
		}

		public void WriteTab(string msg)
		{
			TbxMsg.Text +=
				// (tabId > 0 ? tabId.ToString("D3") : "") + 
				(tabs > 0 ? "    ".Repeat(tabs) : "" ) + msg;
		}

		public void WriteLine(string msg)
		{
			Write(msg + "\n");
		}

		public void Write(string msg)
		{
			TbxMsg.Text += msg;
		}

	#endregion

	#region private methods

		private void getParamsTest1()
		{
			WriteLine("Get all charts| start");
			bool result;

			// get all of the revit chart families and 
			// process each to get its parameters
			result = RevitSystMgr.CollectAllCharts();
			
			if (!result) return;
			
			listInfo.listCharts(RevitSystMgr.Charts);

			WriteLine("Get all charts| end");
		}

		private void getParamsTest2()
		{
			ChartFamily chart ;

			bool result = RevitParamManager.GetChartFamily(RevitParamManager.CHART_FAMILY_NAME, out chart);

			if (!result) return;

			listInfo.listAllChartFamilies();
		}
		
		private void getParamsTest3()
		{
			WriteLine("Get and list all charts| start");
			bool result;

			// get all of the revit chart families and 
			// process each to get its parameters
			result = RevitSystMgr.CollectAllCharts();

			if (!result)
			{
				WriteLine("collect charts failed");
				return;
			}

			result = RevitSystMgr.ProcessCharts(CellUpdateTypeCode.STANDARD);

			if (!result)
			{
				WriteLine("process charts failed");
				return;
			}

			listInfo.listAllChartsInfo(RevitSystMgr.Charts);
		}

	#endregion

	#region event consuming



		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("@Debug");
		}

		private void BtnListCharts_OnClick(object sender, RoutedEventArgs e)
		{
			getParamsTest1();

			Debug.WriteLine("@Debug");
		}

		private void BtnListChartsFamilies_OnClick(object sender, RoutedEventArgs e)
		{
			getParamsTest2();

			Debug.WriteLine("@List Params");
		}

		private void BtnListChartsAll_OnClick(object sender, RoutedEventArgs e)
		{
			getParamsTest3();

			Debug.WriteLine("@List Params");
		}

		private void BtnListSamples_OnClick(object sender, RoutedEventArgs e)
		{
			SampleAnnoSymbols sample = new SampleAnnoSymbols();
			sample.Process(RevitParamManager.CHART_FAMILY_NAME);
			
			listInfo.listSample(sample.ChartSymbols, sample.Symbols);

			Debug.WriteLine("@Debug");
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
			// result = RevitSystMgr.CollectAllCharts();
			//
			// if (!result) return;
			//
			// listInfo.listCharts(RevitSystMgr.Charts);

			return;


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