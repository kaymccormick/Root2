﻿#region header
// Kay McCormick (mccor)
// 
// WpfApp
// Tests
// GlobalLoggingFixture.cs
// 
// 2020-02-06-3:12 AM
// 
// ---
#endregion
using System ;
using System.Threading.Tasks ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Dev.TestLib.Logging ;
using NLog ;
using Xunit ;
using Xunit.Abstractions ;
using Xunit.Sdk ;

namespace KayMcCormick.Dev.TestLib.Fixtures
{
    /// <summary>
    ///     Test fixture to supply logging through the IMessageSink available
    ///     to infrastructure types in Xunit.
    /// </summary>
    /// <autogeneratedoc />
    // ReSharper disable once ClassNeverInstantiated.Global
    public class GlobalLoggingFixture : IAsyncLifetime
    {
        private static readonly Logger          Logger = LogManager.GetCurrentClassLogger ( ) ;
        private readonly        XunitSinkTarget _xunitSinkTarget ;
        private readonly        bool            _disableLogging ;

        /// <summary>
        ///     Initializes a new instance of the <see cref="System.Object" />
        ///     class.
        /// </summary>
        public GlobalLoggingFixture ( [ NotNull ] IMessageSink sink )
        {
            if ( sink == null )
            {
                throw new ArgumentNullException ( nameof ( sink ) ) ;
            }

            if ( Environment.GetEnvironmentVariable ( "DISABLE_TEST_LOGGING" ) != null )
            {
                LogHelper.DisableLoggingConfiguration ( ) ;
                _disableLogging = true ;
                return ;
            }

            // LogHelper.EnsureLoggingConfigured (
                                               // message => sink.OnMessage (
                                                                          // new DiagnosticMessage (
                                                                                                 // message
                                                                                                // )
                                                                         // )
                                              // ) ;

            var l = AppLoggingConfigHelper.SetupJsonLayout ( ) ;

            // sink.OnMessage ( new DiagnosticMessage ( "Constructing GlobalLoggingFixture." ) ) ;
            _xunitSinkTarget = new XunitSinkTarget ( "Xunitsink" , sink ) { Layout = l } ;
            AppLoggingConfigHelper.AddTarget ( _xunitSinkTarget , null ) ;
            Logger.Warn ( $"{nameof ( GlobalLoggingFixture )} logger added." ) ;
        }


        /// <summary>
        ///     Called immediately after the class has been created, before it is used.
        /// </summary>
        [ NotNull ]
        public Task InitializeAsync ( ) { return Task.CompletedTask ; }

        /// <summary>
        ///     Called when an object is no longer needed. Called just before
        ///     <see cref="System.IDisposable.Dispose" />
        ///     if the class also implements that.
        /// </summary>
        [ NotNull ]
        public Task DisposeAsync ( )
        {
            if ( ! _disableLogging )
            {
                AppLoggingConfigHelper.RemoveTarget ( _xunitSinkTarget.Name ) ;
            }

            return Task.CompletedTask ;
        }
    }
}