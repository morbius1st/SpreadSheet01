#region using directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

#endregion

// username: jeffs
// created:  5/17/2021 2:47:36 PM

/*
The plan with these tests is to 
1. parse an equation into the basic parts
2. validate the equation
3. adjust the parts to create a fully urinary or binary configuration
4. categorize & tokenize the parts
*/

namespace CellsTest.CellsTests
{
	public class Test02A : INotifyPropertyChanged
	{
		#region private fields


		#endregion

		#region ctor

		public Test02A() { }

		#endregion

		#region public properties



		#endregion

		#region private properties



		#endregion

		#region public methods



		#endregion

		#region private methods



		#endregion

		#region event consuming



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
			return "this is Test02A";
		}

		#endregion

	}
}
