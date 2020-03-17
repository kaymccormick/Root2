using System.Windows ;
using System.Windows.Controls ;

namespace KayMcCormick.Lib.Wpf
{
    public class ResourceViewItemContainerStyleSelector : StyleSelector
    {
        #region Overrides of StyleSelector
        public override Style SelectStyle ( object item , DependencyObject container )
        {
            if(item is ResourceNodeInfo)
            {
                var tryFindResource = ((FrameworkElement)container).TryFindResource("DefaultResourceNodeInfoStyle") ;
                return tryFindResource as Style;
            }
            return base.SelectStyle ( item , container ) ;
        }
        #endregion
    }
}