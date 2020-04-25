using System.Collections.Generic ;
using System.ComponentModel.DataAnnotations.Schema ;
using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LogInvocation2 < T >
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get ; set ; }
        private string      _sourceLocation ;
        private string      _followingCode ;
        private string      _precedingCode ;
        private string      _code ;
        private string      _loggerType ;
        private string      _methodName ;
        private string      _methodDisplayName ;
        private IList < T > _arguments ;
        private object      _transformedRelevantNode ;
        private Location    _location ;
        #region Implementation of ILogInvocation2<T>
        /// <summary>
        /// 
        /// </summary>
        public string SourceLocation { get { return _sourceLocation ; } set { _sourceLocation = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public string FollowingCode { get { return _followingCode ; } set { _followingCode = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public string PrecedingCode { get { return _precedingCode ; } set { _precedingCode = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public string Code { get { return _code ; } set { _code = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public string LoggerType { get { return _loggerType ; } set { _loggerType = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public string MethodName { get { return _methodName ; } }

        /// <summary>
        /// 
        /// </summary>
        public string MethodDisplayName { get { return _methodDisplayName ; } set { _methodDisplayName = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public IList < T > Arguments { get { return _arguments ; } }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public object TransformedRelevantNode { get { return _transformedRelevantNode ; } set { _transformedRelevantNode = value ; } }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public Location Location { get { return _location ; } set { _location = value ; } }
        #endregion
    }
}