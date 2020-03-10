using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Threading ;
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
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public static async Task < IEnumerable < ILogInvocation > > FindUsagesFunc ( Document d )
        {
            using ( MappedDiagnosticsLogicalContext.SetScoped ( "Document" , d.FilePath ) )
            {
                try
                {
                    Logger.Trace (
                                  "[{id}] Entering {funcName}"
                                , Thread.CurrentThread.ManagedThreadId
                                , nameof ( FindUsagesFunc )
                                 ) ;
                    var tree = await d.GetSyntaxTreeAsync ( ).ConfigureAwait ( true ) ;
                    var root = tree.GetCompilationUnitRoot ( ) ;
                    var model = await d.GetSemanticModelAsync ( ).ConfigureAwait ( true ) ;

                    var exceptionType =
                        model.Compilation.GetTypeByMetadataName ( "System.Exception" ) ;
                    var t = LogUsages.GetNLogSymbol ( model ) ;
                    if ( t == null )
                    {
                        return Array.Empty < ILogInvocation > ( ) ;
                        throw new MissingTypeException ( "nlog" ) ;
                    }

                    var t2 = LogUsages.GetILoggerSymbol ( model ) ;
                    if ( t2 == null )
                    {
                        return Array.Empty < ILogInvocation > ( ) ;
                        throw new MissingTypeException ( "nlog" ) ;
                    }

                    var logBuilderSymbol = LogUsages.GetLogBuilderSymbol ( model ) ;
                    var rootNode = await tree.GetRootAsync ( ).ConfigureAwait ( true ) ;
                    return
                        from node in root.DescendantNodes ( )//.AsParallel ( )
                        let t_ = t
                        let t2_ = t2
                        let builderSymbol = logBuilderSymbol
                        let tree_ = tree
                        let model_ = model
                        let exType = exceptionType
                        where node.RawKind == ( ushort ) SyntaxKind.InvocationExpression || node.RawKind== (ushort)SyntaxKind.ObjectCreationExpression
                        let @out =
                            LogUsages.CheckInvocationExpression ( node
                                                                , model_
                                                                , builderSymbol
                                                               /*, t_
                                                               , t2_*/
                                                                )
                        where @out.Item1
                        let statement = node.AncestorsAndSelf ( ).Where ( Predicate ).First ( )
                        select new InvocationParms (
                                                    new CodeSource ( tree_.FilePath )
                                                  , tree_
                                                  , model_
                                                  , statement
                                                  , @out
                                                  , exType
                                                   ).ProcessInvocation ( ) ;
                }
                catch ( Exception ex )
                {
                    Logger.Debug ( ex , ex.ToString ( ) ) ;
                    throw ;
                }
            }
        }

        private static bool Predicate ( SyntaxNode arg1 , int arg2 )
        {
            var b = arg1 is StatementSyntax || arg1 is MemberDeclarationSyntax ;
#if TRACE
            Logger.Debug("Got {arg1}- {b}", arg1.Kind(), b);
#endif
            if ( b )
            {
                return true ;
            }

            return false ;
        }
    }
}