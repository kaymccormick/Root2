#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisControls
// JsonSimpleFrameworkElementConverter.cs
// 
// 2020-03-29-11:33 AM
// 
// ---
#endregion

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;
using JetBrains.Annotations;

namespace AnalysisControls.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class JsonSimpleFrameworkElementConverterFactory : JsonConverterFactory
    {
        #region Overrides of JsonConverter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public override bool CanConvert ( Type typeToConvert )
        {
            if ( typeof ( FrameworkElement ).IsAssignableFrom ( typeToConvert ) )
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
            return new JsonSimpleFrameworkElementConverter ( ) ;
        }

        private sealed class JsonSimpleFrameworkElementConverter : JsonConverter < FrameworkElement >
        {
            #region Overrides of JsonConverter<FrameworkElement>
            [ CanBeNull ]
            public override FrameworkElement Read (
                ref Utf8JsonReader    reader
              , Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
                return null ;
            }

            public override void Write (
                [ NotNull ] Utf8JsonWriter        writer
              , [ NotNull ] FrameworkElement      value
              , JsonSerializerOptions options
            )
            {
                writer.WriteStringValue(value.GetType().FullName);
            }
            #endregion
        }
        #endregion
    }
}