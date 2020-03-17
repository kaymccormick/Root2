﻿// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// Tests
// ResourceUiTest.cs
// 
// 2020-03-11-4:43 PM
// 
// ---

using System ;
using System.Collections.Generic ;
using System.Threading ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Automation ;
using System.Windows.Markup ;
using KayMcCormick.Dev.TestLib ;
using KayMcCormick.Dev.TestLib.Fixtures ;
using KayMcCormick.Lib.Wpf ;
using NLog ;
using Tests.Lib.Utils ;
using WpfApp ;
using WpfApp.Application ;
using WpfApp.Controls ;
using Xunit ;
using Xunit.Abstractions ;

namespace Tests.Main.UI
{
    /// <summary>
    ///     Test class for tests of <see cref="TypeControl" />
    /// </summary>
    [ LogTestMethod ] [ BeforeAfterLogger ]
    public sealed class ResourceUiTest : IClassFixture < LoggingFixture > , IDisposable
    {
        private const    string         TypeControlName = "typeControl" ;
        private readonly LoggingFixture _loggingFixture ;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        /// <summary>Constructor for test class</summary>
        /// <param name="output"></param>
        /// <param name="loggingFixture"></param>
        public ResourceUiTest ( ITestOutputHelper output , LoggingFixture loggingFixture )
        {
            _loggingFixture = loggingFixture ;
            loggingFixture.SetOutputHelper ( output ) ;
        }


        /// <summary>
        ///     Test Type Control
        /// </summary>
        /// <exception cref="AggregateException"></exception>
        // [ WpfFact ]
        [ Trait ( "UITest" , "true" ) ]

        [ WpfFact ]
        public void ValueContentCellTest ( )
        {
            App app = new App ( null ) ;
            app.InitializeComponent();

            AllResourcesTree tree = new AllResourcesTree(new AllResourcesTreeViewModel());
            tree.BeginInit();
            tree.EndInit();
            Window w = new Window ( ) ;
            w.Content = tree ;
            w.ShowDialog ( ) ;
        }
    

    /// <summary>Tests the type navigator.</summary>
        /// <exception cref="Exception"></exception>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for TestTypeNavigator
        // [ WpfFact ]
        [ Trait ( "UITest" , "true" ) ]
        private void TestTypeNavigator ( )
        {

            var controlName = SetupTypeNavControl ( out var control ) ;
            var window = MakeWindow ( control , out var r ) ;
            window.Show ( ) ;
            Assert.NotNull ( r.Task.Result ) ;
            if ( r.Task.IsFaulted )
            {
                if ( r.Task.Exception != null )
                {
                    throw r.Task.Exception ;
                }

                throw new Exception ( ) ;
            }


            var autoElem = FindControlAutomationElement ( controlName ) ;
            Assert.NotNull ( autoElem ) ;

            var hyperlinks = FindHyperlinks ( autoElem ) ;

            Automation.AddStructureChangedEventHandler (
                                                        autoElem
                                                      , TreeScope.Descendants
                                                      , StructureChangedEventHandler
                                                       ) ;


            Assert.NotEmpty ( hyperlinks ) ;
            foreach ( AutomationElement hyperlink in hyperlinks )
            {
                // ReSharper disable once UnusedVariable
                var linkText =
                    hyperlink.GetCurrentPropertyValue ( AutomationElement.NameProperty ) ;
                if ( DoInvokeHyperlink ( hyperlink ) )
                {
                    break ;
                }
            }
        }

        /// <summary>Does the invoke hyperlink.</summary>
        /// <param name="hyperlink">The hyperlink.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DoInvokeHyperlink
        private bool DoInvokeHyperlink ( AutomationElement hyperlink )
        {
            if ( hyperlink.TryGetCurrentPattern ( InvokePattern.Pattern , out var patternObject ) )
            {
                if ( patternObject is InvokePattern invoke )
                {
                    Automation.AddAutomationPropertyChangedEventHandler (
                                                                         hyperlink
                                                                       , TreeScope.Element
                                                                       , ( sender , args ) => {
                                                                             Logger.Debug(
                                                                                          "update: "
                                                                                          + args
                                                                                           .Property
                                                                                           .ProgrammaticName
                                                                                          + " = "
                                                                                          + args
                                                                                             .NewValue
                                                                                         ) ;
                                                                         }
                                                                       , hyperlink
                                                                            .GetSupportedProperties ( )
                                                                        ) ;

                    Logger.Debug( "yay" ) ;
                    invoke.Invoke ( ) ;
                    Thread.Sleep ( 2000 ) ;
                    return true ;
                }
            }

            return false ;
        }

        // ReSharper disable once InternalOrPrivateMemberNotDocumented
        private void StructureChangedEventHandler ( object sender , StructureChangedEventArgs args )
        {
            Logger.Debug( $"structure: {args.StructureChangeType}" ) ;
        }


        // ReSharper disable once InternalOrPrivateMemberNotDocumented
        private AutomationElementCollection FindHyperlinks ( AutomationElement autoElem )
        {
            Logger.Debug( "About to find hyperlinks" ) ;

            var hyperlinks = autoElem.FindAll (
                                               TreeScope.Descendants
                                             , new PropertyCondition (
                                                                      AutomationElement
                                                                         .ClassNameProperty
                                                                    , "Hyperlink"
                                                                     )
                                              ) ;
            Logger.Debug( $"Found {hyperlinks.Count} hyperlinks" ) ;
            return hyperlinks ;
        }

