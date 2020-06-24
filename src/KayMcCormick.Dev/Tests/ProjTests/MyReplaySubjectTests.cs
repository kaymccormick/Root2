using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Core.Lifetime;
using KmDevLib;
using Microsoft.Graph;
using Xunit;

namespace ProjTestsCore
{
    public class MyReplaySubjectTests
    {
        [WpfFact]
        public void TestLifetimeScope()
        {
            ContainerBuilder b = new ContainerBuilder();
            var m = new MM(new MySubjectReplaySubject());

            b.RegisterSource(m);
            var container = b.Build();

            var scope1 = container.BeginLifetimeScope("scope1");
            var src = scope1.Resolve<MyReplaySubject<int>>();
            var dst = scope1.Resolve<MyReplaySubject<int>>();
            src.Subject.OnNext(3);
            List<int> out1 = new List<int>();
            dst.Subject.Subscribe(i => out1.Add(i));
            Assert.Collection(out1, i =>
            {
                Assert.Equal(3, i);
            });

        }
        [WpfFact]
        public void TestLifetimeScope1()
        {
            ContainerBuilder b = new ContainerBuilder();
            var m = new MM(new MySubjectReplaySubject(), new RootScopeLifetime());

            b.RegisterSource(m);
            var container = b.Build();
            var scope1 = container.BeginLifetimeScope("scope1");
            var scope2 = container.BeginLifetimeScope("scope2");
            
            var src = scope1.Resolve<MyReplaySubject<int>>();
            var dst = scope1.Resolve<MyReplaySubject<int>>();
            var src2 = scope1.Resolve<MyReplaySubject<int>>();
            var dst2 = scope2.Resolve<MyReplaySubject<int>>();

            src.Subject.OnNext(3);
            List<int> out1 = new List<int>();
            dst.Subject.Subscribe(i => out1.Add(i));
            Assert.Collection(out1, i =>
            {
                Assert.Equal(3, i);
            });
            List<int> out2 = new List<int>();

            dst2.Subject.Subscribe(i => {out2.Add(i); });
            Assert.Empty(out2);


        }


    }
}
