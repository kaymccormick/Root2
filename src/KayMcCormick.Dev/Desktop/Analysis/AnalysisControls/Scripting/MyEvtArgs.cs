using System ;

namespace AnalysisControls.Scripting
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class MyEvtArgs < T > : EventArgs
    {
        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        public MyEvtArgs ( T value ) { Value = value ; }

        /// <summary>
        /// </summary>
        public T Value { get ; }
    }
}