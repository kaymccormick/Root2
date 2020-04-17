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
using System.Threading.Tasks ;
using System.Windows.Input ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Command ;

namespace KayMcCormick.Lib.Wpf.Command
{
    /// <summary>
    /// </summary>
    public sealed class WrappedAppCommand : IAppCommand , ICommand
    {
        private readonly IHandleException _handleException ;
        private readonly IAppCommand      _wrappedCommand ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wrappedCommand"></param>
        /// <param name="handleException"></param>
        /// <param name="argument"></param>
        public WrappedAppCommand ( IAppCommand wrappedCommand , IHandleException handleException , object argument=null )
        {
            _wrappedCommand  = wrappedCommand ;
            _handleException = handleException ;
            Argument = argument ;
        }

        /// <inheritdoc />
        public object Argument { get ; set ; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ ItemCanBeNull ]
        public  Task < IAppCommandResult > ExecuteAsync ( )
        {
            return _wrappedCommand.ExecuteAsync ( ) ;
            DebugUtils.WriteLine ( "Executing command" ) ;
            // IAppCommandResult r = null ;
            // try
            // {
                // r = await _wrappedCommand.ExecuteAsync ( )
                                         // .ContinueWith (
                                                        // ( task , o ) => AppCommandResult.Cancelled
                                                      // , this
                                                      // , CancellationToken.None
                                                      // , TaskContinuationOptions.None
                                                      // , TaskScheduler
                                                           // .FromCurrentSynchronizationContext ( )
                                                       // ) ;

            // }
            // catch ( Exception ex )
            // {
                // DebugUtils.WriteLine (ex.ToString()  );
            // }

            // DebugUtils.WriteLine ( "Complete" ) ;
            // return r ;
        }

        /// <summary>
        /// </summary>
        [ NotNull ] public ICommand Command { get { return this ; } }

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
        public void Execute ( object parameter ) { ExecuteAsync ( ).Wait() ; }

        /// <summary>
        /// </summary>
        public event EventHandler CanExecuteChanged ;
        #endregion
    }
}