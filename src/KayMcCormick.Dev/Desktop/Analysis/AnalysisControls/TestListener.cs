using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Remoting;

namespace AnalysisControls
{
    public class TestListener : TraceListener
    {
        /// <inheritdoc />
        public override void Write(string message)
        {
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <inheritdoc />
        public override void Close()
        {
            base.Close();
        }

        /// <inheritdoc />
        public override void Flush()
        {
            base.Flush();
        }

        /// <inheritdoc />
        public override void Fail(string message)
        {
            base.Fail(message);
        }

        /// <inheritdoc />
        public override void Fail(string message, string detailMessage)
        {
            base.Fail(message, detailMessage);
        }

        /// <inheritdoc />
        protected override string[] GetSupportedAttributes()
        {
            return base.GetSupportedAttributes();
        }

        /// <inheritdoc />
        public override void Write(object o)
        {
            base.Write(o);
        }

        /// <inheritdoc />
        public override void Write(string message, string category)
        {
            base.Write(message, category);
        }

        /// <inheritdoc />
        public override void Write(object o, string category)
        {
            base.Write(o, category);
        }

        /// <inheritdoc />
        protected override void WriteIndent()
        {
            base.WriteIndent();
        }

        /// <inheritdoc />
        public override void WriteLine(object o)
        {
            base.WriteLine(o);
        }

        /// <inheritdoc />
        public override void WriteLine(string message, string category)
        {
            base.WriteLine(message, category);
        }

        /// <inheritdoc />
        public override void WriteLine(object o, string category)
        {
            base.WriteLine(o, category);
        }

        /// <inheritdoc />
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            Elements.Add(new TraceEntry(eventCache, source, eventType, id, data));
        }

        public ObservableCollection<TraceEntry> Elements { get; set; }=new ObservableCollection<TraceEntry>();

        /// <inheritdoc />
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            base.TraceData(eventCache, source, eventType, id, data);
        }

        /// <inheritdoc />
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            base.TraceEvent(eventCache, source, eventType, id);
        }

        /// <inheritdoc />
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            base.TraceEvent(eventCache, source, eventType, id, message);
        }

        /// <inheritdoc />
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format,
            params object[] args)
        {
            base.TraceEvent(eventCache, source, eventType, id, format, args);
        }

        /// <inheritdoc />
        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            base.TraceTransfer(eventCache, source, id, message, relatedActivityId);
        }

        /// <inheritdoc />
        public override bool IsThreadSafe { get; }

        /// <inheritdoc />
        public override object InitializeLifetimeService()
        {
            return base.InitializeLifetimeService();
        }


        /// <inheritdoc />
        public override void WriteLine(string message)
        {
            Elements.Add(new TraceEntry(message));
        }
    }
}