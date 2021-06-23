#region using
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using SharedCode.EquationSupport.Definitions;
using static SharedCode.EquationSupport.EqSupport.ValueSupport.NumSupport;
using static SharedCode.EquationSupport.Definitions.ValueDefinitions;


using ValueType = SharedCode.EquationSupport.Definitions.ValueType;

#endregion

// username: jeffs
// created:  5/22/2021 6:41:52 AM

namespace SharedCode.EquationSupport.TokenSupport.Amounts
{

	public abstract class AAmtBase
	{
		public abstract ValueDataGroup DataGroup { get;}
		public string Original { get; set; }                  // the original value
		public bool IsValid { get; protected set; }      // whether this item is valid

		// public AValDefBase ValDef { get; protected set; }     // the definition for the value

		// public static AAmtBase Invalid => new AmtInvalid();
		// public static AAmtBase Default => new AmtDefault();

		// from ValueDef
		// public string ValueString => ValueDef.ValueStr;
		// public ValueType ValueType => ValDef.ValueType; // the type of the value

		// public int Id => ValDef.Id;                    // number of the definition order
		// public int Seq => ValueDef.Seq;                  // the sequence number within a value def group
		// public int Order => ValDef.Order;              // the order of operation / precedence order / higher gets done first
		// public string Description => ValDef.Description;

		// public AValDefBase SetValueDef(string original, int idx)
		// {
		// 	if (original == null) return (AValDefBase) ValDefInst.Invalid;
		//
		// 	// AmtVarKey v = (AmVarDefInst[idx];
		// 	// AValDefBase b = (AValDefBase) ValDefInst[idx];
		// 	AValDefBase b = ValDefInst[idx];
		//
		// 	return (AValDefBase) ValDefInst[idx];
		// }

		public abstract string AsString();

		// depending on the value definition
		public virtual bool? AsBool()    => DefaultBool;
		public virtual double AsDouble() => DefaultDouble;
		public virtual int AsInteger()   => DefaultInt;
		public virtual object AsObject() => DefaultObj;
		public virtual UoM AsUnit()      => DefaultUnit;

		public override string ToString()
		{
			return $"this is| {this.GetType().Name} ({Original})";
		}
	}
}