#region using

// using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.ParseSupport;
using SharedCode.EquationSupport.TokenSupport.Amounts;
using static SharedCode.EquationSupport.Definitions.ValueType;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;
using static SharedCode.EquationSupport.Definitions.ParseDefinitions;

#endregion

// username: jeffs
// created:  5/22/2021 5:29:23 PM

namespace SharedCode.EquationSupport.TokenSupport
{

	public class Tokens : IEnumerable<Token>
	{
	#region private fields

		private List<List<Token>> tokenList;
		private List<List<ParseData>> parseList;
		
		private ValueDefinitions vds = ValueDefinitions.ValDefInst;
		private string vd_grpRefVal ;
		private string vd_grpBegVal ;
		private string vd_grpEndVal ;


	#endregion

	#region ctor

		public Tokens(string name)
		{
			Name = name;

			vd_grpRefVal  = vds.Definitions[Vd_GrpRef].ValueStr; // <gpr>
			vd_grpBegVal  = vds.Definitions[Vd_GrpBeg].ValueStr; // <gpb>
			vd_grpEndVal  = vds.Definitions[Vd_GrpEnd].ValueStr; // <gpe>
		}

	#endregion

	#region public properties

		public string Name { get; private set; }

		public bool Initialized { get; private set; }

		public List<List<Token>> TokenList => tokenList;

		public List<List<ParseData>> ParseList => parseList;

	#endregion

	#region private properties

	#endregion

	#region public methods

		public bool Process(List<List<ParseData>> parseList)
		{
			if (Initialized) return false;

			Debug.WriteLine("@100");

			Initialized = true;

			this.parseList = parseList;

			return AddList(parseList);
		}


	#endregion

	#region private methods

		private bool AddList(List<List<ParseData>> parseList)
		{
			try
			{
				MakeLists(parseList);

				for (int i = 0; i < parseList.Count; i++)
				{
					for (int j = 0; j < parseList[i].Count; j++)
					{
						string s1 = parseList[i][j].Name;
						string s2 = parseList[i][j].Value;
						
						AValDefBase a = parseList[i][j].Definition;

						// Token t = GetToken(parseList[i][j]);

						tokenList[i].Add(GetToken(parseList[i][j]));
					}
				}
			}
			catch
			{
				return false;
			}

			return true;
		}

		private void MakeLists(List<List<ParseData>> parseList)
		{
			tokenList = new List<List<Token>>();

			for (int i = 0; i < parseList.Count; i++)
			{
				tokenList.Add(new List<Token>());
			}
		}

		private Token GetToken(ParseData pd)
		{
			return pd.Definition.MakeToken(pd.Value, pd.Position, pd.Length, pd.Level);
		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<Token> GetEnumerator()
		{
			if (tokenList == null || tokenList[0].Count == 0) yield break;

			int level = 0;
			int priorLevel = 0;
			int idx = 0;
			int priorIdx = 0;

			bool done = false;

			do
			{
				Token t = tokenList[level][idx];

				// if (t.AmountBase.AsString().Equals(vd_grpRefVal))
				if (t.ValDef.ValueType== VT_GP_REF)
				{
					priorLevel = level;
					priorIdx = idx;

					level = t.RefIdx;
					idx = 0;
					continue;
				}

				yield return t;


				// if (t.AmountBase.AsString().Equals(vd_grpEndVal))
				if (t.ValDef.ValueType == VT_GP_END)
				{
					level = priorLevel;
					idx = priorIdx;
				}

				idx++;

				if (idx >= tokenList[level].Count) done = true;

			}
			while (!done);
		}

		public override string ToString()
		{
			return $"this is| {nameof(Tokens)} ({Name})";
		}

	#endregion
	}
}