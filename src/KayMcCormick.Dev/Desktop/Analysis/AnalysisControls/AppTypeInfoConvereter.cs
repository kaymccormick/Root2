using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using AnalysisAppLib.Syntax;

namespace AnalysisControls
{
    public class AppTypeInfoConvereter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SyntaxFieldInfo info = value as SyntaxFieldInfo;
            if (info != null)
            {
                if (info.ElementTypeMetadataName != null)
                {
                    var n = info.ElementTypeMetadataName.Substring(info.ElementTypeMetadataName.LastIndexOf('.') + 1);
                    info.Model.TryGetAppTypeInfo(n, out var ti);
                    if ((string) parameter == "Verify")
                    {
                        return ti?.ElementName ?? "Unverified";
                    }
                    return ti;
                }
                else
                {
                    info.Model.TryGetAppTypeInfo(info.TypeName, out var ti);
                    if ((string)parameter == "Verify")
                    {
                        return ti?.ElementName ?? "Unverified";
                    }
                    return ti;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}