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
using System.Threading.Tasks ;
using KayMcCormick.Dev.Command ;

namespace KayMcCormick.Lib.Wpf.Command
{
    /// <summary>
    /// </summary>
    public sealed class LambdaAppCommand : AppCommand
    {
        private object                                                 _argument ;
        private readonly Func < LambdaAppCommand , Task < IAppCommandResult > > _commandFunc ;
        private readonly Action < AggregateException >                          _onFaultDelegate ;

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
        public override object Argument
        {
            get { return _argument ; }
            set { _argument = value ; }
        }

        #region Overrides of AppCommand
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override Task < IAppCommandResult > ExecuteAsync ( )
        {
            return CommandFunc ( this ) ;
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
}