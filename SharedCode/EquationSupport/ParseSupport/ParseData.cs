// Solution:     SpreadSheet01
// Project:       CellsTest
// File:             ParseData.cs
// Created:      2021-06-12 (4:16 PM)

using SharedCode.EquationSupport.Definitions;
using SharedCode.EquationSupport.TokenSupport;

namespace SharedCode.EquationSupport.ParseSupport
{
	public struct ParseDataInfo
	{
		private int _position;
		private bool _gotRef;

		public int Level { get; private set; }
		public int Length { get; private set; }
		public bool GotRefIdx => _gotRef;

		public ParseDataInfo(int position, int length, int level)
		{
			Level = level;
			Length = length;
			_position = position;
			_gotRef = false;
			
			if (position < 0)
			{
				RefIdx = position;
			}
		}

		public int Position => _position;
		
		public int RefIdx
		{
			get => -_position;
			private set
			{
				_gotRef = true;
				_position = value;
			}
		}
	}

	public class ParseData
	{
		public ParseDataInfo Info { get; private set; }

		public string Name { get; }
		public string Value { get; }

		public int Position => Info.Position;
		public int Length  => Info.Length;
		public int Level  => Info.Level;
		public AValDefBase Definition  { get; private set; }
		public int RefIdx => Info.RefIdx;
		public bool GotRefIdx => Info.GotRefIdx;

		public bool IsValueDef { get; set; }

		public ParseData(string name, string value, 
			int position, int length, int level)
		{
			Name = name;
			Value = value;
			IsValueDef = false;
			Definition = ParseDefinitions.Classify(name, value);

			Info = new ParseDataInfo(position, length, level);
		}
		
		public ParseData(string name, string value, ParseDataInfo info)
		{
			Name = name;
			Value = value;
			IsValueDef = false;
			Info = info;

			Definition = ParseDefinitions.Classify(name, value);
		}

		public override string ToString()
		{
			return $"name| {Name,-8}  val| {Value,-10}  pos| {Position, -5}  level| {Level}";
		}
	}
}