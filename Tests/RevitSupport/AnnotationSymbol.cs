#region + Using Directives
#endregion

// user name: jeffs
// created:   2/23/2021 6:39:44 PM

namespace Autodesk.Revit.DB
{
	public class AnnotationSymbol
	{
		private static int id = 100000;
		private int elementId = -1;

		public int ElementId
		{
			get
			{
				if (elementId == -1)
				{
					elementId = id++;
				}

				return elementId;
			}
		}
	}


}
