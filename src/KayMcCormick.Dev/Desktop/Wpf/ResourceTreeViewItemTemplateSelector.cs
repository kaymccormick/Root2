using System ;
using System.ComponentModel ;
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
                    Logger.Info("found key {key}", dataTemplateKey);
                    return dt1 ;
                }

                var dt = dType.GetInterfaces ( )
                              .Select ( x => Tuple.Create ( x , fe , fe.TryFindResource ( x ) ) )
                              .Where ( Predicate2)
                              .Where ( Predicate3 )
                              .FirstOrDefault ( ) ;
                if ( dt != null )
                {
                    Logger.Info("using key {key}", dt.Item1);
                    return dt.Item3 as DataTemplate ;
                }
            }

            Logger.Info ( "using key ResourceNodeInfo);" ) ;
            var tryFindResource =
                fe.TryFindResource ( new DataTemplateKey ( typeof ( ResourceNodeInfo ) ) ) ;
            var dt2 = tryFindResource as DataTemplate ;
            return dt2 ;
        }


        private bool Predicate3 (  Tuple < Type , FrameworkElement , object > arg, int i )
        {
            var r = arg.Item3 is DataTemplate ;
            if ( r )
            {
                Logger.Info ( "[{i}] Found Key {key}" , i, arg.Item1.FullName ) ;
            }

            return r ;
        }

        private static bool Predicate2(Tuple<Type, FrameworkElement, object> arg)
        {
            
            var (item1 , item2 , item3) = arg ;
            if ( item1 == typeof ( ISupportInitialize ) || item1.Name == "ISealable")
            {
                return false ;
            }
                Logger.Debug ( "[ {type} ]\t\t\t{fe} {resource}" , item1 , item2.ToString() , item3?.ToString ( ) ) ;
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