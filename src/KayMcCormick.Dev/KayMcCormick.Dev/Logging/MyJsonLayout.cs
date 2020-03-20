#region header
// Kay McCormick (mccor)
// 
// Deployment
// KayMcCormick.Dev
// MyJsonLayout.cs
// 
// 2020-03-10-4:15 AM
// 
// ---
#endregion
using System ;
using System.Reflection ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using Autofac ;
using NLog ;
using NLog.Layouts ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class MyJsonLayout : Layout
    {
        private JsonSerializerOptions options;

        /// <summary>
        /// 
        /// </summary>
        public MyJsonLayout()
        {
            var jsonSerializerOptions = CreateJsonSerializerOptions ( ) ;
            //options.Converters.Add ( new DictConverterFactory ( ) ) ;
            Options = jsonSerializerOptions ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JsonSerializerOptions CreateJsonSerializerOptions ( )
        {
            var jsonSerializerOptions = new JsonSerializerOptions ( ) ;
            
            jsonSerializerOptions.Converters.Add ( new JsonConverterLogEventInfo ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonTypeConverterFactory ( ) ) ;
            return jsonSerializerOptions ;
        }

        /// <summary>
        /// 
        /// </summary>
        public JsonSerializerOptions Options { get => options; set => options = value; }

        #region Overrides of Layout
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logEvent"></param>
        /// <returns></returns>
        protected override string GetFormattedMessage(LogEventInfo logEvent)
        {
            return JsonSerializer.Serialize(logEvent, Options);
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class JsonTypeConverterFactory : JsonConverterFactory
    {
        #region Overrides of JsonConverter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public override bool CanConvert ( Type typeToConvert )
        {
            return typeof ( Type ).IsAssignableFrom ( typeToConvert ) ;
        }
        #endregion
        #region Overrides of JsonConverterFactory
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            if ( typeof ( TypeInfo ).IsAssignableFrom ( typeToConvert ) )
            {
                return new JsonTypeInfoConverter ( ) ;
            }
            return new JsonTypeConverter();
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class JsonTypeInfoConverter : JsonConverter<TypeInfo>
    {
        #region Overrides of JsonConverter<TypeInfo>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override TypeInfo Read ( ref Utf8JsonReader reader , Type typeToConvert , JsonSerializerOptions options ) { return null ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write (
            Utf8JsonWriter        writer
          , TypeInfo              value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject();
            writer.WriteString("TypeInfo", value.AssemblyQualifiedName);
            writer.WriteEndObject();
        }
        #endregion
    }
}