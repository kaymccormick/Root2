using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace KayMcCormick.Dev
{
    public class AppInvalidOperationException : InvalidOperationException
    {
        private readonly string _callerFilename;

        public AppInvalidOperationException([CallerFilename] string callerFilename = "") : base($"{callerFilename}: No message")
        {
        }

        public AppInvalidOperationException(string message, [CallerFilename] string callerFilename = "") : base($"{callerFilename}: {message}")
        {
            _callerFilename = callerFilename;
        }

        public AppInvalidOperationException(string message, Exception innerException, [CallerFilename] string callerFilename = "") : base($"{callerFilename}: {message}", innerException)
        {
            _callerFilename = callerFilename;
        }

        protected AppInvalidOperationException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class CallerFilenameAttribute : Attribute
    {
    }
}
