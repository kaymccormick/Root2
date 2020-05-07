using System ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UnrecognizedElementException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public UnrecognizedElementException ( ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public UnrecognizedElementException ( string message ) : base ( message ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        // ReSharper disable once UnusedMember.Global
        public UnrecognizedElementException ( string message , Exception innerException ) :
            base ( message , innerException )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        // ReSharper disable once UnusedMember.Local
        private UnrecognizedElementException (
            [ NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }
    }
}