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

namespace KayMcCormick.Dev.Command
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
        // ReSharper disable once UnusedMember.Global
        public static IAppCommandResult Cancelled { get ; } =
            new AppCommandResult { IsSuccess = false } ;
        #endregion


        /// <summary>
        /// </summary>
        /// <param name="taskException"></param>
        /// <returns></returns>
        [ JetBrains.Annotations.NotNull ]
        public static AppCommandResult Faulted ( AggregateException taskException )
        {
            return new AppCommandResult { IsSuccess = false , Exception = taskException } ;
        }

        /// <inheritdoc />
        public override string ToString ( ) { return $"Exception: {Exception}, IsSuccess: {IsSuccess}" ; }
    }
}