#region header

// Kay McCormick (mccor)
// 
// Analysis
// WpfLib
// UiElementTypeConverter.cs
// 
// 2020-04-23-9:09 AM
// 
// ---

#endregion

using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using NLog;

namespace KayMcCormick.Lib.Wpf
{
    public class DictConverter<T, T1>
    {
        string DumpInstance(IDictionary<T, T1> dict)
        {
            StringBuilder b = new StringBuilder();
            foreach (var keyValuePair in dict)
            {
                var conv1 = TypeDescriptor.GetConverter(typeof(T1));
                var conv = TypeDescriptor.GetConverter(typeof(T));
                if (conv != null && conv.CanConvertTo(typeof(string)))
                {
                    var s = conv.ConvertTo(keyValuePair.Key, typeof(string));
                    b.Append(s);
                }
                else
                {
                    b.Append(keyValuePair.Key.ToString() ?? "<null>");
                }

                b.Append(" = ");
                if (conv1 != null && conv1.CanConvertTo(typeof(string)))
                {

                    b.Append(conv.ConvertTo(keyValuePair.Value, typeof(string)));
                }
                else
                {
                    b.Append(keyValuePair.Value?.ToString() ?? "<null>");
                }
                b.Append(" ; ");
            }

            return b.ToString();
        }
    }
    
    
}