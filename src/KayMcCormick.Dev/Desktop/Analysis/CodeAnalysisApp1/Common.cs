using System ;
using System.Collections.Generic ;
using System.Linq ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace CodeAnalysisApp1
{
    internal static class Common
    {
        // ReSharper disable twice RedundantNameQualifier
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger ( ) ;
        public static List < StatementSyntax > Query1 ( SyntaxNode root , SemanticModel model )
        {
            var comp = model.Compilation ;
            var t1 = comp.GetTypeByMetadataName("NLog.Logger");
            var namespaceMembers = comp.GlobalNamespace.GetNamespaceMembers ( ) ;
            var ns = namespaceMembers
                    .Select ( symbol => symbol.MetadataName == "NLog" )
                    .FirstOrDefault ( ) ;
#pragma warning disable CS0472 // The result of the expression is always 'false' since a value of type 'bool' is never equal to 'null' of type 'bool?'
            if ( ns == null )
#pragma warning restore CS0472 // The result of the expression is always 'false' since a value of type 'bool' is never equal to 'null' of type 'bool?'
            {
                Logger.Info("{nsMembers}", string.Join(", ", namespaceMembers.Select(symbol => symbol.Name)));
                throw new InvalidOperationException("no NLOG namespace");
            }
            if ( t1 == null )
            {
                throw new InvalidOperationException ( "No NLog.Logger" ) ;
            }
            var t2 = comp.GetTypeByMetadataName("NLog.ILogger");
            var methodSymbols = t1.GetMembers ( )
                                  .Concat ( t2.GetMembers ( ) )
                                  .Where ( symbol => symbol.Kind == SymbolKind.Method )
                                  .Select ( symbol => ( IMethodSymbol ) symbol )
                                  .Where ( symbol => symbol.MethodKind == MethodKind.Ordinary )
                                  .ToList ( ) ;
            // foreach ( IMethodSymbol method in methodSymbols)
            // {
            //     var x = Transforms.TransformMethodSymbol ( method ) ;
            //     new LogBuilder ( Logger )
            //        .Message ( "Method" )
            //        //.Properties ( x.ToDictionary ( ) )
            //        .Level ( LogLevel.Debug )
            //        .Write ( ) ;
            // }

            
            // Logger.Info("{t1}", t1);
            // Logger.Info("{t2}", t2);
            // foreach ( var s in comp.SyntaxTrees )
            // {
            //     Logger.Info ( "{count} {path}" , s.Length , s.FilePath ) ;
            // }
            // foreach ( var extRef in comp.ExternalReferences )
            // {`
            //     var f = Path.GetFileName ( extRef.Display ) ;
            //     Logger.Info (
            //                  "{f} {compilationExternalReference_Display}"
            //                , f, extRef.Display
            //                 ) ;
            // }
            
            var query1 = root.DescendantNodesAndSelf ( )
                            .Where (
                                    ( syntaxNode , i )
                                        => model.GetTypeInfo ( syntaxNode )
                                                .Type?.ContainingAssembly?.Identity?.Name
                                           == "NLog"
                                   );

                var tempq = query1
                            .Select (
                                     ( node , i )
                                         => node.AncestorsAndSelf ( )
                                                .OfType < StatementSyntax > ( )
                                                .FirstOrDefault ( )).ToList();
                var query = tempq ;
            // Logger.Warn ( "{} {}" , query.Count , query.Distinct ( ).Count ( ) ) ;

            foreach ( var statementSyntax in query )
            {
                Logger.Debug ( "{statementSyntax}" , statementSyntax ) ;
            }

                var q2 = query
                                    
                            .Distinct ( )
                            .Where ( ( syntax , i ) => syntax != null )
                            .Select (
                                     syntax => syntax.DescendantNodesAndSelf ( )
                                                     .Select (
                                                              node => Tuple.Create (
                                                                                    node
                                                                                  , model
                                                                                       .GetSymbolInfo (
                                                                                                       node
                                                                                                      )
                                                                                   ))
                                                             
                                                     .Where (
                                                             tuple => tuple.Item2.Symbol != null
                                                             /*|| tuple.Item2.CandidateSymbols
                                                                     .Any ( )*/
               )
        .Where (
                tuple => tuple.Item2.Symbol != null
                         && ( new[]
                              {
                                  "ILogger" , "Logger"
                              }.Contains (
                                          tuple
                                             .Item2
                                             .Symbol
                                             .ContainingType
                                             .Name
                                         )
                              && tuple.Item2.Symbol
                                      .Kind
                              == SymbolKind.Method )
               )
        .Select (
                 tuple => Tuple.Create (
                                        tuple
                                           .Item1
                                           .AncestorsAndSelf ( )
                                           .OfType <
                                                InvocationExpressionSyntax
                                            > ( )
                                           .FirstOrDefault ( )
                                      , ( IMethodSymbol
                                        ) tuple
                                         .Item2.Symbol
                                       )
                )
).ToList();

return query ;
        }
    }
}