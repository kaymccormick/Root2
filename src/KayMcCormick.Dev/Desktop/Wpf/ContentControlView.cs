using System.Windows.Controls ;
using System.Windows.Markup ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    [ContentProperty(nameof(Content))]
    public class ContentControlView : ContentControl, IControlView 
    {
    }
}