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
using System.Xaml ;
using System.Xml ;
using AnalysisAppLib ;
using AnalysisAppLib.XmlDoc ;
using Autofac ;
using Microsoft.CodeAnalysis.CSharp ;
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
            var viewModel = c.Resolve < ITypesViewModel > ( ) ;
            XmlWriter.Create(File.OpenWrite(@"C:\temp\model.xaml"), new XmlWriterSettings(){Async = true,Indent = true});
                    
        }
        
    }

}