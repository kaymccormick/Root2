using System ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev
{
    public class UnableToDeserializeLogEventInfo : Exception
    {
        public UnableToDeserializeLogEventInfo ( ) { }

        public UnableToDeserializeLogEventInfo ( string message ) : base ( message ) { }

        public UnableToDeserializeLogEventInfo ( string message , Exception innerException ) :
            base ( message , innerException )
        {
        }

        protected UnableToDeserializeLogEventInfo (
            [ NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }
    }
}