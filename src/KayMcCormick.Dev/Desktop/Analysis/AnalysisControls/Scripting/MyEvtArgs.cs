using System ;

namespace AnalysisControls.Scripting
{
    public class MyEvtArgs < T > : EventArgs
    {
        public T Value { get ; private set ; }

        public MyEvtArgs ( T value ) { Value = value ; }
    }
}