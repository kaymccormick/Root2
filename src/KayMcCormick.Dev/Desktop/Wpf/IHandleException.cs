#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Lib.Wpf
// IHandleException.cs
// 
// 2020-03-22-7:14 AM
// 
// ---
#endregion
using System ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHandleException
    {
        void HandleException ( Exception exception ) ;
    }
}