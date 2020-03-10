﻿using System ;
using Castle.DynamicProxy ;
using JetBrains.Annotations ;
using NLog ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>Attempt to hook into NLog and fix it up for my application.</summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for LoggerProxyHelper
    public class LoggerProxyHelper
    {
        /// <summary>
        ///     Initializes a new instance of the
        ///     <see
        ///         cref="System.Object" />
        ///     class.
        /// </summary>
        [ UsedImplicitly ]
        public LoggerProxyHelper ( ProxyGenerator generator ) { Generator = generator ; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="System.Object" />
        ///     class.
        /// </summary>
        public LoggerProxyHelper ( ProxyGenerator generator , LogDelegates.LogMethod logMethod )
        {
            Generator    = generator ;
            UseLogMethod = logMethod ;
        }

        /// <summary>Gets the generator.</summary>
        /// <value>The generator.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Generator
        public ProxyGenerator Generator { get ; }

        /// <summary>Gets or sets the use log method.</summary>
        /// <value>The use log method.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for UseLogMethod
        public LogDelegates.LogMethod UseLogMethod { get ; }

        /// <summary>Creates the log factory.</summary>
        /// <param name="logFactory">The log factory.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for CreateLogFactory
        public LogFactory CreateLogFactory ( LogFactory logFactory )
        {
            if ( logFactory == null )
            {
                logFactory = LogManager.LogFactory ;
            }

            var opts = new ProxyGenerationOptions ( new LoggerFactoryHook ( UseLogMethod ) ) ;
            opts.Initialize ( ) ;
            var proxy = ( LogFactory ) Generator.CreateClassProxyWithTarget (
                                                                             logFactory.GetType ( )
                                                                           , Type.EmptyTypes
                                                                           , logFactory
                                                                           , opts
                                                                           , new
                                                                                 LogFactoryInterceptor (
                                                                                                        Generator
                                                                                                      , UseLogMethod
                                                                                                       )
                                                                            ) ;
            return proxy ;
        }
    }
}