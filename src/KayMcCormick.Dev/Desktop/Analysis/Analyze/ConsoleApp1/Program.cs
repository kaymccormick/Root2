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
using System.Xml.Linq ;
using AnalysisAppLib ;
using AnalysisAppLib.Project ;
using AnalysisAppLib.Properties ;
using AnalysisAppLib.Syntax ;
using AnalysisControls ;
using AnalysisControls.ViewModel ;
using Autofac ;
using Autofac.Core ;
using CommandLine ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Dev.Command;
using KayMcCormick.Dev.Logging ;
using KayMcCormick.Lib.Wpf.Command ;
using Microsoft.Build.Locator ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.Text ;
using NLog ;
using NLog.Targets ;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory ;
using AppContext = AnalysisAppLib.AppContext;
using JsonConverters = KayMcCormick.Dev.Serialization.JsonConverters ;
using ProjectInfo = Microsoft.CodeAnalysis.ProjectInfo ;

// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global

// ReSharper disable RedundantOverriddenMember

// ReSharper disable AnnotateNotNullParameter

namespace ConsoleAnalysis
{
    internal sealed class Program
    {
        [ NotNull ] private static string ModelXamlFilename
        {
            get { return Path.Combine ( DataOutputPath , ModelXamlFilenamePart ) ; }
        }

        private const string DataOutputPath        = @"C:\data\logs" ;
        private const string TypesJsonFilename     = "types.json" ;
        private const string ModelXamlFilenamePart = "model.xaml" ;

        private const string SolutionFilePath =
            @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\work2\src\KayMcCormick.Dev\ManagedProd.sln" ;

        // ReSharper disable once InconsistentNaming
        private const string _pocoPrefix = "Poco" ;

        // ReSharper disable once InconsistentNaming
        private const string _collectionSuffix   = "Collection" ;
        private const string PocoSyntaxNamespace = "PocoSyntax" ;

        // ReSharper disable once InconsistentNaming
        private const string ICollection_typename = "ICollection" ;

        // ReSharper disable once InconsistentNaming
        private const string IList_typename = "IList" ;

        // ReSharper disable once InconsistentNaming
        private const string List_typename = "List" ;

        // ReSharper disable once InconsistentNaming
        private const string IEnumerable_typename = "IEnumerable" ;

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

        // ReSharper disable once NotAccessedField.Local
        private static ILogger _logger ;

        private static ApplicationInstance _appInstance ;

        static Program ( ) { _logger = null ; }

        [ NotNull ] public static string PocoPrefix { get { return _pocoPrefix ; } }

