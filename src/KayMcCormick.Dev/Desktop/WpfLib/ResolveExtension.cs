using System ;
using System.ComponentModel ;
using System.ComponentModel.Design.Serialization ;
using System.Globalization ;
using System.Windows ;
using System.Windows.Markup ;
using System.Xaml ;
using Autofac ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf.Properties ;

namespace KayMcCormick.Lib.Wpf
{
    public class NamedParameterExtension : ParameterExtension
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get ; set ; }

        #region Overrides of MarkupExtension
        public override object ProvideValue ( IServiceProvider serviceProvider ) { return new NamedParameter(Name, Value) ; }
        #endregion
    }

    public abstract class ParameterExtension : MarkupExtension
    {
        public string Value { get ; set ; }

        #region Overrides of MarkupExtension
        #endregion
    }

    public class TypedParameterExtension : ParameterExtension
    {
        private Type _type ;
        #region Overrides of MarkupExtension
        public override object ProvideValue ( IServiceProvider serviceProvider ) { return new TypedParameter(Type, Value); }

        public Type Type { get { return _type ; } set { _type = value ; } }
        #endregion
    }

    /// <summary>
    /// </summary>
    [ TypeConverter ( typeof ( ResolveTypeConverter ) ) ]
    [ MarkupExtensionReturnType ( typeof ( object ) ) ]
    public sealed class ResolveExtension : MarkupExtension
    {
        public object Argument { get ; set ; }

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
        public Type ComponentType { get ; set ; }

        #region Overrides of MarkupExtension
        /// <summary>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
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
                try
                {
                    if ( Argument != null )
                    {
                        return scope.Resolve (
                                              ComponentType
                                            , new PositionalParameter ( 1 , Argument )
                                             ) ;
                    }

                    return scope.Resolve ( ComponentType ) ;
                }
                catch ( Exception ex )
                {
                    DebugUtils.WriteLine(ex.ToString());
                    return null ;
                }
            }

            throw new Exception ( "No lifetime scope" ) ;
        }
        #endregion
    }

    /// <summary>
    /// </summary>
    internal sealed class ResolveTypeConverter : TypeConverter
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
                                             string.Format ( Resources.ResolveTypeConverter_ConvertTo__0__must_be_of_type_ResolveExtension , nameof ( value ) )
                                           , nameof ( value )
                                            ) ;
            }

            return new InstanceDescriptor (
                                           typeof ( ResolveExtension ).GetConstructor (
                                                                                       new[]
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