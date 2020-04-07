#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// ITakesHwnd.cs
// 
// 2020-03-22-7:15 PM
// 
// ---
#endregion
using System ;

namespace ProjInterface
{
    public interface ITakesHwnd
    {
        void SetHwnd ( IntPtr hWnd ) ;
    }
}