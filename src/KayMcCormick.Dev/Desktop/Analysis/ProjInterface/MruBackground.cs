#if false
using System ;
using System.Globalization ;
using System.Windows.Data ;
using System.Windows.Media ;

namespace ProjInterface
{
    [ValueConversion( typeof(IMruItem), typeof(SolidColorBrush))]
    public class MruBackground : IValueConverter
    {
        /// <summary>Converts a value. </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
        public object Convert (
            object      value
      , Type        targetType
      , object      parameter
      , CultureInfo culture
        )
        {
            IMruItem item = ( IMruItem ) value ;
            if ( item != null && item.Exists )
            {
                return Brushes.Aquamarine ;
            }
            else
            {
                return Brushes.Red ;
            }
        }

        /// <summary>Converts a value. </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { throw new NotImplementedException ( ) ; }
    }
}
#endif