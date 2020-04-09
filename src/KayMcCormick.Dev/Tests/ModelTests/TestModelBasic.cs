#region header
// Kay McCormick (mccor)
// 
// Analysis
// ModelTests
// TestModelBasic.cs
// 
// 2020-04-08-3:09 PM
// 
// ---
#endregion
using AnalysisAppLib ;
using Autofac ;
using Xunit ;

namespace ModelTests
{
    public class TestModelBasic
    {
        [ Fact ]
        public void TestContainer()
        {
            ContainerBuilder b = new ContainerBuilder();
            b.RegisterModule<AnalysisAppLibModule>();
            var c = b.Build(Autofac.Builder.ContainerBuildOptions.None);

        }
    }
}