#region + Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

// user name: jeffs
// created:   5/17/2021 2:57:08 PM






namespace CellsTest.CellsTests
{
	public class SampleEquations
	{
		public Dictionary<string, Tuple<string, bool, string, Tuple<bool, string, string>[]>>
			EqSamples { get; private set; } = new Dictionary<string, Tuple<string, bool, string, Tuple<bool, string, string>[]>>();

		public SampleEquations() { }

		// "=(1 + (21 + 22 + (31 + 32 * (41 + 42) * (sign(51+52) ) ) ) + 2*3) + 4+5"
		private void MakeSamples()
		{

			EqSamples.Add("sample1", MakeSample("string addition", true,
				"{[A1]} = \"alp\" & \"ha\"",
				new [,]
					{
						{"{[A1]}", "=", "\"alp\"", "&", "\"ha\""},
						{"v2", "eq", "s1", "op1", "s1"}
					},
				new [] {true, true, true, true} ));

			EqSamples.Add("sample2", MakeSample("integer math & parentheses", true,
				"=(1 + (21 + 22 + (31 + 32 * (41 + 42) * (sign(51+52) ) ) ) + 2*3) + 4+5",
				new[,]
					{
						{ "=",  "(",   "1",  "+",   "(",   "21", "+",   "22", "+",   "(",   "31", "+",   "32", "*",   "(",   "41", "+",   "42", ")",   "*",   "(",   "sign",  "(", "51", "+",   "52", ")",   ")",   ")",   ")",   "+",   "2",  "*",   "3",  ")",   "+",   "4",  "+",   "5"},
						{ "eq", "pup", "n1", "op1", "pdn", "n1", "op1", "n1", "op1", "pup", "n1", "op1", "n1", "op1", "pup", "n1", "op1", "n1", "pdn", "op1", "pup", "f1", "pup", "n1", "op1", "n1", "pdn", "pdn", "pdn", "pdn", "op1", "n1", "op1", "n1", "pdn", "op1", "n1", "op1", "n1"}
					},
				new [] {true, true, true, true} ));
		}

		private Tuple<string, bool, string, Tuple<bool, string, string>[]> MakeSample(string description, bool shouldPass,
			string testSample, string[,] tokens, bool[] tokensGood)
		{
			if (tokensGood.Length != tokens.Length) return null;

			Tuple<string, bool, string, Tuple<bool, string, string>[]> sample
				= new Tuple<string, bool, string, Tuple<bool, string, string>[]>(description, shouldPass, testSample,
					new Tuple<bool, string, string>[tokens.Length]);

			for (int i = 0; i < tokens.Length; i++)
			{
				Tuple<bool, string, string> result =
					new Tuple<bool, string, string>(tokensGood[i], tokens[0,i], tokens[1,i]);

				sample.Item4[i] = result;
			}

			return sample;
		}
	}
}


