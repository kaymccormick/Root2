using System.ComponentModel ;
using AnalysisAppLib.Syntax ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace AnalysisAppLib
{
    internal class LogInvocationArgument : ILogInvocationArgument
    {
        [ DesignerSerializationVisibility ( DesignerSerializationVisibility.Hidden ) ]
        private readonly ArgumentSyntax _syntax ;

        private string jSON ;

        public LogInvocationArgument ( ArgumentSyntax syntax )
        {
            _syntax = syntax ;
            var jsonOut = Transforms.TransformExpr ( syntax.Expression ) ;
            Pojo = jsonOut ;
        }


        public LogInvocationArgument ( ) { }

        public string GetJSON ( ILogInvocationArgument arg ) { return jSON ; }

        public void SetJSON ( string value ) { jSON = value ; }

        public object Pojo { get ; set ; }
    }
}