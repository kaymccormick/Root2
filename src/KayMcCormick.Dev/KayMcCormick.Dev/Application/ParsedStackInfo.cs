using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using KayMcCormick.Dev.StackTrace;

namespace KayMcCormick.Dev.Application
{
    /// <summary>
    /// </summary>
    public sealed class ParsedStackInfo
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly string _exMessage;

        private readonly string _typeName;
        private List<StackTraceEntry> _stackTraceEntries;

        /// <summary>
        /// </summary>
        public ParsedStackInfo()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="parsed"></param>
        /// <param name="typeName"></param>
        /// <param name="exMessage"></param>
        public ParsedStackInfo(
            [NotNull] IEnumerable<StackTraceEntry> parsed
            , string typeName
            , string exMessage
        )
        {
            _typeName = typeName;
            _exMessage = exMessage;
            StackTraceEntries = parsed.ToList();
        }

        /// <summary>
        /// </summary>
        public List<StackTraceEntry> StackTraceEntries
        {
            get { return _stackTraceEntries; }
            set { _stackTraceEntries = value; }
        }

        /// <summary>
        /// </summary>
        public string TypeName
        {
            get { return _typeName; }
        }
    }
}