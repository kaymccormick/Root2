using System ;
using System.Diagnostics ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows.Input ;

namespace KayMcCormick.Lib.Wpf
{
    public interface IAppCommand
    {
        Task<IAppCommandResult> ExecuteAsync ( ) ;
        ICommand Command { get ; }

        void OnFault ( AggregateException exception ) ;
    }

    public interface IAppCommandResult
    {
        bool IsSuccess { get ; }
    }

    public class AppCommandResult : IAppCommandResult
    {
        private bool _isSuccess ;

        public static IAppCommandResult Failed =
            new AppCommandResult ( ) { IsSuccess = false } ;

        public static IAppCommandResult Success = new AppCommandResult ( ) { IsSuccess = true } ;
        private Exception _exception ;
        #region Implementation of IAppCommandResult
        public bool IsSuccess { get { return _isSuccess ; } set { _isSuccess = value ; } }
        #endregion

        
        public static AppCommandResult Faulted ( AggregateException taskException ) => new AppCommandResult { IsSuccess = false , Exception = taskException } ;

        public Exception Exception { get { return _exception ; } set { _exception = value ; } }
    }

    public interface IDisplayableAppCommand : IAppCommand , IDisplayable
    {

    }
    public abstract class AppCommand :  IDisplayableAppCommand, ICommand

    {
        private string _displayName ;
        private IAppCommandResult _lastResult ;
        private Task < IAppCommandResult > _lastTask ;
        protected AppCommand ( string displayName ) { _displayName = displayName ; }
        #region Implementation of IDisplayable
        public string DisplayName { get { return _displayName ; } set { _displayName = value ; } }
        #endregion
        #region Implementation of IAppCommand
        public abstract Task < IAppCommandResult > ExecuteAsync ( ) ;

        public ICommand Command
        {
            get { return this ; }
        }
        #endregion
        #region Implementation of ICommand
        public abstract bool CanExecute ( object parameter ) ;

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

        public Task < IAppCommandResult > LastTask { get { return _lastTask ; } set { _lastTask = value ; } }

        public abstract void OnFault ( AggregateException exception ) ;
        
        protected virtual void OnResult ( IAppCommandResult result ) { LastResult = result ; }

        public IAppCommandResult LastResult { get { return _lastResult ; } set { _lastResult = value ; } }

        public event EventHandler CanExecuteChanged ;
        #endregion
        protected virtual void OnCanExecuteChanged ( )
        {
            CanExecuteChanged?.Invoke ( this , EventArgs.Empty ) ;
        }
    }

    public class LambdaAppCommand : AppCommand
    {
        private Func < LambdaAppCommand , IAppCommandResult > _commandFunc ;
        private readonly object _argument ;
        private Action<AggregateException> _onFaultDelegate ;

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

        public Func < LambdaAppCommand, IAppCommandResult > CommandFunc
        {
            get { return _commandFunc ; }
            set { _commandFunc = value ; }
        }

        public object Argument { get { return _argument ; } }

        #region Overrides of AppCommand
        public override async Task < IAppCommandResult > ExecuteAsync ( ) => CommandFunc (this ) ;
        public override bool CanExecute ( object parameter ) { return true ; }

        public override void OnFault ( AggregateException exception )
        {
            OnFaultDelegate?.Invoke ( exception ) ;
        }

        public Action<AggregateException> OnFaultDelegate { get { return _onFaultDelegate ; } set { _onFaultDelegate = value ; } }
        #endregion
    }

    public interface IDisplayable
    {
        string DisplayName { get ; }
    }
}
