using System ;
using System.Diagnostics ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows.Input ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAppCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IAppCommandResult> ExecuteAsync ( ) ;
        /// <summary>
        /// 
        /// </summary>
        ICommand Command { get ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        void OnFault ( AggregateException exception ) ;
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IAppCommandResult
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsSuccess { get ; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class AppCommandResult : IAppCommandResult
    {
        private bool _isSuccess ;

        /// <summary>
        /// 
        /// </summary>
        public static IAppCommandResult Failed =
            new AppCommandResult ( ) { IsSuccess = false } ;

        /// <summary>
        /// 
        /// </summary>
        public static IAppCommandResult Success = new AppCommandResult ( ) { IsSuccess = true } ;
        private Exception _exception ;
        #region Implementation of IAppCommandResult
        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess { get { return _isSuccess ; } set { _isSuccess = value ; } }
        #endregion

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskException"></param>
        /// <returns></returns>
        public static AppCommandResult Faulted ( AggregateException taskException ) => new AppCommandResult { IsSuccess = false , Exception = taskException } ;

        /// <summary>
        /// 
        /// </summary>
        public Exception Exception { get { return _exception ; } set { _exception = value ; } }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IDisplayableAppCommand : IAppCommand , IDisplayable
    {

    }
    /// <summary>
    /// 
    /// </summary>
    public abstract class AppCommand :  IDisplayableAppCommand, ICommand

    {
        private string _displayName ;
        private IAppCommandResult _lastResult ;
        private Task < IAppCommandResult > _lastTask ;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayName"></param>
        protected AppCommand ( string displayName ) { _displayName = displayName ; }
        #region Implementation of IDisplayable
        /// <summary>
        /// 
        /// </summary>
        public string DisplayName { get { return _displayName ; } set { _displayName = value ; } }
        #endregion
        #region Implementation of IAppCommand
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Task < IAppCommandResult > ExecuteAsync ( ) ;

        /// <summary>
        /// 
        /// </summary>
        public ICommand Command
        {
            get { return this ; }
        }
        #endregion
        #region Implementation of ICommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public abstract bool CanExecute ( object parameter ) ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public virtual void Execute ( object parameter )
        {
            LastTask = ExecuteAsync ( )
               .ContinueWith (
                              ( task , state ) => {
                                  AppCommand appCommand = ( AppCommand ) state ;
                                  if ( task.IsFaulted )
                                  {
                                      Debug.WriteLine ( task.Exception ) ;
                                      appCommand.OnFault ( task.Exception ) ;
                                      return AppCommandResult.Faulted ( task.Exception ) ;
                                  }
                                  else
                                  {

                                      appCommand.OnResult ( task.Result ) ;
                                      return task.Result ;
                                  }
                              }
                            , this
                            , CancellationToken.None
                            , TaskContinuationOptions.None
                            , TaskScheduler.FromCurrentSynchronizationContext ( )
                             ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public Task < IAppCommandResult > LastTask { get { return _lastTask ; } set { _lastTask = value ; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public abstract void OnFault ( AggregateException exception ) ;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        protected virtual void OnResult ( IAppCommandResult result ) { LastResult = result ; }

        /// <summary>
        /// 
        /// </summary>
        public IAppCommandResult LastResult { get { return _lastResult ; } set { _lastResult = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CanExecuteChanged ;
        #endregion
        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnCanExecuteChanged ( )
        {
            CanExecuteChanged?.Invoke ( this , EventArgs.Empty ) ;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LambdaAppCommand : AppCommand
    {
        private Func < LambdaAppCommand , IAppCommandResult > _commandFunc ;
        private readonly object _argument ;
        private Action<AggregateException> _onFaultDelegate ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="commandFunc"></param>
        /// <param name="argument"></param>
        /// <param name="onFaultDelegate"></param>
        public LambdaAppCommand (
            string                                        displayName
          , Func < LambdaAppCommand , IAppCommandResult > commandFunc
          , object                                        argument
            , Action<AggregateException> onFaultDelegate = null
        ) : base ( displayName )
        {
            _commandFunc = commandFunc ;
            _argument = argument ;
            _onFaultDelegate = onFaultDelegate ;
        }

        /// <summary>
        /// 
        /// </summary>
        public Func < LambdaAppCommand, IAppCommandResult > CommandFunc
        {
            get { return _commandFunc ; }
            set { _commandFunc = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public object Argument { get { return _argument ; } }

        #region Overrides of AppCommand
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override async Task < IAppCommandResult > ExecuteAsync ( ) => CommandFunc (this ) ;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanExecute ( object parameter ) { return true ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public override void OnFault ( AggregateException exception )
        {
            OnFaultDelegate?.Invoke ( exception ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<AggregateException> OnFaultDelegate { get { return _onFaultDelegate ; } set { _onFaultDelegate = value ; } }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IDisplayable
    {
        /// <summary>
        /// 
        /// </summary>
        string DisplayName { get ; }
    }
}
