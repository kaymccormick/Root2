using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KayMcCormick.Dev
{
    public class JsonResourceNodeConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            DebugUtils.WriteLine(typeToConvert.FullName);
            if (typeof(ResourceNodeInfoBase).IsAssignableFrom(typeToConvert))
            {
                return true;
            }

            return false;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new InnerConverter(options);
            throw new NotImplementedException();
        }

        public class InnerConverter : JsonConverter<ResourceNodeInfoBase>
        {
            private readonly JsonSerializerOptions _options;

            public InnerConverter(JsonSerializerOptions options)
            {
                _options = options;
                
            }

            public override ResourceNodeInfoBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, ResourceNodeInfoBase value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Key");
                JsonSerializer.Serialize(writer, value.Key, options);
                writer.WritePropertyName("Data");
                JsonSerializer.Serialize(writer, value.Data, options);
                writer.WritePropertyName("Children");
                JsonSerializer.Serialize(writer, value.Children, options);
                writer.WriteEndObject();
            }
        }
    }
}