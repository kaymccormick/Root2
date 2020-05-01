using System;
using System.ComponentModel;
using KayMcCormick.Dev;

namespace KayMcCormick.Lib.Wpf.Command
{
    public class LacConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            DebugUtils.WriteLine($"{nameof(LacConverter)}: {destinationType.FullName}");
            return base.CanConvertTo(context, destinationType);
        }
    }
}