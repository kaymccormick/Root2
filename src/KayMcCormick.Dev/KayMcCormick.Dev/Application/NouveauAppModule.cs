using System.Reflection;
using Autofac;
using Autofac.Extras.AttributeMetadata;
using Autofac.Features.AttributeFilters;
using JetBrains.Annotations;
using KayMcCormick.Dev.Container;
using KayMcCormick.Dev.Logging;

namespace KayMcCormick.Dev.Application
{
    /// <summary>
    ///     New app module to replace crusty old app module. Work in progress.
    /// </summary>
    internal sealed class NouveauAppModule : IocModule
    {
        /// <summary>
        ///     Our fun custom load method that is public.
        /// </summary>
        /// <param name="builder"></param>
        public override void DoLoad([NotNull] ContainerBuilder builder)
        {

            var exAs = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(exAs)
                .AssignableTo<IViewModel>()
                .AsSelf()
                .AsImplementedInterfaces()
                .WithAttributedMetadata()
                .WithAttributeFiltering().WithCallerMetadata();
            if (AppLoggingConfigHelper.CacheTarget2 != null)
                builder.RegisterInstance(AppLoggingConfigHelper.CacheTarget2)
                    .WithMetadata("Description", "Cache target")
                    .SingleInstance().WithCallerMetadata();
         }
    }


}