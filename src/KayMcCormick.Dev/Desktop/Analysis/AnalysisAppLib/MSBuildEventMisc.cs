#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// MSBuildEventMisc.cs
// 
// 2020-04-23-8:21 PM
// 
// ---
#endregion
using JetBrains.Annotations ;
using Microsoft.Build.Framework ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class MsBuildEventMisc : IEventMisc2<BuildEventArgs>
    {
        private int            _threadId ;
        private object         _obj ;
        private MiscLevel      _level ;
        private string         _rawJson ;
        private string         _propKeys ;
        private string         _file ;
        private BuildEventArgs _instance ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="level"></param>
        public MsBuildEventMisc ( [ NotNull ] BuildEventArgs args , MiscLevel level )
        {
            _instance = args ;
            _level    = level ;
            Obj       = args ;
            Message   = args.Message ;
        }

        #region Implementation of IEventMisc
        /// <inheritdoc />
        public int ThreadId { get { return _threadId ; } }

        /// <inheritdoc />
        public object Obj
        {
            get { return _obj ; }
            set { _obj = value ; }
        }

        /// <inheritdoc />
        public string Message { get ; set ; }

        /// <inheritdoc />
        public MiscLevel Level { get { return _level ; } set { _level = value ; } }

        /// <inheritdoc />
        public string RawJson { get { return _rawJson ; } set { _rawJson = value ; } }

        /// <inheritdoc />
        public string PropKeys { get { return _propKeys ; } }

        /// <inheritdoc />
        public string File { get { return _file ; } set { _file = value ; } }
        #endregion

        #region Implementation of IEventMisc2<out BuildEventArgs>
        /// <inheritdoc />
        public BuildEventArgs Instance { get { return _instance ; } }
        #endregion
    }
}