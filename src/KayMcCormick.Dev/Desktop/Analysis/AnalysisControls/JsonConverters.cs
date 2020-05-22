using System ;
using System.Text.Json ;
using AnalysisAppLib ;
using AnalysisAppLib.Serialization ;
using AnalysisControls.Serialization;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Serialization ;
using KayMcCormick.Lib.Wpf.JSON ;

namespace AnalysisControls
{
    /// <summary>
    ///     <para>
    ///         Class to add supported converters for System.Text.Json that
    ///         incorporate both Code Analyzers / Roslyn and WPF functions.
    ///     </para>
    ///     <para></para>
    /// </summary>
    public static class JsonConverters
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [ NotNull ]
        public static JsonSerializerOptions CreateJsonSerializeOptions ( )
        {
            var r = new JsonSerializerOptions ( ) ;
            AddJsonConverters ( r ) ;
            return r ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonSerializerOptions"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddJsonConverters (
            [ NotNull ] JsonSerializerOptions jsonSerializerOptions
        )
        {
                if ( jsonSerializerOptions == null )
            {
                throw new ArgumentNullException ( nameof ( jsonSerializerOptions ) ) ;
            }

            KayMcCormick.Dev.Serialization.JsonConverters.AddJsonConverters (
                                                                             jsonSerializerOptions
                                                                            ) ;
            jsonSerializerOptions.Converters.Add ( new JsonPocoSyntaxConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonSyntaxNodeConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonImageConverterFactory ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonConverterResourceDictionary ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new HashtableConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonDependencyPropertyConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonFontFamilyConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonSolidColorBrushConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add (
                                                  new JsonResourceKeyWrapperConverterFactory ( )
                                                 ) ;
            jsonSerializerOptions.Converters.Add ( new JsonBrushConverter ( ) ) ;


            jsonSerializerOptions.Converters.Add (
                                                  new JsonSimpleFrameworkElementConverterFactory ( )
                                                 ) ;
        }
    }
}