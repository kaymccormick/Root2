using System ;
using System.Collections.Generic;
using System.Globalization ;
using System.Reflection ;
using System.Windows.Data ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    public sealed class MethodInfoConverter : IValueConverter
    {
        #region Implementation of IValueConverter
        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        [ NotNull ]
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            var methodInfo = ( MethodInfo ) value ;
            if ( value == null )
            {
                return "(null)" ;
            }

            if ( ( string ) parameter == "Parameters" )
            {
                return methodInfo.GetParameters ( ) ;
            }

            return "(undefined)" ;
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

    public class ParameterInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ParameterInfo p = (ParameterInfo) value;
            if ((string) parameter == "Tags")
            {
                List<string> tags = new List<string>();
                void a(string tag)
                {
                    tags.Add(tag);
                }
                if (p.IsIn)
                {
                    a("in");
                }
                
                if (p.IsLcid)
                {

                }
                if(p.IsOut)
                {
                    a("out");
                }

                return tags;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}