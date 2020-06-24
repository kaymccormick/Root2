using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Autofac.Core;
using JetBrains.Annotations;
using KayMcCormick.Dev.Attributes;
using KayMcCormick.Dev.Interfaces;

namespace KayMcCormick.Dev.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonConverters
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddJsonConverters(
            [NotNull] JsonSerializerOptions jsonSerializerOptions
        )
        {
            if (jsonSerializerOptions == null) throw new ArgumentNullException(nameof(jsonSerializerOptions));


            jsonSerializerOptions.Converters.Add(new JsonLifetimeScopeConverter());
            jsonSerializerOptions.Converters.Add(
                new JsonComponentRegistrationConverterFactory()
            );
            jsonSerializerOptions.Converters.Add(new JsonIViewModelConverterFactory());
            jsonSerializerOptions.Converters.Add(new JsonLazyConverterFactory());
            jsonSerializerOptions.Converters.Add(new JsonTypeConverterFactory());
#if RESOURCENODETREE
            jsonSerializerOptions.Converters.Add(new JsonResourceNodeConverterFactory());
#endif
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [PurposeMetadata("JSON Converter Factory")]
    [ConvertsTypeMetadata(typeof(IComponentRegistration))]
    public sealed class JsonComponentRegistrationConverterFactory : JsonConverterFactory , IHaveObjectId
    {
        #region Overrides of JsonConverter

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public override bool CanConvert(Type typeToConvert)
        {
            if (typeof(IComponentRegistration).IsAssignableFrom(typeToConvert)) return true;

            return false;
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
            return new JsonComponentRegistrationConverter();
        }

        #endregion

        /// <inheritdoc />
        public object InstanceObjectId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [PurposeMetadata("JSON Converter")]
    [ConvertsTypeMetadata(typeof(IComponentRegistration))]
    public sealed class
        JsonComponentRegistrationConverter : JsonConverter<IComponentRegistration> , IHaveObjectId
    {
        #region Overrides of JsonConverter<ComponentRegistration>

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [CanBeNull]
        public override IComponentRegistration Read(
            ref Utf8JsonReader reader
            , Type typeToConvert
            , JsonSerializerOptions options
        )
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(
            [NotNull] Utf8JsonWriter writer
            , [NotNull] IComponentRegistration value
            , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject();
            writer.WriteStartObject("Metadata");
            foreach (var keyValuePair in value.Metadata)
                writer.WriteString(keyValuePair.Key, keyValuePair.Value.ToString());

            writer.WriteEndObject();
            writer.WriteStartArray("Services");
            foreach (var valueService in value.Services) writer.WriteStringValue(valueService.Description);

            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        #endregion

        /// <inheritdoc />
        public object InstanceObjectId { get; set; }
    }
}