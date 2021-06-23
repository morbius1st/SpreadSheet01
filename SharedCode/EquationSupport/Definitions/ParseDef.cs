// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             ParseGen.cs
// Created:      2021-05-30 (7:44 AM)

using System.Collections.Generic;

namespace SharedCode.EquationSupport.Definitions
{
	public class ParseDef : ADefBase
	{
		// public ParseGen() { }

		public ParseDef(string description, string valueStr, ValueType valType, AValDefBase[] aDefs, bool isGood = true)
			: base(description, valueStr, valType)
		{

			if (aDefs == null || aDefs.Length == 0)
			{
				IsGood = false;
			}
			else
			{
				IsGood = isGood;

				this.ValDefs = new List<AValDefBase>();

				foreach (AValDefBase vd in aDefs)
				{
					if (vd == null) continue;
					this.ValDefs.Add(vd);
				}
			}
		}

		public List<AValDefBase> ValDefs { get; private set; } = null;

		public AValDefBase Classify(string test)
		{
			foreach (AValDefBase ab in ValDefs)
			{
				if (ab.Equals(test)) return ab;
			}

			return (AValDefBase) ADefBase.Invalid;
		}

		public bool IsGood { get; private set; }             // indicates token is not valid

		public override bool Equals(string test)
		{
			return (ValueStr?.Equals(string.Empty) ?? false) || (ValueStr?.Equals(test) ?? false);
		}
	}
}