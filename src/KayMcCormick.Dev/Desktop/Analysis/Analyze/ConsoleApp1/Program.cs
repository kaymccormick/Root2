using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Data.SqlClient ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Net ;
using System.Net.Sockets ;
using System.Reactive.Subjects ;
using System.Text ;
using System.Text.Json ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using System.Xml.Linq ;
using AnalysisAppLib ;
using AnalysisAppLib.Project ;
using AnalysisControls ;
using Autofac ;
using Autofac.Core ;
using FindLogUsages ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Logging ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;
using NLog.Targets ;
using NStack ;
using Terminal.Gui ;
using JsonConverters = KayMcCormick.Dev.Serialization.JsonConverters ;

namespace ConsoleApp1
{
    internal sealed class AppContext

    {
        private readonly TermUi _termUi ;
        
        private IProjectBrowserViewModel _projectBrowserViewModel ;

        //public IEnumerable < Meta < Lazy < IAnalyzeCommand2 > > > AnalyzeCommands { get ; }

        public AppContext (
            ILifetimeScope           scope
          , IProjectBrowserViewModel projectBrowserViewModel,
            TermUi termUi
        )
        {
            _termUi = termUi ;
            Scope            = scope ;
            BrowserViewModel = projectBrowserViewModel ;
        }

        public ILifetimeScope Scope { get ; }

        public IProjectBrowserViewModel BrowserViewModel
        {
            // ReSharper disable once UnusedMember.Global
            get { return _projectBrowserViewModel ; }
            set { _projectBrowserViewModel = value ; }
        }

        public TermUi Ui { get { return _termUi ; } }
    }

    internal sealed class AppModule : Module
    {
        // ReSharper disable once AnnotateNotNullParameter
        protected override void Load ( ContainerBuilder builder )
        {
            base.Load ( builder ) ;
            var actionBlock = new ActionBlock < ILogInvocation > ( Program.Action ) ;
            builder.RegisterInstance ( actionBlock )
                   .As < ActionBlock < ILogInvocation > > ( )
                   .SingleInstance ( ) ;
            builder.RegisterType < AppContext > ( ).AsSelf ( ) ;
            builder.RegisterType < TermUi > ( ).AsSelf ( ) ;
        }
    }

    internal static class Program
    {
        private static void PopulateJsonConverters ( bool disableLogging )
        {
            if ( ! disableLogging )
            {
                if ( LogManager.Configuration != null )
                {
                    foreach ( var myJsonLayout in LogManager
                                                 .Configuration.AllTargets
                                                 .OfType < TargetWithLayout > ( )
                                                 .Select ( t => t.Layout )
                                                 .OfType < MyJsonLayout > ( ) )
                    {
                        var options = new JsonSerializerOptions ( ) ;
                        foreach ( var optionsConverter in myJsonLayout.Options.Converters )
                        {
                            options.Converters.Add ( optionsConverter ) ;
                        }

                        JsonConverters.AddJsonConverters ( options ) ;
                        myJsonLayout.Options = options ;
                    }
                }
            }
            else
            {
                var options = new JsonSerializerOptions ( ) ;
                JsonConverters.AddJsonConverters ( options ) ;
            }
        }

        private static ILogger Logger = null ;

        private static ApplicationInstance _appinst = null ;

