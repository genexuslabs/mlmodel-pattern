using Artech.Architecture.BL.Framework.Services;
using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Services;
using Artech.Common.Collections;
using Artech.Genexus.Common;
using Artech.Genexus.Common.CustomTypes;
using Artech.Genexus.Common.Objects;
using Artech.Packages.Patterns.Custom;
using Artech.Packages.Patterns.Definition;
using Artech.Packages.Patterns.Engine;
using Artech.Packages.Patterns.Objects;
using Artech.Udm.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genexus.Patterns.MLModel
{
	internal class MLModelBuildProcess : PatternBuildProcess
	{
		/*
		public override void AfterImportResources(PatternInstance instance)
		{
			base.AfterImportResources(instance);

			BLServices.ModuleManager.InstallBuiltIn(instance.Model, "GeneXusAI");
		}
		*/
		public override void BeforeGenerateObjects(PatternInstance instance, IBaseCollection<PatternObject> buildObjects)
		{
			base.BeforeGenerateObjects(instance, buildObjects);

			System.Diagnostics.Debug.Assert(false, "MCrispino");

			ModulePackage gxAIModule = null;
			foreach (IModuleManagerServer server in BLServices.ModuleManager.ListServers())
			{
				gxAIModule = server.List().Where(m => m.Name == "GeneXusAI").FirstOrDefault();
				if (gxAIModule != null)
					break;
			}

			if (gxAIModule != null && BLServices.ModuleManager.GetInstalledVersion(instance.Model, gxAIModule) == null)
			{
				BLServices.ModuleManager.Install(instance.Model, gxAIModule);
			}
		}

		public override void BeforeSaveObjects(PatternInstance instance, InstanceObjects instanceObjects)
		{
			base.BeforeSaveObjects(instance, instanceObjects);

			foreach (InstanceObject instanceObject in instanceObjects)
			{
				string outputSDTName;

				List<KeyValuePair<string, object>> props = new List<KeyValuePair<string, object>>();
				if (instanceObject.Name.EndsWith("DataProvider"))
				{
					props.Add(new KeyValuePair<string, object>("OutputCollection", true));
					props.Add(new KeyValuePair<string, object>("OutputCollectionName", "DataCollection"));

					outputSDTName = "Data";
				}
				else
				{
					outputSDTName = "DataDefinition";
				}

				SDT outputSDT = SDT.Get(instanceObject.Model, new QualifiedName(outputSDTName));
				if (outputSDT != null)
				{
					props.Add(new KeyValuePair<string, object>("OutputSDT", new DataProviderOutputReference(outputSDT)));
				}				

				instanceObject.GeneratedObject.SetPropertyValues(props);
			}
		}
	}
}
