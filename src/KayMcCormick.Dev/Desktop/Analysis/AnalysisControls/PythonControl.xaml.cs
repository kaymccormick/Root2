﻿using System.Diagnostics ;
using System.Windows.Controls ;
using System.Windows.Input ;
using AnalysisControls.ViewModel ;
using Autofac ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Lib.Wpf ;

namespace AnalysisControls
{
    /// <summary>
    /// Interaction logic for PythonControl.xaml
    /// </summary>
    [TitleMetadata("Python")]
    public partial class PythonControl : UserControl , IView < PythonViewModel >, IView1, IControlView
    {
        private readonly ILifetimeScope _scope ;
        private PythonViewModel _viewModel ;

        public PythonControl ( ) : this(null, null) {
        }

        public PythonControl (ILifetimeScope scope, PythonViewModel viewModel )
        {
            _scope = scope ;
            _viewModel = viewModel ;
            InitializeComponent ( ) ;
            
            _viewModel.FlowDOcument = flow ;
        }

        private void UIElement_OnKeyDown ( object sender , KeyEventArgs e )
        {
            if ( e.Key == Key.Enter )
            {
                Debug.WriteLine ( "rceived key " + e.Key ) ;
                var textBox = ( ( TextBox ) sender ) ;
                ViewModel.TakeLine ( textBox.Text ) ;
                textBox.Text = "" ;
            } else if ( e.Key == Key.Up )
            {
                ViewModel.HistoryUp ( ) ;
            }
        }

        #region Implementation of IView<out PythonViewModel>
        public PythonViewModel ViewModel { get { return _viewModel ; } }
        #endregion

        private void UIElement_OnPreviewKeyDown ( object sender , KeyEventArgs e )
        {
            Debug.WriteLine("rceived key " + e.Key);
            if ( e.Key == Key.Up )
            {
                ViewModel.HistoryUp();
                e.Handled = true ;
            } else if ( e.Key == Key.Down )
            {
                ViewModel.HistoryDown ( ) ;
                e.Handled = true ;
            }

        }
    }
}