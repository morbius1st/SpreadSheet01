using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Application = Autodesk.Revit.ApplicationServices.Application;
using Autodesk.Revit.DB;
using SharedRevitCode.ShParamUtils;
using SharedRevitCode.ShRevitManagement;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SharedCode.DebugAssist;
using SpreadSheet01.RevitSupport.RevitParamManagement;
using UtilityLibrary;


namespace SpreadSheet01.Windows
{

	/// <summary>
	/// Interaction logic for SelectCharts.xaml
	/// </summary>
	public partial class SelectCharts : Window, INotifyPropertyChanged, ISendMessages
	{
		
		private Application app;
		private Document doc;

		private RevitProcessManager rvtProcMgr;
		public RevitSystemManager revitSystMgr;

		private ListInfo<SelectCharts> listInfo;

		private ParamUtils util;

		private string tempFile;

		private string message;

		public SelectCharts(Application app, Document doc)
		{
			InitializeComponent();

			this.app = app;
			this.doc = doc;

			rvtProcMgr = new RevitProcessManager();
			revitSystMgr = new RevitSystemManager(app, doc);

			util = new ParamUtils(app, doc);

			listInfo = new ListInfo<SelectCharts>(this);
		}

	#region public properties

		public string Message
		{
			get => message;
			set
			{
				message = value;

				OnPropertyChanged();
			}
		}

	#endregion

	#region public methods

		
		private int tabs = 0;
		private string tabId = null;

		public bool showTabId { get; set; } = true;

		public void listTabId()
		{
			if (tabId != null && showTabId)
			{
				WriteLine(tabId);
			}
		}

		public void TabUp(string id = "")
		{
			tabs++;
			tabId = id + " up  >>>>>";
			listTabId();
		}

		public void TabDn(string id = "")
		{
			tabs--;
			tabId = id + " dn  <<<<<<<";
			listTabId();
		}

		public void TabClr(string id = "")
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

			Write((tabs > 0 ? "    ".Repeat(tabs) : "" ) + msg);
		}

		public void WriteLine(string msg)
		{
			Write(msg + "\n");
		}

		public void Write(string msg)
		{
			Message += msg;
		}
	#endregion

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void BtnGetCharts_OnClick(object sender, RoutedEventArgs e)
		{

			bool result = revitSystMgr.CollectAllCharts();

			if (!result) return;

			RevitCharts c = revitSystMgr.Charts;

			// list.listCharts(revitSystMgr.Charts);

			revitSystMgr.ProcessCharts(CellUpdateTypeCode.ALL);

			listInfo.listAllChartsInfo(revitSystMgr.Charts);

			Debug.WriteLine("@ BtnGetCharts_OnClick");
		}

		private void BtnReOrder_OnClick(object sender, RoutedEventArgs e)
		{
			bool result = util.ReOrderParameters();


			Debug.WriteLine("@Debug");
		}

		private void BtnMakeSharedParams_OnClick(object sender, RoutedEventArgs e)
		{
			// bool result;
			// bool resultOut;
			//
			// if (util.LoadSharedParametersFromFile(out resultOut))
			// {
			// 	result = !util.AddSharedParameter();
			// }

			bool result = util.CreateSharedParametersFromTempFile(out tempFile);

			Debug.WriteLine("@shared params");
		}
		
		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			
			Debug.WriteLine("@Debug");
		}

		private void SelectCharts_OnLoaded(object sender, RoutedEventArgs e)
		{
			// rvtMgr.GetCharts();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public override string ToString()
		{
			return "I am SelectCharts";
		}
		
	}
}