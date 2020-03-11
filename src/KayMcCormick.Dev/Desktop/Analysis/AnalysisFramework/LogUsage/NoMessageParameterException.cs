using System ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;

namespace AnalysisFramework.LogUsage
{
    [Serializable]
    public class NoMessageParameterException : Exception
    {
        
        public NoMessageParameterException ( ) {
        }

        public NoMessageParameterException ( string message ) : base ( message )
        {
        }

        public NoMessageParameterException ( string message , Exception innerException ) : base ( message , innerException )
        {
        }

        protected NoMessageParameterException ( [ NotNull ] SerializationInfo info , StreamingContext context ) : base ( info , context )
        {
        }
    }
}