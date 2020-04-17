using JetBrains.Annotations ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace FindLogUsages
{
    internal sealed class LogInvocationArgument : ILogInvocationArgument
    {
        private string jSON ;

        public LogInvocationArgument ( [ NotNull ] ArgumentSyntax syntax )
        {
            var jsonOut = GenTransforms.Transform_Expression( syntax.Expression ) ;
            Pojo = jsonOut ;
        }


        // ReSharper disable once UnusedMember.Global
        public LogInvocationArgument ( ) { }

        public string GetJSON ( ILogInvocationArgument arg ) { return jSON ; }

        public object Pojo { get ; set ; }
    }
}