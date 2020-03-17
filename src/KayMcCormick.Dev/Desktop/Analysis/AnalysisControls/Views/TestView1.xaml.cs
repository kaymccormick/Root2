﻿using System.Windows.Controls ;
using JetBrains.Annotations ;
using KayMcCormick.Lib.Wpf ;

namespace AnalysisControls.Views
{
    [TitleMetadata("TEst View 1")]
    [ UsedImplicitly ]
    public partial class TestView1 : UserControl, IView1
    {
        private string _viewTitle = "Test View 1";

        public TestView1()
        {
            InitializeComponent();
        }

        #region Implementation of IView1
        public string ViewTitle { get => _viewTitle ; set => _viewTitle = value ; }
        #endregion
    }
}
