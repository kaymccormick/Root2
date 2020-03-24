using System ;
using System.ComponentModel ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using AnalysisAppLib.ViewModel ;
using JetBrains.Annotations ;
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
    [ TitleMetadata ( "Syntax panel" ) ]
    public sealed partial class SyntaxPanel : UserControl
      , IView < ISyntaxPanelViewModel >
      , IViewWithTitle
      , IControlView
    {
        private ISyntaxPanelViewModel _viewModel ;

        private static readonly Logger
            Logger = LogManager.GetCurrentClassLogger ( ) ;

        public SyntaxPanel ( ) {
        }

        public SyntaxPanel ( ISyntaxPanelViewModel viewModel )
        {
            _viewModel = viewModel ;
            InitializeComponent ( ) ;
            var view = ( CollectionViewSource ) TryFindResource ( "AllNodes" ) ;
            if ( view.View != null )
            {
                Logger.Debug ( "Adding ViewOnCurrentChanged handler to view.View" ) ;
                view.View.CurrentChanged += ViewOnCurrentChanged ;
            }

            ViewModel.PropertyChanged  += ViewModelOnPropertyChanged ;
            ViewModel.PropertyChanging += ViewModelOnPropertyChanging ;
        }

        private void ViewModelOnPropertyChanging (
            object                                sender
          , [ NotNull ] PropertyChangingEventArgs e
        )
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

        private void ViewModelOnPropertyChanged (
            object                               sender
          , [ NotNull ] PropertyChangedEventArgs e
        )
        {
            Logger.Debug (
                          "{method} {property}"
                        , nameof ( ViewModelOnPropertyChanged )
                        , e.PropertyName
                         ) ;
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
            Logger.Debug ( "{method}" , nameof ( ViewOnCurrentChanged ) ) ;
            var view = ( CollectionViewSource ) TryFindResource ( "AllNodes" ) ;
            if ( view.View != null )
            {
                if ( ViewModel != null )
                {
                    Logger.Debug (
                                  "Setting viewmodel.SelectedItem to {item}"
                                , DebugRepr ( view.View.CurrentItem )
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
            object                                                sender
          , [ NotNull ] RoutedPropertyChangedEventArgs < object > e
        )
        {
            var view = ( CollectionViewSource ) TryFindResource ( "AllNodes" ) ;
            var eNewValue = e.NewValue ;
            string reprVal ;
            if ( eNewValue is SyntaxNode sn )
            {
                reprVal = sn.GetLocation ( ) + " [" + sn.Kind ( ) + "]" ;
            }
            else
            {
                reprVal = eNewValue.ToString ( ) ;
            }

            Logger.Debug ( "Selected Item Changed: {new}" , reprVal ) ;
            Logger.Debug ( "Setting current on view to new value." ) ;
            view.View.MoveCurrentTo ( eNewValue ) ;
            Logger.Debug ( "Current is now {cur}" , DebugRepr ( view.View.CurrentItem ) ) ;
        }

        [ NotNull ]
        private static string DebugRepr ( [ NotNull ] object viewCurrentItem )
        {
            string reprVal ;
            if ( viewCurrentItem is SyntaxNode sn )
            {
                reprVal = sn.GetLocation ( ) + " [" + sn.Kind ( ) + "]" ;
            }
            else
            {
                reprVal = viewCurrentItem.ToString ( ) ;
            }

            return reprVal ;
        }

        #region Implementation of IView<ISyntaxPanelViewModel>
        public ISyntaxPanelViewModel ViewModel
        {
            get { return _viewModel ; }
            set { _viewModel = value ; }
        }
        #endregion

        #region Implementation of IView1
        [ NotNull ] public string ViewTitle { get { return "Syntax Panel" ; } }
        #endregion
    }
}