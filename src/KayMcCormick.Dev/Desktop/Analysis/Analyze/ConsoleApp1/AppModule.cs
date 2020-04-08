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
            foreach ( var methodInfo in typeof ( Program ).GetMethods ( BindingFlags.Static|BindingFlags.NonPublic ) )
            {
                var title = (TitleMetadataAttribute)methodInfo.GetCustomAttribute ( typeof ( TitleMetadataAttribute ) ) ;
                if ( title != null )
                {
                    builder.RegisterInstance (
                                      new LambdaAppCommand (
                                                            title.Title
                                                          , async command => {
                                                                methodInfo.Invoke (
                                                                                   null
                                                                                 , Array
                                                                                      .Empty <
                                                                                           object
                                                                                       > ( )
                                                                                  ) ;
                                                                return AppCommandResult.Success ;
                                                            }
                                                          , methodInfo
                                                           )
                                     ) ;
                }
            }
#if TERMUI
            builder.RegisterType < TermUi > ( ).AsSelf ( ) ;
#endif
        }

        private void Action ( ILogInvocation obj ) { }
    }
}