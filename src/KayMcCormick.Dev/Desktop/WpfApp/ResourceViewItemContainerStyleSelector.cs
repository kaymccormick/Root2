using System.Windows ;
using System.Windows.Controls ;

namespace WpfApp
{
    public class ResourceViewItemContainerStyleSelector : StyleSelector
    {
        #region Overrides of StyleSelector
        public override Style SelectStyle ( object item , DependencyObject container )
        {
            if(item is ResourceNodeInfo i)
            {
                var tryFindResource = ((FrameworkElement)container).TryFindResource("DefaultResourceNodeInfoStyle") ;
                return tryFindResource as Style;
            }
            return base.SelectStyle ( item , container ) ;
        }
        #endregion
    }
}