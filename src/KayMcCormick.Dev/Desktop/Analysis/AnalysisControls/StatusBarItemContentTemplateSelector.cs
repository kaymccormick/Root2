using System ;
using System.Collections ;
using System.Windows ;
using System.Windows.Controls ;
using NLog ;

namespace AnalysisControls
{
    public class StatusBarItemContentTemplateSelector : DataTemplateSelector
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Overrides of DataTemplateSelector
        public override DataTemplate SelectTemplate ( object item , DependencyObject container )
        {
            try
            {
                if ( item != null )
                {
                    var type = item.GetType ( ) ;

                    var cType = container.GetType ( ) ;
                    foreach ( DictionaryEntry currentResource in Application.Current.Resources )
                    {
                        if ( currentResource.Value is DataTemplate dt )
                        {
                            if ( dt.DataType is Type t )
                            {
                                if ( t.IsAssignableFrom ( type ) )
                                {
                                    var currentClassLogger = logger ;
                                    currentClassLogger.Info (
                                                             $"{currentResource.Key} is candidate"
                                                            ) ;
                                    return dt ;
                                }
                            }
                        }
                    }
                }
            }
            catch ( Exception ex )
            {
                logger.Warn ( ex , ex.ToString ( ) ) ;
            }

            return base.SelectTemplate ( item , container ) ;
        }
        #endregion
    }
}