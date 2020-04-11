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
namespace AnalysisAppLib.XmlDoc
{
    /// <summary>
    /// 
    /// </summary>
    public class LogViewerConfig

    {
        private int _port ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        public LogViewerConfig ( ushort port ) { _port = port ; }

        /// <summary>
        /// 
        /// </summary>
        public int Port { get { return _port ; } set { _port = value ; } }
    }
}