using System.Collections.Generic ;
using System.ComponentModel.DataAnnotations.Schema ;
using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib
{
    public class LogInvocation2 < T >
    {
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
        public string SourceLocation { get { return _sourceLocation ; } set { _sourceLocation = value ; } }

        public string FollowingCode { get { return _followingCode ; } set { _followingCode = value ; } }

        public string PrecedingCode { get { return _precedingCode ; } set { _precedingCode = value ; } }

        public string Code { get { return _code ; } set { _code = value ; } }

        public string LoggerType { get { return _loggerType ; } set { _loggerType = value ; } }

        public string MethodName { get { return _methodName ; } }

        public string MethodDisplayName { get { return _methodDisplayName ; } set { _methodDisplayName = value ; } }

        public IList < T > Arguments { get { return _arguments ; } }

        [NotMapped]
        public object TransformedRelevantNode { get { return _transformedRelevantNode ; } set { _transformedRelevantNode = value ; } }

        [NotMapped]
        public Location Location { get { return _location ; } set { _location = value ; } }
        #endregion
    }
}