        //============= Config [Edit these with your settings] =====================
        private static async Task < int > Main ( )
        {
            var ConsoleAnalysisProgramGuid = ApplicationInstanceIds.ConsoleAnalysisProgramGuid ;
            Subject<ILogger> subject = new Subject < ILogger > ();
            subject.Subscribe (
                               logger => {
                                   logger.Warn ( "Received logger" ) ;
                                   Debug.WriteLine ( "got logger" ) ;
                                   Logger = logger ;
                                   if ( _appinst != null )
                                   {
                                       _appinst.Logger = logger ;
                                   }
                               }
                              ) ;
            Task.Run (
                      ( ) => AppLoggingConfigHelper
                            .EnsureLoggingConfiguredAsync (
                                                           Console.WriteLine
                                                         , new AppLoggingConfiguration
                                                           {
                                                               IsEnabledConsoleTarget = true
                                                             , MinLogLevel =
                                                                   LogLevel.Trace
                                                           }
                                                         , subject
                                                          )
                            .ContinueWith ( task => Console.WriteLine ( "Logger async complete." ) )
                     ) ;

            Console.WriteLine ( "here" ) ;
            Console.WriteLine ( ConsoleAnalysisProgramGuid.ToString ( "X" ) ) ;
            Init ( ) ;
            _appinst = new ApplicationInstance (
                                                new ApplicationInstance.
                                                    ApplicationInstanceConfiguration (
                                                                                      message
                                                                                          => {
                                                                                      }
                                                                                    , ConsoleAnalysisProgramGuid
                                                                                     )
                                               ) ;
            using ( _appinst )

            {
                _appinst.AddModule ( new AppModule ( ) ) ;
                _appinst.AddModule ( new AnalysisAppLibModule ( ) ) ;
                PopulateJsonConverters ( false ) ;
                ILifetimeScope scope ;
                try
                {
                    scope = _appinst.GetLifetimeScope ( ) ;
                }
                catch ( ContainerBuildException buildException )
                {
                    Console.WriteLine ( buildException.Message ) ;
                    Console.WriteLine ( "Please contact your administrator for assistance." ) ;
                    return 1 ;
                }

                AppContext context ;
                try
                {
                    context = scope.Resolve < AppContext > ( ) ;
                }
                catch ( DependencyResolutionException depex )
                {
                    Exception ex1 = depex ;
                    while ( ex1 != null )
                    {
                        ex1 = ex1.InnerException ;
                    }

                    return 1 ;
                }
                catch ( Exception )
                {
                    return 1 ;
                }

                return await MainCommandAsync ( context ) ;
            }
        }

