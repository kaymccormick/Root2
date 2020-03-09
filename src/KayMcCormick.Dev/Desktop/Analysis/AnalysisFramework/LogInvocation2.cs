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

namespace AnalysisFramework
{
    public class LogInvocation2 : ILogInvocation {
        private string _sourceLocation ;
        private string _sourceContext ;
        private string _followingCode ;
        private string _precedingCode ;
        private string _code ;
        private string _loggerType ;
        private string _methodName ;
        private string _methodDisplayName ;
        private IList < ILogInvocationArgument > _arguments ;
        #region Implementation of ILogInvocation
        public LogInvocation2 ( string sourceLocation , string sourceContext , string followingCode , string precedingCode , string code , string loggerType , string methodName , string methodDisplayName , IList < ILogInvocationArgument > arguments )
        {
            _sourceLocation = sourceLocation ;
            _sourceContext = sourceContext ;
            _followingCode = followingCode ;
            _precedingCode = precedingCode ;
            _code = code ;
            _loggerType = loggerType ;
            _methodName = methodName ;
            _methodDisplayName = methodDisplayName ;
            _arguments = arguments ;
        }

        public string SourceLocation { get => _sourceLocation ; set => _sourceLocation = value ; }

        public string SourceContext { get => _sourceContext ; set => _sourceContext = value ; }

        public string FollowingCode { get => _followingCode ; set => _followingCode = value ; }

        public string PrecedingCode { get => _precedingCode ; set => _precedingCode = value ; }

        public string Code { get => _code ; set => _code = value ; }

        public string LoggerType { get => _loggerType ; set => _loggerType = value ; }

        public string MethodName { get => _methodName ; set => _methodName = value ; }

        public string MethodDisplayName { get => _methodDisplayName ; set => _methodDisplayName = value ; }

        public IList < ILogInvocationArgument > Arguments { get => _arguments ; set => _arguments = value ; }
        #endregion
    }
}