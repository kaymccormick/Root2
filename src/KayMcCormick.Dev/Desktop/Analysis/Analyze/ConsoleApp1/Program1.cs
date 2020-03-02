using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http ;
using System.Net.Http.Headers ;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory ;
using Microsoft.TeamFoundation.Core.WebApi ;

namespace ConsoleApp1
{
    class Program
    {                                                                                     //============= Config [Edit these with your settings] =====================
        static async Task  Main(string[] args)
        {
                    public void TestViewModel1 ( )
        {
            Logger.Debug ( "heelo" ) ;

#if false
            var utempDir = Path.Combine ( @"e:\scratch\projtests" , "temp" ) ;
            var x = Path.GetRandomFileName ( ) ;
            var tempDir = Path.Combine ( utempDir , x ) ;
            var cloneDir = Path.Combine ( tempDir , "clone" ) ;
            var info = Directory.CreateDirectory ( tempDir ) ;
            #if false
            this.Finalizers.Add (
                                 ( ) => {
                                     foreach ( var enumerateFileSystemEntry in Directory
                                        .EnumerateFileSystemEntries (
                                                                     tempDir
                                                                   , "*"
                                                                   , SearchOption.AllDirectories
                                                                    ) )
                                     {
                                         if ( File.Exists ( enumerateFileSystemEntry ) )
                                         {
                                             try
                                             {
                                                 File.SetAttributes(enumerateFileSystemEntry, FileAttributes.Normal);

                                                 var fileInfo = new FileInfo(enumerateFileSystemEntry) ;
                                                 fileInfo.Delete();
                                                 // File.Delete ( enumerateFileSystemEntry ) ;
                                             }
                                             catch ( Exception ex )
                                             {
                                                 Logger.Warn ( ex , ex.ToString() ) ;
                                             }
                                         }
                                     }
                                 }
                                ) ;
            #endif
                Logger.Info ( "tempdir is {tempDir}" , tempDir ) ;
            var cloneOptions = new CloneOptions ( ) ;
            cloneOptions.OnCheckoutProgress = ( path , steps , totalSteps )
                => Logger.Debug (
                                 "Checkout progress: {path} ( {steps} / {totalSteps} )"
                               , path
                               , steps
                               , totalSteps
                                ) ;
            cloneOptions.RepositoryOperationStarting = context => {
                Logger.Info ( "{a} {b}" , context.ParentRepositoryPath , context.RemoteUrl ) ;
                return true ;
            } ;
            //cloneOptions.OnUpdateTips = ( name , id , newId ) => 
            cloneOptions.RepositoryOperationCompleted =
                context => Logger.Info ( context.ToString ( ) ) ;
            cloneOptions.OnTransferProgress = progress => {
                Logger.Debug ("Transfer Progress: {IndexedObjects} {x} {y} {z}", progress.IndexedObjects, progress.TotalObjects,progress.ReceivedBytes , progress.ReceivedObjects ) ;
                return true ;
            } ;
            cloneOptions.OnProgress = output => {
                Logger.Debug ( output ) ;
                return true ;
            } ;
            
            Repository.Clone (
                              "https://kaymccormick@dev.azure.com/kaymccormick/KayMcCormick.Dev/_git/KayMcCormick.Dev"
                            , cloneDir
                      /*      , cloneOptions*/
                             ) ;
            var dd = new DirectoryInfo ( cloneDir ) ;
            var f = Directory.EnumerateFiles (
                                              cloneDir
                                            , "KayMcCormick.dev.sln"
                                            , SearchOption.AllDirectories
                                             )
                             .ToList ( ) ;
            Assert.NotEmpty ( f ) ;
            foreach ( var s in f )
            {
                Logger.Info ( s ) ;
            }
#endif
            var scope = InterfaceContainer.GetContainer ( ) ;
            var viewModel = scope.Resolve < IWorkspacesViewModel > ( ) ;
            viewModel.AnalyzeCommand(null, null, viewModel.ProjectBrowserViewModel.RootCollection.OfType<IProjectBrowserNode>().First());
#if false
            var vsInstance = viewModel.VsCollection.First (instance => instance.InstallationVersion.StartsWith("16.4.")) ;
            var mruItem = vsInstance.MruItems.Skip ( 1 ).First ( item => item.Exists ) ;
            Assert.NotNull ( viewModel.PipelineViewModel.Pipeline ) ;
            var i = MSBuildLocator
                   .QueryVisualStudioInstances (
                                                new VisualStudioInstanceQueryOptions ( )
                                                {
                                                    DiscoveryTypes =
                                                        DiscoveryType.VisualStudioSetup
                                                  , WorkingDirectory = @"c:\\temp\work1"
                                                }
                                               )
                   .Where (
                           instance => instance.VisualStudioRootPath == vsInstance.InstallationPath
                          )
                   .First ( ) ;

            var root = @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos" ;
            var solution = @"V3\TestCopy\src\KayMcCormick.Dev\KayMcCormick.Dev.sln" ;
            //solution = @"V2\LogTest\LogTest.sln";

            MSBuildLocator.RegisterInstance ( i ) ;

            var spath = Path.Combine ( root , solution ) ;
            Logger.Debug("posting {file}", f[0]);
            viewModel.PipelineViewModel.Pipeline.PipelineInstance.Post (f[0]) ;
#endif
            viewModel.PipelineViewModel.Pipeline.PipelineInstance.Completion.Wait ( 10000 ) ;
            if ( viewModel.PipelineViewModel.Pipeline.ResultBufferBlock
                          .TryReceiveAll ( out var list ) )
            {
                Logger.Info ( "Here" ) ;
                foreach ( var logInvocation in list )
                {
                    Logger.Info ( "{logInvocation}" , logInvocation ) ;
                }
            }

        }
    }
}
