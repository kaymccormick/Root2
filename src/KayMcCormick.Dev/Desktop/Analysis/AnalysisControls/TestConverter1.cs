using System;
using System.ComponentModel;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class TestConverter1 : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return base.CanConvertTo(context, destinationType);
        }
    }
}