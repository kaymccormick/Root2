using System ;
using System.IO ;
using System.Xml ;
using CodeAnalysisApp1 ;
using Microsoft.Build.Locator ;
using Microsoft.CodeAnalysis.MSBuild ;
using ProjLib ;
using Xunit ;
using Xunit.Abstractions ;

namespace Tests
{
    public class Class2 
    {
        private readonly ITestOutputHelper _helper ;
        // private VisualStudioInstance VSInstance ;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public Class2 ( ITestOutputHelper helper )
        {
            _helper = helper ;
            // VSInstance = fixture.I ;
        }



        public void a ( )
        {
            // Assert.NotNull(VSInstance);
            // var root = @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos" ;
            // var p = Path.Combine ( root , @"V2\LogTest\LogTest.sln" ) ;
            // Assert.True ( File.Exists ( p ) ) ;
            // ProjLib.ProjectHandler v =
                // new ProjectHandler ( p, VSInstance ) ;
        }
        [Fact]
        public void TestMethodxml ( )
        {
            var doc = new XmlDocument();
            var root = doc.CreateElement ( "X" ) ;
            doc.AppendChild ( root ) ;
            var methodInfo = typeof ( Class2 ).GetMethod ( nameof ( TestMethodxml ) ) ;
            var methodXmlElement = TypeWriter.MethodXmlElement (
                                                                doc.CreateElement
                                                              , doc.CreateElement
                                                              , methodInfo ?? throw new InvalidOperationException ( )
                                                              , WriteStyle.Compact
                                                               ) ;
            root.AppendChild(methodXmlElement.CloneNode(true));
            root.AppendChild(methodXmlElement.CloneNode(true));
            var s = new StringWriter ( ) ;
            var writer = XmlWriter.Create (
                                           s
                                         , new XmlWriterSettings
                                           {
                                               NamespaceHandling =
                                                   NamespaceHandling.OmitDuplicates
                                           }
                                          ) ;
            // methodXmlElement.WriteTo ( writer ) ;
            doc.WriteTo(writer);
            _helper.WriteLine ( s.ToString() ) ;
            // _helper.WriteLine(methodXmlElement.OuterXml);
        }

      //  [Fact]
        public void TestId ( )
        {
            //TypeWriter.SubIdForGenericType ( typeof ( List < string > ) ) ;
            var x = TypeWriter.GetXmlId ( typeof ( TypeWriter ).GetMethod ( "MethodXmlElement" ) ) ;

                                       // typeof(List <string> ).GetMethod("AddRange"));
                                       Assert.Equal (
                                                     "M:TypeWriter.MethodXmlElement(System.Func{System.String,System.Xml.XmlElement},System.Func{System.String,System.String,System.Xml.XmlElement},System.Reflection.MethodInfo,CodeAnalysisApp1.WriteStyle,System.Xml.XmlNamespaceManager)"
                                                   , x
                                                    ) ;

            _helper.WriteLine ( x ) ;


        }
    }
}
