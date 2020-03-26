﻿using System ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev
{
    /// <summary></summary>
    /// <seealso cref="System.Exception" />
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for AttributeNotFoundException
    [ Serializable ]
    public class AttributeNotFoundException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="System.Exception" /> class.</summary>
        public AttributeNotFoundException ( ) { }

        /// <summary>Initializes a new instance of the <see cref="System.Exception" /> class with a specified error message.</summary>
        /// <param name="message">The message that describes the error.</param>
        public AttributeNotFoundException ( string message ) : base ( message ) { }

        /// <summary>Initializes a new instance of the <see cref="System.Exception" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public AttributeNotFoundException ( string message , Exception innerException ) : base (
                                                                                                message
                                                                                              , innerException
                                                                                               )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="System.Exception" /> class with serialized data.</summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="info" /> is <see langword="null" />.</exception>
        /// <exception cref="System.Runtime.Serialization.SerializationException">The class name is <see langword="null" /> or <see cref="System.Exception.HResult"/> is zero (0).</exception>
        protected AttributeNotFoundException (
            [ NotNull ] SerializationInfo info
          , StreamingContext              context
        ) : base ( info , context )
        {
        }

        /// <summary>Gets the name of the attribute.</summary>
        /// <value>The name of the attribute.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for AttributeName
        
        
        public string AttributeName { get ; private set ; }
    }
}