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

namespace KayMcCormick.Lib.Wpf.JSON
{
    /// <summary>
    /// </summary>
    public class JsonResourceKeyWrapperConverterFactory : JsonConverterFactory
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
        public override System.Text.Json.Serialization.JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return new JsonResourceKeyWrapperConverter ( typeToConvert , options ) ;
        }

        /// <summary>
        /// </summary>
        public class JsonResourceKeyWrapperConverter : JsonConverter < IResourceKeyWrapper1 >
        {
            /// <summary>
            /// </summary>
            /// <param name="typeToConvert"></param>
            /// <param name="options"></param>
            public JsonResourceKeyWrapperConverter (
                Type                  typeToConvert
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