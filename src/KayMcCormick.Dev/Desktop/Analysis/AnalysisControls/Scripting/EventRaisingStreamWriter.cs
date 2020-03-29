using System ;
using System.IO ;
using JetBrains.Annotations ;

namespace AnalysisControls.Scripting
{
    /* fOR PYTHON SUTFF */
    /// <summary>
    /// </summary>
    public class EventRaisingStreamWriter : StreamWriter
    {
        public EventRaisingStreamWriter ( [ NotNull ] Stream s ) : base ( s ) { }

        /// <summary>
        /// </summary>
        public event EventHandler < MyEvtArgs < string > > StringWritten ;

        private void LaunchEvent ( string txtWritten )
        {
            if ( StringWritten != null )
            {
                StringWritten ( this , new MyEvtArgs < string > ( txtWritten ) ) ;
            }
        }

        public override void Write ( string value ) { LaunchEvent ( value ) ; }
    }
}