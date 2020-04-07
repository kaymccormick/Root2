﻿using System.Configuration ;
using KayMcCormick.Dev.Attributes ;

namespace KayMcCormick.Dev.Configuration
{
    /// <summary>Configuration section handler for container helper settings.</summary>
    /// <seealso cref="System.Configuration.ConfigurationSection" />
    [ ConfigTarget ( typeof ( ContainerHelperSettings ) ) ]
    public class ContainerHelperSection : ConfigurationSection
    {
        private const string DoTrace = "DoTraceConditionalRegistration" ;

        /// <summary>Gets or sets a value indicating whether [do interception].</summary>
        /// <value>
        ///     <see language="true" /> if [do interception]; otherwise,
        ///     <see language="false" />.
        /// </value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DoInterception
        [ ConfigurationProperty (
                                    "DoInterception"
                                  , DefaultValue = false
                                  , IsRequired   = false
                                  , IsKey        = false
                                ) ]

        public bool DoInterception
        {
            get { return ( bool ) this[ nameof ( DoInterception ) ] ; }
            set { this[ nameof ( DoInterception ) ] = value ; }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [do trace conditional
        ///     registration].
        /// </summary>
        /// <value>
        ///     <see language="true" /> if [do trace conditional registration];
        ///     otherwise, <see language="false" />.
        /// </value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DoTraceConditionalRegistration
        [ ConfigurationProperty (
                                    DoTrace
                                  , DefaultValue = false
                                  , IsRequired   = false
                                  , IsKey        = false
                                ) ]

        public bool DoTraceConditionalRegistration
        {
            get { return ( bool ) this[ DoTrace ] ; }
            set { this[ DoTrace ] = value ; }
        }
    }
}