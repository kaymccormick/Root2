#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Lib.Wpf
// WrappedAppCommand.cs
// 
// 2020-03-22-7:14 AM
// 
// ---
#endregion
using System ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows.Input ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public class WrappedAppCommand : IAppCommand , ICommand
    {
        private readonly IAppCommand      _wrappedCommand ;
        private readonly IHandleException _handleException ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wrappedCommand"></param>
        /// <param name="handleException"></param>
        public WrappedAppCommand (
            IAppCommand      wrappedCommand
          , IHandleException handleException
        )
        {
            _wrappedCommand  = wrappedCommand ;
            _handleException = handleException ;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task < IAppCommandResult > ExecuteAsync ( )
        {
            return _wrappedCommand.ExecuteAsync ( )
                                  .ContinueWith (
                                                 ( task , o ) => {
                                                     OnFault ( task.Exception ) ;
                                                     return (IAppCommandResult)AppCommandResult.Faulted (
                                                                                                         task
                                                                                                            .Exception
                                                                                                        ) ;
                                                 }
                                               , this
                                               , CancellationToken.None
                                               , TaskContinuationOptions.OnlyOnFaulted
                                               , TaskScheduler.FromCurrentSynchronizationContext ( )
                                                ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public ICommand Command => this ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public void OnFault ( AggregateException exception )
        {
            _handleException?.HandleException( exception ) ;
        }

        #region Implementation of ICommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute ( object parameter )
            => _wrappedCommand.Command.CanExecute ( parameter ) ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute ( object parameter ) { ExecuteAsync ( ) ; }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CanExecuteChanged ;
        #endregion
    }
}