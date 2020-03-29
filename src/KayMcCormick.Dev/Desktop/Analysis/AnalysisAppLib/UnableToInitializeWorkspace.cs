using System ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;

#if ROSLYNMSBUILD
using Microsoft.CodeAnalysis.MSBuild ;
#endif

namespace AnalysisAppLib
{
    public class UnableToInitializeWorkspace : Exception
    {
        public UnableToInitializeWorkspace ( ) { }

        public UnableToInitializeWorkspace ( string message ) : base ( message ) { }

        public UnableToInitializeWorkspace ( string message , Exception innerException ) : base (
                                                                                                 message
                                                                                               , innerException
                                                                                                )
        {
        }

        protected UnableToInitializeWorkspace (
            [ NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }
    }
}