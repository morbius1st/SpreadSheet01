#region using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using UtilityLibrary;

#endregion

// username: jeffs
// created:  3/21/2021 1:56:58 PM

namespace SharedRevitCode.ShParamUtils
{
	public class ParamUtils
	{
	#region private fields

		private Application app;
		private Document doc;

		private FamilyManager famMgr;

		private DefinitionFile sharedFile;

		private string filePath;

		// private Dictionary<string, FamilyParam>

	#endregion

	#region ctor

		public ParamUtils(Application app, Document doc)
		{
			this.app = app;
			this.doc = doc;

			// famMgr = doc.FamilyManager;
		}

	#endregion

	#region public properties

	#endregion

	#region private properties

	#endregion

	#region public methods


		public bool ReOrderParameters()
		{
			if (!doc.IsFamilyDocument) return false;

			IList<FamilyParameter> fpNew = reOrderParams(famMgr.GetParameters());

			using (Transaction tx = new Transaction(doc))
			{
				tx.Start("ReOrder Parameters");

				famMgr.ReorderParameters(fpNew);

				tx.Commit();
			}

			return true;
		}

		private IList<FamilyParameter> reOrderParams(IList<FamilyParameter> fpCurr)
		{
			IList<FamilyParameter> fpNew = new List<FamilyParameter>(fpCurr.Count);

			for (int i = 0; i < fpCurr.Count; i++)
			{
				fpNew.Add(null);
			}

			int idx = 2;

			foreach (FamilyParameter fp in fpCurr)
			{
				if (fp.Definition.Name.Equals("Developer"))
				{
					fpNew[1] = fp;
				}
				else if (fp.Definition.Name.Equals("InternalName"))
				{
					fpNew[0] = fp;
				}
				else
				{
					fpNew[idx++] = fp;
				}
			}
			return fpNew;
		}




		private IList<FamilyParameter> orderParams(IList<FamilyParameter> fpCurr)
		{
			SortedDictionary<int, FamilyParameter> fpTemp = 
				new SortedDictionary<int, FamilyParameter>();

			int idx = 2;

			foreach (FamilyParameter fp in fpCurr)
			{
				if (fp.Definition.Name.Equals("Developer"))
				{
					fpTemp[1] = fp;
				}
				else if (fp.Definition.Name.Equals("InternalName"))
				{
					fpTemp[0] = fp;
				}
				else
				{
					fpTemp[idx++] = fp;
				}
			}

			IList<FamilyParameter> fpNew = new List<FamilyParameter>(fpCurr.Count);



			return fpNew;
		}



		private bool addSharedParametersFromMemory()
		{
			using (Transaction tx = new Transaction(doc))
			{
				tx.Start("Add Shared Parameters");

				DefinitionGroups a = sharedFile.Groups;
				// DefinitionGroup g = a.Create("CyberStudio");
				DefinitionGroup g = a.get_Item("CyberStudio");

				addOneSharedParameter(g, "Shared_Length", ParameterType.Length, BuiltInParameterGroup.PG_GEOMETRY, 
					"4127a005-7cac-477d-8edf-496b98785e8d");
				addOneSharedParameter(g, "Shared_Yes_No", ParameterType.YesNo, BuiltInParameterGroup.PG_DATA, 
					"2d83ab6e-7ecb-47f5-8e8a-a31194a2ec19");
				addOneSharedParameter(g, "Formula", ParameterType.Text, BuiltInParameterGroup.PG_DATA,
					"57c57070-0830-4c8b-932c-664e0ed5bc57", "=", "", true, false);
				addOneSharedParameter(g, "Developer", ParameterType.Text, BuiltInParameterGroup.PG_IDENTITY_DATA, 
					"57c57070-0830-4c8b-932c-664e0ed5bc58", "CyberStudio", "", false, false);
				addOneSharedParameter(g, "InternalName", ParameterType.Text, BuiltInParameterGroup.PG_IDENTITY_DATA, 
					"57c57070-0830-4c8b-932c-664e0ed5bc59", "Chart", "", false, false, false);;

				tx.Commit();
			}

			return true;
		}

		private void addOneSharedParameter(DefinitionGroup g,
			string name, 
			ParameterType type,
			BuiltInParameterGroup @group,
			string guid, dynamic value = null, 
			string description = "",
			bool isInstance = true,
			bool userModifiable = true,
			bool visible = true)
		{
			FamilyParameter p  = 
				doc.FamilyManager.get_Parameter(name);

			if (p != null) return;

			ExternalDefinitionCreationOptions o = new ExternalDefinitionCreationOptions(name, type);
			
			o.UserModifiable = userModifiable;
			o.Description = description;
			o.Visible = visible;
			o.GUID = new Guid(guid);
			ExternalDefinition d = (g.Definitions.Create(o) as ExternalDefinition);
			
			p =
				famMgr.AddParameter(d, group, isInstance);

			if (value != null)
			{
				famMgr.Set(p, value);
			}


		}



