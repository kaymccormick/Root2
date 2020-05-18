using System;
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

namespace AnalysisControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls;assembly=AnalysisControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:JsonNodeView/>
    ///
    /// </summary>
    public class JsonNodeView : HeaderedItemsControl
    {
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            "IsExpanded", typeof(bool), typeof(JsonNodeView), new PropertyMetadata(default(bool)));

        public bool IsExpanded
        {
            get { return (bool) GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }
        static JsonNodeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(JsonNodeView), new FrameworkPropertyMetadata(typeof(JsonNodeView)));
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            ChangeVisualState(true);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            ChangeVisualState(true);
        }

        internal void ChangeVisualState(bool useTransitions)
        {
            if (!this.IsEnabled)
                VisualStates.GoToState((Control)this, (useTransitions ? 1 : 0) != 0, "Disabled", "Normal");
            else if (this.IsMouseOver)
                VisualStates.GoToState((Control)this, (useTransitions ? 1 : 0) != 0, "MouseOver", "Normal");
            else
                VisualStates.GoToState((Control)this, (useTransitions ? 1 : 0) != 0, "Normal");
            if (this.IsKeyboardFocused)
                VisualStates.GoToState((Control)this, (useTransitions ? 1 : 0) != 0, "Focused", "Unfocused");
            else
                VisualStates.GoToState((Control)this, (useTransitions ? 1 : 0) != 0, "Unfocused");
            if (this.IsExpanded)
                VisualStates.GoToState((Control)this, (useTransitions ? 1 : 0) != 0, "Expanded");
            else
                VisualStates.GoToState((Control)this, (useTransitions ? 1 : 0) != 0, "Collapsed");
            if (this.HasItems)
                VisualStates.GoToState((Control)this, (useTransitions ? 1 : 0) != 0, "HasItems");
            else
                VisualStates.GoToState((Control)this, (useTransitions ? 1 : 0) != 0, "NoItems");
            if (this.IsSelected)
            {
                if (this.IsSelectionActive)
                    VisualStates.GoToState((Control)this, (useTransitions ? 1 : 0) != 0, "Selected");
                else
                    VisualStates.GoToState((Control)this, (useTransitions ? 1 : 0) != 0, "SelectedInactive", "Selected");
            }
            else
                VisualStates.GoToState((Control)this, (useTransitions ? 1 : 0) != 0, "Unselected");
        }

      public bool IsSelectionActive { get; set; }

        public bool IsSelected { get; set; }
    }
    internal static class VisualStates
    {
        internal const string StateToday = "Today";
        public static void GoToState(Control control, bool useTransitions, params string[] stateNames)
        {
            if (stateNames == null)
                return;
            foreach (string stateName in stateNames)
            {
                if (VisualStateManager.GoToState((FrameworkElement)control, stateName, useTransitions))
                    break;
            }
        }

    }
}