        public static void Action ( ILogInvocation invocation )
        {
            var json = JsonSerializer.Serialize ( invocation ) ;
            Console.WriteLine ( json ) ;
            // $"{invocation.MethodDisplayName}\t{invocation.SourceLocation}\t{invocation.Msgval}\t{invocation.Arguments}"
            // ) ;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private static async Task < int > MainCommandAsync ( [ NotNull ] AppContext context )
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
#if MSBUILDLOCATOR
            // var instances = MSBuildLocator.RegisterDefaults ( ) ;
            var menu = new Menu ( "VS Instance" ) ;
            var vsInstances = MSBuildLocator.QueryVisualStudioInstances (
                                                                         new
                                                                         VisualStudioInstanceQueryOptions
                                                                         {
                                                                             DiscoveryTypes =
                                                                                 DiscoveryType
                                                                                    .VisualStudioSetup
                                                                         }
                                                                        ) ;
            var visualStudioInstances =
                vsInstances as VisualStudioInstance[] ?? vsInstances.ToArray ( ) ;

            string RenderFunc ( VisualStudioInstance inst1 )
            {
                return
                    $"* {inst1.Name,- 30} {inst1.Version.Major:00}.{inst1.Version.Minor:00}.{inst1.Version.Build:00000}.{inst1.Version.MinorRevision:0000}  [{inst1.VisualStudioRootPath}]" ;
            }

            var choices = visualStudioInstances.Select (
                                                        x => new MenuWrapper < VisualStudioInstance
                                                        > ( x , RenderFunc )
                                                       ) ;
            menu.Config.SelectedAppearence =
                new Configuration.SelectedColor { BackgroundColor = ConsoleColor.Yellow } ;
            var selected = menu.Render ( choices ) ;
#if false
            var i2 = (
                         from inst in visualStudioInstances
                         where inst.Version.Major == 15
                         orderby inst.Version descending
                         select inst ).FirstOrDefault ( ) ;

            if ( i2 != null )
            {
#endif
            var i2 = selected.Instance ;
            Logger.Warn ( "Selected instance {instance} {path}" , i2.Name , i2.MSBuildPath ) ;
            MSBuildLocator.RegisterInstance ( i2 ) ;
#if false
        }
#endif
            Console.WriteLine ( "" ) ;

#endif

//await            xx ( ) ;
#if false
            var i = 0 ;
            var browserNodeCollection = context.BrowserViewModel.RootCollection ;
            var nodes = new List < IBrowserNode > ( browserNodeCollection.Count ) ;
            foreach ( var browserNode in browserNodeCollection )
            {
                i += 1 ;

                Console.WriteLine ( $"{i}: {browserNode.Name}" ) ;
                nodes.Add ( browserNode ) ;
                if ( browserNode is IProjectBrowserNode project )
                {
                    Console.WriteLine ( $"\tSolutionPath is {project.SolutionPath}" ) ;
                    Console.WriteLine (
                                       $"\tConfiguration property Platform is {project.Platform ?? "Null"}"
                                      ) ;
                    Console.WriteLine ( $"\tRepositoryUrl is {project.RepositoryUrl}" ) ;
                }
            }

            Console.Write ( typeof ( ContainerBuilder ).Assembly.FullName ) ;
            IProjectBrowserNode projectNode = null ;
            for ( ; ; )
            {
                var key = Console.ReadKey ( ) ;

                if ( ! char.IsDigit ( key.KeyChar ) )
                {
                    continue ;
                }

                var selection = ( int ) char.GetNumericValue ( key.KeyChar ) ;

                projectNode = nodes[ selection - 1 ] as IProjectBrowserNode ;
                if ( projectNode == null )
                {
                    continue ;
                }

                break ;
            }


            var j = 0 ;
            Meta < Lazy < IAnalyzeCommand3 > > command2 = null ;
            // foreach ( var cmd in context.AnalyzeCommands )
            // {
            // Console.WriteLine("Command #" + j);
            // foreach ( var keyValuePair in cmd.Metadata )
            // {
            // Console.WriteLine ( $"  {keyValuePair.Key}: {keyValuePair.Value}" ) ;
            // }

            // command2 = cmd ;
            // }

            Console.ReadLine ( ) ;

            Console.WriteLine ( projectNode.SolutionPath ) ;
            Console.ReadLine ( ) ;

            //    ITargetBlock <RejectedItem> rejectTarget = new ActionBlock < RejectedItem > (item => Console.WriteLine($"Reject: {item.Statement}"));
            if ( command2 != null )
            {
                await command2.Value.Value.AnalyzeCommandAsync ( projectNode ) ;
            }
            else
            {
                Console.WriteLine ( "No commanad" ) ;
                //return 1 ;
            }
#endif

            context.Ui.Init();
            // object o11 = XamlReader.Parse(
            //                             "<Toplevel xmlns=\"clr-namespace:Terminal.Gui;assembly=Terminal.Gui\">\r\n</Toplevel>\r\n"
            //                            ) ;
            context.Ui.Run ( ) ;

            foreach ( var projFile in Directory.EnumerateFiles (
                                                                @"e:\kay2020\source"
                                                              , "*.csproj"
                                                              , SearchOption.AllDirectories
                                                               ) )
            {
                //using Microsoft.CodeAnalysis.MSBuild;

                Console.WriteLine ( projFile ) ;
            }

            var b = new SqlConnectionStringBuilder
                    {
                        IntegratedSecurity = true
                      , DataSource         = @".\sql2017"
                      , InitialCatalog     = "syntaxdb"
                    } ;
            var c = new SqlConnection ( b.ConnectionString ) ;
            await c.OpenAsync ( ) ;

            var bb2 = new SqlCommand (
                                      "insert into syntaxexample (example, syntaxkind, typename) values (@example, @kind, @typename)"
                                    , c
                                     ) ;


            var model1 = context.Scope.Resolve < ITypesViewModel > ( ) ;
            var set = new HashSet < Type > ( ) ;
            NewMethod ( model1.Root.SubTypeInfos , set ) ;
            var syntaxdict =
                new Dictionary < Type , Tuple < X , List < Tuple < SyntaxNode , string > > > > ( ) ;
            var xxx1 = new Dictionary < string , X > ( ) ;
            var xxx = new Dictionary < SyntaxKind , X > ( ) ;
            var r = new Random ( ) ;
            foreach ( var enumerateFile in Directory.EnumerateFiles (
                                                                     @"e:\kay2020\source"
                                                                   , "*.cs"
                                                                   , SearchOption.AllDirectories
                                                                    ) )
            {
                var st = CSharpSyntaxTree.ParseText (
                                                     File.ReadAllText ( enumerateFile )
                                                   , null
                                                   , enumerateFile
                                                    ) ;
                // var y = CSharpCompilation.Create ( "x" , new [] {x} ) ;
                // foreach ( var diagnostic in y.GetDiagnostics ( ) )
                // {
                // Console.WriteLine(diagnostic.Location.GetMappedLineSpan().Path);
                // Console.WriteLine(CSharpDiagnosticFormatter.Instance.Format(diagnostic));
                // }

                foreach ( var o1 in st.GetCompilationUnitRoot ( )
                                      .DescendantNodesAndTokensAndSelf ( ) )
                {
                    if ( o1.IsToken )
                    {
                        var syntaxToken = o1.AsToken ( ) ;
                        if ( ! xxx1.TryGetValue ( syntaxToken.ValueText , out var x2 ) )
                        {
                            x2                            = new X ( ) ;
                            xxx1[ syntaxToken.ValueText ] = x2 ;
                        }

                        x2.len ++ ;
                        continue ;
                    }

                    var o = o1.AsNode ( ) ;
                    if ( o == null )
                    {
                        continue ;
                    }

                    if ( ! xxx.TryGetValue ( o.Kind ( ) , out var x1 ) )
                    {
                        x1                = new X ( ) ;
                        xxx[ o.Kind ( ) ] = x1 ;
                    }

                    if ( x1 != null )
                    {
                        x1.len ++ ;
                    }


                    if ( r.Next ( 9 ) == 1
                         && set.Contains ( o.GetType ( ) ) )
                    {
                        o.DescendantTokens().Select (( token , i ) => new XElement(XName.Get(token.Kind().ToString()), new XText(token.ValueText)));
                        foreach ( var descendantToken in o.DescendantTokens ( ) )
                        {
                            
                        }
                        if ( ! syntaxdict.TryGetValue ( o.GetType ( ) , out var l ) )
                        {
                            l = Tuple.Create (
                                              new X ( )
                                            , new List < Tuple < SyntaxNode , string > > ( )
                                             ) ;
                            syntaxdict[ o.GetType ( ) ] = l ;
                        }

                        l.Item1.len += o.ToString ( ).Length ;
                        l.Item2.Add ( Tuple.Create ( o , o.ToString ( ) ) ) ;
                        var example1 = o.NormalizeWhitespace ( ).ToString ( ) ;

                        bb2.Parameters.Clear ( ) ;
                        bb2.Parameters.AddWithValue ( "@example" ,  example1 ) ;
                        bb2.Parameters.AddWithValue ( "@kind" ,     o.RawKind ) ;
                        bb2.Parameters.AddWithValue ( "@typename" , o.GetType ( ).Name ) ;

                        await bb2.ExecuteNonQueryAsync ( ) ;
                        if ( l.Item2.Count >= 100 )
                        {
                            set.Remove ( o.GetType ( ) ) ;
                        }
                    }
                }

                if ( ! set.Any ( ) )
                {
                    break ;
                }
            }

            foreach ( var k in xxx1 )
            {
                Console.WriteLine ( $"{k.Key} = {k.Value.len}" ) ;
            }

            foreach ( var k in xxx )
            {
                Console.WriteLine ( $"{k.Key} = {k.Value.len}" ) ;
            }

            foreach ( var keyValuePair in syntaxdict )
            {
                Console.WriteLine ( $"{keyValuePair.Key.Name}" ) ;
                Console.WriteLine (
                                   ( double ) keyValuePair.Value.Item1.len
                                   / keyValuePair.Value.Item2.Count
                                  ) ;
            }

            return 1 ;
            // }


            // if ( o.ToString ( ).Length < 80 )
            // {
            // Console.WriteLine ( o.GetType ( ).Name ) ;
            // Console.WriteLine ( o ) ;
            // i1 ++ ;
            // if ( i1% 25 == 0 )
            // {
            // Console.ReadLine ( ) ;
            // }
            // }
        }


