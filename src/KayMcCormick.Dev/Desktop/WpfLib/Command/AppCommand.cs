using System ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows.Input ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Command ;

namespace KayMcCormick.Lib.Wpf.Command
{
    /// <summary>
    /// </summary>
    public abstract class AppCommand : IDisplayableAppCommand , ICommand

    {
        /// <summary>
        /// </summary>
        /// <param name="displayName"></param>
        protected AppCommand ( string displayName ) { DisplayName = displayName ; }

        #region Implementation of IDisplayable
        /// <summary>
        /// </summary>
        public string DisplayName { get ; }
        #endregion

        // ReSharper disable once UnusedMember.Global
        /// <summary>
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
                                      // ReSharper disable once PossibleNullReferenceException
                                      DebugUtils.WriteLine ( task.Exception.ToString() ) ;
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
        public Task < IAppCommandResult > LastTask { get ; set ; }

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
        public IAppCommandResult LastResult { get ; set ; }

        /// <summary>
        /// </summary>
        public abstract object LargeImageSourceKey { get ; set ; }

        /// <summary>
        /// </summary>
        public abstract object Argument { get ; set ; }

        /// <summary>
        /// </summary>
        public event EventHandler CanExecuteChanged ;
        #endregion
    }


    // A TypeCategoryTab property tab lists properties by the
    // category of the type of each property.
}