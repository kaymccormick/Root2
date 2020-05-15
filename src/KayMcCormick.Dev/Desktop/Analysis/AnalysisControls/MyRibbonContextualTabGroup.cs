using System;
using System.Windows;
using System.Windows.Controls.Ribbon;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MyRibbonContextualTabGroup : RibbonContextualTabGroup, IAppControl
    {

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            RibbonDebugUtils.OnPropertyChanged(this.ToString(), this, e);
        }

        public override string ToString()
        {
            return $"MyContextualTabGroup[<{Header?.GetType().Name??"Null"}>{Header};{DataContext};{ControlId}]";
        }

        /// <inheritdoc />
        public Guid ControlId { get; } = Guid.NewGuid();

        static MyRibbonContextualTabGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyRibbonContextualTabGroup),
                new FrameworkPropertyMetadata(typeof(MyRibbonContextualTabGroup)));

        }

    }
}