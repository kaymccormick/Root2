using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Autofac;
using Autofac.Features.AttributeFilters;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class ControlsProvider : TypeDescriptionProvider, IControlsProvider
    {
        public IEnumerable < Type > Types { get ; }

        [Browsable(false)]
        public TypeDescriptionProvider Provider => this;

        private readonly IComponentContext                     _componentContext ;
        private readonly CustomTypes _customTypes;
        private readonly Func<Type, ICustomTypeDescriptor> _customFunc ;
        private ConcurrentDictionary<Type, ICustomTypeDescriptor> _cache = new ConcurrentDictionary<Type, ICustomTypeDescriptor>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentContext"></param>
        /// <param name="customTypes"></param>
        /// <param name="customFunc"></param>
        public ControlsProvider (
            IComponentContext                     componentContext,
            //[KeyFilter("custom")] IEnumerable < Type >                  types
            CustomTypes customTypes
            , Func<Type, ICustomTypeDescriptor> customFunc
        )
        {
            Types       = customTypes.CustomTypeList ;
            // DebugUtils.WriteLine($"{nameof(ControlsProvider)} providing custom type descriptors for {customTypes.CustomTypeList.Count()} types.");
            _componentContext   = componentContext ;
            _customTypes = customTypes;
            _customFunc = customFunc ;
        }

        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        {
            DebugUtils.WriteLine(nameof(CreateInstance));
            return base.CreateInstance(provider, objectType, argTypes, args);
        }

        public override IDictionary GetCache(object instance)
        {
            DebugUtils.WriteLine(nameof(GetCache));
            return base.GetCache(instance);
        }

        public override ICustomTypeDescriptor GetExtendedTypeDescriptor(object instance)
        {
            // DebugUtils.WriteLine(nameof(GetExtendedTypeDescriptor));
            return base.GetExtendedTypeDescriptor(instance);
        }

        protected override IExtenderProvider[] GetExtenderProviders(object instance)
        {
            DebugUtils.WriteLine(nameof(GetExtenderProviders));
            return base.GetExtenderProviders(instance);
        }

        public override string GetFullComponentName(object component)
        {
            DebugUtils.WriteLine(nameof(GetFullComponentName));
            return base.GetFullComponentName(component);
        }

        public override Type GetReflectionType(Type objectType, object instance)
        {
            // DebugUtils.WriteLine(nameof(GetReflectionType));
            return base.GetReflectionType(objectType, instance);
        }

        public override Type GetRuntimeType(Type reflectionType)
        {
            DebugUtils.WriteLine(nameof(GetRuntimeType));
            return base.GetRuntimeType(reflectionType);
        }

        #region Overrides of TypeDescriptionProvider

        /// <inheritdoc />
        public override ICustomTypeDescriptor GetTypeDescriptor (
            Type   objectType
            , object instance
        )
        {
            // DebugUtils.WriteLine($"{nameof(GetTypeDescriptor)} - {objectType.FullName} - {instance}");
            if (_cache.TryGetValue(objectType, out var desc))
            {
                // DebugUtils.WriteLine($"Found in cache for {objectType}");
                return desc;
            }
            // DebugUtils.WriteLine($"Type descriptor for {objectType.FullName} ({instance})");
            //TypeDescriptionProvider tyd = TypeDescriptor.GetProvider(objectType);
            //var parent = tyd.GetTypeDescriptor(objectType);
            var customTypeDescriptor = _customFunc ( objectType ) ;
            _cache.TryAdd(objectType, customTypeDescriptor);
            return customTypeDescriptor ;
        }

        /// <inheritdoc />
        public override bool IsSupportedType(Type type)
        {
            DebugUtils.WriteLine($"{nameof(IsSupportedType)}: {type.FullName}");
            return Types.Contains ( type ) ;
        }
        #endregion
    }
}