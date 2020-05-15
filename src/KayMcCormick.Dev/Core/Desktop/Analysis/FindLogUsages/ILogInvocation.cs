#region header
// Kay McCormick (mccor)
// 
// Proj
// AnalysisFramework
// ILogInvocation.cs
// 
// 2020-03-03-7:28 PM
// 
// ---
#endregion
using System.Collections.Generic ;
using System.ComponentModel ;
using Microsoft.CodeAnalysis ;

namespace FindLogUsages
{
    /// <summary>
    ///  Representa a log invocation source code.
    /// </summary>
    public interface ILogInvocation
    {
        /// <summary>
        /// 
        /// </summary>
        string SourceLocation { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        string FollowingCode { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        string PrecedingCode { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        string Code { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        string LoggerType { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        string MethodName { get ; }

        /// <summary>
        /// 
        /// </summary>
        string MethodDisplayName { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        IList < ILogInvocationArgument > Arguments { get ; }

        /// <summary>
        /// 
        /// </summary>
        object TransformedRelevantNode { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        [Browsable(false)]
        Location Location { get ; set ; }
    }
}