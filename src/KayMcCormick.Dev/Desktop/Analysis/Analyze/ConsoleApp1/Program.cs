using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Collections.Immutable ;
using System.Data ;
using System.Data.SqlClient ;
using System.Data.SqlTypes ;
using System.IO ;
using System.Linq ;
using System.Net ;
using System.Net.Sockets ;
using System.Reactive.Subjects ;
using System.Text ;
using System.Text.Json ;
using System.Threading.Tasks ;
using System.Windows.Markup ;
using System.Xml ;
using System.Xml.Linq ;
using AnalysisAppLib ;
using AnalysisAppLib.Project ;
using AnalysisAppLib.XmlDoc ;
using AnalysisControls ;
using AnalysisControls.ViewModel ;
using Autofac ;
using Autofac.Core ;
using CommandLine ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Lib.Wpf.Command ;
using Microsoft.Build.Locator ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.MSBuild ;
using Microsoft.CodeAnalysis.Text ;
using Microsoft.CodeAnalysis.VisualBasic.Symbols ;
using NLog ;
using NLog.Targets ;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory ;
using JsonConverters = KayMcCormick.Dev.Serialization.JsonConverters ;

namespace ConsoleApp1
{
    internal sealed class Program
    {
        private const string ModelXamlFilename = @"C:\data\logs\model.xaml" ;

        private const string SolutionFilePath =
            @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\reanalyze2\src\KayMcCormick.Dev\ManagedProd.sln" ;

        private const string _pocoPrefix          = "Poco" ;
        private const string _collectionSuffix    = "Collection" ;
        private const string _pocosyntaxnamespace = "PocoSyntax" ;
        private const string _icollection         = "ICollection" ;

        private static readonly string[] AssemblyRefs =
        {
            @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\Microsoft.CSharp.dll"
          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\mscorlib.dll"
          , @"C:\Users\mccor.LAPTOP-T6T0BN1K\.nuget\packages\nlog\4.6.8\lib\net45\NLog.dll"
          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Configuration.dll"
          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Core.dll"
          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Data.DataSetExtensions.dll"
          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Data.dll"
          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.dll"
          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.IO.Compression.dll"
          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Net.Http.dll"
          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Runtime.Serialization.dll"
          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.ServiceModel.dll"
          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Transactions.dll"
          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Xml.dll"
          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\System.Xml.Linq.dll"
          , @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.7.2\Facades\netstandard.dll"
        } ;

        private static ILogger Logger ;

        private static          ApplicationInstance _appinst ;
        private static readonly string              Pocosyntaxtoken = @"PocoSyntaxToken" ;
        private static readonly string              _ilist          = "IList" ;
        private static readonly string              _ienumerable    = "IEnumerable" ;
        static Program ( ) { Logger = null ; }

        [ NotNull ] public static string PocoPrefix { get { return _pocoPrefix ; } }

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

        //============= Config [Edit these with your settings] =====================
        // ReSharper disable once UnusedParameter.Local
        private static async Task < int > Main ( string[] args )
        {
            var ConsoleAnalysisProgramGuid = ApplicationInstanceIds.ConsoleAnalysisProgramGuid ;
            var subject = new Subject < ILogger > ( ) ;
            subject.Subscribe (
                               logger => {
                                   logger.Warn ( "Received logger" ) ;
                                   DebugUtils.WriteLine ( "got logger" ) ;
                                   Logger = logger ;
                                   if ( _appinst != null )
                                   {
                                       _appinst.Logger = logger ;
                                   }
                               }
                              ) ;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed


            _appinst = new ApplicationInstance (
                                                new ApplicationInstance.
                                                    ApplicationInstanceConfiguration (
                                                                                      message => {
                                                                                      }
                                                                                    , ConsoleAnalysisProgramGuid
                                                                                     )
                                               ) ;
            using ( _appinst )

            {
                _appinst.AddModule ( new AppModule ( ) ) ;
                _appinst.AddModule ( new AnalysisAppLibModule ( ) ) ;
                _appinst.AddModule ( new AnalysisControlsModule ( ) ) ;
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

                Options myOptions = new Options();
                bool fExit = false ;
                var program = context.Scope.Resolve < Program > ( ) ;
                CommandLine.Parser.Default.ParseArguments < Options > ( args )
                           .WithParsed ( o => myOptions = o ).WithNotParsed(errors => fExit = true) ;
                if ( fExit ) return 1 ;
                return await program.MainCommandAsync(context, myOptions);
            }
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private async Task < int > MainCommandAsync ( [ NotNull ] AppContext context, [ NotNull ] Options options )
        {
            var cmds = context.Scope.Resolve < IEnumerable < IDisplayableAppCommand > > ( ) ;
            if ( ! string.IsNullOrEmpty ( options.Action )
                 && cmds.All ( a => a.DisplayName != options.Action ) )
            {
                throw new InvalidOperationException ( ) ;
            }
            SelectVsInstance ( ) ;

            context.Options = options ;
            await RunConsoleUiAsync ( context  ) ;

            return 1 ;
        }

        // ReSharper disable once UnusedMember.Local
#pragma warning disable 1998
        private static async Task SelectProjectAsync ( [ NotNull ] AppContext context )
#pragma warning restore 1998
        {
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
            IProjectBrowserNode projectNode ;
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

            Console.ReadLine ( ) ;

            Console.WriteLine ( projectNode.SolutionPath ) ;
            Console.ReadLine ( ) ;
        }


        [ TitleMetadata ( "Build Types View" ) ]
        [ UsedImplicitly ]
#pragma warning disable 1998
        public async Task BuildTypeViewAsync (
#pragma warning restore 1998
            [ NotNull ] IBaseLibCommand command
          , [ NotNull ] AppContext      context
        )
        {
            if ( command == null )
            {
                throw new ArgumentNullException ( nameof ( command ) ) ;
            }

            if ( context == null )
            {
                throw new ArgumentNullException ( nameof ( context ) ) ;
            }

            DebugUtils.WriteLine ( "Begin initialize TypeViewModel" ) ;
            var typesViewModel =
                context.Scope.Resolve < TypesViewModel > (
                                                          new TypedParameter (
                                                                              typeof ( bool )
                                                                            , false
                                                                             )
                                                         ) ;
            DebugUtils.WriteLine (
                                  $"InitializationDateTime : {typesViewModel.InitializationDateTime}"
                                 ) ;

            var sts = context.Scope.Resolve < ISyntaxTypesService > ( ) ;
            var collectionMap = sts.CollectionMap ( ) ;

            SyntaxTypesService.LoadSyntax ( typesViewModel , collectionMap ) ;
            // foreach ( AppTypeInfo ati in typesViewModel.Map.Values )
            // {
            // typesViewModel.PopulateFieldTypes ( ati ) ;
            // }

            typesViewModel.DetailFields ( ) ;
            WriteThisTypesViewModel ( typesViewModel ) ;
            DumpModelToJson ( context , typesViewModel ) ;
        }

        [ TitleMetadata ( "Load Syntax Examples" ) ]
        [ UsedImplicitly ]
        public static async Task LoadSyntaxExamplesAsync ( AppContext context , SqlConnection c )
        {
            var bb2 = new SqlCommand (
                                      "insert into syntaxexample2 (example, syntaxkind, typename, tokens) values (@example, @kind, @typename, @tokens)"
                                    , c
                                     ) ;


            var set = new HashSet < Type > ( ) ;
            var model = context.Scope.Resolve < TypesViewModel > ( ) ;
            PopulateSet ( model.Root.SubTypeInfos , set ) ;
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

                var comp = st.GetCompilationUnitRoot ( ) ;
                var triviaDict = new Dictionary < SyntaxKind , SyntaxInfo > ( ) ;
                foreach ( var syntaxTrivia in comp.DescendantTrivia ( x => true , true ) )
                {
                    var kind = syntaxTrivia.Kind ( ) ;
                    if ( ! triviaDict.TryGetValue ( kind , out var info ) )
                    {
                        info               = new SyntaxInfo { Kind = kind } ;
                        triviaDict[ kind ] = info ;
                    }

                    info.Count ++ ;
                }

                Console.Write (
                               string.Join (
                                            ", "
                                          , triviaDict.Select (
                                                               kv => $"{kv.Key} = {kv.Value.Count}"
                                                              )
                                           )
                              ) ;

                foreach ( var o1 in comp.DescendantNodesAndTokensAndSelf ( ) )
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
                        var doc = new XDocument (
                                                 new XElement (
                                                               XName.Get ( "Tokens" )
                                                             , o.DescendantTokens ( )
                                                                .Select (
                                                                         ( token , i )
                                                                             => new XElement (
                                                                                              XName
                                                                                                 .Get (
                                                                                                       token
                                                                                                          .Kind ( )
                                                                                                          .ToString ( )
                                                                                                      )
                                                                                            , new
                                                                                                  XText (
                                                                                                         token
                                                                                                            .ToString ( )
                                                                                                        )
                                                                                             )
                                                                        )
                                                              )
                                                ) ;
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
                        bb2.Parameters.Add (
                                            new SqlParameter ( "@tokens" , SqlDbType.Xml )
                                            {
                                                Value = new SqlXml ( doc.CreateReader ( ) )
                                            }
                                           ) ;

                        try
                        {
                            await bb2.ExecuteNonQueryAsync ( ) ;
                        }
                        catch ( Exception ex )
                        {
                            DebugUtils.WriteLine ( ex.ToString ( ) ) ;
                        }

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
        }

        [ ItemNotNull ]
        [ UsedImplicitly ]
        private static async Task < SqlConnection > DumpSyntaxExamplesAsync ( )
        {
            var b = new SqlConnectionStringBuilder
                    {
                        IntegratedSecurity = true
                      , DataSource         = @".\sql2017"
                      , InitialCatalog     = "syntaxdb"
                    } ;
            var c = new SqlConnection ( b.ConnectionString ) ;
            await c.OpenAsync ( ) ;

            var bb1 = new SqlCommand (
                                      "select example, syntaxkind, typename, tokens, id from syntaxexample2"
                                    , c
                                     ) ;
            var result = await bb1.ExecuteReaderAsync ( ) ;
            var d1 = new ExampleDict ( ) ;
            for ( ; ; )
            {
                var moreRows = await result.ReadAsync ( ) ;
                if ( ! moreRows )
                {
                    break ;
                }

                var kind = result.GetInt32 ( 1 ) ;
                var k = Enum.GetName ( typeof ( SyntaxKind ) , ( uint ) kind ) ;
                Enum.TryParse < SyntaxKind > ( k , out var k2 ) ;
                ArrayList l ;
                if ( ! d1.Contains ( k2 ) )
                {
                    l        = new ArrayList ( ) ;
                    d1[ k2 ] = l ;
                }
                else
                {
                    l = ( ArrayList ) d1[ k2 ] ;
                }

                var reader1 = result.GetXmlReader ( 3 ) ;
                // ReSharper disable once MethodHasAsyncOverload
#pragma warning disable VSTHRD103 // Call async methods when in an async method
                reader1.MoveToContent ( ) ;
#pragma warning restore VSTHRD103 // Call async methods when in an async method
                var tokens = ( XElement ) XNode.ReadFrom ( reader1 ) ;
                var x = tokens.Elements ( )
                              .Select (
                                       element => new SToken (
                                                              element.Name.LocalName
                                                            , element.Value
                                                             )
                                      )
                              .ToList ( ) ;
                var exampleSyntax = new ExampleSyntax (
                                                       kind
                                                     , result.GetString ( 0 )
                                                     , result.GetString ( 2 )
                                                     , x
                                                     , result.GetInt32 ( 4 )
                                                      ) ;
                l.Add ( exampleSyntax ) ;
            }

            var d2 = ( IDictionary ) d1 ;
            using ( var fileStream = File.OpenWrite ( @"C:\data\logs\xaml" ) )
            {
                XamlWriter.Save ( d2 , fileStream ) ;
            }

            return c ;
        }

        private static void DumpModelToJson (
            [ NotNull ] AppContext     context
          , [ NotNull ] TypesViewModel typesViewModel
        )
        {
            using ( var utf8Json = File.Open ( @"C:\temp\out.json" , FileMode.Create ) )
            {
                var infos = typesViewModel.Map.Values.Cast < AppTypeInfo > ( ).ToList ( ) ;
                var writer = new Utf8JsonWriter (
                                                 utf8Json
                                               , new JsonWriterOptions { Indented = true }
                                                ) ;
                var jsonSerializerOptions = context.Scope.Resolve < JsonSerializerOptions > ( ) ;
                jsonSerializerOptions.WriteIndented = true ;
                if ( ! jsonSerializerOptions
                      .Converters.Select ( conv => conv.CanConvert ( typeof ( Type ) ) )
                      .Any ( ) )

                {
                    throw new InvalidOperationException ( "no type converter" ) ;
                }

                foreach ( var jsonConverter in jsonSerializerOptions.Converters )
                {
                    Console.WriteLine ( jsonConverter ) ;
                }

                try
                {
                    JsonSerializer.Serialize ( writer , infos , jsonSerializerOptions ) ;
                }
                catch ( Exception )
                {
                    // ignored
                }

                writer.Flush ( ) ;
            }
        }

        private static void SelectVsInstance ( )
        {
            var vsInstances = MSBuildLocator
                             .QueryVisualStudioInstances (
                                                          new VisualStudioInstanceQueryOptions
                                                          {
                                                              DiscoveryTypes =
                                                                  DiscoveryType.VisualStudioSetup
                                                          }
                                                         )
                             .First ( inst => inst.Version.Major == 15 ) ;
            MSBuildLocator.RegisterInstance ( vsInstances ) ;
#if CONSOLEMENU
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
                                                        x => new MenuWrapper < VisualStudioInstance > (
                                                                                                       x
, RenderFunc
                                                                                                      )
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
            if ( i2.Name != null )
            {
                Logger?.Warn (
                             "Selected instance {instance} {path}"
, ( object ) i2.Name
, ( object ) i2.MSBuildPath
                            ) ;
            }

            MSBuildLocator.RegisterInstance ( i2 ) ;
#if false
        }
#endif
            Console.WriteLine ( "" ) ;

#endif
        }

