#region + Using Directives

using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
// using Autodesk.Revit.DB;
using SpreadSheet01.RevitSupport.RevitParamValue;
using static SpreadSheet01.RevitSupport.RevitCellParameters;

#endregion

// user name: jeffs
// created:   2/25/2021 6:52:29 PM
// spreadsheet01

namespace SpreadSheet01.RevitSupport
{
	/*
	 the primary collection of the parameters for all of the annotation symbols of a type
	AnnoSymbols
	+--> dictionary<string, RevitCellParams> {RevitContainer} :: ARevitParam
		+-> RevitCellParams
			+-> ARevitParam[] {CellItems}
				+-> RevitParamText (string / text) [ename to RevitParamText]
				+-> RevitParamNumber (double / number)
				+-> etc.
				+-> RevitValueCGroup (has a collection)
					+-> ARevitParam[] {AnnoSymParameters}


	objects:
	1.	dictionary<string, AnnoSymbol>	AnnoSymbols
		> is the collection of all anno symbols of associated with a single chart anno symbol
		> that is, a collection of cellItems [rename to AnnoSym]
		> key is sequence as 000000
	2. AnnoSym
		> is the collection of all of the parameters associated with a single anno symbol
		> except that, value parameters go into a single parameter that has a collection of these parameters
		> collection is: ARevitParam[] called cellItems [rename to AnnoSymParameters]
		> key is index as 00


	notes:
	1. adjust chart anno symbol to use multiple anno symbols
	*/

	public class DynVal
	{
		private dynamic dynamicValue; // base value, as provided // as a number from excel
		private string formatString;  // 
		private string preFormatted;  // as provided directly from excel

		public dynamic DynamicValue => dynamicValue;

		public string AsPreFormatted() => preFormatted;

		public string AsFormatted() => dynamicValue.ToString(formatString);

		public string AsString() => dynamicValue.ToString();

		public double AsDouble() => dynamicValue == typeof(double) ? dynamicValue : Double.NaN;

		public double AsInteger() => dynamicValue == typeof(int) ? dynamicValue : Int32.MaxValue;

		public double AsBool() => dynamicValue == typeof(bool) ? dynamicValue : false;

		public Type BaseType() => dynamicValue.GetType();

	}



	
	public class RevitContainerTest
	{
		private RevitAnnoSyms annoSyms = new RevitAnnoSyms();

		public void test2()
		{
			RevitAnnoSym sym = new RevitAnnoSym();

			string key = RevitValueSupport.MakeAnnoSymKey(sym, false);

			annoSyms.Add(key, sym);
		}


		private RevitAnnoSym makeSym()
		{
			RevitAnnoSym sym = null;

			// sym.ContentList.Add("asf", null);


			return sym;
		}

		private void addParams(RevitAnnoSym sym)
		{

			ParamDesc pd = RevitCellParameters.Match("Name");


			RevitParamBool rb = new RevitParamBool(true, pd);
		}
	}

	public class RevitContainers<T> : ARevitParam
	{
		public Dictionary<string, T> Containers { get; private set; }

		// public List<T>  ContainerList { get; private set; }  
		// 	= new List<T>();

		public RevitContainers()
		{
			Containers = new Dictionary<string, T>();
		}

		public override dynamic GetValue() => null;

		public void Add(string key, T container) => Containers.Add(key, container);


		// public void Add(T container) => ContainerList.Add(container);
	}


	public class RevitAnnoSyms : RevitContainers<RevitAnnoSym>
	{

	}

	public class RevitLabels : RevitContainers<RevitLabel>
	{

	}

	public class RevitContainer
	{
		// public Dictionary<string, T>  ContentList { get; private set; }  =
		// 	new Dictionary<string, T>();

		public ARevitParam[] RevitParamList { get; set; }


		public void Add(int idx, ARevitParam content)
		{
			RevitParamList[idx] = content;
		}

		public ARevitParam this[int idx]
		{
			get => RevitParamList[idx];
			set => RevitParamList[idx] = value;
		}

	}

	public class RevitAnnoSym : RevitContainer
	{
		public RevitAnnoSym()
		{
			int a = LabelIdx;

			RevitParamList = new ARevitParam[RevitCellParameters.ParamCounts[(int) ParamGroup.DATA]];

			RevitParamList[LabelsIdx] = new RevitLabels();
		}

		public AnnotationSymbol AnnoSymbol { get; set; }

		public Element RvtElement {get; set; }
	}

	public class RevitLabel : RevitContainer
	{

		public RevitLabel()
		{
			RevitParamList = new ARevitParam[RevitCellParameters.ParamCounts[(int) ParamGroup.LABEL]];
		}

	}

}
