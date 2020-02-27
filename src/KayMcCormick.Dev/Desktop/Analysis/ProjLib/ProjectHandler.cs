using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Runtime.Serialization ;
using System.Threading.Tasks ;
using CodeAnalysisApp1 ;
using JetBrains.Annotations ;
using MessageTemplates ;
using MessageTemplates.Parsing ;
using Microsoft.Build.Locator ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.MSBuild ;
using Microsoft.CodeAnalysis.Text ;
using NLog ;

namespace ProjLib
{
    public class ProjectHandler : ISupportInitialize
    {
        public delegate void ProcessDocumentDelegate ( Document document ) ;

        public delegate void ProcessProjectDelegate (
            MSBuildWorkspace workspace
          , Project          project
        ) ;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public ProcessDocumentDelegate ProcessDocument ;
        public ProcessProjectDelegate  ProcessProject ;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" />
        ///     class.
        /// </summary>
        public ProjectHandler ( string solutionPath , VisualStudioInstance instance )
        {
            SolutionPath = solutionPath ;
            Instance     = instance ;
        }

        public string SolutionPath { get ; }

        public VisualStudioInstance Instance { get ; }

        public MSBuildWorkspace Workspace { get ; set ; }


        /// <summary>Signals the object that initialization is starting.</summary>
        public void BeginInit ( ) { }

        /// <summary>Signals the object that initialization is complete.</summary>
        public void EndInit ( ) { throw new NotImplementedException ( ) ; }

        public async Task < MSBuildWorkspace > LoadSolutionInstanceAsync(
            string               solutionPath
          , VisualStudioInstance instance
        )

        {
            if ( MSBuildLocator.IsRegistered )
            {
                // MSBuildLocator.Unregister ( ) ;
            }

            if ( MSBuildLocator.CanRegister )
            {
                MSBuildLocator.RegisterInstance ( instance ) ;
            }
            else
            {
                // throw new Exception ( "Unable to register msbuildlocator" ) ;
            }

            MSBuildWorkspace workspace ;
            try
            {
                workspace = MSBuildWorkspace.Create ( ) ;
            }
            catch ( Exception ex )
            {
                throw ;
            }

            // Print message for WorkspaceFailed event to help diagnosing project load failures.
            workspace.WorkspaceFailed += ( o , e ) => Console.WriteLine ( e.Diagnostic.Message ) ;

            // ReSharper disable once LocalizableElement
            Debug.Assert ( solutionPath != null , nameof ( solutionPath ) + " != null" ) ;
            Logger.Debug ( $"Loading solution '{solutionPath}'" ) ;

            // Attach progress reporter so we print projects as they are loaded.
            await workspace.OpenSolutionAsync ( solutionPath , progressReporter ) ;
            // , new Program.ConsoleProgressReporter()
            // );
            Logger.Debug( $"Finished loading solution '{solutionPath}'" ) ;
            return workspace ;
        }

        public IProgress < ProjectLoadProgress > progressReporter { get ; set ; }

        public async Task < bool > LoadAsync ( )
        {
            Workspace = await LoadSolutionInstanceAsync( SolutionPath , Instance ) ;
            return true ;
        }

        public async Task ProcessAsync (Action<LogInvocation> consumeLogInvocation)
        {
            foreach ( var pr in Workspace.CurrentSolution.Projects )
            {
                ProcessProject?.Invoke(Workspace , pr ) ;
                foreach ( var prDocument in pr.Documents )
                {
                    ProcessDocument?.Invoke( prDocument ) ;
                    await OnPrepareProcessDocumentAsync ( prDocument ) ;
                    await OnProcessDocumentAsync ( prDocument, consumeLogInvocation ) ;
                }
            }
        }

        public virtual async Task OnProcessDocumentAsync (
            Document                                    document
          , Action < LogInvocation > consumeLogInvocation
        ) { return ; }
        public virtual async Task OnPrepareProcessDocumentAsync ( Document doc )      { return ; }
    }

    public partial class ProjectHandlerImpl : ProjectHandler
    {
        private static readonly Logger Logger              = LogManager.GetCurrentClassLogger ( ) ;

        public ProjectHandlerImpl ( string s , VisualStudioInstance vsi ) : base ( s , vsi ) { }

        /// <inheritdoc />
        public override async Task OnProcessDocumentAsync (
            Document                 document1
          , Action < LogInvocation > consumeLogInvocation
        )
        {
            if ( document1.Project.Name == "NLog" )
            {
                return ;
            }
            
            List < Tuple < int , string , List < Tuple < ExpressionSyntax , object > > > > query ;
            try
            {
                LogUsages.FindLogUsages (
                                  new CodeSource(document1.FilePath), CurrentRoot , CurrentModel, consumeLogInvocation
                                , this.LimitToMarkedStatements
                                , this.LogVisitedStatements, (parms) => LogUsages.ProcessInvocation( parms ), CurrentTree);

            }
            catch ( Exception ex )
            {
                Logger.Info ( ex , ex.ToString ( ) ) ;
            }

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

        public override async Task OnPrepareProcessDocumentAsync ( Document doc )
        {
            if ( doc.Project.Name == "NLog" )
            {
                return ;
            }

            Logger.Trace ( nameof ( OnProcessDocumentAsync ) ) ;
            if ( doc == null )
            {
                throw new ArgumentNullException ( nameof ( doc ) ) ;
            }

            var tree = await doc.GetSyntaxTreeAsync ( ) ;
            var model = await doc.GetSemanticModelAsync ( ) ;
            var root = tree.GetCompilationUnitRoot ( ) ;
            CurrentTree  = tree ;
            CurrentModel = model ;
            CurrentRoot  = root ;
        }

        public SyntaxTree CurrentTree { get ; set ; }

        public SemanticModel CurrentModel { get ; set ; }

        public CompilationUnitSyntax CurrentRoot { get ; set ; }


        public bool LogVisitedStatements { get ; set ; }

        public bool LimitToMarkedStatements { get ; set ; }

        public string PreviousCode { get ; set ; }

        public string FollowingCode { get ; set ; }

        public string InvocationCode { get ; set ; }

        public List < LogInvocation > Invocations { get ; set ; } = new List < LogInvocation > ( ) ;


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