        // ReSharper disable once UnusedMember.Local
        private static void WriteTypesViewModel ( )
        {
            var model = new TypesViewModel ( ) ;
            model.BeginInit ( ) ;
            model.EndInit ( ) ;
            WriteThisTypesViewModel ( model ) ;
        }

        public static void WriteThisTypesViewModel ( [ NotNull ] TypesViewModel model )
        {
            DebugUtils.WriteLine ( $"Writing {ModelXamlFilename}" ) ;
            var writer = XmlWriter.Create (
                                           ModelXamlFilename
                                         , new XmlWriterSettings { Indent = true , Async = true }
                                          ) ;
            XamlWriter.Save ( model , writer ) ;
            writer.Close ( ) ;
        }

        // ReSharper disable once UnusedMember.Local
        [ TitleMetadata ( "Process solution" ) ]
        [ UsedImplicitly ]
        public async Task ProcessSolutionAsync (
            IBaseLibCommand        command
          , [ NotNull ] AppContext context
        )
        {
            //options.WriteIndented = true ;
            var workspace = MSBuildWorkspace.Create ( ) ;
            var optionsSolutionFile = context.Options?.SolutionFile ?? SolutionFilePath;
            if ( string.IsNullOrEmpty ( optionsSolutionFile ) )
            {
                throw new InvalidOperationException ( "No solution file" ) ;
            }

            var solution = await workspace.OpenSolutionAsync ( optionsSolutionFile ) ;
            var documentsOut = new List < CodeElementDocumentation > ( ) ;
            var solutionProjects = solution.Projects ;

            foreach ( var project in solutionProjects.Where (
                                                             proj => proj.Name != "Explorer"
                                                                     && proj.CompilationOptions
                                                                           ?.OutputKind
                                                                     == OutputKind
                                                                        .DynamicallyLinkedLibrary
                                                                     || proj.CompilationOptions
                                                                           ?.OutputKind
                                                                     == OutputKind
                                                                        .WindowsApplication
                                                            ) )
            {
                var callers = new List < CallerInfo > ( ) ;

                DebugUtils.WriteLine ( $"{project} {project.CompilationOptions.OutputKind}" ) ;
                // ReSharper disable once UnusedVariable
                var compilation = await project.GetCompilationAsync ( ) ;
                foreach ( var diagnostic in compilation
                                           .GetDiagnostics ( )
                                           .Where (
                                                   d => ! d.IsSuppressed
                                                        && d.Severity >= DiagnosticSeverity.Info
                                                  ) )
                {
                    DebugUtils.WriteLine ( diagnostic.ToString ( ) ) ;
                }

                var compilationAssembly = compilation.Assembly ;
                foreach(var tn in compilationAssembly.TypeNames )
                {
                    DebugUtils.WriteLine(compilationAssembly.Name);
                    DebugUtils.WriteLine(tn);
                }
                foreach ( var symbol in compilation.GetSymbolsWithName (
                                                                        ( s ) => true
                                                                       ) )
                {
                    if ( !symbol.ContainingAssembly.Equals( compilationAssembly) )
                    {
                        DebugUtils.WriteLine ( $"Skipping {symbol}" ) ;
                        continue ;
                    }

                    DebugUtils.WriteLine (
                                          string.Join (
                                                       ";"
                                                     , compilation.SyntaxTrees.Select (
                                                                                       dt => dt
                                                                                          .FilePath
                                                                                      )
                                                      )
                                         ) ;
                    DebugUtils.WriteLine (
                                          $"{symbol.ToDisplayString ( )} {symbol.DeclaredAccessibility}"
                                         ) ;
                    
                    var res =
                        await Microsoft.CodeAnalysis.FindSymbols.SymbolFinder.FindCallersAsync (
                                                                                                symbol
                                                                                              , solution
                                                                                               ) ;
                    var uses = 0 ;
                    foreach ( var use in res )
                    {
                        // DebugUtils.WriteLine ( "Symbol kind "   + use.CalledSymbol.Kind ) ;
                        // DebugUtils.WriteLine ( "Called symbol " + use.CalledSymbol.ToString ( ) ) ;
                        // DebugUtils.WriteLine (
                                              // "Calling symbol " + use.CallingSymbol.ToString ( )
                                             // ) ;
                        callers.Add (
                                     new CallerInfo (
                                                     use.CalledSymbol.ToDisplayString ( )
                                                   , use.CallingSymbol.ToDisplayString ( )
                                                   , use.IsDirect
                                                   , use.Locations.Select (
                                                                           l => {
                                                                               var
                                                                                   fileLinePositionSpan
                                                                                       = l
                                                                                          .GetMappedLineSpan ( ) ;
                                                                               return new
                                                                                   LocationInfo (
                                                                                                 fileLinePositionSpan
                                                                                                    .Path
                                                                                               , fileLinePositionSpan
                                                                                                .StartLinePosition
                                                                                                .Character
                                                                                               , fileLinePositionSpan
                                                                                                .StartLinePosition
                                                                                                .Line
                                                                                               , fileLinePositionSpan
                                                                                                .EndLinePosition
                                                                                                .Character
                                                                                               , fileLinePositionSpan
                                                                                                    .EndLinePosition.Line
                                                                                               , l
                                                                                                .MetadataModule
                                                                                               ?.MetadataName
                                                                                               , l
                                                                                                .SourceSpan
                                                                                                .Start
                                                                                               , l
                                                                                                .SourceSpan
                                                                                                .End
                                                                                                ) ;
                                                                           }
                                                                          )
                                                    )
                                    ) ;
                        uses += use.Locations.Count ( ) ;
                    }

                    if ( uses > 0 )
                    {
                        DebugUtils.WriteLine ( "Total usages is " + uses ) ;
                    }
                    else
                    {
                        DebugUtils.WriteLine ( "0 uses" ) ;
                        DebugUtils.WriteLine ( "Symbol kind "   + symbol.Kind ) ;
                        DebugUtils.WriteLine ( "Called symbol " + symbol ) ;
                    }
                }
                
                // foreach ( var namespaceOrTypeSymbol in compilation
                                                      // .GetCompilationNamespace (compilationAssembly.ContainingNamespace )
                                                      // .GetMembers ( ) )
                // {
                    // if ( namespaceOrTypeSymbol.IsNamespace )
                    // {
                        
                    // } else if ( namespaceOrTypeSymbol.IsType )
                    // {
                        // var c = namespaceOrTypeSymbol.ContainingType ;
                        
                    // }
                // }
                foreach ( var doc in project.Documents )
                {
                    // var textAsync = await doc.GetTextAsync() ;
                    // var classified = await Microsoft.CodeAnalysis.Classification.Classifier.GetClassifiedSpansAsync (
                    // doc
                    // , new
                    // TextSpan (
                    // 0
                    // , textAsync
                    // .Length
                    // )
                    // ) ;

                    // foreach ( var classifiedSpan in classified )
                    // {
                    // DebugUtils.WriteLine($"{classifiedSpan.ClassificationType} : {classifiedSpan.TextSpan}");
                    // }
                    
                    // ReSharper disable once UnusedVariable
                    var model = await doc.GetSemanticModelAsync ( ) ;

                    Console.WriteLine ( doc.Name ) ;
                    // ReSharper disable once UnusedVariable
                    var tree = await doc.GetSyntaxRootAsync ( ) ;
                    
                    var visitor = new SyntaxWalker2(model);
                    visitor.Visit ( tree ) ;
                    foreach ( var node in tree
                                         .DescendantNodesAndSelf ( )
                                         .OfType < MemberDeclarationSyntax > ( ) )
                    {
                        var declared = model.GetDeclaredSymbol ( node ) ;
                        if ( declared == null )
                        {
                            continue ;
                        }

                        var xml1 = declared.GetDocumentationCommentXml ( ) ;
                        if ( declared.DeclaredAccessibility != Accessibility.Public
                             || ! SupportsDocumentationComments ( node ) )
                        {
                            DebugUtils.WriteLine (
                                                  $"Documentation accessibility is {declared.DeclaredAccessibility}"
                                                 ) ;
                        }
                        else
                        {
                            var docid = declared.GetDocumentationCommentId ( ) ;

                            // ReSharper disable once UnusedVariable
                            var o = new
                                    {
                                        docId    = docid
                                      , xml      = xml1
                                      , declared = declared.ToDisplayString ( )
                                    } ;

                            try
                            {
                                XDocument doc1 = null ;
                                if ( ! string.IsNullOrWhiteSpace ( xml1 ) )
                                {
                                    doc1 = XDocument.Parse ( xml1 ) ;
                                }

                                CodeElementDocumentation o1 = null ;
                                if ( doc1 != null )
                                {
                                    o1 = XmlDocElements.HandleDocElementNode (
                                                                              doc1
                                                                            , docid
                                                                            , node
                                                                            , declared
                                                                             )
                                         ?? throw new InvalidOperationException (
                                                                                 "Null from HandleDocElementNode"
                                                                                ) ;
                                }
                                else
                                {
                                    o1 = XmlDocElements.CreateCodeDocumentationElementType (
                                                                                            node
                                                                                          , docid
                                                                                           )
                                         ?? throw new InvalidOperationException (
                                                                                 "Null from CreateCodeDocumentationElementType"
                                                                                ) ;
                                }

                                if ( string.IsNullOrWhiteSpace ( xml1 ) )
                                {
                                    o1.NeedsAttention = true ;
                                }

                                documentsOut.Add ( o1 ) ;
                            }
                            catch ( Exception ex )
                            {
                                DebugUtils.WriteLine ( ex.ToString ( ) ) ;
                            }

                            // DebugUtils.WriteLine ( JsonSerializer.Serialize ( o ) ) ;
                        }
                    }

                    
                    // => tuple.Item2.Symbol
                    // != null
                    // && tuple.Item2.Symbol
                    // .DeclaredAccessibility
                    // == Accessibility.Public
                    // ) )
                    // {
                    // if ( ! tuple.Item1.GetLeadingTrivia ( )
                    // .Any ( SyntaxKind.SingleLineDocumentationCommentTrivia ) )
                    // {
                    // DebugUtils.WriteLine(DocumentationCommentId.CreateDeclarationId(tuple.Item2.Symbol));
                    // }

                    // var gen =
                    // FindLogUsages.GenTransforms.Transform_CSharp_Node (
                    // ( CSharpSyntaxNode ) tree
                    // ) ;

                    // DebugUtils.WriteLine ( JsonSerializer.Serialize ( gen , options ) ) ;
                }

                var jsonout = JsonSerializer.Serialize (
                                                        callers
                                                      , new JsonSerializerOptions ( )
                                                        {
                                                            WriteIndented = true
                                                        }
                                                       ) ;
                File.WriteAllText ( @"C:\temp\" + project.Name + ".json" , jsonout ) ;
            }

            var li = new ArrayList ( ) ;
            foreach ( var codeElementDocumentation in documentsOut )
            {
                if ( codeElementDocumentation != null )
                {
                    li.Add ( codeElementDocumentation ) ;
                }
            }


            var xmlWriter = XmlWriter.Create (
                                              @"C:\temp\docs.xaml"
                                            , new XmlWriterSettings { Indent = true }
                                             ) ;
            XamlWriter.Save ( li , xmlWriter ) ;
            xmlWriter.Close ( ) ;
        }

        // ReSharper disable once UnusedMember.Local
#pragma warning disable 1998
        private static async Task RunConsoleUiAsync ( [ NotNull ] AppContext context )
#pragma warning restore 1998
        {
            if ( context == null )
            {
                throw new ArgumentNullException ( nameof ( context ) ) ;
            }

            var ui = context.Scope.Resolve < TermUi > ( ) ;
            if ( ! ui.Commands.Any ( ) )
            {
                DebugUtils.WriteLine ( "No commands" ) ;
            }

            foreach ( var cmd in ui.Commands )
            {
                DebugUtils.WriteLine ( cmd.DisplayName ) ;
            }
#if TERMUI
            ui.Init ( ) ;
            ui.Run ( ) ;
#endif
        }

        [ TitleMetadata ( "Code gen" ) ]
        [ UsedImplicitly ]
        public async Task CodeGenAsync ( IBaseLibCommand command , [ NotNull ] AppContext context )
        {
            await Task.Run ( ( ) => CodeGen ( command , context ) ) ;
        }

#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        public async Task CodeGen (
#pragma warning restore VSTHRD200 // Use "Async" suffix for async methods
            [ NotNull ] IBaseLibCommand command
          , [ NotNull ] AppContext      context
        )
        {
            var outputFunc = ( Action < string > ) command.Argument ;

            void DebugOut ( string s1 )
            {
                DebugUtils.WriteLine ( s1 ) ;
                outputFunc ( s1 ) ;
            }

            var model1 = context.Scope.Resolve < TypesViewModel > ( ) ;
            var sts = context.Scope.Resolve < ISyntaxTypesService > ( ) ;
            var collectionMap = sts.CollectionMap ( ) ;
            outputFunc ( "Beginning" ) ;
            var x = CSharpCompilation.Create (
                                              "test"
                                            , new[] { SyntaxTree ( CompilationUnit ( ) ) }
                                            , AssemblyRefs.Select (
                                                                   r => MetadataReference
                                                                      .CreateFromFile ( r )
                                                                  )
                                             ) ;


            var types = new SyntaxList < MemberDeclarationSyntax > ( ) ;
            outputFunc ( $"{model1.Map.Count} Entries in Type map" ) ;
            var rewriter1 = new SyntaxRewriter1 ( model1 ) ;
            foreach ( var mapKey1 in model1.Map.dict.Keys )
            {
                var t = model1.Map.dict[ mapKey1 ] ;
                // ReSharper disable once UnusedVariable
                var curIterAppTypeInfo = t ;
                var colTypeClassName = $"{PocoPrefix}{t.Type.Name}{_collectionSuffix}" ;

                var classDecl1 = CreatePoco ( mapKey1 , t ) ;
                var curComp = ReplaceSyntaxTree ( x , classDecl1 ) ;

                // ReSharper disable once UnusedVariable
                var classDecl1Type =
                    curComp.GetTypeByMetadataName ( classDecl1.Identifier.ValueText ) ;
                var Ilist1 = SimpleBaseType ( ParseTypeName ( _ilist ) ) ;
                var IEnumerable1 = SimpleBaseType ( ParseTypeName ( _ienumerable ) ) ;
                var ICollectionType = SimpleBaseType ( ParseTypeName ( _icollection ) ) ;
                var classContainerDecl = ClassDeclaration ( colTypeClassName )
                   .WithBaseList (
                                  BaseList ( ).AddTypes ( Ilist1 , IEnumerable1 , ICollectionType )
                                 ) ;

                var typeSyntax2 = ParseTypeName ( t.Type.FullName ) ;
                var typeSyntax = ParseTypeName (
                                                PocoPrefix
                                                + ( ( QualifiedNameSyntax ) typeSyntax2 )
                                                 .Right.Identifier
                                               ) ;
                var genericinternalListType = GenericName (
                                                           Identifier ( _ilist )
                                                         , TypeArgumentList (
                                                                             SeparatedList <
                                                                                     TypeSyntax
                                                                                 > ( )
                                                                                .Add ( typeSyntax )
                                                                            )
                                                          ) ;
                var IListRuntimeType = typeof ( IList ) ;
                var ICollectionRuntimeType = typeof ( ICollection ) ;
                var IEnumerableRuntimeType = typeof ( IEnumerable ) ;
                foreach ( var type1 in new[]
                                       {
                                           IListRuntimeType , ICollectionRuntimeType
                                         , IEnumerableRuntimeType
                                       } )
                {
                    //var prov = CSharpCodeProvider.CreateProvider ( LanguageNames.CSharp ) ;

                    var typeByMetadataName =
                        x.GetTypeByMetadataName (
                                                 type1.FullName
                                                 ?? throw new InvalidOperationException ( )
                                                ) ;
                    if ( typeByMetadataName == null )
                    {
                        continue ;
                    }

                    var generic1 =
                        typeByMetadataName ; // t123.ConstructUnboundGenericType ( ).Construct ( classDecl1Type ) ;

                    // var i = model.GetSymbolInfo ( SyntaxFactory.ParseTypeName ( s1 ) ) ;


                    var _listField = IdentifierName ( "_list" ) ;

                    var publicKeyword = TokenList ( Token ( SyntaxKind.PublicKeyword ) ) ;

                    PropertyDeclarationSyntax Selector1 ( IPropertySymbol prop )
                    {
                        var propTypeSymbol = prop.Type ;
                        var propTypeSymbolMetadataName = propTypeSymbol.MetadataName ;
                        var propertyTypeParsed = SyntaxTypesService.FieldPocoCollectionType (
                                                                                             ParseTypeName (
                                                                                                            propTypeSymbolMetadataName
                                                                                                           )
                                                                                           , collectionMap
                                                                                           , t
                                                                                            ) ;
                        var propDescl = PropertyDeclaration ( propertyTypeParsed , prop.Name ) ;
                        propDescl = propDescl.WithModifiers ( publicKeyword ) ;
                        var propertyIdentifierNameSyntax = IdentifierName ( prop.Name ) ;
                        var propertyAccess = MemberAccessExpression (
                                                                     SyntaxKind
                                                                        .SimpleMemberAccessExpression
                                                                   , _listField
                                                                   , propertyIdentifierNameSyntax
                                                                    ) ;
                        var arrowExpressionClauseSyntax = ArrowExpressionClause ( propertyAccess ) ;
                        propDescl = propDescl
                                   .WithExpressionBody ( arrowExpressionClauseSyntax )
                                   .WithSemicolonToken ( Token ( SyntaxKind.SemicolonToken ) ) ;
                        return propDescl ;
                    }

                    var props1 = generic1.GetMembers ( )
                                         .OfType < IPropertySymbol > ( )
                                         .Where (
                                                 x22 => x22.Kind         == SymbolKind.Property
                                                        && x22.IsIndexer == false
                                                )
                                         .Select ( Selector1 ) ;
                    var indexers = generic1.GetMembers ( )
                                           .OfType < IPropertySymbol > ( )
                                           .Where (
                                                   x22 => x22.Kind == SymbolKind.Property
                                                          && x22.IsIndexer
                                                  )
                                           .Select (
                                                    prop => {
                                                        var brackArgList =
                                                            BracketedArgumentList (
                                                                                   SeparatedList <
                                                                                           ArgumentSyntax
                                                                                       > ( )
                                                                                      .AddRange (
                                                                                                 prop
                                                                                                    .Parameters
                                                                                                    .Select (
                                                                                                             pp1
                                                                                                                 => Argument (
                                                                                                                              IdentifierName (
                                                                                                                                              pp1
                                                                                                                                                 .Name
                                                                                                                                             )
                                                                                                                             )
                                                                                                            )
                                                                                                )
                                                                                  ) ;
                                                        var elementAccess =
                                                            ElementAccessExpression (
                                                                                     _listField
                                                                                   , brackArgList
                                                                                    ) ;
                                                        var setArrowExpression =
                                                            ArrowExpressionClause (
                                                                                   AssignmentExpression (
                                                                                                         SyntaxKind
                                                                                                            .SimpleAssignmentExpression
                                                                                                       , elementAccess
                                                                                                       , IdentifierName (
                                                                                                                         "value"
                                                                                                                        )
                                                                                                        )
                                                                                  ) ;
                                                        var setAcessor =
                                                            AccessorDeclaration (
                                                                                 SyntaxKind
                                                                                    .SetAccessorDeclaration
                                                                                )
                                                               .WithExpressionBody (
                                                                                    setArrowExpression
                                                                                   )
                                                               .WithSemicolonToken (
                                                                                    Token (
                                                                                           SyntaxKind
                                                                                              .SemicolonToken
                                                                                          )
                                                                                   ) ;
                                                        var getArrow =
                                                            ArrowExpressionClause (
                                                                                   ElementAccessExpression (
                                                                                                            _listField
                                                                                                          , brackArgList
                                                                                                           )
                                                                                  ) ;


                                                        var parameterSyntaxes =
                                                            prop.Parameters.Select (
                                                                                    p2
                                                                                        => Parameter (
                                                                                                      Identifier (
                                                                                                                  p2
                                                                                                                     .Name
                                                                                                                 )
                                                                                                     )
                                                                                           .WithType (
                                                                                                      ParseTypeName (
                                                                                                                     p2
                                                                                                                        .Type
                                                                                                                        .MetadataName
                                                                                                                    )
                                                                                                     )
                                                                                   ) ;
                                                        var separatedSyntaxList =
                                                            SeparatedList < ParameterSyntax > ( )
                                                               .AddRange ( parameterSyntaxes ) ;
                                                        return IndexerDeclaration (
                                                                                   ParseTypeName (
                                                                                                  prop
                                                                                                     .Type
                                                                                                     .MetadataName
                                                                                                 )
                                                                                  )
                                                              .WithParameterList (
                                                                                  BracketedParameterList (
                                                                                                          separatedSyntaxList
                                                                                                         )
                                                                                 )
                                                              .WithModifiers ( publicKeyword )
                                                              .WithAccessorList (
                                                                                 AccessorList (
                                                                                               List
                                                                                                   <
                                                                                                       AccessorDeclarationSyntax
                                                                                                   > ( )
                                                                                                  .AddRange (
                                                                                                             new
                                                                                                             []
                                                                                                             {
                                                                                                                 AccessorDeclaration (
                                                                                                                                      SyntaxKind
                                                                                                                                         .GetAccessorDeclaration
                                                                                                                                     )
                                                                                                                    .WithExpressionBody (
                                                                                                                                         getArrow
                                                                                                                                        )
                                                                                                                    .WithSemicolonToken (
                                                                                                                                         Token (
                                                                                                                                                SyntaxKind
                                                                                                                                                   .SemicolonToken
                                                                                                                                               )
                                                                                                                                        )
                                                                                                               , setAcessor
                                                                                                             }
                                                                                                            )
                                                                                              )
                                                                                ) ;
                                                    }
                                                   ) ;

                    var memb1 = generic1.GetMembers ( )
                                        .OfType < IMethodSymbol > ( )
                                        .Where (
                                                x22 => x22.Kind          == SymbolKind.Method
                                                       && x22.MethodKind == MethodKind.Ordinary
                                               )
                                        .Select (
                                                 m => {
                                                     var returnType =
                                                         ParseTypeName (
                                                                        m.ReturnType.MetadataName
                                                                       ) ;
                                                     var methodDeclarationSyntax =
                                                         MethodDeclaration (
                                                                            m.ReturnsVoid
                                                                                ? PredefinedType (
                                                                                                  Token (
                                                                                                         SyntaxKind
                                                                                                            .VoidKeyword
                                                                                                        )
                                                                                                 )
                                                                                : returnType
                                                                          , m.Name
                                                                           ) ;

                                                     ParameterSyntax Selector (
                                                         IParameterSymbol p1
                                                     )
                                                     {
                                                         if ( p1.Type.SpecialType
                                                              == SpecialType.System_Object )
                                                         {
                                                             DebugUtils.WriteLine ( $"{p1.Type}" ) ;
                                                         }

                                                         return Parameter (
                                                                           List <
                                                                               AttributeListSyntax
                                                                           > ( )
                                                                         , new SyntaxTokenList ( )
                                                                         , ParseTypeName (
                                                                                          p1.Type
                                                                                            .MetadataName
                                                                                         )
                                                                         , Identifier ( p1.Name )
                                                                         , null
                                                                          ) ;
                                                     }

                                                     var separatedSyntaxList =
                                                         SeparatedList (
                                                                        m.Parameters.Select (
                                                                                             Selector
                                                                                            )
                                                                       ) ;
                                                     return methodDeclarationSyntax
                                                           .WithModifiers ( publicKeyword )
                                                           .WithLeadingTrivia (
                                                                               SyntaxTriviaList
                                                                                  .Create (
                                                                                           Comment (
                                                                                                    $"// {typeByMetadataName.ToDisplayString ( )}"
                                                                                                   )
                                                                                          )
                                                                              )
                                                           .WithParameterList (
                                                                               ParameterList (
                                                                                              separatedSyntaxList
                                                                                             )
                                                                              )
                                                           .WithExpressionBody (
                                                                                ArrowExpressionClause (
                                                                                                       InvocationExpression (
                                                                                                                             MemberAccessExpression (
                                                                                                                                                     SyntaxKind
                                                                                                                                                        .SimpleMemberAccessExpression
                                                                                                                                                   , _listField
                                                                                                                                                   , IdentifierName (
                                                                                                                                                                     m.Name
                                                                                                                                                                    )
                                                                                                                                                    )
                                                                                                                            )
                                                                                                          .WithArgumentList (
                                                                                                                             ArgumentList (
                                                                                                                                           new
                                                                                                                                                   SeparatedSyntaxList
                                                                                                                                                   < ArgumentSyntax
                                                                                                                                                   > ( )
                                                                                                                                              .AddRange (
                                                                                                                                                         m.Parameters
                                                                                                                                                          .Select (
                                                                                                                                                                   p
                                                                                                                                                                       => Argument (
                                                                                                                                                                                    CastExpression (
                                                                                                                                                                                                    p
                                                                                                                                                                                                       .Type
                                                                                                                                                                                                       .SpecialType
                                                                                                                                                                                                    == SpecialType
                                                                                                                                                                                                       .System_Object
                                                                                                                                                                                                        ? genericinternalListType
                                                                                                                                                                                                         .TypeArgumentList
                                                                                                                                                                                                         .Arguments
                                                                                                                                                                                                              [
                                                                                                                                                                                                               0 ]
                                                                                                                                                                                                        : ParseTypeName (
                                                                                                                                                                                                                         p
                                                                                                                                                                                                                            .Type
                                                                                                                                                                                                                            .MetadataName
                                                                                                                                                                                                                        )
                                                                                                                                                                                                  , IdentifierName (
                                                                                                                                                                                                                    p
                                                                                                                                                                                                                       .Name
                                                                                                                                                                                                                   )
                                                                                                                                                                                                   )
                                                                                                                                                                                   )
                                                                                                                                                                  )
                                                                                                                                                        )
                                                                                                                                          )
                                                                                                                            )
                                                                                                      )
                                                                               )
                                                           .WithSemicolonToken (
                                                                                Token (
                                                                                       SyntaxKind
                                                                                          .SemicolonToken
                                                                                      )
                                                                               ) ;
                                                 }
                                                ) ;

                    classContainerDecl = classContainerDecl.WithMembers (
                                                                         List (
                                                                               classContainerDecl
                                                                                  .Members
                                                                                  .Concat ( memb1 )
                                                                                  .Concat ( props1 )
                                                                                  .Concat (
                                                                                           indexers
                                                                                          )
                                                                              )
                                                                        ) ;

                    classDecl1 = ( ClassDeclarationSyntax ) rewriter1.Visit ( classDecl1 ) ;
                    DebugUtils.WriteLine (
                                          "\n***\n"
                                          + classContainerDecl
                                           .NormalizeWhitespace ( )
                                           .ToFullString ( )
                                          + "\n****\n"
                                         ) ;
                }


                var argumentListSyntax = ArgumentList (
                                                       Token ( SyntaxKind.OpenParenToken )
                                                     , SeparatedList < ArgumentSyntax > ( )
                                                     , Token ( SyntaxKind.CloseParenToken )
                                                      ) ;
                var ListIdentifier = Identifier ( "List" ) ;
                var constructedListGeneric = GenericName (
                                                          ListIdentifier
                                                        , TypeArgumentList (
                                                                            SingletonSeparatedList (
                                                                                                    typeSyntax
                                                                                                   )
                                                                           )
                                                         ) ;
                var ocex = ObjectCreationExpression (
                                                     constructedListGeneric
                                                   , argumentListSyntax
                                                   , default
                                                    ) ;
                var fds = FieldDeclaration (
                                            VariableDeclaration ( ParseTypeName ( _ilist ) )
                                               .WithVariables (
                                                               SeparatedList <
                                                                       VariableDeclaratorSyntax
                                                                   > ( )
                                                                  .Add (
                                                                        VariableDeclarator (
                                                                                            "_list"
                                                                                           )
                                                                           .WithInitializer (
                                                                                             EqualsValueClause (
                                                                                                                ocex
                                                                                                               )
                                                                                            )
                                                                       )
                                                              )
                                           ) ;
                classContainerDecl = classContainerDecl.WithMembers (
                                                                     List (
                                                                           classContainerDecl
                                                                              .Members.Concat (
                                                                                               new[]
                                                                                               {
                                                                                                   fds
                                                                                               }
                                                                                              )
                                                                          )
                                                                    ) ;

                types = types.Add ( classContainerDecl ) ;
                var members = new SyntaxList < MemberDeclarationSyntax > ( ) ;
                foreach ( SyntaxFieldInfo tField in t.Fields )
                {
                    if ( tField.ClrTypeName != null )
                    {
                        tField.Type = Type.GetType ( tField.ClrTypeName ) ;
                        if ( tField.Type == null )
                        {
                            DebugUtils.WriteLine (
                                                  $"unable to resolve type {tField.ClrTypeName}e "
                                                 ) ;
                        }
                    }


                    if ( tField.Type        == null
                         && tField.Type     != typeof ( bool )
                         && tField.TypeName != "bool" )
                    {
                        continue ;
                    }

                    var acdsl = new SyntaxList < AccessorDeclarationSyntax > (
                                                                              new[]
                                                                              {
                                                                                  AccessorDeclaration (
                                                                                                       SyntaxKind
                                                                                                          .GetAccessorDeclaration
                                                                                                      )
                                                                                     .WithSemicolonToken (
                                                                                                          Token (
                                                                                                                 SyntaxKind
                                                                                                                    .SemicolonToken
                                                                                                                )
                                                                                                         )
                                                                                , AccessorDeclaration (
                                                                                                       SyntaxKind
                                                                                                          .SetAccessorDeclaration
                                                                                                      )
                                                                                     .WithSemicolonToken (
                                                                                                          Token (
                                                                                                                 SyntaxKind
                                                                                                                    .SemicolonToken
                                                                                                                )
                                                                                                         )
                                                                              }
                                                                             ) ;
                    var accessorListSyntax = AccessorList ( acdsl ) ;

                    TypeSyntax type = null ;
                    if ( tField.IsCollection )
                    {
                        type = ParseTypeName ( tField.ElementTypeMetadataName ) ;
                    }
                    else
                    {
                        var tFieldTypeName = tField.TypeName ;

                        type = ParseTypeName ( tFieldTypeName ) ;
                    }

                    if ( type is GenericNameSyntax gen )
                    {
                        var ss = ( SimpleNameSyntax ) gen.TypeArgumentList.Arguments[ 0 ] ;
                        type = gen.WithTypeArgumentList (
                                                         TypeArgumentList (
                                                                           new SeparatedSyntaxList <
                                                                                   TypeSyntax > ( )
                                                                              .Add (
                                                                                    ParseTypeName (
                                                                                                   $"{PocoPrefix}{ss.Identifier.Text}"
                                                                                                  )
                                                                                   )
                                                                          )
                                                        ) ;
                    }


                    var tokens = new List < SyntaxToken >
                                 {
                                     Token ( SyntaxKind.PublicKeyword )
                                   , tField.Override
                                         ? Token ( SyntaxKind.OverrideKeyword )
                                         : Token ( SyntaxKind.VirtualKeyword )
                                 } ;


                    var nameSyntax = MemberAccessExpression (
                                                             SyntaxKind.SimpleMemberAccessExpression
                                                           , IdentifierName (
                                                                             "DesignerSerializationVisibility"
                                                                            )
                                                           , IdentifierName ( "Content" )
                                                            ) ;
                    ////.ParseName ( "System.ComponentModel.DesignerSerializationVisibility" ) ;
                    var attributeSyntax = Attribute (
                                                     ParseName ( "DesignerSerializationVisibility" )
                                                   , AttributeArgumentList (
                                                                            SeparatedList (
                                                                                           new[]
                                                                                           {
                                                                                               AttributeArgument (
                                                                                                                  nameSyntax
                                                                                                                 )
                                                                                           }
                                                                                          )
                                                                           )
                                                    ) ;
                    DebugUtils.WriteLine ( attributeSyntax.ToFullString ( ) ) ;
                    var separatedSyntaxList =
                        new SeparatedSyntaxList < AttributeSyntax > ( ).Add ( attributeSyntax ) ;
                    var attributeListSyntaxes = List (
                                                      new[]
                                                      {
                                                          AttributeList ( separatedSyntaxList )
                                                      }
                                                     ) ;
                    var syntaxTokenList = TokenList ( tokens.ToArray ( ) ) ;
                    var propertyName = Identifier ( tField.Name ) ;
                    var propertyDeclarationSyntax = PropertyDeclaration (
                                                                         attributeListSyntaxes
                                                                       , syntaxTokenList
                                                                       , XmlDocElements
                                                                            .SubstituteType (
                                                                                             tField
                                                                                           , type
                                                                                           , collectionMap
                                                                                           , sts
                                                                                            )
                                                                       , null
                                                                       , propertyName
                                                                       , accessorListSyntax
                                                                        ) ;
                    DebugUtils.WriteLine (
                                          propertyDeclarationSyntax
                                             .NormalizeWhitespace ( )
                                             .ToFullString ( )
                                         ) ;
                    members = members.Add ( propertyDeclarationSyntax ) ;
                }

                classDecl1 = classDecl1.WithMembers ( members ) ;
                types      = types.Add ( classDecl1 ) ;
            }

            DebugOut ( "About to build compilation unit" ) ;

            var compl = CompilationUnit ( ) ;
            compl = SyntaxTypesService.WithCollectionUsings ( compl ) ;
            compl = compl.WithMembers (
                                       new SyntaxList < MemberDeclarationSyntax > (
                                                                                   NamespaceDeclaration (
                                                                                                         ParseName (
                                                                                                                    _pocosyntaxnamespace
                                                                                                                   )
                                                                                                        )
                                                                                      .WithMembers (
                                                                                                    types
                                                                                                   )
                                                                                  )
                                      )
                         .NormalizeWhitespace ( ) ;

            foreach ( var token in compl.DescendantTokens ( ) )
            {
            }

            DebugOut ( "built" ) ;
            var tree = SyntaxTree ( compl ) ;
            var src = tree.ToString ( ) ;

            DebugOut ( "Reparsing text ??" ) ;

            var tree2 = CSharpSyntaxTree.Create (
                                                 compl
                                               , new CSharpParseOptions (
                                                                         LanguageVersion.CSharp7_3
                                                                        )
                                                ) ;

            File.WriteAllText ( @"C:\data\logs\gen.cs" , compl.ToString ( ) ) ;

            //refs, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, false, "test", null, null, null, OptimizationLevel.Debug, false, false, null, null, default, default ,default, default, default,default, default, default, default, default, new MetadataReferenceResolver())
            var source = tree2.ToString ( ).Split ( new[] { "\r\n" } , StringSplitOptions.None ) ;

            //var compilation = CSharpCompilation.Create ( "test" , new[] { tree2 } ) ;
            var adhoc = new AdhocWorkspace ( ) ;
            var projectId = ProjectId.CreateNewId ( ) ;
            DebugOut ( "Add solution" ) ;

            var s = adhoc.AddSolution (
                                       SolutionInfo.Create (
                                                            SolutionId.CreateNewId ( )
                                                          , VersionStamp.Create ( )
                                                          , null
                                                          , new[]
                                                            {
                                                                ProjectInfo.Create (
                                                                                    projectId
                                                                                  , VersionStamp
                                                                                       .Create ( )
                                                                                  , "test"
                                                                                  , "test"
                                                                                  , LanguageNames
                                                                                       .CSharp
                                                                                  , null
                                                                                  , null
                                                                                  , new
                                                                                        CSharpCompilationOptions (
                                                                                                                  OutputKind
                                                                                                                     .DynamicallyLinkedLibrary
                                                                                                                 )
                                                                                   )
                                                            }
                                                           )
                                      ) ;


            var documentInfo = DocumentInfo.Create (
                                                    DocumentId.CreateNewId ( projectId )
                                                  , "test"
                                                  , null
                                                  , SourceCodeKind.Regular
                                                  , TextLoader.From (
                                                                     TextAndVersion.Create (
                                                                                            SourceText
                                                                                               .From (
                                                                                                      src
                                                                                                     )
                                                                                          , VersionStamp
                                                                                               .Create ( )
                                                                                           )
                                                                    )
                                                   ) ;

            var document2 = DocumentInfo.Create (
                                                 DocumentId.CreateNewId ( projectId )
                                               , "misc"
                                               , null
                                               , SourceCodeKind.Regular
                                               , TextLoader.From (
                                                                  TextAndVersion.Create (
                                                                                         SourceText
                                                                                            .From (
                                                                                                   $@"public class {Pocosyntaxtoken} {{ public int RawKind {{ get; set; }} public string Kind {{ get; set; }} public object Value {{get; set;}} public string ValueText {{ get; set; }} }}"
                                                                                                  )
                                                                                       , VersionStamp
                                                                                            .Create ( )
                                                                                        )
                                                                 )
                                                ) ;

            //todo investigate
            var s2 = s.AddDocuments (
                                     ImmutableArray < DocumentInfo >
                                         // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable
                                        .Empty.Add ( documentInfo )
                                        .Add ( document2 )
                                    ) ;

            //var d = project.AddDocument ( "test.cs" , src ) ;
            var rb1 = adhoc.TryApplyChanges ( s2 ) ;
            if ( ! rb1 )
            {
                throw new InvalidOperationException ( ) ;
            }

            DebugOut ( "Applying assembly refs" ) ;
            foreach ( var ref1 in AssemblyRefs.Union (
                                                      new[]
                                                      {
                                                          typeof ( DesignerSerializationOptions )
                                                             .Assembly.Location
                                                      }
                                                     ) )

            {
                var s3 = adhoc.CurrentSolution.AddMetadataReference (
                                                                     projectId
                                                                   , MetadataReference
                                                                        .CreateFromFile ( ref1 )
                                                                    ) ;
                var rb = adhoc.TryApplyChanges ( s3 ) ;
                if ( ! rb )
                {
                    throw new InvalidOperationException ( ) ;
                }
            }

            DebugOut ( "Applying assembly done" ) ;
            var project = adhoc.CurrentSolution.Projects.First ( ) ;

            var compilation = await project.GetCompilationAsync ( ) ;
            using ( var f = new StreamWriter ( @"C:\data\logs\errors.txt" ) )
            {
                if ( compilation != null )
                {
                    foreach ( var diagnostic in compilation.GetDiagnostics ( ) )
                    {
                        if ( ! diagnostic.IsSuppressed )
                        {
                            // ReSharper disable once UnusedVariable
                            var line = source
                                      .Skip (
                                             diagnostic
                                                .Location.GetLineSpan ( )
                                                .StartLinePosition.Line
                                             - 1
                                            )
                                      .Take (
                                             diagnostic
                                                .Location.GetLineSpan ( )
                                                .EndLinePosition.Line
                                             - diagnostic
                                              .Location.GetLineSpan ( )
                                              .StartLinePosition.Line
                                            ) ;
                            await f.WriteLineAsync (
                                                    string.Join (
                                                                 "\n"
                                                               , diagnostic.ToString ( )
                                                               , line
                                                                )
                                                   ) ;
                        }
                    }
                }
            }

            if ( compilation != null )
            {
                var errors = compilation.GetDiagnostics ( )
                                        .Where ( d => d.Severity == DiagnosticSeverity.Error )
                                        .ToList ( ) ;
                if ( errors.Any ( ) )
                {
                    DebugUtils.WriteLine ( string.Join ( "\n" , errors ) ) ;
                }
            }

            DebugOut ( "attempting emit" ) ;

            var result =
                ( compilation ?? throw new InvalidOperationException ( ) ).Emit (
                                                                                 @"C:\data\logs\output.dll"
                                                                                ) ;
            if ( result.Success )
            {
                DebugOut ( "Success" ) ;
            }
            else
            {
                DebugUtils.WriteLine ( "Failure" ) ;
            }

            //File.WriteAllText ( @"C:\data\logs\gen.cs" , comp.ToString ( ) ) ;
        }

