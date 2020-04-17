using System ;
using System.ComponentModel ;
using System.ComponentModel.Design.Serialization ;
using System.Globalization ;
using System.Windows ;
using System.Windows.Markup ;
using System.Xaml ;
using Autofac ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    [ TypeConverter ( typeof ( ResolveTypeConverter ) ) ]
    [ MarkupExtensionReturnType ( typeof ( object ) ) ]
    public sealed class ResolveExtension : MarkupExtension
    {
        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public ResolveExtension ( ) { }

        /// <summary>
        /// </summary>
        /// <param name="componentType"></param>
        public ResolveExtension ( Type componentType ) { ComponentType = componentType ; }

        /// <summary>
        /// </summary>
        public Type ComponentType { get ; }

        #region Overrides of MarkupExtension
        /// <summary>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        [ NotNull ]
        public override object ProvideValue ( IServiceProvider serviceProvider )
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

            if ( scope == null )
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
                return scope.Resolve ( ComponentType ) ;
            }

            throw new Exception ( "No lifetime scope" ) ;
        }
        #endregion
    }

    /// <summary>
    /// </summary>
    public sealed class ResolveTypeConverter : TypeConverter
    {
        #region Overrides of TypeConverter
        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
        {
            return destinationType == typeof ( InstanceDescriptor )
                   || base.CanConvertTo ( context , destinationType ) ;
            return base.CanConvertTo ( context , destinationType ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public override object ConvertTo (
            ITypeDescriptorContext context
          , CultureInfo            culture
          , object                 value
          , Type                   destinationType
        )
        {
            if ( ! ( destinationType == typeof ( InstanceDescriptor ) ) )
            {
                return base.ConvertTo ( context , culture , value , destinationType ) ;
            }

            if ( value == null )
            {
                throw new ArgumentNullException ( nameof ( value ) ) ;
            }

            if ( ! ( value is ResolveExtension resolveExtension ) )
            {
                throw new ArgumentException (
                                             $"{nameof ( value )} must be of type ResolveExtension"
                                           , nameof ( value )
                                            ) ;
            }

            return new InstanceDescriptor (
                                           typeof ( ResolveExtension ).GetConstructor (
                                                                                       new Type[]
                                                                                       {
                                                                                           typeof (
                                                                                               object
                                                                                           )
                                                                                       }
                                                                                      )
                                         , new object[] { resolveExtension.ComponentType }
                                          ) ;
        }
        #endregion
    }
}