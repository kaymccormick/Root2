﻿using System;
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
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf.ViewModel ;

namespace KayMcCormick.Lib.Wpf.View
{
    /// <summary>
    /// Interaction logic for EventLogView.xaml
    /// </summary>
    public partial class EventLogView : UserControl, IView1, IView<EventLogViewModel>, IControlView
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        public EventLogView ( EventLogViewModel viewModel )
        {
            ViewModel = viewModel ;
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        public EventLogView() : this(null)
        {
        }

        #region Overrides of FrameworkElement
        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate ( )
        {
            ViewModel.View = this ;
        }
        #endregion

        #region Implementation of IView<out EventLogViewModel>
        /// <summary>
        /// 
        /// </summary>
        public EventLogViewModel ViewModel { get ; }
        #endregion
    }
}