        [ NotNull ]
        private static CSharpCompilation ReplaceSyntaxTree (
            [ NotNull ] CSharpCompilation x
          , ClassDeclarationSyntax        classDecl1
        )
        {
            return x.ReplaceSyntaxTree (
                                        x.SyntaxTrees[ 0 ]
                                      , SyntaxTree (
                                                    CompilationUnit ( )
                                                       .WithMembers (
                                                                     List < MemberDeclarationSyntax
                                                                     > ( new[] { classDecl1 } )
                                                                    )
                                                   )
                                       ) ;
        }

        [ NotNull ]
        private static ClassDeclarationSyntax CreatePoco (
            [ NotNull ] AppTypeInfoKey mapKey
          , [ NotNull ] AppTypeInfo    t
        )
        {
            var classDecl1 = ClassDeclaration ( $"{_pocoPrefix}{mapKey.StringValue}" )
               .WithModifiers ( SyntaxTokenList.Create ( Token ( SyntaxKind.PublicKeyword ) ) ) ;

            if ( t.ParentInfo != null )
            {
                var identifierNameSyntax = IdentifierName ( _pocoPrefix + t.ParentInfo.Type.Name ) ;
                classDecl1 = classDecl1.WithBaseList (
                                                      BaseList (
                                                                new SeparatedSyntaxList <
                                                                    BaseTypeSyntax > ( ).Add (
                                                                                              SimpleBaseType (
                                                                                                              identifierNameSyntax
                                                                                                             )
                                                                                             )
                                                               )
                                                     ) ;
            }

            return classDecl1 ;
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

        private static void PopulateSet (
            [ NotNull ] AppTypeInfoCollection subTypeInfos
          , HashSet < Type >                  set
        )
        {
            foreach ( var rootSubTypeInfo in subTypeInfos )
            {
                if ( rootSubTypeInfo.Type.IsAbstract == false )
                {
                    set.Add ( rootSubTypeInfo.Type ) ;
                }

                PopulateSet ( rootSubTypeInfo.SubTypeInfos , set ) ;
            }
        }

        // ReSharper disable once UnusedMember.Local
        private static bool SupportsDocumentationComments (
            [ CanBeNull ] MemberDeclarationSyntax member
        )
        {
            if ( member == null )
            {
                return false ;
            }

            switch ( member.Kind ( ) )
            {
                case SyntaxKind.ClassDeclaration :
                case SyntaxKind.InterfaceDeclaration :
                case SyntaxKind.StructDeclaration :
                case SyntaxKind.DelegateDeclaration :
                case SyntaxKind.EnumDeclaration :
                case SyntaxKind.EnumMemberDeclaration :
                case SyntaxKind.FieldDeclaration :
                case SyntaxKind.MethodDeclaration :
                case SyntaxKind.ConstructorDeclaration :
                case SyntaxKind.DestructorDeclaration :
                case SyntaxKind.PropertyDeclaration :
                case SyntaxKind.IndexerDeclaration :
                case SyntaxKind.EventDeclaration :
                case SyntaxKind.EventFieldDeclaration :
                case SyntaxKind.OperatorDeclaration :
                case SyntaxKind.ConversionOperatorDeclaration :
                    return true ;

                default : return false ;
            }
        }
    }

