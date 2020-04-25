using JetBrains.Annotations ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace FindLogUsages
{
    internal sealed class LogInvocationArgument : ILogInvocationArgument
    {
#pragma warning disable 649
        private string _jSon ;
#pragma warning restore 649

        public LogInvocationArgument ( [ NotNull ] ArgumentSyntax syntax )
        {
            var jsonOut = GenTransforms.Transform_Expression( syntax.Expression ) ;
            Pojo = jsonOut ;
        }


        // ReSharper disable once UnusedMember.Global
        public LogInvocationArgument ( ) { }

        public string GetJson ( ILogInvocationArgument arg ) { return _jSon ; }

        public object Pojo { get ; set ; }
    }
}