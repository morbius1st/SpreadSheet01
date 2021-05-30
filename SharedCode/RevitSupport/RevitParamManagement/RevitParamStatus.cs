#region using

using System;
using System.Collections.Generic;
using SharedCode.RevitSupport.RevitParamManagement;
using SpreadSheet01.Management;
using SpreadSheet01.RevitSupport.RevitCellsManagement;
using SpreadSheet01.RevitSupport.RevitParamManagement;
using static SpreadSheet01.RevitSupport.RevitParamManagement.ParamType;
//using static SharedCode.RevitSupport.RevitParamManagement.ErrorCodeList2;

#endregion

// username: jeffs
// created:  4/6/2021 7:20:58 PM

namespace SharedCode.RevitSupport.RevitManagement
{
	public class RevitParamStatus
	{

		public class ParamStatus
		{
			private ParamDesc paramDesc;
#pragma warning disable CS0649 // Field 'RevitParamStatus.ParamStatus.errors' is never assigned to, and will always have its default value null
			private ErrorCodeList errors;
#pragma warning restore CS0649 // Field 'RevitParamStatus.ParamStatus.errors' is never assigned to, and will always have its default value null

			public ParamStatus(ParamDesc pd)
			{
				paramDesc = pd;
			}

			public bool IsFound { get; set; }
			public bool HasErrors => errors.HasErrors;

			public ErrorCodes ErrorCode
			{
				set
				{
					errors.Add(value);
				}
			}
			public IEnumerator<ErrorCodes> GetErrors => errors.GetEnumerator();
			public List<ErrorCodes> ErrorsList => errors.ErrorsList;
		}


	#region private fields

		private Family fam;

		private ParamStatus[] statusListType;
		private ParamStatus[] statusListInstance;
		private ParamStatus[] statusListInternal;

		private List<ParamStatus[]> statusListLabel;

		
		private ParamStatus[][] statusList;

	#endregion

	#region ctor

		public RevitParamStatus(Family family)
		{
			fam = family;
			config();

		}

	#endregion

	#region public properties

		public Family Family => fam;

		public ParamStatus this[ParamType pt, int idx] => statusList[(int) pt][idx];


	#endregion

	#region private properties

	#endregion

	#region public methods

		public void SetFound(ParamType pt, int idx)
		{
			statusList[(int) pt][idx].IsFound = true;
		}

		public void SetError(ParamType pt, int idx, ErrorCodes error)
		{
			statusList[(int) pt][idx].ErrorCode = error;
		}
		
		public bool VerifyMustExist(ParamType pt)
		{
			int p = (int) pt;

			for (int i = 0; i < statusList[p].Length; i++)
			{
				if (fam[p,i].Exist == ParamExistReqmt.EX_PARAM_MUST_EXIST &&
					!statusList[p][i].IsFound) return false;
			}

			return true;
		}

	#endregion

	#region private methods

		private void config()
		{
			statusList = new ParamStatus[fam.NumberOfLists][];

			for (int i = 0; i < statusList.Length; i++)
			{
				statusList[i] = new ParamStatus[fam.ParamCounts[i]];
			}

			statusListType = new ParamStatus[fam.ParamCounts[(int) PT_TYPE]];
			statusListInstance = new ParamStatus[fam.ParamCounts[(int) PT_INSTANCE]];
			statusListInternal = new ParamStatus[fam.ParamCounts[(int) PT_INTERNAL]];

			statusListLabel = new List<ParamStatus[]>(2);
			statusListLabel[0] = new ParamStatus[fam.ParamCounts[(int) PT_LABEL]];

		}

	#endregion

	#region event consuming

	#endregion

	#region event publishing

	#endregion

	#region system overrides

		public override string ToString()
		{
			return "this is RevitParamStatus";
		}

	#endregion
	}
}