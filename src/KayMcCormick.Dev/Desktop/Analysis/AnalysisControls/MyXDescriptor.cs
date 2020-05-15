using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Autofac;
using JetBrains.Annotations;
using KayMcCormick.Lib.Wpf;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public class MyXDescriptor<TInput, TReturn> : CustomPropertyDescriptor, IUpdatableProperty, IHasComponentContext where TInput : class
    {
        private readonly PropertyInfo _propertyInfo;

        public override bool IsReadOnly => false;

        protected override AttributeCollection CreateAttributeCollection()
        {
            var attributeCollection = base.CreateAttributeCollection();
            return attributeCollection;
        }

        protected override Attribute[] AttributeArray
        {
            get
            {
                var val = base.AttributeArray;
                return val;
            }
            set
            {
                base.AttributeArray = value;
            }
        }

        public override void SetValue(object component, object value)
        {
            try
            {
                if (component is TInput component1)
                {
                     _propertyInfo.SetValue(component1, value);
                }
            }
            catch (Exception eX)
            {
                // ignored
            }
        }

    public override AttributeCollection Attributes
        {
            get
            {
                var val = base.Attributes;
                return val;
            }
        }

        public override bool IsBrowsable
        {
            get { return _isBrowsable; }
        }

        public void SetIsBrowsable(bool isBrowsable)
        {
            _isBrowsable = isBrowsable;
        }

        private GetDelegate _getValue;
        private SetDelegate _setValue;
        private Type _type;
        private TypeConverter _converter;
        private Type _intype;
        private bool _isBrowsable = true;
        private string _typeConverterTypeName;
        private Type _typeConverterType;
        private List<EditorAttribute> _editorAttributes = new List<EditorAttribute>();
        private IComponentContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attrs"></param>
        public MyXDescriptor([NotNull] string name, Attribute[] attrs) : base(name, attrs)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="descr"></param>
        public MyXDescriptor([NotNull] MemberDescriptor descr) : base(descr)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="descr"></param>
        /// <param name="attrs"></param>
        public MyXDescriptor([NotNull] MemberDescriptor descr, Attribute[] attrs) : base(descr, attrs)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="TReturn"></typeparam>
        /// <typeparam name="TInput"></typeparam>
        public delegate TReturn GetDelegate(TInput instance);
        public delegate void SetDelegate(TInput instance, TReturn val);
        public MyXDescriptor(PropertyInfo propertyInfo) : base(propertyInfo.Name, Array.Empty<Attribute>())
        {
            _propertyInfo = propertyInfo;
            var att = _propertyInfo.GetCustomAttribute<BrowsableAttribute>();
            if (att != null)
            {
                _isBrowsable = att.Browsable;
            }
            var att2 = _propertyInfo.GetCustomAttribute<TypeConverterAttribute>();
            if (att2 != null)
            {
                _typeConverterTypeName = att2.ConverterTypeName;
                _typeConverterType = Type.GetType(_typeConverterTypeName);
            }

            var att3 = _propertyInfo.GetCustomAttributes<EditorAttribute>();
            foreach (var editorAttribute in att3)
            {
                _editorAttributes.Add(editorAttribute);
            }

            //var it = typeof(GetDelegate<,>).MakeGenericType(propertyInfo.PropertyType, propertyInfo.DeclaringType);
            _getValue = (GetDelegate) propertyInfo.GetMethod.CreateDelegate(typeof(GetDelegate));
            //_setValue = (SetDelegate)propertyInfo.SetMethod.CreateDelegate(typeof(SetDelegate));
            _type = typeof(TReturn);
            _intype = typeof(TInput);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public override object GetValue(object component)
        {
            try
            {
                if (component is TInput component1)
                {
                    var value = _propertyInfo.GetValue(component1);
                    return value;
                }
            }
            catch (Exception eX)
            {
                // ignored
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public override TypeConverter Converter
        {
            get
            {
                if (_converter == null)
                {

                    if (_typeConverterType != null)
                    {

                        try
                        {
                            _converter = (TypeConverter) _context.ResolveOptional(_typeConverterType);
                        }
                        catch
                        {

                        }

                        if (_converter == null)
                        {
                            try
                            {
                                _converter = (TypeConverter) Activator.CreateInstance(_typeConverterType);
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                    } else
                    {
                    }
                }


                return _converter;
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override Type PropertyType => _type;

        public IComponentContext Context
        {
            get { return _context; }
            set { _context = value; }
        }
    }

    public interface IHasComponentContext
    {
        IComponentContext Context { get; set; }
    }

    public class PropertyTypeConverter : TypeConverter
    {
        private readonly MyXDescriptor<object, object> _myXDescriptor;

        public PropertyTypeConverter(MyXDescriptor<object, object> myXDescriptor)
        {
            
        }
    }

    public interface IUpdatableProperty 
    {
        string Name { get; }
        void SetIsBrowsable(bool isBrowsable);
    }
}