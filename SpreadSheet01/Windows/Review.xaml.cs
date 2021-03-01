using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SpreadSheet01.Windows
{
	/// <summary>
	/// Interaction logic for Review.xaml
	/// </summary>
	public partial class Review : Window
	{
		public Review()
		{
			InitializeComponent();
		}

		private void BtnDone_OnClick(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
