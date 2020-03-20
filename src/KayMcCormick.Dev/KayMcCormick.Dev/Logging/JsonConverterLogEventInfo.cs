#region header
// Kay McCormick (mccor)
// 
// Deployment
// KayMcCormick.Dev
// LogEventInfoConverter.cs
// 
// 2020-03-10-4:15 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using System.Threading ;
using System.Threading.Tasks ;
using NLog ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonConverterLogEventInfo : JsonConverter < LogEventInfo >
    {
        #region Overrides of JsonConverter<LogEventInfo>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="JsonException"></exception>
        public override LogEventInfo Read (
            ref Utf8JsonReader    reader
          , Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            if ( reader.TokenType != JsonTokenType.StartObject )
            {
                throw new JsonException ( ) ;
            }

            //var builder = new LogBuilder(null);
            var info = new LogEventInstance( ) ;
            while ( reader.Read ( ) )
            {
                if ( reader.TokenType == JsonTokenType.EndObject )
                {
                    break ;
                }

                if ( reader.TokenType != JsonTokenType.PropertyName )
                {
                    throw new JsonException ( ) ;
                }

                var field = reader.GetString ( ) ;
                switch ( field )
                {
                    case "Level" :
                        reader.Read ( ) ;
                        if ( reader.TokenType != JsonTokenType.Number )
                        {
                            throw new JsonException ( ) ;
                        }

                        info.Level = reader.GetInt32 ( ) ;
                        break ;
                    case nameof ( LogEventInfo.LoggerName ) :
                        reader.Read ( ) ;
                        if ( reader.TokenType != JsonTokenType.String )
                        {
                            throw new JsonException ( ) ;
                        }

                        info.LoggerName = reader.GetString ( ) ;
                        break ;
                    case nameof ( LogEventInfo.FormattedMessage ) :
                        reader.Read ( ) ;
                        if ( reader.TokenType != JsonTokenType.String )
                        {
                            throw new JsonException ( ) ;
                        }

                        var msg = reader.GetString ( ) ;
                        info.FormattedMessage = msg ;
                        break ;
                    case "Message":
                        reader.Read();
                        if (reader.TokenType != JsonTokenType.String)
                        {
                            throw new JsonException();
                        }

                        var msg2 = reader.GetString();
                        info.Message = msg2;
                        break;
                    case nameof ( LogEventInfo.SequenceID ) :
                        reader.Read ( ) ;
                        if ( reader.TokenType != JsonTokenType.Number )
                        {
                            throw new JsonException ( ) ;
                        }

                        info.SequenceID = reader.GetInt32 ( ) ;
                        break ;
                    case "Properties" :
                        reader.Read ( ) ;
                        while ( reader.Read ( ) )
                        {
                            if ( reader.TokenType == JsonTokenType.EndObject )
                            {
                                break ;
                            }

                            if ( reader.TokenType != JsonTokenType.PropertyName )
                            {
                                throw new JsonException ( ) ;
                            }

                            var curKey = reader.GetString ( ) ;
                            reader.Read ( ) ;
                            object value = null ;
                            if ( reader.TokenType == JsonTokenType.String )
                            {
                                value = reader.GetString ( ) ;
                            }
                            else if ( reader.TokenType == JsonTokenType.StartObject )
                            {
                                reader.Read ( ) ;
                                if ( reader.TokenType != JsonTokenType.PropertyName )
                                {
                                    throw new JsonException ( ) ;
                                }

                                var key = reader.GetString ( ) ;
                                if ( key != "JsonConverter" )
                                {
                                    var dict = new Dictionary < string , JsonElement > ( ) ;
                                    var myVal =
                                        JsonSerializer.Deserialize < JsonElement > (
                                                                                    ref reader
                                                                                  , options
                                                                                   ) ;
                                    dict[ key ] = myVal ;
                                    while ( reader.Read ( ) )
                                    {
                                        if ( reader.TokenType == JsonTokenType.EndObject )
                                        {
                                            break ;
                                        }

                                        if ( reader.TokenType != JsonTokenType.PropertyName )
                                        {
                                            throw new JsonException ( ) ;
                                        }

                                        var key1 = reader.GetString ( ) ;
                                        var myVal2 =
                                            JsonSerializer.Deserialize<JsonElement>(
                                                                                    ref reader
                                                                                  , options
                                        
                                                                                  );
                                        dict[key1] = myVal2;
                                    }

                                    info.Properties[ curKey ] = dict ;
                                }
                                else
                                {
                                    reader.Read ( ) ;
                                    if ( reader.TokenType != JsonTokenType.True )
                                    {
                                        throw new JsonException ( ) ;
                                    }

                                    reader.Read ( ) ;
                                    if ( reader.TokenType != JsonTokenType.PropertyName )
                                    {
                                        throw new JsonException ( ) ;
                                    }

                                    var key2 = reader.GetString ( ) ;
                                    if ( key2 != "Type" )
                                    {
                                        throw new JsonException ( ) ;
                                    }

                                    reader.Read ( ) ;
                                    if ( reader.TokenType != JsonTokenType.String )
                                    {
                                        throw new JsonException ( ) ;
                                    }

                                    var typeName = reader.GetString ( ) ;
                                    var type = Type.GetType ( typeName ) ;

                                    reader.Read ( ) ;
                                    if ( reader.TokenType != JsonTokenType.PropertyName )
                                    {
                                        throw new JsonException ( ) ;
                                    }

                                    var key3 = reader.GetString ( ) ;
                                    if ( key3 != "Value" )
                                    {
                                        throw new JsonException ( ) ;
                                    }

                                    var o1 = JsonSerializer.Deserialize (
                                                                         ref reader
                                                                       , type
                                                                       , options
                                                                        ) ;

                                    reader.Read ( ) ;
                                    if ( reader.TokenType != JsonTokenType.EndObject )
                                    {
                                        throw new JsonException ( ) ;
                                    }

                                    value = o1 ;
                                    // reader.Read ( ) ;
                                    // if(reader.TokenType != JsonTokenType.EndObject)
                                        // throw new JsonException();
                                }
                            }

                            info.Properties[ curKey ] = value ;
                        }

                        break ;
                    case "CallerClassName":
                        reader.Read();
                        if (reader.TokenType != JsonTokenType.String && reader.TokenType != JsonTokenType.Null)
                            throw new JsonException();

                        info.CallerClassName = reader.GetString();
                        break;
                    case "CallerFilePath":
                        reader.Read();
                        if (reader.TokenType != JsonTokenType.String && reader.TokenType != JsonTokenType.Null)
                            throw new JsonException();

                        info.CallerFilePath = reader.GetString();
                        break;
                    case "CallerMemberName":
                        reader.Read();
                        if (reader.TokenType != JsonTokenType.String && reader.TokenType != JsonTokenType.Null)
                            throw new JsonException();

                        info.CallerMemberName = reader.GetString();
                        break;
                    case "CallerLineNumber": reader.Read ( ) ;
                        if ( reader.TokenType    != JsonTokenType.Number
                             && reader.TokenType != JsonTokenType.Null )
                        {
                            throw new JsonException();
                        }

                        if (  reader.TokenType != JsonTokenType.Null )
                        {
                            info.CallerLineNumber = reader.GetInt32 ( ) ;
                        }

                        break ;
                    case "ProcessId":
                        reader.Read ( ) ;
                        if(reader.TokenType != JsonTokenType.Number)
                            throw new JsonException();
                        info.ProcessId = reader.GetInt32 ( ) ;

                        break ;
                    case "ManagedThreadId": reader.Read ( ) ;
                        if ( reader.TokenType != JsonTokenType.Number )
                        {
                            throw new JsonException();
                        }

                        info.ManagedThreadId = reader.GetInt32 ( ) ;
                        break ;
                    case "ThreadName": reader.Read ( ) ;
                        if ( reader.TokenType != JsonTokenType.String )
                        {
                            throw new JsonException();
                        }

                        info.ThreadName = reader.GetString ( ) ;
                        break ;
                    case "CurrentTaskId":
                        reader.Read ( ) ;
                        if (reader.TokenType != JsonTokenType.Number && reader.TokenType != JsonTokenType.Null)
                        {
                            throw new JsonException();
                        }

                        info.CurrentTaskId = reader.GetInt32 ( ) ;
                        break ;
                    case "TimeStamp": reader.Read ( ) ;
                        var v  =
                            JsonSerializer.Deserialize<DateTime>(
                                                                    ref reader
                                                                  , options
                                                                   );
                        info.TimeStamp = v ;
                        break ;


                    /*
                     * {"Level":1,"SequenceID":69,"LoggerName":"test","CallerClassName":null,"ProcessId":5944,
                     * "ManagedThreadId":15,"ThreadName":"Thread22",
                     * "CurrentTaskId":1,
                     * "Message":"test",
                     * "TimeStamp":"2020-03-19T07:30:58.2107828-07:00","FormattedMessage":"test","Properties":{"node":{"JsonConverter":true,"Type":"Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax, Microsoft.CodeAnalysis.CSharp, Version=3.4.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35","Value":{"Usings":[{"RawKind":8843,"Kind":"UsingDirective","Alias":null,"Name":{"RawKind":8508,"Kind":"IdentifierToken","Value":"System"}},{"RawKind":8843,"Kind":"UsingDirective","Alias":null,"Name":{"RawKind":8617,"Kind":"QualifiedName","Left":{"RawKind":8617,"Kind":"QualifiedName","Left":{"RawKind":8508,"Kind":"IdentifierToken","Value":"System"},"Right":{"RawKind":8508,"Kind":"IdentifierToken","Value":"Collections"}},"Right":{"RawKind":8508,"Kind":"IdentifierToken","Value":"Generic"}}},{"RawKind":8843,"Kind":"UsingDirective","Alias":null,"Name":{"RawKind":8617,"Kind":"QualifiedName","Left":{"RawKind":8508,"Kind":"IdentifierToken","Value":"System"},"Right":{"RawKind":8508,"Kind":"IdentifierToken","Value":"Linq"}}},{"RawKind":8843,"Kind":"UsingDirective","Alias":null,"Name":{"RawKind":8617,"Kind":"QualifiedName","Left":{"RawKind":8508,"Kind":"IdentifierToken","Value":"System"},"Right":{"RawKind":8508,"Kind":"IdentifierToken","Value":"Text"}}},{"RawKind":8843,"Kind":"UsingDirective","Alias":null,"Name":{"RawKind":8617,"Kind":"QualifiedName","Left":{"RawKind":8617,"Kind":"QualifiedName","Left":{"RawKind":8508,"Kind":"IdentifierToken","Value":"System"},"Right":{"RawKind":8508,"Kind":"IdentifierToken","Value":"Threading"}},"Right":{"RawKind":8508,"Kind":"IdentifierToken","Value":"Tasks"}}},{"RawKind":8843,"Kind":"UsingDirective","Alias":null,"Name":{"RawKind":8508,"Kind":"IdentifierToken","Value":"NLog"}}],"ExternAliases":[],"AttributeLists":[],"Members":[{"RawKind":8842,"Kind":"NamespaceDeclaration","Members":[{"Identifier":{"Kind":"IdentifierToken","RawKind":8508,"Value":"Program"},"Members":[{"RawKind":8873},{"Statements":["Action\u003Cstring\u003E xx = Logger.Info;","xx(\u0022hi\u0022);","Logger.Debug ( $\u0022Hello {1}\u0022 ) ;","try {\r\n                string xxx = null;\r\n                var q = xxx.ToString();\r\n            } catch(Exception ex) {\r\n                Logger.Info(ex, ex.Message);\r\n            }","var x = Logger;","x.Info(\u0022hello {test} {ab}\u0022, 123, 45);"]}]}]}]}}},"GDC":{},"MDLC":{}}
                     */
                    case "GDC":

                        var gdc =
                            JsonSerializer.Deserialize < Dictionary < string , object > > (
                                                                                           ref
                                                                                           reader
                                                                                         , options
                                                                                          ) ;
                        foreach ( var keyValuePair in gdc )
                        {
                            info.GDC[ keyValuePair.Key ] = gdc.Values ;
                        }

                        break ;
                    case "MDLC":

                        var mdlc =
                            JsonSerializer.Deserialize<Dictionary<string, object>>(
                                                                                   ref
                                                                                   reader
                                                                                 , options
                                                                                  );
                        foreach (var keyValuePair in mdlc)
                        {
                            info.MDLC[keyValuePair.Key] = mdlc.Values;
                        }

                        break;

                }
            }

            var logEventInfo = new LogEventInfo ( ) ;
            logEventInfo.Properties[ "LogEventInstance" ] = info ;
            return logEventInfo ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write (
            Utf8JsonWriter        writer
          , LogEventInfo          value
          , JsonSerializerOptions options
        )
        {
            if ( writer == null )
            {
                return ;
            }

            writer.WriteStartObject ( ) ;
            if ( value == null ) { }
            else
            {
                writer.WriteNumber ( "Level" ,      value.Level.Ordinal ) ;
                writer.WriteNumber ( "SequenceID" , value.SequenceID ) ;
                writer.WriteString ( "LoggerName" ,      value.LoggerName ) ;
                writer.WriteString ( "CallerClassName" , value.CallerClassName ) ;
                writer.WriteNumber ( "ProcessId" , Process.GetCurrentProcess ( ).Id ) ;
                if ( value.CallerFilePath != null )
                {
                    writer.WriteString ( "CallerFilePath" , value.CallerFilePath ) ;
                    writer.WriteNumber ( "CallerLineNumber" , value.CallerLineNumber ) ;
                }

                if ( value.CallerMemberName != null )
                {
                    writer.WriteString ( "CallerMemberName" , value.CallerMemberName ) ;
                }

                if ( value.Exception != null )
                {
                    writer.WriteString ( "ExceptionString" , value.Exception.ToString ( ) ) ;
                }

                writer.WriteNumber ( "ManagedThreadId" , Thread.CurrentThread.ManagedThreadId ) ;
                if ( Thread.CurrentThread.Name == null )
                {
                    Thread.CurrentThread.Name = "Thread" + value.SequenceID ;
                }

                writer.WriteString ( "ThreadName" , Thread.CurrentThread.Name ) ;
                if ( Task.CurrentId.HasValue )
                {
                    writer.WriteNumber ( "CurrentTaskId" , Task.CurrentId.Value ) ;
                }

                writer.WriteString ( "Message" ,          value.Message ) ;
                writer.WriteString ( "TimeStamp" ,        value.TimeStamp ) ;
                writer.WriteString ( "FormattedMessage" , value.FormattedMessage ) ;
                if ( value.HasProperties )
                {
                    writer.WriteStartObject ( "Properties" ) ;
                    foreach ( var p in value.Properties )
                    {
                        writer.WritePropertyName ( p.Key.ToString ( ) ) ;
                        if ( p.Value is Type t )
                        {
                            JsonSerializer.Serialize ( writer , t , typeof ( Type ) , options ) ;
                        }
                        else
                        {
                            try
                            {
                                JsonSerializer.Serialize ( writer , p.Value , options ) ;
                            }
                            catch ( Exception ex )
                            {
                                Debug.WriteLine ( $"{p.Key}: {p.Value}: {ex}" ) ;
                                throw ;
                            }
                        }
                    }

                    writer.WriteEndObject ( ) ;
                }
            }

            writer.WriteStartObject ( "GDC" ) ;
            foreach ( var name in GlobalDiagnosticsContext.GetNames ( ) )
            {
                writer.WritePropertyName ( name ) ;
                var value1 = GlobalDiagnosticsContext.GetObject ( name ) ;
                if ( value1 is Type t )
                {
                    JsonSerializer.Serialize ( writer , t , typeof ( Type ) , options ) ;
                }
                else
                {
                    try
                    {
                        JsonSerializer.Serialize ( writer , value1 , options ) ;
                    }
                    catch ( Exception ex )
                    {
                        Debug.WriteLine ( $"{name}: {value1}: {ex}" ) ;
                        throw ;
                    }
                }
            }

            writer.WriteEndObject ( ) ;

            writer.WriteStartObject ( "MDLC" ) ;

            foreach ( var name in MappedDiagnosticsLogicalContext.GetNames ( ) )
            {
                writer.WritePropertyName ( name ) ;
                var value1 = GlobalDiagnosticsContext.GetObject ( name ) ;
                if ( value1 is Type t )
                {
                    JsonSerializer.Serialize ( writer , t , typeof ( Type ) , options ) ;
                }
                else
                {
                    try
                    {
                        JsonSerializer.Serialize ( writer , value1 , options ) ;
                    }
                    catch ( Exception ex )
                    {
                        Debug.WriteLine ( $"{name}: {value1}: {ex}" ) ;
                        throw ;
                    }
                }
            }

            writer.WriteEndObject ( ) ;

            writer.WriteEndObject ( ) ;
        }
        #endregion
    }
}
