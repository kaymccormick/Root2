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
using System.Windows ;
using System.Windows.Controls ;

namespace KayMcCormick.Lib.Wpf
{
#pragma warning disable DV2002 // Unmapped types
    public class CustomDataTemplateSelector : DataTemplateSelector
#pragma warning restore DV2002 // Unmapped types
    {
        #region Overrides of DataTemplateSelector
        public override DataTemplate SelectTemplate ( object item , DependencyObject container )
        {
            Func < object , DependencyObject, DataTemplate > baseFunc = base.SelectTemplate ;
            var template = TemplateSelectorHelper.HelpSelectDataTemplate ( item , container , baseFunc ) ;
            return template ;
        }
        #endregion
    }
}