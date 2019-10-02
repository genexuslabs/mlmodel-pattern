using Artech.Architecture.Common.Events;
using Artech.Architecture.Common.Packages;
using Artech.Packages.Patterns;
using Artech.Packages.Patterns.Custom;
using Artech.Packages.Patterns.Definition;
using Microsoft.Practices.CompositeUI.EventBroker;
using System;

[assembly: PackageCompatibility(Version = 123130)]
[assembly: PatternImplementation(typeof(Genexus.Patterns.MLModel.MLModelPattern))]

namespace Genexus.Patterns.MLModel
{
	public class MLModelPattern : PatternImplementation
	{
		public static Guid Id
		{
			get {  return new Guid("0A957A50-C1E5-42FF-84FB-B55980FD2638"); }
		}

		public static PatternDefinition Definition
		{
			get { return PatternEngine.GetPatternDefinition(Id); }
		}
	
		public override void Initialize()
		{
			base.Initialize();
			EventsService.Events.Participate(this);
		}

		[EventSubscription(ArchitectureEvents.AfterOpenKB)]
		public virtual void OnAfterOpenKB(object sender, KBEventArgs e)
		{
			// Subscribe to contextual events
			e.KB.Events.Participate(this);
		}

		[EventSubscription(ArchitectureEvents.AfterSaveKBObject)]
		public void OnAfterSave(object sender, KBObjectSaveEventArgs args)
		{
		}

		public override IDefaultInstanceGenerator GetInstanceGenerator()
		{
			return new MLModelInstanceGenerator();
		}

		public override IPatternBuildProcess GetBuildProcess()
		{
			return new MLModelBuildProcess();
		}
	}
}
