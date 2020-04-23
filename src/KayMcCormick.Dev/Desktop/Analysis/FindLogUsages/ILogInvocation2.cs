#region header
// Kay McCormick (mccor)
// 
// Analysis
// FindLogUsages
// ILogInvocation2.cs
// 
// 2020-04-22-10:12 PM
// 
// ---
#endregion
using System.Collections.Generic ;
using Microsoft.CodeAnalysis ;

namespace FindLogUsages
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILogInvocation2<T>
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
        IList < T > Arguments { get ; }

        /// <summary>
        /// 
        /// </summary>
        object TransformedRelevantNode { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        Location Location { get ; set ; }
    }
}