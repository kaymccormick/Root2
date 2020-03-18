using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Text ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Documents ;
using System.Windows.Input ;
using System.Windows.Media ;
using System.Windows.Media.Imaging ;
using System.Windows.Shapes ;
using Autofac ;
using Autofac.Features.Metadata ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.Build.Logging.StructuredLogger ;
using ProjLib.Interfaces ;
using Xceed.Wpf.AvalonDock.Layout ;

namespace ProjInterface
{
    [ TitleMetadata ( "Window 1" ) ]
    public partial class Window1 : AppWindow , IView1 , IView < DockWindowViewModel >
    {
        private string              _viewTitle = "window 1" ;
        private DockWindowViewModel _viewModel ;

        public Window1 ( ) { InitializeComponent ( ) ; }

        public Window1 ( ILifetimeScope lifetimeScope , DockWindowViewModel viewModel ) :
            base ( lifetimeScope )
        {
            ViewModel = viewModel ;

            InitializeComponent ( ) ;
        }

        #region Overrides of FrameworkElement
        public override void OnApplyTemplate ( )
        {
            base.OnApplyTemplate ( ) ;
            if (_viewModel != null)
            {
                _viewModel.ResourcesElement = this;
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
            set
            {
                _viewModel = value ;
            }
        }
        #endregion

        private void CommandBinding_OnExecuted ( object sender , ExecutedRoutedEventArgs e )
        {
            if ( e.Parameter is Meta < Lazy < IView1 > > meta )
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

//            MessageBox.Show ( e.Parameter.ToString ( ) ) ;
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