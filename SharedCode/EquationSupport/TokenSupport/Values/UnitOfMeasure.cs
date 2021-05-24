// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             UnitOfMeasure.cs
// Created:      2021-05-22 (6:42 AM)

namespace SharedCode.EquationSupport.TokenSupport.Values
{
	public class UoM
	{
		public double Amount { get; private set; }
		public string UnitType { get; private set; }

		public UoM(double amt, string unitType)
		{
			Amount = amt;
			UnitType = unitType;
		}
	}
}