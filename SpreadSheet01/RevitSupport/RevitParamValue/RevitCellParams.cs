using System.Collections.Generic;
using Autodesk.Revit.DB;
using static SpreadSheet01.RevitSupport.RevitCellParameters;


// Solution:     SpreadSheet01
// Project:       SpreadSheet01
// File:             RevitCellParams.cs
// Created:      2021-02-17 (6:43 PM)

namespace SpreadSheet01.RevitSupport.RevitParamValue
{
	public class RevitCellParams
	{
		public ARevitParam[] CellValues = new ARevitParam[ParamCounts[(int) ParamGroupType.DATA]];

		private AnnotationSymbol annoSymbol;

		private ParamDataType cellParamDataType;

		private List<RevitCellErrorCode> errors = new List<RevitCellErrorCode>();


		public RevitCellParams()
		{
			cellParamDataType = ParamDataType.EMPTY;

			HasError = false;
		}

		public ARevitParam this[int idx]
		{
			get => CellValues[idx];
			set => CellValues[idx] = value;
		}

		// information held in array

	#region held in array

		public string Key => annoSymbol.Id.ToString();

		public string Name
		{
			get => CellValues[NameIdx].GetValue();
			set
			{
				RevitParamText rv = new RevitParamText(value, CellAllParams[NameIdx]);
				CellValues[NameIdx] = rv;
			}
		}

		public string SeqId
		{
			get => CellValues[SeqIdx].GetValue();
			set
			{
				RevitParamText rv = new RevitParamText(value, CellAllParams[SeqIdx]);
				CellValues[NameIdx] = rv;
			}
		}

		public string CellAddr
		{
			get => CellValues[CellAddrIdx].GetValue();
			set
			{
				RevitParamText rv = new RevitParamText(value, CellAllParams[CellAddrIdx]);
				CellValues[CellAddrIdx] = rv;
			}
		}

		public bool DataIsToCell
		{
			get => CellValues[DataIsToCellIdx].GetValue();
			set
			{
				RevitParamBool rv = new RevitParamBool(value, CellAllParams[DataIsToCellIdx]);
				CellValues[DataIsToCellIdx] = rv;
			}
		}

		public bool? HasError
		{
			get => CellValues[HasErrorsIdx].GetValue();
			set
			{
				RevitParamBool rv = new RevitParamBool(value, CellAllParams[HasErrorsIdx]);
				CellValues[HasErrorsIdx] = rv;
			}
		}

	#endregion


	#region cell item data

		public ParamDataType CellParamDataType
		{
			get => cellParamDataType;
			set => cellParamDataType = value;
		}

		public AnnotationSymbol AnnoSymbol
		{
			get => annoSymbol;
			set => annoSymbol = value;
		}

		public RevitCellErrorCode Error
		{
			set
			{
				if (errors.Contains(value)) return;

				errors.Add(value);
				HasError = true;
			}
		}

	#endregion

		public IEnumerable<RevitCellErrorCode> Errors
		{
			get
			{
				foreach (RevitCellErrorCode error in errors)
				{
					yield return error;
				}
			}
		}

		public override string ToString()
		{
			return Name + " <|> " + CellAddr + " <|> " + (errors[0].ToString() ?? "No Errors");
		}
	}
}