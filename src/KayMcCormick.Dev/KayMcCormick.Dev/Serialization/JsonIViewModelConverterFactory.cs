using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using KayMcCormick.Dev.Attributes;
using KayMcCormick.Dev.Interfaces;

namespace KayMcCormick.Dev.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    [PurposeMetadata("JSON Converter Factory")]
    [ConvertsTypeMetadata(typeof(IViewModel))]
    public sealed class JsonIViewModelConverterFactory : JsonConverterFactory, IHaveObjectId
    {
        #region Overrides of JsonConverter

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(IViewModel).IsAssignableFrom(typeToConvert)
                   && !typeToConvert
                       .GetCustomAttributes(typeof(NoJsonConverterAttribute), true)
                       .Any();
        }

        #endregion

        #region Overrides of JsonConverterFactory

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [NotNull]
        public override JsonConverter CreateConverter(
            Type typeToConvert
            , JsonSerializerOptions options
        )
        {
            return new JsonViewModelConverter();
        }

        private sealed class JsonViewModelConverter : JsonConverter<IViewModel>
        {
            #region Overrides of JsonConverter<IViewModel>

            [CanBeNull]
            public override IViewModel Read(
                ref Utf8JsonReader reader
                , Type typeToConvert
                , JsonSerializerOptions options
            )
            {
                return null;
            }

            public override void Write(
                [NotNull] Utf8JsonWriter writer
                , [NotNull] IViewModel value
                , JsonSerializerOptions options
            )
            {
                writer.WriteStringValue(value.ToString());
            }

            #endregion
        }

        #endregion

        /// <inheritdoc />
        public object InstanceObjectId { get; set; }
    }
}