        // ReSharper disable once UnusedMember.Local
        private static async Task ListenNetAsync ( )
        {
            var tcp = new TcpListener (
                                       new IPEndPoint ( IPAddress.Parse ( "10.25.0.102" ) , 17727 )
                                      ) ;
            tcp.Start ( ) ;
            var client = await tcp.AcceptTcpClientAsync ( ) ;
            var s = client.GetStream ( ) ;
            var r = new StreamReader ( s ) ;
            var t = new StreamWriter ( s , Encoding.ASCII ) ;
            await t.WriteLineAsync ( "Welcome to my server" ) ;
            await t.FlushAsync ( ) ;
            var x = await r.ReadLineAsync ( ) ;
            await t.WriteLineAsync ( x ) ;
            await t.FlushAsync ( ) ;
        }

        private static void NewMethod (
            [ NotNull ] AppTypeInfoCollection subTypeInfos
          , HashSet < Type >                                 set
        )
        {
            foreach ( AppTypeInfo rootSubTypeInfo in subTypeInfos )
            {
                if ( rootSubTypeInfo.Type.IsAbstract == false )
                {
                    set.Add ( rootSubTypeInfo.Type ) ;
                }

                NewMethod ( rootSubTypeInfo.SubTypeInfos , set ) ;
            }
        }

