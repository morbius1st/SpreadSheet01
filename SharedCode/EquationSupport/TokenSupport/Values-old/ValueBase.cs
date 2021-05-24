#region using

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SharedCode.EquationSupport.Definitions;

using static SharedCode.EquationSupport.Definitions.ValueDefinitions;
using ValueType = SharedCode.EquationSupport.Definitions.ValueType;

#endregion

// username: jeffs
// created:  5/19/2021 9:23:08 AM

namespace SharedCode.EquationSupport.TokenSupport.Values
{
	// unit of measurement

	public interface IValBase
	{
		string Original { get; set; }		// the original value
		ValueDef ValueDef { get; set; }		// the definition for the value
		ValueType DataType { get;}			// the type of the value
		string ValueString {get;}			// the value as a string

		// from ValueDef
		int Id { get; }						// the Id of the value| simple numerical identifier
		int Seq { get; }					// a sequence number| varies per ValueDefGroup
		int Order { get; }					// the precedence for the value
		string Description { get; }			// the description of the value

		bool IsValid { get; set; }			// whether this item is valid

		// depending on the value definition
		string AsString();					// value as a string
		object AsObject();					// value as an object
		bool AsBool();						// value as a bool
		int AsInteger();					// value as an integer
		double AsDouble();					// value as a double
		UoM AsUnit();						// value as a unit of measure

	}

	public interface IValTypeSpecific<T>
	{
		T Amount { get; set; }				// the original value converted to its native value

		T DefaultAmt { get; }				// a default value
		T InvalidAmt { get; }				// value when invalid

		T ConvertFromString(string original);	// the conversion method
	}

	public abstract class AValBase<T> : IValBase, IValTypeSpecific<T> 
	{
		public abstract string Original { get; set; }
		public ValueDef ValueDef { get; set; }
		public ValueType DataType => ValueDef.ValueType;

		public string ValueString => ValueDef.ValueStr;
		public int Id    => ValueDef.Id;			// number for the definition order
		public int Seq   => ValueDef.Seq;			// the sequence number within a value def group
		public int Order => ValueDef.Order;		    // the order of operation / precedence order / higher gets done first
		public string Description => ValueDef.Description;

		public bool IsValid { get; set; }

		public abstract T Amount { get; set; }

		public virtual T DefaultAmt { get; }
		public virtual T InvalidAmt { get; }

		public virtual AValBase<T> Default { get; }
		public virtual AValBase<T> Invalid { get; }

		public abstract string AsString();
		public abstract T ConvertFromString(string test);

		public virtual bool AsBool() { return false;}            // value as a string
		public virtual double AsDouble() { return Double.NaN;}	 // value as an object
		public virtual int AsInteger() { return Int32.MinValue;} // value as a bool
		public virtual object AsObject() { return null;}		 // value as an integer
		public virtual UoM AsUnit() { return null;}				 // value as a double
	}															 // value as a unit of measure
}
