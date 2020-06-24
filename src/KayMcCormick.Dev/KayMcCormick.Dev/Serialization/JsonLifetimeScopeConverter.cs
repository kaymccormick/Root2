using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Autofac.Core.Lifetime;
using Autofac.Core.Registration;
using JetBrains.Annotations;
using KayMcCormick.Dev.Attributes;
using KayMcCormick.Dev.Interfaces;

namespace KayMcCormick.Dev.Serialization
{
    /// <inheritdoc />
    [PurposeMetadata("JSON Converter")]
    [ConvertsTypeMetadata(typeof(LifetimeScope))]
    public sealed class JsonLifetimeScopeConverter : JsonConverter<LifetimeScope>, IHaveObjectId
    {
        #region Overrides of JsonConverter<LifetimeScope>

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [CanBeNull]
        public override LifetimeScope Read(
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
            , [NotNull] LifetimeScope value
            , JsonSerializerOptions options
        )
        {
            ConverterUtil.WritePreamble(this, writer, value, options);
            writer.WriteStartObject();
            writer.WritePropertyName("Tag");
            JsonSerializer.Serialize(writer, value.Tag, value.Tag.GetType(), options);
            var p = value.ParentLifetimeScope;
            if (p != null)
            {
                writer.WritePropertyName("ParentLifetimeScope");
                JsonSerializer.Serialize(writer, p.Tag, p.Tag.GetType(), options);
            }

            writer.WritePropertyName("ComponentRegistry");
            writer.WriteStartObject();
            writer.WriteStartArray("Registrations");
            foreach (var componentRegistryRegistration in value.ComponentRegistry.Registrations)
                JsonSerializer.Serialize(
                    writer
                    , componentRegistryRegistration
                    , typeof(ComponentRegistration)
                    , options
                );

            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.WriteEndObject();
            ConverterUtil.WriteTerminal(writer);
        }

        #endregion

        /// <inheritdoc />
        public object InstanceObjectId { get; set; }
    }
}