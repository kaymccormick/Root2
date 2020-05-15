using System.Windows ;
using System.Windows.Controls ;
using JetBrains.Annotations ;
using NLog ;

namespace AnalysisControls
{
    // ReSharper disable once UnusedType.Global
    internal static class ToolTips
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        [ NotNull ]
        // ReSharper disable once UnusedMember.Global
        public static UIElement MakeToolTipContent ( [ CanBeNull ] object resource )
        {
            var displayString = string.Empty ;
            // ReSharper disable once UnusedVariable
            var textBlock = new TextBlock { Text = displayString } ;

            var contentPresenter = new ContentControl { Content = resource } ;
            Logger.Info (
                         "Resource is {Resource}{t}"
                       , resource                       ?? "null"
                       , resource?.GetType ( ).FullName ?? "null"
                        ) ;
            contentPresenter.ContentTemplate = resource as DataTemplate ;
            Logger.Info (
                         "{t}"
                       , contentPresenter.ContentTemplate?.DataType?.ToString ( ) ?? "null"
                        ) ;


            return contentPresenter ;
        }
    }
}