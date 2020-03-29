using System.Configuration ;
using System.Diagnostics ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Dev.Logging ;
using NLog ;

namespace KayMcCormick.Dev.Configuration
{
    /// <summary>Configuration section handler for container helper settings.</summary>
    /// <seealso cref="System.Configuration.ConfigurationSection" />
    [ ConfigTarget ( typeof ( LoggerSettings ) ) ]
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
                Debug.WriteLine ( $"Setting MinLogLevel to {level}" ) ;
                this[ nameof ( MinLogLevel ) ] = level ;
            }
        }
    }
}