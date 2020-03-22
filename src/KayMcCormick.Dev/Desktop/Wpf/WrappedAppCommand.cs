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
    public class WrappedAppCommand : IAppCommand , ICommand
    {
        private readonly IAppCommand      _wrappedCommand ;
        private readonly IHandleException _handleException ;

        public WrappedAppCommand (
            IAppCommand      wrappedCommand
          , IHandleException handleException
        )
        {
            _wrappedCommand  = wrappedCommand ;
            _handleException = handleException ;
        }
        
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

        public ICommand Command => this ;

        public void OnFault ( AggregateException exception )
        {
            _handleException?.HandleException( exception ) ;
        }

        #region Implementation of ICommand
        public bool CanExecute ( object parameter )
            => _wrappedCommand.Command.CanExecute ( parameter ) ;

        public void Execute ( object parameter ) { ExecuteAsync ( ) ; }

        public event EventHandler CanExecuteChanged ;
        #endregion
    }
}