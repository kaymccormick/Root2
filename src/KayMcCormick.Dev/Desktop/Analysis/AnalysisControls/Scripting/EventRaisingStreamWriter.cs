using System ;
using System.IO ;
using JetBrains.Annotations ;

namespace AnalysisControls.Scripting
{
    /* fOR PYTHON STUFF */
    /// <summary>
    /// </summary>
    internal sealed class EventRaisingStreamWriter : StreamWriter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public EventRaisingStreamWriter ( [ NotNull ] Stream s ) : base ( s ) { }

        /// <summary>
        /// </summary>
        public event EventHandler < MyEvtArgs < string > > StringWritten ;

        private void LaunchEvent ( string txtWritten )
        {
            StringWritten?.Invoke ( this , new MyEvtArgs < string > ( txtWritten ) ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public override void Write ( string value ) { LaunchEvent ( value ) ; }
    }
}