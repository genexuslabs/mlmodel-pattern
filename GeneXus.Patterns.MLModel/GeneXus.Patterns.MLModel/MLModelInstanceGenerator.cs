using Artech.Architecture.Common.Descriptors;
using Artech.Architecture.Common.Objects;
using Artech.Architecture.Common.Packages;
using Artech.Common.Collections;
using Artech.Genexus.Common;
using Artech.Genexus.Common.Objects;
using Artech.Genexus.Common.Parts;
using Artech.Packages.Patterns;
using Artech.Packages.Patterns.Custom;
using Artech.Packages.Patterns.Objects;
using GeneXus.Patterns.MLModel.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genexus.Patterns.MLModel
{
    internal class MLModelInstanceGenerator: DefaultInstanceGenerator
	{
		public override void Generate(KBObject baseObject, PatternInstance instance)
		{
			MLModelInstance mlModelInstance = new MLModelInstance(instance.Model);
			mlModelInstance.Initialize();
			mlModelInstance.SaveTo(instance);
		}

		public override bool GetDependencies(IList<KBObjectDescriptor> dependencies)
		{
			dependencies.Add(PackageManager.Manager.GetKBObjectDescriptor(ObjClass.Attribute));
			return true;
		}
	}
}
