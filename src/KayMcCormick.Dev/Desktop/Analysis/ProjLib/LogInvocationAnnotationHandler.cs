#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// Visitor2Impl.cs
// 
// 2020-02-27-12:36 AM
// 
// ---
#endregion
using System.Linq ;
using Microsoft.CodeAnalysis ;
using NLog ;

namespace ProjLib
{

#if false
    public class LogInvocationAnnotationHandler : Visitor2
    {

        public override void DefaultVisit ( SyntaxNode node )
        {
            var logInvAnno_ = node.GetAnnotations("LogInvocation");
            SyntaxAnnotation logInvAnno = null;
            if ( logInvAnno_.Any ( ) )

            {
                logInvAnno = logInvAnno_.First ( ) ;
                var inv = JsonConvert.DeserializeObject < LogInvocation > ( logInvAnno.Data ) ;
                if ( inv != null )
                {
                    inv.CurrentModel = Model ;
                    inv.MethodSymbol = Enumerable.FirstOrDefault < IMethodSymbol > (
                                                                                    Model
                                                                                       .Compilation
                                                                                       .GetTypeByMetadataName (
                                                                                                               inv
                                                                                                                  .LoggerType
                                                                                                              )
                                                                                       .GetMembers (
                                                                                                    inv
                                                                                                       .MethodName
                                                                                                   )
                                                                                       .OfType <
                                                                                            IMethodSymbol
                                                                                        > ( )
                                                                                   ) ;
                    ProjLib.ActiveSpans[ logInvAnno ] = new LogInvocationSpan ( node.Span, inv ) ;
                }
                else
                {
                    LogManager.GetCurrentClassLogger ( ).Warn ( "null deserialziation" ) ;
                }
            }

            base.DefaultVisit ( node ) ;

            if (logInvAnno != null)
            {
                ProjLib.ActiveSpans.Remove(logInvAnno);
            }

        }


        public LogInvocationAnnotationHandler ( ) : base ( null )
{

}

        public object logInvAnno { get { return _logInvAnno ; } set { _logInvAnno = value ; } }

    }
#endif
}