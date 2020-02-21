#region header
// Kay McCormick (mccor)
// 
// WpfApp
// Tests
// GeneralPurpose.cs
// 
// 2020-02-06-10:45 PM
// 
// ---
#endregion

using JetBrains.Annotations ;
using  KayMcCormick.Dev.TestLib.Fixtures ;
using Xunit ;

namespace Tests.CollectionDefinitions
{
    /// <summary>Defines the Xunit collection definition for tests that do not fall into other buckets. Provides the <see cref=" KayMcCormick.Dev.TestLib.Fixtures.GlobalLoggingFixture"/>.</summary>
    /// GlobalLoggingFixture" />
    [ CollectionDefinition ( "GeneralPurpose" ) ] [ UsedImplicitly ]
    public class GeneralPurpose : ICollectionFixture <GlobalLoggingFixture>
    {
    }
}