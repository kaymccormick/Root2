using System ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev
{
/// <summary>
/// 
/// </summary>
[Serializable
]    public class UnableToDeserializeLogEventInfo : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public UnableToDeserializeLogEventInfo ( ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public UnableToDeserializeLogEventInfo ( string message ) : base ( message ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UnableToDeserializeLogEventInfo ( string message , Exception innerException ) :
            base ( message , innerException )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected UnableToDeserializeLogEventInfo (
            [ NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }
    }
}