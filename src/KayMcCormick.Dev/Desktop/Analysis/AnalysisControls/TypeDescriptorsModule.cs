using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using AnalysisControls.TypeDescriptors;
using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Core.Resolving;
using Autofac.Features.AttributeFilters;
using KayMcCormick.Dev.Container;
using KayMcCormick.Lib.Wpf;
using Microsoft.CodeAnalysis.CSharp;
using Container = Autofac.Core.Container;
using MethodInfo = AnalysisAppLib.MethodInfo;
using Module = Autofac.Module;

namespace AnalysisControls
{
    /// <summary>
    /// </summary>
    public sealed class TypeDescriptorsModule : Module
    {
        /// <summary>
        /// </summary>
        /// <param name="builder"></param>
        // ReSharper disable once AnnotateNotNullParameter
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<SyntaxNodeProperties>().AsImplementedInterfaces();
            builder.RegisterType<MiscInstanceInfoProvider>()
                .AsSelf()
                .As<TypeDescriptionProvider>()
                .WithCallerMetadata()
                .OnActivating(
                    args =>
                    {
                        foreach (var type in new[]
                        {
                            typeof(IComponentRegistration), typeof(ComponentRegistration)
                        })
                            TypeDescriptor.AddProvider(args.Instance, type);
                    }
                );


            var kayTypes = AppDomain.CurrentDomain.GetAssemblies()
                .Where(
                    a => a
                        .GetCustomAttributes<AssemblyCompanyAttribute
                        >()
                        .Any(tx => tx.Company == "Kay McCormick")
                )
                .SelectMany(az => az.GetTypes())
                .ToList();
            // foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            // {

            // if (assembly.IsDynamic)
            // continue;
            // foreach (var exportedType in assembly.GetExportedTypes())
            // {
            // if (typeof(IDictionary).IsAssignableFrom(exportedType) ||
            // typeof(IEnumerable).IsAssignableFrom(exportedType))
            // {
            // kayTypes.Add(exportedType);
            // DebugUtils.WriteLine(exportedType.FullName);
            // }
            // }
            // }
            kayTypes.Add(typeof(IInstanceLookup));
            kayTypes.Add(typeof(Container));
            kayTypes.Add(typeof(IResolveOperation));
            //kayTypes.Add(typeof(Type));
            kayTypes.Add(typeof(MethodInfo));
            kayTypes.Add(typeof(PropertyPath));
            kayTypes.Add(typeof(PropertyInfo));
            kayTypes.Add(typeof(MemberInfo));
            kayTypes.Add(typeof(FileInfo));
            kayTypes.Add(typeof(DirectoryInfo));
            var collection = typeof(CSharpSyntaxNode).Assembly.GetExportedTypes()
                .Where(t => typeof(CSharpSyntaxNode).IsAssignableFrom(t)).ToList();
            foreach (var type in collection)
            {
                //DebugUtils.WriteLine("Type enabled for custom descriptor " + type.FullName);
                kayTypes.Add(type);
            }

            kayTypes.Clear();
            var xx = new CustomTypes(kayTypes);
            builder.RegisterInstance(xx).OnActivating(args => { args.Instance.ComponentContext = args.Context; });
            builder.RegisterType<UiElementTypeConverter>().SingleInstance().WithCallerMetadata();

            builder.RegisterType<ControlsProvider>().WithAttributeFiltering().InstancePerLifetimeScope()
                .As<IControlsProvider>()
                .AsSelf();
            builder.RegisterType<AnalysisCustomTypeDescriptor>().AsSelf().AsImplementedInterfaces();
        }
    }
}