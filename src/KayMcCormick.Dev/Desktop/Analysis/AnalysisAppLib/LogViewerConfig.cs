#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// LogViewerConfig.cs
// 
// 2020-03-20-7:11 PM
// 
// ---
#endregion
namespace AnalysisAppLib
{

    public class LogViewerConfig

    {
        private int _port ;

        public LogViewerConfig ( ushort port ) { _port = port ; }

        public int Port { get { return _port ; } set { _port = value ; } }
    }
}