using System ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.TestLib.Fixtures ;
using NLog ;
using NLog.Layouts ;
using Xunit ;
using Xunit.Abstractions ;

namespace ProjTests
{
    [ Collection ( "GeneralPurpose" ) ]
    public sealed class ContainerTests : IClassFixture < LoggingFixture > , IDisposable
    {
        // ReSharper disable once UnusedMember.Local
        private static readonly Logger            Logger = LogManager.GetCurrentClassLogger ( ) ;
        private readonly        LoggingFixture    _loggingFixture ;
        // ReSharper disable once NotAccessedField.Local
        private readonly        ITestOutputHelper _output ;

        /// <summary>
        ///     Initializes a new instance of the <see cref="System.Object" />
        ///     class.
        /// </summary>
        public ContainerTests ( ITestOutputHelper output , [ CanBeNull ] LoggingFixture loggingFixture )
        {
            _output         = output ;
            _loggingFixture = loggingFixture ;
            loggingFixture?.SetOutputHelper ( output ) ;

            if ( _loggingFixture != null )
            {
                _loggingFixture.Layout = Layout.FromString ( "${message}" ) ;
            }
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing,
        ///     releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose ( )
        {
            // _loggingFixture?.Dispose ( ) ;
            _loggingFixture.SetOutputHelper ( null ) ;
        }
    }
}