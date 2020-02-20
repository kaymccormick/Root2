using System;
using System.Collections.Generic;
using Xunit ;
using Xunit.Abstractions ;

namespace Tests.Main.Utils
{
    internal class RunnerReporter : IRunnerReporter
    {
        /// <summary>
        /// Creates a message handler that will report messages for the given
        /// test assembly. Ideally, the handler should also implement <see cref="Xunit.IMessageSinkWithTypes" />
        /// for optimal performance, but plain implementations of <see cref="Xunit.Abstractions.IMessageSink" /> are supported
        /// for backward compatibility reasons.
        /// </summary>
        /// <param name="logger">The logger used to send result messages to</param>
        /// <returns>The message handler that handles the messages</returns>
        public IMessageSink CreateMessageHandler ( IRunnerLogger logger )
        {
            return new MessageSink ( ) ;

        }

        /// <summary>
        /// Gets the description of the reporter. This is typically used when showing
        /// the user the invocation option for the reporter.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string Description { get ; set ; }

        /// <summary>
        /// Gets a value which indicates whether the reporter should be
        /// environmentally enabled.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public bool IsEnvironmentallyEnabled { get ; set ; }

        /// <summary>
        /// Gets a value which indicates a runner switch which can be used
        /// to explicitly enable the runner. If the return value is <see language="null"/>,
        /// then the reported can only be environmentally enabled (implicitly).
        /// This value is used either as a command line switch (with the console or
        /// .NET CLI runner) or as a runner configuration value (with the MSBuild runner).
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string RunnerSwitch { get ; set ; }
    }

    internal class MessageSink : IMessageSink, IMessageSinkWithTypes
    {
        /// <summary>
        /// Reports the presence of a message on the message bus. This method should
        /// never throw exceptions.
        /// </summary>
        /// <param name="message">The message from the message bus</param>
        /// <returns>Return <see language="true"/> to continue running tests, or <see language="false"/> to stop.</returns>
        public bool OnMessage ( IMessageSinkMessage message ) { throw new NotImplementedException ( ) ; }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose ( ) { throw new NotImplementedException ( ) ; }

        /// <summary>
        /// Reports the presence of a message on the message bus with an optional list of message types.
        /// This method should never throw exceptions.
        /// </summary>
        /// <param name="message">The message from the message bus.</param>
        /// <param name="messageTypes">The list of message types, or <see language="null"/>.</param>
        /// <returns>Return <see language="true"/> to continue running tests, or <see language="false"/> to stop.</returns>
        public bool OnMessageWithTypes (
            IMessageSinkMessage message
          , HashSet < string >  messageTypes
        )
        {
            
            throw new NotImplementedException ( ) ;
        }
    }
}
