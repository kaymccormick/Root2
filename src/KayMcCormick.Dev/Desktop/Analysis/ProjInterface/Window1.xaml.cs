using System ;
using System.Diagnostics ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Input ;
using System.Windows.Interop ;
using Autofac ;
using Autofac.Features.Metadata ;
using AvalonDock.Layout ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
using NLog ;

namespace ProjInterface
{
    [ TitleMetadata ( "Docking window" ) ]
    public partial class Window1 : AppWindow , IView1 , IView < DockWindowViewModel >
    {
        private string              _viewTitle = "Docking window" ;
        private DockWindowViewModel _viewModel ;

        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public Window1 ( ) { InitializeComponent ( ) ; }

        public Window1 ( ILifetimeScope lifetimeScope , DockWindowViewModel viewModel ) :
            base ( lifetimeScope )
        {
            ViewModel = viewModel ;
            var wih = new WindowInteropHelper ( this ) ;
            var hWnd = wih.Handle ;
            viewModel.SethWnd ( hWnd ) ;
            InitializeComponent ( ) ;
        }

        #region Overrides of FrameworkElement
        public override async void OnApplyTemplate ( )
        {
            base.OnApplyTemplate ( ) ;
            if ( _viewModel != null )
            {
                _viewModel.ResourcesElement = this ;
                await _viewModel.LoginSilentAsync ( ).ConfigureAwait ( true ) ;
            }
        }
        #endregion
        #region Implementation of IView1
        public string ViewTitle { get { return _viewTitle ; } set { _viewTitle = value ; } }
        #endregion

        #region Implementation of IView<out DockWindowViewModel>
        public DockWindowViewModel ViewModel
        {
            get { return _viewModel ; }
            set { _viewModel = value ; }
        }
        #endregion

        private void CommandBinding_OnExecuted ( object sender , ExecutedRoutedEventArgs e )
        {
            Logger.Warn ( nameof ( CommandBinding_OnExecuted ) ) ;
            if ( e.Parameter is Meta < Lazy < IView1 > > meta )
            {
                try
                {
                    var val = meta.Value.Value ;
                    if ( val is Window w )
                    {
                        w.Show ( ) ;
                    }
                    else if ( val is Control c )
                    {
                        var doc = new LayoutDocument { Content = c } ;
                        doc.Title = val.ViewTitle ;
                        docpane.Children.Add ( doc ) ;
                    }
                }
                catch ( Exception ex )
                {
                    Debug.WriteLine ( ex.ToString ( ) ) ;
                }
            }
        }

        private void CommandBinding_OnExecuted2 ( object sender , ExecutedRoutedEventArgs e )
        {
            System.Windows.Application.Current.Shutdown ( ) ;
        }

        private async void CommandBinding_OnExecuted3 ( object sender , ExecutedRoutedEventArgs e )
        {
            await ViewModel.Login ( ) ;
        }
    }

    public static class InputBindingsManager
    {
        public static readonly DependencyProperty UpdatePropertySourceWhenEnterPressedProperty =
            DependencyProperty.RegisterAttached (
                                                 "UpdatePropertySourceWhenEnterPressed"
                                               , typeof ( DependencyProperty )
                                               , typeof ( InputBindingsManager )
                                               , new PropertyMetadata (
                                                                       null
                                                                     , OnUpdatePropertySourceWhenEnterPressedPropertyChanged
                                                                      )
                                                ) ;

        static InputBindingsManager ( ) { }

        public static void SetUpdatePropertySourceWhenEnterPressed (
            DependencyObject   dp
          , DependencyProperty value
        )
        {
            dp.SetValue ( UpdatePropertySourceWhenEnterPressedProperty , value ) ;
        }

        public static DependencyProperty
            GetUpdatePropertySourceWhenEnterPressed ( DependencyObject dp )
        {
            return ( DependencyProperty ) dp.GetValue (
                                                       UpdatePropertySourceWhenEnterPressedProperty
                                                      ) ;
        }

        private static void OnUpdatePropertySourceWhenEnterPressedPropertyChanged (
            DependencyObject                   dp
          , DependencyPropertyChangedEventArgs e
        )
        {
            var element = dp as UIElement ;

            if ( element == null )
            {
                return ;
            }

            if ( e.OldValue != null )
            {
                element.PreviewKeyDown -= HandlePreviewKeyDown ;
            }

            if ( e.NewValue != null )
            {
                element.PreviewKeyDown += HandlePreviewKeyDown ;
            }
        }

        private static void HandlePreviewKeyDown ( object sender , KeyEventArgs e )
        {
            if ( e.Key == Key.Enter )
            {
                DoUpdateSource ( e.Source ) ;
            }
        }

        private static void DoUpdateSource ( object source )
        {
            var property = GetUpdatePropertySourceWhenEnterPressed ( source as DependencyObject ) ;
            if ( property == null )
            {
                return ;
            }

            var elt = source as UIElement ;
            if ( elt == null )
            {
                return ;
            }

            BindingOperations.GetBindingExpression ( elt , property )?.UpdateSource ( ) ;
        }
    }
}