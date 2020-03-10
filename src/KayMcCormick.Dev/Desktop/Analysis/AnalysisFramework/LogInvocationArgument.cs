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
using System ;
using System.ComponentModel ;
using System.Text.Json ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;


namespace AnalysisFramework
{
    public interface ILogInvocationArgument
    {
        [JsonIgnore]
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
            JSON = JsonSerializer.Serialize(jsonOut);
            Pojo = jsonOut;
        }

        
        public LogInvocationArgument ( ) {
        }

        public LogInvocationArgument ( [ NotNull ] ILogInvocation debugInvo , [ NotNull ] ArgumentSyntax syntax ) : this(syntax)
        {
            _syntax    = syntax ?? throw new ArgumentNullException ( nameof ( syntax ) ) ;


        }
        [JsonIgnore]
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public string JSON { get ; set ; }

        public object Pojo { get ; set ; }

        public override string ToString ( ) { return $"{nameof ( JSON )}: {JSON}" ; }
    }
}