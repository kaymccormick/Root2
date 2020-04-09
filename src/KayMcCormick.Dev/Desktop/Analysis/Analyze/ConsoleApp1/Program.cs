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
using AnalysisAppLib.Properties ;
using AnalysisControls ;
using AnalysisControls.ViewModel ;
using Autofac ;
using Autofac.Core ;
using Autofac.Features.Metadata ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Application ;
using KayMcCormick.Dev.Attributes ;
using KayMcCormick.Dev.Logging ;
using Microsoft.Build.Locator ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.MSBuild ;
using Microsoft.CodeAnalysis.Text ;
using NLog ;
using NLog.Targets ;
using JsonConverters = KayMcCormick.Dev.Serialization.JsonConverters ;

namespace ConsoleApp1
{
    internal sealed class Program
    {
        private const string ModelXamlFilename = @"C:\data\logs\model.xaml" ;

        private const string SolutionFilePath =
            @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\reanalyze2\src\KayMcCormick.Dev\ManagedProd.sln" ;

        private static readonly string[] AssemblyRefs = new[]
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
                                                        } ;

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

        private static ILogger Logger ;

        private static ApplicationInstance _appinst ;
        static Program ( ) { Logger = null ; }

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

            Init ( ) ;
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

                var program = context.Scope.Resolve < Program > ( ) ;

