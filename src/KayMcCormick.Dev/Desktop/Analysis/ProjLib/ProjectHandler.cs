#if false
using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Markup ;
using System.Windows.Threading ;
using AnalysisFramework ;

using JetBrains.Annotations ;
using MessageTemplates ;
using MessageTemplates.Parsing ;

using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.MSBuild ;
using Microsoft.CodeAnalysis.Text ;
using Microsoft.VisualStudio.Shell ;
using NLog ;
using Task = System.Threading.Tasks.Task ;

namespace ProjLib
{
    public struct SynchronizationContextAwaiter : INotifyCompletion
    {
        private static readonly SendOrPostCallback _postCallback = state => ((Action)state)();

        private readonly SynchronizationContext _context;
        public SynchronizationContextAwaiter(SynchronizationContext context)
        {
            _context = context;
        }

        public bool IsCompleted => _context == SynchronizationContext.Current;

        public void OnCompleted(Action continuation) => _context.Post(_postCallback, continuation);

        public void GetResult() { }
    }
    public abstract class ProjectHandler : ISupportInitialize
    {

        public delegate void ProcessDocumentDelegate ( Document document ) ;

        public delegate void ProcessProjectDelegate (
            Workspace workspace
          , Project          project
        ) ;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public ProcessDocumentDelegate ProcessDocument ;
        public ProcessProjectDelegate  ProcessProject ;


        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" />
        ///     class.
        /// </summary>
        public ProjectHandler (
            string                 solutionPath
          //, VisualStudioInstance   instance
        )
        {
            SolutionPath = solutionPath ;
            //Instance     = instance ;
        }

        public string SolutionPath { get ; }

        //public VisualStudioInstance Instance { get ; }

        public Workspace Workspace { get ; set ; }


        /// <summary>Signals the object that initialization is starting.</summary>
        public void BeginInit ( ) { }

        /// <summary>Signals the object that initialization is complete.</summary>
        public void EndInit ( ) { throw new NotImplementedException ( ) ; }

        public IProgress < ProjectLoadProgress > progressReporter { get ; set ; }
        #if false
        public async Task < bool > LoadAsync ( )
        {
            Workspace = await ProjLibUtils.LoadSolutionInstanceAsync( progressReporter , SolutionPath , Instance ) ;
            return true ;
        }
#endif

        public async Task<object> ProcessAsync (
            Action < ILogInvocation > consumeLogInvocation
          , SynchronizationContext   current
            ,Func<object, IFormattedCode> func
        )
        {
            foreach ( var pr in Workspace.CurrentSolution.Projects )
            {
                ProcessProject?.Invoke(Workspace , pr ) ;
                foreach ( var prDocument in pr.Documents )
                {
                    ProcessDocument?.Invoke( prDocument ) ;
                    Triple triple = await OnPrepareProcessDocumentAsync ( prDocument ) ;
                    await OnProcessDocumentAsync ( prDocument, triple, consumeLogInvocation, func ) ;
                }
            }

            return new object ( ) ;
        }

        public abstract  Task < object > OnProcessDocumentAsync (
            Document                        document
          , Triple                          triple
          , Action < ILogInvocation >        consumeLogInvocation
          , Func < object , IFormattedCode > func
        ) ;

        public abstract  Task < Triple > OnPrepareProcessDocumentAsync ( Document doc ) ;

    }

    public partial class ProjectHandlerImpl : ProjectHandler
    {
        [ NotNull ] private readonly SynchronizationContext _context ;
        private static readonly Logger Logger              = LogManager.GetCurrentClassLogger ( ) ;

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
            Document                        document1
          , Triple                          triple
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

    public class Triple
    {
        public SyntaxTree Tree { get ; }

        public SemanticModel Model { get ; }

        public CompilationUnitSyntax Root { get ; }

        public void Deconstruct(out SyntaxTree tree, out SemanticModel model, out CompilationUnitSyntax root)
        {
            tree = Tree ;
            model = Model ;
            root = Root ;
        }

        public Triple ( SyntaxTree tree , SemanticModel model , CompilationUnitSyntax root )
        {
            Tree = tree ;
            Model = model ;
            Root = root ;
        }
    }

    internal class NoMessageParameterException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class.</summary>
        public NoMessageParameterException ( ) { }

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message.</summary>
        /// <param name="message">The message that describes the error. </param>
        public NoMessageParameterException ( string message ) : base ( message ) { }

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified. </param>
        public NoMessageParameterException ( string message , Exception innerException ) : base (
                                                                                                 message
                                                                                               , innerException
                                                                                                )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with serialized data.</summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is <see langword="null" />. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is <see langword="null" /> or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected NoMessageParameterException (
            [ NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }
    }
}
#endif