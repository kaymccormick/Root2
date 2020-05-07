#region header
// Kay McCormick (mccor)
// 
// Analysis
// KayMcCormick.Lib.Wpf
// IDisplayableAppCommand.cs
// 
// 2020-04-08-5:38 AM
// 
// ---
#endregion

using KayMcCormick.Dev;

namespace KayMcCormick.Lib.Wpf.Command
{
    /// <summary>
    /// A "Displayable" app command. Subinterface of <see cref="IAppIAppCommand"/> <see cref="IDisplayable"/>
    /// </summary>
    public interface IDisplayableAppCommand : IAppCommand , IDisplayable
    {
        /// <summary>
        /// 
        /// </summary>
        object LargeImageSourceKey { get ; set ; }
        
    }
}