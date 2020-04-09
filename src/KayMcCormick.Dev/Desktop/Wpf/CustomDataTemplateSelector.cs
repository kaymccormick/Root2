#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Lib.Wpf
// CustomDataTemplateSelector.cs
// 
// 2020-03-22-8:12 AM
// 
// ---
#endregion
using System ;
using System.Diagnostics ;
using System.Text ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Media ;
using KayMcCormick.Dev ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    public class CustomDataTemplateSelector : DataTemplateSelector

    {
        #region Overrides of DataTemplateSelector
        /// <summary>
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate ( object item , DependencyObject container )
        {
            if ( item == null )
            {
                return base.SelectTemplate ( item , container ) ;
            }

            Func < object , DependencyObject , DataTemplate > baseFunc = base.SelectTemplate ;
            var itemRepr = item.ToString ( ) ;
            if ( itemRepr.Length > 40 )
            {
                itemRepr = itemRepr.Substring ( 0 , 40 ) + "..." ;
            }

            itemRepr = itemRepr + $" [{item.GetType ( ).FullName}]" ;
            var containerRepr = new StringBuilder ( container.ToString ( ) ) ;
            if ( container is FrameworkElement fe )
            {
                object name = null ;
                while ( fe != null )
                {
                    name = fe.GetValue ( FrameworkElement.NameProperty ) ;
                    if ( ! string.IsNullOrEmpty ( ( string ) name ) )
                    {
                        break ;
                    }

                    var orig = fe ;
                    var visualParent = ( FrameworkElement ) VisualTreeHelper.GetParent ( fe ) ;
                    //fe = ( FrameworkElement ) LogicalTreeHelper.GetParent ( fe ) ;
                    fe = visualParent ;
                    // fe = ( FrameworkElement ) fe.Parent ;
                    // if ( fe == null )
                    // {
                    // fe = ( FrameworkElement ) orig.TemplatedParent ;
                    // }
                }

                containerRepr.Append ( " " ) ;
                containerRepr.Append ( name ) ;
                if ( fe != null )
                {
                    containerRepr.Append ( fe.GetType ( ).FullName ) ;
                }
            }

            DebugUtils.WriteLine (
                             $"{GetType ( ).FullName} calling TemplateSelectorHelper.HelpSelectDataTemplate with {itemRepr}, {containerRepr} and base.SelectTemplate"
                            ) ;
            var template =
                TemplateSelectorHelper.HelpSelectDataTemplate ( item , container , baseFunc ) ;
            return template ;
        }
        #endregion
    }
}