using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using AnalysisAppLib;
using AnalysisControls;
using Autofac;
using Autofac.Core.Lifetime;
using Autofac.Features.Metadata;
using KayMcCormick.Dev.Command;
using KayMcCormick.Dev.Container;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Lib.Wpf.Command;

namespace Client2
{
    internal class Client2Module :Module
    {
        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((c) =>
                {
                    var lifetimeScope = c.Resolve<ILifetimeScope>();
                    bool used = false;
                    if (lifetimeScope is LifetimeScope l2)
                    {
                        while (l2 != null)
                        {
                            if(l2.Tag == "Client2Window1")
                            {
                                used = true;
                            }
                            l2 = l2.ParentLifetimeScope as LifetimeScope;
                        }
                    }
                     
                    var ls = lifetimeScope.BeginLifetimeScope(used ? "Client2Window1_2" :  "Client2Window1");
                    return new Client2Window1(ls, ls.Resolve<ClientModel>(),
                        ls.ResolveOptional<MyCacheTarget2>());
                })
                .As<Window>().WithCallerMetadata();
            builder.RegisterAdapter<Meta<Lazy<Window>>, Meta<IDisplayableAppCommand>>((context, parameters, arg3) =>
            {
                var props = MetaHelper.GetMetadataProps(arg3.Metadata);
                return new Meta<IDisplayableAppCommand>(new LambdaAppCommand(
                    props.Title ?? props.TypeHint?.ToString() ?? "no title", (command, o) =>
                    {
                        var w = command.Argument as Lazy<Window>;

                        var ww = w.Value;
                        ww.Show();
                        return Task.FromResult(AppCommandResult.Success);
                    }, arg3.Value), new Dictionary<string, object>() { ["CallerFilePath"] = "Client2Module"});

            }).WithCallerMetadata();
        }
    }
}