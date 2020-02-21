using Xunit ;
using  KayMcCormick.Dev.TestLib.Fixtures ;

namespace CodeAnalysisApp1Tests
{
    [ CollectionDefinition ( "General Purpose" ) ]
    public class GeneralPurpose : ICollectionFixture < GlobalLoggingFixture >
    {
    }
}