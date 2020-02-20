using Xunit ;
using KayMcCormick.Test.Common.Fixtures ;

namespace CodeAnalysisApp1Tests
{
    [ CollectionDefinition ( "General Purpose" ) ]
    public class GeneralPurpose : ICollectionFixture < GlobalLoggingFixture >
    {
    }
}