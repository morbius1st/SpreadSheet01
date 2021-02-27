#region + Using Directives

using System.Collections.Generic;
using System.Text.RegularExpressions;

#endregion

// user name: jeffs
// created:   2/25/2021 6:52:29 PM

namespace SpreadSheet01.RevitSupport.RevitParamValue
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



	public class RevitContainerTest
	{
		private RevitAnnoSyms annoSyms = new RevitAnnoSyms();

		public void test2()
		{
			RevitAnnoSym sym = new RevitAnnoSym();

			annoSyms.ContainerList.Add(sym);
		}


		private RevitAnnoSym makeSym()
		{
			RevitAnnoSym sym = null;

			sym.ContentList.Add("asf", );


			return sym;
		}

		private void addParams(RevitAnnoSym sym)
		{

			ParamDesc pd = RevitCellParameters.Match("Name");


			RevitParamBool rb = new RevitParamBool(true)
		}




	}







	public class RevitAnnoSyms : RevitContainers<RevitAnnoSym>
	{

	}

	public class RevitAnnoSym : RevitContainer<ARevitParam>
	{

	}

	public class RevitLabels : RevitContainers<RevitLabel>
	{

	}

	public class RevitLabel : RevitContainer<ARevitParam>
	{

	}

	public class RevitContainers<T> : ARevitParam
	{
		public List<T>  ContainerList { get; private set; }  
			= new List<T>();

		public override dynamic GetValue()
		{
			return null;
		}
	}

	public class RevitContainer<T>
	{
		public Dictionary<string, T>  ContentList { get; private set; }  =
			new Dictionary<string, T>();
	}



/*

	public class RevitAnnoSyms : RevitContainers<RevitAnnoSym>
	{
		private void test()
		{

			RevitAnnoSym annoSym = new RevitAnnoSym();

			this.ContainerList.Add(annoSym);
		}
	}

	public class RevitAnnoSym : RevitContainer<ARevitParam>
	{

		private void test()
		{
			ParamDesc pd = RevitCellParameters.Match("name");

			RevitParamBool rb1 = new RevitParamBool(true, pd);
			RevitParamBool rb2 = new RevitParamBool(true, pd);
			RevitParamBool rb3 = new RevitParamBool(true, pd);
			RevitParamBool rb4 = new RevitParamBool(true, pd);

			this.ContentList.Add("asf", rb1);

			RevitLabels labels = new RevitLabels();

			this.ContentList.Add("asdf", labels);

		}
	}

	public class RevitLabels : RevitContainers<RevitLabel>
	{
		private void test()
		{
			RevitLabel label = new RevitLabel();

			this.ContainerList.Add(label);
		}
	}
	
	public class RevitLabel : RevitContainer<ARevitParam>
	{
		private void test()
		{
			ParamDesc pd = RevitCellParameters.Match("name");

			RevitParamBool rb1 = new RevitParamBool(true, pd);
			RevitParamBool rb2 = new RevitParamBool(true, pd);
			RevitParamBool rb3 = new RevitParamBool(true, pd);
			RevitParamBool rb4 = new RevitParamBool(true, pd);
		}
	}

	*/

}
