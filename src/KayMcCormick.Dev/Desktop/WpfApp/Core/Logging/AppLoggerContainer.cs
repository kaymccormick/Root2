﻿using System.Linq ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Logging.Common ;
using NLog ;
using NLog.Config ;

namespace WpfApp.Core.Logging
{
    /// <summary></summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for AppLoggerContainer
    public class AppLoggerContainer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="System.Object" />
        ///     class.
        /// </summary>
        public AppLoggerContainer ( ) { AppLoggingConfigHelper.EnsureLoggingConfigured ( ) ; }

        /// <summary>Gets or sets the application logger.</summary>
        /// <value>The application logger.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for AppLogger
        public AppLogger AppLogger { get ; set ; }

        /// <summary>Gets the internal log.</summary>
        /// <value>The internal log.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for InternalLog
        public string InternalLog
        {
            get
            {
                if ( AppLoggingConfigHelper.Writer != null )
                {
                    return AppLoggingConfigHelper.Writer.ToString ( ) ;
                }

                return "" ;
            }
        }

        /// <summary>Gets the configuration.</summary>
        /// <value>The configuration.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Configuration
        public LoggingConfiguration Configuration => LogManager.Configuration ;

        /// <summary>Gets the configuration.</summary>
        /// <value>The configuration.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Config
        public string Config
        {
            get
            {
                return string.Join (
                                    "; "
                                  , LogManager.Configuration.ConfiguredNamedTargets.Select (
                                                                                            (
                                                                                                target
                                                                                              , i
                                                                                            ) => target
                                                                                               .Name
                                                                                           )
                                   ) ;
            }
        }
    }
}