using Artech.Architecture.Common.Objects;
using Artech.Genexus.Common;
using Artech.Genexus.Common.CustomTypes;
using Artech.Genexus.Common.Objects;
using Artech.Packages.Patterns.Custom;
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
