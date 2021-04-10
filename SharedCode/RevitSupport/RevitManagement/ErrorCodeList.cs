// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             ErrorCodeList.cs
// Created:      2021-04-08 (7:25 PM)

using System.Collections;
using System.Collections.Generic;
using SpreadSheet01.Management;

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
			if (errors.IndexOf(error) < 0) return;
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
}