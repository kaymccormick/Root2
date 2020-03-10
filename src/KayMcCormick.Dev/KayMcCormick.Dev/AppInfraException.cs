using JetBrains.Annotations;
using System;
using System.Runtime.Serialization;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    [Serializable]
    public class AppInfraException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public AppInfraException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public AppInfraException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public AppInfraException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected AppInfraException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}