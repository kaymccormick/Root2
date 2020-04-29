using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Autofac;
using Autofac.Features.AttributeFilters;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class ControlsProvider : TypeDescriptionProvider
    {
        public IEnumerable < Type > Types { get ; }

        private readonly IComponentContext                     _contex1T ;
        private readonly Func < Type , ICustomTypeDescriptor > _customFunc ;
        private readonly AnalysisCustomTypeDescriptor          _customType ;

        public ControlsProvider (
            IComponentContext                     contex1t,
            [KeyFilter("custom")] IEnumerable < Type >                  types
            , Func < Type , ICustomTypeDescriptor > customFunc
        )
        {
            Types       = types ;
            _contex1T   = contex1t ;
            _customFunc = customFunc ;
        }

        #region Overrides of TypeDescriptionProvider
        public override ICustomTypeDescriptor GetTypeDescriptor (
            Type   objectType
            , object instance
        )
        {
            DebugUtils.WriteLine($"Type descriptor for {objectType.FullName} ({instance})");
            var customTypeDescriptor = _customFunc ( objectType ) ;
            return customTypeDescriptor ;
            var x = _contex1T.Resolve < ICustomTypeDescriptor > (
                new PositionalParameter (
                    0
                    , objectType
                )
            ) ;
            return x ;
        }

        public override bool IsSupportedType(Type type)
        {
            DebugUtils.WriteLine($"{nameof(IsSupportedType)}: {type.FullName}");
            return Types.Contains ( type ) ;
        }
        #endregion
    }
}