        private static void Init ( ) { }
    }

    internal class ListView2Base < T > : ListView
        where T : class , IEnumerable < T >
    {
        private readonly List < ItemData < T > > _itemDatas = new List < ItemData < T > > ( 20 ) ;
        private readonly ListWrapper < T >       _source ;

        protected ListView2Base ( Rect rect , [ NotNull ] List < T > list ) : base (
                                                                                    rect
                                                                                  , new ListWrapper
                                                                                        < T > (
                                                                                               list
                                                                                                  .ToList ( )
                                                                                              )
                                                                                   )
        {
            _source = ( ListWrapper < T > ) Source ;
            for ( var i = 0 ; i < _source.Count ; i ++ )
            {
                _itemDatas.Add ( new ItemData < T > ( ) ) ;
            }
        }

        public List < T > List { get { return _source.List ; } }

        public override bool ProcessKey ( KeyEvent kb )
        {
            if ( kb.Key == Key.CursorRight )
            {
                var selectedItem = SelectedItem ;
                var e = List[ selectedItem ] ;
                var d = _itemDatas[ selectedItem ] ;
                if ( d.Expanded )
                {
                    return true ;
                }

                d.Expanded = true ;
                if ( d.RemovedChildren != null )
                {
                    List.InsertRange ( selectedItem + 1 , d.RemovedChildren ) ;
                    d.SubtreeCount = d.RemovedChildren.Count ;
                }
                else
                {
                    d.InsertedChildren = e.ToList ( ) ;
                    List.InsertRange ( selectedItem + 1 , d.InsertedChildren ) ;
                    var insertedChildrenCount = d.InsertedChildren.Count ;
                    d.SubtreeCount = insertedChildrenCount ;
                }

                var c = d.Container ;
                while ( c != null )
                {
                    ItemData < T > itemData = null ;
                    for ( var i = selectedItem ; i >= 0 ; i -- )
                    {
                        if ( ReferenceEquals ( List[ i ] , c ) )
                        {
                            itemData = _itemDatas[ i ] ;
                            break ;
                        }
                    }

                    if ( itemData != null )
                    {
                        itemData.SubtreeCount += d.SubtreeCount ;
                        c                     =  itemData.Container ;
                    }
                    else
                    {
                        c = null ;
                    }
                }

                _itemDatas.InsertRange (
                                        selectedItem + 1
                                      , Enumerable
                                       .Repeat ( 1 , d.SubtreeCount )
                                       .Select ( x => new ItemData < T > { Container = e } )
                                       ) ;

                Source       = _source ;
                SelectedItem = selectedItem ;
                // foreach ( var r in x.Children )
                // {
                return true ;
                // }
            }

            if ( kb.Key == Key.CursorLeft )
            {
                var selectedItem = SelectedItem ;

                var d = _itemDatas[ selectedItem ] ;
                if ( ! d.Expanded )
                {
                    return true ;
                }

                var c = d.Container ;
                while ( c != null )
                {
                    ItemData < T > itemData = null ;
                    for ( var i = selectedItem ; i >= 0 ; i -- )
                    {
                        if ( ReferenceEquals ( List[ i ] , c ) )
                        {
                            itemData = _itemDatas[ i ] ;
                            break ;
                        }
                    }

                    if ( itemData != null )
                    {
                        itemData.SubtreeCount -= d.SubtreeCount ;
                        c                     =  itemData.Container ;
                    }
                    else
                    {
                        c = null ;
                    }
                }

                d.RemovedChildren = List.GetRange ( selectedItem + 1 , d.SubtreeCount ) ;
                List.RemoveRange ( selectedItem + 1 , d.SubtreeCount ) ;
                d.Expanded   = false ;
                Source       = _source ;
                SelectedItem = selectedItem ;
                return true ;
            }

            return base.ProcessKey ( kb ) ;
        }

        private sealed class ItemData < T2 >
        {
            public T2 Container { get ; set ; }

            public bool Expanded { get ; set ; }

            public List < T2 > InsertedChildren { get ; set ; }

            public int SubtreeCount { get ; set ; }

            public List < T2 > RemovedChildren { get ; set ; }
        }
    }

