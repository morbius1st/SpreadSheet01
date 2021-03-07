#region using

using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using Cells.CellsTests;
using SpreadSheet01.RevitSupport;
using SpreadSheet01.RevitSupport.RevitCellsManagement;

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

		public RevitChartTests RevitChartTests { get; } = new RevitChartTests();

		public RevitChartManager RevitChartMgr { get; } = new RevitChartManager();

		public RevitManager RevitMgr { get; } = new RevitManager();

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
			WriteLine("Loaded...");

			// RevitParamTests.Process();

			// RevitChartTests.Process();

			// RevitChartMgr.GetCurrentCharts();

			RevitMgr.ProcessAlwaysCharts();

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