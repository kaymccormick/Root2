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
using Microsoft.Build.Framework ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public class MSBuildEventMisc : IEventMisc2<BuildEventArgs>
    {
        private int            _threadId ;
        private object         _obj ;
        private MiscLevel      _level ;
        private string         _rawJson ;
        private string         _propKeys ;
        private string         _file ;
        private BuildEventArgs _instance ;

        public MSBuildEventMisc ( BuildEventArgs args , MiscLevel level )
        {
            _instance = args ;
            _level    = level ;
            Obj       = args ;
            Message   = args.Message ;
        }

        #region Implementation of IEventMisc
        public int ThreadId { get { return _threadId ; } }

        public object Obj
        {
            get { return _obj ; }
            set { _obj = value ; }
        }

        public string Message { get ; set ; }

        public MiscLevel Level { get { return _level ; } set { _level = value ; } }

        public string RawJson { get { return _rawJson ; } set { _rawJson = value ; } }

        public string PropKeys { get { return _propKeys ; } }

        public string File { get { return _file ; } set { _file = value ; } }
        #endregion

        #region Implementation of IEventMisc2<out BuildEventArgs>
        public BuildEventArgs Instance { get { return _instance ; } }
        #endregion
    }
}