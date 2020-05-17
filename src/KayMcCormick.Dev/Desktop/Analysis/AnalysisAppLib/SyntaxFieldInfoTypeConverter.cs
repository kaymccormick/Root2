#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisAppLib
// SyntaxFieldInfoTypeConverter.cs
// 
// 2020-04-11-4:43 PM
// 
// ---
#endregion

using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using AnalysisAppLib.Syntax;

namespace AnalysisAppLib
{
    /// <inheritdoc />
    public sealed class SyntaxFieldInfoTypeConverter : TypeConverter
    {
        #region Overrides of TypeConverter
        /// <inheritdoc />
        public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
        {
            if ( destinationType == typeof ( string ) )
            {
                return true ;
            }

            //DebugUtils.WriteLine ( $"Ca convert to {destinationType}" ) ;
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

            if ( ! ( value is SyntaxFieldInfo f ) )
            {
                return base.ConvertTo ( context , culture , value , destinationType ) ;
            }

            var json = JsonSerializer.Serialize ( f ) ;
            return json ;
        }
        #endregion
    }
}