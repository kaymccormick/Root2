#region header
// Kay McCormick (mccor)
// 
// ConsoleApp1
// AnalysisFramework
// LogInvocation2.cs
// 
// 2020-03-02-3:04 AM
// 
// ---
#endregion
using System.Collections.Generic ;

namespace AnalysisAppLib
{
    public sealed class LogInvocation2 : ILogInvocation {
        private string _sourceLocation ;
        private string _followingCode ;
        private string _precedingCode ;
        private string _code ;
        private string _loggerType ;
        private string _methodName ;
        private string _methodDisplayName ;
        private object _transformedRelevantNode ;
        private IList < ILogInvocationArgument > _arguments = new List < ILogInvocationArgument > ();
        #region Implementation of ILogInvocation
        public LogInvocation2 (
            string sourceLocation
          , string followingCode
          , string precedingCode
          , string code
          , string loggerType
          , string methodName
          , string methodDisplayName
          , object transformedRelevantNode
        )
        {
            _sourceLocation = sourceLocation ;
            _followingCode = followingCode ;
            _precedingCode = precedingCode ;
            _code = code ;
            _loggerType = loggerType ;
            _methodName = methodName ;
            _methodDisplayName = methodDisplayName ;
            _transformedRelevantNode = transformedRelevantNode ;
        }

        public LogInvocation2 ( ) {
        }

        public string SourceLocation
        {
            get { return _sourceLocation ; }
            set { _sourceLocation = value ; }
        }

        public string FollowingCode
        {
            get { return _followingCode ; }
            set { _followingCode = value ; }
        }

        public string PrecedingCode
        {
            get { return _precedingCode ; }
            set { _precedingCode = value ; }
        }

        public string Code { get => _code ; set => _code = value ; }

        public string LoggerType { get => _loggerType ; set => _loggerType = value ; }

        public string MethodName { get => _methodName ; set => _methodName = value ; }

        public string MethodDisplayName { get => _methodDisplayName ; set => _methodDisplayName = value ; }

        public IList < ILogInvocationArgument > Arguments { get => _arguments ; set => _arguments = value ; }

        public object TransformedRelevantNode
        {
            get { return _transformedRelevantNode ; }
            set { _transformedRelevantNode = value ; }
        }
        #endregion

        public override string ToString ( )
        {
            return $"{nameof ( _sourceLocation )}: {_sourceLocation}, {nameof ( _code )}: {_code}, {nameof ( _methodDisplayName )}: {_methodDisplayName}, {nameof ( _arguments )}: {_arguments}" ;
        }
    }
}