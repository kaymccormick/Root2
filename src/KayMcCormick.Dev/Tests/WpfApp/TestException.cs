using System ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;

namespace Tests
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class TestException : Exception
    {
        public TestException ( ) {
        }

        public TestException ( string message ) : base ( message )
        {
        }

        public TestException ( string message , Exception innerException ) : base ( message , innerException )
        {
        }

        protected TestException ( [ NotNull ] SerializationInfo info , StreamingContext context ) : base ( info , context )
        {
        }
    }
}