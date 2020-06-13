﻿using System ;
using System.Reactive.Linq ;
using System.Reactive.Subjects ;
using JetBrains.Annotations ;
using NLog ;
using NLog.Config ;
using NLog.Targets ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary></summary>
    /// <seealso cref="NLog.Targets.Target" />
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for MyCacheTarget
    [ Target ( nameof ( MyCacheTarget ) ) ]
    public class MyCacheTarget : Target
    {
        // ##############################################################################################################################
        // Constructor
        // ##############################################################################################################################

        #region Constructor
        /// <summary>
        ///     Initializes a new instance of the <see cref="MyCacheTarget" />
        ///     class.
        /// </summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for #ctor
        public MyCacheTarget ( )
        {
            _cacheSubject = new ReplaySubject < LogEventInfo > ( MaxCount ) ;
        }
        #endregion

        // ##############################################################################################################################
        // override
        // ##############################################################################################################################

        #region override
        /// <summary>
        ///     Writes logging event to the log target. Must be overridden in inheriting
        ///     classes.
        /// </summary>
        /// <param name="logEvent">Logging event to be written out.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Write
        protected override void Write ( [ JetBrains.Annotations.NotNull ] LogEventInfo logEvent )
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
        [ JetBrains.Annotations.NotNull ] public IObservable < LogEventInfo > Cache
        {
            get { return _cacheSubject.AsObservable ( ) ; }
        }

        private readonly ReplaySubject < LogEventInfo > _cacheSubject ;

        // ##########################################################################################
        // Private Properties
        // ##########################################################################################
        #endregion
    }
}