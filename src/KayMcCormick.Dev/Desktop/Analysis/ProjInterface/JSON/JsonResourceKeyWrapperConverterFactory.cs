#region header
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
using JsonConverter = System.Text.Json.Serialization.JsonConverter ;

namespace KayMcCormick.Lib.Wpf.JSON
{
    public class JsonResourceKeyWrapperConverterFactory : JsonConverterFactory
    {
        #region Overrides of JsonConverter
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
        public override System.Text.Json.Serialization.JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return new JsonResourceKeyWrapaperConverter ( typeToConvert , options ) ;
        }

        public class JsonResourceKeyWrapaperConverter : JsonConverter < IResourceKeyWrapper1 >
        {
            public JsonResourceKeyWrapaperConverter (
                Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
            }

            #region Overrides of JsonConverter<IResourceKeyWrapper1>
            public override IResourceKeyWrapper1 Read (
                ref Utf8JsonReader    reader
              , Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
                return null ;
            }

            public override void Write (
                Utf8JsonWriter        writer
              , IResourceKeyWrapper1  value
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