        // ReSharper disable once InternalOrPrivateMemberNotDocumented
        private AutomationElement FindControlAutomationElement ( string controlName )
        {
            Logger.Debug( "About to find automation element" ) ;
            var first = AutomationElement.RootElement.FindFirst (
                                                                 TreeScope.Children
                                                               , new PropertyCondition (
                                                                                        AutomationElement
                                                                                           .AutomationIdProperty
                                                                                      , "MYWin"
                                                                                       )
                                                                ) ;
            Logger.Debug( "Found automation element " + first ) ;
            Assert.NotNull ( first ) ;

            Logger.Debug(
                         "Trying to find control with Automation ID property " + controlName
                        ) ;
            var autoElem = first.FindFirst (
                                            TreeScope.Descendants
                                          , new PropertyCondition (
                                                                   AutomationElement
                                                                      .AutomationIdProperty
                                                                 , controlName
                                                                  )
                                           ) ;
            Logger.Debug( "Found automation element " + autoElem ) ;
            return autoElem ;
        }

        // ReSharper disable once InternalOrPrivateMemberNotDocumented
        private Window MakeWindow (
            UIElement                           control
          , out TaskCompletionSource < Result > taskCompletionSource
        )
        {
            var window = new Window { Name = "MYWin" , Content = control } ;
            taskCompletionSource = new TaskCompletionSource < Result > ( ) ;

            var source = taskCompletionSource ;
            window.Loaded += ( sender , args ) => {
                try
                {
                    Logger.Debug( "Window loaded." ) ;
                    throw new TestException ( ) ;
                }
                catch ( Exception ex )
                {
                    Logger.Debug( $"Exception: {ex.Message}." ) ;
                    source.TrySetException ( ex ) ;
                }
            } ;
            return window ;
        }

        // ReSharper disable once InternalOrPrivateMemberNotDocumented
        private string SetupTypeNavControl ( out TypeNavigator control )
        {
            const string controlName = "typeNav" ;
            control = new TypeNavigator { Name = controlName } ;
            control.SetValue (
                              Props.RenderedTypeProperty
                            , typeof ( Dictionary < string , List < Tuple < int , object > > > )
                             ) ;
            return controlName ;
        }

        // ReSharper disable once InternalOrPrivateMemberNotDocumented
        private string SetupTypeControl ( out TypeControl control )
        {
            const string controlName = TypeControlName;
            control = new TypeControl { Name = controlName } ;
            control.SetValue (
                              Props.RenderedTypeProperty
                            , typeof ( Dictionary < string , List < Tuple < int , object > > > )
                             ) ;
            return controlName ;
        }

        // ReSharper disable once InternalOrPrivateMemberNotDocumented

        // ReSharper disable once InternalOrPrivateMemberNotDocumented
        // ReSharper disable once UnusedMember.Local
        private void WalkContentElements ( AutomationElement autoElem , bool b )
        {
            var elementNode = TreeWalker.ContentViewWalker.GetFirstChild ( autoElem ) ;

            while ( elementNode != null )
            {
                if ( b )
                {
                    foreach ( var automationProperty in elementNode.GetSupportedProperties ( ) )
                    {
                        Logger.Debug(
                                     $"{automationProperty.ProgrammaticName}: {elementNode.GetCurrentPropertyValue ( automationProperty )}"
                                    ) ;
                    }
                }

                try
                {
                    var automationId =
                        elementNode.GetCurrentPropertyValue (
                                                             AutomationElement.AutomationIdProperty
                                                            ) ;
                    Logger.Debug( automationId?.ToString ( ) ) ;
                }
                catch ( Exception ex )
                {
                    Logger.Debug( ex.Message ) ;
                }

                WalkContentElements ( elementNode , b ) ;
                elementNode = TreeWalker.ContentViewWalker.GetNextSibling ( elementNode ) ;
            }
        }


        // ReSharper disable once InternalOrPrivateMemberNotDocumented
        // ReSharper disable once UnusedMember.Local
        private void WalkControlElements ( AutomationElement rootElement , bool dumpProps )
        {
            // Conditions for the basic views of the subtree (content, control, and raw) 
            // are available as fields of TreeWalker, and one of these is used in the 
            // following code.
            var elementNode = TreeWalker.ControlViewWalker.GetFirstChild ( rootElement ) ;

            while ( elementNode != null )
            {
                if ( dumpProps )
                {
                    foreach ( var automationProperty in elementNode.GetSupportedProperties ( ) )
                    {
                        Logger.Debug(
                                     $"{automationProperty.ProgrammaticName}: {elementNode.GetCurrentPropertyValue ( automationProperty )}"
                                    ) ;
                    }
                }

                try
                {
                    var automationId =
                        elementNode.GetCurrentPropertyValue (
                                                             AutomationElement.AutomationIdProperty
                                                            ) ;
                    Logger.Debug( automationId?.ToString ( ) ) ;
                }
                catch ( Exception ex )
                {
                    Logger.Debug( ex.Message ) ;
                }

                WalkControlElements ( elementNode , dumpProps ) ;
                elementNode = TreeWalker.ControlViewWalker.GetNextSibling ( elementNode ) ;
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose ( )
        {
            _loggingFixture?.Dispose ( ) ;
        }
    }
}