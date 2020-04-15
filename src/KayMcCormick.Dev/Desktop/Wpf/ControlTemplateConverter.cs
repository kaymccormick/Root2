using System ;
using System.Globalization ;
using System.Reflection ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Markup ;
using KayMcCormick.Dev ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    public class ControlTemplateConverter : IValueConverter
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
            if ( value == null )
            {
                return null ;
            }

            var x = ( ControlTemplate ) value ;
            var t = x.Template ;
            foreach ( var fieldInfo in t.GetType ( )
                                        .GetFields (
                                                    BindingFlags.Instance | BindingFlags.NonPublic
                                                   ) )
            {
                DebugUtils.WriteLine ( fieldInfo.Name ) ;
            }

            foreach ( var fieldInfo in t.GetType ( )
                                        .GetProperties (
                                                        BindingFlags.Instance
                                                        | BindingFlags.NonPublic
                                                       ) )
            {
                DebugUtils.WriteLine ( fieldInfo.Name ) ;
            }

            try
            {
                var xaml = XamlWriter.Save ( value ) ;
                DebugUtils.WriteLine ( xaml ) ;
                var result = XamlReader.Parse ( xaml ) ;
                DebugUtils.WriteLine ( $"{result.GetType ( ).FullName}" ) ;
                return result ;
            }
            catch ( Exception ex )
            {
                return ex.ToString ( ) ;
            }
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