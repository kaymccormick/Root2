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
using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LogInvocation2 : ILogInvocation
    {
        private IList < ILogInvocationArgument > _arguments =
            new List < ILogInvocationArgument > ( ) ;

        private string _code ;
        private string _followingCode ;
        private string _loggerType ;
        private string _methodDisplayName ;
        private string _methodName ;
        private string _precedingCode ;
        private string _sourceLocation ;
        private object _transformedRelevantNode ;
        private Location _location ;

        public override string ToString ( )
        {
            return
                $"{nameof ( _sourceLocation )}: {_sourceLocation}, {nameof ( _code )}: {_code}, {nameof ( _methodDisplayName )}: {_methodDisplayName}, {nameof ( _arguments )}: {_arguments}" ;
        }

        #region Implementation of ILogInvocation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceLocation"></param>
        /// <param name="followingCode"></param>
        /// <param name="precedingCode"></param>
        /// <param name="code"></param>
        /// <param name="loggerType"></param>
        /// <param name="methodName"></param>
        /// <param name="methodDisplayName"></param>
        /// <param name="transformedRelevantNode"></param>
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
            _sourceLocation          = sourceLocation ;
            _followingCode           = followingCode ;
            _precedingCode           = precedingCode ;
            _code                    = code ;
            _loggerType              = loggerType ;
            _methodName              = methodName ;
            _methodDisplayName       = methodDisplayName ;
            _transformedRelevantNode = transformedRelevantNode ;
        }

        /// <summary>
        /// 
        /// </summary>
        public LogInvocation2 ( ) { }

        /// <summary>
        /// 
        /// </summary>
        public string SourceLocation
        {
            get { return _sourceLocation ; }
            set { _sourceLocation = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FollowingCode
        {
            get { return _followingCode ; }
            set { _followingCode = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PrecedingCode
        {
            get { return _precedingCode ; }
            set { _precedingCode = value ; }
        }

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
        public string MethodName { get { return _methodName ; } set { _methodName = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public string MethodDisplayName
        {
            get { return _methodDisplayName ; }
            set { _methodDisplayName = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList < ILogInvocationArgument > Arguments
        {
            get { return _arguments ; }
            set { _arguments = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public object TransformedRelevantNode
        {
            get { return _transformedRelevantNode ; }
            set { _transformedRelevantNode = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Location Location { get { return _location ; } set { _location = value ; } }
        #endregion
    }
}