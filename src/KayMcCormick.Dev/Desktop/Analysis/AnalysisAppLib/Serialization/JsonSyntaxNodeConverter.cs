#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// JsonSyntaxNodeConverter.cs
// 
// 2020-03-19-7:55 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Reflection ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using FindLogUsages ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;

namespace AnalysisAppLib.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    public class JsonSyntaxNodeConverter : JsonConverterFactory
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Overrides of JsonConverter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <returns></returns>
        public override bool CanConvert ( Type typeToConvert )
        {
            return typeof ( CSharpSyntaxNode ).IsAssignableFrom ( typeToConvert ) ;
        }
        #endregion
        #region Overrides of JsonConverterFactory
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override JsonConverter CreateConverter (
            Type                  typeToConvert
          , JsonSerializerOptions options
        )
        {
            return ( JsonConverter ) Activator.CreateInstance (
                                                               typeof ( InnerConverter <> )
                                                                  .MakeGenericType ( typeToConvert )
                                                             , BindingFlags.Instance
                                                               | BindingFlags.Public
                                                             , null
                                                             , new object[] { options }
                                                             , null
                                                              ) ;
        }
        #endregion

        private class InnerConverter < T > : JsonConverter < T >
            where T : CSharpSyntaxNode
        {
            public InnerConverter ( JsonSerializerOptions options ) { }

            #region Overrides of JsonConverter<T>
            public override T Read (
                ref Utf8JsonReader    reader
              , Type                  typeToConvert
              , JsonSerializerOptions options
            )
            {
                if ( typeToConvert == typeof ( ThisExpressionSyntax ) )
                {
                    var d =
                        JsonSerializer.Deserialize < Dictionary < string , JsonElement > > (
                                                                                            ref
                                                                                            reader
                                                                                          , options
                                                                                           ) ;
                    return ( T ) ( CSharpSyntaxNode ) SyntaxFactory.ThisExpression ( ) ;
                }

                if ( typeToConvert == typeof ( CompilationUnitSyntax ) )
                {
                    var d = JsonSerializer.Deserialize < PojoCompilationUnit > (
                                                                                ref reader
                                                                              , options
                                                                               ) ;

                    if ( d?.ExternAliases != null )
                    {
                        foreach ( var dExternAliase in d.ExternAliases )
                        {
                            Logger.Info ( "{x}" , dExternAliase ) ;
                        }
                    }

                    if ( d            != null
                         && d.Members != null )
                    {
                        foreach ( var xx in d.Members )
                        {
                            Logger.Info ( "{x}" , xx ) ;
                        }
                    }

                    return ( T ) ( CSharpSyntaxNode ) SyntaxFactory.CompilationUnit ( ) ;
                }

                return null ;
            }

            public override void Write (
                Utf8JsonWriter        writer
              , T                     value
              , JsonSerializerOptions options
            )
            {
                writer.WriteStartObject ( ) ;
                writer.WriteBoolean ( "JsonConverter" , true ) ;
                writer.WriteString ( "Type" , value.GetType ( ).AssemblyQualifiedName ) ;
                writer.WritePropertyName ( "Value" ) ;
                // MemoryStream s = new MemoryStream();
                // value.SerializeTo(s);
                // writer.WriteBase64StringValue(s.GetBuffer());
                var transformed = GenTransforms.Transform_CSharp_Node(value);
                JsonSerializer.Serialize ( writer , transformed , options ) ;
                writer.WriteEndObject ( ) ;
            }
            #endregion
        }
    }
}