﻿#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// JsonResourceKeyWrapperConverterFactory.cs
// 
// 2020-03-20-4:00 AM
// 
// ---
#endregion
using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf.JSON
{
    /// <summary>
    /// </summary>
    public sealed class JsonResourceKeyWrapperConverterFactory : JsonConverterFactory
    {
        #region Overrides of JsonConverter
        /// <summary>
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public override bool CanConvert ( Type typeToConvert )
        {
            if ( typeof ( IResourceKeyWrapper1 ).IsAssignableFrom ( typeToConvert ) )
            {
                return true ;
            }

            return false ;
        }
        #endregion
        #region Overrides of JsonConverterFactory
        /// <summary>
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
            return new JsonResourceKeyWrapperConverter ( typeToConvert , options ) ;
        }

        /// <summary>
        /// </summary>
        private sealed class JsonResourceKeyWrapperConverter : JsonConverter < IResourceKeyWrapper1 >
        {
            /// <summary>
            /// </summary>
            /// <param name="typeToConvert"></param>
            /// <param name="options"></param>
            public JsonResourceKeyWrapperConverter (
                // ReSharper disable once UnusedParameter.Local
                Type                  typeToConvert
                // ReSharper disable once UnusedParameter.Local
              , JsonSerializerOptions options
            )
            {
            }

            #region Overrides of JsonConverter<IResourceKeyWrapper1>
            /// <summary>
            /// </summary>
            /// <param name="reader"></param>
            /// <param name="typeToConvert"></param>
            /// <param name="options"></param>
            /// <returns></returns>
            [ CanBeNull ]
            public override IResourceKeyWrapper1 Read (
                ref Utf8JsonReader    reader
              , Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
                return null ;
            }

            /// <summary>
            /// </summary>
            /// <param name="writer"></param>
            /// <param name="value"></param>
            /// <param name="options"></param>
            public override void Write (
                [ NotNull ] Utf8JsonWriter        writer
              , [ NotNull ] IResourceKeyWrapper1  value
              , JsonSerializerOptions options
            )
            {
                writer.WriteStringValue ( value.ResourceKeyObject.ToString ( ) ) ;
            }
            #endregion
        }
        #endregion
    }
}