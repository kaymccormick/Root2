using System.Windows.Input ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    ///     Static class containing <see cref="RoutedUICommand" /> instances
    ///     for the application.
    /// </summary>
    public static class WpfAppCommands
    {
        /// <summary>
        ///     <see cref="RoutedUICommand" /> to launch the settings dialog /
        ///     window.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public static readonly RoutedUICommand AppSettings =
            new RoutedUICommand (
                                 "Settings"
                               , nameof ( AppSettings )
                               , typeof ( WpfAppCommands )
                                ) ;

        /// <summary><see cref="RoutedUICommand" /> to launch a new window.</summary>
        public static readonly RoutedUICommand OpenWindow =
            new RoutedUICommand (
                                 "Open Window"
                               , nameof ( OpenWindow )
                               , typeof ( WpfAppCommands )
                                ) ;

        /// <summary><see cref="RoutedUICommand" /> to quit the application.</summary>
        public static readonly RoutedUICommand QuitApplication =
            new RoutedUICommand (
                                 "Quit Application"
                               , nameof ( QuitApplication )
                               , typeof ( WpfAppCommands )
                                ) ;

        /// <summary><see cref="RoutedUICommand" /> to load assembly list</summary>
        public static readonly RoutedUICommand LoadAssemblyList =
            new RoutedUICommand (
                                 "Load"
                               , nameof ( LoadAssemblyList )
                               , typeof ( WpfAppCommands )
                                ) ;

        /// <summary><see cref="RoutedUICommand" /> to restart the application.</summary>
        public static readonly RoutedUICommand Restart =
            new RoutedUICommand ( "Restart" , nameof ( Restart ) , typeof ( WpfAppCommands ) ) ;

        /// <summary><see cref="RoutedUICommand" /> to dump debug information.</summary>
        public static readonly RoutedUICommand DumpDebug =
            new RoutedUICommand (
                                 "Dump Debug"
                               , nameof ( DumpDebug )
                               , typeof ( WpfAppCommands )
                                ) ;

        /// <summary>
        ///     <see cref="RoutedUICommand" /> to limit filtering to rows with
        ///     associated instances.
        /// </summary>
        public static readonly RoutedUICommand FilterInstances =
            new RoutedUICommand (
                                 "Filter Instances"
                               , nameof ( FilterInstances )
                               , typeof ( WpfAppCommands )
                                ) ;

        /// <summary><see cref="RoutedUICommand" /> to load the selected object.</summary>
        public static readonly RoutedUICommand Load =
            new RoutedUICommand ( "Load" , nameof ( Load ) , typeof ( WpfAppCommands ) ) ;

        /// <summary>
        ///     <see cref="RoutedUICommand" /> to view metadata for selected
        ///     object.
        /// </summary>
        public static readonly RoutedUICommand Metadata =
            new RoutedUICommand ( "Metadata" , nameof ( Metadata ) , typeof ( WpfAppCommands ) ) ;

        /// <summary><see cref="RoutedUICommand" /> to visit the selected type.</summary>
        public static readonly RoutedUICommand VisitType =
            new RoutedUICommand ( "VisitType" , nameof ( VisitType ) , typeof ( WpfAppCommands ) ) ;

        /// <summary>
        /// </summary>
        public static readonly RoutedUICommand LoginAD =
            new RoutedUICommand (
                                 "Login to 365"
                               , nameof ( LoginAD )
                               , typeof ( WpfAppCommands )
                                ) ;

        /// <summary>
        /// </summary>
        public static readonly RoutedUICommand Execute =
            new RoutedUICommand (
                                 "Execute code"
                               , nameof ( Execute )
                               , typeof ( WpfAppCommands )
                                ) ;

        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedUICommand CreateWorkspace = new RoutedUICommand("Create workspacE", nameof(CreateWorkspace),
                                                                                     typeof(WpfAppCommands));

        /// <summary>
        /// 
        /// </summary>
        public static readonly RoutedUICommand AddDocument =
            new RoutedUICommand (
                                 "Add document"
                               , nameof ( AddDocument )
                               , typeof ( WpfAppCommands )
                                ) ;
        public static readonly RoutedUICommand CreateSolution = new RoutedUICommand("Create solution", nameof(CreateSolution),
            typeof(WpfAppCommands));
        public static readonly RoutedUICommand CreateProject = new RoutedUICommand("Create project", nameof(CreateProject),
            typeof(WpfAppCommands));
        public static readonly RoutedUICommand OpenSolutionItem = new RoutedUICommand("Open solution item", nameof(OpenSolutionItem),
            typeof(WpfAppCommands));
        public static readonly RoutedUICommand LoadSolution =
            new RoutedUICommand(
                "Load solution"
                , nameof(LoadSolution)
                , typeof(WpfAppCommands)
            );
        public static readonly RoutedUICommand BrowseSymbols =
            new RoutedUICommand(
                "Browse symbols"
                , nameof(BrowseSymbols)
                , typeof(WpfAppCommands)
            );

        public static readonly RoutedUICommand SerializeContents=
            new RoutedUICommand(
                "Browse symbols"
                , nameof(BrowseSymbols)
                , typeof(WpfAppCommands)
            );

        public static readonly RoutedUICommand ViewDetails =
            new RoutedUICommand(
                "View details"
                , nameof(ViewDetails)
                , typeof(WpfAppCommands)
            );
        public static readonly RoutedUICommand ViewFlatHierarchy =
            new RoutedUICommand(
                "View Flat Hierarchy"
                , nameof(ViewFlatHierarchy)
                , typeof(WpfAppCommands)
            );

        public static readonly RoutedUICommand ViewNestedHierarchy =
            new RoutedUICommand(
                "View Nested Hierarchy"
                , nameof(ViewNestedHierarchy)
                , typeof(WpfAppCommands)
            );
        public static readonly RoutedUICommand ViewResources =
            new RoutedUICommand(
                "View resources"
                , nameof(ViewResources)
                , typeof(WpfAppCommands)
            );

    }
}