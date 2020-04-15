using System.Windows ;
using System.Windows.Controls ;
using JetBrains.Annotations ;
using NLog ;

namespace AnalysisControls
{
    internal static class ToolTips
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger ( ) ;

        [ NotNull ]
        public static UIElement MakeToolTipContent ( [ CanBeNull ] object resource )
        {
            var displayString = string.Empty ;
            // ReSharper disable once UnusedVariable
            var textBlock = new TextBlock { Text = displayString } ;

            var contentPresenter = new ContentControl { Content = resource } ;
            logger.Info (
                         "Resource is {Resource}{t}"
                       , resource                       ?? "null"
                       , resource?.GetType ( ).FullName ?? "null"
                        ) ;
            contentPresenter.ContentTemplate = resource as DataTemplate ;
            logger.Info (
                         "{t}"
                       , contentPresenter.ContentTemplate?.DataType?.ToString ( ) ?? "null"
                        ) ;


            return contentPresenter ;
        }
    }
}