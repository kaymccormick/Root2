using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Threading ;
using System.Threading.Tasks ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;

namespace AnalysisAppLib
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
                    #if TRACE
                    Logger.Trace (
                                  "[{id}] Entering {funcName}"
                                , Thread.CurrentThread.ManagedThreadId
                                , nameof ( FindUsagesFunc )
                                 ) ;
#endif
                    var tree = await d.GetSyntaxTreeAsync ( ).ConfigureAwait ( true ) ;
                    var root = (tree ?? throw new InvalidOperationException ( )).GetCompilationUnitRoot ( ) ;
                    var model = await d.GetSemanticModelAsync ( ).ConfigureAwait ( true ) ;

                    if ( model != null )
                    {
                        var exceptionType =
                            model.Compilation.GetTypeByMetadataName ( "System.Exception" ) ;
                        var t = LogUsages.GetNLogSymbol ( model ) ;
                        if ( t == null )
                        {
                            return Array.Empty < ILogInvocation > ( ) ;
#pragma warning disable CA1303 // Do not pass literals as localized parameters
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                        }

                        var t2 = LogUsages.GetILoggerSymbol ( model ) ;
                        if ( t2 == null )
                        {
                            return Array.Empty < ILogInvocation > ( ) ;
#pragma warning disable CA1303 // Do not pass literals as localized parameters
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                        }

                        var logBuilderSymbol = LogUsages.GetLogBuilderSymbol ( model ) ;
                        var rootNode = await tree.GetRootAsync ( ).ConfigureAwait ( true ) ;
                        return
                            from node in root.DescendantNodes ( ) //.AsParallel ( )
                            let t_ = t
                            let t2_ = t2
                            let builderSymbol = logBuilderSymbol
                            let tree_ = tree
                            let model_ = model
                            let exType = exceptionType
                            where node.RawKind == ( ushort ) SyntaxKind.InvocationExpression || node.RawKind == (ushort)SyntaxKind.ObjectCreationExpression
                            let @out =
                                LogUsages.CheckInvocationExpression ( node
                                                                    , model_
                                                                    , builderSymbol
                                                                    , t_
                                                                    , t2_
                                                                    )
                            where @out.Item1
                            let statement = Enumerable.Where < SyntaxNode > ( node.AncestorsAndSelf ( ) , Predicate ).First ( )
                            select new InvocationParams (
                                                         tree_
                                                       , model_
                                                       , statement
                                                       , @out
                                                       , exType
                                                        ).ProcessInvocation ( ) ;
                    }
                }
                catch ( Exception ex )
                {
                    Logger.Debug ( ex , ex.ToString ( ) ) ;
                    throw ;
                }
            }

            throw new InvalidOperationException ( ) ;
        }

        private static bool Predicate ( SyntaxNode arg1 , int arg2 )
        {
            var b = arg1 is StatementSyntax || arg1 is MemberDeclarationSyntax ;
#if TRACE
            Logger.Debug("Got {arg1} - {b}", arg1.Kind(), b);
#endif
            if ( b )
            {
                return true ;
            }

            return false ;
        }

        private static class LogUsages
        {
            private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

            public const string LogBuilderClassName = "LogBuilder";
            public const string LogBuilderNamespaceName = NLogNamespace + ".Fluent";

            public const string LogBuilderClassFullName =
                LogBuilderNamespaceName + "." + LogBuilderClassName;

            public const string ILoggerClassName = "ILogger";
            public const string LoggerClassName = "Logger";
            private static readonly string LoggerClassFullName = NLogNamespace + '.' + LoggerClassName;

            public const string ILoggerClassFullName = NLogNamespace + "." + ILoggerClassName;

            private const string NLogNamespace = "NLog";


            // ReSharper disable once SuggestBaseTypeForParameter
            private static bool CheckSymbol(IMethodSymbol methSym, params INamedTypeSymbol[] t1)
            {
                var cType = methSym.ContainingType;
                return CheckTypeSymbol(cType, t1);
            }

            private static bool CheckTypeSymbol(
                // ReSharper disable once SuggestBaseTypeForParameter
                INamedTypeSymbol cType
              , params INamedTypeSymbol[] t1
            )
            {
                var r = t1.Any(symbol => SymbolEqualityComparer.Default.Equals(cType, symbol));
                return r;
            }


            public static Tuple<bool, IMethodSymbol, SyntaxNode> CheckInvocationExpression(
                SyntaxNode n1
              , SemanticModel currentModel
              , params INamedTypeSymbol[] t
            )
            {
                try
                {
                    if (n1 is InvocationExpressionSyntax node)
                    {
                        var symbolInfo = currentModel.GetSymbolInfo(node.Expression);
#if TRACE
                        Logger.Debug(
                                      "{method} node location is {node}"
                                    , nameof(CheckInvocationExpression)
                                    , node.GetLocation().ToString()
                                     );
                        Logger.Debug(
                                      "{exprKind}, {expr}"
                                    , node.Expression.Kind()
                                    , node.Expression.ToString()
                                     );

                        Logger.Info(
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                                 "symbolinfo is {node}"
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                               , symbolInfo.Symbol?.ToDisplayString() ?? "null"
                                    );
                        if (symbolInfo.Symbol == null)
                        {
                            Logger.Info("candidate symbols: {x}", symbolInfo.CandidateSymbols);
                        }
#endif

                        var methodSymbol = symbolInfo.Symbol as IMethodSymbol;
                        var result = methodSymbol != null
                                     // TODO optmize
                                     && CheckSymbol(methodSymbol, t);
#if TRACE
#pragma warning disable CA1303 // Do not pass literals as localized parameters
                        Logger.Debug("result is {result}", result);
#pragma warning restore CA1303 // Do not pass literals as localized parameters
#endif
                        return Tuple.Create(result, methodSymbol, n1);
                    }
                    else if (n1 is ObjectCreationExpressionSyntax o)
                    {
                        var symbolInfo = currentModel.GetSymbolInfo(o.Type);
                        var typeSymbol = symbolInfo.Symbol as INamedTypeSymbol;
                        var result = CheckTypeSymbol(typeSymbol, t);
                        return Tuple
                           .Create<bool, IMethodSymbol, SyntaxNode>(result, null, n1);
                    }

#pragma warning disable CA1303 // Do not pass literals as localized parameters
                    throw new Exception("Error");
#pragma warning restore CA1303 // Do not pass literals as localized parameters
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    throw;
                    // return Tuple.Create ( false , null , node  ) ;
                }
            }

            public static INamedTypeSymbol GetILoggerSymbol([NotNull] SemanticModel model)
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                return model.Compilation.GetTypeByMetadataName(ILoggerClassFullName);
            }

            public static INamedTypeSymbol GetLogBuilderSymbol([NotNull] SemanticModel model)
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                return model.Compilation.GetTypeByMetadataName(LogBuilderClassFullName);
            }

            public static INamedTypeSymbol GetNLogSymbol([NotNull] SemanticModel model)
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                return model.Compilation.GetTypeByMetadataName(LoggerClassFullName);
            }



        }
    }
}