                return await program.MainCommandAsync ( context ) ;
            }
        }

        private async Task < int > MainCommandAsync ( [ NotNull ] AppContext context )
        {
            SelectVsInstance ( ) ;

            await RunConsoleUiAsync ( context ) ;

            return 1 ;
        }

        public static async Task SelectProjectAsync ( [ NotNull ] AppContext context )
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
        }

        [ TitleMetadata ( "Build Types View" ) ]
        public async Task BuildTypeViewAsync ( IBaseLibCommand command , AppContext context )
        {
            var options = context.Scope.Resolve < JsonSerializerOptions > ( ) ;
            // options.Converters.Add (new JsonPocoSyntaxConverter()  );
            // await            CollectDocs ( options ) ;
            // return 1 ;
            var typesViewModel = context.Scope.Resolve < TypesViewModel > ( ) ;

            WriteThisTypesViewModel ( typesViewModel ) ;

            LoadSyntax ( typesViewModel ) ;
            foreach ( AppTypeInfo ati in typesViewModel.Map.Values )
            {
                typesViewModel.PopulateFieldTypes ( ati ) ;
            }

            WriteThisTypesViewModel ( typesViewModel ) ;
            typesViewModel.DetailFields ( ) ;
            WriteThisTypesViewModel ( typesViewModel ) ;
            DumpModelToJson ( context , typesViewModel ) ;
            // return 1 ;
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
                        info               = new SyntaxInfo ( ) { Kind = kind } ;
                        triviaDict[ kind ] = info ;
                    }

                    info.Count ++ ;
                }

                Console.Write (
                               string.Join (
                                            ", "
                                          , triviaDict.Select (
                                                               ( kv )
                                                                   => $"{kv.Key} = {kv.Value.Count}"
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
        private static async Task < SqlConnection > DumpSyntaxExamples ( )
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
                                               , new JsonWriterOptions ( ) { Indented = true }
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
                             .Where ( inst => inst.Version.Major == 15 )
                             .First ( ) ;
            MSBuildLocator.RegisterInstance ( vsInstances ) ;
            return ;
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
            var solution = await workspace.OpenSolutionAsync ( SolutionFilePath ) ;
            var documentsOut = new List < CodeElementDocumentation > ( ) ;
            foreach ( var project in solution.Projects.Where ( proj => proj.Name != "Explorer" ) )
            {
                // ReSharper disable once UnusedVariable
                var compilation = await project.GetCompilationAsync ( ) ;
                foreach ( var doc in project.Documents )
                {
                    // ReSharper disable once UnusedVariable
                    var model = await doc.GetSemanticModelAsync ( ) ;
                    Console.WriteLine ( doc.Name ) ;
                    // ReSharper disable once UnusedVariable
                    var tree = await doc.GetSyntaxRootAsync ( ) ;

                    foreach ( var node in tree
                                         .DescendantNodesAndSelf ( )
                                         .OfType < MemberDeclarationSyntax > ( ) )
                    {
                        //
                        // if ( SupportsDocumentationComments ( tuple ) )
                        // {
                        //     
                        var declared = model.GetDeclaredSymbol ( node ) ;
                        if ( declared != null )
                        {
                            var xml1 = declared.GetDocumentationCommentXml ( ) ;
                            if ( declared.DeclaredAccessibility == Accessibility.Public
                                 && SupportsDocumentationComments ( node ) )
                            {
                                var docid = declared.GetDocumentationCommentId ( ) ;
                                // if ( xml1 == null
                                //      || String.IsNullOrEmpty ( xml1 ) )
                                // {
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


                                    var o1 = XmlDocElements.HandleDocElementNode (
                                                                                  doc1
                                                                                , docid
                                                                                , node
                                                                                , ( ISymbol )
                                                                                  declared
                                                                                 ) ;
                                    if ( ! string.IsNullOrWhiteSpace ( xml1 ) )
                                    {
                                        o1.NeedsAttention = true ;
                                    }

                                    documentsOut.Add ( o1 ) ;
                                }
                                catch
                                {
                                }




                                // DebugUtils.WriteLine ( JsonSerializer.Serialize ( o ) ) ;
                            }
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
            }

            var li = new ArrayList ( ) ;
            foreach ( var codeElementDocumentation in documentsOut )
            {
                li.Add ( codeElementDocumentation ) ;
            }

            XamlWriter.Save (
                             li
                           , XmlWriter.Create (
                                               @"C:\temp\docs.xaml"
                                             , new XmlWriterSettings ( ) { Indent = true }
                                              )
                            ) ;
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

        public async Task CodeGen ( IBaseLibCommand command , [ NotNull ] AppContext context )
        {
            var outputFunc = ( Action < string > ) command.Argument ;
            Action < string > debugOut = s1 => {
                DebugUtils.WriteLine ( s1 ) ;
                outputFunc ( s1 ) ;
            } ;
            outputFunc ( "Beginning" ) ;
            var model1 = context.Scope.Resolve < TypesViewModel > ( ) ;
            var types =
                new SyntaxList < MemberDeclarationSyntax
                > ( ) ; //new [] { SyntaxFactory.ClassDeclaration("SyntaxToken")} ) ;
            outputFunc ( $"{model1.Map.Count} Entries in Type map" ) ;
            foreach ( Type mapKey in model1.Map.Keys )
            {
                debugOut ( $"{mapKey}" ) ;
                var t = ( AppTypeInfo ) model1.Map[ mapKey ] ;
                var members = new SyntaxList < MemberDeclarationSyntax > ( ) ;
                foreach ( SyntaxFieldInfo tField in t.Fields )
                {
                    debugOut ( $"{tField}" ) ;
                    if ( tField.ClrTypeName != null
                         && tField.Type     == null )
                    {
                        tField.Type = Type.GetType ( tField.ClrTypeName ) ;
                        if ( tField.Type == null )
                        {
                            DebugUtils.WriteLine (
                                                  $"unable to resolve typ{tField.ClrTypeName}e "
                                                 ) ;
                        }
                    }

                    if ( tField.Type        == null
                         && tField.Type     != typeof ( bool )
                         && tField.TypeName != "bool" )
                    {
                        continue ;
                    }

                    var accessorDeclarationSyntaxes = new SyntaxList < AccessorDeclarationSyntax > (
                                                                                                    new
                                                                                                    []
                                                                                                    {
                                                                                                        SyntaxFactory
                                                                                                           .AccessorDeclaration (
                                                                                                                                 SyntaxKind
                                                                                                                                    .GetAccessorDeclaration
                                                                                                                                )
                                                                                                           .WithSemicolonToken (
                                                                                                                                SyntaxFactory
                                                                                                                                   .Token (
                                                                                                                                           SyntaxKind
                                                                                                                                              .SemicolonToken
                                                                                                                                          )
                                                                                                                               )
                                                                                                      , SyntaxFactory
                                                                                                       .AccessorDeclaration (
                                                                                                                             SyntaxKind
                                                                                                                                .SetAccessorDeclaration
                                                                                                                            )
                                                                                                       .WithSemicolonToken (
                                                                                                                            SyntaxFactory
                                                                                                                               .Token (
                                                                                                                                       SyntaxKind
                                                                                                                                          .SemicolonToken
                                                                                                                                      )
                                                                                                                           )
                                                                                                    }
                                                                                                   ) ;
                    var accessorListSyntax =
                        SyntaxFactory.AccessorList ( accessorDeclarationSyntaxes ) ;

                    var tFieldTypeName = tField.TypeName ;
                    if ( tFieldTypeName == "SyntaxTokenList" )
                    {
                        tFieldTypeName = "List<SyntaxToken>" ;
                    }
                    else if ( tFieldTypeName.StartsWith ( "SyntaxList<" ) )
                    {
                        tFieldTypeName = tFieldTypeName.Replace ( "SyntaxList<" , "List<" ) ;
                    }
                    else if ( tFieldTypeName.StartsWith ( "SeparatedSyntaxList<" ) )
                    {
                        tFieldTypeName =
                            tFieldTypeName.Replace ( "SeparatedSyntaxList<" , "List<" ) ;
                    }

                    var typeSyntax =
                        SyntaxFactory.ParseTypeName (
                                                     tFieldTypeName
                                                    ) ; /*SyntaxFactory.IdentifierName (
                                                                                                                 tField
                                                                                                                    .Type
                                                                                                                    .Name
                                                                                                                ) ;*/
                    if ( typeSyntax is GenericNameSyntax gen )
                    {
                        var ss = ( SimpleNameSyntax ) gen.TypeArgumentList.Arguments[ 0 ] ;
                        typeSyntax = gen.WithTypeArgumentList (
                                                               SyntaxFactory.TypeArgumentList (
                                                                                               new
                                                                                                       SeparatedSyntaxList
                                                                                                       < TypeSyntax
                                                                                                       > ( )
                                                                                                  .Add (
                                                                                                        SyntaxFactory
                                                                                                           .ParseTypeName (
                                                                                                                           "Poco"
                                                                                                                           + ss
                                                                                                                            .Identifier
                                                                                                                            .Text
                                                                                                                          )
                                                                                                       )
                                                                                              )
                                                              ) ;
                    }
                    else if ( tField.TypeName != "bool" )
                    {
                        typeSyntax =
                            SyntaxFactory.ParseTypeName (
                                                         "Poco"
                                                         + ( ( SimpleNameSyntax ) typeSyntax )
                                                          .Identifier.Text
                                                        ) ;
                    }

                    var tokens = new List < SyntaxToken >
                                 {
                                     SyntaxFactory.Token ( SyntaxKind.PublicKeyword )
                                   , tField.Override
                                         ? SyntaxFactory.Token ( SyntaxKind.OverrideKeyword )
                                         : SyntaxFactory.Token ( SyntaxKind.VirtualKeyword )
                                 } ;


                    var nameSyntax = SyntaxFactory.MemberAccessExpression (
                                                                           SyntaxKind
                                                                              .SimpleMemberAccessExpression
                                                                         , SyntaxFactory
                                                                              .IdentifierName (
                                                                                               "DesignerSerializationVisibility"
                                                                                              )
                                                                         , SyntaxFactory
                                                                              .IdentifierName (
                                                                                               "Content"
                                                                                              )
                                                                          ) ;
                    ////.ParseName ( "System.ComponentModel.DesignerSerializationVisibility" ) ;
                    var attributeSyntax = SyntaxFactory.Attribute (
                                                                   SyntaxFactory.ParseName (
                                                                                            "DesignerSerializationVisibility"
                                                                                           )
                                                                 , SyntaxFactory
                                                                      .AttributeArgumentList (
                                                                                              SyntaxFactory
                                                                                                 .SeparatedList
                                                                                                  < AttributeArgumentSyntax
                                                                                                  > (
                                                                                                     new
                                                                                                     []
                                                                                                     {
                                                                                                         SyntaxFactory
                                                                                                            .AttributeArgument (
                                                                                                                                nameSyntax
                                                                                                                               )
                                                                                                     }
                                                                                                    )
                                                                                             )
                                                                  ) ;
                    DebugUtils.WriteLine ( attributeSyntax ) ;
                    var separatedSyntaxList =
                        new SeparatedSyntaxList < AttributeSyntax > ( ).Add ( attributeSyntax ) ;
                    var attributeListSyntaxes = SyntaxFactory.List (
                                                                    new[]
                                                                    {
                                                                        SyntaxFactory
                                                                           .AttributeList (
                                                                                           separatedSyntaxList
                                                                                          )
                                                                    }
                                                                   ) ;
                    var syntaxTokenList = SyntaxFactory.TokenList ( tokens.ToArray ( ) ) ;
                    var propertyName = SyntaxFactory.Identifier ( tField.Name ) ;
                    var propertyDeclarationSyntax =
                        SyntaxFactory.PropertyDeclaration (
                                                           attributeListSyntaxes
                                                         , syntaxTokenList
                                                         , typeSyntax
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

                var classDecl = SyntaxFactory
                               .ClassDeclaration ( "Poco" + mapKey.Name )
                               .WithModifiers (
                                               SyntaxTokenList.Create (
                                                                       SyntaxFactory.Token (
                                                                                            SyntaxKind
                                                                                               .PublicKeyword
                                                                                           )
                                                                      )
                                              )
                               .WithMembers ( members ) ;
                if ( t.ParentInfo != null )
                {
                    classDecl = classDecl.WithBaseList (
                                                        SyntaxFactory.BaseList (
                                                                                new
                                                                                    SeparatedSyntaxList
                                                                                    < BaseTypeSyntax
                                                                                    > ( ).Add (
                                                                                               SyntaxFactory
                                                                                                  .SimpleBaseType (
                                                                                                                   SyntaxFactory
                                                                                                                      .IdentifierName (
                                                                                                                                       "Poco"
                                                                                                                                       + t
                                                                                                                                        .ParentInfo
                                                                                                                                        .Type
                                                                                                                                        .Name
                                                                                                                                      )
                                                                                                                  )
                                                                                              )
                                                                               )
                                                       ) ;
                }

                types = types.Add ( classDecl ) ;
            }

            debugOut ( "About to build compilation unit" ) ;
            var compl = SyntaxFactory.CompilationUnit ( )
                                     .WithUsings (
                                                  new SyntaxList < UsingDirectiveSyntax > (
                                                                                           new[]
                                                                                           {
                                                                                               SyntaxFactory
                                                                                                  .UsingDirective (
                                                                                                                   SyntaxFactory
                                                                                                                      .ParseName (
                                                                                                                                  "System"
                                                                                                                                 )
                                                                                                                  )
                                                                                             , SyntaxFactory
                                                                                                  .UsingDirective (
                                                                                                                   SyntaxFactory
                                                                                                                      .ParseName (
                                                                                                                                  "System.Collections.Generic"
                                                                                                                                 )
                                                                                                                  )
                                                                                             , SyntaxFactory
                                                                                                  .UsingDirective(
                                                                                                                  SyntaxFactory
                                                                                                                     .ParseName(
                                                                                                                                "System.ComponentModel"
                                                                                                                               )
                                                                                                                 )

        }
                                                                                          )
                                                 )
                                     .WithMembers (
                                                   new SyntaxList < MemberDeclarationSyntax > (
                                                                                               SyntaxFactory
                                                                                                  .NamespaceDeclaration (
                                                                                                                         SyntaxFactory
                                                                                                                            .ParseName (
                                                                                                                                        "PocoSyntax"
                                                                                                                                       )
                                                                                                                        )
                                                                                                  .WithMembers (
                                                                                                                types
                                                                                                               )
                                                                                              )
                                                  )
                                     .NormalizeWhitespace ( ) ;
            debugOut ( "built" ) ;
            var tree = SyntaxFactory.SyntaxTree ( compl ) ;
            var src = tree.ToString ( ) ;

            debugOut ( "Reparsing text ??" ) ;
            var tree2 = CSharpSyntaxTree.Create(compl,new CSharpParseOptions(LanguageVersion.CSharp7_3));

            File.WriteAllText(@"C:\data\logs\gen.cs", compl.ToString());

            //refs, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, false, "test", null, null, null, OptimizationLevel.Debug, false, false, null, null, default, default ,default, default, default,default, default, default, default, default, new MetadataReferenceResolver())
            var source = tree2.ToString ( ).Split ( new[] { "\r\n" } , StringSplitOptions.None ) ;
            //var compilation = CSharpCompilation.Create ( "test" , new[] { tree2 } ) ;
            var adhoc = new AdhocWorkspace ( ) ;
            var projectId = ProjectId.CreateNewId ( ) ;
            debugOut ( "Add solution" ) ;
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
                                                                                                   @"public class PocoSyntaxToken { public int RawKind { get; set; } public string Kind { get; set; } public object Value {get; set;} public string ValueText { get; set; } }"
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

            debugOut ( "Applying assembly refs" ) ;
            
            foreach ( var ref1 in AssemblyRefs.Union(new [] {typeof(DesignerSerializationOptions).Assembly.Location}) )
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

            debugOut ( "Applying assembly done" ) ;
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
                            var line =
                                source[ diagnostic
                                       .Location.GetLineSpan ( )
                                       .StartLinePosition.Line ] ;
                            await f.WriteLineAsync ( diagnostic.ToString ( ) ) ;
                        }
                    }
                }
            }

            var errors = compilation.GetDiagnostics ( )
                                 .Where ( d => d.Severity == DiagnosticSeverity.Error ).ToList() ;
            if ( errors .Any())
            {
                DebugUtils.WriteLine ( string.Join ( "\n" , errors ) ) ;
            }
            debugOut ( "attempting emit" ) ;
            var result =
                ( compilation ?? throw new InvalidOperationException ( ) ).Emit (
                                                                                 @"C:\data\logs\output.dll"
                                                                                ) ;
            if ( result.Success )
            {
                debugOut ( "Success" ) ;
            }

            //File.WriteAllText ( @"C:\data\logs\gen.cs" , comp.ToString ( ) ) ;
        }

        // ReSharper disable once UnusedMember.Local
        private static void LoadSyntax ( [ NotNull ] TypesViewModel model1 )
        {
            var syntax = XDocument.Parse ( Resources.Syntax ) ;
            if ( syntax.Root != null )
            {
                foreach ( var xElement in syntax.Root.Elements ( ) )
                {
                    switch ( xElement.Name.LocalName )
                    {
                        case "PredefinedNode" :
                            var typeName = xElement.Attribute ( XName.Get ( "Name" ) )?.Value ;
                            typeName = $"Microsoft.CodeAnalysis.CSharp.Syntax.{typeName}" ;
                            var t = typeof ( CSharpSyntaxNode ).Assembly.GetType ( typeName ) ;
                            if ( t == null )
                            {
                                DebugUtils.WriteLine ( "No type for " + typeName ) ;
                            }
                            else if ( model1.Map.Contains ( t ) )
                            {
                                var typ = ( AppTypeInfo ) model1.Map[ t ] ;
                                typ.ElementName = xElement.Name.LocalName ;
                            }

                            break ;
                        case "AbstractNode" :
                            ParseNodeBasics ( model1 , xElement ) ;
                            break ;
                        case "Node" :
                            ParseNodeBasics ( model1 , xElement ) ;

                            break ;


                        default : throw new InvalidOperationException ( ) ;
                    }
                }
            }

            var d = new TypeMapDictionary { [ typeof ( string ) ] = new AppTypeInfo ( ) } ;
            DebugUtils.WriteLine ( XamlWriter.Save ( d ) ) ;

            DebugUtils.WriteLine ( XamlWriter.Save ( model1 ) ) ;
        }

        private static void ParseNodeBasics (
            TypesViewModel       model1
          , [ NotNull ] XElement xElement
        )
        {
            var typeName2 = xElement.Attribute ( XName.Get ( "Name" ) )?.Value ;
            typeName2 = $"Microsoft.CodeAnalysis.CSharp.Syntax.{typeName2}" ;
            var t2 = typeof ( CSharpSyntaxNode ).Assembly.GetType ( typeName2 ) ;
            if ( t2 == null )
            {
                DebugUtils.WriteLine ( "No type for " + typeName2 ) ;
            }
            else
            {
                var typ2 = ( AppTypeInfo ) model1.Map[ t2 ] ;
                typ2.ElementName = xElement.Name.LocalName ;
                var kinds1 = xElement.Elements ( XName.Get ( "Kind" ) ) ;
                var xElements = kinds1 as XElement[] ?? kinds1.ToArray ( ) ;
                if ( xElements.Any ( ) )
                {
                    typ2.Kinds.Clear ( ) ;
                    var nodekinds = xElements
                                   .Select ( element => element.Attribute ( "Name" )?.Value )
                                   .ToList ( ) ;
                    foreach ( var nodekind in nodekinds )
                    {
                        typ2.Kinds.Add ( nodekind ) ;
                    }

                    DebugUtils.WriteLine ( typ2.Title ) ;
                }

                // ReSharper disable once UnusedVariable
                var comment = xElement.Element ( XName.Get ( "TypeComment" ) ) ;
                //DebugUtils.WriteLine ( comment ) ;

                typ2.Fields.Clear ( ) ;
                var choices = xElement.Elements ( XName.Get ( "Choice" ) ) ;
                var choiceAry = choices as XElement[] ?? choices.ToArray ( ) ;
                if ( choiceAry.Any ( ) )
                {
                    var choice = choiceAry.First ( ) ;
                    // ReSharper disable once UnusedVariable
                    foreach ( var element in choice.Elements ( ) )
                    {
                    }

                    // ReSharper disable once UnusedVariable
                    foreach ( var element in choice.Elements ( XName.Get ( "Field" ) ) )
                    {
                    }

                    DebugUtils.WriteLine ( typ2.Title ) ;
                }

                foreach ( var field in xElement.Elements ( XName.Get ( "Field" ) )
                                               .Concat (
                                                        xElement.Elements ( XName.Get ( "Choice" ) )
                                                                .Elements ( XName.Get ( "Field" ) )
                                                       ) )
                {
                    ParseField ( field , typ2 ) ;
                }
            }
        }

        private static void ParseField ( [ NotNull ] XElement field , [ NotNull ] AppTypeInfo typ2 )
        {
            var fieldName = field.Attribute ( XName.Get ( "Name" ) )?.Value ;
            var fieldType = field.Attribute ( XName.Get ( "Type" ) )?.Value ;
            var @override = field.Attribute ( XName.Get ( "Override" ) )?.Value == "true" ;
            var optional = field.Attribute ( XName.Get ( "Optional" ) )?.Value  == "true" ;

            var kinds = field.Elements ( "Kind" )
                             .Select ( element => element.Attribute ( "Name" )?.Value )
                             .ToList ( ) ;
            if ( kinds.Any ( ) )
            {
                //DebugUtils.WriteLine(string.Join(", ", kinds));
            }

            //DebugUtils.WriteLine ($"{typ2.Title}: {fieldName}: {fieldType} = {string.Join(", ", kinds)}"  );
            typ2.Fields.Add (
                             new SyntaxFieldInfo ( fieldName , fieldType , kinds.ToArray ( ) )
                             {
                                 Override = @override , Optional = optional
                             }
                            ) ;
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

        private static void Init ( ) { }

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
}