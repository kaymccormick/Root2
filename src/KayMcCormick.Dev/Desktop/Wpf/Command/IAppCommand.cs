#region header
// Kay McCormick (mccor)
// 
// Analysis
// KayMcCormick.Lib.Wpf
// IAppCommand.cs
// 
// 2020-04-08-5:38 AM
// 
// ---
#endregion
using System.Windows.Input ;
using KayMcCormick.Dev ;

namespace KayMcCormick.Lib.Wpf.Command
{
    /// <summary>
    /// An interface to an "Application command" provided by the application.
    /// </summary>
    public interface IAppCommand: IBaseLibCommand
    {
        /// <summary>
        /// Access to the ICommand interface provided by the command.
        /// </summary>
        ICommand Command { get ; }
    }
}