using System.Windows ;
using System.Windows.Controls ;

namespace KayMcCormick.Lib.Wpf
{
    public class ResourceDetailTemplateSelector : DataTemplateSelector

    {
        #region Overrides of DataTemplateSelector
        public override DataTemplate SelectTemplate ( object item , DependencyObject container )
        {
            FrameworkElement fe = (FrameworkElement)container;
            if ( item is ControlWrap < Window > )
            {
                return fe.TryFindResource("WindowTemplate") as DataTemplate;
            }
            if ( item is ResourceNodeInfo r)
            {
                if ( r.TemplateKey != null )
                {
                    return fe.TryFindResource ( r.TemplateKey ) as DataTemplate ;
                }
            }
            return base.SelectTemplate ( item , container ) ;
        }
        #endregion
    }
}