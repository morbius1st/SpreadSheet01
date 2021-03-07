using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SpreadSheet01.RevitSupport;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitChartInfo;

namespace SpreadSheet01.Windows
{
	/// <summary>
	/// Interaction logic for SelectCharts.xaml
	/// </summary>
	public partial class SelectCharts : Window, INotifyPropertyChanged
	{
		private RevitManager rvtMgr;

		public SelectCharts()
		{
			InitializeComponent();

			rvtMgr = new RevitManager();
		}

		// public RevitManager RevitMgr => rvtMgr;

		// public List<RevitChartItem> Charts => rvtMgr.ChartList.Charts;


		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
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