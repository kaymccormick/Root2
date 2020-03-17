using System ;
using System.Linq ;
using System.Windows ;
using System.Windows.Controls ;
using NLog ;

namespace KayMcCormick.Lib.Wpf
{
    public class ResourceTreeViewItemTemplateSelector : DataTemplateSelector

    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        #region Overrides of DataTemplateSelector
        public override DataTemplate SelectTemplate ( object item , DependencyObject container )
        {
            Logger.Debug ( "item is {item}" , item ) ;
            var fe = ( FrameworkElement ) container ;

            if ( item is ResourceNodeInfo node )
            {
                var dType = node.Data.GetType ( ) ;
                var dataTemplateKey = new DataTemplateKey(dType) ;
                Logger.Debug ( "trying key {key}" , dataTemplateKey ) ;
                var o = fe.TryFindResource ( dataTemplateKey) ;
                if ( o != null
                     && o is DataTemplate dt1 )
                {
                    Logger.Debug("found key {key}", dataTemplateKey);
                    return dt1 ;
                }

                var dt = dType.GetInterfaces ( )
                              .Select ( x => Tuple.Create ( x , fe , fe.TryFindResource ( x ) ) )
                              .Where ( Predicate )
                              .Select ( tuple => tuple.Item3 )
                              .OfType < DataTemplate > ( )
                              .FirstOrDefault ( ) ;
                if ( dt != null )
                {
                    return dt ;
                }
            }

            var tryFindResource =
                fe.TryFindResource ( new DataTemplateKey ( typeof ( ResourceNodeInfo ) ) ) ;
            var dt2 = tryFindResource as DataTemplate ;
            return dt2 ;
        }

        private static bool Predicate(Tuple<Type, FrameworkElement, object> arg)
        {
            var (item1 , item2 , item3) = arg ;
            Logger.Debug ( "{type} {fe} {resource}" , item1 , item2.ToString() , item3?.ToString ( ) ) ;
            return true ;
        }

        private bool Predicate(DataTemplate arg)
        {
            Logger.Debug ( arg.ToString ) ;
            
            return true ;
        }
        #endregion
    }
}