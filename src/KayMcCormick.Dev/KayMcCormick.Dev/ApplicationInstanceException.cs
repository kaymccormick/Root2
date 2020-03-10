#region header
// Kay McCormick (mccor)
// 
// ConfigTest
// KayMcCormick.Dev
// ApplicationInstanceException.cs
// 
// 2020-03-09-8:43 PM
// 
// ---
#endregion
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
    public class ApplicationInstanceException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public ApplicationInstanceException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ApplicationInstanceException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ApplicationInstanceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ApplicationInstanceException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}