using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using Autofac.Core ;
using Autofac.Core.Lifetime ;
using JetBrains.Annotations ;

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
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class JsonIViewModelConverterFactory : JsonConverterFactory
    {
        #region Overrides of JsonConverter
        public override bool CanConvert ( Type typeToConvert )
        {
            return typeof ( IViewModel ).IsAssignableFrom ( typeToConvert ) ;
        }
        #endregion
        #region Overrides of JsonConverterFactory
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
            public override IViewModel Read (
                ref Utf8JsonReader    reader
              , Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
                return null ;
            }

            public override void Write (
                Utf8JsonWriter        writer
              , IViewModel            value
              , JsonSerializerOptions options
            )
            {
                writer.WriteStringValue ( value.ToString ( ) ) ;
            }
            #endregion
        }
        #endregion
    }

    public class JsonComponentRegistrationConverterFactory : JsonConverterFactory
    {
        #region Overrides of JsonConverter
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
        public override JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return new JsonComponentRegistrationConverter ( ) ;
        }
        #endregion
    }

    public class JsonComponentRegistrationConverter : JsonConverter < IComponentRegistration >
    {
        #region Overrides of JsonConverter<ComponentRegistration>
        public override IComponentRegistration Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        public override void Write (
            Utf8JsonWriter         writer
          , IComponentRegistration value
          , JsonSerializerOptions  options
        )
        {
            writer.WriteStringValue ( value.ToString ( ) ) ;
        }
        #endregion
    }

    public class JsonLifetimeScopeConverter : JsonConverter < LifetimeScope >
    {
        #region Overrides of JsonConverter<LifetimeScope>
        public override LifetimeScope Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        public override void Write (
            Utf8JsonWriter        writer
          , LifetimeScope         value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStringValue ( value.ToString ( ) ) ;
        }
        #endregion
    }
}