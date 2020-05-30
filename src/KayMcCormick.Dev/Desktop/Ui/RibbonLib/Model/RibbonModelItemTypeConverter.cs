using System.Collections;
using System.ComponentModel;

namespace RibbonLib.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonModelItemTypeConverter : TypeConverter
    {
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            return base.CreateInstance(context, propertyValues);
        }

        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
            return base.GetCreateInstanceSupported(context);
        }
    }
}