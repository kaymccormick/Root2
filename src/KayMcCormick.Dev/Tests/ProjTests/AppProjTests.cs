using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using AnalysisAppLib;
using AnalysisAppLib.Syntax;
using AnalysisControls;
using AnalysisControls.ViewModel;
using Autofac;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Application;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;
using Xunit.Abstractions;

namespace ProjTests
{
    public class AppProjTests
    {
        private readonly ITestOutputHelper _helper;
        private ILifetimeScope _lifetimeScope;

        public AppProjTests(ITestOutputHelper helper)
        {
            _helper = helper;
            var instance = new ApplicationInstance(
                new ApplicationInstance.
                    ApplicationInstanceConfiguration(LogMethod
                        ,
                        ApplicationInstanceIds.ProjTests
                    )
            );
            
                instance.AddModule(new AnalysisControlsModule());
                instance.AddModule(new AnalysisAppLibModule());
                instance.Initialize();
                _lifetimeScope = instance.GetLifetimeScope();
        }

        private void LogMethod(string message)
        {
        }

        //[Fact]
        public void TestResourceNodeInfo1()
        {
            var subject = _lifetimeScope.Resolve<ReplaySubject<IHierarchicalNode>>();
            subject.Subscribe(hierarchicalNode =>
            {
                _helper.WriteLine(hierarchicalNode.ToString());
            });
            var func = _lifetimeScope.Resolve<Func<IHierarchicalNode>>();
            var node = func();
            subject.OnNext(node);

            
        }
    }
}
