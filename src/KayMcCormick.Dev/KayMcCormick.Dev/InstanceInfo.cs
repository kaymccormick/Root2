﻿#region header
// Kay McCormick (mccor)
// 
// FileFinder3
// WpfApp1
// InstanceInfo.cs
// 
// 2020-01-25-7:05 PM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Globalization ;
using Autofac.Core ;

namespace KayMcCormick.Dev
{
    /// <summary></summary>
    /// <autogeneratedoc />
    [TypeConverter(typeof(InstanceInfoTypeConverter))]
    [DefaultProperty("Instance")]
    public sealed class InstanceInfo
    {
        private IDictionary < string , object  > _metadata ;

        /// <summary>Gets or sets the instance.</summary>
        /// <value>The instance.</value>
        public object Instance { get ; internal set ; }

        /// <summary>Gets or sets the parameters.</summary>
        /// <value>The parameters.</value>
        public IEnumerable < Parameter > Parameters { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary < string , object  > Metadata { get { return _metadata ; } set { _metadata = value ; } }


        /// <inheritdoc />
        public override string ToString ( ) { return $"InstanceInfo({Instance})" ; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class InstanceInfoTypeConverter : TypeConverter
    {
        #region Overrides of TypeConverter
        /// <inheritdoc />
        public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
        {
            if ( destinationType == typeof ( string ) )
            {
                return true ;
            }
            return base.CanConvertTo ( context , destinationType ) ;
        }

        /// <inheritdoc />
        public override object ConvertTo (
            ITypeDescriptorContext context
          , CultureInfo            culture
          , object                 value
          , Type                   destinationType
        )
        {
            if ( destinationType != typeof ( string ) )
            {
                return base.ConvertTo ( context , culture , value , destinationType ) ;
            }

            var v = ( InstanceInfo ) value ;
            return $"{v.Instance.GetType ( ).FullName}[{v.Instance}]" ;
        }
        #endregion
    }
}