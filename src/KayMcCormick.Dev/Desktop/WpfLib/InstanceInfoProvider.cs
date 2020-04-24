#region header
// Kay McCormick (mccor)
// 
// Analysis
// KayMcCormick.Lib.Wpf
// InstanceInfoProvider.cs
// 
// 2020-04-08-3:24 PM
// 
// ---
#endregion
using System ;
using System.ComponentModel ;
using System.Globalization ;
using Autofac ;
using Autofac.Core ;
using Autofac.Core.Registration ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;

namespace KayMcCormick.Lib.Wpf
{
    /// <inheritdoc />
    public sealed class MiscInstanceInfoProvider : TypeDescriptionProvider
    {
        private ILifetimeScope _scope ;

        public override bool IsSupportedType ( Type type )
        {

            return base.IsSupportedType ( type ) ;
        }

        #region Overrides of TypeDescriptionProvider
        /// <inheritdoc />
        public override ICustomTypeDescriptor GetTypeDescriptor (
            Type   objectType
          , object instance
        )
        {
            if ( typeof ( ComponentRegistration ).IsAssignableFrom ( objectType ) )
            {
                return new ComponentRegistrationTypeDescriptor(_scope);
            }

            return base.GetTypeDescriptor ( objectType , instance ) ;
        }
        #endregion

        public ILifetimeScope Scope { get { return _scope ; } set { _scope = value ; } }
    }

    /// <inheritdoc />
    public class CustomPropertyDescriptor : PropertyDescriptor
    {
#pragma warning disable 649
        private Type _componentType ;
#pragma warning restore 649
#pragma warning disable 649
        private bool _isReadOnly ;
#pragma warning restore 649
#pragma warning disable 649
        private Type _propertyType ;
#pragma warning restore 649

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attrs"></param>
        public CustomPropertyDescriptor ( [ NotNull ] string name , Attribute[] attrs ) : base ( name , attrs )
        {
        }

        /// <inheritdoc />
        public CustomPropertyDescriptor ( [ NotNull ] MemberDescriptor descr ) : base ( descr )
        {
        }

        /// <inheritdoc />
        public CustomPropertyDescriptor ( [ NotNull ] MemberDescriptor descr , Attribute[] attrs ) : base ( descr , attrs )
        {
        }

        #region Overrides of PropertyDescriptor
        /// <inheritdoc />
        public override bool CanResetValue ( object component ) { return false ; }

        /// <inheritdoc />
        public override object GetValue ( object component ) { return null ; }

        /// <inheritdoc />
        public override void ResetValue ( object component ) { }

        /// <inheritdoc />
        public override void SetValue ( object component , object value ) { }

        /// <inheritdoc />
        public override bool ShouldSerializeValue ( object component ) { return false ; }

        /// <inheritdoc />
        public override Type ComponentType { get { return _componentType ; } }

        /// <inheritdoc />
        public override bool IsReadOnly { get { return _isReadOnly ; } }

        /// <inheritdoc />
        public override Type PropertyType { get { return _propertyType ; } }
        #endregion
    }

    /// <inheritdoc />
    public sealed class ComponentRegistrationTypeDescriptor : CustomTypeDescriptor
    {
        private readonly ILifetimeScope _scope ;
        public ComponentRegistrationTypeDescriptor ( ILifetimeScope scope )
        {
            _scope = scope ;
        }
        #region Overrides of CustomTypeDescriptor
        public override TypeConverter GetConverter ( )
        {
            return new ComponentRegistrationTypeConverter (_scope ) ;
        }
        #endregion
    }

    public class ComponentRegistrationTypeConverter : UiElementTypeConverter
    {
        public ComponentRegistrationTypeConverter ( ILifetimeScope scope ):base(scope) { }
        #region Overrides of TypeConverter
        public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType ) { return base.CanConvertTo ( context , destinationType ) ; }

        public override object ConvertTo (
            ITypeDescriptorContext context
          , CultureInfo            culture
          , object                 value
          , Type                   destinationType
        )
        {

            return base.ConvertTo ( context , culture , value , destinationType ) ;
        }
        #endregion
    }

    /// <summary>
    /// Type description provider for <see cref="KayMcCormick.Dev.InstanceInfo"/>
    /// </summary>
    public sealed class InstanceInfoProvider : TypeDescriptionProvider
    {
        /// <inheritdoc />
        public override bool IsSupportedType ( Type type )
        {
            return typeof ( InstanceInfo ).IsAssignableFrom ( type ) ;
        }

        #region Overrides of TypeDescriptionProvider
        /// <inheritdoc />
        public override ICustomTypeDescriptor GetTypeDescriptor (
            Type   objectType
          , object instance
        )
        {
            if ( objectType == typeof ( InstanceInfo ) )
            {
                return new InstanceInfoTypeDescriptor();
            }
            return base.GetTypeDescriptor ( objectType , instance ) ;
        }
        #endregion
    }
}
