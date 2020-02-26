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
using CodeAnalysisApp1 ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Newtonsoft.Json ;

namespace ProjLib
{
    public class LogInvocationArgument
    {
        private readonly LogInvocation  _debugInvo ;
        private readonly ArgumentSyntax _syntax ;

        public LogInvocationArgument ( LogInvocation debugInvo , ArgumentSyntax syntax )
        {
            _debugInvo = debugInvo ;
            _syntax    = syntax ;
            var jsonOut = Transforms.TransformExpr ( syntax.Expression ) ;
            JSON = JsonConvert.SerializeObject ( jsonOut ) ;
            Pojo = jsonOut ;


        }

        public string JSON { get ; set ; }

        public object Pojo { get ; set ; }
    }
}