        private static void PopulateJsonConverters ( bool disableLogging )
        {
            if ( ! disableLogging )
            {
                if ( LogManager.Configuration == null )
                {
                    return ;
                }

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
            var consoleAnalysisProgramGuid = ApplicationInstanceIds.ConsoleAnalysisProgramGuid ;
            var subject = new Subject < ILogger > ( ) ;
            subject.Subscribe (
                               logger => {
                                   logger.Warn ( "Received logger" ) ;
                                   DebugUtils.WriteLine ( "got logger" ) ;
                                   _logger = logger ;
                                   if ( _appInstance != null )
                                   {
                                       _appInstance.Logger = logger ;
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
#pragma warning disable VSTHRD105 // Avoid method overloads that assume TaskScheduler.Current
                            .ContinueWith (
                                           task => Console.WriteLine (
                                                                      Resources
                                                                         .Program_Main_Logger_async_complete_
                                                                     )
                                          )
#pragma warning restore VSTHRD105 // Avoid method overloads that assume TaskScheduler.Current
                     ) ;
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed


            _appInstance = new ApplicationInstance (
                                                    new ApplicationInstance.
                                                        ApplicationInstanceConfiguration (
                                                                                          message
                                                                                              => {
                                                                                          }
                                                                                        , consoleAnalysisProgramGuid
                                                                                         )
                                                   ) ;
            using ( _appInstance )

            {
                _appInstance.AddModule ( new ConsoleAnalyzeAppModule ( ) ) ;
                _appInstance.AddModule ( new AnalysisAppLibModule ( ) ) ;
                _appInstance.AddModule ( new AnalysisControlsModule ( ) ) ;
                PopulateJsonConverters ( false ) ;
                ILifetimeScope scope ;
                try
                {
                    scope = _appInstance.GetLifetimeScope ( ) ;
                }
                catch ( ContainerBuildException buildException )
                {
                    Console.WriteLine ( buildException.Message ) ;
                    Console.WriteLine (
                                       Resources
                                          .Program_Main_Please_contact_your_administrator_for_assistance_
                                      ) ;
                    return 1 ;
                }

                AppContext context ;
                try
                {
                    context = scope.Resolve < AppContext > ( ) ;
                }
                catch ( DependencyResolutionException dependencyResolutionException )
                {
                    Exception ex1 = dependencyResolutionException ;
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


                var myOptions = new Options ( ) ;
                var fExit = false ;
                var program = context.Scope.Resolve < Program > ( ) ;
                Parser.Default.ParseArguments < Options > ( args )
                      .WithParsed ( o => myOptions     = o )
                      .WithNotParsed ( errors => fExit = true ) ;
                if ( fExit )
                {
                    return 1 ;
                }

                return await program.MainCommandAsync ( context , myOptions ) ;
            }
        }

        // ReSharper disable once MemberCanBeMadeStatic.Local
        private async Task < int > MainCommandAsync (
            [ NotNull ] AppContext context
          , [ NotNull ] Options    options
        )
        {
            // using ( var db = new AppDbContext ( ) )
            // {
            //     var x = new AppTypeInfo ( ) ;
            //     db.AppTypeInfos.Add ( x ) ;
            //     await db.SaveChangesAsync ( ) ;
            // }
            var cmds = context.Scope.Resolve < IEnumerable < IDisplayableAppCommand > > ( ) ;
            if ( ! string.IsNullOrEmpty ( options.Action )
                 && cmds.All ( a => a.DisplayName != options.Action ) )
            {
                throw new AppInvalidOperationException ( ) ;
            }

            SelectVsInstance ( ) ;

            //context.Options = options ;
            await RunConsoleUiAsync ( context ) ;

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
                if ( ! ( browserNode is IProjectBrowserNode project ) )
                {
                    continue ;
                }

                Console.WriteLine ( $"\tSolutionPath is {project.SolutionPath}" ) ;
                Console.WriteLine (
                                   $"\tConfiguration property Platform is {project.Platform ?? "Null"}"
                                  ) ;
                Console.WriteLine ( $"\tRepositoryUrl is {project.RepositoryUrl}" ) ;
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



        [ TitleMetadata ( "Load Syntax Examples" ) ]
        [ UsedImplicitly ]
        // ReSharper disable once FunctionComplexityOverflow
        public static async Task LoadSyntaxExamplesAsync ( AppContext context , SqlConnection c , MiscCommands miscCommands)
        {
            var bb2 = new SqlCommand (
                                      "insert into syntaxexample2 (example, syntaxkind, typename, tokens) values (@example, @kind, @typename, @tokens)"
                                    , c
                                     ) ;


            var set = new HashSet < Type > ( ) ;
            var model = context.Scope.Resolve < TypesViewModel > ( ) ;
            
            miscCommands.PopulateSet ( model.Root.SubTypeInfos , set ) ;
            var syntaxDict =
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
                foreach ( var kind in comp.DescendantTrivia ( x => true , true )
                                          .Select ( syntaxTrivia => syntaxTrivia.Kind ( ) ) )
                {
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

                        x2.Len ++ ;
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
                        x1.Len ++ ;
                    }


                    if ( r.Next ( 9 ) != 1
                         || ! set.Contains ( o.GetType ( ) ) )
                    {
                        continue ;
                    }

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
                    if ( ! syntaxDict.TryGetValue ( o.GetType ( ) , out var l ) )
                    {
                        l = Tuple.Create (
                                          new X ( )
                                        , new List < Tuple < SyntaxNode , string > > ( )
                                         ) ;
                        syntaxDict[ o.GetType ( ) ] = l ;
                    }

                    l.Item1.Len += o.ToString ( ).Length ;
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

                if ( ! set.Any ( ) )
                {
                    break ;
                }
            }

            foreach ( var k in xxx1 )
            {
                Console.WriteLine (
                                   Resources.Program_LoadSyntaxExamplesAsync_
                                 , k.Key
                                 , k.Value.Len
                                  ) ;
            }

            foreach ( var k in xxx )
            {
                Console.WriteLine (
                                   Resources.Program_LoadSyntaxExamplesAsync_
                                 , k.Key
                                 , k.Value.Len
                                  ) ;
            }

            foreach ( var keyValuePair in syntaxDict )
            {
                Console.WriteLine (
                                   Resources.Program_LoadSyntaxExamplesAsync__0_
                                 , keyValuePair.Key.Name
                                  ) ;
                Console.WriteLine (
                                   ( double ) keyValuePair.Value.Item1.Len
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


        private static void SelectVsInstance ( )
        {
            if ( ! MSBuildLocator.CanRegister )
            {
                return ;
            }

            var vsInstances = MSBuildLocator
                             .QueryVisualStudioInstances (
                                                          new VisualStudioInstanceQueryOptions
                                                          {
                                                              DiscoveryTypes =
                                                                  DiscoveryType.VisualStudioSetup
                                                          }
                                                         )
                             .First ( inst => inst.Version.Major == 16 ) ;
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

        // ReSharper disable once UnusedMember.Local

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
        public async Task 
            CodeGenAsync ( IBaseLibCommand command , [ NotNull ] AppContext context )
        {
            await Task.Run ( ( ) => CodeGen ( command , context ) ) ;
        }

        [ TitleMetadata ( "DB Populate" ) ]
        [ UsedImplicitly ]
        public async Task PopulateDbAsync (
            IBaseLibCommand        command
          , [ NotNull ] AppContext context
        )
        {
            using ( var db = new AppDbContext ( ) )
            {
                var p = new ProjectBrowserViewModel ( ) ;
                await db.Projects.AddRangeAsync ( p.Projects ) ;
                await db.SaveChangesAsync ( ) ;
            }
        }


#pragma warning disable VSTHRD200 // Use "Async" suffix for async methods
        // ReSharper disable once FunctionComplexityOverflow
        public static async Task CodeGen (
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
            // ReSharper disable once UnusedVariable
            var sts = context.Scope.Resolve < ISyntaxTypesService > ( ) ;
            var collectionMap = model1.CollectionMap ( ) ;
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
            foreach ( var mapKey1 in model1.Map.Dict.Keys )
            {
                var t = model1.Map.Dict[ mapKey1 ] ;
                // ReSharper disable once UnusedVariable
                var curIterAppTypeInfo = t ;
                var colTypeClassName = $"{PocoPrefix}{t.Type.Name}{_collectionSuffix}" ;

                var classDecl1 = CreatePoco ( mapKey1 , t ) ;
                var curComp = ReplaceSyntaxTree ( x , classDecl1 ) ;

                // ReSharper disable once UnusedVariable
                var classDecl1Type =
                    curComp.GetTypeByMetadataName ( classDecl1.Identifier.ValueText ) ;
                // ReSharper disable once IdentifierTypo
                // ReSharper disable once InconsistentNaming
                var simplebaseType_ilist1 = SimpleBaseType ( ParseTypeName ( IList_typename ) ) ;
                var enumerable1 = SimpleBaseType ( ParseTypeName ( IEnumerable_typename ) ) ;
                // ReSharper disable once InconsistentNaming
                var simpleBaseType_ICollection =
                    SimpleBaseType ( ParseTypeName ( ICollection_typename ) ) ;
                var classContainerDecl = ClassDeclaration ( colTypeClassName )
                                        .WithBaseList (
                                                       BaseList ( )
                                                          .AddTypes (
                                                                     simplebaseType_ilist1
                                                                   , enumerable1
                                                                   , simpleBaseType_ICollection
                                                                    )
                                                      )
                                        .WithModifiers (
                                                        SyntaxTokenList.Create (
                                                                                Token (
                                                                                       SyntaxKind
                                                                                          .PublicKeyword
                                                                                      )
                                                                               )
                                                       ) ;

                var typeSyntax2 =
                    ParseTypeName ( t.Type.FullName ?? throw new AppInvalidOperationException ( ) ) ;
                var typeSyntax = ParseTypeName (
                                                PocoPrefix
                                                + ( ( QualifiedNameSyntax ) typeSyntax2 )
                                                 .Right.Identifier
                                               ) ;
                // ReSharper disable once InconsistentNaming
                var internal_genericIList = GenericName (
                                                         Identifier ( IList_typename )
                                                       , TypeArgumentList (
                                                                           SeparatedList <
                                                                                   TypeSyntax > ( )
                                                                              .Add ( typeSyntax )
                                                                          )
                                                        ) ;
                // ReSharper disable once InconsistentNaming
                var internal_genericList = GenericName (
                                                        Identifier ( List_typename )
                                                      , TypeArgumentList (
                                                                          SeparatedList < TypeSyntax
                                                                              > ( )
                                                                             .Add ( typeSyntax )
                                                                         )
                                                       ) ;
                // ReSharper disable once InconsistentNaming
                var IListRuntimeType = typeof ( IList ) ;
                // ReSharper disable once InconsistentNaming
                var ICollectionRuntimeType = typeof ( ICollection ) ;
                // ReSharper disable once InconsistentNaming
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
                                                 ?? throw new AppInvalidOperationException ( )
                                                ) ;
                    if ( typeByMetadataName == null )
                    {
                        continue ;
                    }

                    var generic1 =
                        typeByMetadataName ; // t123.ConstructUnboundGenericType ( ).Construct ( classDecl1Type ) ;

                    // var i = model.GetSymbolInfo ( SyntaxFactory.ParseTypeName ( s1 ) ) ;


                    var listField = IdentifierName ( "_list" ) ;

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
                        var propDecl = PropertyDeclaration ( propertyTypeParsed , prop.Name ) ;
                        propDecl = propDecl.WithModifiers ( publicKeyword ) ;
                        var propertyIdentifierNameSyntax = IdentifierName ( prop.Name ) ;
                        var propertyAccess = MemberAccessExpression (
                                                                     SyntaxKind
                                                                        .SimpleMemberAccessExpression
                                                                   , listField
                                                                   , propertyIdentifierNameSyntax
                                                                    ) ;
                        var arrowExpressionClauseSyntax = ArrowExpressionClause ( propertyAccess ) ;
                        propDecl = propDecl.WithExpressionBody ( arrowExpressionClauseSyntax )
                                           .WithSemicolonToken (
                                                                Token ( SyntaxKind.SemicolonToken )
                                                               ) ;
                        return propDecl ;
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
                                                        var bArgList =
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
                                                                                     listField
                                                                                   , bArgList
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
                                                        var setAccessor =
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
                                                                                                            listField
                                                                                                          , bArgList
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
                                                                                                               , setAccessor
                                                                                                             }
                                                                                                            )
                                                                                              )
                                                                                ) ;
                                                    }
                                                   ) ;

                    var members1 = generic1.GetMembers ( )
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
                                                                DebugUtils.WriteLine (
                                                                                      $"{p1.Type}"
                                                                                     ) ;
                                                            }

                                                            return Parameter (
                                                                              List <
                                                                                  AttributeListSyntax
                                                                              > ( )
                                                                            , new
                                                                                  SyntaxTokenList ( )
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
                                                                                                                                                      , listField
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
                                                                                                                                                                                                           ? internal_genericIList
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
                                                                                  .Concat (
                                                                                           members1
                                                                                          )
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

                var invocationExpressionSyntax =
                    InvocationExpression (
                                          MemberAccessExpression (
                                                                  SyntaxKind
                                                                     .SimpleMemberAccessExpression
                                                                , ParenthesizedExpression (
                                                                                           CastExpression (
                                                                                                           internal_genericList
                                                                                                         , IdentifierName (
                                                                                                                           "_list"
                                                                                                                          )
                                                                                                          )
                                                                                          )
                                                                , Token ( SyntaxKind.DotToken )
                                                                , ( SimpleNameSyntax ) ParseName (
                                                                                                  "AddRange"
                                                                                                 )
                                                                 )
                                         )
                       .WithArgumentList (
                                          ArgumentList (
                                                        Token ( SyntaxKind.OpenParenToken )
                                                      , new SeparatedSyntaxList < ArgumentSyntax
                                                            > ( )
                                                           .Add (
                                                                 Argument (
                                                                           IdentifierName (
                                                                                           "initList"
                                                                                          )
                                                                          )
                                                                )
                                                      , Token ( SyntaxKind.CloseParenToken )
                                                       )
                                         ) ;
                var typeArgument = internal_genericIList.TypeArgumentList.Arguments[ 0 ] ;

                var constructor = ConstructorDeclaration ( classContainerDecl.Identifier.ValueText )
                                 .WithParameterList (
                                                     ParameterList (
                                                                    new SeparatedSyntaxList <
                                                                        ParameterSyntax > ( ).Add (
                                                                                                   Parameter (
                                                                                                              Identifier (
                                                                                                                          "initList"
                                                                                                                         )
                                                                                                             )
                                                                                                      .WithType (
                                                                                                                 GenericName (
                                                                                                                              Identifier (
                                                                                                                                          "IList"
                                                                                                                                         )
                                                                                                                             )
                                                                                                                    .WithTypeArgumentList (
                                                                                                                                           TypeArgumentList (
                                                                                                                                                             SingletonSeparatedList (
                                                                                                                                                                                     typeArgument
                                                                                                                                                                                    )
                                                                                                                                                            )
                                                                                                                                          )
                                                                                                                )
                                                                                                  )
                                                                   )
                                                    )
                                 .WithBody (
                                            Block (
                                                   ExpressionStatement (
                                                                        invocationExpressionSyntax
                                                                       )
                                                  )
                                           )
                                 .WithModifiers (
                                                 SyntaxTokenList.Create (
                                                                         Token (
                                                                                SyntaxKind
                                                                                   .PublicKeyword
                                                                               )
                                                                        )
                                                ) ;

                classContainerDecl = classContainerDecl.WithMembers (
                                                                     List (
                                                                           classContainerDecl
                                                                              .Members.Concat (
                                                                                               new
                                                                                               MemberDeclarationSyntax
                                                                                               []
                                                                                               {
                                                                                                   constructor
                                                                                               }
                                                                                              )
                                                                          )
                                                                    ) ;

                var argumentListSyntax = ArgumentList (
                                                       Token ( SyntaxKind.OpenParenToken )
                                                     , SeparatedList < ArgumentSyntax > ( )
                                                     , Token ( SyntaxKind.CloseParenToken )
                                                      ) ;
                var listIdentifier = Identifier ( "List" ) ;
                var constructedListGeneric = GenericName (
                                                          listIdentifier
                                                        , TypeArgumentList (
                                                                            SingletonSeparatedList (
                                                                                                    typeSyntax
                                                                                                   )
                                                                           )
                                                         ) ;
                var ocEx = ObjectCreationExpression (
                                                     constructedListGeneric
                                                   , argumentListSyntax
                                                   , default
                                                    ) ;
                var fds = FieldDeclaration (
                                            VariableDeclaration ( ParseTypeName ( IList_typename ) )
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
                                                                                                                ocEx
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

                    // ReSharper disable once IdentifierTypo
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

                    TypeSyntax type ;
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
                                                                                           , model1
                                                                                            )
                                                                         // ReSharper disable once AssignNullToNotNullAttribute
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

                var documentationCommentTriviaSyntax = DocumentationCommentTrivia (
                                                                                   SyntaxKind
                                                                                      .SingleLineDocumentationCommentTrivia
                                                                                 , List <
                                                                                           XmlNodeSyntax
                                                                                       > ( )
                                                                                      .Add (
                                                                                            XmlSummaryElement ( )
                                                                                           )
                                                                                  ) ;
                // ReSharper disable once UnusedVariable
                var tokens1 = documentationCommentTriviaSyntax
                             .DescendantTokens ( x111 => true , true )
                             .ToList ( ) ;
                classDecl1 = classDecl1.WithMembers ( members ) ;

                types = types.Add ( classDecl1 ) ;
            }


            DebugOut ( "About to build compilation unit" ) ;

            var compilation = CompilationUnit ( ) ;
            compilation = SyntaxTypesService.WithCollectionUsings ( compilation ) ;
            compilation = compilation.WithMembers (
                                                   new SyntaxList < MemberDeclarationSyntax > (
                                                                                               NamespaceDeclaration (
                                                                                                                     ParseName (
                                                                                                                                PocoSyntaxNamespace
                                                                                                                               )
                                                                                                                    )
                                                                                                  .WithMembers (
                                                                                                                types
                                                                                                               )
                                                                                              )
                                                  )
                                     .NormalizeWhitespace ( ) ;

            DebugOut ( "built" ) ;
            var tree = SyntaxTree ( compilation ) ;
            var src = tree.ToString ( ) ;

            DebugOut ( "Reparsing text ??" ) ;

            var tree2 = CSharpSyntaxTree.Create (
                                                 compilation
                                               , new CSharpParseOptions (
                                                                         LanguageVersion.CSharp7_3
                                                                        )
                                                ) ;

            File.WriteAllText ( @"C:\data\logs\gen.cs" , compilation.ToString ( ) ) ;

            //refs, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, false, "test", null, null, null, OptimizationLevel.Debug, false, false, null, null, default, default ,default, default, default,default, default, default, default, default, new MetadataReferenceResolver())
            var source = tree2.ToString ( ).Split ( new[] { "\r\n" } , StringSplitOptions.None ) ;

            //var compilation = CSharpCompilation.Create ( "test" , new[] { tree2 } ) ;
            var workspace = new AdhocWorkspace ( ) ;
            var projectId = ProjectId.CreateNewId ( ) ;
            DebugOut ( "Add solution" ) ;

            var s = workspace.AddSolution (
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

            var sourceText = SourceText.From (
                                              @"     using System;
using System.Collections;
using System.Collections.Generic;

public class PocoSyntaxTokenList : IList, IEnumerable, ICollection
    {
        // System.Collections.IList
        public Int32 Add(Object value) => _list.Add((PocoSyntaxToken)value);
        // System.Collections.IList
        public Boolean Contains(Object value) => _list.Contains((PocoSyntaxToken)value);
        // System.Collections.IList
        public void Clear() => _list.Clear();
        // System.Collections.IList
        public Int32 IndexOf(Object value) => _list.IndexOf((PocoSyntaxToken)value);
        // System.Collections.IList
        public void Insert(Int32 index, Object value) => _list.Insert((Int32)index, (PocoSyntaxToken)value);
        // System.Collections.IList
        public void Remove(Object value) => _list.Remove((PocoSyntaxToken)value);
        // System.Collections.IList
        public void    RemoveAt(Int32 index) => _list.RemoveAt((Int32)index);
        public Boolean IsReadOnly            => _list.IsReadOnly;
        public Boolean IsFixedSize           => _list.IsFixedSize;
        public Object this[Int32 index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        // System.Collections.ICollection
        public void    CopyTo(Array array, Int32 index) => _list.CopyTo((Array)array, (Int32)index);
        public Int32   Count          => _list.Count;
        public Object  SyncRoot       => _list.SyncRoot;
        public Boolean IsSynchronized => _list.IsSynchronized;
        // System.Collections.IEnumerable
        public IEnumerator GetEnumerator() => _list.GetEnumerator();
        IList              _list = new List<PocoSyntaxToken>();
    }

    public class PocoSyntaxToken
    {
        public int RawKind { get; set; }

        public string Kind { get; set; }

        public object Value { get; set; }

        public string ValueText { get; set; }
    }
"
                                             ) ;

            File.WriteAllText ( "misc.cs" , sourceText.ToString ( ) ) ;

            var document2 = DocumentInfo.Create (
                                                 DocumentId.CreateNewId ( projectId )
                                               , "misc"
                                               , null
                                               , SourceCodeKind.Regular
                                               , TextLoader.From (
                                                                  TextAndVersion.Create (
                                                                                         sourceText
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

            var rb1 = workspace.TryApplyChanges ( s2 ) ;
            if ( ! rb1 )
            {
                throw new AppInvalidOperationException ( ) ;
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
                var s3 = workspace.CurrentSolution.AddMetadataReference (
                                                                         projectId
                                                                       , MetadataReference
                                                                            .CreateFromFile ( ref1 )
                                                                        ) ;
                var rb = workspace.TryApplyChanges ( s3 ) ;
                if ( ! rb )
                {
                    throw new AppInvalidOperationException ( ) ;
                }
            }

            DebugOut ( "Applying assembly done" ) ;
            var project = workspace.CurrentSolution.Projects.First ( ) ;

            var comp1 = await project.GetCompilationAsync ( ) ;
            using ( var f = new StreamWriter ( @"C:\data\logs\errors.txt" ) )
            {
                if ( comp1 != null )
                {
                    foreach ( var diagnostic in comp1.GetDiagnostics ( ) )
                    {
                        if ( diagnostic.IsSuppressed )
                        {
                            continue ;
                        }

                        // ReSharper disable once UnusedVariable
                        var line = source
                                  .Skip (
                                         diagnostic.Location.GetLineSpan ( ).StartLinePosition.Line
                                         - 1
                                        )
                                  .Take (
                                         diagnostic.Location.GetLineSpan ( ).EndLinePosition.Line
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

            var errors = comp1?.GetDiagnostics ( )
                               .Where ( d => d.Severity == DiagnosticSeverity.Error )
                               .ToList ( ) ;
            if ( errors?.Any ( ) == true )
            {
                DebugUtils.WriteLine ( string.Join ( "\n" , errors ) ) ;
            }

            DebugOut ( "attempting emit" ) ;

            var result =
                ( comp1 ?? throw new AppInvalidOperationException ( ) ).Emit (
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
            if ( t.ParentInfo == null )
            {
                return classDecl1 ;
            }

            var identifierNameSyntax = IdentifierName ( _pocoPrefix + t.ParentInfo.Type.Name ) ;
            classDecl1 = classDecl1.WithBaseList (
                                                  BaseList (
                                                            new SeparatedSyntaxList < BaseTypeSyntax
                                                            > ( ).Add (
                                                                       SimpleBaseType (
                                                                                       identifierNameSyntax
                                                                                      )
                                                                      )
                                                           )
                                                 ) ;

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

        // ReSharper disable once UnusedMember.Local

        
    }

    internal sealed class Options
    {
        [ Option ( 's' , "sln" , Required = false ) ]
        public string SolutionFile { get ; set ; }

        [ Option ( 'a' , "action" , Required = false ) ]
        public string Action { get ; set ; }
    }
}