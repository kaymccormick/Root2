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
using System.Threading.Tasks.Dataflow ;
using Autofac ;
using FindLogUsages ;

namespace ConsoleApp1
{
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
#if TERMUI
            builder.RegisterType < TermUi > ( ).AsSelf ( ) ;
#endif
        }

        private void Action ( ILogInvocation obj ) { }
    }
}