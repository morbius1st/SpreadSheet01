using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Application = Autodesk.Revit.ApplicationServices.Application;
using Autodesk.Revit.DB;
using SharedRevitCode.ShParamUtils;
using SharedRevitCode.ShRevitManagement;
using SpreadSheet01.RevitSupport.RevitCellsManagement;


namespace SpreadSheet01.Windows
{
	/// <summary>
	/// Interaction logic for SelectCharts.xaml
	/// </summary>
	public partial class SelectCharts : Window, INotifyPropertyChanged
	{
		// private RevitManager rvtMgr;

		private Application app;
		private Document doc;

		private RevitProcessManager rvtProcMgr;
		public RevitSystemManager revitSystMgr;

		private ParamUtils util;

		private string tempFile;

		public SelectCharts(Application app, Document doc)
		{
			InitializeComponent();

			this.app = app;
			this.doc = doc;

			rvtProcMgr = new RevitProcessManager();
			revitSystMgr = new RevitSystemManager(app, doc);

			util = new ParamUtils(app, doc);

			// rvtMgr = new RevitManager();
		}

		// public RevitManager RevitMgr => rvtMgr;

		// public List<RevitChartItem> Charts => rvtMgr.ChartList.Charts;

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}

		private void BtnGetCharts_OnClick(object sender, RoutedEventArgs e)
		{

			bool result = revitSystMgr.GetCurrentCharts();

			RevitCharts c = revitSystMgr.Charts;

			Debug.WriteLine("@ BtnGetCharts_OnClick");
		}

		private void BtnReOrder_OnClick(object sender, RoutedEventArgs e)
		{
			bool result = util.ReOrderParameters();


			Debug.WriteLine("@Debug");
		}

		private void BtnDebug_OnClick(object sender, RoutedEventArgs e)
		{
			

			// bool result;
			// bool resultOut;
			//
			// if (util.LoadSharedParametersFromFile(out resultOut))
			// {
			// 	result = !util.AddSharedParameter();
			// }

			bool result = util.CreateSharedParametersFromTempFile(out tempFile);

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