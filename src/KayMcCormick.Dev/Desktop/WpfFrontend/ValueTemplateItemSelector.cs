using System.Windows ;
using System.Windows.Controls ;
using Microsoft.CodeAnalysis ;

namespace AnalysisControls
{
    public class ValueTemplateItemSelector : DataTemplateSelector
    {
        #region Overrides of DataTemplateSelector
        public override DataTemplate SelectTemplate ( object item , DependencyObject container )
        {
            if ( item is ISymbol )
            {
                var tryFindResource = ( container as FrameworkElement ).TryFindResource ( typeof ( ISymbol ) ) ;
                return tryFindResource as
                           DataTemplate ;
            }
            return base.SelectTemplate ( item , container ) ;
        }
        #endregion
    }
}