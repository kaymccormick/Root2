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