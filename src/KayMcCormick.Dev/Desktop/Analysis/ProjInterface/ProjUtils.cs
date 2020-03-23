using System.Windows.Controls ;
using System.Windows.Input ;
using System.Windows.Markup ;
using System.Windows.Media ;
using System.Windows.Threading ;
using AnalysisAppLib ;
using AnalysisControls ;

using AnalysisFramework ;
using NLog ;

namespace ProjInterface
{
    public static class ProjUtils
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
    

        public static void SetupFormattedCode1 (
             ContentControl    w
          , string                sourceText, ISyntaxTreeContext ctx
          , IAddChild             addChild
          , FormattedCode         control
        )
        {
            ContainFormattedCode (
                                  w
                                , sourceText
                                , ctx
                                , addChild
                            ,     control
                                 ) ;
        }

        private static void _ContainFormattedCode (
            ContentControl        w
          , string                SouceText
            , ISyntaxTreeContext ctx
          , IAddChild             addChild
          , FormattedCode         Control
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
          , ISyntaxTreeContext ctx
          , IAddChild             addChild
          , FormattedCode         Control
        ) ;
        
        private static void ContainFormattedCode (
            ContentControl        w
          , string                SouceText
          , ISyntaxTreeContext ctx
          , IAddChild             addChild
          , FormattedCode                Control
        )
        {
            Logger.Info ( "contain" ) ;
            if ( Control.Dispatcher != null )
            {
                Control.Dispatcher.Invoke (
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
}