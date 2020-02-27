﻿using System;
using System.Linq ;
using Autofac ;
using KayMcCormick.Dev.TestLib.Fixtures ;
using NLog;
using NLog.Layouts;
using ProjInterface ;
using ProjLib ;
using Xunit;
using Xunit.Abstractions;

namespace Tests.Main
{
    /// <summary>Tests for primary application class <see cref="App"/>.</summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for AppTests
    [Collection("GeneralPurpose")]
    public class ContainerTests : IClassFixture<LoggingFixture>, IDisposable
    {
        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once InconsistentNaming
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly LoggingFixture _loggingFixture;

        /// <summary>Initializes a new instance of the <see cref="System.Object" /> class.</summary>
        public ContainerTests(ITestOutputHelper output, LoggingFixture loggingFixture)
        {
            _loggingFixture = loggingFixture;
            loggingFixture.SetOutputHelper(output);
            _loggingFixture.Layout = Layout.FromString("${message}");
        }

        /// <summary>Tests application of configuration in the app.config file.</summary>
        /// <autogeneratedoc />d ndfajdsad
        /// TODO Edit XML Comment Template for TestApplyConfiguration

        [Fact]
        void TestContanier ( )
        {
            var x = ProjInterface.InterfaceContainer.GetContainer ( ) ;
            var y = x.Resolve < IWorkspacesViewModel > ( ) ;
            Logger.Debug("{viewModel}", y);

            var p = x.Resolve<IMruItemProvider>();
            Assert.NotEmpty(y.VsCollection);
            foreach ( var vsInstance in y.VsCollection )
            {
                
                Logger.Debug(vsInstance.DisplayName);
                // Assert.NotNull(vsInstance.InstallationPath);
                // Assert.NotNull(vsInstance.ProductPath);
                var mruItemListFor = p.GetMruItemListFor ( vsInstance ) ;
                if ( !mruItemListFor.Any ( ) )
                {
                    Logger.Debug ( "no mru" ) ;
                }
                Assert.Equal(vsInstance.MruItems.Count, mruItemListFor.Count);
                foreach ( var mruItem in mruItemListFor )
                {
                    Logger.Debug(mruItem.FilePath);
                }
            }

            
        }
        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            // _loggingFixture?.Dispose ( ) ;
            _loggingFixture.SetOutputHelper(null);
        }
    }
}