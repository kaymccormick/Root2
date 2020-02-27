using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Reflection ;
using System.Resources ;
using System.Runtime.InteropServices ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Input ;
using System.Windows.Markup ;
using System.Windows.Media ;
using System.Windows.Threading ;
using CodeAnalysisApp1 ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;
using ProjInterface.Properties ;
using ProjLib ;

namespace ProjInterface
{
    public static class ProjUtils
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        public static void MakeFormattedCode (
            object o
        )
        {
            Logger.Info ( nameof ( MakeFormattedCode ) ) ;
            MakeInfo makeInfo = o as MakeInfo ;
            double scale = 1 ;
            var text = makeInfo.SourceText ;
            Logger.Trace("{SourceText}", text);

            CodeAnalyseContext ctx = makeInfo.Context ;

            SetujpFormattedCode1 (
                                  makeInfo.W
                                , makeInfo.SourceText,
                                  ctx
                   ,              makeInfo.AddChild
                   ,              makeInfo.Control
                                 ) ;

        }

        public static void SetujpFormattedCode1 (
             ContentControl    w
          , string                sourceText, CodeAnalyseContext ctx
          , IAddChild             addChild
          , ProjLib.FormattedCode         control
        )
        {
            double scale = 1 ;
            ContainFormattedCode (
                                  w
                                , sourceText
                                , ctx
                                , addChild
                            ,     control
                                 ) ;
        }

        private static void SetupFormattedCode (
             ContentControl    w
          , string                sourceText
          , CSharpCompilation     compilation
          , SyntaxTree            syntaxTree
          , CompilationUnitSyntax compilationUnitSyntax
          , double            scale
          , IAddChild             addChild
          , ProjLib.FormattedCode         control
        )
        {
            control.Dispatcher.Invoke(
                                      DispatcherPriority.Send
                                    , new ContainDelegate ( _ContainFormattedCode )
                                    , w
                                    , sourceText
                                    , compilation
                                    , syntaxTree
                                    , compilationUnitSyntax
                                    , addChild
                                    , control
                                     ) ;
        }

        private static void _ContainFormattedCode (
            ContentControl        w
          , string                SouceText
            , CodeAnalyseContext ctx
          , IAddChild             addChild
          , ProjLib.FormattedCode         Control
        )
        {
            LogManager.GetCurrentClassLogger ( ).Info ( nameof ( _ContainFormattedCode ) ) ;

            Control.Tag = ctx ;
            double scale = 1;
            var f = Control;
            addChild?.AddChild(w);
            if (w != null)
            {
                Logger.Error ( "xxxx" ) ;
                var stackPanel = new StackPanel();
                stackPanel.Children.Add(f);
                w.Content =
                    new ScrollViewer()
                    {
                        Content = stackPanel
                    };
                w.KeyDown += (sender, args) => {
                    LogManager.GetCurrentClassLogger()
                              .Info(args.Key);
                    if (args.Key
                         == Key.OemPlus
                         && args.KeyboardDevice.Modifiers
                         == ModifierKeys.Control)
                    {
                        args.Handled = true;
                        scale = scale * 1.25;
                        var scal =
                            new ScaleTransform(
                                                scale
                                              , scale
                                               );
                        f.RenderTransform = scal;
                    }
                    else if (args.Key
                              == Key.OemMinus
                              && args.KeyboardDevice
                                     .Modifiers
                              == ModifierKeys.Control)
                    {
                        args.Handled = true;
                        scale = scale * 0.75;
                        var scal =
                            new ScaleTransform(
                                                scale
                                              , scale
                                               );
                        f.RenderTransform = scal;
                    }
                };
            }
            else
            {
            }
        }
    

    delegate void ContainDelegate (
            ContentControl        w
          , string                SouceText
          , CodeAnalyseContext ctx
          , IAddChild             addChild
          , ProjLib.FormattedCode         Control
        ) ;
        
        private static void ContainFormattedCode (
            ContentControl        w
          , string                SouceText
          , CodeAnalyseContext ctx
          , IAddChild             addChild
          , ProjLib.FormattedCode                Control
        )
        {
            Logger.Info ( "contain" ) ;
            Control.Dispatcher.Invoke(
                                      DispatcherPriority.Send
                                    , new ContainDelegate ( _ContainFormattedCode )
                                    , w
                                    , SouceText
                                    , ctx
                                    , addChild
                                    , Control
                                     ) ;
        }
    }
}