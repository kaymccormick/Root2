using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Reflection;
using System.Text ;
using System.Threading.Tasks ;
using Autofac.Core ;
using JetBrains.Annotations;
using KayMcCormick.Lib.Wpf ;
using Microsoft.CodeAnalysis.Host ;

namespace AnalysisControls
{
    public class TypeProvider : TypeDescriptionProvider
    {
        private IEnumerable < ICustomTypeDescriptor > _supported ;

        public TypeProvider ( IEnumerable < ICustomTypeDescriptor > supported )
        {
            _supported = supported ;
        }

        #region Overrides of TypeDescriptionProvider
        public override ICustomTypeDescriptor GetTypeDescriptor (
            Type   objectType
          , object instance
        )
        {
            return base.GetTypeDescriptor ( objectType , instance ) ;
        }

        public override bool IsSupportedType ( Type type )
        {
            return base.IsSupportedType ( type ) ;
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class AnalysisCustomTypeDescriptor : CustomTypeDescriptor
    {
        private readonly Func < Type , TypeConverter > _funcConverter ;
        private readonly UiElementTypeConverter        _uiElementTypeConverter ;
        public override AttributeCollection GetAttributes()
        {
            return base.GetAttributes();
        }

        public override string GetClassName()
        {
            return base.GetClassName();
        }

        public override string GetComponentName()
        {
            return base.GetComponentName();
        }

        public override EventDescriptor GetDefaultEvent()
        {
            return base.GetDefaultEvent();
        }

        public override PropertyDescriptor GetDefaultProperty()
        {
            return base.GetDefaultProperty();
        }

        public override object GetEditor(Type editorBaseType)
        {
            return base.GetEditor(editorBaseType);
        }

        public override EventDescriptorCollection GetEvents()
        {
            return base.GetEvents();
        }

        public override EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return base.GetEvents(attributes);
        }

        public override PropertyDescriptorCollection GetProperties()
        {
            List<PropertyDescriptor> p = new List<PropertyDescriptor>();
            foreach (var propertyInfo in Type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                PropertyDescriptor xDescriptor = new MyXDescriptor(propertyInfo);
                p.Add(xDescriptor);
            }
            PropertyDescriptorCollection coll = new PropertyDescriptorCollection(p.ToArray());
            return coll;
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return this.GetProperties();
        }

        public override object GetPropertyOwner(PropertyDescriptor pd)
        {
            return base.GetPropertyOwner(pd);
        }

        public Type Type { get ; }

        public AnalysisCustomTypeDescriptor (
            Func < Type , TypeConverter > funcConverter
          , UiElementTypeConverter        uiElementTypeConverter
          , Type                          type
        )
        {
            _funcConverter          = funcConverter ;
            _uiElementTypeConverter = uiElementTypeConverter ;
            Type                    = type ;
        }

        #region Overrides of CustomTypeDescriptor

        public override TypeConverter GetConverter() => _uiElementTypeConverter;
        #endregion
    }

    public class MyXDescriptor : CustomPropertyDescriptor
    {
        private Func<object, object> _getValue;
        private Type _type;
        private readonly TypeConverter _converter = new TestConverter1();

        public MyXDescriptor([NotNull] string name, Attribute[] attrs) : base(name, attrs)
        {
        }

        public MyXDescriptor([NotNull] MemberDescriptor descr) : base(descr)
        {
        }

        public MyXDescriptor([NotNull] MemberDescriptor descr, Attribute[] attrs) : base(descr, attrs)
        {
        }

        public MyXDescriptor(PropertyInfo propertyInfo) : base(propertyInfo.Name, Array.Empty<Attribute>())
        {
            _getValue = propertyInfo.GetValue;
            _type = propertyInfo.PropertyType;
        }

        public override object GetValue(object component) => _getValue(component);

        public override TypeConverter Converter => _converter;

        public override Type PropertyType => _type;
    }

    public class TestConverter1 : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return base.CanConvertTo(context, destinationType);
        }
    }
}