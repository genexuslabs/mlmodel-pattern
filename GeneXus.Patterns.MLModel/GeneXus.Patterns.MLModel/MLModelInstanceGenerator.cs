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
			Transaction trn = baseObject as Transaction ?? throw new PatternException(Messages.ParentMustBeTransaction);

			MLModelInstance mlModelInstance = new MLModelInstance(baseObject.Model);

			mlModelInstance.Transaction = trn;

			TransactionAttribute outputAtt = BestGuessForOutputAttribute(trn);

			mlModelInstance.Inputs.AddRange(
				trn.Structure.Root.Attributes
					.Where(ShouldAddToDefaultInstance)
					.Where(att => att != outputAtt)
					.Select(att => new InputElement() {
						Attribute = att,
						ColumnType = ColumnTypeFromAttribute(att)
					})
			);

			mlModelInstance.Output = new OutputElement()
			{
				Attribute = outputAtt,
				ColumnType = ColumnTypeFromAttribute(outputAtt)
			};

			mlModelInstance.SaveTo(instance);
		}

		public override bool GetDependencies(IList<KBObjectDescriptor> dependencies)
		{
			dependencies.Add(PackageManager.Manager.GetKBObjectDescriptor(ObjClass.Transaction));
			dependencies.Add(PackageManager.Manager.GetKBObjectDescriptor(ObjClass.Table));
			dependencies.Add(PackageManager.Manager.GetKBObjectDescriptor(ObjClass.Attribute));
			return true;
		}

		private bool ShouldAddToDefaultInstance(TransactionAttribute att)
		{
			if (att.IsKey) return false;
			if (att.IsInferred) return false;
			if (att.TableAttribute.Attribute.Type != eDBType.NUMERIC && !att.IsForeignKey) return false;
			return true;
		}

		private TransactionAttribute BestGuessForOutputAttribute(Transaction trn)
		{
			IEnumerable<TransactionAttribute> candidateAtts = trn.Structure.Root.Attributes
				.Reverse()
				.Where(ShouldAddToDefaultInstance)
				.Where(att => !att.IsInferred)
				.Where(att => att.TableAttribute.Attribute.Type == eDBType.NUMERIC);

			if (candidateAtts.Count() == 0)
				return null;

			TransactionAttribute bestCandidate = candidateAtts.FirstThat(att => att.TableAttribute.Attribute.Decimals > 0) ?? candidateAtts.First();
			return bestCandidate;
		}

		private string ColumnTypeFromAttribute(TransactionAttribute att)
		{
			if (att == null) {
				return "Numeric";
			}
			return att.TableAttribute.Attribute.Type == eDBType.NUMERIC ? "Numeric" : "Label";
		}
	}
}
