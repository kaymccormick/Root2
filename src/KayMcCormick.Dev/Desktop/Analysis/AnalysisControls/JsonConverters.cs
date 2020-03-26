using System ;
using System.Text.Json ;
using AnalysisAppLib.Serialization ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Serialization ;
using KayMcCormick.Lib.Wpf.JSON ;

namespace AnalysisControls
{
    /// <summary>
    ///   <para>Ckass to add supported converters for System.Text.Json that incorporate both Code Analyzers / Roslyn and WPF functions.</para>
    ///   <para></para>
    /// </summary>
    public static class JsonConverters
    {
        [ NotNull ]
        public static JsonSerializerOptions CreateJsonSerializeOptions ( )
        {
            var r = new JsonSerializerOptions();
            AddJsonConverters ( r ) ;
            return r ;
        }
        public static void AddJsonConverters ( [ NotNull ] JsonSerializerOptions jsonSerializerOptions )
        {
            if ( jsonSerializerOptions == null )
            {
                throw new ArgumentNullException ( nameof ( jsonSerializerOptions ) ) ;
            }

            jsonSerializerOptions.Converters.Add ( new JsonSyntaxNodeConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonConverterImage ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonConverterResourceDictionary ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new HashtableConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonDependencyPropertyConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonFontFamilyConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add ( new JsonSolidColorBrushConverter ( ) ) ;
            jsonSerializerOptions.Converters.Add (
                                                  new JsonResourceKeyWrapperConverterFactory ( )
                                                 ) ;
            jsonSerializerOptions.Converters.Add ( new JsonBrushConverter ( ) ) ;
            //jsonSerializerOptions.Converters.Add ( new JsonFrameworkElementConverter ( ) ) ;
        }
    }
}