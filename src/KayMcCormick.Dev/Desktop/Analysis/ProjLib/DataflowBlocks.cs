using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using AnalysisFramework ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;
using ProjLib ;

internal static class DataflowBlocks
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

    public static TransformManyBlock < Workspace , Document > SolutionDocumentsBlock (
 
        //ITargetBlock < Document > takeDocument
    )
    {
        return new TransformManyBlock < Workspace , Document > (
                                                                workspace => workspace
                                                                            .CurrentSolution
                                                                            .Projects.SelectMany (
                                                                                                  project
                                                                                                      => project
                                                                                                         .Documents
                                                                                                 )
                                                               ) ;
    }

    public static TransformBlock < BuildResults , Workspace > WorkspaceBlock ( )
    {
        var makeWs =
            new TransformBlock < BuildResults , Workspace > (
                                                             results => ProjLibUtils
                                                                .MakeWorkspaceAsync ( results )
                                                            ) ;
        return makeWs ;
    }

    public static TransformManyBlock < Document , ILogInvocation > FindLogUsagesBlock ( )
    {
        var findLogUsagesBlock = new TransformManyBlock < Document , ILogInvocation > (
                                                                                       FindUsagesFunc
                                                                                      ) ;
        return findLogUsagesBlock ;
    }

    private static async Task < IEnumerable < ILogInvocation > > FindUsagesFunc ( Document d )
    {
        try
        {
            var tree = await d.GetSyntaxTreeAsync ( ).ConfigureAwait ( true ) ;
            var root = tree.GetCompilationUnitRoot ( ) ;
            var model = await d.GetSemanticModelAsync ( ) ;
            var exceptionType = model.Compilation.GetTypeByMetadataName ( "System.Exception" ) ;
            var logInvocations = tree.GetRoot ( )
                                     .DescendantNodes ( )
                                     .OfType < InvocationExpressionSyntax > ( )
                                     .AsParallel ( )
                                     .Select (
                                              node => {
                                                  var match = LogUsages.CheckInvocationExpression ( node , out var methodSymbol , model ) ;
                                                  return Tuple.Create ( node , match , methodSymbol ) ;
                                              }
                                             )
                                     .Where ( tuple => tuple.Item2 )
                                     .Select (
                                              tuple => {
                                                  try
                                                  {
                                                      return LogUsages.ProcessInvocation (
                                                                                          new InvocationParms (
                                                                                                               root
                                                                                                             , model
                                                                                                             , new CodeSource ( "" )
                                                                                                             , tuple.Item1.AncestorsAndSelf ( ).OfType < StatementSyntax > ( ).First ( )
                                                                                                             , tuple.Item1
                                                                                                             , tuple.Item3
                                                                                                             , exceptionType
                                                                                                             , null
                                                                                                             , null
                                                                                                              )
                                                                                         ) ;
                                                  }
                                                  catch ( Exception ex )
                                                  {
                                                      Logger.Error ( ex , ex.ToString ( ) ) ;
                                                      return null ;
                                                  }
                                              }
                                             )
                                     .Where ( invocation => invocation != null ) ;
            return logInvocations ;
        }
        catch ( Exception ex )
        {
            Logger.Error ( ex , ex.ToString ( ) ) ;
            return Array.Empty < ILogInvocation > ( ) ;
        }
    }

    public static TransformBlock < string , BuildResults > BuildBlock ( )
    {
        var buildTransformBlock =
            new TransformBlock < string , BuildResults > (
                                                          s => ProjLibUtils.BuildRepository ( s )
                                                         ) ;
        return buildTransformBlock ;
    }
}