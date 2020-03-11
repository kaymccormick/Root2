using System ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;

namespace AnalysisFramework.LogUsage
{
    [Serializable]
        public class MissingTypeException : Exception
        {
            public MissingTypeException ( ) { }

            public MissingTypeException ( string message ) : base ( message ) { }

            public MissingTypeException ( string message , Exception innerException ) : base (
                                                                                              message
                                                                                            , innerException
                                                                                             )
            {
            }

            protected MissingTypeException (
                [ NotNull ] SerializationInfo info
              , StreamingContext              context
            ) : base ( info , context )
            {
            }
        }
        }