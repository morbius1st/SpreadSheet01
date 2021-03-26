using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Autodesk.Revit.ApplicationServices;
using Application = Autodesk.Revit.ApplicationServices.Application;
using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitParamManagement;

using SharedRevitCode.ShParamUtils;

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

		private ParamUtils util;

		private string tempFile;

		public SelectCharts(Application app, Document doc)
		{
			InitializeComponent();

			this.app = app;
			this.doc = doc;

			util = new ParamUtils(app, doc);

			// rvtMgr = new RevitManager();
		}

		// public RevitManager RevitMgr => rvtMgr;

		// public List<RevitChartItem> Charts => rvtMgr.ChartList.Charts;


		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
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