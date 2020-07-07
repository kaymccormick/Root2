using System.Windows;

namespace AnalysisControls
{
    public static class ViewProperties
    {
        public static readonly DependencyProperty ViewNameProperty = DependencyProperty.RegisterAttached(
            "ViewName", typeof(string), typeof(ViewProperties), new PropertyMetadata(default(string)));

        public static void SetViewName(DependencyObject d, string viewName)
        {
            d.SetValue(ViewNameProperty, viewName);
        }

        public static string GetViewName(DependencyObject d)
        {
            return (string) d.GetValue(ViewNameProperty);
        }
    }
}