    internal sealed class ListView2 : ListView2Base < ResourceNodeInfo >
    {
        public ListView2 ( Rect rect , [ NotNull ] List < ResourceNodeInfo > list ) :
            base ( rect , list )
        {
        }
    }

    public sealed class ListWrapper < T > : IListDataSource
    {
        private readonly int      count ;
        private readonly BitArray marks ;

        public ListWrapper ( [ NotNull ] List < T > source )
        {
            count = source.Count ;
            marks = new BitArray ( count ) ;
            List  = source ;
        }

        public List < T > List { get ; }

        public int Count { get { return List.Count ; } }

        public void Render (
            [ NotNull ] ListView container
          , ConsoleDriver        driver
          , bool                 marked
          , int                  item
          , int                  col
          , int                  line
          , int                  width
        )
        {
            container.Move ( col , line ) ;
            var t = List[ item ] ;

            RenderUstr (
                        driver
                      , t.ToString ( )
                      , col
                      , line
             ,          width
                       ) ;
        }

        public bool IsMarked ( int item )
        {
            if ( item    >= 0
                 && item < count )
            {
                return marks[ item ] ;
            }

            return false ;
        }

        public void SetMark ( int item , bool value )
        {
            if ( item    >= 0
                 && item < count )
            {
                marks[ item ] = value ;
            }
        }

        private void RenderUstr (
            ConsoleDriver       driver
          , [ NotNull ] ustring ustr
            // ReSharper disable once UnusedParameter.Local
          , int col
            // ReSharper disable once UnusedParameter.Local
          , int line
          , int width
        )
        {
            var byteLen = ustr.Length ;
            var used = 0 ;
            for ( var i = 0 ; i < byteLen ; )
            {
                var (rune , size) = Utf8.DecodeRune ( ustr , i , i - byteLen ) ;
                var columnWidth = Rune.ColumnWidth ( rune ) ;
                if ( used + columnWidth >= width )
                {
                    break ;
                }

                driver.AddRune ( rune ) ;
                used += columnWidth ;
                i    += size ;
            }

            for ( ; used < width ; used ++ )
            {
                driver.AddRune ( ' ' ) ;
            }
        }
    }

    // ReSharper disable once UnusedType.Global
    internal class MenuWrapper < T >
    {
        private readonly T                   _instance ;
        private readonly Func < T , string > _renderFunc ;

        public MenuWrapper ( T instance , Func < T , string > renderFunc )
        {
            _instance   = instance ;
            _renderFunc = renderFunc ;
        }

        public T Instance { get { return _instance ; } }

        #region Overrides of Object
        public override string ToString ( ) { return _renderFunc ( Instance ) ; }
        #endregion
    }

    internal sealed class X
    {
        public int len ;
    }
}
