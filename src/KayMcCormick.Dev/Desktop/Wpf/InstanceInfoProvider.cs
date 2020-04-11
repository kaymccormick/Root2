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
using Autofac.Core ;
using Autofac.Core.Registration ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;

namespace KayMcCormick.Lib.Wpf
{
    /// <inheritdoc />
    public sealed class MiscInstanceInfoProvider : TypeDescriptionProvider
    {
        #region Overrides of TypeDescriptionProvider
        /// <inheritdoc />
        public override ICustomTypeDescriptor GetTypeDescriptor (
            Type   objectType
          , object instance
        )
        {
            if ( typeof ( ComponentRegistration ).IsAssignableFrom ( objectType ) ) return new ComponentRegistrationTypeDescriptor();
            
            return base.GetTypeDescriptor ( objectType , instance ) ;
        }
        #endregion
    }

    /// <inheritdoc />
    public class CustomPropertyDescriptor : PropertyDescriptor
    {
        private Type _componentType ;
        private bool _isReadOnly ;
        private Type _propertyType ;

        public CustomPropertyDescriptor ( [ NotNull ] string name , Attribute[] attrs ) : base ( name , attrs )
        {
        }

        public CustomPropertyDescriptor ( [ NotNull ] MemberDescriptor descr ) : base ( descr )
        {
        }

        public CustomPropertyDescriptor ( [ NotNull ] MemberDescriptor descr , Attribute[] attrs ) : base ( descr , attrs )
        {
        }

        #region Overrides of PropertyDescriptor
        public override bool CanResetValue ( object component ) { return false ; }

        public override object GetValue ( object component ) { return null ; }

        public override void ResetValue ( object component ) { }

        public override void SetValue ( object component , object value ) { }

        public override bool ShouldSerializeValue ( object component ) { return false ; }

        public override Type ComponentType { get { return _componentType ; } }

        public override bool IsReadOnly { get { return _isReadOnly ; } }

        public override Type PropertyType { get { return _propertyType ; } }
        #endregion
    }
    public class ComponentRegistrationTypeDescriptor : CustomTypeDescriptor
    {
    }

    /// <summary>
    /// Type description provider for <see cref="InstanceInfo"/>
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