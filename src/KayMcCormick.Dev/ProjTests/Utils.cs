using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Input ;
using System.Windows.Media ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;
using ProjInterface ;

namespace ProjLib
{
    static internal class Utils
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        public static FormattedCode MakeFormattedCode ( out Window w )
        {
            double scale = 1 ;
            var c = new FormattedCode ( ) ;
            var syntaxTree = CSharpSyntaxTree.ParseText (
                                                         "using System;\nusing System.Collections.Generic;\nusing System.Linq;\nusing System.Text;\nusing System.Threading.Tasks;\nusing NLog ;\n\nnamespace LogTest\n{\n    class Program\n    {\n        private static readonly  Logger Logger = LogManager.GetCurrentClassLogger();\n        static void Main(string[] args) {\n            Action<string> xx = Logger.Info;\n            xx(\"hi\");\n            Logger.Debug ( \"Hello\" ) ;\n            try {\n                string xxx = null;\n                var q = xxx.ToString();\n            } catch(Exception ex) {\n                Logger.Info(ex, ex.Message);\n            }\n            var x = Logger;\n            // doprocess\n            x.Info(\"hello {test} {ab}\", 123, 45);\n        }\n\n    }\n}\n"
                                                        ) ;
            var compilationUnitSyntax = syntaxTree.GetCompilationUnitRoot ( ) ;
            var compilation = CSharpCompilation.Create ( "HelloWorld" )
                                               .AddReferences (
                                                               MetadataReference.CreateFromFile (
                                                                                                 typeof (
                                                                                                         string
                                                                                                     ).Assembly
                                                                                                      .Location
                                                                                                )
                                                             , MetadataReference.CreateFromFile (
                                                                                                 typeof (
                                                                                                         Logger
                                                                                                     ).Assembly
                                                                                                      .Location
                                                                                                )
                                                              )
                                               .AddSyntaxTrees ( syntaxTree ) ;

            var f = new FormattedCode ( )
                    {
                        Tag = new LogInvocationBase (
                                                     compilation.GetSemanticModel ( syntaxTree )
                                                   , compilationUnitSyntax
                                                   , null
                                                   , syntaxTree.GetRoot ( )
                                                    )
                    } ;
            w = new Window ( ) ;
            var stackPanel = new StackPanel ( ) ;
            stackPanel.Children.Add ( f ) ;
            w.Content = new ScrollViewer ( ) { Content = stackPanel } ;
            w.KeyDown += ( sender , args ) => {
                LogManager.GetCurrentClassLogger ( ).Info ( args.Key ) ;
                if ( args.Key                         == Key.OemPlus
                     && args.KeyboardDevice.Modifiers == ModifierKeys.Control )
                {
                    args.Handled = true ;
                    scale        = scale * 1.25 ;
                    var scal = new ScaleTransform ( scale , scale ) ;
                    f.RenderTransform = scal ;
                }
                else if ( args.Key                         == Key.OemMinus
                          && args.KeyboardDevice.Modifiers == ModifierKeys.Control )
                {
                    args.Handled = true ;
                    scale        = scale * 0.75 ;
                    var scal = new ScaleTransform ( scale , scale ) ;
                    f.RenderTransform = scal ;
                }
            } ;
            return f ;
        }
    }
}