using System ;
using System.ComponentModel ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using AnalysisControls.Interfaces ;
using AnalysisControls.ViewModels ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;

namespace AnalysisControls.Views
{
    /// <summary>
    /// Interaction logic for SyntaxPanel.xaml
    /// </summary>
    [TitleMetadata("Syntax panel")]
    public partial class SyntaxPanel : UserControl, IView <ISyntaxPanelViewModel>, IView1
    {
        private ISyntaxPanelViewModel viewModel = new SyntaxPanelViewModel();
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        public SyntaxPanel()
        {
            InitializeComponent();
            var view = (CollectionViewSource)TryFindResource("AllNodes");
            if ( view.View != null )
            {
                Logger.Debug("Adding ViewOnCurrentChanged handler to view.View");
                view.View.CurrentChanged += ViewOnCurrentChanged ;
            }

            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
            ViewModel.PropertyChanging += ViewModelOnPropertyChanging;
        }

        private void ViewModelOnPropertyChanging ( object sender , PropertyChangingEventArgs e )
        {
            Logger.Debug (
                          "{method} {property}"
                        , nameof ( ViewModelOnPropertyChanging )
                        , e.PropertyName
                         ) ;
            if ( e.PropertyName == "CompilationUnitSyntax" )
            {
                var view = ( CollectionViewSource ) TryFindResource ( "AllNodes" ) ;
                if ( view.View != null )
                {
                    Logger.Debug ( "removing ViewOnCurrentChanged handler from view.View" ) ;
                    view.View.CurrentChanged -= ViewOnCurrentChanged ;
                }
                else
                {
                    Logger.Debug ( "view.View is null not removing handler" ) ;
                }
            }
        }

        private void ViewModelOnPropertyChanged ( object sender , PropertyChangedEventArgs e )
        {
            Logger.Debug(
                         "{method} {property}"
                       , nameof(ViewModelOnPropertyChanged)
                       , e.PropertyName
                        );
            if ( e.PropertyName == "CompilationUnitSyntax" )
            {
                var view = ( CollectionViewSource ) TryFindResource ( "AllNodes" ) ;

                if ( view.View != null )
                {
                    Logger.Debug ( "Adding ViewOnCurrentChanged handler to view.View" ) ;
                    view.View.CurrentChanged += ViewOnCurrentChanged ;
                }
                else
                {
                    Logger.Debug ( "view.View is null not adding handler" ) ;
                }
            }
        }

        private void ViewOnCurrentChanged ( object sender , EventArgs e )
        {

            Logger.Debug(
                         "{method}"
                       , nameof(ViewOnCurrentChanged)
                        );
            var view = (CollectionViewSource)TryFindResource("AllNodes");
            if (view.View != null)
            {
                if ( ViewModel != null )
                {
                    Logger.Debug (
                                  "Setting viewmodel.SelectedItem to {item}"
                                , DebugRepr(view.View.CurrentItem)
                                 ) ;
                    ViewModel.SelectedItem = view.View.CurrentItem ;
                }
            }
            else
            {
                Logger.Debug ( "view.view is null" ) ;
            }
        }

        private void tv_SelectedItemChanged (
            object                                    sender
          , RoutedPropertyChangedEventArgs < object > e
        )
        {
            var view = (CollectionViewSource)TryFindResource("AllNodes");
            var eNewValue = e.NewValue ;
            string reprVal ;
            if(eNewValue is SyntaxNode sn)
            {
                reprVal = sn.GetLocation ( ) + " [" + sn.Kind ( ) + "]" ;
            }
            else
            {
                reprVal = eNewValue.ToString ( ) ;
            }
            Logger.Debug ( "Selected Item Changed: {new}" , reprVal ) ;
            Logger.Debug ( "Setting current on view to new value." ) ;
            view.View.MoveCurrentTo(eNewValue);
            Logger.Debug ( "Current is now {cur}" , DebugRepr(view.View.CurrentItem) ) ;
            
        }

        private static string DebugRepr ( object viewCurrentItem )
        {
            string reprVal;
            if (viewCurrentItem is SyntaxNode sn)
            {
                reprVal = sn.GetLocation() + " [" + sn.Kind() + "]";
            }
            else
            {
                reprVal = viewCurrentItem.ToString();
            }

            return reprVal ;
        }

        #region Implementation of IView<ISyntaxPanelViewModel>
        public ISyntaxPanelViewModel ViewModel { get => viewModel ; set => viewModel = value ; }
        #endregion

        #region Implementation of IView1
        public string ViewTitle => "Syntax Panel" ;
        #endregion
    }
}
