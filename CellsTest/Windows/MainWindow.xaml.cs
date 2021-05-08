#region using

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using CellsTest.CellsTests;
using RevitSupport.RevitChartManagement;
using SharedCode.DebugAssist;
using SharedCode.RevitSupport.RevitManagement;
using SharedCode.RevitSupport.RevitParamManagement;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitParamManagement;
using SpreadSheet01.RevitSupport.RevitParamValue;
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

		private RevitSystemManager systMgr;

		private StringBuilder sb = new StringBuilder();

		private ListInfo<MainWindow> listInfo;

		private Tests01 tests01;
		private Tests01A tests01A;

	#endregion

	#region ctor

		public MainWindow()
		{
			systMgr = new RevitSystemManager();

			InitializeComponent();

			listInfo = new ListInfo<MainWindow>(this);

			tests01 = new Tests01(this);
			tests01A = new Tests01A(this);

			configTrace();
		}

	#endregion


	#region public properties

		public RevitSystemManager RevitSystMgr => systMgr;


		// public RevitChartManager RevitSystMgr { get; } = new RevitChartManager();

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
			// TbxMsg.Text +=
			// 	// (tabId > 0 ? tabId.ToString("D3") : "") + 
				Write((tabs > 0 ? "    ".Repeat(tabs) : "" ) + msg);
		}

		public void WriteLine(string msg)
		{
			Write(msg + "\n");
		}

		public void Write(string msg)
		{

			sb.Append(msg);

			// TbxMsg.Text += msg;
		}

		public void ShowMessage()
		{
			TbxMsg.Text += sb.ToString();
		}

	#endregion

	#region private methods

	
	#endregion

	#region event consuming

		private void BtnSplit1_OnClick(object sender, RoutedEventArgs e)
		{
			WriteLineTab("SplitTest 1");

			tests01.splitTest1();

			ShowMessage();

			Debug.WriteLine("@split test 1");
		}
		
		private void BtnSplit2_OnClick(object sender, RoutedEventArgs e)
		{
			WriteLineTab("SplitTest 2");

			tests01.splitTest2();

			ShowMessage();

			Debug.WriteLine("@split test 2");
		}
		
		private void BtnSplit3_OnClick(object sender, RoutedEventArgs e)
		{
			WriteLineTab("SplitTest 3");

			tests01.splitTest3();

			ShowMessage();

			Debug.WriteLine("@split test 3");
		}
			
		private void BtnSplit5_OnClick(object sender, RoutedEventArgs e)
		{
			WriteLineTab("SplitTest 5");

			tests01.splitTest5();

			ShowMessage();

			Debug.WriteLine("@split test 5");
		}
			
		private void BtnSplit6_OnClick(object sender, RoutedEventArgs e)
		{
			WriteLineTab("SplitTest 6");

			tests01.splitTest6();

			ShowMessage();

			Debug.WriteLine("@split test 6");
		}
					
		private void BtnSplit7_OnClick(object sender, RoutedEventArgs e)
		{
			WriteLineTab("SplitTest 7");

			tests01.splitTest7();

			ShowMessage();

			Debug.WriteLine("@split test 7");
		}
								
		private void BtnSplit8_OnClick(object sender, RoutedEventArgs e)
		{
			WriteLineTab("SplitTest 8");

			tests01.splitTest8();

			ShowMessage();

			Debug.WriteLine("@split test 8");
		}
											
		private void BtnSplit9_OnClick(object sender, RoutedEventArgs e)
		{
			WriteLineTab("SplitTest 9");

			tests01A.splitTest9();

			ShowMessage();

			Debug.WriteLine("@split test 9");
		}
													
		private void BtnSplit10_OnClick(object sender, RoutedEventArgs e)
		{
			WriteLineTab("SplitTest 10");

			tests01A.splitTest10();

			ShowMessage();

			Debug.WriteLine("@split test 10");
		}
			
		private void BtnReset_OnClick(object sender, RoutedEventArgs e)
		{
			sb = new StringBuilder("Reset\n");
			ShowMessage();

			RevitSystMgr.ResetAllCharts();

			Debug.WriteLine("@List Params");
		}
				
		private void BtnLabelsAndFormulas_OnClick(object sender, RoutedEventArgs e)
		{
			listInfo.listLablesAndFormulas();

			Debug.WriteLine("@List listLablesAndFormulas");
		}
						
		private void BtnListErrors_OnClick(object sender, RoutedEventArgs e)
		{
			listInfo.listErrors1();

			Debug.WriteLine("@List errors");
		}
		
		private void BtnGetChartsAll_OnClick(object sender, RoutedEventArgs e)
		{
			listInfo.getParamsTest4a();

			Debug.WriteLine("@List Params");
		}

		private void BtnListGetChartsAll_OnClick(object sender, RoutedEventArgs e)
		{
			listInfo.getParamsTest4b();

			Debug.WriteLine("@List Params");
		}

		private void BtnListCharts_OnClick(object sender, RoutedEventArgs e)
		{
			listInfo.getParamsTest1();

			Debug.WriteLine("@Debug");
		}

		private void BtnListChartsAll_OnClick(object sender, RoutedEventArgs e)
		{
			listInfo.getParamsTest3();

			Debug.WriteLine("@List Params");
		}

		private void BtnListChartsFamilies_OnClick(object sender, RoutedEventArgs e)
		{
			listInfo.getParamsTest2();

			Debug.WriteLine("@List Params");
		}

		private void BtnListSamples_OnClick(object sender, RoutedEventArgs e)
		{
			myTrace.TraceEvent(TraceEventType.Information, 1, "List Sample| Init");

			SampleAnnoSymbols sample = new SampleAnnoSymbols();
			sample.Process(RevitParamManager.CHART_FAMILY_NAME);
			
			// listInfo.listSample(sample.ChartElements, sample.CellElements);
			myTrace.TraceEvent(TraceEventType.Information, 2, "List Sample| Start");
			listInfo.listSample(sample.ChartElements, sample.CellSyms);
			myTrace.TraceEvent(TraceEventType.Information, 3, "List Sample| Start");

			Debug.WriteLine("@Debug");
		}

		private void BtnProcessStandard_OnClick(object sender, RoutedEventArgs e)
		{
			systMgr.CollectAndPreProcessCharts(CellUpdateTypeCode.STANDARD);

			Debug.WriteLine("@process normal");
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			// SampleAnnoSymbols2 s2 = new SampleAnnoSymbols2();
			// s2.Process(RevitParamManager.CHART_FAMILY_NAME);

			/*
			// enable trace
			myTrace.Switch.Level = SourceLevels.All;
			writerListener.Filter = new EventTypeFilter(SourceLevels.All);

			writerListener.WriteLine(DateTime.Now);
			writerListener.WriteLine("This line gets auto flushed");
			*/

			Debug.WriteLine("@Debug");
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			// errorTest();

			WriteLineTab("Loaded...");

			ShowMessage();

			return;
		}

		// private void errorTest()
		// {
		// 	RevitParamText pt = new RevitParamText("test", ParamDesc.Empty);
		// 	
		// 	// ErrorCodeList2 ec2 = new ErrorCodeList2();
		//
		// 	// ec2.Add(pt, ErrorCodes.CEL_HAS_ERROR_CS001107);
		// }

	#endregion

		public static TraceSource myTrace = new TraceSource("CellsTest");
		private TextWriterTraceListener writerListener;


		private void configTrace()
		{
			string fileName = "trace.log";
			string path = AppDomain.CurrentDomain.BaseDirectory;
			string filepath = path + fileName;

			if (File.Exists(filepath))
			{
				File.Delete(filepath);
			}

			myTrace.Switch = new SourceSwitch("SourceSwitch", "Off");
			
			myTrace.Listeners.Remove("Default");

			writerListener = new TextWriterTraceListener("trace.log");

			writerListener.Filter = new EventTypeFilter(SourceLevels.Off);

			myTrace.Listeners.Add(writerListener);

			Trace.AutoFlush = true;
			

		}




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