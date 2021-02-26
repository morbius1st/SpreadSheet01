#region + Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   2/25/2021 6:52:29 PM

namespace SpreadSheet01.RevitSupport
{
	/*
	 the primary collection of the parameters for all of the annotation symbols of a type
	AnnoSymbols
	+--> dictionary<string, AnnoSymbol> {AnnoSymbols}
		+-> AnnoSymbol
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

	public class RevitAnnoSymbols
	{



	}
}
