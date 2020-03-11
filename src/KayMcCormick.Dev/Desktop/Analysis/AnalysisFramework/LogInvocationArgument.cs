#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// LogInvocationArgument.cs
// 
// 2020-02-25-10:17 PM
// 
// ---
#endregion
using System.ComponentModel ;
#if !NETSTANDARD2_0
using System.Text.Json ;
using System.Text.Json.Serialization ;
#endif
using Microsoft.CodeAnalysis.CSharp.Syntax ;


namespace AnalysisFramework
{
    public interface ILogInvocationArgument
    {
#if !NETSTANDARD2_0
        [JsonIgnore]
#endif
        string JSON { get ; set ; }

        object Pojo { get ; set ; }
    }

    internal class LogInvocationArgument : ILogInvocationArgument
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        private readonly ArgumentSyntax _syntax ;

        public LogInvocationArgument ( ArgumentSyntax syntax )
        {
            _syntax = syntax ;
            var jsonOut = Transforms.TransformExpr(syntax.Expression);
#if !NETSTANDARD2_0
            JSON = JsonSerializer.Serialize(jsonOut);
#endif
            Pojo = jsonOut;
        }

        
        public LogInvocationArgument ( ) {
        }

#if !NETSTANDARD2_0
        [JsonIgnore]
#endif
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public string JSON { get ; set ; }

        public object Pojo { get ; set ; }

        public override string ToString ( ) { return $"{nameof ( JSON )}: {JSON}" ; }
    }
}