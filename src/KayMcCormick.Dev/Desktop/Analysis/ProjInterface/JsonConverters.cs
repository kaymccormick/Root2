using System ;
using System.Text.Json ;
using JetBrains.Annotations ;
using KayMcCormick.Lib.Wpf.JSON ;
using NLog.Targets ;

namespace ProjInterface
{
    public static class JsonConverters
    {
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
            jsonSerializerOptions.Converters.Add ( new ProjInterfaceAppConverter ( ) ) ;
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