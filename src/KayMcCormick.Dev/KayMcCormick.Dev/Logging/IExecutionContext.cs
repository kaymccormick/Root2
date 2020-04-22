#region header
// Kay McCormick (mccor)
// 
// Analysis
// KayMcCormick.Dev
// IExecutionContext.cs
// 
// 2020-04-22-8:23 AM
// 
// ---
#endregion
namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    ///     Execution context of application.
    /// </summary>
    internal interface IExecutionContext
    {
        /// <summary>
        ///     Application execution context.
        /// </summary>
        Application Application { get ; }
    }
}