#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// WpfApp
// ControlWrap.cs
// 
// 2020-03-12-2:15 AM
// 
// ---
#endregion
using System.Windows ;

namespace KayMcCormick.Lib.Wpf
{
    public class ControlWrap<T> where T : FrameworkElement
    {
        public T Control { get ; }
        public ControlWrap ( T c ) { Control = c ; }
    }
}