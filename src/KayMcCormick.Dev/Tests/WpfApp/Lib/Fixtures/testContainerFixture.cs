﻿using Autofac ;
using JetBrains.Annotations ;
using Tests.Lib.Misc ;

namespace Tests.Lib.Fixtures
{
    /// <summary></summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for TestContainerFixture
    [ UsedImplicitly ]
    public class TestContainerFixture
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="System.Object" />
        ///     class.
        /// </summary>
        public TestContainerFixture ( ) { _init ( ) ; }

        /// <summary>Gets the container.</summary>
        /// <value>The container.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Container
        public IContainer Container { get ; private set ; }

        private void _init ( )
        {
            var builder = new ContainerBuilder ( ) ;
            builder.RegisterType < RandomClass > ( ).As < IRandom > ( ) ;

            Container = builder.Build ( ) ;
        }
    }
}