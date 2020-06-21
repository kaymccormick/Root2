using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using Autofac;
using KayMcCormick.Dev;
using NLog;
using NLog.Fluent;
using RibbonLib.Model;

namespace AnalysisControls.TypeDescriptors
{
    /// <summary>
    /// 
    /// </summary>
    public class AnalysisCustomTypeDescriptor : CustomTypeDescriptor
    {
        private readonly UiElementTypeConverter        _uiElementTypeConverter ;
        private readonly List<IPropertiesAdapter> _props;
        private IComponentContext _context;

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
            if (Type == typeof(RibbonModelGroupItemCollection))
            {
                DebugUtils.WriteLine(editorBaseType.ToString());
                //return new GroupItemCollectionEditor();
            }
            return null;
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

            var x = new TestProperty1();
            // DebugUtils.WriteLine("GetProperties for " + Type);
            List<PropertyDescriptor> p = new List<PropertyDescriptor>();
            // p.Add(x);
            foreach (var propertyInfo in Type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var ti = typeof(MyXDescriptor<,>).MakeGenericType(Type, propertyInfo.PropertyType);
                var inst = Activator.CreateInstance(ti, BindingFlags.CreateInstance, null, new object[] {propertyInfo},
                    Thread.CurrentThread.CurrentCulture);
                IHasComponentContext xx = (IHasComponentContext) inst;
                xx.Context = _context;
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

        /// <inheritdoc />
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return this.GetProperties();
        }

        /// <inheritdoc />
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
        /// <param name="propsAdapters"></param>
        /// <param name="context"></param>
        public AnalysisCustomTypeDescriptor (
            //Func < Type , TypeConverter > funcConverter,
            UiElementTypeConverter        uiElementTypeConverter
            , Type                          type, IEnumerable<IPropertiesAdapter> propsAdapters
            , IComponentContext context,
            ILogger logger
        )
        {

            // DebugUtils.WriteLine("Constructor " + GetType().FullName + " " + type.FullName);
            // _funcConverter          = funcConverter ;
            _uiElementTypeConverter = uiElementTypeConverter ;
            Type                    = type ;
            _context = context;
            _props = propsAdapters.Where(p => p.HandleType(type)).ToList();
            new LogBuilder(logger).LoggerName(GetType().FullName).Message($"Constructing instance for {type.FullName}");
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
}