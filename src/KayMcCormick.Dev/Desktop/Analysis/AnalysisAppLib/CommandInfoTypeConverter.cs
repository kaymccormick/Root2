using System;
using System.ComponentModel;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandInfoTypeConverter :TypeConverter
    {
        public CommandInfoTypeConverter()
        {
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return base.CanConvertTo(context, destinationType);
        }
    }
}