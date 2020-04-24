#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// InvocationMisc.cs
// 
// 2020-04-23-8:21 PM
// 
// ---
#endregion
using FindLogUsages ;

namespace AnalysisAppLib
{
    public class InvocationMisc : IEventMisc2 <ILogInvocation>
    {
        public InvocationMisc ( string rawJson , ILogInvocation instance )
        {
            _rawJson  = rawJson ;
            _instance = instance ;
        }

        private int            _threadId ;
        private object         _obj ;
        private MiscLevel      _level = MiscLevel.INFO;
        private string         _rawJson ;
        private ILogInvocation _instance ;
        private string         _propKeys ;
        #region Implementation of IEventMisc
        public int ThreadId { get { return _threadId ; } }

        public object Obj => Instance ;

        public string Message => _instance.MethodDisplayName ;

        public MiscLevel Level { get { return _level ; } set { _level = value ; } }

        public string RawJson { get { return _rawJson ; } set { _rawJson = value ; } }

        public string PropKeys { get { return _propKeys ; } }

        public string File => _instance.SourceLocation ;
        #endregion

        #region Implementation of IEventMisc2<out ILogInvocation>
        public ILogInvocation Instance { get { return _instance ; } }
        #endregion
    }
}