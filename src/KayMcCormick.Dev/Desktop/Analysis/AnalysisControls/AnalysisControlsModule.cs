#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// AnalysisControlsModule.cs
// 
// 2020-03-06-12:50 PM
// 
// ---
#endregion
using System ;
using System.Collections.ObjectModel ;
using System.Windows.Controls ;
using Autofac ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;
using ProjLib ;

namespace AnalysisControls
{
    public class AnalysisControlsModule : Module {
        #region Overrides of Module
        protected override void Load ( ContainerBuilder builder )
        {
            builder.RegisterType < CompilationView > ( ).AsSelf ( ) ;
            builder.RegisterType < CompilationViewModel > ( ).As < ICompilationViewModel > ( ) ;
            builder.RegisterType < ComponentViewModel > ( ).As < IComponentViewModel > ( ) ;
            builder.RegisterType < ApplicationViewModel > ( )
                   .As < IApplicationViewModel > ( )
                   .SingleInstance ( ) ;
            builder.RegisterType < ComponentPage > ( ).As < Page > ( ) ;
            builder.RegisterAssemblyTypes ( ThisAssembly )
                   .Where (
                           type => typeof ( IView1 ).IsAssignableFrom ( type )
                                   || typeof ( IViewModel ).IsAssignableFrom ( type )
                                   || typeof(AppWindow).IsAssignableFrom(type)
                          )
                   .AsSelf ( )
                   .AsImplementedInterfaces ( ) ;
        }
        #endregion
    }

    public interface IApplicationViewModel
    {
        ObservableCollection < SyntaxItem > SyntaxItems { get ; }
    }

    public class ApplicationViewModel : IApplicationViewModel
    {
        public ObservableCollection<SyntaxItem> SyntaxItems { get ; } = new ObservableCollection < SyntaxItem > ();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        public ApplicationViewModel ( )
        {
            foreach ( SyntaxKind  value in Enum.GetValues ( typeof ( SyntaxKind ) ) )
            {
                //Logger.Info ( "{value} {type}" , value , value.GetType ( ).FullName ) ;
                SyntaxToken? token = null ;
                if ( SyntaxFacts.IsAnyToken ( value ) )
                {
                    try
                    {
                        // if ( Enum.GetName ( typeof ( SyntaxKind ) , value ).EndsWith ( "Token" ) )
                        // {
                        token = SyntaxFactory.Token ( value ) ;
                        // }

                    }
                    catch ( Exception ex )
                    {
                        Logger.Debug ( ex , ex.Message ) ;
                    }
                }

                SyntaxItems.Add ( new SyntaxItem ( ) { SyntaxKind = value, Token = token } ) ;
            
            }
        }
    }

    public class SyntaxItem
    {
        private SyntaxToken ? _token ;

        public SyntaxKind SyntaxKind { get ; set ; }

        public SyntaxToken ? Token { get { return _token ; } set { _token = value ; } }
    }
}