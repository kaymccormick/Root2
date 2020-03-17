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
                var tryFindResource = ( ( FrameworkElement ) container ).TryFindResource ( typeof ( ISymbol ) ) ;
                return ( DataTemplate ) tryFindResource ;
            }
            return base.SelectTemplate ( item , container ) ;
        }
        #endregion
    }
}