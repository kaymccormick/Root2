using System.Diagnostics;

namespace AnalysisControls
{
    public class TraceEntry
    {
        public string Source { get; }
        private readonly TraceEventType _eventType;
        private readonly int _id;
        public object Data { get; }

        public TraceEntry(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            Source = source;
            _eventType = eventType;
            _id = id;
            Data = data;
        }

        public TraceEntry(string m)
        {
            Data = m;
        }
    }
}