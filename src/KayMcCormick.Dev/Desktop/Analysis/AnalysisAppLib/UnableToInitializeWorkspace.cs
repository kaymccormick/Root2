using System ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;

#if ROSLYNMSBUILD
using Microsoft.CodeAnalysis.MSBuild ;
#endif

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public class UnableToInitializeWorkspace : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public UnableToInitializeWorkspace ( ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public UnableToInitializeWorkspace ( string message ) : base ( message ) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UnableToInitializeWorkspace ( string message , Exception innerException ) : base (
                                                                                                 message
                                                                                               , innerException
                                                                                                )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected UnableToInitializeWorkspace (
            [ JetBrains.Annotations.NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }
    }
}