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
using System.Diagnostics ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows.Input ;

namespace KayMcCormick.Lib.Wpf.Command
{
    /// <summary>
    /// </summary>
    public class WrappedAppCommand : IAppCommand , ICommand
    {
        private readonly IHandleException _handleException ;
        private readonly IAppCommand      _wrappedCommand ;

        /// <summary>
        /// </summary>
        /// <param name="wrappedCommand"></param>
        /// <param name="handleException"></param>
        public WrappedAppCommand ( IAppCommand wrappedCommand , IHandleException handleException )
        {
            _wrappedCommand  = wrappedCommand ;
            _handleException = handleException ;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public async Task < IAppCommandResult > ExecuteAsync ( )
        {
            Debug.WriteLine ( "Executinng command" ) ;
            IAppCommandResult r = null ;
            try
            {
                r = await _wrappedCommand.ExecuteAsync ( )
                                         .ContinueWith (
                                                        ( task , o ) => {
                                                            return ( IAppCommandResult )
                                                                AppCommandResult.Cancelled ;
                                                        }
                                                      , this
                                                      , CancellationToken.None
                                                      , TaskContinuationOptions.NotOnRanToCompletion
                                                      , TaskScheduler
                                                           .FromCurrentSynchronizationContext ( )
                                                       ) ;

            }
            catch ( Exception ex )
            {
                Debug.WriteLine (ex.ToString()  );
            }

            Debug.WriteLine ( "Complete" ) ;
            return r ;
        }

        /// <summary>
        /// </summary>
        public ICommand Command { get { return this ; } }

        /// <summary>
        /// </summary>
        /// <param name="exception"></param>
        public void OnFault ( AggregateException exception )
        {
            _handleException?.HandleException ( exception ) ;
        }

        #region Implementation of ICommand
        /// <summary>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute ( object parameter )
        {
            return _wrappedCommand.Command.CanExecute ( parameter ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute ( object parameter ) { ExecuteAsync ( ) ; }

        /// <summary>
        /// </summary>
        public event EventHandler CanExecuteChanged ;
        #endregion
    }
}