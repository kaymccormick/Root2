#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// Tests
// AppContainerFixture2.cs
// 
// 2020-03-20-2:08 AM
// 
// ---
#endregion
using System.Threading.Tasks ;
using Autofac ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Logging ;
using NLog ;
using Xunit ;
using Xunit.Abstractions ;
using Xunit.Sdk ;

namespace ProjTests
{
    /// <summary>
    ///     Test fixture configured to supply the primary application container
    ///     from Autofac.
    /// </summary>
    /// <seealso cref="Xunit.IAsyncLifetime" />
    /// <seealso cref="LegacyAppBuildModule" />
    // ReSharper disable once UnusedType.Global
    public class AppContainerFixture : IAsyncLifetime
    {
        // ReSharper disable once UnusedMember.Local
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

#pragma warning disable 649
        private readonly ApplicationInstanceBase _applicationInstance ;
#pragma warning restore 649
#pragma warning disable 649
        private readonly IMessageSink            _sink ;
#pragma warning restore 649
        private          ILifetimeScope          _lifetimeScope ;


        /// <summary>
        ///     Called immediately after the class has been created, before it is used.
        /// </summary>
        [ NotNull ]
        public Task InitializeAsync ( )
        {
            _sink.OnMessage ( new DiagnosticMessage ( "Initializing container." ) ) ;

            _lifetimeScope = _applicationInstance.GetLifetimeScope ( ) ;
            return Task.CompletedTask ;
        }


        /// <summary>
        ///     Called when an object is no longer needed. Called just before
        ///     <see cref="System.IDisposable.Dispose()" />
        ///     if the class also implements that.
        /// </summary>
        [ NotNull ]
        public Task DisposeAsync ( )
        {
            _applicationInstance.Dispose ( ) ;
            return Task.CompletedTask ;
        }


        /// <summary>
        ///     Begin a new nested scope. Component instances created via the new scope
        ///     will be disposed along with it.
        /// </summary>
        /// <param name="tag">
        ///     The tag applied to the
        ///     <see cref="Autofac.ILifetimeScope" />.
        /// </param>
        /// <returns>A new lifetime scope.</returns>
        [ NotNull ]
        public ILifetimeScope BeginLifetimeScope ( [ NotNull ] object tag )
        {
            return _lifetimeScope.BeginLifetimeScope ( tag ) ;
        }
    }
}