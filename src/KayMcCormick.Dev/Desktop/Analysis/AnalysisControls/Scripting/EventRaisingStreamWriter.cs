using System ;
using System.IO ;

namespace AnalysisControls.Scripting
{
    /* fOR PYTHON SUTFF */
    public class EventRaisingStreamWriter : StreamWriter
    {
        public event EventHandler < MyEvtArgs < string > > StringWritten ;

        public EventRaisingStreamWriter ( Stream s ) : base ( s ) { }

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