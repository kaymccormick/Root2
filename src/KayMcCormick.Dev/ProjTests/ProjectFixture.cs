#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// CodeAnalysisApp1Tests
// ProjectFixture.cs
// 
// 2020-02-21-12:23 AM
// 
// ---
#endregion
#if MSBUILDLOCATOR
using System.Linq ;
using Microsoft.Build.Locator ;
#endif

namespace ProjTests
{
    public class ProjectFixture 
    {
#if MSBUILDLOCATOR
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public ProjectFixture ( ) {

            var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances();
            var r = visualStudioInstances.Where (
                                                 instance => instance.DiscoveryType
                                                             == DiscoveryType.VisualStudioSetup
                                                ) ;
            I = r.First ( ) ;

    }

        public VisualStudioInstance I { get ; set ; }
#endif
    }
}