﻿using NLog ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary></summary>
    /// <seealso cref="NLog.LogFactory" />
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for MyLogFactory
    public class MyLogFactory : LogFactory
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NLog.LogFactory" />
        ///     class.
        /// </summary>
        public MyLogFactory ( ) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MyLogFactory" />
        ///     class.
        /// </summary>
        /// <param name="doLogMessage">The do log message.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for #ctor
        public MyLogFactory ( LogDelegates.LogMethod doLogMessage )
        {
            DoLogMessage = doLogMessage ;
        }

        /// <summary>
        /// </summary>
        public LogDelegates.LogMethod DoLogMessage { get ; }
    }
}