﻿using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Collections.Immutable ;
using System.ComponentModel ;
using System.Data ;
using System.Data.SqlClient ;
using System.Data.SqlTypes ;
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

using FindLogUsages ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Application ;
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
    internal sealed class AppContext

    {
#if TERMUI
        private readonly TermUi _termUi ;
        public TermUi Ui { get { return _termUi ; } }
        public AppContext (
            ILifetimeScope           scope
      , IProjectBrowserViewModel projectBrowserViewModel
      , TermUi                   termUi
        )
        {
            _termUi          = termUi ;
            Scope            = scope ;
            BrowserViewModel = projectBrowserViewModel ;
        }
#else
        public AppContext(
            ILifetimeScope           scope
          , IProjectBrowserViewModel projectBrowserViewModel
          
        )
        {
            
            Scope            = scope;
            BrowserViewModel = projectBrowserViewModel;
        }


#endif

        private IProjectBrowserViewModel _projectBrowserViewModel ;

        //public IEnumerable < Meta < Lazy < IAnalyzeCommand2 > > > AnalyzeCommands { get ; }

        public ILifetimeScope Scope { get ; }

        public IProjectBrowserViewModel BrowserViewModel
        {
            // ReSharper disable once UnusedMember.Global
            get { return _projectBrowserViewModel ; }
            set { _projectBrowserViewModel = value ; }
        }


    }

    internal sealed class AppModule : Module
    {
        // ReSharper disable once AnnotateNotNullParameter
        protected override void Load ( ContainerBuilder builder )
        {
            base.Load ( builder ) ;
            var actionBlock = new ActionBlock < ILogInvocation > (Action) ;
            builder.RegisterInstance ( actionBlock )
                   .As < ActionBlock < ILogInvocation > > ( )
                   .SingleInstance ( ) ;
            builder.RegisterType < AppContext > ( ).AsSelf ( ) ;
#if TERMUI
            builder.RegisterType < TermUi > ( ).AsSelf ( ) ;
#endif
        }

        private void Action ( ILogInvocation obj ) { }
    }

    internal static class Program
    {
        private const string ModelXamlFilename = @"C:\data\logs\model.xaml";
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
                                                        };
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
        private static async Task < int > Main ( string[] args)
        {
            var ConsoleAnalysisProgramGuid = ApplicationInstanceIds.ConsoleAnalysisProgramGuid ;
            var subject = new Subject < ILogger > ( ) ;
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
                    context = scope.Resolve < AppContext > () ;
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
            SelectVsInstance ( ) ;

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

            var options = context.Scope.Resolve < JsonSerializerOptions > ( ) ;
            // options.Converters.Add (new JsonPocoSyntaxConverter()  );
            // await            CollectDocs ( options ) ;
            // return 1 ;
            var typesViewModel = context.Scope.Resolve < TypesViewModel > ( ) ;
            WriteThisTypesViewModel(typesViewModel);

            LoadSyntax(typesViewModel);
            foreach ( AppTypeInfo ati in typesViewModel.Map.Values )
            {
                typesViewModel.PopulateFieldTypes ( ati ) ;
            }
            WriteThisTypesViewModel(typesViewModel);
            typesViewModel.DetailFields();
            WriteThisTypesViewModel(typesViewModel);
            DumpModelToJson( context , typesViewModel ) ;
            // return 1 ;
            //var json = JsonSerializer.Serialize ( typesViewModel, jsonSerializerOptions ) ;
//             File.WriteAllText ( @"C:\data\logs\model.json" , json ) ;
            //await CodeGenAsync ( typesViewModel ) ;
            return 1 ;
//            RunConsoleUi (context ) ;

            //
            // foreach ( var projFile in Directory.EnumerateFiles (
            //                                                     @"e:\kay2020\source"
            //                                                   , "*.csproj"
            //                                                   , SearchOption.AllDirectories
            //                                                    ) )
            // {
            //     //using Microsoft.CodeAnalysis.MSBuild;
            //
            //     Console.WriteLine ( projFile ) ;
            // }

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
                ArrayList l = null ;
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
                reader1.MoveToContent ( ) ;
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

            var bb2 = new SqlCommand (
                                      "insert into syntaxexample2 (example, syntaxkind, typename, tokens) values (@example, @kind, @typename, @tokens)"
                                    , c
                                     ) ;


            var set = new HashSet < Type > ( ) ;
            var model = context.Scope.Resolve < TypesViewModel > ( ) ;
            NewMethod ( model.Root.SubTypeInfos , set ) ;
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
                Dictionary <SyntaxKind, SyntaxInfo> triviaDict = new Dictionary < SyntaxKind , SyntaxInfo > ();
                foreach ( var syntaxTrivia in comp.DescendantTrivia ( x => true , true ) )
                {
                    var kind = syntaxTrivia.Kind ( ) ;
                    if (! triviaDict.TryGetValue ( kind , out var info ) )
                    {
                        info = new SyntaxInfo ( ) { Kind = kind } ;
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
                return 1 ;
                foreach ( var o1 in comp
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
                            Debug.WriteLine ( ex.ToString ( ) ) ;
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

        private static void DumpModelToJson ( AppContext context , TypesViewModel typesViewModel )
        {
            using ( var utf8Json = File.Open ( @"C:\temp\out.json" , FileMode.Create ) )
            {
                List < AppTypeInfo > infos = typesViewModel.Map.Values.Cast < AppTypeInfo > ( ).ToList ( ) ;
                Utf8JsonWriter writer = new Utf8JsonWriter (
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
                catch ( Exception ex )
                {
                }

                writer.Flush ( ) ;
            }
        }

        private static void SelectVsInstance ( )
        {
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
            var visualStudioInstances = vsInstances as VisualStudioInstance[] ?? vsInstances.ToArray ( ) ;

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
#endif
        }

        private static void WriteTypesViewModel ( )
        {
            var model = new TypesViewModel ( ) ;
            model.BeginInit ( ) ;
            model.EndInit ( ) ;
            WriteThisTypesViewModel ( model ) ;
        }

        private static void WriteThisTypesViewModel ( TypesViewModel model )
        {
            var writer = XmlWriter.Create (
                                           ModelXamlFilename
                                         , new XmlWriterSettings { Indent = true , Async = true }
                                          ) ;
            XamlWriter.Save ( model , writer ) ;
            writer.Close ( ) ;
        }

        // ReSharper disable once UnusedMember.Local
        private static async Task CollectDocs (JsonSerializerOptions options )
        {
            options.WriteIndented = true ;
            var workspace = MSBuildWorkspace.Create ( ) ;
            var solution = await workspace.OpenSolutionAsync (
                                                              @"C:\Users\mccor.LAPTOP-T6T0BN1K\source\repos\v3\NewRoot\src\KayMcCormick.Dev\ManagedProd.sln"
                                                             ) ;
            foreach ( var project in solution.Projects )
            {
                Console.WriteLine ( project.Name ) ;


                var compilation = await project.GetCompilationAsync ( ) ;
                foreach ( var doc in project.Documents )
                {
                    var model = await doc.GetSemanticModelAsync ( ) ;
                    Console.WriteLine ( doc.Name ) ;
                    var tree = await doc.GetSyntaxRootAsync ( ) ;
                    var gen = FindLogUsages.GenTransforms.Transform_CSharp_Node ( ( CSharpSyntaxNode ) tree ) ;

                    Debug.WriteLine ( JsonSerializer.Serialize ( gen , options ) ) ;
                    continue ;
                    foreach ( var node in tree.DescendantNodesAndSelf ( )
                                              .Where ( node => node.HasStructuredTrivia ) )
                    {
                        var nn = node.GetLeadingTrivia ( )
                                     .Where (
                                             triv => triv.Kind ( )
                                                     == SyntaxKind.SingleLineDocumentationCommentTrivia
                                            )
                                     .ToList ( ) ;
                        if ( nn.Count > 1 )
                        {
                            throw new InvalidOperationException ( ) ;
                        }

                        if ( ! nn.Any ( ) )
                        {
                            continue ;
                        }

                        try
                        {
                            CodeElementDocumentation xdoc = null ;
                            var declaredSymbol = model.GetDeclaredSymbol ( node ) ;
                            if ( declaredSymbol == null )
                            {
                                Console.WriteLine ( node.Kind ( ) ) ;
                                continue ;
                            }

                            var declarationId =
                                DocumentationCommentId.CreateDeclarationId ( declaredSymbol ) ;
                            var coll = new XmlDocumentElementCollection ( ) ;
                            switch ( node )
                            {
                                case MethodDeclarationSyntax m :
                                    xdoc = new MethodDocumentation (
                                                                    declarationId
                                                                  , null
                                                                  , m.Identifier.Text
                                                                  , null
                                                                   ) ;
                                    break ;
                                case ClassDeclarationSyntax cl :
                                case InterfaceDeclarationSyntax idcl :
                                case EnumDeclarationSyntax eds :
                                    xdoc = new TypeDocumentation ( declarationId , null ) ;
                                    break ;
                                case DelegateDeclarationSyntax dds :
                                    xdoc = new DelegateDocumentation ( declarationId ) ;
                                    break ;
                                case PropertyDeclarationSyntax pp :
                                    xdoc = new PropertyDocumentation (
                                                                      declarationId
                                                                    , null
                                                                    , pp.Identifier.Text
                                                                     ) ;
                                    break ;
                                case ConstructorDeclarationSyntax cds :
                                    xdoc = new ConstructorDocumentation ( declarationId ) ;
                                    break ;
                                case EnumMemberDeclarationSyntax ems :
                                    xdoc = new EnumMemberDocumentation ( declarationId ) ;
                                    break ;
                                case EventDeclarationSyntax eds :
                                    xdoc = new EventDocumentation ( declarationId ) ;
                                    break ;
                                case IndexerDeclarationSyntax ids :
                                    xdoc = new IndexerDocumentation ( declarationId ) ;
                                    break ;
                                default :
                                    throw new InvalidOperationException ( node.Kind ( ).ToString ( ) ) ;
                            }

                            foreach ( var syntaxTrivia in nn )
                            {
                                var structure = syntaxTrivia.GetStructure ( ) ;
                                var x = ( DocumentationCommentTriviaSyntax ) structure ;
                                foreach ( var xml in x.Content )
                                {
                                    XmlDocElement elem = null ;
                                    //Console.WriteLine ( xml.GetType ( ).Name ) ;
                                    switch ( xml )
                                    {
                                        case XmlCDataSectionSyntax xmlCDataSectionSyntax : break ;
                                        case XmlCommentSyntax xmlCommentSyntax :           break ;
                                        case XmlElementSyntax xmlElementSyntax :
                                            var xdoc1 = XDocument.Parse ( xmlElementSyntax.ToString ( ) ) ;
                                            elem = XmlDocElements.HandleXDocument ( xdoc1 ) ;

                                            break ;
                                        case XmlEmptyElementSyntax xmlEmptyElementSyntax : break ;
                                        case XmlProcessingInstructionSyntax xmlProcessingInstructionSyntax :
                                            break ;
                                        case XmlTextSyntax xmlTextSyntax :
                                            elem = new XmlDocText ( xmlTextSyntax.ToFullString ( ) ) ;
                                            break ;
                                    }

                                    if ( elem != null )
                                    {
                                        xdoc.XmlDoc.Add ( elem ) ;
                                    }
                                }

                                Console.WriteLine ( XamlWriter.Save ( xdoc ) ) ;
                            }
                        }
                        catch ( Exception exx )
                        {
                        }
                    }
                }
            }
        }

        private static void RunConsoleUi ( [ JetBrains.Annotations.NotNull ] AppContext context )
        {
#if TERMUI
            context.Ui.Init ( ) ;
            context.Ui.Run ( ) ;
#endif
        }

        // ReSharper disable once UnusedMember.Local
        private static async Task CodeGenAsync ( [ JetBrains.Annotations.NotNull ] TypesViewModel model1 )
        {
            var types = new SyntaxList < MemberDeclarationSyntax > ( ) ;//new [] { SyntaxFactory.ClassDeclaration("SyntaxToken")} ) ;
            foreach ( Type mapKey in model1.Map.Keys )
            {
                Logger.Debug ($"{mapKey}");
                var t = ( AppTypeInfo ) model1.Map[ mapKey ] ;
                var members = new SyntaxList < MemberDeclarationSyntax > ( ) ;
                foreach ( SyntaxFieldInfo tField in t.Fields )
                {
                    if ( tField.Type == null && tField.TypeName != "bool")
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
                    else if (tFieldTypeName.StartsWith("SyntaxList<"))
                    {
                        tFieldTypeName = tFieldTypeName.Replace("SyntaxList<", "List<");
                    }
                 else if (tFieldTypeName.StartsWith("SeparatedSyntaxList<"))
                {
                    tFieldTypeName = tFieldTypeName.Replace("SeparatedSyntaxList<", "List<");
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
                         SimpleNameSyntax ss = ( SimpleNameSyntax ) gen.TypeArgumentList.Arguments[ 0 ] ;
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
                    else if(tField.TypeName != "bool")
                    {
                        typeSyntax =
                            SyntaxFactory.ParseTypeName (
                                                         "Poco"
                                                         + ( ( SimpleNameSyntax ) typeSyntax )
                                                          .Identifier.Text
                                                        ) ;
                    }
                    List <SyntaxToken> tokens = new List < SyntaxToken > ();
                    tokens.Add (SyntaxFactory.Token(SyntaxKind.PublicKeyword));
                    if ( tField.Override )
                    {
                        tokens.Add (SyntaxFactory.Token(SyntaxKind.OverrideKeyword));
                    }
                    else
                    {
                        tokens.Add(SyntaxFactory.Token(SyntaxKind.VirtualKeyword));
                    }

                    
                    members = members.Add (
                                           SyntaxFactory.PropertyDeclaration (
                                                                              new SyntaxList <
                                                                                  AttributeListSyntax
                                                                              > ( )
                                                                            , SyntaxFactory.TokenList(tokens.ToArray()) 
                                                                            , typeSyntax
                                                                            , null
                                                                            , SyntaxFactory
                                                                                 .Identifier (
                                                                                              tField
                                                                                                 .Name
                                                                                             )
                                                                            , accessorListSyntax
                                                                             )
                                          ) ;
                }

                var classDecl =
                    SyntaxFactory.ClassDeclaration ( "Poco" + mapKey.Name ).WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token (SyntaxKind.PublicKeyword))).WithMembers ( members ) ;
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
                                                                                                                      .IdentifierName ("Poco" + 
                                                                                                                                       t.ParentInfo
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

            var compl = SyntaxFactory.CompilationUnit ( )
                                     .WithUsings (
                                                  new SyntaxList < UsingDirectiveSyntax > (new [] {
                                                                                           SyntaxFactory
                                                                                              .UsingDirective (
                                                                                                               SyntaxFactory
                                                                                                                  .ParseName (
                                                                                                                              "System"
                                                                                                                             )
                                                                                                              ),
                                                                                           SyntaxFactory
                                                                                              .UsingDirective(
                                                                                                              SyntaxFactory
                                                                                                                 .ParseName(
                                                                                                                            "System.Collections.Generic"
                                                                                                                           )
                                                                                                             )}
                                                                                          )
                                                 )
                                     .WithMembers (new SyntaxList<MemberDeclarationSyntax>(SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("PocoSyntax")).WithMembers(types) ))
                                     .NormalizeWhitespace ( ) ;
            var tree = SyntaxFactory.SyntaxTree ( compl ) ;
            var src = tree.ToString ( ) ;
            File.WriteAllText ( @"C:\data\logs\stuff.cs" , src ) ;
            var tree2 = CSharpSyntaxTree.ParseText ( SourceText.From ( src ) ) ;
            //refs, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, false, "test", null, null, null, OptimizationLevel.Debug, false, false, null, null, default, default ,default, default, default,default, default, default, default, default, new MetadataReferenceResolver())
            var source = tree2.ToString ( ).Split ( new[] { "\r\n" } , StringSplitOptions.None ) ;
            //var compilation = CSharpCompilation.Create ( "test" , new[] { tree2 } ) ;
            var adhoc = new AdhocWorkspace ( ) ;

            var projectId = ProjectId.CreateNewId ( ) ;
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
            
            var document2 = DocumentInfo.Create(DocumentId.CreateNewId(projectId), "misc", null, SourceCodeKind.Regular, TextLoader.From (TextAndVersion.Create(SourceText.From(
                @"public class PocoSyntaxToken { public int RawKind { get; set; } public string Kind { get; set; } public object Value {get; set;} public string ValueText { get; set; } }"), VersionStamp.Create())));
                
            var s2 = s.AddDocuments ( ImmutableArray < DocumentInfo >.Empty.Add ( documentInfo ).Add (document2  ) ) ;

            //var d = project.AddDocument ( "test.cs" , src ) ;
            var rb1 = adhoc.TryApplyChanges ( s2 ) ;
            if ( ! rb1 )
            {
                throw new InvalidOperationException ( ) ;
            }

            foreach (var ref1 in AssemblyRefs)
            {
                var s3 = adhoc.CurrentSolution.AddMetadataReference(
                                                                        projectId
                                                                      , MetadataReference
                                                                           .CreateFromFile(ref1)
                                                                       );
                var rb = adhoc.TryApplyChanges(s3);
                if (!rb)
                {
                    throw new InvalidOperationException();
                }
            }

            
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
                            var line =
                                source[ diagnostic
                                       .Location.GetLineSpan ( )
                                       .StartLinePosition.Line ] ;
                            Console.WriteLine ( diagnostic.ToString ( ) ) ;
                            f.WriteLine ( diagnostic.ToString ( ) ) ;
                        }
                    }
                }
            }

            var result =compilation.Emit ( @"C:\data\logs\output.dll" ) ;
            if (result.Success)
            {
                Console.WriteLine("Success");
            }
            
            File.WriteAllText ( @"C:\data\logs\gen.cs" , compl.ToString ( ) ) ;
            
        }

        // ReSharper disable once UnusedMember.Local
        private static void LoadSyntax ( TypesViewModel model1 )
        {
            var syntax = XDocument.Parse ( Resources.Syntax ) ;
            if ( syntax.Root != null )
            {
                foreach ( var xElement in syntax.Root.Elements ( ) )
                {
                    switch ( xElement.Name.LocalName )
                    {
                        case "PredefinedNode" :
                            var typeName = xElement.Attribute ( XName.Get ( "Name" ) ).Value ;
                            typeName = $"Microsoft.CodeAnalysis.CSharp.Syntax.{typeName}" ;
                            var t = typeof ( CSharpSyntaxNode ).Assembly.GetType ( typeName ) ;
                            if ( t == null )
                            {
                                Debug.WriteLine ( "No type for " + typeName ) ;
                            }
                            else if ( model1.Map.Contains ( t ) )
                            {
                                var typ = ( AppTypeInfo ) model1.Map[ t ] ;
                                typ.ElementName = xElement.Name.LocalName ;
                            }

                            break ;
                        case "AbstractNode" :
                            ParseNodeBasics(model1, xElement);
                            break ;
                        case "Node" :
                            ParseNodeBasics ( model1 , xElement ) ;

                            break ;


                        default : throw new InvalidOperationException ( ) ;
                    }
                }
            }

            var d = new TypeMapDictionary { [ typeof ( string ) ] = new AppTypeInfo ( ) } ;
            Debug.WriteLine ( XamlWriter.Save ( d ) ) ;

            Debug.WriteLine ( XamlWriter.Save ( model1 ) ) ;
        }

        private static void ParseNodeBasics ( TypesViewModel model1 , XElement xElement )
        {
            var typeName2 = xElement.Attribute ( XName.Get ( "Name" ) ).Value ;
            typeName2 = $"Microsoft.CodeAnalysis.CSharp.Syntax.{typeName2}" ;
            var t2 = typeof ( CSharpSyntaxNode ).Assembly.GetType ( typeName2 ) ;
            if ( t2 == null )
            {
                Debug.WriteLine ( "No type for " + typeName2 ) ;
            }
            else
            {
                var typ2 = ( AppTypeInfo ) model1.Map[ t2 ] ;
                typ2.ElementName = xElement.Name.LocalName ;
                var kinds1 = xElement.Elements ( XName.Get ( "Kind" ) ) ;
                var xElements = kinds1 as XElement[] ?? kinds1.ToArray ( ) ;
                if ( xElements.Any ( ) )
                {
                    typ2.Kinds.Clear();
                    var nodekinds = xElements
                                     .Select(element => element.Attribute("Name")?.Value)
                                     .ToList();
                    foreach ( var nodekind in nodekinds )
                    {
                        typ2.Kinds.Add ( nodekind ) ;
                    }
                    Debug.WriteLine ( typ2.Title ) ;
                    
                }
                var comment = xElement.Element ( XName.Get ( "TypeComment" ) ) ;
                //Debug.WriteLine ( comment ) ;
                var fields = new List < Tuple < string , string , List < string > > > ( ) ;
                typ2.Fields.Clear ( ) ;
                var choices = xElement.Elements ( XName.Get ( "Choice" ) ) ;
                if ( choices.Any ( ) )
                {

                    var choice = choices.First ( ) ;
                    foreach ( var element in choice.Elements ( ) )
                    {

                    }
                    foreach ( var element in choice.Elements ( XName.Get ( "Field" ) ) )
                    {

                    }
                    Debug.WriteLine ( typ2.Title ) ;
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
                //Debug.WriteLine(string.Join(", ", kinds));
            }

            //Debug.WriteLine ($"{typ2.Title}: {fieldName}: {fieldType} = {string.Join(", ", kinds)}"  );
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

        private static void NewMethod (
            [ NotNull ] AppTypeInfoCollection subTypeInfos
          , HashSet < Type >                  set
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

    internal sealed class SyntaxInfo
    {
        public SyntaxKind Kind { get ; set ; }

        public int Count { get ; set ; }
    }

    internal class MyVisitor : SymbolVisitor
    {
    }

    public sealed class ExampleTokens : IList , ICollection , IEnumerable
    {
        private readonly IList _listImplementation = new List < SToken > ( ) ;
#region Implementation of IEnumerable
        public IEnumerator GetEnumerator ( ) { return _listImplementation.GetEnumerator ( ) ; }
#endregion
#region Implementation of ICollection
        public void CopyTo ( Array array , int index )
        {
            _listImplementation.CopyTo ( array , index ) ;
        }

        public int Count { get { return _listImplementation.Count ; } }

        public object SyncRoot { get { return _listImplementation.SyncRoot ; } }

        public bool IsSynchronized { get { return _listImplementation.IsSynchronized ; } }
#endregion
#region Implementation of IList
        public int Add ( object value ) { return _listImplementation.Add ( value ) ; }

        public bool Contains ( object value ) { return _listImplementation.Contains ( value ) ; }

        public void Clear ( ) { _listImplementation.Clear ( ) ; }

        public int IndexOf ( object value ) { return _listImplementation.IndexOf ( value ) ; }

        public void Insert ( int index , object value )
        {
            _listImplementation.Insert ( index , value ) ;
        }

        public void Remove ( object value ) { _listImplementation.Remove ( value ) ; }

        public void RemoveAt ( int index ) { _listImplementation.RemoveAt ( index ) ; }

        public object this [ int index ]
        {
            get { return _listImplementation[ index ] ; }
            set { _listImplementation[ index ] = value ; }
        }

        public bool IsReadOnly { get { return _listImplementation.IsReadOnly ; } }

        public bool IsFixedSize { get { return _listImplementation.IsFixedSize ; } }
#endregion
    }

    public class ExampleDict : IDictionary , ICollection , IEnumerable
    {
        private readonly IDictionary _dictionaryImplementation =
            new Dictionary < SyntaxKind , ArrayList > ( ) ;

#region Implementation of IEnumerable
        public bool Contains ( object key ) { return _dictionaryImplementation.Contains ( key ) ; }

        public void Add ( object key , object value )
        {
            _dictionaryImplementation.Add ( key , value ) ;
        }

        public void Clear ( ) { _dictionaryImplementation.Clear ( ) ; }

        public IDictionaryEnumerator GetEnumerator ( )
        {
            return _dictionaryImplementation.GetEnumerator ( ) ;
        }

        public void Remove ( object key ) { _dictionaryImplementation.Remove ( key ) ; }

        public object this [ object key ]
        {
            get { return _dictionaryImplementation[ key ] ; }
            set { _dictionaryImplementation[ key ] = value ; }
        }

        public ICollection Keys { get { return _dictionaryImplementation.Keys ; } }

        public ICollection Values { get { return _dictionaryImplementation.Values ; } }

        public bool IsReadOnly { get { return _dictionaryImplementation.IsReadOnly ; } }

        public bool IsFixedSize { get { return _dictionaryImplementation.IsFixedSize ; } }

        IEnumerator IEnumerable.GetEnumerator ( )
        {
            return ( ( IEnumerable ) _dictionaryImplementation ).GetEnumerator ( ) ;
        }
#endregion
#region Implementation of ICollection
        public void CopyTo ( Array array , int index )
        {
            _dictionaryImplementation.CopyTo ( array , index ) ;
        }

        public int Count { get { return _dictionaryImplementation.Count ; } }

        public object SyncRoot { get { return _dictionaryImplementation.SyncRoot ; } }

        public bool IsSynchronized { get { return _dictionaryImplementation.IsSynchronized ; } }
#endregion
    }

    [ ContentProperty ( "Example" ) ]
    public class ExampleSyntax
    {
        private int    _kind ;
        private string _example ;
        private string _typeName ;

        private readonly ExampleTokens _tokens = new ExampleTokens ( ) ;
        private int           _id ;

        public ExampleSyntax (
            int             kind
          , string          example
          , string          typeName
          , [ NotNull ] List < SToken > tokens
          , int             id
        )
        {
            Kind     = kind ;
            Example  = example ;
            TypeName = typeName ;
            Id       = id ;
            foreach ( var sToken in tokens )
            {
                Tokens.Add ( sToken ) ;
            }
        }

        public int Kind { get { return _kind ; } set { _kind = value ; } }

        public string Example { get { return _example ; } set { _example = value ; } }

        public string TypeName { get { return _typeName ; } set { _typeName = value ; } }

        public int Id { get { return _id ; } set { _id = value ; } }

        [ DesignerSerializationVisibility ( DesignerSerializationVisibility.Content ) ]
        public ExampleTokens Tokens { get { return _tokens ; } }
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