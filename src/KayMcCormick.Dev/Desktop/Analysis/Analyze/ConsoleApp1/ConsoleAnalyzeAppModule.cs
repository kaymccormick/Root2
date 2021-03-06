﻿#region header
// Kay McCormick (mccor)
// 
// Analysis
// ConsoleApp1
// ConsoleAnalyzeAppModule.cs
// 
// 2020-04-08-5:32 AM
// 
// ---
#endregion
using System.Reflection ;
using System.Threading.Tasks.Dataflow ;
using AnalysisAppLib;
using Autofac ;
using FindLogUsages ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Dev.Command ;
using KayMcCormick.Dev.Container ;
using KayMcCormick.Lib.Wpf.Command ;
using Module = Autofac.Module ;

namespace ConsoleAnalysis
{
    /// <inheritdoc />
    internal sealed class ConsoleAnalyzeAppModule : Module
    {
        // ReSharper disable once AnnotateNotNullParameter
        protected override void Load ( ContainerBuilder builder )
        {
            base.Load ( builder ) ;
            builder.RegisterType<AppDbContextHelper>().WithCallerMetadata();
            var actionBlock = new ActionBlock < ILogInvocation > ( Action ) ;
            builder.RegisterInstance ( actionBlock )
                   .As < ActionBlock < ILogInvocation > > ( )
                   .SingleInstance ( ).WithCallerMetadata() ;
            builder.RegisterType < AppContext > ( ).AsSelf ( ).WithCallerMetadata() ;
            foreach ( var methodInfo in typeof ( Program ).GetMethods (
                                                                       BindingFlags.Instance
                                                                       | BindingFlags.Public
                                                                      ) )
            {
                var title =
                    ( TitleMetadataAttribute ) methodInfo.GetCustomAttribute (
                                                                              typeof (
                                                                                  TitleMetadataAttribute
                                                                              )
                                                                             ) ;
                builder.RegisterType < Program > ( ).WithCallerMetadata (  ) ;
                if ( title != null )
                {
                    builder.Register (
                                      ( c , p ) => {
                                          var program = c.Resolve < Program > ( ) ;
                                          var appContext = c.Resolve < AppContext > ( ) ;
                                          return new LambdaAppCommand (
                                                                       title.Title
                                                                     , async command => {
                                                                           var @delegate =
                                                                               ( Util.
                                                                                   AsyncCommandDelegate
                                                                               ) methodInfo
                                                                                  .CreateDelegate (
                                                                                                   typeof
                                                                                                   ( Util
                                                                                                       .AsyncCommandDelegate
                                                                                                   )
                                                                                                 , program
                                                                                                  ) ;
                                                                           await @delegate.Invoke (
                                                                                                   command
                                                                                                 , appContext
                                                                                                  ) ;
                                                                           return AppCommandResult
                                                                              .Success ;
                                                                       }
                                                                     , methodInfo
                                                                      ) ;
                                      }
                                     )
                           .AsImplementedInterfaces ( )
                           .AsSelf ( ).WithCallerMetadata (  ) ;
                }
            }
#if TERMUI
            builder.RegisterType < TermUi > ( ).AsSelf ( ).WithCallerMetadata() ;
#endif
        }

        private static void Action ( ILogInvocation obj ) { }
    }
}