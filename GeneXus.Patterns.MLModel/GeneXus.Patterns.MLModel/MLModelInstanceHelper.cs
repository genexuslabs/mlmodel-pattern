using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Genexus.Patterns.MLModel
{
    public class MLModelInstanceHelper
    {
        #region Constants

        private static string kGeneXusAICustomModuleName = "GeneXusAI.Custom";
        private static string kDataOutputTypeFullName = $"{kGeneXusAICustomModuleName}.DataOutputType";
        private static string kLayerTypeFullName = $"{kGeneXusAICustomModuleName}.LayerType";

        #endregion

        #region Inputs

        #endregion

        #region Outputs

        public static string TemplateValueForOutputType(string outputType) {
            string templateValue = outputType;
            if (templateValue == "Category") {
                templateValue = "Label";
            }
            return $"{kDataOutputTypeFullName}.{templateValue}";
        }

        #endregion

		#region Layers

		public static string TemplateValueForLayerType(string layerType) {
            return $"{kLayerTypeFullName}." + Regex.Replace(layerType, @"\s+", "");
        }

		public static Dictionary<string, string> GetLayerProperties(LayerElement layer)
		{
			Dictionary<string, string> props = new Dictionary<string, string>();
			if (layer.LayerType == "Convolutional")
			{
				props.Add("num_filters", layer.Num_filters.ToString());
				props.Add("filter_size", layer.Filter_size.ToString());
				props.Add("pool_size", layer.Pool_size.ToString());
				props.Add("pool_stride", layer.Pool_stride.ToString());
				props.Add("dropout", layer.Dropout.ToString());
			}
			else
			{   // Fully Connected
				props.Add("fc_size", layer.Num_units.ToString());
				props.Add("dropout", layer.Dropout.ToString());
			}
			return props;
		}

		#endregion
	}
}
