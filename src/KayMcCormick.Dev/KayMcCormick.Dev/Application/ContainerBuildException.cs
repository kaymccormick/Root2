using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace KayMcCormick.Dev.Application
{
    /// <summary>
    ///     Fatal error building container. Wraps any autofac exceptions.
    /// </summary>
    [Serializable]
    public class ContainerBuildException : Exception
    {
        /// <summary>
        ///     Parameterless constructor.
        /// </summary>
        public ContainerBuildException()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="message"></param>
        public ContainerBuildException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ContainerBuildException(string message, Exception innerException) : base(
            message
            , innerException
        )
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ContainerBuildException(
            [NotNull] SerializationInfo info
            , StreamingContext context
        ) : base(info, context)
        {
        }
    }
}