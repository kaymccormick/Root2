#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Dev
// AppFileTarget.cs
// 
// 2020-03-24-4:57 PM
// 
// ---
#endregion
using NLog ;
using NLog.Targets ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class AppFileTarget : FileTarget
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public AppFileTarget ( string name ) : base ( name )
        {
            ConcurrentWrites = false ;
            KeepFileOpen = true ;
        }
        
    }
}