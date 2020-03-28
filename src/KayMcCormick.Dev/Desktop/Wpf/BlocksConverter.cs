using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Globalization ;
using System.Linq ;
using System.Windows ;
using System.Windows.Data ;
using System.Windows.Documents ;

namespace KayMcCormick.Lib.Wpf
{
    public class BlocksConverter : IValueConverter

    {
        #region Implementation of IValueConverter
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            var r = new List<Run>();
            if ( value is String s )
            {
                r.Add (  new Run ( s ) ) ;
            }
            else
            {
                if ( value is IEnumerable enumerable )
                {
                    foreach ( var part in enumerable )
                    {
                        r.Add ( new Run ( part.ToString ( ) ) ) ;
                    }
                }

            }

            var span = new Span ( ) ;
            span.Inlines.AddRange(r);
                
            return new Paragraph(span);
            
        }

        public object ConvertBack ( object value , Type targetType , object parameter , CultureInfo culture ) { return null ; }
        #endregion
    }
}