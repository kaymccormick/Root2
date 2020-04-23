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
using Microsoft.CodeAnalysis ;

namespace FindLogUsages
{
    /// <summary>
    /// 
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
        Location Location { get ; set ; }
    }

    class LogInvocation
    {
        private string _sourceLocation ;
        private string _followingCode ;
        private string _precedingCode ;
        private string _code ;
        private string _loggerType ;
        private string _methodName ;
        private string _methodDisplayName ;
        private IList < object > _arguments = new List < object > ();
        private object _transformedRelevantNode ;
        private Location _location ;
        #region Implementation of ILogInvocation
        public string SourceLocation { get { return _sourceLocation ; } set { _sourceLocation = value ; } }

        public string FollowingCode { get { return _followingCode ; } set { _followingCode = value ; } }

        public string PrecedingCode { get { return _precedingCode ; } set { _precedingCode = value ; } }

        public string Code { get { return _code ; } set { _code = value ; } }

        public string LoggerType { get { return _loggerType ; } set { _loggerType = value ; } }

        public string MethodName { get { return _methodName ; } }

        public string MethodDisplayName { get { return _methodDisplayName ; } set { _methodDisplayName = value ; } }

        public IList <object > Arguments { get { return _arguments ; } }

        public object TransformedRelevantNode { get { return _transformedRelevantNode ; } set { _transformedRelevantNode = value ; } }

        public Location Location { get { return _location ; } set { _location = value ; } }
        #endregion
    }
}