using System.Windows.Input ;

namespace ProjInterface
{
    public static class ProjCommands
    {
        public static readonly RoutedUICommand LoadSolution =
            new RoutedUICommand (
                                 "Load Solution"
                               , nameof ( LoadSolution )
                               , typeof ( ProjCommands )
                                ) ;

        public static readonly RoutedUICommand NewAdHocWorkspace = new RoutedUICommand (
                                                                                        "New Ad-hoc Workspace"
                                                                                      , nameof (
                                                                                            NewAdHocWorkspace
                                                                                        )
                                                                                      , typeof (
                                                                                            ProjCommands
                                                                                        )
                                                                                       ) ;
    }
}