    internal sealed class Options
    {
        [Option('s', "sln", Required = false)]
        public string SolutionFile { get ; set ; }
        [Option('a', "action", Required = false)]
        public string Action { get ; set ; }
    }

    public sealed class LocationInfo
    {
        public string FileName { get ; }

        public int CharStart { get ; }

        public int LineStart { get ; }

        public int CharEnd { get ; }

        public int LineEnd { get ; }

        public LocationInfo (
            string metadataModuleMetadataName
          , int    sourceSpanStart
          , int    sourceSpanEnd
        )
        {
            MetadataModuleMetadataName = metadataModuleMetadataName ;
            SourceSpanStart            = sourceSpanStart ;
            SourceSpanEnd              = sourceSpanEnd ;
        }

        public LocationInfo (
            string          fileName
          , int             charStart
          , int             lineStart
          , int             charEnd
          , int lineEnd
          , string          metadataModuleMetadataName
          , int             sourceSpanStart
          , int             sourceSpanEnd
        )
        {
            FileName = fileName ;
            CharStart = charStart ;
            LineStart = lineStart ;
            CharEnd = charEnd ;
            LineEnd = lineEnd ;
            MetadataModuleMetadataName = metadataModuleMetadataName ;
            SourceSpanStart = sourceSpanStart ;
            SourceSpanEnd = sourceSpanEnd ;
        }

