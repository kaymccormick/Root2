﻿using System.Collections.Generic ;
using Autofac ;
using KayMcCormick.Dev ;
using WpfApp.Core.Infos ;
using WpfApp.Core.Menus ;

namespace WpfApp
{
    /// <summary>Class to host design data binding elements.</summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for DesignData
    public class DesignData
    {
        /// <summary>
        ///     Parameterless constructor.
        /// </summary>
        public DesignData ( )
        {
            var containerBuilder = new ContainerBuilder ( ) ;
            containerBuilder.RegisterModule < MenuModule > ( ) ;
            LifetimeScope = containerBuilder.Build ( ).BeginLifetimeScope ( "Design scope" ) ;
        }

        /// <summary>Gets the lifetime scope.</summary>
        /// <value>The lifetime scope.</value>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public ILifetimeScope LifetimeScope { get ; }

        /// <summary>Gets the instance list.</summary>
        /// <value>The instance list.</value>
        // ReSharper disable once CollectionNeverUpdated.Global
        public static List < InstanceInfo > InstanceList { get ; } = new List < InstanceInfo > ( ) ;

        /// <summary>Gets the instance information.</summary>
        /// <value>The instance information.</value>
        public static InstanceInfo InstanceInfo { get ; } = new InstanceInfo ( ) ;
    }
}
