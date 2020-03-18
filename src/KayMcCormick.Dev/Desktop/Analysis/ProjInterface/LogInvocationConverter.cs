#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// LogInvocationConverter.cs
// 
// 2020-03-17-1:17 PM
// 
// ---
#endregion
using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using AnalysisFramework.Interfaces ;
using JetBrains.Annotations ;

namespace ProjInterface
{
    internal class LogInvocationConverter : JsonConverter<ILogInvocation>
    {
        #region Overrides of JsonConverter<ILogInvocation>
        public override ILogInvocation Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        public override void Write (
            Utf8JsonWriter        writer
          , [ NotNull ] ILogInvocation        value
          , JsonSerializerOptions options
        )
        {
            if ( value == null )
            {
                throw new ArgumentNullException ( nameof ( value ) ) ;
            }

            writer.WriteStartObject();
            writer.WriteString ( "SourceLocation" , value.SourceLocation ) ;
            writer.WriteString ( "LoggerType" ,     value.LoggerType ) ;
            writer.WriteString ( "MethodName" ,     value.MethodName ) ;
            writer.WriteString ( "Code" ,           value.Code ) ;
            writer.WritePropertyName ( "RelevantNode" ) ;
            JsonSerializer.Serialize ( writer , value.TransformedRelevantNode ) ;
            writer.WriteString("PrecedingCode",     value.PrecedingCode);
            writer.WriteString ( "FollowingCode" ,  value.FollowingCode ) ;
            writer.WriteStartArray ( "Arguments" ) ;
            foreach ( var logInvocationArgument in value.Arguments )
            {
                JsonSerializer.Serialize ( writer , logInvocationArgument.Pojo , options ) ;
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
        #endregion
    }
}