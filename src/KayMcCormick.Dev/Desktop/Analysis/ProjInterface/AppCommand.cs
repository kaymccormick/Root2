using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input ;

namespace ProjInterface
{
    public interface IAppCommand
    {
        Task<IAppCommandResult> ExecuteAsync ( ) ;
        ICommand Command { get ; }
    }

    public interface IAppCommandResult
    {
        bool IsSuccess { get ; }
    }

    class AppCommandResult : IAppCommandResult
    {
        private bool _isSuccess ;

        public static IAppCommandResult Failed =
            new AppCommandResult ( ) { IsSuccess = false } ;

        public static IAppCommandResult Success = new AppCommandResult ( ) { IsSuccess = true } ;
        #region Implementation of IAppCommandResult
        public bool IsSuccess { get { return _isSuccess ; } set { _isSuccess = value ; } }
        #endregion
    }

    public interface IDisplayableAppCommand : IAppCommand , IDisplayable
    {

    }
    public abstract class AppCommand :  IDisplayableAppCommand, ICommand

    {
        private string _displayName ;
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

        public void Execute ( object parameter ) { ExecuteAsync ( ) ; }

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

        public LambdaAppCommand (
            string                                        displayName
          , Func < LambdaAppCommand , IAppCommandResult > commandFunc
          , object                                        argument
        ) : base ( displayName )
        {
            _commandFunc = commandFunc ;
            _argument = argument ;
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
        #endregion
    }

    public interface IDisplayable
    {
        string DisplayName { get ; }
    }
}
