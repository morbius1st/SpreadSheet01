#region + Using Directives

using System.Collections.Generic;

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

	public class RevitContainer : ARevitParam
	{
		public Dictionary<string, RevitCellParams>  CellParams { get; private set; }  =
			new Dictionary<string, RevitCellParams>();

		public override dynamic GetValue()
		{
			return null;
		}

		private void test()
		{
			RevitContainer rg0 = new RevitContainer();

			RevitCellParams cp1A = new RevitCellParams();
			

			rg0.CellParams.Add(cp1A.Key, cp1A); // parameters for (1) anno symbol


			RevitCellParams cp1B = new RevitCellParams();

			ParamDesc pd = RevitCellParameters.Match("Label");

			RevitParamText rt = new RevitParamText("", pd);




			rg0.CellParams.Add(cp1B.Key, cp1B); // parameters for (1) anno symbol




		}
	}
}
