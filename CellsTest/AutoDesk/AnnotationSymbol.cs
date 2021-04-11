#region + Using Directives
#endregion

// user name: jeffs
// created:   2/23/2021 6:39:44 PM
using System.Collections.Generic;
using SpreadSheet01.RevitSupport.RevitParamManagement;


namespace Autodesk.Revit.DB
{
	public static class RevitDoc
	{
		public static Document Doc { get; set; }
	}

	public class Document {}

	public class ParameterSet
	{
		public List<Parameter> Parameters { get; set; } = new List<Parameter>();

		public int Size => Parameters.Count;

		public void Add(Parameter p)
		{
			Parameters.Add(p);
		}
	}


	public class FamilySymbol : Element
	{
		public FamilySymbol(string familyName)
		{
			// base.Parameters = new List<Parameter>();
			base.Parameters = new ParameterSet();

			FamilyName = familyName;
		}

		public string FamilyName { get; private set; }
	}

	public class AnnotationSymbol : Element
	{
		private FamilySymbol famSym;

		public AnnotationSymbol(string typeName, string familyName)
		{
			famSym = new FamilySymbol(familyName);
			Name = typeName;
		}

		public FamilySymbol Symbol => famSym;
	}


	public class Element
	{
		private static int id = 100000;
		private int elementId = -1;

		protected Element() {}

		// public Element(string typeName)
		// {
		// 	Name = typeName;
		// }

		public ParameterSet Parameters { get; set; }

		public IList<Parameter> parameters;

		public IList<Parameter> GetOrderedParameters()
		{
			return Parameters.Parameters;
		}

		public string Name { get; protected set; }

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

		private bool userModifiable;

		public Definition Definition { get; set; }

		public string AsString () =>  asString;
		public double AsDouble () =>  asDouble;
		public int AsInteger () =>  asInteger;

		public bool UserModifiable => userModifiable;

		public Parameter(  string name, ParamDataType type,
			string strVal, double dblVal, int intVal, bool userModifiable = true)
		{
			Definition = new Definition() {Name = name, Type = type};
			this.userModifiable = userModifiable;
			asString = strVal;
			asDouble = dblVal;
			asInteger = intVal;
		}

		public override string ToString()
		{
			return Definition.Name;
		}
	}

	public class Definition
	{
		public string Name { get; set; }
		public ParamDataType Type { get; set; }
	}
}