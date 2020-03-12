using System.Linq ;
using System.Windows ;
using System.Windows.Controls ;
using NLog ;

namespace WpfApp
{
    public class ResourceTreeViewItemTemplateSelector : DataTemplateSelector

    {
        public ResourceTreeViewItemTemplateSelector ( ) { }

        #region Overrides of DataTemplateSelector
        public override DataTemplate SelectTemplate ( object item , DependencyObject container )
        {
            var fe = ( FrameworkElement ) container ;

            var node = item as ResourceNodeInfo ;
            if ( node != null )
            {
                var o = fe.TryFindResource ( node.Data.GetType ( ) ) ;
                if ( o != null
                     && o is DataTemplate dt1 )
                {
                    return dt1 ;
                }

                var dt = node.Data.GetType ( )
                             .GetInterfaces ( )
                             .Select ( x => fe.TryFindResource ( x ) )
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
        #endregion
    }
}