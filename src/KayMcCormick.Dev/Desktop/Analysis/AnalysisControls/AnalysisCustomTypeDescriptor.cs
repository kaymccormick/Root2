using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Linq;
using System.Reflection;
using System.Threading;
using Autofac;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf ;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class AnalysisCustomTypeDescriptor : CustomTypeDescriptor
    {

        private readonly Func < Type , TypeConverter > _funcConverter ;
        private readonly UiElementTypeConverter        _uiElementTypeConverter ;
        private IPropertiesAdapter _propsAdapter;
        private List<IPropertiesAdapter> _props;

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
            var pp = base.GetProperties();
            List<PropertyDescriptor> p = new List<PropertyDescriptor>();
            foreach (var propertyInfo in Type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var ti = typeof(MyXDescriptor<,>).MakeGenericType(Type, propertyInfo.PropertyType);
                var inst = Activator.CreateInstance(ti, BindingFlags.CreateInstance, null, new object[] {propertyInfo},
                    Thread.CurrentThread.CurrentCulture);
                PropertyDescriptor xDescriptor = (PropertyDescriptor) inst;
                foreach (var propertiesAdapter in _props)
                {
                    propertiesAdapter.UpdateProperty((IUpdatableProperty)inst);
                }

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

        /// <summary>
        /// 
        /// </summary>
        public Type Type { get ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="funcConverter"></param>
        /// <param name="uiElementTypeConverter"></param>
        /// <param name="type"></param>
        /// <param name="context"></param>
        public AnalysisCustomTypeDescriptor (
            Func < Type , TypeConverter > funcConverter
          , UiElementTypeConverter        uiElementTypeConverter
          , Type                          type, IEnumerable<IPropertiesAdapter> propsAdapters
	  , IComponentContext context
        ) : base()
        {
            DebugUtils.WriteLine("Constructor " + GetType().FullName + " " + type.FullName);
            _funcConverter          = funcConverter ;
            _uiElementTypeConverter = uiElementTypeConverter ;
            Type                    = type ;
            _props = propsAdapters.Where(p => p.HandleType(type)).ToList();
            
        }

        #region Overrides of CustomTypeDescriptor

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override TypeConverter GetConverter()
        {
            DebugUtils.WriteLine("Returning converter for " + Type.FullName);
            return _uiElementTypeConverter;
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPropertiesAdapter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        PropertyDescriptorCollection GetProperties(Type t);

        bool HandleType(Type type);
        void UpdateProperty(IUpdatableProperty propertyDescriptor);
    }
}