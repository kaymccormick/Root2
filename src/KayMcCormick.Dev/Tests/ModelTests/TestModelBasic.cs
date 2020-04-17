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
using System.IO ;
using System.Xml ;
using AnalysisAppLib ;
using Autofac ;
using Xunit ;

namespace ModelTests
{
    public sealed class TestModelBasic
    {
        [ Fact ]
        public void TestContainer()
        {
            var b = new ContainerBuilder();
            b.RegisterModule<AnalysisAppLibModule>();
            // ReSharper disable once UnusedVariable
            var c = b.Build( );
            
            XmlWriter.Create(File.OpenWrite(@"C:\temp\model.xaml"), new XmlWriterSettings {Async = true,Indent = true});
                    
        }
        
    }

}