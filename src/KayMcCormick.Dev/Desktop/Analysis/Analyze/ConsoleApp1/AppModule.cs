#region header
// Kay McCormick (mccor)
// 
// Analysis
// ConsoleApp1
// AppModule.cs
// 
// 2020-04-08-5:32 AM
// 
// ---
#endregion
using System ;
using System.Reflection ;
using System.Threading.Tasks.Dataflow ;
using Autofac ;
using FindLogUsages ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Dev.Command ;
using KayMcCormick.Lib.Wpf.Command ;
using Module = Autofac.Module ;

namespace ConsoleApp1
{
    /// <inheritdoc />
    internal sealed class AppModule : Module
    {
        // ReSharper disable once AnnotateNotNullParameter
        protected override void Load ( ContainerBuilder builder )
        {
            base.Load ( builder ) ;
            var actionBlock = new ActionBlock < ILogInvocation > ( Action ) ;
            builder.RegisterInstance ( actionBlock )
                   .As < ActionBlock < ILogInvocation > > ( )
                   .SingleInstance ( ) ;
            builder.RegisterType < AppContext > ( ).AsSelf ( ) ;
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
                builder.RegisterType < Program > ( ) ;
                if ( title != null )
                {
                    builder.Register (
                                      ( c , p ) => {
                                          var program = c.Resolve < Program > ( ) ;
                                          var appContext = c.Resolve
                                          < AppContext
                                          > ( ) ;
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
                                                                                                 , program);
                                                                           await @delegate.Invoke (command,
                                                                                                   appContext
                                                                                                  ) ;
                                                                           return AppCommandResult
                                                                              .Success ;
                                                                       }
                                                                     , methodInfo
                                                                      ) ;
                                      }
                                     )
                           .AsImplementedInterfaces ( )
                           .AsSelf ( ) ;
                }
            }
#if TERMUI
            builder.RegisterType < TermUi > ( ).AsSelf ( ) ;
#endif
        }

        private void Action ( ILogInvocation obj ) { }
    }
}