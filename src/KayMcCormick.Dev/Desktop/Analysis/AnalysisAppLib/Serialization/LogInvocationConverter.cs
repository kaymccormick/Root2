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
#if FINDLOGUSAGES
using System ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using FindLogUsages ;
using JetBrains.Annotations ;

namespace AnalysisAppLib.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public class LogInvocationConverter : JsonConverter < ILogInvocation >
    {
        #region Overrides of JsonConverter<ILogInvocation>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [ CanBeNull ]
        public override ILogInvocation Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return null ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public override void Write (
            [ JetBrains.Annotations.NotNull ] Utf8JsonWriter             writer
          , [ JetBrains.Annotations.NotNull ] ILogInvocation value
          , JsonSerializerOptions      options
        )
        {
            if ( value == null )
            {
                throw new ArgumentNullException ( nameof ( value ) ) ;
            }

            writer.WriteStartObject ( ) ;
            writer.WriteString ( "SourceLocation" , value.SourceLocation ) ;
            writer.WriteString ( "LoggerType" ,     value.LoggerType ) ;
            writer.WriteString ( "MethodName" ,     value.MethodName ) ;
            writer.WriteString ( "Code" ,           value.Code ) ;
            writer.WritePropertyName ( "RelevantNode" ) ;
            JsonSerializer.Serialize ( writer , value.TransformedRelevantNode ) ;
            writer.WriteString ( "PrecedingCode" , value.PrecedingCode ) ;
            writer.WriteString ( "FollowingCode" , value.FollowingCode ) ;
            writer.WriteStartArray ( "Arguments" ) ;
            foreach ( var logInvocationArgument in value.Arguments )
            {
                JsonSerializer.Serialize ( writer , logInvocationArgument.Pojo , options ) ;
            }

            writer.WriteEndArray ( ) ;
            writer.WriteEndObject ( ) ;
        }
        #endregion
    }
}
#endif