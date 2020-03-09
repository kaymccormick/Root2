using System ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev
{
    public class AppInfraException : Exception
    {
        public AppInfraException ( ) {
        }

        public AppInfraException ( string message ) : base ( message )
        {
        }

        public AppInfraException ( string message , Exception innerException ) : base ( message , innerException )
        {
        }

        protected AppInfraException ( [ NotNull ] SerializationInfo info , StreamingContext context ) : base ( info , context )
        {
        }
    }
}