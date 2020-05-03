using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using JetBrains.Annotations;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:AnalysisControls;assembly=AnalysisControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl2/>
    ///
    /// </summary>
    public class CustomControl2 : Control, INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        public Type Type
        {
            get
            {
                var value = (Type)GetValue(TypeProperty);
                return value;
            }
            set
            {
                SetValue(TypeProperty, value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Attributes
        {
            get
            {
                if (Type == null)
                {
                    return Enumerable.Empty<string>();
                }
                List<string> r = new List<string>();
                void a(string s)
                {
                    r.Add(s);
                }
                if (Type.IsNested)
                {
                    a("Nested");
                    //yield return "Nested";
                }

                if (Type.IsGenericType)
                {
                    var genericType = "Generic Type";
                    //yield return genericType;
                    a(genericType);
                }

                if (Type.IsGenericParameter)

                {
                    var genericParameter = "Generic Parameter";
                    a(genericParameter);
                    //yield return genericParameter;
                }

                if (Type.IsClass)

                {
                    var cl = "Class";
                    a(cl);
                    //yield return cl;
                }

                if (Type.IsPrimitive)
                {
                    a("Primitive");

                }

                if (Type.IsAbstract)
                {
                    a("Abstract");
                }

                if (Type.IsArray)
                {
                    a("Array");
                }

                if (Type.IsEnum)
                {
                    a("Enum");

                }

                if (Type.IsInterface)
                {
                    a("Interface");
                }

                if (Type.IsPublic)
                {
                    a("Public");
                }

                if (Type.IsSealed)
                {
                    a("Sealed");

                }

                if (Type.IsGenericTypeDefinition)
                {
                    a("Generic Type Definition");
                }

                if (Type.IsConstructedGenericType)
                {
                    a("Constructed Generic Type");
                }

                if (Type.IsValueType)
                {
                    a("Value Type");

                }
                return r;

            }
            
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(Type), typeof(CustomControl2),
                new PropertyMetadata(null, new PropertyChangedCallback(_OnTypeChanged), CoerceType));

        private static object CoerceType(DependencyObject d, object basevalue)
        {
            return basevalue;
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public static readonly RoutedEvent TypeChangedEvent =
    EventManager.RegisterRoutedEvent(
                                      "TypeChanged"
                                    , RoutingStrategy.Bubble
                                    , typeof(RoutedPropertyChangedEventHandler<
                                          Type>)
                                    , typeof(CustomControl2)
                                     );

        private static void _OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CustomControl2 cc2 = (CustomControl2)d;
            var newValue = (Type)e.NewValue;
            cc2.OnTypeChanged(newValue);

            // var evt = TypeChangedEvent;
            // var ev = new RoutedPropertyChangedEventArgs<Type>((Type) e.OldValue, (Type) e.NewValue, evt);
            // switch (d)
            // {
                // case UIElement uie:
                    // uie.RaiseEvent(ev);
                    // break;
                // case ContentElement ce:
                    // ce.RaiseEvent(ev);
                    // break;
                // default:
                    // break;

            
        }

        private void OnTypeChanged(Type newValue)
        {
            Descriptor = null;
            PropertyDescriptorCollection = null;
            Provider = null;
            var d = Descriptor;
            var c = PropertyDescriptorCollection;
            var p = Provider;

        }

        private Border _border1;
        private TypeDescriptionProvider _provider;
        private ICustomTypeDescriptor _descriptor;
        private PropertyDescriptorCollection _propertyDescriptorCollection;

        static CustomControl2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomControl2), new FrameworkPropertyMetadata(typeof(CustomControl2)));
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Type> Ancestors
        {
            get
            {
                List<Type> a = new List<Type>();
                var b = Type;//Type.IsGenericType? Type.GetGenericTypeDefinition() : Type;
                while (b != null)
                {
                    a.Add(b);
                    b = b.BaseType;
                }

                return ((IEnumerable<Type>)a).Reverse();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public CustomControl2()
        {
            // AddHandler(TypeChangedEvent, new DependencyPropertyChangedEventHandler((sender, args) =>
            // {
                // Provider = null;
                // Descriptor = null;
                // PropertyDescriptorCollection = null;
            // }));
        }

        /// <inheritdoc />
        public override void OnApplyTemplate()
        {
            _border1 = (Border)GetTemplateChild("border1");
            base.OnApplyTemplate();
        }

        /// <summary>
        /// 
        /// </summary>
        public PropertyDescriptorCollection PropertyDescriptorCollection
        {
            get
            {
                if (_propertyDescriptorCollection == null && Descriptor != null)
                {
                    PropertyDescriptorCollection = Descriptor.GetProperties();
                }
                return _propertyDescriptorCollection;
            }
            set
            {
                if (Equals(value, _propertyDescriptorCollection)) return;
                _propertyDescriptorCollection = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summarym
        public TypeDescriptionProvider Provider
        {
            get
            {
                if (_provider == null)
                {
                    DebugUtils.WriteLine("Provider is null");
                    if (Type != null)
                    {
                        DebugUtils.WriteLine("Type is " + Type.FullName);
                        _provider = TypeDescriptor.GetProvider(Type);
                        DebugUtils.WriteLine("PRovider is " + _provider);
                    }
                }
                    DebugUtils.WriteLine("REturning provider " + _provider);
                return _provider;
            }
            set
            {
                if (Equals(value, _provider)) return;
                _provider = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Descriptor));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICustomTypeDescriptor Descriptor
        {
            get
            {
                if (_descriptor == null && Provider != null && Type != null)
                {
                    _descriptor = Provider.GetTypeDescriptor(Type);
                }

                return _descriptor;
            }
            set
            {
                if (Equals(value, _descriptor)) return;
                _descriptor = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PropertyDescriptorCollection));
            }
        }

        /// <inheritdoc />
        protected override Size MeasureOverride(Size constraint)
        {
            //DebugUtils.WriteLine($"{constraint.Width}x{constraint.Height}");
             _border1.Measure(constraint);
             var dwidth = _border1.DesiredSize.Width;
             
             var dheight = _border1.DesiredSize.Height;
             var measureOverride = new Size(dwidth, dheight);
             //DebugUtils.WriteLine(measureOverride.ToString());
             return measureOverride;

        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            return base.ArrangeOverride(arrangeBounds);
        }

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));  
        }
    }
}
