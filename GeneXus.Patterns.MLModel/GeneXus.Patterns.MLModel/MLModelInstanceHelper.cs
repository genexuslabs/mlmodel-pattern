using Artech.Packages.Patterns.Objects;
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

		private static bool IsMultimediaType(string columnType)
		{
			return columnType == "Image" || columnType == "Audio" || columnType == "Video";
		}

		public static string TemplateValueForInputAttValue(InputElement input)
		{
			if (IsMultimediaType(input.ColumnType))
			{
				return $"&{input.Attribute.Name}.{input.ColumnType}URI";
			}
			else
			{
				return $"&{input.Attribute.Name}.ToString().Trim()";
			}
		}

		public static IEnumerable<KeyValuePair<string, string>> EnumerateInputProperties(InputElement input)
		{
			string columnType = input.ColumnType;
			yield return new KeyValuePair<string, string>("column_type", columnType);

			string elementType = input.ElementType;
			if (columnType == "Category" || columnType == "Vector" || columnType == "Set")
			{
				yield return new KeyValuePair<string, string>("element_type", elementType);
			}

			string missingValueStrategy = "";
			
			if (columnType == "Numeric" || (columnType == "Category" && elementType == "Numerics"))
			{
				missingValueStrategy = input.MissingValueStrategyNumeric;
				yield return new KeyValuePair<string, string>("missing_value_strategy", missingValueStrategy);
			}

			if (columnType == "Boolean" || (columnType == "Category" && elementType != "Numerics") || columnType == "Text" || columnType == "Datetime" | columnType == "Geopoint")
			{
				missingValueStrategy = input.MissingValueStrategy;
				yield return new KeyValuePair<string, string>("missing_value_strategy", missingValueStrategy);
			}

			if (columnType == "Boolean" && missingValueStrategy == "Fill with constant")
			{
				yield return new KeyValuePair<string, string>("fill_value", input.FillValueBoolean.ToString());
			}

			if (columnType == "Numeric" && missingValueStrategy == "Fill with constant")
			{
				yield return new KeyValuePair<string, string>("fill_value", input.FillValueNumeric.ToString());
			}

			if (columnType == "Category" && elementType == "Numerics" && missingValueStrategy == "Fill with constant")
			{
				yield return new KeyValuePair<string, string>("fill_value", input.FillValueCategoryNumerics.ToString());
			}

			if (columnType == "Category" && elementType == "Characters" && missingValueStrategy == "Fill with constant")
			{
				yield return new KeyValuePair<string, string>("fill_value", input.FillValueCategoryCharacters);
			}

			if (columnType == "Category")
			{
				yield return new KeyValuePair<string, string>("most_common_category", input.MostCommonCategory.ToString());
			}

			if (columnType == "Text" && missingValueStrategy == "Fill with constant")
			{
				yield return new KeyValuePair<string, string>("fill_value", input.FillValueText);
			}

			if (columnType == "Text")
			{
				yield return new KeyValuePair<string, string>("remove_diacritics", input.RemoveDiacriticsText.ToString());
				yield return new KeyValuePair<string, string>("stopwords_file", input.StopwordsFileText.ToString());
				yield return new KeyValuePair<string, string>("max_characters", input.MaxCharactersText.ToString());
				yield return new KeyValuePair<string, string>("char_padding_direction", input.CharacterPaddingDirectionText);
				yield return new KeyValuePair<string, string>("char_padding_symbol", input.CharacterPaddingSymbolText);
				yield return new KeyValuePair<string, string>("max_words", input.MaxWordsText.ToString());
				yield return new KeyValuePair<string, string>("word_padding_direction", input.WordPaddingDirectionText);
				yield return new KeyValuePair<string, string>("word_padding_symbol", input.WordPaddingSymbolText);
				yield return new KeyValuePair<string, string>("most_common_text", input.MostCommonText.ToString());
				yield return new KeyValuePair<string, string>("unknown_symbol", input.UnknownSymbolText);
				yield return new KeyValuePair<string, string>("tokenizer", input.TokenizerText);
			}

			if (columnType == "Audio")
			{
				yield return new KeyValuePair<string, string>("format", input.FormatAudio);
				yield return new KeyValuePair<string, string>("tracks", input.TracksAudio);
				yield return new KeyValuePair<string, string>("sample_rate", input.SampleRateMedia.ToString());
			}

			if (columnType == "Audio" || columnType == "Video")
			{
				yield return new KeyValuePair<string, string>("max_duration", input.MaxDurationMedia.ToString());
			}

			if (columnType == "Image")
			{
				yield return new KeyValuePair<string, string>("format", input.FormatImage);
				yield return new KeyValuePair<string, string>("channels", input.ChannelsImage);
				yield return new KeyValuePair<string, string>("add_horizontal_flip", input.AddHFlipImage.ToString());
				yield return new KeyValuePair<string, string>("add_rotated", input.AddRotated);
				yield return new KeyValuePair<string, string>("add_vertical_flip", input.AddVFlipImage.ToString());
				yield return new KeyValuePair<string, string>("add_decolored", input.AddDecolored.ToString());
				yield return new KeyValuePair<string, string>("add_detextured", input.AddDetextured.ToString());
				yield return new KeyValuePair<string, string>("add_edge_salient", input.AddEdgeSalient.ToString());
			}

			if (columnType == "Image" || columnType == "Video")
			{
				yield return new KeyValuePair<string, string>("scale_type", input.ScaleTypeMedia);
				yield return new KeyValuePair<string, string>("width", input.WidthMedia.ToString());
				yield return new KeyValuePair<string, string>("height", input.HeightMedia.ToString());
			}

			if (columnType == "Video")
			{
				yield return new KeyValuePair<string, string>("format", input.FormatVideo);
			}

			if (columnType == "Datetime")
			{
				if (missingValueStrategy == "Fill with constant")
				{
					yield return new KeyValuePair<string, string>("fill_value", input.FillValueDatetime);
				}

				var datetimeFormatDatetime = input.DatetimeFormatDatetime;
				yield return new KeyValuePair<string, string>("datetime_format", datetimeFormatDatetime);
				if (datetimeFormatDatetime != "UNIX epoch" && datetimeFormatDatetime != "Time only")
				{
					yield return new KeyValuePair<string, string>("date_format", input.DateFormatDatetime);
				}
				if (datetimeFormatDatetime != "UNIX epoch" && datetimeFormatDatetime != "Date only")
				{
					yield return new KeyValuePair<string, string>("time_format", input.TimeFormatDatetime);
				}
			}

			if (columnType == "Geopoint")
			{
				if (missingValueStrategy == "Fill with constant")
				{
					yield return new KeyValuePair<string, string>("fill_value", input.FillValueDatetime);
				}

				yield return new KeyValuePair<string, string>("format", input.FormatGeopoint);
			}

			if (columnType == "Region")
			{
				yield return new KeyValuePair<string, string>("format", input.FormatRegion);
				yield return new KeyValuePair<string, string>("separator", input.SeparatorRegion);
			}

			if (columnType == "Vector" || columnType == "Set")
			{
				yield return new KeyValuePair<string, string>("separator", input.SeparatorVectorOrSet);
				yield return new KeyValuePair<string, string>("size", input.SizeVectorOrSet.ToString());

				if (missingValueStrategy == "Fill with constant")
				{
					if (elementType == "Characters")
					{
						yield return new KeyValuePair<string, string>("padding_symbol", input.PaddingSymbolVectorOrSetCategories);
					}
					else
					{
						yield return new KeyValuePair<string, string>("padding_symbol", input.PaddingSymbolVectorOrSetNumerics.ToString());
					}
				}
			}

			if (columnType == "Text" || ((columnType == "Category" || columnType == "Vector" || columnType == "Set") && elementType == "Characters"))
			{
				yield return new KeyValuePair<string, string>("lowercase_characters", input.LowercaseCharacters.ToString());
			}

			if (columnType == "Category" && elementType == "Numerics")
			{
				yield return new KeyValuePair<string, string>("bucketize_numerics", input.BucketizeNumerics);
			}

			if (columnType == "Numeric")
			{
				yield return new KeyValuePair<string, string>("normalization_numerics", input.NormalizationNumeric);
			}
		}

		#endregion

		#region Outputs

		public static string TemplateValueForOutputType(string outputType) {
            string templateValue = TemplateKeyForOutputType(outputType);
            return $"{kDataOutputTypeFullName}.{templateValue}";
        }

        public static string TemplateKeyForOutputType(string outputType) 
		{
            string templateValue = outputType;
            if (templateValue == "Category") 
			{
                templateValue = "Label";
            }
            return templateValue;
        }
        
		public static string TemplateKeyForOutputType(OutputElement output)
		{
			if (string.IsNullOrEmpty(output?.ColumnType))
				return "Numeric";
			return TemplateKeyForOutputType(output?.ColumnType);
		}

		public static string TemplateValueForOutputAttValue(OutputElement output)
		{
			if (output == null)
			{
				return "'0'";
			}
			if (output.ColumnType == "Numeric")
			{
				return $"&{output.Attribute.Name}";
			}
			else
			{
				return $"&{output.Attribute.Name}.ToString().Trim()";
			}
		}

		public static IEnumerable<KeyValuePair<string, string>> EnumerateOutputProperties(OutputElement output)
		{
			string columnType = output.ColumnType;
			yield return new KeyValuePair<string, string>("column_type", columnType);

			yield return new KeyValuePair<string, string>("inspect_output", output.InspectOutput.ToString());

			
			if (columnType == "Numeric")
			{
				string missingValueStrategy = output.MissingValueStrategyNumeric;
				yield return new KeyValuePair<string, string>("missing_value_strategy", missingValueStrategy);

				if (missingValueStrategy == "Fill with constant")
				{
					yield return new KeyValuePair<string, string>("fill_value", output.FillValueNumeric.ToString());
				}

				yield return new KeyValuePair<string, string>("normalization_numeric", output.NormalizationNumeric);

				yield return new KeyValuePair<string, string>("loss", output.LossNumeric);
			}

			if (columnType == "Category")
			{
				string elementType = output.ElementType;
				yield return new KeyValuePair<string, string>("element_type", elementType);

				string missingValueStrategy = "";
				if (elementType == "Numerics")
				{
					missingValueStrategy = output.MissingValueStrategyCategoryNumerics;
				}
				else
				{
					missingValueStrategy = output.MissingValueStrategyCategoryCharacters;
				}
				yield return new KeyValuePair<string, string>("missing_value_strategy", missingValueStrategy);

				if (missingValueStrategy == "Fill with constant")
				{
					if (elementType == "Numerics")
					{
						yield return new KeyValuePair<string, string>("fill_value", output.FillValueCategoryNumerics.ToString());
					}
					else
					{
						yield return new KeyValuePair<string, string>("fill_value", output.FillValueCategoryCharacters);
					}
				}

				yield return new KeyValuePair<string, string>("most_common_category", output.MostCommonCategory.ToString());

				if (elementType == "Numerics")
				{
					yield return new KeyValuePair<string, string>("bucketize_numerics", output.BucketizeCategoryNumerics.ToString());
				}

				yield return new KeyValuePair<string, string>("loss", output.LossCategory);
			}

			if (columnType == "Region")
			{
				yield return new KeyValuePair<string, string>("format", output.FormatRegion);
				yield return new KeyValuePair<string, string>("separator", output.SeparatorRegion);
				yield return new KeyValuePair<string, string>("loss", output.LossRegion);
			}

			if (columnType == "Text")
			{
				yield return new KeyValuePair<string, string>("remove_diacritics", output.RemoveDiacriticsText.ToString());
				yield return new KeyValuePair<string, string>("stopwords_file", output.StopwordsFileText.ToString());
				yield return new KeyValuePair<string, string>("max_characters", output.MaxCharactersText.ToString());
				yield return new KeyValuePair<string, string>("char_padding_direction", output.CharacterPaddingDirectionText);
				yield return new KeyValuePair<string, string>("char_padding_symbol", output.CharacterPaddingSymbolText);
				yield return new KeyValuePair<string, string>("max_words", output.MaxWordsText.ToString());
				yield return new KeyValuePair<string, string>("word_padding_direction", output.WordPaddingDirectionText);
				yield return new KeyValuePair<string, string>("word_padding_symbol", output.WordPaddingSymbolText);
				yield return new KeyValuePair<string, string>("most_common_text", output.MostCommonText.ToString());
				yield return new KeyValuePair<string, string>("unknown_symbol", output.UnknownSymbolText);
				yield return new KeyValuePair<string, string>("tokenizer", output.TokenizerText);
				yield return new KeyValuePair<string, string>("loss", output.LossText);
				yield return new KeyValuePair<string, string>("level_text", output.LevelText);
			}

			yield return new KeyValuePair<string, string>("lowercase_characters", output.LowercaseCharacters.ToString());
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
