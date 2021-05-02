#region using

using System.Collections.Generic;
using SpreadSheet01.RevitSupport.RevitParamValue;

#endregion

// username: jeffs
// created:  4/25/2021 6:16:13 AM

/*
={[A8]}  = excel cell address
={$name} = system variable	
={#name} = revit variable	
={%name} = project parameter
={!name} = global parameter
={@name} = label name
*/

namespace CellsTest.FormulaSupport.FormulaStorage.RevitFormulaDefinition
{
	public partial class RevitFormula
	{
	#region private fields

		private string _formula;

		private string _destinition;

		private DynamicValue _value;

		private Equation _equation;

	#endregion

	#region ctor

		public RevitFormula()
		{
			_equation = new Equation(null);
			

		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods

	#endregion

	#region private methods

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is RevitFormula";
		}

	#endregion

		private class Equation
		{
			private FunctionOperators functOps = new FunctionOperators();

			private DynamicValue _value;

			private OpElement _left;
			private OpElement _right;

			private string _opCodeString;
			private Operation _opCode;

			private bool? _isBinary = null;
			private bool? _status;

			public Equation(string equation)
			{

			}

			public dynamic Value => _value.Value;

			public string AsString => _value.AsString();
			public int AsInteger => _value.AsInteger();
			public double AsDouble => _value.AsDouble();

			public string OpCodeString => _opCodeString;
			public Operation OpCode => _opCode;

			public string OperandLeft => _left.Operand;
			public string OperandRight => _right.Operand;

			public Equation EquationLeft => _left.Equation;
			public Equation EquationRight => _right.Equation;


			public bool? IsBinary => _isBinary;
			public bool Parsed => _status.Value == true;

			private bool parse(string equation)
			{
				_status = null;




				return true;
			}



			private class FunctionOperators
			{
				private Dictionary<string, Operation> _functionOps = new Dictionary<string, Operation>();

				public FunctionOperators()
				{
					_functionOps.Add("+", Operation.NUMERIC_ADDITION);
					_functionOps.Add("&", Operation.STRING_ADDITION);
				}

				public bool Find(string opString, out Operation op)
				{
					return _functionOps.TryGetValue(opString, out op);
				}
			}

			public enum Operation
			{
				NO_OP,
				NUMERIC_ADDITION,
				STRING_ADDITION
			}

			private struct OpElement
			{
				private Equation _equation;
				private string _operand;

				private bool _isEquation;

				public OpElement(Equation eq)
				{
					_equation = eq;
					_operand = null;
					_isEquation = true;
				}

				public OpElement(string op)
				{
					_equation = null;
					_operand = op;
					_isEquation = false;
				}

				public bool IsEquation => _isEquation;

				public Equation Equation => _equation;
				public string Operand => _operand;

				public override string ToString()
				{
					string result;

					if (_isEquation)
					{
						result = _equation.ToString();
					} 
					else
					{
						result = _operand;
					}

					return result;
				}
			}
		}
	}
}