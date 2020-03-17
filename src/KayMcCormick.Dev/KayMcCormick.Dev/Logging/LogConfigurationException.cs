#region header
// Kay McCormick (mccor)
// 
// Deployment
// KayMcCormick.Dev
// LogConfigurationException.cs
// 
// 2020-03-17-12:05 AM
// 
// ---
#endregion
using System ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class LogConfigurationException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public LogConfigurationException ( ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public LogConfigurationException ( string message ) : base ( message ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public LogConfigurationException ( string message , Exception innerException ) : base (
                                                                                               message
                                                                                             , innerException
                                                                                              )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected LogConfigurationException (
            [ NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }
    }
}