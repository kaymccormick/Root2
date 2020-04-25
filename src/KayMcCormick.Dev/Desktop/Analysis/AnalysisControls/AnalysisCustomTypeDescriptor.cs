using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Linq ;
using System.Text ;
using System.Threading.Tasks ;
using Autofac ;
using Autofac.Core ;
using Autofac.Features.AttributeFilters ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
using Microsoft.CodeAnalysis.Host ;

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

        public override bool IsSupportedType ( Type type ) { return Types.Contains ( type ) ; }
        #endregion
    }

    public class TypeConverter1 : TypeConverter
    {
        private readonly IComponentContext _context ;
        private          Type              _t ;

        public TypeConverter1 ( IComponentContext context , Type type )
        {
            _context = context ;
            _t       = type ;
            DebugUtils.WriteLine ( type.FullName ) ;
        }

        #region Overrides of TypeConverter
        public override StandardValuesCollection GetStandardValues (
            ITypeDescriptorContext context
        )
        {
            var examples =
                ( IEnumerable ) _context.Resolve (
                                                  typeof ( IEnumerable <> ).MakeGenericType (
                                                                                             new[]
                                                                                             {
                                                                                                 _t
                                                                                             }
                                                                                            )
                                                 ) ;
            var retur = new StandardValuesCollection ( new[] { examples } ) ;
            return retur ;
        }

        public override bool GetStandardValuesSupported ( ITypeDescriptorContext context )
        {
            return true ;
            return base.GetStandardValuesSupported ( context ) ;
        }
        #endregion
    }

    internal class context
    {
    }

    public class TypeProvider : TypeDescriptionProvider
    {
        private IEnumerable < ICustomTypeDescriptor > _supported ;

        public TypeProvider ( IEnumerable < ICustomTypeDescriptor > supported )
        {
            _supported = supported ;
        }

        #region Overrides of TypeDescriptionProvider
        public override ICustomTypeDescriptor GetTypeDescriptor (
            Type   objectType
          , object instance
        )
        {
            return base.GetTypeDescriptor ( objectType , instance ) ;
        }

        public override bool IsSupportedType ( Type type )
        {
            return base.IsSupportedType ( type ) ;
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class AnalysisCustomTypeDescriptor : CustomTypeDescriptor
    {
        private readonly Func < Type , TypeConverter > _funcConverter ;
        private readonly UiElementTypeConverter        _uiElementTypeConverter ;

        public Type Type { get ; }

        public AnalysisCustomTypeDescriptor (
            Func < Type , TypeConverter > funcConverter
          , UiElementTypeConverter        uiElementTypeConverter
          , Type                          type
        )
        {
            _funcConverter          = funcConverter ;
            _uiElementTypeConverter = uiElementTypeConverter ;
            Type                    = type ;
        }

        #region Overrides of CustomTypeDescriptor
        public override TypeConverter GetConverter ( ) { return _funcConverter ( Type ) ; }
        #endregion
    }
}