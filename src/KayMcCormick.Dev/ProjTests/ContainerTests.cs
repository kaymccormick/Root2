using System ;
using KayMcCormick.Dev.TestLib.Fixtures ;
using NLog ;
using NLog.Layouts ;
using Xunit ;
using Xunit.Abstractions ;

namespace ProjTests
{
    [Collection("GeneralPurpose")]
    public sealed class ContainerTests : IClassFixture<LoggingFixture>, IDisposable
    {
        
        
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ITestOutputHelper _output ;
        private readonly LoggingFixture _loggingFixture;

        /// <summary>Initializes a new instance of the <see cref="System.Object" /> class.</summary>
        public ContainerTests(ITestOutputHelper output, LoggingFixture loggingFixture)
        {
            _output = output ;
            _loggingFixture = loggingFixture;
            if ( loggingFixture != null )
            {
                loggingFixture.SetOutputHelper ( output ) ;
            }

            _loggingFixture.Layout = Layout.FromString("${message}");
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            // _loggingFixture?.Dispose ( ) ;
            _loggingFixture.SetOutputHelper(null);
        }
    }
}