        public string MetadataModuleMetadataName { get ; set ; }

        public int SourceSpanStart { get ; set ; }

        public int SourceSpanEnd { get ; set ; }
    }

    public class CallerInfo
    {
        private List < LocationInfo > _locations = new List < LocationInfo > ( ) ;

        public string CalledSymbol { get ; }

        public string CallingSymbol { get ; }

        public bool IsDirect { get ; }

        public CallerInfo (
            ImmutableArray < SymbolDisplayPart > toDisplayParts
          , ImmutableArray < SymbolDisplayPart > symbolDisplayParts
          , bool                                 useIsDirect
          , IEnumerable < LocationInfo >         @select
        )
        {
        }

        public CallerInfo (
            string                       calledSymbol
          , string                       callingSymbol
          , bool                         isDirect
          , IEnumerable < LocationInfo > @select
        )
        {
            CalledSymbol  = calledSymbol ;
            CallingSymbol = callingSymbol ;
            IsDirect      = isDirect ;
            Locations.AddRange ( @select ) ;
        }

        public List < LocationInfo > Locations
        {
            get { return _locations ; }
            set { _locations = value ; }
        }
    }

    public class SyntaxWalker2 : CSharpSyntaxWalker
    {
        private readonly SemanticModel model ;

        #region Overrides of CSharpSyntaxVisitor
        public SyntaxWalker2 ( SemanticModel model , SyntaxWalkerDepth depth = SyntaxWalkerDepth.Node ) : base ( depth )
        {
            this.model = model ;
        }

