// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             ADefBase2.cs
// Created:      2021-05-30 (7:44 AM)

using SharedCode.EquationSupport.TokenSupport;

namespace SharedCode.EquationSupport.Definitions
{
	public abstract class ADefBase2 : ADefBase
	{
		public int Seq { get; private set; }   // the sequence number in a group
		public int Order { get; private set; } // order of operation - higher gets done first
		public bool IsNumeric { get; private set; }

		public ADefBase2() { }

		protected ADefBase2(string description, 
			string valueStr, 
			ValueType valType,
			int seq, 
			int order, 
			bool isNumeric
			) : base(description, valueStr, valType)
		{
			Seq = seq;
			Order = order;
			IsNumeric = isNumeric;
		}

		public abstract Token MakeToken(int pos, int len);

	}
}