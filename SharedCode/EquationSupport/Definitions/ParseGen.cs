// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             ParseGen.cs
// Created:      2021-05-30 (7:44 AM)

using System.Collections.Generic;

namespace SharedCode.EquationSupport.Definitions
{
	public class ParseGen : ADefBase
	{
		public List<ADefBase2> aDefBase2 = null;
		public ParseGroupGeneral Group { get; private set; } // functional grouping
		public bool IsGood { get; private set; }             // indicates token is not valid

		public ParseGen() { }

		public ParseGen(   string description, string valueStr, ValueType valType,
			ParseGroupGeneral @group, ADefBase2[] aDefs, bool isGood = true)
			: base(description, valueStr, valType)
		{
			Group = group;

			if (aDefs == null || aDefs.Length == 0)
			{
				IsGood = false;
			}
			else
			{
				IsGood = isGood;

				this.aDefBase2 = new List<ADefBase2>();

				foreach (ADefBase2 vd in aDefs)
				{
					if (vd == null) continue;
					this.aDefBase2.Add(vd);
				}
			}
		}

		public ADefBase2 Classify(string test)
		{
			foreach (ADefBase2 ab in aDefBase2)
			{
				if (ab.Equals(test)) return ab;
			}

			return (ADefBase2) ADefBase.Invalid;
		}

		public override bool Equals(string test)
		{
			return (ValueStr?.Equals(string.Empty) ?? false) || (ValueStr?.Equals(test) ?? false);
		}
	}
}