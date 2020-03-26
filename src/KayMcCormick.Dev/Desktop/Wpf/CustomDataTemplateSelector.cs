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
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{

    public class CustomDataTemplateSelector : DataTemplateSelector

    {
        #region Overrides of DataTemplateSelector
        public override DataTemplate SelectTemplate ( [ NotNull ] object item , DependencyObject container )
        {
            if ( item == null )
            {
                throw new ArgumentNullException ( nameof ( item ) ) ;
            }

            Func < object , DependencyObject, DataTemplate > baseFunc = base.SelectTemplate ;
            var itemRepr = item.ToString ( ) ;
            if ( itemRepr.Length > 40 )
            {
                itemRepr = itemRepr.Substring ( 0 , 40 ) + "..." ;
            }

            itemRepr = itemRepr + $" [{item.GetType ( ).FullName}]" ;
            var containerRepr = new StringBuilder ( container.ToString ( ) ) ;
            if ( container is FrameworkElement fe )
            {
                object name = null;
                while ( fe != null )
                {
                    name = fe.GetValue ( FrameworkElement.NameProperty ) ;
                    if (! String.IsNullOrEmpty(( string ) name)) break ;
                    var orig = fe ;
                    FrameworkElement visualParent = ( FrameworkElement ) VisualTreeHelper.GetParent ( fe ) ;
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
            Debug.WriteLine (
                             $"{GetType ( ).FullName} calling TemplateSelectorHelper.HelpSelectDataTemplate with {itemRepr}, {containerRepr} and base.SelectTemplate"
                            ) ;
            var template = TemplateSelectorHelper.HelpSelectDataTemplate ( item , container , baseFunc ) ;
            return template ;
        }
        #endregion
    }
}