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
using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Windows ;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonSimpleFrameworkElementConverterFactory : JsonConverterFactory
    {
        #region Overrides of JsonConverter
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
            public override FrameworkElement Read (
                ref Utf8JsonReader    reader
              , Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
                return null ;
            }

            public override void Write (
                Utf8JsonWriter        writer
              , FrameworkElement      value
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