#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharedCode.EquationSupport.Definitions.ParseGroupGeneral;
using static SharedCode.EquationSupport.Definitions.ValueType;
#endregion

// user name: jeffs
// created:   5/17/2021 6:40:33 PM

namespace SharedCode.EquationSupport.Definitions
{
	public class ParseGeneralDefinitions : DefinitionBase<ParseGen>
	{
		private const int MAX_TOKENS = 20;

		static ParseGeneralDefinitions() { Init();}

		public override ParseGen Invalid => new ParseGen("Invalid", null, VT_INVALID,    PGG_INVALID, (int) PGG_INVALID, false);
		public override ParseGen Default => new ParseGen("Default", null, VT_DEFAULT, PGG_DEFAULT, (int) PGG_DEFAULT, false);

		// protected override void Initialize() { }

		private static void Init()
		{
			idDefArray = new ParseGen[MAX_TOKENS];

			int idx = 0;
			int id = 1;

			idDefArray[idx++] = new ParseGen("Invalid"                , "x1"  , VT_STRING, PGG_INVALID  , id++, false);
			idDefArray[idx++] = new ParseGen("Word"                   , "w1"  , VT_STRING, PGG_INVALID  , id++, false);
			idDefArray[idx++] = new ParseGen("Assignment"             , "="   , VT_STRING, PGG_ASSIGNMENT, id++);
			idDefArray[idx++] = new ParseGen("Operator"               , "op1" , VT_STRING, PGG_OPERATOR , id++);
			idDefArray[idx++] = new ParseGen("String"                 , "s1"  , VT_STRING, PGG_STRING   , id++);
			idDefArray[idx++] = new ParseGen("Boolean"                , "?"   , VT_STRING, PGG_BOOLEAN  , id++);
			idDefArray[idx++] = new ParseGen("Number Integer"         , "n1"  , VT_STRING, PGG_NUMBER   , id++);
			idDefArray[idx++] = new ParseGen("Number Real"            , "d1"  , VT_STRING, PGG_NUMBER   , id++);
			idDefArray[idx++] = new ParseGen("Number Fraction"        , "fr1" , VT_STRING, PGG_NUMBER   , id++);
			idDefArray[idx++] = new ParseGen("Number Length"          , "l1"  , VT_STRING, PGG_UNIT     , id++);
			idDefArray[idx++] = new ParseGen("Function"               , "fn1" , VT_STRING, PGG_FUNCTION , id++);
			idDefArray[idx++] = new ParseGen("Variable Address"       , "v1"  , VT_STRING, PGG_VARIABLE , id++);
			idDefArray[idx++] = new ParseGen("Variable Special"       , "v2"  , VT_STRING, PGG_VARIABLE , id++);
			idDefArray[idx++] = new ParseGen("Group Reference"        , "ref" , VT_STRING, PGG_GROUP_REF, id++);
			idDefArray[idx++] = new ParseGen("Group Parenthesis Begin", "pup" , VT_STRING, PGG_GROUPING , id++);
			idDefArray[idx++] = new ParseGen("Group Parenthesis End"  , "pdn" , VT_STRING, PGG_GROUPING , id++);

			count = idx;
		}
	}
}