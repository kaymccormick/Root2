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

namespace ProjInterface
{
    public partial class ProjectHandlerImpl : ProjectHandler
    {
        [ NotNull ] private readonly SynchronizationContext _context ;
        private static readonly      Logger                 Logger = LogManager.GetCurrentClassLogger ( ) ;

        public ProjectHandlerImpl ( string s , VisualStudioInstance vsi, [ NotNull ]
                                    SynchronizationContext context) : base ( s , vsi )
        {
            if ( context == null )
            {
                throw new ArgumentNullException ( nameof ( context ) ) ;
            }

            _context = context ;
        }

        /// <inheritdoc />
        public override async Task < object > OnProcessDocumentAsync (
            Document                         document1
          , Triple                           triple
          , Action < ILogInvocation >        consumeLogInvocation
          , Func < object , IFormattedCode > func
        )
        {
            _context.Post (
                           ( t2 ) => {
                               var (tree , model , root) = ( Triple ) t2 ;
                               if ( tree     == null
                                    || model == null
                                    || root  == null )
                               {
                                   return ;
                               }

                               IFormattedCode codeControl = func ( t2 ) ;
                               // var sourceText = ProjLib.LibResources.Program_Parse ;
                               // codeControl.SourceCode = sourceText ;

                               codeControl.SyntaxTree            = tree ;
                               codeControl.Model                 = model;
                               codeControl.CompilationUnitSyntax = root ;


                               // var argument1 = XamlWriter.Save ( codeControl.FlowViewerDocument ) ;
                               // File.WriteAllText ( @"c:\data\out.xaml" , argument1 ) ;
                               // Logger.Info ( "xaml = {xaml}" , argument1 ) ;
                               // var tree = Transforms.TransformTree ( context.SyntaxTree ) ;
                               // Logger.Info ( "Tree is {tree}" , tree ) ;
                               codeControl.Refresh();
                           }, triple
                          ) ;


            if ( document1.Project.Name == "NLog" )
            {
                return new object ( ) ;
            }
            
            List < Tuple < int , string , List < Tuple < ExpressionSyntax , object > > > > query ;
            try
            {
                LogUsages.FindLogUsages (
                                         new CodeSource(document1.FilePath), CurrentRoot , CurrentModel
                                       , CurrentTree);

            }
            catch ( Exception ex )
            {
                Logger.Info ( ex , ex.ToString ( ) ) ;
            }

            return new object ( ) ;
            //Debug.Assert ( query != null , nameof ( query ) + " != null" ) ;
        }

        public List < Tuple < int , string , List < Tuple < ExpressionSyntax , object > > > >
            OutputList { get ; } =
            new List < Tuple < int , string , List < Tuple < ExpressionSyntax , object > > > > ( ) ;

        private void Collect (
            List < Tuple < int , string , List < Tuple < ExpressionSyntax , object > > > > query
        )
        {
            OutputList.AddRange ( query ) ;
        }

        public override async Task<Triple> OnPrepareProcessDocumentAsync ( Document doc )
        {
            
            Logger.Trace ( nameof ( OnProcessDocumentAsync ) ) ;
            if ( doc == null )
            {
                throw new ArgumentNullException ( nameof ( doc ) ) ;
            }

            var tree = await doc.GetSyntaxTreeAsync ( ) ;
            var model = await doc.GetSemanticModelAsync ( ) ;
            var root = tree.GetCompilationUnitRoot ( ) ;
            CurrentTree  = tree;
            CurrentModel = model;
            CurrentRoot  = root;

            return new Triple ( tree , model , root ) ;
        }

        public SyntaxTree CurrentTree { get ; set ; }

        public SemanticModel CurrentModel { get ; set ; }

        public CompilationUnitSyntax CurrentRoot { get ; set ; }


        public bool LogVisitedStatements { get ; set ; }

        public bool LimitToMarkedStatements { get ; set ; }

        public string PreviousCode { get ; set ; }

        public string FollowingCode { get ; set ; }

        public string InvocationCode { get ; set ; }

        public List < ILogInvocation > Invocations { get ; set ; } = new List < ILogInvocation > ( ) ;


        private static void NewMethod1 ( Compilation comp , INamedTypeSymbol t1 )
        {
            var t2 = comp.GetTypeByMetadataName ( LogUsages.ILoggerClassFullName ) ;
            var methodSymbols = t1.GetMembers ( )
                                  .Concat ( t2.GetMembers ( ) )
                                  .Where ( symbol => symbol.Kind == SymbolKind.Method )
                                  .Select ( symbol => ( IMethodSymbol ) symbol )
                                  .Where ( symbol => symbol.MethodKind == MethodKind.Ordinary )
                                  .ToList ( ) ;
            foreach ( var methodSymbol in methodSymbols )
            {
                Logger.Debug ( methodSymbol.ToString ( ) ) ;
            }
        }
    }
}