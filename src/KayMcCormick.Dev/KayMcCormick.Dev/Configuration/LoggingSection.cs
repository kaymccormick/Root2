﻿using System.Configuration ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Dev.Logging ;
using NLog ;

namespace KayMcCormick.Dev.Configuration
{
    /// <summary>Configuration section handler for container helper settings.</summary>
    /// <seealso cref="System.Configuration.ConfigurationSection" />
    [ ConfigTarget ( typeof ( LoggerSettings ) ) ]
    [ UsedImplicitly ]
    public class LoggingSection : ConfigurationSection

    {
        /// <summary>
        /// </summary>
        [ ConfigurationProperty (
                                    "IsEnabledConsoleTarget"
                                  , DefaultValue = false
                                  , IsRequired   = false
                                  , IsKey        = false
                                ) ]
        // ReSharper disable once UnusedMember.Global
        public bool ? IsEnabledConsoleTarget
        {
            get { return ( bool ? ) this[ nameof ( IsEnabledConsoleTarget ) ] ; }
            set { this[ nameof ( IsEnabledConsoleTarget ) ] = value ; }
        }

        /// <summary>
        /// </summary>
        [ ConfigurationProperty (
                                    "IsEnabledCacheTarget"
                                  , DefaultValue = false
                                  , IsRequired   = false
                                  , IsKey        = false
                                ) ]
        [ UsedImplicitly ]
        public bool ? IsEnabledCacheTarget
        {
            get { return ( bool ? ) this[ nameof ( IsEnabledCacheTarget ) ] ; }
            set { this[ nameof ( IsEnabledCacheTarget ) ] = value ; }
        }

        /// <summary>
        /// </summary>
        [ ConfigurationProperty (
                                    "LogThrowExceptions"
                                  , DefaultValue = false
                                  , IsRequired   = false
                                  , IsKey        = false
                                ) ]
        // ReSharper disable once UnusedMember.Global
        public bool ? LogThrowExceptions
        {
            get { return ( bool ? ) this[ nameof ( LogThrowExceptions ) ] ; }
            set { this[ nameof ( LogThrowExceptions ) ] = value ; }
        }

        /// <summary>
        /// </summary>
        [ ConfigurationProperty (
                                    "MinLogLevel"
                                  , DefaultValue = "Trace"
                                  , IsRequired   = false
                                  , IsKey        = false
                                ) ]
        public LogLevel MinLogLevel
        {
            get { return ( LogLevel ) this[ nameof ( MinLogLevel ) ] ; }
            set
            {
                var level = LogLevel.FromString ( value.ToString ( ) ) ;
                DebugUtils.WriteLine ( $"Setting MinLogLevel to {level}" ) ;
                this[ nameof ( MinLogLevel ) ] = level ;
            }
        }
    }
}