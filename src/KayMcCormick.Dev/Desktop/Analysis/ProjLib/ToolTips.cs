using System ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Markup ;
using CodeAnalysisApp1 ;
using NLog ;

namespace ProjLib
{
    static internal class ToolTips
    {
        public static UIElement MakeToolTipContent ( out object resource )
        {
            var displayString = """ ;
            var textBlock = new TextBlock ( ) { Text = displayString } ;

            var contentPresenter = new ContentControl ( ) { } ;
            contentPresenter.Content = resource ;
            LogManager.GetCurrentClassLogger ( )
                      .Info (
                             "Resource is {Resource}{t}"
                           , resource                       ?? "null"
                           , resource?.GetType ( ).FullName ?? "null"
                            ) ;
            contentPresenter.ContentTemplate = resource as DataTemplate ;
            LogManager.GetCurrentClassLogger ( )
                      .Info ( "{t}" , contentPresenter.ContentTemplate?.DataType?.ToString ( ) ?? "null" ) ;
            // toolTipContent.Children.Add ( contentPresenter ) ;
            // toolTipContent.Children.Add ( textBlock ) ;
            try
            {
                LogInvocationSpan.Logger.Info ( "xaml = {xaml}" , XamlWriter.Save ( contentPresenter ) ) ;
            }
            catch ( Exception ex )
            {
            }

            return contentPresenter ;
        }
    }
}