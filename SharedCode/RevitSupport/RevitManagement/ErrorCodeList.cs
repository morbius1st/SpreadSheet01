using System;
using System.Collections;
using System.Collections.Generic;
using SpreadSheet01.Management;
//using static SharedCode.RevitSupport.RevitParamManagement.ErrorCodeList2;

namespace SharedCode.RevitSupport.RevitParamManagement
{
	public class ErrorCodeList : IEnumerable<ErrorCodes>
	{
		private List<ErrorCodes> errors;

		public ErrorCodeList()
		{
			Reset();
		}

		public List<ErrorCodes> ErrorsList => errors;

		public int Count => errors.Count;

		public bool HasErrors => Count > 0;

		public ErrorCodes this[int idx] => errors[idx];

		public void Add(ErrorCodes error)
		{
			
			if (errors.IndexOf(error) > -1 || error == ErrorCodes.EC_NO_ERROR) return;
			errors.Add(error);
		}

		public void Reset()
		{
			errors = new List<ErrorCodes>();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<ErrorCodes> GetEnumerator()
		{
			yield break;
		}
	}
	
	public struct ErrorCodeListMember
	{
		public ErrorCodes ErrorCode { get; private set; }
		public object Source { get; private set; }
		public Type Type {get; private set; }

		public ErrorCodeListMember(ErrorCodes errorCode, object source) 
		{
			ErrorCode = errorCode;
			Source = source;
			Type = source.GetType();
		}
	}
	
	// public class ErrorCodeList2
	// {
	//
	// 	private List<ErrorCodeListMember> errorList;
	//
	// 	static ErrorCodeList2()
	// 	{
	// 		ErrCodeList = new ErrorCodeList2();
	// 	}
	//
	// 	public ErrorCodeList2()
	// 	{
	// 		Reset();
	// 	}
	//
	// 	public static ErrorCodeList2 ErrCodeList { get; private set; }
	//
	// 	public int Count => errorList.Count;
	//
	// 	public List<ErrorCodeListMember> Errors => errorList;
	//
	// 	public bool HasErrors => errorList.Count > 0;
	//
	// 	public ErrorCodes this[ int idx] => errorList[idx].ErrorCode;
	//
	// 	public void Add(object source, ErrorCodes ec)
	// 	{
	// 		errorList.Add(new ErrorCodeListMember(ec, source));
	// 	}
	//
	// 	public void Reset()
	// 	{
	// 		errorList = new List<ErrorCodeListMember>();
	// 	}
	//
	// }


}