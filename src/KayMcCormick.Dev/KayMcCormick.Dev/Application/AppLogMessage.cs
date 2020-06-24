using System.Threading;

namespace KayMcCormick.Dev.Application
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AppLogMessage
    {
        private readonly string _message;
        private readonly int _threadId;

        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public AppLogMessage(string message)
        {
            _message = message;
            _threadId = Thread.CurrentThread.ManagedThreadId;
        }

        #region Overrides of Object

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(AppLogMessage)}[{_threadId}]: {Message}";
        }

        #endregion
    }
}