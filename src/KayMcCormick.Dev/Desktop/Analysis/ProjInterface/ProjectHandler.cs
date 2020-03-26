using System ;
using System.ComponentModel ;
using System.Threading ;
using System.Threading.Tasks ;
using Microsoft.CodeAnalysis ;
using NLog ;

namespace ProjInterface
{
    public abstract class ProjectHandler : ISupportInitialize
    {

        public delegate void ProcessDocumentDelegate ( Document document ) ;

        public delegate void ProcessProjectDelegate (
            Workspace workspace
          , Project   project
        ) ;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public ProcessDocumentDelegate ProcessDocument ;
        public ProcessProjectDelegate  ProcessProject ;


        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" />
        ///     class.
        /// </summary>
        public ProjectHandler (
            string solutionPath
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
            Action < ILogInvocation >    consumeLogInvocation
          , SynchronizationContext       current
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

        public abstract Task < object > OnProcessDocumentAsync (
            Document                         document
          , Triple                           triple
          , Action < ILogInvocation >        consumeLogInvocation
          , Func < object , IFormattedCode > func
        ) ;

        public abstract Task < Triple > OnPrepareProcessDocumentAsync ( Document doc ) ;

    }
}