		// public bool AddSharedParameter()
		// {
		// 	if (File.Exists(filePath) &&
		// 		null == sharedFile)
		// 	{
		// 		Debug.WriteLine("SharedParameter.txt has an invalid format.");
		// 		return false;
		// 	}
		//
		// 	using (Transaction tx = new Transaction(doc))
		// 	{
		// 		tx.Start("Add Shared Parameters");
		//
		// 		foreach (DefinitionGroup group in sharedFile.Groups)
		// 		{
		// 			int count = 0;
		//
		// 			foreach (ExternalDefinition def in group.Definitions)
		// 			{
		//
		// 				// check whether the parameter already exists in the document
		// 				FamilyParameter param = famMgr.get_Parameter(def.Name);
		// 				if (null != param)
		// 				{
		// 					continue;
		// 				}
		//
		// 				try
		// 				{
		// 					famMgr.AddParameter(def, def.ParameterGroup, true);
		// 				}
		// 				catch (Exception e)
		// 				{
		// 					Debug.WriteLine("! *** Exception| " + e.Message);
		//
		// 					if (e.InnerException != null)
		// 					{
		// 						Debug.WriteLine("! *** Inner Exception| " + e.InnerException.Message);
		// 					}
		//
		// 					return false;
		// 				}
		// 			}
		// 		}
		//
		// 		// DefinitionGroups a = sharedFile.Groups;
		// 		// DefinitionGroup g = a.Create("CyberStudio");
		// 		// ExternalDefinitionCreationOptions o = new ExternalDefinitionCreationOptions("Test Name", ParameterType.Text);
		// 		// o.UserModifiable = false;
		// 		// o.Description = "my description";
		// 		// o.GUID = Guid.NewGuid();
		// 		// ExternalDefinition d = (g.Definitions.Create(o) as ExternalDefinition);
		// 		//
		// 		// famMgr.AddParameter(d, BuiltInParameterGroup.PG_DATA, true);
		//
		//
		// 		tx.Commit();
		// 	}
		//
		// 	return true;
		// }

		public bool CreateSharedParametersFromTempFile(out string tempFile)
		{
			tempFile = null;

			if (!doc.IsFamilyDocument) return false;

			tempFile = Path.GetTempFileName();

			try
			{

				using(StreamWriter writer = new StreamWriter(tempFile))
				{
					writer.WriteLine(@"*META	VERSION	MINVERSION");
					writer.WriteLine(@"META	2	1");
					writer.WriteLine(@"*GROUP	ID	NAME");
					writer.WriteLine(@"GROUP	1	CyberStudio");
					writer.WriteLine(@"*PARAM	GUID	NAME	DATATYPE	DATACATEGORY	GROUP	VISIBLE	DESCRIPTION	USERMODIFIABLE	HIDEWHENNOVALUE");
					// //                                                                                  data   data            usr hid
					// //                          Guid                                    name            type   cat grp vis des mod none
					// writer.WriteLine(@"PARAM	4127a005-7cac-477d-8edf-496b98785e8d	Shared_Length	LENGTH		1	1		1	0");
					// writer.WriteLine(@"PARAM	2d83ab6e-7ecb-47f5-8e8a-a31194a2ec19	Shared_Yes_No	YESNO		1	1		1	0");
					// writer.WriteLine(@"PARAM	fab0ba7f-7ae0-45d0-bbaf-179231793c96	Shared_Integer	INTEGER		1	1		1	0");
					//
					// //                                                                          data  data             usr hid
					// //                          Guid                                    name    type  cat  grp vis des mod none
					// writer.WriteLine(@"PARAM	57c57070-0830-4c8b-932c-664e0ed5bc57	Formula	TEXT		1	1		0	0");
					//
					// //                                                                              data  data                     usr hid
					// //                          Guid                                    name        type  cat  grp vis  des        mod none
					// writer.WriteLine(@"PARAM	57c57070-0830-4c8b-932c-664e0ed5bc58	Developer	TEXT		1	1	Description	0	0");
					//
					//
					// //                                                                              data  data             usr hid
					// //                          Guid                                    name        type  cat  grp vis des mod none
					// writer.WriteLine(@"PARAM	426b089d-a89c-4a73-bf7e-dc328bd53491	Test Name	TEXT		1	0		1	0");
				}

				Debug.WriteLine("temp file| " + tempFile);

				bool exist;

				bool result = loadSharedParametersFromFile(tempFile, out exist);

				if (!result) return false;

				return addSharedParametersFromMemory();

			}
			catch
			{
				return false;
			}

			return true;
		}

		// public bool LoadSharedParametersFromFile(out bool exists)
		// {
		// 	return loadSharedParametersFromFile(
		// 		@"D:\Users\Jeff\Documents\Programming\VisualStudioProjects\RevitProjects\SpreadSheet\Revit\SharedParameter.txt",
		// 		out exists);
		// }

		private bool loadSharedParametersFromFile(string filepath, out bool exists)
		{
			exists = true;

			filePath = filepath;

			if (!File.Exists(filePath))
			{
				exists = false;

				return false;
			}

			app.SharedParametersFilename = filePath;

			try
			{
				sharedFile = app.OpenSharedParameterFile();
			}
			catch (Exception e)
			{
				Debug.WriteLine("! *** Exception| " + e.Message);

				if (e.InnerException != null)
				{
					Debug.WriteLine("! *** Inner Exception| " + e.InnerException.Message);
				}
				return false;
			}


			return true;
		}

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
			return "this is ParamUtils";
		}

	#endregion
	}
}