using System ;
using System.Diagnostics ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows.Input ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf.Command
{
    /// <summary>
    /// </summary>
    public abstract class AppCommand : IDisplayableAppCommand , ICommand

    {
        private readonly string                     _displayName ;
        // ReSharper disable once NotAccessedField.Local
        private IAppCommandResult          _lastResult ;
        // ReSharper disable once NotAccessedField.Local
        private Task < IAppCommandResult > _lastTask ;

        /// <summary>
        /// </summary>
        /// <param name="displayName"></param>
        protected AppCommand ( string displayName ) { _displayName = displayName ; }

        #region Implementation of IDisplayable
        /// <summary>
        /// </summary>
        public string DisplayName { get { return _displayName ; } }
        #endregion

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// 
        /// </summary>
        protected void OnCanExecuteChanged ( )
        {
            CanExecuteChanged?.Invoke ( this , EventArgs.Empty ) ;
        }

        #region Implementation of IAppCommand
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public abstract Task < IAppCommandResult > ExecuteAsync ( ) ;

        /// <summary>
        /// </summary>
        [ NotNull ] public ICommand Command { get { return this ; } }
        #endregion
        #region Implementation of ICommand
        /// <summary>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public abstract bool CanExecute ( object parameter ) ;

        /// <summary>
        /// </summary>
        /// <param name="parameter"></param>
        public virtual void Execute ( object parameter )
        {
            LastTask = ExecuteAsync ( )
               .ContinueWith (
                              ( task , state ) => {
                                  var appCommand = ( AppCommand ) state ;
                                  if ( task.IsFaulted )
                                  {
                                      Debug.WriteLine ( task.Exception ) ;
                                      appCommand.OnFault ( task.Exception ) ;
                                      return AppCommandResult.Faulted ( task.Exception ) ;
                                  }

                                  appCommand.OnResult ( task.Result ) ;
                                  return task.Result ;
                              }
                            , this
                            , CancellationToken.None
                            , TaskContinuationOptions.None
                            , TaskScheduler.FromCurrentSynchronizationContext ( )
                             ) ;
        }

        /// <summary>
        /// </summary>
        public Task < IAppCommandResult > LastTask
        {
            set { _lastTask = value ; }
        }

        /// <summary>
        /// </summary>
        /// <param name="exception"></param>
        public abstract void OnFault ( AggregateException exception ) ;

        /// <summary>
        /// </summary>
        /// <param name="result"></param>
        protected void OnResult ( IAppCommandResult result ) { LastResult = result ; }

        /// <summary>
        /// </summary>
        public IAppCommandResult LastResult
        {
            set { _lastResult = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract object LargeImageSourceKey { get ; set ; }

        /// <summary>
        /// </summary>
        public event EventHandler CanExecuteChanged ;
        #endregion
    }

    /// <summary>
    /// </summary>
    public sealed class LambdaAppCommand : AppCommand
    {
        private readonly object                                        _argument ;
        private readonly Func < LambdaAppCommand , Task < IAppCommandResult > > _commandFunc ;
        private readonly Action < AggregateException >                 _onFaultDelegate ;

        /// <summary>
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="commandFunc"></param>
        /// <param name="argument"></param>
        /// <param name="onFaultDelegate"></param>
        public LambdaAppCommand (
            string                                        displayName
          , Func < LambdaAppCommand , Task < IAppCommandResult > > commandFunc
          , object                                        argument
          , Action < AggregateException >                 onFaultDelegate = null
        ) : base ( displayName )
        {
            _commandFunc     = commandFunc ;
            _argument        = argument ;
            _onFaultDelegate = onFaultDelegate ;
        }

        /// <summary>
        /// </summary>
        public Func < LambdaAppCommand , Task < IAppCommandResult > > CommandFunc
        {
            get { return _commandFunc ; }
        }

        /// <summary>
        /// </summary>
        public object Argument { get { return _argument ; } }

        #region Overrides of AppCommand
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override async Task < IAppCommandResult > ExecuteAsync ( )
        {
            var r = await CommandFunc ( this ) ;
            return r ;
        }

        /// <summary>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanExecute ( object parameter ) { return true ; }

        /// <summary>
        /// </summary>
        /// <param name="exception"></param>
        public override void OnFault ( AggregateException exception )
        {
            OnFaultDelegate?.Invoke ( exception ) ;
        }

        /// <summary>
        /// </summary>
        public Action < AggregateException > OnFaultDelegate
        {
            get { return _onFaultDelegate ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override object LargeImageSourceKey { get ; set ; }
        #endregion
    }

    /// <summary>
    /// </summary>
    public interface IDisplayable
    {
        /// <summary>
        /// </summary>
        string DisplayName { get ; }
    }
}