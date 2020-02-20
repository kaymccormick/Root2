﻿#if false
using System;
using System.Diagnostics ;
using KayMcCormick.Test.Common.Fixtures ;
using NLog ;
using NLog.Layouts ;
using WpfApp.Proxy ;
using Xunit ;
using Xunit.Abstractions ;

namespace Tests.Main.Utils
{
    /// <summary>Tests for ProxyUtils</summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for TestsProxyUtils
    [Collection("GeneralPurpose")]
    public class TestsProxyUtils : IClassFixture <LoggingFixture>, IDisposable
    {
        private readonly LoggingFixture _loggingFixture ;

        /// <summary>Initializes a new instance of the <see cref="System.Object" /> class.</summary>
        public TestsProxyUtils (LoggingFixture loggingFixture, ITestOutputHelper helper )
        {
            _loggingFixture = loggingFixture ;
            _loggingFixture.SetOutputHelper(helper);
            _loggingFixture.Layout = Layout.FromString ( "${message}" ) ;
        }

        // ReSharper disable once InconsistentNaming
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private void WriteOut ( string s )
        {
            Logger.Info(s);
        }

        /// <summary>Test1s this instance.</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Test1
        [WpfFact]
        public void Test1()
        {
            var p = new ProxyUtils(WriteOut, ProxyUtilsBase.CreateInterceptor(WriteOut));
            var xamlSchemaContext = p.CreateXamlSchemaContext();
            Logger.Info("{schemaContext}", xamlSchemaContext);
            foreach (var ns in xamlSchemaContext.GetAllXamlNamespaces())
            {
                var allXamlTypes = xamlSchemaContext.GetAllXamlTypes(ns);
                Logger.Debug("{numtypes} {namespace}", allXamlTypes.Count, ns);

                // continue ;
                foreach (var t in allXamlTypes)
                {
                    if ( t.IsMarkupExtension )
                    {
                        Logger.Debug ( "{t}" , t ) ;
                    }
                }


            }
        }

        /// <summary>Test1s this instance.</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Test1
        [ WpfFact ]
        public void Test2 ( )
        {
            PresentationTraceSources.Refresh();
            var x1 = new NLogTraceListener { ForceLogLevel = LogLevel.Debug } ;
            PresentationTraceSources.MarkupSource.Listeners.Add ( x1 ) ;
            PresentationTraceSources.MarkupSource.Switch.Level = SourceLevels.All ;
            var x = new ProxyUtils(WriteOut, ProxyUtilsBase.CreateInterceptor(WriteOut));
            var @out = x.TransformXaml2 ( @"files/test.xaml" ) ;
            Logger.Debug ( "{out}" , @out ) ;

        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
            public void Dispose ( )
        {
            _loggingFixture.SetOutputHelper(null);
        }
    }
}
#endif
