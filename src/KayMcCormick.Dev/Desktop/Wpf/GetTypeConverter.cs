﻿#region header
// Kay McCormick (mccor)
// 
// FileFinder3
// Common
// GetTypeConverter.cs
// 
// 2020-01-29-3:21 PM
// 
// ---
#endregion
using System ;
using System.Globalization ;
using System.Windows.Data ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary></summary>
    /// <seealso cref="System.Windows.Data.IValueConverter" />
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for GetTypeConverter
    [ ValueConversion ( typeof ( object ) , typeof ( Type ) ) ]
    public sealed class GetTypeConverter : IValueConverter
    {
        /// <summary>Converts a value. </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        ///     A converted value. If the method returns <see langword="null" />, the
        ///     valid null value is used.
        /// </returns>
        [ NotNull ]
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            if ( value == null )
            {
                return typeof ( DBNull ) ;
            }

            var convert = value.GetType ( ) ;
            // LogManager.GetCurrentClassLogger ( )
            // .Debug( "{type}" , convert.GetType ( ).FullName ) ;
            return convert ;

        }

        /// <summary>Converts a value. </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        ///     A converted value. If the method returns <see langword="null" />, the
        ///     valid null value is used.
        /// </returns>
        public object ConvertBack (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            throw new NotImplementedException ( ) ;
        }
    }
}