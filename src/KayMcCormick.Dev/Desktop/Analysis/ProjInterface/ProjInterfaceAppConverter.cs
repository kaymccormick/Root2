#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// ProjInterfaceAppConverter.cs
// 
// 2020-03-20-3:59 AM
// 
// ---
#endregion
using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;

namespace ProjInterface
{
    public class ProjInterfaceAppConverter : JsonConverter < ProjInterfaceApp >
    {
        #region Overrides of JsonConverter<ProjInterfaceApp>
        public override ProjInterfaceApp Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        public override void Write (
            Utf8JsonWriter        writer
          , ProjInterfaceApp      value
          , JsonSerializerOptions options
        )
        {
            writer.WriteStartObject ( ) ;
            writer.WriteString ( "ApplicationType" , value.GetType ( ).FullName ) ;
            writer.WriteEndObject ( ) ;
        }
        #endregion
    }
}