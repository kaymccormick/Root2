#region header
// Kay McCormick (mccor)
// 
// Analysis
// KayMcCormick.Lib.Wpf
// AppCommandResult.cs
// 
// 2020-04-08-5:38 AM
// 
// ---
#endregion
using System ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf.Command
{
    /// <summary>
    /// </summary>
    public sealed class AppCommandResult : IAppCommandResult
    {
        /// <summary>
        /// </summary>
        public static readonly IAppCommandResult Failed = new AppCommandResult { IsSuccess = false } ;

        /// <summary>
        /// </summary>
        public static readonly IAppCommandResult Success = new AppCommandResult { IsSuccess = true } ;

        private Exception _exception ;
        private bool      _isSuccess ;

        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public Exception Exception { get { return _exception ; } set { _exception = value ; } }

        #region Implementation of IAppCommandResult
        /// <summary>
        /// </summary>
        public bool IsSuccess { get { return _isSuccess ; } set { _isSuccess = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public static IAppCommandResult Cancelled { get ; } =
            new AppCommandResult ( ) { IsSuccess = false } ;
        #endregion


        /// <summary>
        /// </summary>
        /// <param name="taskException"></param>
        /// <returns></returns>
        [ NotNull ]
        public static AppCommandResult Faulted ( AggregateException taskException )
        {
            return new AppCommandResult { IsSuccess = false , Exception = taskException } ;
        }
    }
}