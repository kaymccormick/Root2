using System ;
using System.Windows ;
using System.Windows.Controls ;
using NLog ;

namespace AnalysisControls
{
    static internal class ToolTips
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger ( ) ;

        public static UIElement MakeToolTipContent ( object resource )
        {
            var displayString = "" ;
            var textBlock = new TextBlock ( ) { Text = displayString } ;

            var contentPresenter = new ContentControl ( ) { } ;
            contentPresenter.Content = resource ;
            logger
                      .Info (
                             "Resource is {Resource}{t}"
                           , resource                       ?? "null"
                           , resource?.GetType ( ).FullName ?? "null"
                            ) ;
            contentPresenter.ContentTemplate = resource as DataTemplate ;
            logger
                      .Info ( "{t}" , contentPresenter.ContentTemplate?.DataType?.ToString ( ) ?? "null" ) ;
            // toolTipContent.Children.Add ( contentPresenter ) ;
            // toolTipContent.Children.Add ( textBlock ) ;
            try
            {
                //Logger.Info ( "xaml = {xaml}" , XamlWriter.Save ( contentPresenter ) ) ;
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch ( Exception ex )
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
            }

            return contentPresenter ;
        }
    }
}