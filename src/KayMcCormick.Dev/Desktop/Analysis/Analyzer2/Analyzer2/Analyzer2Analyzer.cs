using System.Collections.Immutable ;
using System.Linq ;

using Analyzer2.Properties ;
using FindLogUsages ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.Diagnostics ;

namespace Analyzer2
{
    [ DiagnosticAnalyzer ( LanguageNames.CSharp ) ]
    public class Analyzer2Analyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Analyzer2" ;

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title =
            new LocalizableResourceString (
                                           nameof ( Resources.AnalyzerTitle )
                                         , Resources.ResourceManager
                                         , typeof ( Resources )
                                          ) ;

        private static readonly LocalizableString MessageFormat =
            new LocalizableResourceString (
                                           nameof ( Resources.AnalyzerMessageFormat )
                                         , Resources.ResourceManager
                                         , typeof ( Resources )
                                          ) ;

        private static readonly LocalizableString Description =
            new LocalizableResourceString (
                                           nameof ( Resources.AnalyzerDescription )
                                         , Resources.ResourceManager
                                         , typeof ( Resources )
                                          ) ;

        private const string Category = "Naming" ;

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor (
                                                                             DiagnosticId
                                                                           , Title
                                                                           , MessageFormat
                                                                           , Category
                                                                           , DiagnosticSeverity
                                                                                .Warning
                                                                           , true
                                                                           , Description
                                                                            ) ;

        public override ImmutableArray < DiagnosticDescriptor > SupportedDiagnostics
        {
            get { return ImmutableArray.Create ( Rule ) ; }
        }

        public override void Initialize ( AnalysisContext context )
        {
            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterCodeBlockAction ( Action ) ;
        }

        private void Action ( CodeBlockAnalysisContext obj )
        {
            var result = FindLogUsagesMain.Process (
                                                ( ) => new LogInvocation2 ( )
                                              , obj.SemanticModel
                                              , obj.SemanticModel.SyntaxTree
                                              , obj.CodeBlock
                                              , null
                                               ) ;
            result.ContinueWith (
                                 task => {
                                     foreach ( var logInvocation in task.Result )
                                     {
                                         obj.ReportDiagnostic (
                                                               Diagnostic.Create (Rule
                                                                                , logInvocation
                                                                                     .Location
                                                                                 )
                                                              ) ;
                                     }
                                 }
                                ) ;
        }

        private static void AnalyzeSymbol ( SymbolAnalysisContext context )
        {
            // TODO: Replace the following code with your own analysis, generating Diagnostic objects for any issues you find
            var namedTypeSymbol = ( INamedTypeSymbol ) context.Symbol ;

            // Find just those named type symbols with names containing lowercase letters.
            if ( namedTypeSymbol.Name.ToCharArray ( ).Any ( char.IsLower ) )
            {
                // For all such symbols, produce a diagnostic.
                var diagnostic = Diagnostic.Create (
                                                    Rule
                                                  , namedTypeSymbol.Locations[ 0 ]
                                                  , namedTypeSymbol.Name
                                                   ) ;

                context.ReportDiagnostic ( diagnostic ) ;
            }
        }
    }
}