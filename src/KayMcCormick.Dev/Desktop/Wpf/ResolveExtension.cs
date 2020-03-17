using System ;
using System.Collections ;
using System.ComponentModel ;
using System.ComponentModel.Design.Serialization ;
using System.Globalization ;
using System.Reflection ;
using System.Windows ;
using System.Windows.Markup ;
using System.Xaml ;
using Autofac ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    [TypeConverter(typeof(ResolveTypeConverter))]
    [MarkupExtensionReturnType( typeof(object))]
    public class ResolveExtension : MarkupExtension
    {
        private Type _componentType ;

        public ResolveExtension ( ) {
        }

        public ResolveExtension (Type componentType ) { _componentType = componentType ; }

        public Type ComponentType
        {
            get { return _componentType ; }
            set { _componentType = value ; }
        }

        #region Overrides of MarkupExtension
        public override object 
            // ReSharper disable once AnnotationRedundancyInHierarchy
            ProvideValue ( [ NotNull ] IServiceProvider serviceProvider )
        {
            if ( serviceProvider == null )
            {
                throw new ArgumentNullException ( nameof ( serviceProvider ) ) ;
            }

            // ReSharper disable once UnusedVariable
            var p = ( IAmbientProvider ) serviceProvider.GetService (
                                                                     typeof ( IAmbientProvider )
                                                                    ) ;
            ILifetimeScope scope = null ;
            var service = serviceProvider.GetService ( typeof ( IProvideValueTarget ) ) ;
            if ( service != null )
            {
                var pp = ( IProvideValueTarget ) service ;

                var o = pp.TargetObject ;
                if ( o is DependencyObject d )
                {
                    scope = ( ILifetimeScope ) d.GetValue (
                                                           AttachedProperties.LifetimeScopeProperty
                                                          ) ;
                }
            }
            if(scope == null)
            {
                var svc = serviceProvider.GetService ( typeof ( IRootObjectProvider ) ) ;
                if ( svc != null )
                {
                    var rootP = ( IRootObjectProvider ) svc ;
                    if ( rootP.RootObject is DependencyObject d )
                    {
                        scope = ( ILifetimeScope ) d.GetValue (
                                                               AttachedProperties
                                                                  .LifetimeScopeProperty
                                                              ) ;
                    }
                }

            }

            if ( scope != null )
            {
                return scope.Resolve ( _componentType ) ;
            }
            else
            {
                throw new Exception ( "No lifetime scope" ) ;
            }

        }
        #endregion
    }

    public class ResolveTypeConverter : TypeConverter
    {
        #region Overrides of TypeConverter
        public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
        {
            return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType); 
            return base.CanConvertTo ( context , destinationType ) ;
        }

        public override object ConvertTo (
            ITypeDescriptorContext context
          , CultureInfo            culture
          , object                 value
          , Type                   destinationType
        )
        {
            if (!(destinationType == typeof(InstanceDescriptor)))
                return base.ConvertTo(context, culture, value, destinationType);
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if ( ! ( value is ResolveExtension resolveExtension ) )
                throw new ArgumentException (
                                             $"{nameof ( value )} must be of type ResolveExtension"
                                           , nameof ( value )
                                            ) ;
            return (object)new InstanceDescriptor((MemberInfo)typeof(ResolveExtension).GetConstructor(new Type[1]
                                                                                                              {
                                                                                                                  typeof (object)
                                                                                                              }), (ICollection)new object[1]
                                                                                                                               {
                                                                                                                                   resolveExtension.ComponentType
                                                                                                                               });
        }
        #endregion
    }
}