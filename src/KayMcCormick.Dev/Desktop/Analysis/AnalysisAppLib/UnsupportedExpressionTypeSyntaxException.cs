using System ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;

namespace AnalysisAppLib
{
    [ Serializable ]
    public class UnsupportedExpressionTypeSyntaxException : Exception
    {
        public UnsupportedExpressionTypeSyntaxException ( ) { }

        public UnsupportedExpressionTypeSyntaxException ( string message ) : base ( message ) { }

        public UnsupportedExpressionTypeSyntaxException (
            string    message
          , Exception innerException
        ) : base ( message , innerException )
        {
        }

        protected UnsupportedExpressionTypeSyntaxException (
            [ NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }
    }
}