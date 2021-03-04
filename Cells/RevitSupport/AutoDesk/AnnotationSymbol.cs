

#region + Using Directives
#endregion

// user name: jeffs
// created:   2/23/2021 6:39:44 PM

using System.Collections.Generic;
using SpreadSheet01.RevitSupport.RevitParamValue;

namespace Autodesk.Revit.DB
{
	public class Element
	{

	}


	public class AnnotationSymbol
	{
		private static int id = 100000;
		private int elementId = -1;

		public IList<Parameter> parameters;

		public IList<Parameter> GetOrderedParameters()
		{
			return parameters;
		}

		public string Name { get; set; }

		public int Id
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

	public class Parameter
	{
		private string asString;
		private double asDouble;
		private int asInteger;

		public Definition Definition { get; set; }

		public string AsString () =>  asString;
		public double AsDouble () =>  asDouble;
		public int AsInteger () =>  asInteger;

		public Parameter(   string name, ParamDataType type, 
			string strVal, double dblVal, int intVal)
		{
			Definition = new Definition() {Name = name, Type = type};
			asString = strVal;
			asDouble = dblVal;
			asInteger = intVal;
		}
	}

	public class Definition
	{
		public string Name { get; set; }
		public ParamDataType Type { get; set; }
	}


}
