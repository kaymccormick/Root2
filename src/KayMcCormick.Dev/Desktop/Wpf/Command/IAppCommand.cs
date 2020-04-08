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
using System ;
using System.Threading.Tasks ;
using System.Windows.Input ;

namespace KayMcCormick.Lib.Wpf.Command
{
    /// <summary>
    /// An interface to an "Application command" provided by the application.
    /// </summary>
    public interface IAppCommand
    {
        /// <summary>
        /// Access to the ICommand interface provided by the command.
        /// </summary>
        ICommand Command { get ; }

        /// <summary>
        /// An Async method to execute the command.
        /// </summary>
        /// <returns></returns>
        Task < IAppCommandResult > ExecuteAsync ( ) ;

        /// <summary>
        /// A method to handle faults.
        /// </summary>
        /// <param name="exception">Exception</param>
        // ReSharper disable once UnusedMemberInSuper.Global
        void OnFault ( AggregateException exception ) ;
    }
}