        public override void VisitCompilationUnit ( CompilationUnitSyntax node ) { base.VisitCompilationUnit ( node ) ; }

        public override void VisitNamespaceDeclaration ( NamespaceDeclarationSyntax node )
        {
            var nsSymbol = model.GetDeclaredSymbol ( node ) ;
            base.VisitNamespaceDeclaration ( node ) ;
        }

        public override void VisitClassDeclaration ( ClassDeclarationSyntax node )
        {
            var classSymbol = model.GetDeclaredSymbol(node);
            if ( classSymbol == null )
            {
                throw new InvalidOperationException ( "No class symbol" ) ;
            }
            foreach ( var member in classSymbol.GetMembers ( ) )
            {
                switch ( member )
                {
                    case IAliasSymbol aliasSymbol : break ;
                    case IArrayTypeSymbol arrayTypeSymbol : break ;
                    case ISourceAssemblySymbol sourceAssemblySymbol : break ;
                    case IAssemblySymbol assemblySymbol : break ;
                    case IDiscardSymbol discardSymbol : break ;
                    case IDynamicTypeSymbol dynamicTypeSymbol : break ;
                    case IErrorTypeSymbol errorTypeSymbol : break ;
                    case IEventSymbol eventSymbol : break ;
                    case IFieldSymbol fieldSymbol : break ;
                    case ILabelSymbol labelSymbol : break ;
                    case ILocalSymbol localSymbol : break ;
                    case IMethodSymbol methodSymbol :
                        var m = CreateMethodInfo ( methodSymbol ) ;
                        var json = JsonSerializer.Serialize ( m ) ;
                        DebugUtils.WriteLine(json);
                        break ;
                    case IModuleSymbol moduleSymbol : break ;
                    case INamedTypeSymbol namedTypeSymbol : break ;
                    case INamespaceSymbol namespaceSymbol : break ;
                    case IPointerTypeSymbol pointerTypeSymbol : break ;
                    case ITypeParameterSymbol typeParameterSymbol : break ;
                    case ITypeSymbol typeSymbol : break ;
                    case INamespaceOrTypeSymbol namespaceOrTypeSymbol : break ;
                    case IParameterSymbol parameterSymbol : break ;
                    case IPreprocessingSymbol preprocessingSymbol : break ;
                    case IPropertySymbol propertySymbol : break ;
                    case IRangeVariableSymbol rangeVariableSymbol : break ;
                    default : throw new ArgumentOutOfRangeException ( nameof ( member ) ) ;
                }
            }
            base.VisitClassDeclaration ( node ) ;
        }

