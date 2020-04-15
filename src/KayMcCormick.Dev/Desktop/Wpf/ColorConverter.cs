using System ;
using System.Globalization ;
using System.Windows.Data ;
using System.Windows.Media ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    public class ColorConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            SolidColorBrush brush ;
            if ( value == null )
            {
                return new SolidColorBrush ( Colors.Transparent ) ;
            }

            var color = ( uint ) value ;
            brush = new SolidColorBrush (
                                         Color.FromArgb (
                                                         ( byte ) ( ( color & 0xff000000 )
                                                                    >> 24 )
                                                       , ( byte ) ( ( color & 0xff0000 ) >> 16 )
                                                       , ( byte ) ( ( color & 0xff00 )   >> 8 )
                                                       , ( byte ) ( color & 0xff )
                                                        )
                                        ) ;
            return brush ;

        }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            return null ;
        }
        #endregion
    }
}