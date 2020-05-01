using System;
using System.Globalization;
using System.Windows.Data;

namespace KayMcCormick.Lib.Wpf
{
    public class BasicConverte : IValueConverter
    {
        #region Implementation of IValueConverter
        public object Convert (
            object      value
            , Type        targetType
            , object      parameter
            , CultureInfo culture
        )
        {
            return value?.ToString ( ) ;
        }

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