﻿using System ;
using System.Linq ;
using System.Reactive.Linq ;
using System.Reactive.Subjects ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Logging.Common ;
using NLog ;
using NLog.Config ;
using NLog.Targets ;

namespace WpfApp.Core.Logging
{
    /// <summary></summary>
    /// <seealso cref="NLog.Targets.Target" />
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for MyCacheTarget
    [Target ( nameof ( MyCacheTarget ) ) ]
    public class MyCacheTarget : Target
    {
        // ##############################################################################################################################
        // Constructor
        // ##############################################################################################################################

        #region Constructor
        /// <summary>Initializes a new instance of the <see cref="MyCacheTarget"/> class.</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for #ctor
        public MyCacheTarget ( )
        {
            _cacheSubject = new ReplaySubject < LogEventInfo > ( MaxCount ) ;
        }
        #endregion

        /// <summary>
        ///     If there is no target in NLog.config defined a new one is registered
        ///     with
        ///     the default maxCount
        /// </summary>
        /// <param name="defaultMaxCount"></param>
        /// <returns></returns>
        public static MyCacheTarget GetInstance ( int defaultMaxCount )
        {
            AppLoggingConfigHelper.EnsureLoggingConfigured ( ) ;
            var target =
                ( MyCacheTarget ) LogManager.Configuration.AllTargets.FirstOrDefault (
                                                                                      t => t is
                                                                                               MyCacheTarget
                                                                                     ) ;
            if ( target == null )
            {
                target = new MyCacheTarget
                         {
                             MaxCount = defaultMaxCount , Name = nameof ( MyCacheTarget )
                         } ;
                LogManager.Configuration.AddTarget ( target.Name , target ) ;
                LogManager.Configuration.LoggingRules.Insert (
                                                              0
                                                            , new LoggingRule (
                                                                               "*"
                                                                             , LogLevel.FromString (
                                                                                                    "Trace"
                                                                                                   )
                                                                             , target
                                                                              )
                                                             ) ;
                LogManager.ReconfigExistingLoggers ( ) ;
            }

            return target ;
        }

        // ##############################################################################################################################
        // override
        // ##############################################################################################################################

        #region override
        /// <summary>Writes logging event to the log target. Must be overridden in inheriting
        /// classes.</summary>
        /// <param name="logEvent">Logging event to be written out.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Write
        protected override void Write ( LogEventInfo logEvent )
        {
            _cacheSubject.OnNext ( logEvent ) ;
        }
        #endregion

        // ##############################################################################################################################
        // Properties
        // ##############################################################################################################################

        #region Properties
        // ##########################################################################################
        // Public Properties
        // ##########################################################################################

        /// <summary>
        ///     The maximum amount of entries held
        /// </summary>
        [ RequiredParameter ]
        public int MaxCount { get ; set ; } = 1000 ;

        /// <summary>Gets the cache.</summary>
        /// <value>The cache.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Cache
        public IObservable < LogEventInfo > Cache => _cacheSubject.AsObservable ( ) ;

        private readonly ReplaySubject < LogEventInfo > _cacheSubject ;

        // ##########################################################################################
        // Private Properties
        // ##########################################################################################
        #endregion
    }
}