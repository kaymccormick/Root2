using System.Collections.Generic ;
using System.Windows ;
using System.Windows.Controls ;
using ProjLib ;

namespace AnalysisControls
{
    public class CodeWindow : Window
    {
        public delegate IFormattedCode GetFormattedCodeDelegate ( object o ) ;

        private readonly TabControl control = new TabControl ( ) ;

        private readonly Dictionary < object , IFormattedCode > dict =
            new Dictionary < object , IFormattedCode > ( ) ;

        public CodeWindow ( ) { Content = control ; }
#if false
        public Task < IFormattedCode > GetFormattedCodeAsync ( object o )
        {
            return Task.FromResult (
                                    ( IFormattedCode ) Dispatcher.Invoke (
                                                                         DispatcherPriority.Send
                                                                   , new
                                                                             GetFormattedCodeDelegate (
                                                                                                       o1
                                                                                                           => new
                                                                                                               FormattedCode ( )
                                                                                                      )
                                                                   , o
                                                                        )
                                   ) ;
        }
#endif
#if false
        public IFormattedCode GetFormattedCode ( object o )
        {
            var taskFactory = new TaskFactory < IFormattedCode > ( ) ;
            if ( dict.TryGetValue ( o , out var item ) )
            {
                return item ;
            }

            dict[ o ] = new FormattedCode ( ) ;
            var item2 = new TabItem { Header = "123" , Content = dict[ o ] } ;
            ( ( IAddChild ) control ).AddChild ( item2 ) ;
            return dict[ o ] ;
        }
#endif
    }
}