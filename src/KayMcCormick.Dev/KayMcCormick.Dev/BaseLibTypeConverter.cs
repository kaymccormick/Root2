using System;
using System.ComponentModel;

namespace KayMcCormick.Dev
{
    public class BaseLibTypeConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return base.CanConvertTo(context, destinationType);
        }
    }
}