        [ NotNull ]
        private static MethodInfo CreateMethodInfo ( [ NotNull ] IMethodSymbol methodSymbol )
        {
            var m = new MethodInfo (
                                    methodSymbol.Name
                                  , methodSymbol.Parameters.Select (
                                                                    p => new ParameterInfo (
                                                                                            p.Name
                                                                                          , p.Type
                                                                                          , p
                                                                                           .CustomModifiers
                                                                                           .Select (
                                                                                                    md
                                                                                                        => new
                                                                                                            CustommodifierInfo (
                                                                                                                                md
                                                                                                                                   .IsOptional
                                                                                                                              , md
                                                                                                                               .Modifier
                                                                                                                               .ToDisplayString ( )
                                                                                                                               )
                                                                                                   )
                                                                                          , p
                                                                                           .Type
                                                                                           .ToDisplayString ( )
                                                                                           )
                                                                   )
                                   ) ;
            return m ;
        }

        public override void VisitStructDeclaration ( StructDeclarationSyntax node ) { base.VisitStructDeclaration ( node ) ; }

        public override void VisitInterfaceDeclaration ( InterfaceDeclarationSyntax node ) { base.VisitInterfaceDeclaration ( node ) ; }

        public override void VisitEnumDeclaration ( EnumDeclarationSyntax node ) { base.VisitEnumDeclaration ( node ) ; }

