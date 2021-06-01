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

		public string ValueStr { get; }      // the actual token value - i.e. "v1" or "+"
		public string Description { get; }   // general description of the token
		public ValueType ValueType { get; }  // the type of value held
		public int Id { get; }               // a numeric id // sequential number


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