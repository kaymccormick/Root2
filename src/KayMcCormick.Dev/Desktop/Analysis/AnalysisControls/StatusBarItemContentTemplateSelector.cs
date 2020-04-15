using System ;
using System.Collections ;
using System.Windows ;
using System.Windows.Controls ;
using NLog ;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class StatusBarItemContentTemplateSelector : DataTemplateSelector
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Overrides of DataTemplateSelector
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
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
                        if ( ! ( currentResource.Value is DataTemplate dt ) )
                        {
                            continue ;
                        }

                        if ( ! ( dt.DataType is Type t ) )
                        {
                            continue ;
                        }

                        if ( ! t.IsAssignableFrom ( type ) )
                        {
                            continue ;
                        }

                        var currentClassLogger = logger ;
                        currentClassLogger.Info (
                                                 $"{currentResource.Key} is candidate"
                                                ) ;
                        return dt ;
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