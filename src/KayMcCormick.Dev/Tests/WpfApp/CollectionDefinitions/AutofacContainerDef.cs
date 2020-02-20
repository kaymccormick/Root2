using Tests.Lib.Fixtures ;
using Xunit ;

namespace Tests.CollectionDefinitions
{
    [ CollectionDefinition ( "AutofacContainer" ) ]
    internal class AutofacContainerDef : ICollectionFixture < AppContainerFixture >
    {
    }
}