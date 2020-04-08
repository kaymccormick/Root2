using System ;
using System.Linq ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using Autofac.Core ;
using Autofac.Core.Lifetime ;
using Autofac.Core.Registration ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Attributes ;

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
            if (jsonSerializerOptions == null)
            {
                throw new ArgumentNullException(nameof(jsonSerializerOptions));
            }

            
            jsonSerializerOptions.Converters.Add(new JsonLifetimeScopeConverter());
            jsonSerializerOptions.Converters.Add(
                                                 new JsonComponentRegistrationConverterFactory()
                                                );
            jsonSerializerOptions.Converters.Add(new JsonIViewModelConverterFactory());
            jsonSerializerOptions.Converters.Add(new JsonLazyConverterFactory());
            jsonSerializerOptions.Converters.Add (new JsonTypeConverterFactory()  );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [PurposeMetadata( "JSON Converter Factory")]
    [ConvertsTypeMetadata( typeof(IViewModel))]
    public sealed class JsonIViewModelConverterFactory : JsonConverterFactory
    {
        #region Overrides of JsonConverter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public override bool CanConvert ( Type typeToConvert )
        {
            return typeof ( IViewModel ).IsAssignableFrom ( typeToConvert )
                   && ! typeToConvert.GetCustomAttributes ( typeof ( NoJsonConverterAttribute ), true )
                                     .Any ( ) ;
        }
        #endregion
        #region Overrides of JsonConverterFactory
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [ NotNull ]
        public override JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return new JsonIViewModelConverter ( ) ;
        }

        private sealed class JsonIViewModelConverter : JsonConverter < IViewModel >
        {
            #region Overrides of JsonConverter<IViewModel>
            [ CanBeNull ]
            public override IViewModel Read (
                ref Utf8JsonReader    reader
              , Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
                return null ;
            }

            public override void Write (
                [ NotNull ] Utf8JsonWriter        writer
              , [ NotNull ] IViewModel            value
              , JsonSerializerOptions options
            )
            {
                writer.WriteStringValue ( value.ToString ( ) ) ;
            }
            #endregion
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class NoJsonConverterAttribute : Attribute
    {
    }

    /// <summary>
    /// 
    /// </summary>
    [PurposeMetadata( "JSON Converter Factory")]
    [ConvertsTypeMetadata( typeof(IComponentRegistration))]
    public sealed class JsonComponentRegistrationConverterFactory : JsonConverterFactory
    {
        #region Overrides of JsonConverter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public override bool CanConvert ( Type typeToConvert )
        {
            if ( typeof ( IComponentRegistration ).IsAssignableFrom ( typeToConvert ) )
            {
                return true ;
            }

            return false ;
        }
        #endregion
        #region Overrides of JsonConverterFactory
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [ NotNull ]
        public override JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return new JsonComponentRegistrationConverter ( ) ;
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    [PurposeMetadata( "JSON Converter")]
    [ConvertsTypeMetadata( typeof(IComponentRegistration))]
    public sealed class JsonComponentRegistrationConverter : JsonConverter < IComponentRegistration >
    {
        #region Overrides of JsonConverter<ComponentRegistration>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [ CanBeNull ]
        public override IComponentRegistration Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write (
            [ NotNull ] Utf8JsonWriter         writer
          , [ NotNull ] IComponentRegistration value
          , JsonSerializerOptions  options
        )
        {
            writer.WriteStartObject();
            writer.WriteStartObject("Metadata");
            foreach ( var keyValuePair in value.Metadata )
            {
                writer.WriteString (keyValuePair.Key, keyValuePair.Value.ToString()  );
            }
            writer.WriteEndObject();
            writer.WriteStartArray ( "Services" ) ;
            foreach ( var valueService in value.Services )
            {
                writer.WriteStringValue( valueService.Description ) ;
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary
    [PurposeMetadata("JSON Converter")]
    [ConvertsTypeMetadata(typeof(LifetimeScope))]
    public sealed class JsonLifetimeScopeConverter : JsonConverter < LifetimeScope >
    {
        #region Overrides of JsonConverter<LifetimeScope>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [ CanBeNull ]
        public override LifetimeScope Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write (
            [ NotNull ] Utf8JsonWriter        writer
          , [ NotNull ] LifetimeScope         value
          , JsonSerializerOptions options
        )
        {
            ConverterUtil.WritePreamble(this, writer, value, options);
            writer.WriteStartObject();
            writer.WritePropertyName("Tag");
            JsonSerializer.Serialize ( writer , value.Tag , value.Tag.GetType ( ) , options ) ;
            var p = value.ParentLifetimeScope ;
            if ( p != null )
            {
                writer.WritePropertyName ( "ParentLifetimeScope" ) ;
                JsonSerializer.Serialize(writer, p.Tag, p.Tag.GetType(), options);
            }

            writer.WritePropertyName ( "ComponentRegistry" ) ;
            writer.WriteStartObject();
            writer.WriteStartArray("Registrations");
            foreach ( var componentRegistryRegistration in value.ComponentRegistry.Registrations )
            {
                JsonSerializer.Serialize (writer,
                                          componentRegistryRegistration
                                        , typeof ( ComponentRegistration )
                                        , options
                                         ) ;
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.WriteEndObject();
            ConverterUtil.WriteTerminal(writer);
        }
        #endregion
    }


}