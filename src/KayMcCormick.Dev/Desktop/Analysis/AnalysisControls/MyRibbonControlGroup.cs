using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Media;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonControlGroup : RibbonControlGroup , IAppControl
    {
        /// <summary>
        /// 
        /// </summary>
        public MyRibbonControlGroup()
        {
            ControlId = Guid.NewGuid();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public Guid ControlId { get; }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new MyRibbonControl();
        }
    }
}