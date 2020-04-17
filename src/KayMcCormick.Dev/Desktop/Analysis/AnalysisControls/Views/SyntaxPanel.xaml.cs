using System ;
using System.ComponentModel ;
using System.Runtime.CompilerServices ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using AnalysisAppLib.ViewModel ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;

namespace AnalysisControls.Views
{
    /// <summary>
    ///     Interaction logic for SyntaxPanel.xaml
    /// </summary>
    [ TitleMetadata ( "Syntax panel" ) ]
    public sealed partial class SyntaxPanel : UserControl
      , INotifyPropertyChanged
      , IView < ISyntaxPanelViewModel >
      , IViewWithTitle
      , IControlView
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private ISyntaxPanelViewModel _viewModel ;

        /// <summary>
        /// 
        /// </summary>
        public SyntaxPanel ( ) : this ( null ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        public SyntaxPanel ( ISyntaxPanelViewModel viewModel )
        {
            _viewModel = viewModel ;
            if ( _viewModel != null )
            {
                ViewModel.PropertyChanged  += ViewModelOnPropertyChanged ;
                ViewModel.PropertyChanging += ViewModelOnPropertyChanging ;
            }

            InitializeComponent ( ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged ;

        #region Implementation of IView<ISyntaxPanelViewModel>
        /// <summary>
        /// 
        /// </summary>
        public ISyntaxPanelViewModel ViewModel
        {
            get { return _viewModel ; }
            set
            {
                if ( Equals ( value , _viewModel ) )
                {
                    return ;
                }

                if ( _viewModel != null )
                {
                    _viewModel.PropertyChanged -= ViewModelOnPropertyChanged ;

                    _viewModel.PropertyChanging -= ViewModelOnPropertyChanging ;
                    RemoveViewChangedHAndler ( ) ;
                }

                _viewModel = value ;
                if ( _viewModel != null )
                {
                    _viewModel.PropertyChanged  += ViewModelOnPropertyChanged ;
                    _viewModel.PropertyChanging += ViewModelOnPropertyChanging ;
                    AddViewChangedHAndler ( ) ;
                }

                OnPropertyChanged ( ) ;
            }
        }
        #endregion

        #region Implementation of IView1
        /// <summary>
        /// 
        /// </summary>
        [ NotNull ] public string ViewTitle { get { return "Syntax Panel" ; } }
        #endregion

        #region Overrides of FrameworkElement
        /// <summary>
        /// 
        /// </summary>
        public override void EndInit ( )
        {
            base.EndInit ( ) ;
            var view = ( CollectionViewSource ) TryFindResource ( "AllNodes" ) ;
            if ( view.View == null )
            {
                return ;
            }

            Logger.Debug ( "Adding ViewOnCurrentChanged handler to view.View" ) ;
            view.View.CurrentChanged += ViewOnCurrentChanged ;
        }
        #endregion

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
                RemoveViewChangedHAndler ( ) ;
            }
        }

        private void RemoveViewChangedHAndler ( )
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
                AddViewChangedHAndler ( ) ;
            }
        }

        private void AddViewChangedHAndler ( )
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

        private void ViewOnCurrentChanged ( object sender , EventArgs e )
        {
            Logger.Debug ( "{method}" , nameof ( ViewOnCurrentChanged ) ) ;
            var view = ( CollectionViewSource ) TryFindResource ( "AllNodes" ) ;
            if ( view.View != null )
            {
                if ( ViewModel == null )
                {
                    return ;
                }

                Logger.Debug (
                              "Setting viewmodel.SelectedItem to {item}"
                            , DebugRepr ( view.View.CurrentItem )
                             ) ;
                ViewModel.SelectedItem = view.View.CurrentItem ;
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

        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}