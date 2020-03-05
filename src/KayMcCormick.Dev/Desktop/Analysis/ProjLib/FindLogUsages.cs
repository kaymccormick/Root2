using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Threading.Tasks ;
using AnalysisFramework ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;

namespace ProjLib
{
    internal static class FindLogUsages
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public static async Task < IEnumerable < ILogInvocation > > FindUsagesFunc ( Document d )
        {
            try
            {
                var tree = await d.GetSyntaxTreeAsync ( ).ConfigureAwait ( true ) ;
                var root = tree.GetCompilationUnitRoot ( ) ;
                var model = await d.GetSemanticModelAsync ( ).ConfigureAwait ( true ) ;

                var exceptionType = model.Compilation.GetTypeByMetadataName ( "System.Exception" ) ;

                var rootNode = await tree.GetRootAsync ( ).ConfigureAwait ( true ) ;
                return
                    from node in root.DescendantNodes ( )
                    where node.RawKind == ( ushort ) SyntaxKind.InvocationExpression
                    let @out =
                        LogUsages.CheckInvocationExpression (
                                                             ( InvocationExpressionSyntax ) node
                                                           , model
                                                            )
                    let statement =
                        node.AncestorsAndSelf ( ).OfType < StatementSyntax > ( ).First ( )
                    select InvocationParms.ProcessInvocation (
                                                              new InvocationParms (
                                                                                   new CodeSource (
                                                                                                   ""
                                                                                                  )
                                                                                 , tree
                                                                                 , model
                                                                                 , statement
                                                                                 , @out
                                                                                 , exceptionType
                                                                                  )
                                                             ) ;
            }
            catch ( Exception ex )
            {
                Logger.Error ( ex , ex.ToString ( ) ) ;
                return null ;
            }

            return null ;
        }
    }
}