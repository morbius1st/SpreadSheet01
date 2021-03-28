// Solution:     SpreadSheet01
// // projname: CellsTest// File:             ParseSupport.cs
// Created:      2021-03-20 (2:52 PM)

namespace SharedCode.FormulaSupport.ParseSupport
{
	internal class ParseUtil
	{

		internal static charValidation IdFirstChar { get; private set; }
		internal static charValidation IdRemainder { get; private set; }
		internal static charValidation ParamNameFirstChar { get; private set; }
		internal static charValidation ParamNameRemainder { get; private set; }

		// static char[] invalidRevitNameChars = new [] {'\\',':','{','}', '[', ']', '|', ';', '<', '>', '?', '`', '~' }; //@"\:{}[]|;<>?`~";
		static string invalidRevitNameTypicalChars = @"\:{}[]|;<>?`~";
		static string invalidRevitNameFirstChars = invalidRevitNameTypicalChars+"0123456789";
		
		static char[] charRangeLettersLc = new char[] {'a', 'z'};
		static char[] charRangeLettersUc = new char[] {'A', 'Z'};
		static char[] charRangeDigits    = new char[] {'0', '9'};
		static char[] charRangeAll       = new char[] {' ', '~'};
		
		static char[][] validcharsLetters = new char[][]
		{
			charRangeLettersLc, charRangeLettersUc
		};
		
		static char[][] validcharsWord = new char[][]
		{
			charRangeLettersLc, charRangeLettersUc, charRangeDigits
		};
		
		static char[][] validcharsAll = new char[][]
		{
			charRangeAll
		};

		public ParseUtil()
		{
			IdFirstChar = new charValidation(null, validcharsLetters);
			IdRemainder= new charValidation(null, validcharsWord);

			ParamNameFirstChar = new charValidation(null, validcharsAll, invalidRevitNameFirstChars, true);
			ParamNameRemainder = new charValidation(null, validcharsAll, invalidRevitNameTypicalChars, true);
		}
	}
}