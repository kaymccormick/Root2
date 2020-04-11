using System ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;

namespace AnalysisAppLib.XmlDoc
{
    /// <summary>
    /// 
    /// </summary>
    public class UnrecognizedElementException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
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
        public UnrecognizedElementException ( string message , Exception innerException ) :
            base ( message , innerException )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected UnrecognizedElementException (
            [ NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }
    }
}