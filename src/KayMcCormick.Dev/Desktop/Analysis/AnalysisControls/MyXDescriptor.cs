using System;
using System.ComponentModel;
using System.Reflection;
using JetBrains.Annotations;
using KayMcCormick.Lib.Wpf;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public class MyXDescriptor<TInput, TReturn> : CustomPropertyDescriptor where TInput : class
    {
        private readonly PropertyInfo _propertyInfo;
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

        private GetDelegate _getValue;
        private Type _type;
        private readonly TypeConverter _converter = new TestConverter1();
        private Type _intype;
        private bool _isBrowsable = true;

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
        public MyXDescriptor(PropertyInfo propertyInfo) : base(propertyInfo.Name, Array.Empty<Attribute>())
        {
            _propertyInfo = propertyInfo;
            var att = _propertyInfo.GetCustomAttribute<BrowsableAttribute>();
            if (att != null)
            {
                _isBrowsable = att.Browsable;
            }

            //var it = typeof(GetDelegate<,>).MakeGenericType(propertyInfo.PropertyType, propertyInfo.DeclaringType);
            _getValue = (GetDelegate) propertyInfo.GetMethod.CreateDelegate(typeof(GetDelegate));
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
        public override TypeConverter Converter => _converter;

        /// <summary>
        /// 
        /// </summary>
        public override Type PropertyType => _type;
    }
}