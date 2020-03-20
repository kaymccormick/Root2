using System.Windows ;
using System.Windows.Controls ;
using NLog ;

namespace KayMcCormick.Lib.Wpf
{
    public class ResourceDetailTemplateSelector : DataTemplateSelector

    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Overrides of DataTemplateSelector
        public override DataTemplate SelectTemplate ( object item , DependencyObject container )
        {
            if ( item == null )
            {
                Logger.Warn(
                             "Selecting detail template for NuLL item"
                            );
            }
            else
            {
                Logger.Debug (
                              "Selecting detail template for {item} {itemType}"
                            , item.ToString()
                            , item.GetType ( )
                             ) ;
            }

            FrameworkElement fe = (FrameworkElement)container;
            object resourceKey = null ;
            DataTemplate returnVal = null ;
            if ( item is ControlWrap < Window > )
            {
                resourceKey = "WindowTemplate" ;
                
            }
            if ( item is ResourceNodeInfo r)
            {
                if ( r.TemplateKey != null )
                {
                    resourceKey = r.TemplateKey ;
                }
            }

            if ( resourceKey != null )
            {
                returnVal = TryFindDataTemplate(fe, resourceKey);
            }
            if ( returnVal == null )
            {
                Logger.Info ( "Calling base method for template" ) ;
                returnVal = base.SelectTemplate(item, container);
                if ( returnVal != null )
                {
                    Logger.Info ( "Got template from base method" ) ;
                }
                else
                {
                    Logger.Info ( "no template from base method" ) ;
                }
            }

            return returnVal ;
        }

        private DataTemplate TryFindDataTemplate ( FrameworkElement fe, object resourceKey )
        {
            Logger.Debug (
                          "Trying to find data template with resource key {resourceKey}"
                        , resourceKey
                         ) ;
            var resource = fe.TryFindResource ( resourceKey ) ;
            Logger.Debug ( "Found resource of type {resourceType}" , resource.GetType ( ) ) ;
            if ( resource is HierarchicalDataTemplate )
            {
                Logger.Debug ( "suppressing heirarchicala data template" ) ;
                return null;
            }
            return resource as DataTemplate ;
        }
        #endregion
    }
}