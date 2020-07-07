#region header
// Kay McCormick (mccor)
// 
// Analysis
// KayMcCormick.Lib.Wpf
// LambdaAppCommand.cs
// 
// 2020-04-08-5:42 AM
// 
// ---
#endregion
using System ;
using System.ComponentModel;
using System.Threading.Tasks ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Command ;

namespace KayMcCormick.Lib.Wpf.Command
{
    /// <summary>
    /// </summary>
    [TypeConverter(typeof(LacConverter))]
    public sealed class LambdaAppCommand : AppCommand
    {
        private readonly Func<LambdaAppCommand, object, Task<IAppCommandResult>> _commandFuncWithArg;
        private object                                                 _argument ;
        public override string ToString()
        {
            return "Lambda [ " + DisplayName + " ]";
        }

        private readonly Func < LambdaAppCommand , Task < IAppCommandResult > > _commandFunc ;
        private readonly Action < AggregateException >                          _onFaultDelegate ;
        private readonly Func<LambdaAppCommand, object, bool> _canExecuteFunc;

        /// <summary>
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="commandFunc"></param>
        /// <param name="argument"></param>
        /// <param name="onFaultDelegate"></param>
        public LambdaAppCommand (
            string                                                 displayName
          , Func < LambdaAppCommand , Task < IAppCommandResult > > commandFunc
          , object                                                 argument
          , Action < AggregateException >                          onFaultDelegate = null
            , Func<LambdaAppCommand, object, bool> canExecuteFunc = null
        ) : base ( displayName )
        {
            _commandFunc     = commandFunc ;
            _argument        = argument ;
            _onFaultDelegate = onFaultDelegate ;
            _canExecuteFunc = canExecuteFunc;
        }
        public LambdaAppCommand(
            string displayName
          , Func<LambdaAppCommand,object, Task<IAppCommandResult>> commandFuncWithArg
          , object argument
          , Action<AggregateException> onFaultDelegate = null
            , Func<LambdaAppCommand, object, bool> canExecuteFunc = null
        ) : base(displayName)
        {
            _commandFuncWithArg = commandFuncWithArg;
            _argument = argument;
            _onFaultDelegate = onFaultDelegate;
            _canExecuteFunc = canExecuteFunc;
        }

        /// <summary>
        /// </summary>
        [Browsable(false)]
        public Func < LambdaAppCommand , Task < IAppCommandResult > > CommandFunc
        {
            get { return _commandFunc ; }
        }

        /// <summary>
        /// </summary>
        public override object Argument
        {
            get { return _argument ; }
            set { _argument = value ; }
        }

        #region Overrides of AppCommand

        /// <summary>
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public override async Task<IAppCommandResult> ExecuteAsync(object parameter)
        {
            DebugUtils.WriteLine ( nameof(ExecuteAsync)) ;
            Task<IAppCommandResult> task;
            IAppCommandResult result1;
            try
            {
                if (_commandFuncWithArg != null)
                {
                    result1 = await _commandFuncWithArg(this, parameter);
                }
                else
                {
                    result1 = await (CommandFunc(this));
                }
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine(ex.ToString());
                return AppCommandResult.Faulted(ex as AggregateException);
            }
            // return  task.ContinueWith(
                                                     // task2 => {
                                                         // if ( task2.IsFaulted )
                                                         // {
                                                             // DebugUtils.WriteLine (
                                                                                   // task2.Exception?.ToString ( )
                                                                                  // ) ;
                                                         // }

                                                         // return task2.Result ;
                                                     // }) ;
                                                     DebugUtils.WriteLine("Command completed");
                                                     return result1;

        }

        /// <summary>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanExecute(object parameter)
        {
            return _canExecuteFunc == null ? true : _canExecuteFunc(this, parameter);
        }

        /// <summary>
        /// </summary>
        /// <param name="exception"></param>
        public override void OnFault ( AggregateException exception )
        {
            OnFaultDelegate?.Invoke ( exception ) ;
        }

        /// <summary>
        /// </summary>
        [Browsable(false)]
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
}