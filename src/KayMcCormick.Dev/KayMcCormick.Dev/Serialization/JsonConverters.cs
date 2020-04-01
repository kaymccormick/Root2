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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public override bool CanConvert ( Type typeToConvert )
        {
            return typeof ( IViewModel ).IsAssignableFrom ( typeToConvert ) ;
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

    /// <summary>
    /// 
    /// </summary>
    public class JsonComponentRegistrationConverterFactory : JsonConverterFactory
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
    public class JsonComponentRegistrationConverter : JsonConverter < IComponentRegistration >
    {
        #region Overrides of JsonConverter<ComponentRegistration>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
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
            Utf8JsonWriter         writer
          , IComponentRegistration value
          , JsonSerializerOptions  options
        )
        {
            writer.WriteStringValue ( value.ToString ( ) ) ;
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class JsonLifetimeScopeConverter : JsonConverter < LifetimeScope >
    {
        #region Overrides of JsonConverter<LifetimeScope>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
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