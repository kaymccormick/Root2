using System.Windows ;
using System.Windows.Controls ;
using KayMcCormick.Dev ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    ///     <para>
    ///         This is the item container style selector for the TreeView that
    ///         hosts the resources tree. The logic is very simple - if the item is
    ///         of type (or ubtype of) ResourceNodeInfo then it uses the style named
    ///         "
    ///     </para>
    ///     <para></para>
    /// </summary>
    /// <seealso cref="System.Windows.Controls.StyleSelector" />
    public class ResourceViewItemContainerStyleSelector : StyleSelector
    {
        #region Overrides of StyleSelector
        /// <summary>
        ///     When overridden in a derived class, returns a
        ///     <see cref="T:System.Windows.Style" /> based on custom logic.
        ///     The logic is very simple it ues a stlye named
        ///     DefaultResourceNodeInfoStyle if the item is f type or subtype
        ///     ResourceNodeInfo
        /// </summary>
        /// <param name="item">The content.</param>
        /// <param name="container">The element to which the style will be applied.</param>
        /// <returns>
        ///     Returns an application-specific style to apply; otherwise,
        ///     <span class="keyword">
        ///         <span class="languageSpecificText">
        ///             <span class="cs">null</span><span class="vb">Nothing</span>
        ///             <span class="cpp">nullptr</span>
        ///         </span>
        ///     </span>
        ///     <span class="nu">
        ///         a null reference (<span class="keyword">Nothing</span>
        ///         in Visual Basic)
        ///     </span>
        ///     .
        /// </returns>
        public override Style SelectStyle ( object item , DependencyObject container )
        {
            if ( ! ( item is ResourceNodeInfo ) )
            {
                return base.SelectStyle ( item , container ) ;
            }

            var tryFindResource =
                ( ( FrameworkElement ) container ).TryFindResource (
                                                                    "DefaultResourceNodeInfoStyle"
                                                                   ) ;
            return tryFindResource as Style ;

        }
        #endregion
    }
}