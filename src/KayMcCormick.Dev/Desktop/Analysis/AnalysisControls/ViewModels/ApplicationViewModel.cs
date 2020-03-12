#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// ApplicationViewModel.cs
// 
// 2020-03-11-6:59 PM
// 
// ---
#endregion
using System ;
using System.Collections.ObjectModel ;
using AnalysisControls.Interfaces ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;

namespace AnalysisControls.ViewModels
{
    public class ApplicationViewModel : IApplicationViewModel
    {
        public ObservableCollection < SyntaxItem > SyntaxItems { get ; } =
            new ObservableCollection < SyntaxItem > ( ) ;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public ApplicationViewModel ( )
        {
            foreach ( SyntaxKind value in Enum.GetValues ( typeof ( SyntaxKind ) ) )
            {
                //Logger.Info ( "{value} {type}" , value , value.GetType ( ).FullName ) ;
                SyntaxToken ? token = null ;
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

                SyntaxItems.Add ( new SyntaxItem ( ) { SyntaxKind = value , Token = token } ) ;
            }
        }
    }
}