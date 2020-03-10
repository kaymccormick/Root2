#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// Container.cs
// 
// 2020-02-27-1:59 AM
// 
// ---
#endregion
using System.Collections.Generic ;
using AnalysisFramework ;
using Autofac ;
using Autofac.Core ;
using Autofac.Integration.Mef ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;
using NLog.Fluent ;

namespace ProjLib
{
    public static class ProjLibContainer
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public static ILifetimeScope GetScope (params IModule[] modules )
        {
            
            var b = new ContainerBuilder ( ) ;
            b.RegisterMetadataRegistrationSources ( ) ;

            // b.RegisterGeneric ( typeof ( SpanObject <> ) ).As ( typeof ( ISpanObject <> ) ) ;
            foreach ( var module in modules )
            {
                b.RegisterModule ( module ) ;
            }
            b.RegisterType < LogInvocationSpan > ( )
             .As < ISpanViewModel > ( )
             .As < ISpanObject < ILogInvocation > > ( ) ;
            b.RegisterType < TokenSpanObject > ( )
             .As < ISpanViewModel > ( )
             .As < ISpanObject < SyntaxToken > > ( ) ;
            b.RegisterType < TriviaSpanObject > ( )
             .As < ISpanViewModel > ( )
             .As < ISpanObject < SyntaxTrivia > > ( ) ;
            b.RegisterType < Visitor2 > ( ).AsSelf ( ).As < CSharpSyntaxWalker > ( ) ;
            //b.RegisterType < FormattedCode > ( ).AsSelf ( ) ;

            b.RegisterType < TransformScope > ( )
             .WithParameter ( new NamedParameter ( "sourceCode" , LibResources.Program_Parse ) )
             .AsSelf ( )
             .InstancePerLifetimeScope ( ) ;

            return b.Build ( ).BeginLifetimeScope ( ) ;
        }
    }
}