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
using CodeAnalysisApp1 ;
using Microsoft.CodeAnalysis ;
using Newtonsoft.Json ;
using NLog ;

namespace ProjLib
{
    public class Visitor2Impl : Visitor2
    {
        #region Overrides of Visitor2
        public override void VisitToken ( SyntaxToken token )
        {
            base.VisitToken ( token ) ;

        }

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
        #endregion

        public Visitor2Impl ( ) : base ( null )
{

}
    }
}