        public override void VisitDelegateDeclaration ( DelegateDeclarationSyntax node ) { base.VisitDelegateDeclaration ( node ) ; }

        public override void VisitFieldDeclaration ( FieldDeclarationSyntax node ) { base.VisitFieldDeclaration ( node ) ; }

        public override void VisitEventFieldDeclaration ( EventFieldDeclarationSyntax node ) { base.VisitEventFieldDeclaration ( node ) ; }

        public override void VisitExplicitInterfaceSpecifier ( ExplicitInterfaceSpecifierSyntax node ) { base.VisitExplicitInterfaceSpecifier ( node ) ; }

        public override void VisitMethodDeclaration ( MethodDeclarationSyntax node )
        {
            var symbol = model.GetDeclaredSymbol ( node ) ;
            if ( symbol.MethodKind != MethodKind.Ordinary )
            {
                throw new InvalidOperationException ( symbol.MethodKind.ToString ( ) ) ;
            }
            var rt = symbol.ReturnType ;
            var origDef = rt.OriginalDefinition ;
            var displayString = rt.ToDisplayString ( ) ;
            base.VisitMethodDeclaration ( node ) ;
        }

        public override void VisitParameterList ( ParameterListSyntax node ) { base.VisitParameterList ( node ) ; }

        public override void VisitParameter ( ParameterSyntax node )
        {
            var symbol = model.GetDeclaredSymbol ( node ) ;
            foreach ( var symbolDisplayPart in symbol.ToDisplayParts ( ) )
            {
                var k = ( int ) symbolDisplayPart.Kind ;
                var s = symbolDisplayPart.Symbol ;
                string interfaces ="";
                if ( s != null )
                {
                    interfaces = string.Join (
                                              ", "
                                            , s.GetType ( )
                                                .GetInterfaces ( )
                                                .Select ( i => i.FullName )
                                             ) ;
                }

                DebugUtils.WriteLine($"{symbolDisplayPart} {s?.Kind} {s?.GetType().FullName} {interfaces ?? ""}");
                switch ( s )
                {
                    case IAliasSymbol aliasSymbol : break ;
                    case IArrayTypeSymbol arrayTypeSymbol : break ;
                    case ISourceAssemblySymbol sourceAssemblySymbol : break ;
                    case IAssemblySymbol assemblySymbol : break ;
                    case IDiscardSymbol discardSymbol : break ;
                    case IDynamicTypeSymbol dynamicTypeSymbol : break ;
                    case IErrorTypeSymbol errorTypeSymbol : break ;
                    case IEventSymbol eventSymbol : break ;
                    case IFieldSymbol fieldSymbol : break ;
                    case ILabelSymbol labelSymbol : break ;
                    case ILocalSymbol localSymbol : break ;
                    case IMethodSymbol methodSymbol : break ;
                    case IModuleSymbol moduleSymbol : break ;
                    case INamedTypeSymbol namedTypeSymbol : break ;
                    case INamespaceSymbol namespaceSymbol : break ;
                    case IPointerTypeSymbol pointerTypeSymbol : break ;
                    case ITypeParameterSymbol typeParameterSymbol : break ;
                    case ITypeSymbol typeSymbol : break ;
                    case INamespaceOrTypeSymbol namespaceOrTypeSymbol : break ;
                    case IParameterSymbol parameterSymbol : break ;
                    case IPreprocessingSymbol preprocessingSymbol : break ;
                    case IPropertySymbol propertySymbol : break ;
                    case IRangeVariableSymbol rangeVariableSymbol : break ;
                    default : throw new ArgumentOutOfRangeException ( nameof ( s ) ) ;
                }
            }
            base.VisitParameter ( node ) ;
        }

        public override void VisitOperatorDeclaration ( OperatorDeclarationSyntax node ) { base.VisitOperatorDeclaration ( node ) ; }

        public override void VisitConversionOperatorDeclaration ( ConversionOperatorDeclarationSyntax node ) { base.VisitConversionOperatorDeclaration ( node ) ; }

        public override void VisitConstructorDeclaration ( ConstructorDeclarationSyntax node ) { base.VisitConstructorDeclaration ( node ) ; }

        public override void VisitDestructorDeclaration ( DestructorDeclarationSyntax node ) { base.VisitDestructorDeclaration ( node ) ; }

        public override void VisitPropertyDeclaration ( PropertyDeclarationSyntax node ) { base.VisitPropertyDeclaration ( node ) ; }

        public override void VisitEventDeclaration ( EventDeclarationSyntax node ) { base.VisitEventDeclaration ( node ) ; }

        public override void VisitIndexerDeclaration ( IndexerDeclarationSyntax node ) { base.VisitIndexerDeclaration ( node ) ; }
        #endregion
    }

    public class MethodInfo
    {
        private string _name ;
        private List<ParameterInfo> _params = new List<ParameterInfo>();
        public MethodInfo ( string methodSymbolName , IEnumerable < ParameterInfo > @select )
        {
            this.Name = methodSymbolName ;
            Parameters.AddRange ( @select ) ;
        }

        public string Name { get { return _name ; } set { _name = value ; } }

        public List < ParameterInfo > Parameters
        {
            get { return _params ; }
            set { _params = value ; }
        }
    }

    public class ParameterInfo
    {
        public string Name { get ; }

        public string TypeDisplayString { get ; }

        public string TypeFullName { get ; }

        public 
            List <CustommodifierInfo> custommodifiers = new List < CustommodifierInfo > ();
        public ParameterInfo (
            string                             name
          , ITypeSymbol                        typeSymbol
          , IEnumerable < CustommodifierInfo > @select
          , string                             typeDisplayString
        )
        {
            Name = name ;
            TypeDisplayString = typeDisplayString ;
            TypeFullName = typeSymbol.ContainingNamespace.MetadataName
                           + '.'
                           + typeSymbol.MetadataName ; ;
            custommodifiers.AddRange (@select  );
        }
    }

    public class CustommodifierInfo
    {
        public bool IsOptional { get ; }

        public string DisplayString { get ; }

        public CustommodifierInfo ( bool isOptional , string displayString )
        {
            IsOptional = isOptional ;
            DisplayString = displayString ;
        }
    }
}