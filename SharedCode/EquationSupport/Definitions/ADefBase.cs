// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             ADefBase.cs
// Created:      2021-05-30 (7:44 AM)

using System;

namespace SharedCode.EquationSupport.Definitions
{
	public abstract class ADefBase : IEquatable<string>
	{
		private static int id = 1;

		public string Description { get; private set; }  // general description of the token
		public string ValueStr { get; private set; }     // the actual token value - i.e. "v1" or "+"
		public ValueType ValueType { get; private set; } // the type of value held
		public int Id { get; private set; }              // a numeric id // sequential number

		public ADefBase() { }

		public ADefBase(string description, string valueStr,
			ValueType valType)
		{
			Description = description;
			ValueStr = valueStr;
			ValueType = valType;
			Id = id++;
		}

		public abstract bool Equals(string test);

		public static ADefBase Invalid => (ADefBase) ValueDefinitions.ValDefInst[ValueDefinitions.Vd_Invalid];
	}
}