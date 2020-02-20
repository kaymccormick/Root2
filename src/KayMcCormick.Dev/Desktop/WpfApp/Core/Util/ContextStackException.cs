﻿
#region header
// Kay McCormick (mccor)
// 
// FileFinder3
// WpfApp1Tests3
// AttachedContext.cs
// 
// 2020-01-22-9:21 AM
// 
// ---
#endregion

using System ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;

namespace WpfApp.Core.Util
{
    /// <summary></summary>
    /// <seealso cref="System.Exception" />
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for ContextStackException
    [Serializable]
    public class ContextStackException : Exception
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="System.Exception" />
        ///     class.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public ContextStackException ( ) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="System.Exception" />
        ///     class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ContextStackException ( string message ) : base ( message ) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="System.Exception" />
        ///     class with a specified error message and a reference to the inner
        ///     exception
        ///     that is the cause of this exception.
        /// </summary>
        /// <param name="message">
        ///     The error message that explains the reason for the
        ///     exception.
        /// </param>
        /// <param name="innerException">
        ///     The exception that is the cause of the current
        ///     exception, or a null reference 
        ///     if no inner exception is specified.
        /// </param>
        // ReSharper disable once UnusedMember.Global
        public ContextStackException ( string message , Exception innerException ) : base (
                                                                                           message
                                                                                         , innerException
                                                                                          )
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="System.Exception" />
        ///     class with serialized data.
        /// </summary>
        /// <param name="info">
        ///     The
        ///     <see cref="System.Runtime.Serialization.SerializationInfo" /> that
        ///     holds
        ///     the serialized object data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The
        ///     <see cref="System.Runtime.Serialization.StreamingContext" /> that
        ///     contains contextual information about the source or destination.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="info" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">
        ///     The
        ///     class name is <see langword="null" /> or
        ///     <see cref="System.Exception.HResult"/> is zero (0).
        /// </exception>
        // ReSharper disable once UnusedMember.Global
        protected ContextStackException (
            [ NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }
    }
}