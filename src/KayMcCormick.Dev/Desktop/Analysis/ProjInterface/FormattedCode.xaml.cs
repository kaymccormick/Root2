using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.CodeAnalysis ;
using NLog ;
using ProjLib ;

namespace ProjInterface
{
    /// <summary>
    /// Interaction logic for FormattedCode.xaml
    /// </summary>
    public partial class FormattedCode : UserControl
    {
        public FormattedCode ( )
        {
            InitializeComponent ( ) ;
        }

        /// <summary>Invoked whenever the effective value of any dependency property on this <see cref="T:System.Windows.FrameworkElement" /> has been updated. The specific dependency property that changed is reported in the arguments parameter. Overrides <see cref="M:System.Windows.DependencyObject.OnPropertyChanged(System.Windows.DependencyPropertyChangedEventArgs)" />.</summary>
        /// <param name="e">The event data that describes the property that changed, as well as old and new values.</param>
        protected override void OnPropertyChanged ( DependencyPropertyChangedEventArgs e )
        {
            base.OnPropertyChanged ( e ) ;
            if ( e.Property.Name == "Tag" )
            {
                try
                {
                    TextBlock block = ( TextBlock ) this.Content ;
                    block.Text = "" ;
                    LogInvocationBase exx = ( LogInvocationBase ) this.Tag ;
                    var statementSyntax = exx.Node ;
                    if ( statementSyntax == null )
                    {
                        throw new Exception ( "no st" ) ;
                    }

                    if ( DoSym )
                    {
                        Z z = new Z ( block , statementSyntax.FullSpan , TryFindResource ) ;
                        var enclosingSymbol =
                            exx.CurrentModel.GetEnclosingSymbol ( statementSyntax.SpanStart ) ;
                        z.Visit ( enclosingSymbol ) ;
                    }

                    if ( DoVisit )
                    {
                        Visitor x = new Visitor (
                                                 block
                                               , TryFindResource
                                               , exx
                                               , SyntaxWalkerDepth.Trivia
                                                ) ;
                        x.Visit ( statementSyntax ) ;
                    }
                }
                catch ( Exception ex )
                {
                    MessageBox.Show ( ex.ToString ( ) , "Error" ) ;
                    LogManager.GetCurrentClassLogger().Error ( ex , ex.ToString ( ) ) ;
                }
            }
        }

        public bool DoSym { get ; set ; }

        public bool DoVisit { get ; set ; } = true ;
    }
}